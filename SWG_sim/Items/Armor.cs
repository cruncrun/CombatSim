using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    class Armor
    {
        public int DefencePoints { get; set; }
        public int InitiativeModifier { get; set; }

        public Armor()
        {
            Utils utils = new Utils();
            InitiativeModifier = utils.RandomNumber(-10, 11);
            DefencePoints = InitiativeModifier * 2;
        }
    }
}
