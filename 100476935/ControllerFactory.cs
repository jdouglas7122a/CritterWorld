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
        public string Author => "100476935";

        public ICritterController[] GetCritterControllers()
        {
            List<ICritterController> controllers = new List<ICritterController>();

            controllers.Add(new Compas("Compas" + (1)));
            controllers.Add(new CompasV2("CompasV2_" + 1));

            for (int i = 0; i < 23; i++)
            {
                controllers.Add(new Wanderer("Wanderer" + (i + 1)));
            }
            return controllers.ToArray();
        }
    }
}
