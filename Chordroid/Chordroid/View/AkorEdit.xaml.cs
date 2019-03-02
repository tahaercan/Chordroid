using Chordroid.Model;
using System;
using System.Linq;

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
            tSongName.Text = s.Ad + " (EDIT MODE)";
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
            tDeleteRow.Order = ToolbarItemOrder.Secondary;
            ToolbarItems.Add(tDeleteRow);

            ToolbarItem tUp = new ToolbarItem();
            tUp.Text = "UP";
            tUp.Icon = "arrow_up_green.png";
            tUp.Clicked += TUp_Clicked;
            tUp.Order = ToolbarItemOrder.Secondary;
            ToolbarItems.Add(tUp);

            ToolbarItem tDown = new ToolbarItem();
            tDown.Text = "DOWN";
            tDown.Icon = "arrow_down_green.png";
            tDown.Clicked += TDown_Clicked;
            tDown.Order = ToolbarItemOrder.Secondary;
            ToolbarItems.Add(tDown);

            BindingContext = SeciliSarki.Satirlar.OrderBy(x => x.Sira);
        }

        private void refreshList()
        {
            ListviewSatirlar.ItemsSource = null;
            ListviewSatirlar.ItemsSource = SeciliSarki.Satirlar.OrderBy(x => x.Sira);
        }

        private void TDown_Clicked(object sender, EventArgs e)
        {
            if (ListviewSatirlar.SelectedItem == null) return;
            Satir seciliSatir = (Satir)ListviewSatirlar.SelectedItem;
            if (seciliSatir.Sira <SeciliSarki.Satirlar.Count-1)
            {
                Satir alttakiSatir = SeciliSarki.Satirlar.Where(x => x.Sira == seciliSatir.Sira + 1).FirstOrDefault();
                alttakiSatir.Sira = seciliSatir.Sira;
                seciliSatir.Sira += 1;
                SeciliSarki.Satirlar.OrderBy(x => x.Sira);
                refreshList();
            }
        }

        private void TUp_Clicked(object sender, EventArgs e)
        {
            if (ListviewSatirlar.SelectedItem == null) return;
            Satir seciliSatir = (Satir)ListviewSatirlar.SelectedItem;
            if (seciliSatir.Sira>0)
            {
                Satir usttekiSatir = SeciliSarki.Satirlar.Where(x => x.Sira == seciliSatir.Sira - 1).FirstOrDefault();
                usttekiSatir.Sira = seciliSatir.Sira;
                seciliSatir.Sira -= 1;
                refreshList();
            }            
        }

        private void TDeleteRow_Clicked(object sender, EventArgs e)
        {
            if (ListviewSatirlar.SelectedItem == null) return;
            Satir seciliSatir = (Satir)ListviewSatirlar.SelectedItem;

            //BENDEN SONRAKİ SATIRLARIN NUMARASINI BİR EKSİLT
            foreach (Satir s in SeciliSarki.Satirlar)
            {
                if (s.Sira > seciliSatir.Sira)
                {
                    s.Sira -= 1;
                }
            }

            SeciliSarki.Satirlar.Remove(seciliSatir);
            Helper.Save(SeciliSarki);
            refreshList();
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

            refreshList();
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

    }
}