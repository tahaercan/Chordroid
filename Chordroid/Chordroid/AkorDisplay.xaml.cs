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
	public partial class AkorDisplay : ContentPage
	{
        Sarki SeciliSarki = null;

        public AkorDisplay (Sarki s)
		{
			InitializeComponent ();

            SeciliSarki = s;

            ToolbarItem tLow = new ToolbarItem();
            tLow.Text = "LOW";
            tLow.Icon = "music_blue.png";
            tLow.Clicked += TLow_Clicked;
            ToolbarItems.Add(tLow);

            ToolbarItem tHigh = new ToolbarItem();
            tHigh.Text = "HIGH";
            tHigh.Icon = "music_yellow.png";
            tHigh.Clicked += THigh_Clicked;
            ToolbarItems.Add(tHigh);

            ToolbarItem tEdit = new ToolbarItem();
            tEdit.Text = "EDIT";
            tEdit.Icon = "pencil.png";
            tEdit.Clicked += TEdit_Clicked;
            ToolbarItems.Add(tEdit);

            stepper.Value = SeciliSarki.AkorFontBuyuklugu;
            
            BindingContext = SeciliSarki.Satirlar;
                        
        }

        private void THigh_Clicked(object sender, EventArgs e)
        {
            PesTiz(false);
            Helper.Save(SeciliSarki);
            ListviewRefresh();
        }

        private void TLow_Clicked(object sender, EventArgs e)
        {
            PesTiz(true);
            Helper.Save(SeciliSarki);
            ListviewRefresh();
        }

        private void ListviewRefresh()
        {
            var itemsSource = ListviewSatirlar.ItemsSource;
            ListviewSatirlar.ItemsSource = null;
            ListviewSatirlar.ItemsSource = itemsSource;
        }

        private async void TEdit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AkorEdit(SeciliSarki));
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SeciliSarki.AkorFontBuyuklugu = (int)e.NewValue;
            Helper.Save(SeciliSarki);
        }


        private void PesTiz(bool peslestir)
        {
            foreach (Satir satir in SeciliSarki.Satirlar)
            {
                if (satir.AkorSatiri)
                {
                    string[] akorlar = satir.Metin.Split(' ');
                    string yeniMetin = "";
                    for (int i = 0; i < akorlar.Length; i++)
                    {
                        if (akorlar[i] != "")
                        {
                            yeniMetin += transpozeEt(akorlar[i], peslestir);
                            if (i < akorlar.Length - 1)
                            {
                                yeniMetin += " ";
                            }
                        }
                        else
                        {
                            yeniMetin += " ";
                        }
                    }
                    satir.Metin = yeniMetin;
                }
            }            
        }

        private string transpozeEt(string akor, bool peslestir)
        {
            string nota = akor.Substring(0, 1);
            string yeniNota = "";
            if (akor.Length > 1)
            {
                string ikinciKarakter = akor.Substring(1, 1);
                if (ikinciKarakter == "#" || ikinciKarakter == "b")
                {
                    nota = nota + ikinciKarakter;
                }
            }
            if (peslestir)
            {
                switch (nota.ToUpper())
                {
                    case "C":
                        yeniNota = "B";
                        break;
                    case "C#":
                        yeniNota = "C";
                        break;
                    case "D":
                        yeniNota = "C#";
                        break;
                    case "D#":
                    case "Eb":
                        yeniNota = "D";
                        break;
                    case "E":
                        yeniNota = "D#";
                        break;
                    case "F":
                        yeniNota = "E";
                        break;
                    case "F#":
                        yeniNota = "F";
                        break;
                    case "G":
                        yeniNota = "F#";
                        break;
                    case "G#":
                        yeniNota = "G";
                        break;
                    case "A":
                        yeniNota = "G#";
                        break;
                    case "A#":
                    case "Bb":
                        yeniNota = "A";
                        break;
                    case "B":
                        yeniNota = "A#";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (nota)
                {
                    case "C":
                        yeniNota = "C#";
                        break;
                    case "C#":
                        yeniNota = "D";
                        break;
                    case "D":
                        yeniNota = "D#";
                        break;
                    case "D#":
                    case "Eb":
                        yeniNota = "E";
                        break;
                    case "E":
                        yeniNota = "F";
                        break;
                    case "F":
                        yeniNota = "F#";
                        break;
                    case "F#":
                        yeniNota = "G";
                        break;
                    case "G":
                        yeniNota = "G#";
                        break;
                    case "G#":
                        yeniNota = "A";
                        break;
                    case "A":
                        yeniNota = "A#";
                        break;
                    case "A#":
                    case "Bb":
                        yeniNota = "B";
                        break;
                    case "B":
                        yeniNota = "C";
                        break;
                    default:
                        break;
                }
            }
            return yeniNota + akor.Substring(akor.IndexOf(nota) + nota.Length);
        }

    }
}