
using Android.App;
using Android.OS;
using Android.Support.V7.App;

namespace Chordroid.Droid
{
    [Activity(Label = "Chordroid", Icon = "@drawable/guitar", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}
