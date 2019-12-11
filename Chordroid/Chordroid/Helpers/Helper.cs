using Newtonsoft.Json;
using Poco.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Chordroid
{
    public static class Helper
    {
        public static string KlasorAdi = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Songs";
        public static string FtpSettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/FtpSettings.txt";
        public static string FtpUrl = "";
        public static int FtpPort = 0;
        public static string FtpUserName = "";
        public static string FtpPassword = "";

        public static string SqlUserName = "taha";
        public static string SqlIp = "94.73.146.4";
        public static string SqlPassword = "KUnl83G3BNri44M";
        public static string SqlInstance = "MSSQL49";

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

        public async static Task<DataSet> DataSetOku(string komut, string tabloadi)
        {

            //SqlConnection cn = new SqlConnection("Server=" + SqlIp + ";Database=Genel;User Id=" + SqlUserName + ";Password=" + SqlPassword);
            SqlConnection cn = new SqlConnection("Server=94.73.146.4,1433\\MSSQL49;Initial Catalog=Genel; User Id=taha;Password=KUnl83G3BNri44M;");
            DataSet ds = new DataSet();
            try
            {
                await cn.OpenAsync();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand(komut, cn);                
                da.Fill(ds,tabloadi);
            }
            catch (Exception ex)
            {
                string s = ex.Message; 
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }

    }
}
