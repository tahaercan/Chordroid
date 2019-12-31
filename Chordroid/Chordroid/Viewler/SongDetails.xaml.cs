using Acr.UserDialogs;
using ModernHttpClient;
using Poco.Model;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace Chordroid.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SongDetails : ContentPage
	{
        private Sarki SeciliSarki;
		public SongDetails (Sarki s)
		{
			InitializeComponent ();
            SeciliSarki = s;

            ToolbarItem tSongName = new ToolbarItem();
            tSongName.Text = s.Ad;
            ToolbarItems.Add(tSongName);

            EntName.Text = s.Ad;
            EntSpotifyLink.Text = s.SpotifyAdresi;
        }

        private void EntSpotifyLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                SeciliSarki.SpotifyAdresi = e.NewTextValue;
                Helper.SaveLocal(SeciliSarki);                
            }
        }

        private async void EntName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (hataliKarakterVar(e.NewTextValue))
            {
                await DisplayAlert("Warning", "Song name cannot contain / \\ . ? * < > | : % characters.", "Cancel");
                return;
            }
            string old = SeciliSarki.Ad;
            SeciliSarki.Ad = e.NewTextValue;
            Helper.SaveLocal(SeciliSarki, old);
            File.Move(Helper.SarkiAdindanPathBul(old), Helper.SarkiAdindanPathBul(e.NewTextValue));
        }

        private async void BtnUpload_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                ac.IsRunning = true;
                ac.IsVisible = true;

                var httpClient = new HttpClient(new NativeMessageHandler());                
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");                

                string message = JsonConvert.SerializeObject(SeciliSarki);                
                byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
                var content = new ByteArrayContent(messageBytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = httpClient.PostAsync(Helper.SunucuAdresi + "/api/Sarki/Upload", content).Result;
                if (response.StatusCode == HttpStatusCode.OK )
                {
                    await DisplayAlert("Upload Successful", "'" + SeciliSarki.Ad + "' has been uploaded successfully.", "OK");
                    if (SeciliSarki.Id == 0)
                    {
                        string s = await response.Content.ReadAsStringAsync();
                        SeciliSarki.Id = int.Parse(s);
                        Helper.SaveLocal(SeciliSarki);
                    }                    
                }
                else
                {
                    await DisplayAlert("Error", response.StatusCode.ToString() , "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                ac.IsRunning = false;
                ac.IsVisible = false;
            }
        }

        private void BtnSpotify_Clicked(object sender, System.EventArgs e)
        {
            if (SeciliSarki.SpotifyAdresi.ToLower().Contains("spotify.com"))
            {
                Device.OpenUri(new Uri(SeciliSarki.SpotifyAdresi));
            }
            else
            {
                DisplayAlert("Warning", "Incorrect spotify address!", "OK");
            }
        }

        private async void BtnDelete_Clicked(object sender, System.EventArgs e)
        {
            try
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
                    await DisplayAlert("Deleted", "'" + SeciliSarki.Ad + "' deleted locally.", "OK");
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
    }
}