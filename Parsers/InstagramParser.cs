using System;
using System.Net;
using System.Text.RegularExpressions;

namespace DLBuddy.Parsers
{
    public class InstagramParser : IParser
    {
        const string mp4regex = "property=\"og:video\" content=\"https://.*?\"";

        public string GetParserName()
        {
            return "instagram";
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

            // if valid insta, this should be a url
            var url = input.Substring(input.IndexOf("https://"));

            if (url.StartsWith("https://www.instagram.com/p/"))
            {
                result = url;
                return true;
            }
            return false;
        }
    }
}