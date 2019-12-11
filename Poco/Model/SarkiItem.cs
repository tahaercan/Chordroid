using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poco.Model
{
    public class SarkiItem
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public bool Secili { get; set; } = false;
        public string Link { get; set; } = "";
        public SarkiItem()
        {

        }
    }
}
