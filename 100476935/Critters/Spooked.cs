using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace _100476935
{
    public class Spooked : BaseCritter
    {
        public Spooked(string name)
        {
            Name = name;
            behavior.Add("Bomb", "Avoid");
            behavior.Add("Food", "DoNothing");
            behavior.Add("Gift", "DoNothing");
            behavior.Add("Exit", "GoTo");
            behavior.Add("Terrain", "Avoid");
        }
        public override void LaunchUI()
        {
            SpookedSettings settings = new SpookedSettings(this);
            settings.Show();
            settings.Focus();
        }
    }

}
