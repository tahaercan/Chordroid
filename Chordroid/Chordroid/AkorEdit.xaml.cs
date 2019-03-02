using Chordroid.Model;
using Newtonsoft.Json;
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
	public partial class AkorEdit : ContentPage
	{
        Sarki SeciliSarki = null;

        public AkorEdit (Sarki s)
		{
			InitializeComponent ();

            SeciliSarki = s;

            ToolbarItem tSongName = new ToolbarItem();
            tSongName.Text = s.Ad;
            ToolbarItems.Add(tSongName);

            ToolbarItem tAddRow = new ToolbarItem();
            tAddRow.Text = "ADD ROW";
            tAddRow.Icon = "add.png";
            tAddRow.Clicked += TAddRow_Clicked;
            ToolbarItems.Add(tAddRow);

            ToolbarItem tDeleteRow = new ToolbarItem();
            tDeleteRow.Text = "DELETE ROW";
            tDeleteRow.Icon = "delete.png";
            tDeleteRow.Clicked += TDeleteRow_Clicked;
            ToolbarItems.Add(tDeleteRow);

            BindingContext = SeciliSarki.Satirlar;
        }

        private void TDeleteRow_Clicked(object sender, EventArgs e)
        {
            Helper.Save(SeciliSarki);
        }

        private void TAddRow_Clicked(object sender, EventArgs e)
        {
            var maxValue = 0;
            if (SeciliSarki.Satirlar.Count > 0)
            {
                maxValue = SeciliSarki.Satirlar.Max(x => x.Sira);
            }
            Satir s = new Satir();
            s.Sira = maxValue + 1;
            SeciliSarki.Satirlar.Add(s);
            Helper.Save(SeciliSarki);
        }


        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Helper.Save(SeciliSarki);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            foreach (Satir s in SeciliSarki.Satirlar)
            {
                s.Renk = s.AkorSatiri == true ? Color.Red : Color.Black;
            }
            Helper.Save(SeciliSarki);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            int i = 0;
        }
    }
}