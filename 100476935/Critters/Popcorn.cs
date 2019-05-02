using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace _100476935
{
    public class Popcorn : BaseCritter
    {
        public Popcorn(string name)
        {
            Name = name;
            behavior.Add("Bomb", "Avoid");
            behavior.Add("Food", "GoTo");
            behavior.Add("Gift", "Avoid");
            behavior.Add("Exit", "GoTo");
            behavior.Add("Terrain", "Avoid");
        }
        public override void LaunchUI()
        {
            PopcornSettings settings = new PopcornSettings(this);
            settings.Show();
            settings.Focus();
        }
    }

}
