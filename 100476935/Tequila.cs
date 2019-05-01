using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace _100476935
{
    public class Tequila : BaseCritter
    {
        public Tequila(string name)
        {
            Name = name;
            behavior.Add("Bomb", "Avoid"); // can be Avoid, GoTo or DoNothing
            behavior.Add("Food", "GoTo");
            behavior.Add("Gift", "Avoid");
            behavior.Add("Exit", "GoTo");
        }
        public override void LaunchUI()
        {
            TequilaSettings settings = new TequilaSettings(this);
            settings.Show();
            settings.Focus();
        }
    }

}
