using Acr.UserDialogs;
using Poco.Model;
using Chordroid.View;
using Chordroid.Viewler;
using Newtonsoft.Json;
using Plugin.Clipboard;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chordroid
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<SarkiItem> lSarkilar = new ObservableCollection<SarkiItem>();
        private Sarki SeciliSarki;
        private string SongListText;
        public MainPage()
        {
            try
            {
                InitializeComponent();

                if (!Directory.Exists(Helper.KlasorAdi))
                {
                    Directory.CreateDirectory(Helper.KlasorAdi);
                }
                else
                {
                    //Directory.Delete(Helper.KlasorAdi, true);
                }

                if (System.IO.File.Exists(Helper.SunucuDosyasiPath))
                {
                    string temp = System.IO.File.ReadAllText(Helper.SunucuDosyasiPath);
                    if (temp != "")
                    {
                        Helper.SunucuAdresi = temp.TrimEnd('/');
                    }
                }
                else
                {
                    System.IO.FileStream fs = File.Create(Helper.SunucuDosyasiPath);
                    fs.Close();
                    fs.Dispose();
                }

                ToolbarItem tSongCount = new ToolbarItem();
                tSongCount.Text = "";
                ToolbarItems.Add(tSongCount);

                ToolbarItem tAddSong = new ToolbarItem();
                tAddSong.Text = "ADD NEW SONG";
                tAddSong.Icon = "add.png";
                tAddSong.Clicked += TAddSong_Clicked;
                tAddSong.Order = ToolbarItemOrder.Secondary;
                ToolbarItems.Add(tAddSong);

                ToolbarItem tImportSong = new ToolbarItem();
                tImportSong.Text = "DOWNLOAD SONGS";
                tImportSong.Clicked += TImportSong_Clicked;
                tImportSong.Order = ToolbarItemOrder.Secondary;
                ToolbarItems.Add(tImportSong);

                ToolbarItem tSongList = new ToolbarItem();
                tSongList.Text = "COPY SONG LIST";
                tSongList.Clicked += TSongList_Clicked; 
                tSongList.Order = ToolbarItemOrder.Secondary;
                ToolbarItems.Add(tSongList);

                ToolbarItem tFtpSettings = new ToolbarItem();
                tFtpSettings.Text = "SERVER SETTINGS";
                tFtpSettings.Clicked += TFtpSettings_Clicked;
                tFtpSettings.Order = ToolbarItemOrder.Secondary;
                ToolbarItems.Add(tFtpSettings);

                ListviewSarki.ItemSelected += ListviewSarki_ItemSelected;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");                
            }            
        }

        protected override void OnAppearing()
        {
            refreshList();
        }

        private void refreshList()
        {
            ac.IsRunning = true;
            ac.IsVisible = true;

            lSarkilar.Clear();
            
            foreach (string song in System.IO.Directory.GetFiles(Helper.KlasorAdi))
            {
                if (System.IO.Path.GetExtension(song) == ".json")
                {
                    SarkiItem s = new SarkiItem();
                    s.Ad = System.IO.Path.GetFileNameWithoutExtension(song);
                    lSarkilar.Add(s);
                }
            }

            SongListText = "";
            int i = 0;
            foreach (SarkiItem s in lSarkilar.OrderBy(x=>x.Ad))
            {
                i++;
                SongListText += i.ToString() + ". " + s.Ad + System.Environment.NewLine;
            }

            BindingContext = lSarkilar.OrderBy(x=> x.Ad);
            ToolbarItems[0].Text = lSarkilar.Count.ToString() + " SONGS";

            ac.IsRunning = false;
            ac.IsVisible = false;

        }

        #region TOOLBAR
        
        private async void TAddSong_Clicked(object sender, EventArgs e)
        {
            try
            {
                PromptResult pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
                {
                    InputType = InputType.Name,
                    OkText = "Add",
                    CancelText = "Cancel",
                    Title = "Add New Song",
                });
                if (pResult.Ok && !string.IsNullOrWhiteSpace(pResult.Text))
                {
                    if (hataliKarakterVar(pResult.Text))
                    {
                        await DisplayAlert("Warning", "Song name cannot contain / \\ . ? * < > | : % characters.", "Cancel");
                        return;
                    }

                    SeciliSarki = new Sarki();
                    SeciliSarki.Ad = pResult.Text;
                    for(int i = 0;i<5;i++)
                    {
                        Satir satir = new Satir();
                        satir.Sira = i;
                        if (i==0)
                        {
                            satir.Metin = "Please edit song...";
                        }
                        SeciliSarki.Satirlar.Add(satir);
                    }

                    Helper.SaveLocal(SeciliSarki);

                    SarkiItem s = new SarkiItem();
                    s.Ad = pResult.Text;
                    lSarkilar.Add(s);
                    refreshList();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
            
        }

        private bool hataliKarakterVar(string sarkiAdi)
        {
            if (sarkiAdi.Contains('/') ||
                    sarkiAdi.Contains('\\') ||
                    sarkiAdi.Contains('.') ||
                    sarkiAdi.Contains('?') ||
                    sarkiAdi.Contains('*') ||
                    sarkiAdi.Contains('<') ||
                    sarkiAdi.Contains('>') ||
                    sarkiAdi.Contains('"') ||
                    sarkiAdi.Contains(':') ||
                    sarkiAdi.Contains('|') ||
                    sarkiAdi.Contains('%'))
            {
                return true;                
            }
            else
            {
                return false;
            }
        }

        private async void TImportSong_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ServerSongList());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }

        private void TSongList_Clicked(object sender, EventArgs e)
        {
            CrossClipboard.Current.SetText(SongListText);
            DisplayAlert("Song List", "Song list is copied to clipboard.", "OK");
        }

        private async void TFtpSettings_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new FtpSettings());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
        }

        #endregion

        private async void ListviewSarki_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    string path = Helper.SarkiAdindanPathBul(((SarkiItem)e.SelectedItem).Ad);
                    if (File.Exists(path))
                    {
                        DosyadanSarkiOku(path);
                        if (SeciliSarki == null)
                        {
                            await DisplayAlert("Error", "There is a problem on song file. Please download this song again.", "OK");                            
                        }
                        else
                        {
                            await Navigation.PushAsync(new DisplayCarousel(SeciliSarki));
                        }                        
                    }
                }
            }
            catch (Exception ex )
            {
               await DisplayAlert("Error", ex.Message, "OK");
            }            
        }

        private void DosyadanSarkiOku(string path)
        {
            try
            {
                using (StreamReader file = File.OpenText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    SeciliSarki = (Sarki)serializer.Deserialize(file, typeof(Sarki));
                }
            }
            catch (Exception)
            {
                SeciliSarki = null;
            }            
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ListviewSarki.BeginRefresh();
                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                    ListviewSarki.ItemsSource = lSarkilar;
                else
                    ListviewSarki.ItemsSource = lSarkilar.Where(i => i.Ad.ToLower().Contains(e.NewTextValue.ToLower()));
                ListviewSarki.EndRefresh();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK", "Cancel");
            }
            
        }      
        
    }
}
