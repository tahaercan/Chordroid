
using Android.App;
using Android.Support.V7.App;

namespace Chordroid.Droid
{
    [Activity(Label = "Chordroid", Icon = "@drawable/guitar", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}