using System.Collections.ObjectModel;

namespace Chordroid.Model
{
    public class Sarki
    {
        public Sarki()
        {
            Satirlar = new ObservableCollection<Satir>();
            AkorFontBuyuklugu = 20;
            SozFontBuyuklugu = 14;
            SozRtf = "";
        }

        public string Ad { get; set; }
        public int AkorFontBuyuklugu { get; set; }
        public int SozFontBuyuklugu { get; set; }
        public string SozRtf { get; set; }
        public ObservableCollection<Satir> Satirlar { get; set; }

    }
}
