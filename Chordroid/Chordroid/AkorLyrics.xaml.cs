using Chordroid.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chordroid
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AkorLyrics : ContentPage
	{
        Sarki SeciliSarki = null;
        public AkorLyrics (Sarki s)
		{
			InitializeComponent ();

            SeciliSarki = s;
		}

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            SeciliSarki.SozRtf = e.NewTextValue;
        }
    }
}