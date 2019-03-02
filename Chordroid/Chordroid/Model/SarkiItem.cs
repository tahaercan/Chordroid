using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Chordroid.Model
{
    public class SarkiItem 
    {

        public string Ad { get; set; }
        public bool Secili { get; set; } = false;
        public string Link { get; set; } = "";
        public SarkiItem()
        {

        }

    }
}