using System;
using System.Text.RegularExpressions;

namespace Json
{
    public class JsonParser : IJsonParser
    {
        private const char ObjectOpenDelimiter = '{';
        private const char ObjectCloseDelimiter = '}';
        private const char ArrayOpenDelimiter = '[';
        private const char ArrayCloseDelimiter = ']';
        private const char StringDelimiter = '"';
        private const char KeyValueDelimiter = ':';
        private const char VirguleDelimiter = ',';
        private const char StringDelimiterCanceler = '\\';
        private const string NumberPattern = "^([0-9]+(?:.[0-9])?)$";
        private const string BoolPattern = "(true|false)"; // i

        private bool IsObjectOpenDelimiter(char c)
        {
            return c == ObjectOpenDelimiter;
        }
        private bool IsObjectCloseDelimiter(char c)
        {
            return c == ObjectCloseDelimiter;
        }
        private bool IsArrayOpenDelimiter(char c)
        {
            return c == ArrayOpenDelimiter;
        }
        private bool IsArrayCloseDelimiter(char c)
        {
            return c == ArrayCloseDelimiter;
        }
        private bool IsStringDelimiter(char c)
        {
            return c == StringDelimiter;
        }
        private bool IsKeyValueDelimiter(char c)
        {
            return c == KeyValueDelimiter;
        }
        private bool IsVirguleDelimiter(char c)
        {
            return c == VirguleDelimiter;
        }
        private bool IsStringDelimiterCanceler(char c)
        {
            return c == StringDelimiterCanceler;
        }
        private bool IsNumberValue(string value)
        {
            return new Regex(NumberPattern).IsMatch(value);
        }
        private bool IsBoolValue(string value)
        {
            return new Regex(BoolPattern).IsMatch(value);
        }

        private JsonElement CreateElement(string value)
        {
            if (IsNumberValue(value))
            {
                value = value.Replace(".", ",");
                return JsonElement.CreateNumber(Convert.ToDouble(value));
            }
            else if (IsBoolValue(value))
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

            string currentValue = "";
            bool isStringOpen = false;

            for (int i = index; i < json.Length; i++)
            {
                var currentChar = json[i];
                if (IsStringDelimiter(currentChar)) // "
                {
                    // "
                    if (!isStringOpen)
                    {
                        isStringOpen = true;
                    }
                    // "value"
                    else
                    {
                        isStringOpen = false;
                    }
                }
                else if (IsVirguleDelimiter(currentChar)) // ,
                {
                    if (!isStringOpen)
                    {
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            result.Add(CreateElement(currentValue));
                        }
                        currentValue = "";
                    }
                    else
                    {
                        currentValue += currentChar;
                    }
                }
                else if (IsObjectOpenDelimiter(currentChar)) // {
                {
                    if (!isStringOpen)
                    {
                        var innerJsonObject = ParseObject(json, 1, i + 1);
                        if (innerJsonObject != null)
                        {
                            result.Add(innerJsonObject);
                            currentValue = "";
                            // move i
                            i = ((JsonElementObject)innerJsonObject).EndIndex;
                        }
                    }
                    else
                    {
                        currentValue += currentChar;
                    }
                }
                else if (IsArrayOpenDelimiter(currentChar)) // [
                {
                    if (!isStringOpen)
                    {
                        arrayLevel++;
                        var innerJsonArray = ParseArray(json, arrayLevel, i + 1);
                        if (innerJsonArray != null)
                        {
                            result.Add(innerJsonArray);
                            currentValue = "";
                            // move i
                            i = ((JsonElementArray)innerJsonArray).EndIndex;
                        }
                    }
                    else
                    {
                        currentValue += currentChar;
                    }
                }
                else if (IsArrayCloseDelimiter(currentChar)) // ]
                {
                    if (!isStringOpen)
                    {
                        // assign last value
                        if (!string.IsNullOrEmpty(currentValue))
                        {
                            result.Add(CreateElement(currentValue));
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
                        currentValue += currentChar;
                    }
                }
                else
                {
                    currentValue += currentChar;
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
            string currentKey = "";
            string currentValue = "";

            for (int i = index; i < json.Length; i++)
            {
                var currentChar = json[i];
                if (IsStringDelimiter(currentChar)) // "
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
                else if (IsKeyValueDelimiter(currentChar)) // :
                {
                    if (!isStringOpen)
                    {
                        // next will be value
                        isValue = true;
                    }
                    else
                    {
                        currentValue += currentChar;
                    }
                }
                else if (IsArrayOpenDelimiter(currentChar)) // [
                {
                    if (!isStringOpen)
                    {
                        var innerJsonArray = ParseArray(json, 1, i + 1);
                        if (innerJsonArray != null)
                        {
                            result[currentKey] = innerJsonArray;
                            currentKey = "";
                            currentValue = "";
                            isValue = false;

                            // move i
                            i = ((JsonElementArray)innerJsonArray).EndIndex;
                        }
                    }
                    else
                    {
                        currentValue += currentChar;
                    }
                }
                else if (IsObjectOpenDelimiter(currentChar)) // {
                {
                    if (!isStringOpen)
                    {
                        objectLevel++;
                        var innerJsonObject = ParseObject(json, objectLevel, i + 1);
                        if (innerJsonObject != null)
                        {
                            result[currentKey] = innerJsonObject;
                            currentKey = "";
                            currentValue = "";
                            isValue = false;

                            // move i
                            i = ((JsonElementObject)innerJsonObject).EndIndex;
                        }
                    }
                    else
                    {
                        currentValue += currentChar;
                    }
                }
                // delimiter of json elements if not in a string
                else if (IsVirguleDelimiter(currentChar)) // ,
                {
                    if (!isStringOpen)
                    {
                        if (!string.IsNullOrEmpty(currentKey) && !string.IsNullOrEmpty(currentValue))
                        {
                            result[currentKey] = CreateElement(currentValue);
                        }

                        currentKey = "";
                        currentValue = "";
                        isValue = false;
                    }
                    else
                    {
                        currentValue += currentChar;
                    }
                }
                else if (IsObjectCloseDelimiter(currentChar)) // }
                {
                    if (!isStringOpen)
                    {
                        // assign last values
                        if (!string.IsNullOrEmpty(currentKey) && !string.IsNullOrEmpty(currentValue))
                        {
                            result[currentKey] = CreateElement(currentValue);
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
                        currentValue += currentChar;
                    }
                }
                else if (IsArrayCloseDelimiter(currentChar)) // ]
                {
                    if (!isStringOpen)
                    {
                        // assign last values
                        if (!string.IsNullOrEmpty(currentKey) && !string.IsNullOrEmpty(currentValue))
                        {
                            result[currentKey] = CreateElement(currentValue);
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
                        currentValue += currentChar;
                    }
                }
                else
                {
                    if (isKey)
                    {
                        currentKey += currentChar;
                    }
                    else if (isValue)
                    {
                        currentValue += currentChar;
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
