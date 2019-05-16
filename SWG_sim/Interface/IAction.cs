using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SWG_sim.CombatSim_Enum;

namespace SWG_sim
{
    public interface IAction
    {
        #region Properties
        ActionType ActionTypeId { get; set; }
        Character Character { get; set; }
        List<Character> Target { get; set; }
        Character Target_EOTValues { get; set; }
        List<Character> AliveParticipants { get; set; }        
        bool IsAccurate { get; set; }
        bool IsCritical { get; set; }
        bool CleanupDone { get; set; }
        #endregion

        #region Methods
        void PerformAction(IAction action);
        List<Character> TargetSelection(IAction action);
        bool CheckForHit();
        bool CheckForCriticalHit();
        void GetWeaponDamage();
        void CalculateAmountDone();
        void ActionCleanup();
        #endregion
    }
}
