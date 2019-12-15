using Poco.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace Chordroid.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ServerSongList : ContentPage
	{

        private ObservableCollection<SarkiItem> lSarkilar = new ObservableCollection<SarkiItem>();

        public ServerSongList ()
		{
			InitializeComponent ();

            ToolbarItem tSongCount = new ToolbarItem();
            tSongCount.Text = "";
            ToolbarItems.Add(tSongCount);

            ToolbarItem tDownloadSelectedSongs = new ToolbarItem();
            tDownloadSelectedSongs.Text = "DOWNLOAD SELECTED SONGS";
            tDownloadSelectedSongs.Icon = "cloud_computing_download.png";
            tDownloadSelectedSongs.Clicked += TDownloadSelectedSongs_Clicked;
            tDownloadSelectedSongs.Order = ToolbarItemOrder.Secondary;
            ToolbarItems.Add(tDownloadSelectedSongs);

            ToolbarItem tSelectAll = new ToolbarItem();
            tSelectAll.Text = "SELECT ALL";
            tSelectAll.Clicked += TSelectAll_Clicked;
            tSelectAll.Order = ToolbarItemOrder.Secondary;
            ToolbarItems.Add(tSelectAll);

            List();

        }

        private void TSelectAll_Clicked(object sender, EventArgs e)
        {
            foreach (SarkiItem s in lSarkilar.Where(x => x.Secili == false))
            {
                s.Secili = true;
            }
            ListviewSarki.ItemsSource = null;
            ListviewSarki.ItemsSource = lSarkilar.OrderBy(x => x.Ad); 
        }

        private async void List()
        {
            try
            {
                ac.IsVisible = true;
                ac.IsRunning = true;

                lSarkilar.Clear();

                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync("http://10.0.2.2:5000/api/Sarki/Get");
                var sarkiListesi = JsonConvert.DeserializeObject<List<Sarki>>(response);
                
                foreach (Sarki sar in sarkiListesi)
                {
                    SarkiItem s = new SarkiItem();
                    s.Id = sar.Id;
                    s.Ad = sar.Ad;
                    s.Link = "";
                    lSarkilar.Add(s);
                }

                BindingContext = lSarkilar.OrderBy(x => x.Ad);
                ToolbarItems[0].Text = lSarkilar.Count.ToString() + " SONGS CAN BE DOWNLOADED";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                ac.IsVisible = false;
                ac.IsRunning = false;
            }            
        }

        private async void TDownloadSelectedSongs_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(lSarkilar.Where(x => x.Secili == true).Count() > 0 )
                {
                    ac.IsVisible = true;
                    ac.IsRunning = true;
                    lblIndirilenSarki.IsVisible = true;
                    lblIndirilenSarki.Text = "Downloading Songs...";
                    string sarkiIdleri = "";
                    foreach (SarkiItem s in lSarkilar.Where(x => x.Secili == true))
                    {
                        sarkiIdleri += s.Id.ToString() + ",";
                    }
                    sarkiIdleri = sarkiIdleri.TrimEnd(','); 

                    var httpClient = new HttpClient();
                    var response = await httpClient.GetStringAsync("http://10.0.2.2:5000/api/Sarki/DownloadSongs/" + sarkiIdleri);
                    var sarkilar = JsonConvert.DeserializeObject<List<Sarki>>(response);

                    await DisplayAlert("Download Done", "Selected songs have been downloaded successfully.", "OK");
                    await Navigation.PopAsync();
                }                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }  
            finally
            {
                ac.IsVisible = false;
                ac.IsRunning = false;
                lblIndirilenSarki.IsVisible = false;
            }
        }
    }
}