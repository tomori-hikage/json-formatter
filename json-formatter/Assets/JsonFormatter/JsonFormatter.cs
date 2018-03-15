using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace HC.Common
{
    /// <summary>
    /// JSONのフォーマットクラス
    /// </summary>
    public static class JsonFormatter
    {
        #region 列挙型

        public enum IndentType
        {
            Space,
            Tab
        }

        public enum AppendType
        {
            None,
            Append,
            AppendLine,
            AppendSpace
        }

        #endregion


        #region メソッド

        /// <summary>
        /// JSONを見易く整形する
        /// </summary>
        /// <param name="json">json.</param>
        /// <param name="indentType">indentType.</param>
        /// <param name="indentSize">indentSize.</param>
        /// <returns>見易く整形したJSON</returns>
        public static string ToPrettyPrint(string json, IndentType indentType, int indentSize = 4)
        {
            json = ToMinifyPrint(json);


            var stringBuilder = new StringBuilder();

            AppendType appendType = AppendType.None;
            int i = 0;
            int quoteCount = 0;
            int indent = 0;
            int position = 0;
            int lastIndex = 0;

            while (i < json.Length)
            {
                if (i > 0 && json[i - 1] != '\\' && json[i] == '"') ++quoteCount;

                if (quoteCount % 2 == 0)
                {
                    switch (json[i])
                    {
                        case '{':
                            ++indent;
                            position = 1;
                            appendType = AppendType.AppendLine;
                            break;

                        case '[':
                            position = 1;
                            if (json[i + 1] == ']')
                            {
                                appendType = AppendType.Append;
                            }
                            else
                            {
                                ++indent;
                                appendType = AppendType.AppendLine;
                            }
                            break;

                        case '}':
                        case ']':
                            --indent;
                            position = 0;
                            appendType = AppendType.AppendLine;
                            break;

                        case ',':
                            position = 1;
                            appendType = AppendType.AppendLine;
                            break;

                        case ':':
                            position = 1;
                            appendType = AppendType.AppendSpace;
                            break;
                    }

                    switch (appendType)
                    {
                        case AppendType.None:
                            break;

                        case AppendType.Append:
                            stringBuilder.Append(json.Substring(lastIndex, i + position - lastIndex));
                            lastIndex = i + position;
                            i = lastIndex;
                            break;

                        case AppendType.AppendLine:
                            stringBuilder.AppendLine(json.Substring(lastIndex, i + position - lastIndex));

                            switch (indentType)
                            {
                                case IndentType.Space:
                                    stringBuilder.Append(new string(' ', indent * indentSize));
                                    break;

                                case IndentType.Tab:
                                    stringBuilder.Append(new string('\t', indent));
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException("indentType", indentType, null);
                            }

                            lastIndex = i + position;
                            break;

                        case AppendType.AppendSpace:
                            stringBuilder.Append(json.Substring(lastIndex, i + position - lastIndex)).Append(' ');
                            lastIndex = i + position;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    appendType = AppendType.None;
                }

                ++i;
            }

            stringBuilder.Append(json.Substring(lastIndex));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// JSONを最小限に整形する
        /// </summary>
        /// <param name="json">json.</param>
        /// <returns>最小限に整形したJSON</returns>
        public static string ToMinifyPrint(string json)
        {
            json = Regex.Unescape(json);
            return new[] { Environment.NewLine, "\t", " " }.Aggregate(json, (current, oldValue) => current.Replace(oldValue, string.Empty));
        }
    }

    #endregion
}