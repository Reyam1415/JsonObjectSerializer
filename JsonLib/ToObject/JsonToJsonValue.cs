using System;
using System.Globalization;
using System.Text;

namespace JsonLib
{

    public enum JsonToken
    {
        None,
        CurlyOpen,    // {
        CurlyClose,   // }
        SquaredOpen,  // [
        SquaredClose, // ]
        Colon,         // :
        Comma,         // ,
        String,
        Number,
        True,
        False,
        Null
    }

    public class JsonToJsonValue : IJsonToJsonValue
    {
        public void SkipWhitespaces(char[] jsonChars, ref int index)
        {
            char c;
            while (true)
            {
                if (index == jsonChars.Length)
                {
                    break;
                }

                c = jsonChars[index];

                if (c != ' ' && c != '\t' && c != '\n' && c != '\r')
                {
                    break;
                }

                index ++;
            }
        }

        public JsonToken GetNextToken(char[] jsonChars, int index)
        {
            char c = jsonChars[index];

            switch (c)
            {
                case ',':
                    return JsonToken.Comma;
                case ':':
                    return JsonToken.Colon;
                case '{':
                    return JsonToken.CurlyOpen;
                case '[':
                    return JsonToken.SquaredOpen;
                case '}':
                    return JsonToken.CurlyClose;
                case ']':
                    return JsonToken.SquaredClose;
                case '"':
                    return JsonToken.String;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case '-':
                case '+':
                case '.':
                    return JsonToken.Number;
                case 'f':
                    if (jsonChars.Length > index + 4 &&
                        jsonChars[index + 1] == 'a' &&
                        jsonChars[index + 2] == 'l' &&
                        jsonChars[index + 3] == 's' &&
                        jsonChars[index + 4] == 'e')
                    {
                        return JsonToken.False;
                    }
                    break;

                case 't':
                    if (jsonChars.Length > index + 3 &&
                        jsonChars[index + 1] == 'r' &&
                        jsonChars[index + 2] == 'u' &&
                        jsonChars[index + 3] == 'e')
                    {
                        return JsonToken.True;
                    }
                    break;

                case 'n':
                    if (jsonChars.Length > index + 3 &&
                        jsonChars[index + 1] == 'u' &&
                        jsonChars[index + 2] == 'l' &&
                        jsonChars[index + 3] == 'l')
                    {
                        return JsonToken.Null;
                    }
                    break;
            }

            return JsonToken.None;
        }

        public void MoveIndex(JsonToken token, ref int index)
        {
            if (token == JsonToken.True || token == JsonToken.Null)
            {
                index += 4;
            }
            else if (token == JsonToken.False)
            {
                index += 5;
            }
            else if (token == JsonToken.None) { }
            else
            {
                index += 1;
            }
        }

        public JsonToken MoveAndGetNextToken(char[] jsonChars, ref int index)
        {
            this.SkipWhitespaces(jsonChars, ref index);
            var token = this.GetNextToken(jsonChars, index);
            this.MoveIndex(token, ref index);
            return token;
        }

        public JsonToken LookAheadNextToken(char[] jsonChars, int index)
        {
            int fakeIndex = index;
            this.SkipWhitespaces(jsonChars, ref fakeIndex);
            return this.GetNextToken(jsonChars, fakeIndex);
        }

