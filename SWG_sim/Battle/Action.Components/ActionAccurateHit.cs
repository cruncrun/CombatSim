using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{ 
    public class ActionAccurateHit
    {
        public bool IsAccurate { get; set; }
        public int BaseHitChance { get; set; }
        public int AttackersDexterity { get; set; }
        public int DefendersDexterity { get; set; }
        public int HitModifiersSum { get; set; }

        public ActionAccurateHit(int attackersDex, int defendersDex)
        {
            BaseHitChance = 90;
            AttackersDexterity = attackersDex;
            DefendersDexterity = defendersDex;
            HitModifiersSum = AttackersDexterity = DefendersDexterity;
            IsAccurate = CheckForHit();
        }

        private bool CheckForHit()
        {
            Utils utils = new Utils();
            if (utils.RandomNumber(100) <= 90 + HitModifiersSum)
            {
                return true;
            }
            else return false;
        }
    }
}
