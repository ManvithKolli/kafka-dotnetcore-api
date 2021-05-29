using System.Net;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// ToHtmlFormat added  to format files to replace &lt; with <  and &gt; with >
        /// And to replace \t , \n and \r with empty
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToHtmlFormat(this string input)
        {
            string decodeLesserAndGreaterThanResult = WebUtility.HtmlDecode(input);
            string ReplaceNewLineResult = Regex.Replace(decodeLesserAndGreaterThanResult, @"\t|\n|\r", "");
            return ReplaceNewLineResult;
        }
    }
}
