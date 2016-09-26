using System;
using System.Text;

namespace Json
{

    public class JsonParser : IJsonParser
    {
        private bool IsNumberValue(string value)
        {
            var tested = value.Replace(".", ",");
            double result;
            return double.TryParse(tested, out result);
        }
        private bool IsBoolValue(string value)
        {
            bool result;
            return bool.TryParse(value, out result);
        }

        private JsonElement CreateElement(string value, bool isStringElement)
        {
            value = value.Trim();
            if (!isStringElement && IsNumberValue(value))
            {
                value = value.Replace(".", ",");
                return JsonElement.CreateNumber(Convert.ToDouble(value));
            }
            else if (!isStringElement && IsBoolValue(value))
            {
                return JsonElement.CreateBoolean(Convert.ToBoolean(value));
            }
            else
            {
                return JsonElement.CreateString(value);
            }
        }

        // [1,2,3,"a","b",true,{"myitnt":10},[1,2,{"arrint":100}]]
        public IJsonElement ParseArray(string json, int arrayLevel = 0, int index = 1)
        {
            var result = new JsonElementArray();
            result.StartIndex = index - 1;
            bool isStringElement = false;

            var valueWriter = new StringBuilder();
            bool isStringOpen = false;

            for (int i = index; i < json.Length; i++)
            {
                var currentChar = json[i];
                if (currentChar == '\n' || currentChar == '\r' || currentChar == '\t')
                { }
                else if (currentChar == ' ')
                {
                    if (isStringOpen)
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else if (currentChar == '"') // "
                {
                    // "
                    // "key" or "value"
                    isStringOpen = !isStringOpen;
                    if (isStringOpen)
                    {
                        isStringElement = true;
                    }

                }
                else if (currentChar == ',') // ,
                {
                    if (!isStringOpen)
                    {
                        var value = valueWriter.ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            result.Add(CreateElement(value, isStringElement));
                        }
                        valueWriter.Clear();
                        isStringElement = false;
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else if (currentChar == '{') // {
                {
                    if (!isStringOpen)
                    {
                        var innerJsonObject = ParseObject(json, 1, i + 1);
                        if (innerJsonObject != null)
                        {
                            result.Add(innerJsonObject);
                            valueWriter.Clear();
                            // move i
                            i = ((JsonElementObject)innerJsonObject).EndIndex;
                        }
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else if (currentChar == '[') // [
                {
                    if (!isStringOpen)
                    {
                        arrayLevel++;
                        var innerJsonArray = ParseArray(json, arrayLevel, i + 1);
                        if (innerJsonArray != null)
                        {
                            result.Add(innerJsonArray);
                            valueWriter.Clear();
                            // move i
                            i = ((JsonElementArray)innerJsonArray).EndIndex;
                        }
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else if (currentChar == ']') // ]
                {
                    if (!isStringOpen)
                    {
                        // assign last value
                        var value = valueWriter.ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            result.Add(CreateElement(value, isStringElement));
                            result.EndIndex = i;
                        }

                        // 
                        if (arrayLevel > 0)
                        {
                            result.EndIndex = i;
                            return result;
                        }
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else
                {
                    valueWriter.Append(currentChar);
                }
            }
            result.EndIndex = json.Length;
            return result;
        }


        //{
        //"id":10,
        //"my key":"my value",
        //"mybool":true,
        //"myobj":{"subid":100,"subname":"sub value"},
        //"myarr":[1,"a",true,{"id":5},[2,"b",false,{"susubid":20}]]
        //}
        // Mapping
        //{"myint":10} // json
        // public int MyInt {get;set} // property
        public IJsonElement ParseObject(string json, int objectLevel = 0, int index = 1)
        {
            var result = new JsonElementObject();
            result.StartIndex = index - 1;

            bool isKey = false;
            bool isStringOpen = false;
            bool isValue = false;
            bool isStringElement = false;

            var keyWriter = new StringBuilder();
            var valueWriter = new StringBuilder();

            for (int i = index; i < json.Length; i++)
            {
                var currentChar = json[i];
                if (currentChar == '\n' || currentChar == '\r' || currentChar == '\t')
                { }
                else if (currentChar == ' ')
                {
                    if (isValue && isStringOpen)
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else if (currentChar == '"') // "
                {
                    // "
                    // "key":"
                    if (!isStringOpen) // string not opened
                    {
                        isStringOpen = true;
                        if (!isValue) isKey = true;
                        if (isValue) isStringElement = true;
                    }
                    // "key"
                    // "key":"value"
                    else
                    {
                        isStringOpen = false;
                        isKey = false;
                        if (isStringElement) isValue = false;
                    }
                }
                else if (currentChar == ':') // :
                {
                    if (!isStringOpen)
                    {
                        // next will be value
                        isValue = true;
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else if (currentChar == '[') // [
                {
                    if (!isStringOpen)
                    {
                        var innerJsonArray = ParseArray(json, 1, i + 1);
                        if (innerJsonArray != null)
                        {
                            result[keyWriter.ToString()] = innerJsonArray;
                            keyWriter.Clear();
                            valueWriter.Clear();
                            isValue = false;
                            isStringElement = false;

                            // move i
                            i = ((JsonElementArray)innerJsonArray).EndIndex;
                        }
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                // delimiter of json elements if not in a string
                else if (currentChar == ',') // ,
                {
                    if (!isStringOpen)
                    {
                        var key = keyWriter.ToString();
                        var value = valueWriter.ToString();
                        if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
                        {
                            result[key] = CreateElement(value, isStringElement);
                        }

                        keyWriter.Clear();
                        valueWriter.Clear();
                        isValue = false;
                        isStringElement = false;
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else if (currentChar == '{') // {
                {
                    if (!isStringOpen)
                    {
                        objectLevel++;
                        var innerJsonObject = ParseObject(json, objectLevel, i + 1);
                        if (innerJsonObject != null)
                        {
                            result[keyWriter.ToString()] = innerJsonObject;
                            keyWriter.Clear();
                            valueWriter.Clear();
                            isValue = false;
                            isStringElement = false;

                            // move i
                            i = ((JsonElementObject)innerJsonObject).EndIndex;
                        }
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else if (currentChar == '}') // }
                {
                    if (!isStringOpen)
                    {
                        // assign last values
                        var key = keyWriter.ToString();
                        var value = valueWriter.ToString();
                        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                        {
                            result[key] = CreateElement(value, isStringElement);
                            result.EndIndex = i;
                        }

                        // try add object
                        if (objectLevel > 0)
                        {
                            result.EndIndex = i;
                            return result;
                        }
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else if (currentChar == ']') // ]
                {
                    if (!isStringOpen)
                    {
                        // assign last values
                        var key = keyWriter.ToString();
                        var value = valueWriter.ToString();
                        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                        {
                            result[key] = CreateElement(value, isStringElement);
                            result.EndIndex = i;
                        }

                        // try add object
                        if (objectLevel > 0)
                        {
                            result.EndIndex = i;
                            return result;
                        }
                    }
                    else
                    {
                        valueWriter.Append(currentChar);
                    }
                }
                else
                {
                    if (currentChar != '\r' && currentChar != '\n' && currentChar != '\t')
                    {
                        if (isKey) keyWriter.Append(currentChar);
                        else if (isValue) valueWriter.Append(currentChar);
                    }
                }
            }
            result.EndIndex = json.Length;
            return result;
        }

        public IJsonElement Parse(string json)
        {
            var firstCharacter = json[0];
            if (firstCharacter == '{')
            {
                // object
                return ParseObject(json);
            }
            else if (firstCharacter == '[')
            {
                // array
                return ParseArray(json);
            }
            return null;
        }
    }

}
