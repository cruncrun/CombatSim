using System;
using System.Collections.Generic;
using System.Linq;
using Console = Colorful.Console;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace SWG_sim
{
    public class Battle
    {
        #region Properties
        public List<Character> Participants { get; } = new List<Character>();
        public List<Turn> Turns { get; } = new List<Turn>();
        public BattleOutcome BattleResult { get; set; }
        #endregion

        #region Enum
        public enum BattleOutcome
	    {
            Draw,
            AttackersWin,
            DefendersWin
	    }
        #endregion

        #region Constructors
        public Battle()
        {

        }

        public Battle(List<Character> participants)
        {
            Participants = participants;
        }
        #endregion

        #region Public members
        public void GenerateBattleReport()
        {            
            BattleReport();
        }
        #endregion

        #region Private members
        private void BattleReport()
        {
            for (int turnIterator = 1; turnIterator <= 12 && AreThereAnyParticipantsLeft(Participants); turnIterator++)
            {
                List<Character> aliveParticipants = GetAliveParticipants(Participants);

                Turn turn = new Turn(turnIterator, aliveParticipants);                              

                for (int i = 1; AreThereAnyAttacksLeft(aliveParticipants) && AreThereAnyParticipantsLeft(aliveParticipants); i++)
                {                    
                    List<Character> attackingParticipants = InitiativeCheck(aliveParticipants, i);
                    if (attackingParticipants.Count != 0)
                    {
                        PerformAllActions(aliveParticipants, turn, attackingParticipants);
                    }
                }  
                Turns.Add(turn);
                TurnReset(Participants);                
            }
            BattleResultCheck(GetAliveParticipants(Participants));
        }

        private void BattleResultCheck(List<Character> participants)
        {
            if (!AreThereAnyDefendersLeft(participants))
                BattleResult = BattleOutcome.AttackersWin;
            else if (!AreThereAnyAttackersLeft(participants))
                BattleResult = BattleOutcome.DefendersWin;
            else BattleResult = BattleOutcome.Draw;
        }

        private static void PerformAllActions(List<Character> aliveParticipants, Turn turn, List<Character> attackingParticipants)
        {
            foreach (var character in attackingParticipants)
            {
                if (character.IsAlive)
                {
                    Action action = new Action(character, aliveParticipants);
                    action.ActionTypeId = action.GetActionTypeId(character);
                    action.PerformAction(action);
                    turn.ActionList.Add(action);
                }
            }
        }

        private List<Character> GetAliveParticipants(List<Character> participants)
        {
            List<Character> aliveParticipants = new List<Character>();
            foreach (var character in participants)
            {
                if (character.IsAlive)
                {
                    aliveParticipants.Add(character);
                }
            }
            return aliveParticipants;
        } 

        private void TurnReset(List<Character> participants)
        {
            foreach (var character in participants)
            {
                character.Weapon.RemainingAttacks = character.Weapon.AttacksPerTurn;
                character.CummulativeInitiative = character.Initiative;
            }
        }  

        private bool AreThereAnyAttacksLeft(List<Character> participants)
        {
            bool areThereAnyAttacksLeft = false;
            foreach (var character in participants)
            {
                if (character.Weapon.RemainingAttacks != 0 && character.IsAlive)
                {
                    areThereAnyAttacksLeft = true;
                }
            }
            return areThereAnyAttacksLeft;
        }

        private bool AreThereAnyParticipantsLeft(List<Character> participants)
        {
            bool areThereAnyAttackersLeft = AreThereAnyAttackersLeft(participants);
            bool areThereAnyDefendersLeft = AreThereAnyDefendersLeft(participants);            

            return areThereAnyAttackersLeft && areThereAnyDefendersLeft ? true : false;
        }

        private bool AreThereAnyAttackersLeft(List<Character> participants)
        {
            bool areThereAnyAttackersLeft = false;
            foreach (var character in participants)
            {
                if (character.IsAlive)
                {
                    if (character.IsAttacker)
                    {
                        areThereAnyAttackersLeft = true;
                    }
                }
            }
            return areThereAnyAttackersLeft;
        }

        private bool AreThereAnyDefendersLeft(List<Character> participants)
        {
            bool areThereAnyDefendersLeft = false;
            foreach (var character in participants)
            {
                if (character.IsAlive)
                {
                    if (!character.IsAttacker)
                    {
                        areThereAnyDefendersLeft = true;
                    }
                }
            }
            return areThereAnyDefendersLeft;
        }

        private bool IsCharacterAttacking(Character participant)
        {
            return (participant.IsAttacker) ? true : false;
        }

        private bool IsCharacterDefending(Character participant)
        {
            return (!participant.IsAttacker) ? true : false;
        }

        private List<Character> InitiativeCheck(List<Character> participants, int initiative)
        {
            List<Character> attackingParticipants = new List<Character>();

            foreach (var character in participants)
            {
                if ((character.Weapon.RemainingAttacks > 0) && (character.CummulativeInitiative == initiative) && character.IsAlive)
                {
                    attackingParticipants.Add(character);
                }
            }

            return attackingParticipants;
        }

        #endregion
    }
}
