using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Poco.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace ChordroidWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SarkiController : ControllerBase
    {
        private string ConnectionString = "";

        public SarkiController()
        {
            ConnectionString = "Server=94.73.146.4,1433\\MSSQL49;Initial Catalog=Genel; User Id=taha;Password=KUnl83G3BNri44M;";
        }

        // GET: api/Sarki
        [HttpGet]
        public async Task<IEnumerable<Sarki>> GetAll()
        {
            List<Sarki> l = new List<Sarki>();
            DataSet ds = await DataSetOku("select Id,Ad from Genel..Sarki with (nolock)", "SarkiListesi");
            try
            {
                foreach (DataRow r in ds.Tables["SarkiListesi"].Rows)
                {
                    Sarki s = new Sarki();
                    s.Id = (int)r["Id"];
                    s.Ad = r["Ad"].ToString();
                    l.Add(s);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return l;
        }

        [HttpGet("{sarkiIdleri}", Name = "DownloadSongs")]
        public async Task<IEnumerable<Sarki>> DownloadSongs(string sarkiIdleri)
        {
            List<Sarki> l = new List<Sarki>();
            try
            {
                DataSet dsSarki = await DataSetOku("select * from Genel..Sarki with (nolock) where Id in (" + sarkiIdleri + ")", "Sarki");
                DataSet dsSatir = await DataSetOku("select * from Genel..Satir with (nolock) where SarkiId in (" + sarkiIdleri + ")", "Satir");
                foreach (DataRow rSarki in dsSarki.Tables["Sarki"].Rows)
                {
                    Sarki s = new Sarki();
                    s.Id = (int)rSarki["Id"];
                    s.Ad = rSarki["Ad"].ToString();
                    s.AkorFontBuyuklugu = (int)rSarki["AkorFontBuyuklugu"];
                    s.SozFontBuyuklugu = (int)rSarki["SozFontBuyuklugu"];
                    s.SozRtf = "";
                    s.SpotifyAdresi = rSarki["SpotifyAdresi"].ToString();
                    s.Satirlar = new ObservableCollection<Satir>();
                    foreach (DataRow rSatir in dsSatir.Tables["Satir"].Select("SarkiId=" + rSarki["Id"].ToString()))
                    {
                        Satir sa = new Satir();
                        sa.SarkiId = s.Id;
                        sa.AkorSatiri = (bool)rSatir["AkorSatiri"];
                        sa.Metin = rSatir["Metin"].ToString();
                        sa.Sira = (int)rSatir["Sira"];
                        s.Satirlar.Add(sa);
                    }
                    l.Add(s);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return l;
        }

        // GET: api/Sarki/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<bool> PostTest(string s)
        {
            return true;
        }



        [HttpPost]
        public async Task<bool> Upload(Sarki sarki)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            
            try
            {
                await cn.OpenAsync();
                SqlTransaction tr = cn.BeginTransaction(IsolationLevel.ReadUncommitted);
                SqlCommand cmdSarki = new SqlCommand("", cn, tr);                                
                cmdSarki.CommandText = 
                    "insert into Genel..Sarki with (rowlock) " + Environment.NewLine +
                    "(Ad,AkorFontBuyuklugu,SozFontBuyuklugu,SozRtf,SpotifyAdresi) " + Environment.NewLine + 
                    "values " + Environment.NewLine + 
                    "(@Ad,@AkorFontBuyuklugu,@SozFontBuyuklugu,@SozRtf,@SpotifyAdresi) select scope_identity()";
                cmdSarki.Parameters.Add("@Id", SqlDbType.NVarChar).Value = sarki.Id;
                cmdSarki.Parameters.Add("@Ad", SqlDbType.NVarChar).Value = sarki.Ad;
                cmdSarki.Parameters.Add("@AkorFontBuyuklugu", SqlDbType.Int).Value = sarki.AkorFontBuyuklugu;
                cmdSarki.Parameters.Add("@SozFontBuyuklugu", SqlDbType.Int).Value = sarki.SozFontBuyuklugu;
                cmdSarki.Parameters.Add("@SozRtf", SqlDbType.NVarChar).Value = sarki.SozRtf;
                cmdSarki.Parameters.Add("@SpotifyAdresi", SqlDbType.VarChar).Value = sarki.SpotifyAdresi;
                
                if (sarki.Id > 0)
                {
                    cmdSarki.CommandText =
                        "update Genel..Sarki with (rowlock) " + Environment.NewLine +
                        "set Ad=@Ad,AkorFontBuyuklugu=@AkorFontBuyuklugu,SozFontBuyuklugu=@SozFontBuyuklugu,SozRtf=@SozRtf,SpotifyAdresi=@SpotifyAdresi) " + Environment.NewLine +
                        "where Id=@Id";
                    await cmdSarki.ExecuteNonQueryAsync();

                    SqlCommand cmdSatirDelete = new SqlCommand("", cn, tr);                    
                    cmdSatirDelete.CommandText = "delete from Genel..Satir with (rowlock) where SarkiId=@SarkiId";
                    cmdSatirDelete.Parameters.Add("@SarkiId", SqlDbType.NVarChar).Value = sarki.Id;
                    await cmdSatirDelete.ExecuteNonQueryAsync();
                }
                else
                {
                    object id = await cmdSarki.ExecuteScalarAsync();
                    sarki.Id = (int)(decimal)id;
                }                

                SqlCommand cmdSatirInsert = new SqlCommand("", cn, tr);
                cmdSatirInsert.CommandText =
                    "insert into Genel..Satir with (rowlock) " + Environment.NewLine +
                    "(SarkiId,Metin,Sira,AkorSatiri) " + Environment.NewLine +
                    "values " + Environment.NewLine +
                    "(@SarkiId,@Metin,@Sira,@AkorSatiri)";
                cmdSatirInsert.Parameters.Add("@SarkiId", SqlDbType.NVarChar).Value = sarki.Id;
                cmdSatirInsert.Parameters.Add("@Metin", SqlDbType.NVarChar);
                cmdSatirInsert.Parameters.Add("@Sira", SqlDbType.Int);
                cmdSatirInsert.Parameters.Add("@AkorSatiri", SqlDbType.Bit);                

                foreach(Satir s in sarki.Satirlar)
                {
                    cmdSatirInsert.Parameters["@Metin"].Value = s.Metin;
                    cmdSatirInsert.Parameters["@Sira"].Value = s.Sira;
                    cmdSatirInsert.Parameters["@AkorSatiri"].Value = s.AkorSatiri;                    
                    await cmdSatirInsert.ExecuteNonQueryAsync(); 
                }

                await tr.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await cn.CloseAsync(); 
            }            
        }

        private async Task<DataSet> DataSetOku(string komut, string table)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            DataSet ds = new DataSet();
            try
            {
                await cn.OpenAsync();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand(komut, cn);
                da.Fill(ds, table);                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                cn.Close();
            }
            return ds;
        }
    }
}
