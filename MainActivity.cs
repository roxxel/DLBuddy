using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace DLBuddy
{
    [Activity(Label = "DLBuddy", Theme = "@style/AppTheme", MainLauncher = true)]
    [IntentFilter(new[] { "android.intent.action.SEND" }, 
        Categories = new[] { "android.intent.category.BROWSABLE", "android.intent.category.DEFAULT" }, 
        DataMimeTypes = new[] { "text/plain" })]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            if (this.Intent.Action == "android.intent.action.SEND")
            {
                // if sharing, hide UI immediately. it's ok bud
                Finish();
                Toast.MakeText(this, "Attempting download...", ToastLength.Short).Show();

                var dl = new Downloader(this);

                var uri = this.Intent.Extras.GetString("android.intent.extra.TEXT");
                if (dl.TryDownloadFile(uri, out var e))
                {
                    Toast.MakeText(this, $"Enqueued download.", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, $"Failed downloading. {e?.Message}", ToastLength.Short).Show();
                }
                // use libvideo for youtube
            }

            this.FindViewById<Button>(Resource.Id.button1).Click += MainActivity_Click;

            this.FindViewById<TextView>(Resource.Id.textView3).Text = "Supported apps: " + ParserFinder.ListParsers();
        }

        private void MainActivity_Click(object sender, System.EventArgs e)
        {
            Finish();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}