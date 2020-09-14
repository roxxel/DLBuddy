using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace DLBuddy.Parsers
{
    public class IFunnyParser : IParser
    {
        const string mp4regex = @"https://img.ifunny.co/videos/.*?\.mp4";

        public string GetParserName()
        {
            return "ifunny";
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
                result = regx.Match(page).Captures.First().Value;
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

            if (input.StartsWith("Tap to see the meme - https://ifunny.co/fun/"))
            {
                result = input.Substring(22);
                return true;
            }
            return false;
        }
    }
}