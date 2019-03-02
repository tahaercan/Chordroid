using Chordroid.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Chordroid
{
    public class DisplayCarousel :CarouselPage
    {
        public DisplayCarousel(Sarki s)
        {
            Children.Add(new AkorDisplay(s));
            Children.Add(new AkorLyrics(s));
        }
    }
}
