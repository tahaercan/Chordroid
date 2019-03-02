using Chordroid.Model;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

                FtpClient client = new FtpClient(Helper.FtpUrl);
                client.Credentials = new NetworkCredential(Helper.FtpUserName, Helper.FtpPassword );
                client.Port = 21;
                await client.ConnectAsync();

                foreach (FtpListItem item in client.GetListing("/htdocs"))
                {
                    if (item.Name.ToLower().EndsWith(".json"))
                    {
                        SarkiItem s = new SarkiItem();
                        s.Ad = item.Name.Substring(0, item.Name.LastIndexOf("."));
                        s.Link = item.FullName;
                        lSarkilar.Add(s);
                    }
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
                ac.IsVisible = true;
                ac.IsRunning = true;
                lblIndirilenSarki.IsVisible = true;

                FtpClient client = new FtpClient(Helper.FtpUrl);
                client.Credentials = new NetworkCredential(Helper.FtpUserName, Helper.FtpPassword);
                client.Port = Helper.FtpPort;
                await client.ConnectAsync();

                foreach (SarkiItem s in lSarkilar.Where(x => x.Secili == true))
                {
                    await client.DownloadFileAsync(Helper.SarkiAdindanPathBul(s.Ad), s.Link, true,FtpVerify.Retry);
                    lblIndirilenSarki.Text = "Downloading " + s.Ad;
                }

                await DisplayAlert("Download Done", "Selected songs have been downloaded successfully.", "OK");
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