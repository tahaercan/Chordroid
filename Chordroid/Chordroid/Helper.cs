using Chordroid.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chordroid
{
    public static class Helper
    {
        public static string KlasorAdi="";

        public static void Save(Sarki sar)
        {
            string json = JsonConvert.SerializeObject(sar);
            System.IO.File.WriteAllText(SarkiAdindanPathBul(sar.Ad), json, Encoding.UTF8);
        }

        public static string SarkiAdindanPathBul(string sarkiAdi)
        {
            string KlasorAdi = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Songs";
            return KlasorAdi + '/' + sarkiAdi + ".json";
        }

    }
}
