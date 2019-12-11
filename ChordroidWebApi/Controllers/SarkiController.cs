using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Poco.Model;

namespace ChordroidWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SarkiController : ControllerBase
    {
        // GET: api/Sarki
        [HttpGet]
        public IEnumerable<Sarki> Get()
        {
            List<Sarki> l = new List<Sarki>();
            SqlConnection cn = new SqlConnection("Server=94.73.146.4,1433\\MSSQL49;Initial Catalog=Genel; User Id=taha;Password=KUnl83G3BNri44M;");
            DataSet ds = new DataSet();
            try
            {
                cn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand("select Id,Ad from Genel..Sarki with (nolock)", cn);
                da.Fill(ds, "SarkiListesi");
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
                string s = ex.Message;
            }
            finally
            {
                cn.Close();
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
    }
}
