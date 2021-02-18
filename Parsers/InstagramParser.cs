using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace DLBuddy.Parsers
{
    public class InstagramParser : IParser
    {

        public string GetParserName()
        {
            return "instagram";
        }

        public bool TryFetchVideoUrl(string source, out string[] result)
        {
            result = null;
            try
            {
                var web = new WebClient();
                Uri uri = new Uri(source);
                var page = web.DownloadString($"{uri.Scheme}://{uri.Host}{uri.AbsolutePath}?__a=1");
                var urls = new List<string>();

                var json = JObject.Parse(page);
                if (json["graphql"]["shortcode_media"]["edge_sidecar_to_children"] == null)
                {
                    if (json["graphql"]["shortcode_media"]["is_video"].Value<bool>())
                    {
                        urls.Add(json["graphql"]["shortcode_media"]["video_url"].Value<string>());
                    }
                    else
                    {
                        urls.Add(json["graphql"]["shortcode_media"]["display_url"].Value<string>());
                    }
                }
                else
                {
                    var edges = json["graphql"]["shortcode_media"]["edge_sidecar_to_children"]["edges"].AsJEnumerable();

                    foreach (var edge in edges)
                    {
                        var isVideo = edge["node"]["is_video"].Value<bool>();
                        if (isVideo)
                        {
                            urls.Add(edge["node"]["video_url"].Value<string>());
                        }
                        else
                        {
                            urls.Add(edge["node"]["display_url"].Value<string>());
                        }
                    }

                }
                result = urls.ToArray();

            }
            catch (Exception e)
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

            if (url.StartsWith("https://www.instagram.com/p/") || url.StartsWith("https://www.instagram.com/tv/"))
            {
                result = url;
                return true;
            }
            return false;
        }
    }
}