        public string ParseString(char[] jsonChars, ref int index)
        {
            //  "   find end  " ... add chars to string builder ... escape \", \t, \n , etc. => \\\", \\\t, \\\n
            // return string formatted
            var result = new StringBuilder();
            char c;

            // move to first "
            var stringToken = this.MoveAndGetNextToken(jsonChars, ref index);
            if (stringToken != JsonToken.String)
            {
                throw new Exception("Invalid Json. Expected double quotes");
            }

            bool stringClosed = false;
            while (!stringClosed)
            {
                if (index == jsonChars.Length)
                {
                    break;
                }

                c = jsonChars[index];

                index++;

                if (c == '"')
                {
                    stringClosed = true;
                    break;
                }
                else if (c == '\\')
                {
                    if (index == jsonChars.Length)
                    {
                        break;
                    }
                    // next char \\\ => "
                    c = jsonChars[index++];
                    if (c == '"')
                    {
                        result.Append('"');
                    }
                    else if (c == '\\')
                    {
                        result.Append('\\');
                    }
                    else if (c == '/')
                    {
                        result.Append('/');
                    }
                    else if (c == 'b')
                    {
                        result.Append('\b');
                    }
                    else if (c == 'f')
                    {
                        result.Append('\f');
                    }
                    else if (c == 'n')
                    {
                        result.Append('\n');
                    }
                    else if (c == 'r')
                    {
                        result.Append('\r');
                    }
                    else if (c == 't')
                    {
                        result.Append('\t');
                    }
                    else if (c == 'u')
                    {
                        int remainingLength = jsonChars.Length - index;
                        if (remainingLength >= 4)
                        {
                            if (!UInt32.TryParse(new string(jsonChars, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint codePoint))
                            {
                                // exception ?
                                return "";
                            }
                            result.Append(Char.ConvertFromUtf32((int)codePoint));
                            index += 4;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    result.Append(c);
                }
            }

            if (!stringClosed)
            {
                throw new Exception("Invalid Json.String not closed");
            }

            return result.ToString();
        }

        public int GetLastCharIndexOfNumber(char[] jsonChars, int index)
        {
            int lastIndex;
            for (lastIndex = index; lastIndex < jsonChars.Length; lastIndex++)
            {
                if ("0123456789+-.eE".IndexOf(jsonChars[lastIndex]) == -1)
                {
                    break;
                }
            }
            return lastIndex - 1;
        }

        public object ParseNumber(char[] jsonChars, ref int index)
        {
            this.SkipWhitespaces(jsonChars, ref index);

            int lastIndex = this.GetLastCharIndexOfNumber(jsonChars, index);
            int charLength = (lastIndex - index) + 1;

            var valueString = new string(jsonChars, index, charLength);

            if (int.TryParse(valueString, out int intResult))
            {
                index = lastIndex + 1;
                return intResult;
            }
            else if (double.TryParse(valueString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double doubleResult))
            {
                index = lastIndex + 1;
                return doubleResult;
            }

            throw new JsonLibException("Cannot resolve number");
        }

        public IJsonElementValue ToJsonValue(char[] jsonChars, ref int index)
        {
            var token = this.LookAheadNextToken(jsonChars, index);
            switch (token)
            {
                case JsonToken.String:
                    return new JsonElementString(this.ParseString(jsonChars, ref index));
                case JsonToken.Number:
                    return new JsonElementNumber(this.ParseNumber(jsonChars, ref index));
                case JsonToken.CurlyOpen:
                    return this.ToJsonObject(jsonChars, ref index);
                case JsonToken.SquaredOpen:
                    return this.ToJsonArray(jsonChars, ref index);
                case JsonToken.True:
                    index += 4;
                    return new JsonElementBool(true);
                case JsonToken.False:
                    index += 5;
                    return new JsonElementBool(false);
                case JsonToken.Null:
                    index += 4;
                    return new JsonElementNullable(null);
                case JsonToken.None:
                    break;
            }

            return null;
        }

        public JsonElementObject ToJsonObject(char[] jsonChars, ref int index)
        {
            var result = new JsonElementObject();

            var curlyOpenToken = this.MoveAndGetNextToken(jsonChars, ref index);
            if (curlyOpenToken != JsonToken.CurlyOpen)
            {
                throw new Exception("Invalid Json. Curly open expected at index " + index);
            }

            bool curlyClosed = false;
            while (!curlyClosed)
            {
                var token = this.LookAheadNextToken(jsonChars, index);
                if (token == JsonToken.None)
                {
                    throw new JsonLibException("Invalid Json");
                }
                else if (token == JsonToken.Comma)
                {
                    // move index to ,
                    this.MoveAndGetNextToken(jsonChars, ref index);
                }
                else if (token == JsonToken.CurlyClose)
                {
                    // move index to }
                    this.MoveAndGetNextToken(jsonChars, ref index);

                    // } return result
                    curlyClosed = true;
                }
                else
                {
                    // key
                    string key = this.ParseString(jsonChars, ref index);
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        throw new JsonLibException("Invalid Json. Expected a key at index " + index);
                    }

                    // :
                    token = this.MoveAndGetNextToken(jsonChars, ref index);
                    if (token != JsonToken.Colon)
                    {
                        throw new JsonLibException("Invalid Json. Colon expected at  index " + index);
                    }

                    // value
                    var value = this.ToJsonValue(jsonChars, ref index);
                    if (value == null)
                    {
                        throw new JsonLibException("Invalid Json. No value for key " + key);
                    }

                    result.Add(key, value);
                }
            }

            return result;
        }

        public JsonElementArray ToJsonArray(char[] jsonChars, ref int index)
        {
            var result = new JsonElementArray();

            var squareOpenToken = this.MoveAndGetNextToken(jsonChars, ref index);
            if (squareOpenToken != JsonToken.SquaredOpen)
            {
                throw new Exception("Invalid Json. Squared open expected at index " + index);
            }

            bool squareClosed = false;
            while (!squareClosed)
            {
                var token = this.LookAheadNextToken(jsonChars, index);
                if (token == JsonToken.None)
                {
                    throw new JsonLibException("Invalid Json");
                }
                else if (token == JsonToken.Comma)
                {
                    // move index to ,
                    this.MoveAndGetNextToken(jsonChars, ref index);
                }
                else if (token == JsonToken.SquaredClose)
                {
                    // move index to ] 
                    this.MoveAndGetNextToken(jsonChars, ref index);

                    // ] return result
                    squareClosed = true;
                }
                else
                {
                    var jsonValue = this.ToJsonValue(jsonChars, ref index);
                    if (jsonValue == null)
                    {
                        throw new JsonLibException("Invalid Json");
                    }
                    result.Add(jsonValue);
                }
            }
            return result;
        }

        public IJsonElementValue ToJsonValue(string json)
        {
            char[] jsonChars = json.ToCharArray();
            int index = 0;
            var result = this.ToJsonValue(jsonChars, ref index);
            if (index + 1 < json.Length)
            {
                throw new JsonLibException("Invalid Json");
            }
            return result;
        }
    }
   
}
