using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poco.Model
{
    public class Sarki
    {
        public Sarki()
        {
            Satirlar = new ObservableCollection<Satir>();
            AkorFontBuyuklugu = 20;
            SozFontBuyuklugu = 14;
            SozRtf = "";
            SpotifyAdresi = "";
        }

        public int Id { get; set; } = 0;
        public string Ad { get; set; }
        public int AkorFontBuyuklugu { get; set; }
        public int SozFontBuyuklugu { get; set; }
        public string SozRtf { get; set; }
        public string SpotifyAdresi { get; set; } = "";

        public ICollection<Satir> Satirlar { get; set; }
    }
}
