using System;

using Android.App;
using Android.Content;
using Android.Webkit;
using Android.Widget;

namespace DLBuddy
{
    public class Downloader
    {
        DownloadManager manager;
        Context ctx;

        public Downloader(Context ctx)
        {
            this.manager = DownloadManager.FromContext(ctx);
            this.ctx = ctx;
        }

        // for now it only downloads ifunny
        public bool TryDownloadFile(string input, out Exception ex)
        {
            ex = null;
            try
            {
                var parser = ParserFinder.FindParser(this.ctx, input, out var url);

                if(parser == null)
                {
                    Toast.MakeText(ctx, "Incompatible site.", ToastLength.Short).Show();
                    return false;
                }

                parser.TryFetchVideoUrl(url, out string[] urls);

                foreach (var _url in urls)
                {
                    var uri = Android.Net.Uri.Parse(_url);
                    var request = new DownloadManager.Request(uri);
                    string fileExtension = MimeTypeMap.GetFileExtensionFromUrl(_url);
                    string filename = $"download.{fileExtension}";

                    request.SetDestinationInExternalPublicDir(Android.OS.Environment.DirectoryDownloads,
                        $"DLBuddy/{parser.GetParserName()}/{filename}");

                    request.SetVisibleInDownloadsUi(true);
                    request.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                    request.SetTitle($"DLBuddy - {parser.GetParserName()}");
                    manager.Enqueue(request);
                }


                return true;
            }
            catch (Exception e)
            {
                ex = e;
                return false;
            }
        }
    }
}