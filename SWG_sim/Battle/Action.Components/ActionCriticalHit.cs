using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    public class ActionCriticalHit
    {
        public bool IsCritical { get; set; }
        public int AttackingWeaponCriticalChance { get; set; }
        public int DefendersArmorModifier { get; set; }
        public int EventModifier { get; set; }
        public int CriticalChanceSum { get; set; }

        public ActionCriticalHit(int weaponCriticalChance)
        {
            AttackingWeaponCriticalChance = weaponCriticalChance;
            DefendersArmorModifier = 0;
            EventModifier = 0;
            CriticalChanceSum = AttackingWeaponCriticalChance + DefendersArmorModifier + EventModifier;
            IsCritical = CheckForCriticalHit();
        }

        private bool CheckForCriticalHit()
        {
            Utils utils = new Utils();
            if (utils.RandomNumber(100) <= CriticalChanceSum)
            {
                return true;
            }
            else return false;
        }
    }
}

