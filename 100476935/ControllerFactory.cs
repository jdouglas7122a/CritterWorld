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

            controllers.Add(new HungryCompas("HungryCompas_1"));
            controllers.Add(new ScaredCompas("ScaredCompas_1"));
            controllers.Add(new PopcornCompas("PopcornCompas_1"));

            for (int i = 0; i != 25; i++)
            {
                controllers.Add(new Wanderer("basic "+ i + 1));

            }



            return controllers.ToArray();
        }
    }
}
