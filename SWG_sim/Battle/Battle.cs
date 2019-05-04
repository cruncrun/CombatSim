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
        #endregion

        #region Constructors
        public Battle()
        {

        }
        #endregion

        #region Public members
        public void Fight(Battle battle)
        {
            Prepare();
            BattleReport();

            ConsoleWriter cw = new ConsoleWriter();
            cw.GenerateBattleReport(battle);            
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
            bool areThereAnyAttackersLeft = false;
            bool areThereAnyDefendersLeft = false;
            
            foreach (var character in participants)
            {
                if (character.IsAlive)
                {
                    if (character.IsAttacker)
                    {
                        areThereAnyAttackersLeft = true;
                    }
                    if (!character.IsAttacker)
                    {
                        areThereAnyDefendersLeft = true;
                    }
                }
            }

            return areThereAnyAttackersLeft && areThereAnyDefendersLeft ? true : false;
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

        private void Prepare() // metoda tymczasowo stosowana do debuga
        {   
            int numberOfAttackers = 20;
            int numberOfDefenders = 10;

            for (int i = 0; i < numberOfAttackers; i++)
            {
                Participants.Add(BossGenerator.GenerateCharacter(true, i));
            }

            
            //for (int i = 0; i < numberOfDefenders; i++)
            //{
            //    Participants.Add(BossGenerator.GenerateCharacter(false, i));
            //}
            
            
            // BOSS                       
            Participants.Add(BossGenerator.GenerateBoss(1));
            Participants.Add(BossGenerator.GenerateBoss(2));
            Participants.Add(BossGenerator.GenerateBoss(2));
            Participants.Add(BossGenerator.GenerateBoss(4));
            Participants.Add(BossGenerator.GenerateBoss(4));

        }
        #endregion
    }
}
