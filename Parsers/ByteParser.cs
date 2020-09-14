using System;
using System.Net;
using System.Text.RegularExpressions;

namespace DLBuddy.Parsers
{
    public class ByteParser : IParser
    {
        const string mp4regex = "property=\"og:video\" content=\"https://.*?\"";

        public string GetParserName()
        {
            return "byte";
        }

        // could possibly just use substring magic because of og:video tags.
        public bool TryFetchVideoUrl(string source, out string result)
        {
            result = null;
            try
            {
                var web = new WebClient();

                var page = web.DownloadString(source);

                var regx = new Regex(mp4regex);
                var match = regx.Match(page).Value;
                result = match.Substring(29);
                result = result.Remove(result.Length - 1);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool TryParseShareText(string input, out string result)
        {
            result = null;

            if (input.StartsWith("https://byte.co/b/"))
            {
                result = input;
                return true;
            }
            return false;
        }
    }
}