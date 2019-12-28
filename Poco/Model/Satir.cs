using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poco.Model
{
    public class Satir
    {
        public int SarkiId { get; set; }
        public string Metin { get; set; } = "";
        public int Sira { get; set; } = 0;
        public bool AkorSatiri { get; set; } = false;

        public string Renk
        {
            get
            {
                if (AkorSatiri)
                {
                    return "Red";
                }
                else
                {
                    return "Black";
                }
            }
            set { }
        }

        //public Color Renk 
        //{
        //    get
        //    {
        //        if(AkorSatiri)
        //        {
        //            return Color.Red;
        //        }
        //        else
        //        {
        //            return Color.Black;
        //        }
        //    }
        //}
    }
}
