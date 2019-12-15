using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Poco.Model;

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
        public async Task<IEnumerable<Sarki>> Get()
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

        [HttpGet]
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
                    foreach (DataRow rSatir in dsSarki.Tables["Satir"].Select("SatirId=" + rSarki["Id"].ToString()))
                    {
                        Satir sa = new Satir();
                        sa.SarkiId = s.Id;
                        sa.AkorSatiri = (bool)rSatir["AkorSatiri"];
                        sa.Metin = rSatir["Metin"].ToString();
                        sa.Renk = sa.AkorSatiri == true ? Color.Red : Color.Black;
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

        // POST: api/Sarki
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Sarki/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
