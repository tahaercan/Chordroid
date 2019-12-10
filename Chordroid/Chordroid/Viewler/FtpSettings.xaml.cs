using System;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chordroid.Viewler
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FtpSettings : ContentPage
    {
        public FtpSettings()
        {
            InitializeComponent();
            string temp = System.IO.File.ReadAllText(Helper.FtpSettingsPath);
            if (temp != "")
            {
                string[] ayarlar = temp.Split(Environment.NewLine);
                EntServer.Text = ayarlar[0];
                EntPort.Text = ayarlar[1];
                EntUsername.Text = ayarlar[2];
                EntPassword.Text = ayarlar[3];
            }
            else
            {
                DisplayAlert("Default", "FTP settings cannot be found. Default settings will be loaded.", "OK");
                EntServer.Text = "f17-preview.your-hosting.net";
                EntPort.Text = "21";
                EntUsername.Text = "3242617";
                EntPassword.Text = "qwerTy123";
            }
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            string ayarlar =
                EntServer.Text + Environment.NewLine +
                EntPort.Text + Environment.NewLine +
                EntUsername.Text + Environment.NewLine +
                EntPassword.Text;
            System.IO.File.WriteAllText(Helper.FtpSettingsPath, ayarlar, Encoding.UTF8);
            Helper.FtpUrl = EntServer.Text;
            Helper.FtpPort = Int32.Parse(EntPort.Text);
            Helper.FtpUserName = EntUsername.Text;
            Helper.FtpPassword = EntPassword.Text;

            DisplayAlert("Saved", "FTP settings saved.", "OK");
        }
    }
}