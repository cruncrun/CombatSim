using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    public class CombatSim_Enum
    {
		public enum ActionType
        {
            SingleTargetAttack,
            MultiTargetAttack,
            SingleTargetHealing,
            MultiTargetHealing
        }   
    }
}
