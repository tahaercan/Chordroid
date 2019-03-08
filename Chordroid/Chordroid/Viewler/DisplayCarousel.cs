using Chordroid.Model;
using Chordroid.View;
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
            Children.Add(new SongDetails(s));
            Children.Add(new AkorEdit(s));
        }
    }
}
