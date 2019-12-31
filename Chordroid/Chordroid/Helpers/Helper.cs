using Newtonsoft.Json;
using Poco.Model;
using System;
using System.Text;

namespace Chordroid
{
    public static class Helper
    {
        public static string KlasorAdi = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Songs";
        public static string SunucuDosyasiPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Sunucu.txt";
        //public static string SunucuAdresi = "https://chordroid.azurewebsites.net/";
        public static string SunucuAdresi = "http://10.0.2.2:5000"; //local için
        
        public static void SaveLocal(Sarki sar, string sarkiadi="")
        {
                string json = JsonConvert.SerializeObject(sar);
                if (sarkiadi == "")
                {
                    System.IO.File.WriteAllText(SarkiAdindanPathBul(sar.Ad), json, Encoding.UTF8);
                }
                else
                {
                    System.IO.File.WriteAllText(SarkiAdindanPathBul(sarkiadi), json, Encoding.UTF8);
                }  
        }

        public static string SarkiAdindanPathBul(string sarkiAdi)
        {
            return KlasorAdi + '/' + sarkiAdi + ".json";
        }
        
    }
}
