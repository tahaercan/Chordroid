using Poco.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chordroid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AkorLyrics : ContentPage
	{
        Sarki SeciliSarki = null;
        private bool ekranaAliniyor = true;
        public AkorLyrics (Sarki s)
		{
			InitializeComponent ();

            SeciliSarki = s;

            ToolbarItem tSongName = new ToolbarItem();
            tSongName.Text = s.Ad;
            ToolbarItems.Add(tSongName);

            stepper.Value = SeciliSarki.SozFontBuyuklugu;

            LabelLyric.Text = s.SozRtf;
            ekranaAliniyor = false;
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (ekranaAliniyor) return;
            SeciliSarki.SozFontBuyuklugu = (int)e.NewValue;
            Helper.Save(SeciliSarki);
        }
    }
}