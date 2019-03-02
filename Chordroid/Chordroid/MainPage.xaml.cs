using Acr.UserDialogs;
using Android.Views;
using Chordroid.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chordroid
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<SarkiItem> lSarkilar = new ObservableCollection<SarkiItem>();
        private string KlasorPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Songs";
        private Sarki SeciliSarki;
        public MainPage()
        {
            try
            {
                InitializeComponent();

                if (!Directory.Exists(KlasorPath))
                {
                    Directory.CreateDirectory(KlasorPath);
                }
                else
                {
                    //Directory.Delete(KlasorPath, true);
                }

                ToolbarItem tAddSong = new ToolbarItem();
                tAddSong.Text = "ADD SONG";
                tAddSong.Icon = "add.png";
                tAddSong.Clicked += TAddSong_Clicked;
                ToolbarItems.Add(tAddSong);

                ToolbarItem tDeleteSong = new ToolbarItem();
                tDeleteSong.Text = "DELETE SONG";
                tDeleteSong.Icon = "delete.png";
                tDeleteSong.Clicked += TDeleteSong_Clicked;
                ToolbarItems.Add(tDeleteSong);

                ToolbarItem tEditSong = new ToolbarItem();
                tEditSong.Text = "EDIT SONG";
                tEditSong.Icon = "Pencil.png";
                tEditSong.Clicked += TEditSong_Clicked;
                ToolbarItems.Add(tEditSong);

                ToolbarItem tOpenSong = new ToolbarItem();
                tOpenSong.Text = "OPEN SONG";
                tOpenSong.Icon = "document_music.png";
                tOpenSong.Clicked += TOpenSong_Clicked;
                ToolbarItems.Add(tOpenSong);

                foreach (string song in System.IO.Directory.GetFiles(KlasorPath))
                {
                    if (System.IO.Path.GetExtension(song) == ".json")
                    {
                        SarkiItem s = new SarkiItem();
                        s.Ad = System.IO.Path.GetFileNameWithoutExtension(song);
                        lSarkilar.Add(s);
                    }
                }

                BindingContext = lSarkilar;

                ListviewSarki.ItemSelected += ListviewSarki_ItemSelected;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");                
            }            
        }

        private  void ListviewSarki_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    string path = Helper.SarkiAdindanPathBul(((SarkiItem)e.SelectedItem).Ad);
                    if (File.Exists(path))
                    {
                        using (StreamReader file = File.OpenText(path))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            SeciliSarki = (Sarki)serializer.Deserialize(file, typeof(Sarki));
                            if (SeciliSarki == null)
                            {
                                DisplayAlert("Error", "Null Song", "OK");
                                File.Delete(path);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex )
            {
                DisplayAlert("Error", ex.Message, "OK");
            }            
        }

        private async void TOpenSong_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ListviewSarki.SelectedItem != null)
                {
                    await Navigation.PushAsync(new DisplayCarousel(SeciliSarki));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message,"OK","Cancel");
            }            
        }

        
        private async void TDeleteSong_Clicked(object sender, EventArgs e)
        {
            if (SeciliSarki == null) return;
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Message = "Are you sure you want to delete selected song?",
                OkText = "Delete",
                CancelText = "Cancel"
            });
            if (result)
            {
                File.Delete(Helper.SarkiAdindanPathBul(SeciliSarki.Ad));

                ListviewSarki.BeginRefresh();
                var silinecekSarkiItem = (from itm in lSarkilar
                                          where itm.Ad == SeciliSarki.Ad
                                          select itm).FirstOrDefault<SarkiItem>();

                lSarkilar.Remove(silinecekSarkiItem);
                ListviewSarki.EndRefresh();
            }
        }

        private async void TAddSong_Clicked(object sender, EventArgs e)
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
                SeciliSarki = new Sarki();
                SeciliSarki.Ad = pResult.Text;

                Helper.Save(SeciliSarki);

                ListviewSarki.BeginRefresh();
                SarkiItem s = new SarkiItem();
                s.Ad = pResult.Text;
                lSarkilar.Add(s);
                ListviewSarki.EndRefresh();
            }
        }

        private async void TEditSong_Clicked(object sender, EventArgs e)
        {
            PromptResult pResult = await UserDialogs.Instance.PromptAsync(new PromptConfig
            {
                InputType = InputType.Name,
                Message = SeciliSarki.Ad,
                OkText = "Change",
                CancelText = "Cancel",
                Title = "Change Song",
            });
            if (pResult.Ok && !string.IsNullOrWhiteSpace(pResult.Text))
            {
                string old = SeciliSarki.Ad;
                SeciliSarki.Ad = pResult.Text;
                SarkiItem s = (SarkiItem)ListviewSarki.SelectedItem;
                s.Ad = SeciliSarki.Ad;
                File.Move(Helper.SarkiAdindanPathBul(old), Helper.SarkiAdindanPathBul(pResult.Text));
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListviewSarki.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                ListviewSarki.ItemsSource = lSarkilar;
            else
                ListviewSarki.ItemsSource = lSarkilar.Where(i => i.Ad.ToLower().Contains(e.NewTextValue.ToLower()));

            ListviewSarki.EndRefresh();
        }      
        
    }
}
