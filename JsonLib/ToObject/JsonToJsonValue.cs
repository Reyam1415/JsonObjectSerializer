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
        public IJsonElementValue ToJsonValue(string json)
        {
            char[] jsonChars = json.ToCharArray();
            int index = 0;
            var result = this.ToJsonValue(jsonChars, ref index);
            if (index + 1 < json.Length)
            {
                this.HandleException(index);
            }
            return result;
        }

        public void HandleException(int index, string message = null)
        {
            var exceptionMessage = message != null ? "Invalid Json at position " + index + "." + message : "Invalid Json at position " + index;
            throw new Exception(exceptionMessage);
        }

        protected JsonElementObject ToJsonObject(char[] json, ref int index)
        {
            // {"key":VALUE,...}
            // separator , or }
            // : => key value separator
            // return result }

            var result = new JsonElementObject();

            // {
            this.NextToken(json, ref index);

            while (true)
            {
                var token = this.LookAhead(json, index);
                if (token == JsonToken.None)
                {
                    this.HandleException(index);
                }
                else if (token == JsonToken.Comma)
                {
                    this.NextToken(json, ref index);
                }
                else if (token == JsonToken.CurlyClose)
                {
                    this.NextToken(json, ref index);
                    break;
                }
                else
                {
                    // key
                    string key = this.ParseString(json, ref index);
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        this.HandleException(index, "No object key provided");
                    }

                    // :
                    token = this.NextToken(json, ref index);
                    if (token != JsonToken.Colon)
                    {
                        this.HandleException(index, "No colon (key value serparator) found for " + key);
                    }

                    // value
                    var value = this.ToJsonValue(json, ref index);
                    if (value == null)
                    {
                        this.HandleException(index, "No value for " + key);
                    }

                    result.Add(key, value);
                }
            }

            return result;
        }

        protected JsonElementArray ToJsonArray(char[] json, ref int index)
        {
            // [...]
            // separator , or ]
            // return result ]

            var result = new JsonElementArray();

            // [
            NextToken(json, ref index);

            while (true)
            {
                var token = this.LookAhead(json, index);
                if (token == JsonToken.None)
                {
                    this.HandleException(index);
                }
                else if (token == JsonToken.Comma)
                {
                    this.NextToken(json, ref index);
                }
                else if (token == JsonToken.SquaredClose)
                {
                    this.NextToken(json, ref index);
                    break;
                }
                else
                {
                    var value = this.ToJsonValue(json, ref index);
                    if (value == null)
                    {
                        this.HandleException(index);
                    }

                    result.Add(value);
                }
            }

            return result;
        }

        protected IJsonElementValue ToJsonValue(char[] json, ref int index)
        {
            switch (this.LookAhead(json, index))
            {
                case JsonToken.String:
                    return new JsonElementString(this.ParseString(json, ref index));
                case JsonToken.Number:
                    return new JsonElementNumber(this.ParseNumber(json, ref index));
                case JsonToken.CurlyOpen:
                    return this.ToJsonObject(json, ref index);
                case JsonToken.SquaredOpen:
                    return this.ToJsonArray(json, ref index);
                case JsonToken.True:
                    this.NextToken(json, ref index);
                    return new JsonElementBool(true);
                case JsonToken.False:
                    this.NextToken(json, ref index);
                    return new JsonElementBool(false);
                case JsonToken.Null:
                    this.NextToken(json, ref index);
                    return new JsonElementNullable(null);
                case JsonToken.None:
                    break;
            }

            return null;
        }

        protected string ParseString(char[] json, ref int index)
        {
            // " => "
            // escape \\\"
            var s = new StringBuilder();

            this.EatWhitespace(json, ref index);

            // "
            var c = json[index++];

            bool stringClosed = false;
            while (!stringClosed)
            {

                if (index == json.Length)
                {
                    break;
                }

                c = json[index++];
                if (c == '"')
                {
                    stringClosed = true;
                    break;
                }
                else if (c == '\\')
                {
                    if (index == json.Length)
                    {
                        break;
                    }
                    // next char \\\ => "
                    c = json[index++];
                    if (c == '"')
                    {
                        s.Append('"');
                    }
                    else if (c == '\\')
                    {
                        s.Append('\\');
                    }
                    else if (c == '/')
                    {
                        s.Append('/');
                    }
                    else if (c == 'b')
                    {
                        s.Append('\b');
                    }
                    else if (c == 'f')
                    {
                        s.Append('\f');
                    }
                    else if (c == 'n')
                    {
                        s.Append('\n');
                    }
                    else if (c == 'r')
                    {
                        s.Append('\r');
                    }
                    else if (c == 't')
                    {
                        s.Append('\t');
                    }
                    else if (c == 'u')
                    {
                        int remainingLength = json.Length - index;
                        if (remainingLength >= 4)
                        {
                            if (!UInt32.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint codePoint))
                            {
                                return "";
                            }
                            s.Append(Char.ConvertFromUtf32((int)codePoint));
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
                    s.Append(c);
                }
            }

            if (!stringClosed)
            {
                this.HandleException(index);
            }

            return s.ToString();
        }

        protected object ParseNumber(char[] json, ref int index)
        {
            this.EatWhitespace(json, ref index);

            int lastIndex = this.GetLastCharIndexOfNumber(json, index);
            int charLength = (lastIndex - index) + 1;

            var valueString = new string(json, index, charLength);

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

            throw new Exception("Cannot resolve number");
        }

        protected int GetLastCharIndexOfNumber(char[] json, int index)
        {
            int lastIndex;

            for (lastIndex = index; lastIndex < json.Length; lastIndex++)
            {
                if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1)
                {
                    break;
                }
            }
            return lastIndex - 1;
        }

        protected void EatWhitespace(char[] json, ref int index)
        {
            for (; index < json.Length; index++)
            {
                if (" \t\n\r".IndexOf(json[index]) == -1)
                {
                    break;
                }
            }
        }

        protected JsonToken LookAhead(char[] json, int index)
        {
            int saveIndex = index;
            return NextToken(json, ref saveIndex);
        }

        protected JsonToken NextToken(char[] json, ref int index)
        {
            this.EatWhitespace(json, ref index);

            if (index == json.Length)
            {
                return JsonToken.None;
            }

            char c = json[index];
            index++;
            switch (c)
            {
                case '{':
                    return JsonToken.CurlyOpen;
                case '}':
                    return JsonToken.CurlyClose;
                case '[':
                    return JsonToken.SquaredOpen;
                case ']':
                    return JsonToken.SquaredClose;
                case ',':
                    return JsonToken.Comma;
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
                    return JsonToken.Number;
                case ':':
                    return JsonToken.Colon;
            }
            index--;

            int remainingLength = json.Length - index;

            // false
            if (remainingLength >= 5)
            {
                if (json[index] == 'f' &&
                    json[index + 1] == 'a' &&
                    json[index + 2] == 'l' &&
                    json[index + 3] == 's' &&
                    json[index + 4] == 'e')
                {
                    index += 5;
                    return JsonToken.False;
                }
            }

            // true
            if (remainingLength >= 4)
            {
                if (json[index] == 't' &&
                    json[index + 1] == 'r' &&
                    json[index + 2] == 'u' &&
                    json[index + 3] == 'e')
                {
                    index += 4;
                    return JsonToken.True;
                }
            }

            // null
            if (remainingLength >= 4)
            {
                if (json[index] == 'n' &&
                    json[index + 1] == 'u' &&
                    json[index + 2] == 'l' &&
                    json[index + 3] == 'l')
                {
                    index += 4;
                    return JsonToken.Null;
                }
            }

            return JsonToken.None;
        }

    }

}
