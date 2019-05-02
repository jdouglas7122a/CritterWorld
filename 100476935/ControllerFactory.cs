using CritterController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _100476935
{
    class ControllerFactory : ICritterControllerFactory
    {
        public string Author => "Josh Douglas 100476935";

        public ICritterController[] GetCritterControllers()
        {
            List<ICritterController> controllers = new List<ICritterController>();

            controllers.Add(new Tequila("Tequila_1"));
            controllers.Add(new Spooked("Spooked_1"));
            controllers.Add(new Popcorn("Popcorn_1"));

            return controllers.ToArray();
        }
    }
}
