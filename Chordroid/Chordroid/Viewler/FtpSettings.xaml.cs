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
            string temp = System.IO.File.ReadAllText(Helper.SunucuDosyasiPath);
            if (temp != "")
            {
                EntServer.Text = temp;
            }
            else
            {
                DisplayAlert("Default", "Server settings cannot be found. Default settings will be loaded.", "OK");
                EntServer.Text = "https://chordroid.azurewebsites.net/";                
            }
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(Helper.SunucuDosyasiPath, EntServer.Text, Encoding.UTF8);
            Helper.SunucuAdresi = EntServer.Text;            
            DisplayAlert("Saved", "Server settings saved.", "OK");
        }
    }
}