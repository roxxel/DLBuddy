using System;
using System.Linq;
using System.Reflection;
using Android.Content;
using Android.Widget;
using DLBuddy.Parsers;

namespace DLBuddy
{
    public static class ParserFinder
    {
        public static IParser FindParser(Context ctx, string input, out string result)
        {
            result = null;

            var parsers = Assembly.GetExecutingAssembly().GetTypes().Where(x => typeof(IParser).IsAssignableFrom(x) && x != typeof(IParser));

            foreach(var p in parsers)
            {
                var parser = (IParser)Activator.CreateInstance(p);

                if(parser.TryParseShareText(input, out result))
                {
                    Toast.MakeText(ctx, $"Detected {parser.GetParserName()} link.", ToastLength.Short).Show();
                    return parser;
                }
            }

            return null;
        }

        public static string ListParsers()
        {
            var parsers = Assembly.GetExecutingAssembly().GetTypes().Where(x => typeof(IParser).IsAssignableFrom(x) && x != typeof(IParser));

            return string.Join(", ", parsers.Select(x => ((IParser)Activator.CreateInstance(x)).GetParserName()));
        }
    }
}