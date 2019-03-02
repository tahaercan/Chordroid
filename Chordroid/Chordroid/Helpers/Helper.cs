using Chordroid.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chordroid
{
    public static class Helper
    {
        public static string KlasorAdi = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Songs";
        public static string FtpUrl = "ftpupload.net";
        public static int FtpPort = 21;
        public static string FtpUserName = "unaux_23475095";
        public static string FtpPassword = "la5uj4jce";

        public static void Save(Sarki sar, string sarkiadi="")
        {
                string json = JsonConvert.SerializeObject(sar);
                if (sarkiadi=="")
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
