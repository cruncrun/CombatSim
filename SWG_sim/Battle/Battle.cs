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
    class Battle
    {        
        public List<Character> Participants { get; } = new List<Character>();
        public Character Attacker { get; set; }
        public Character Defender { get; set; }
        public bool FightContinues { get; set; }        
        
        public Battle()
        {

        }       

        public void Fight(Battle battle)
        {
            Prepare(battle);
            BattleReportHeader(Participants);
            BattleReport(battle);            
            ConsoleWriter.BattleSummary(Participants);
        }

        private void BattleReportHeader(List<Character> participants)
        {
            foreach (var character in participants)
            {
                ConsoleWriter.ParticipantDetails(character);                              
            }
            System.Console.WriteLine("\r\n");
        }
    
        private void BattleReport(Battle battle)
        {
            for (int turnIterator = 1; turnIterator <= 10 && AreThereAnyParticipantsLeft(Participants); turnIterator++)
            {
                List<Character> aliveParticipants = GetAliveParticipants(Participants);

                ConsoleWriter.TurnNumberMessage(turnIterator);                

                for (int i = 1; AreThereAnyAttacksLeft(aliveParticipants) && AreThereAnyParticipantsLeft(aliveParticipants); i++)
                {                    
                    List<Character> attackingParticipants = InitiativeCheck(aliveParticipants, i);
                    if (attackingParticipants.Count != 0)
                    {
                        foreach (var character in attackingParticipants)
                        {
                            if (character.IsAlive)
                            {
                                Attack attack = new Attack(character, aliveParticipants);
                                attack.PerformAttack(attack);
                                //Thread.Sleep(300);
                            }                             
                        }
                    }
                    else
                    {
                        
                    }
                }                
                TurnReset(Participants);
                System.Console.WriteLine("\r\n");
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

        private void Prepare(Battle battle) // metoda tymczasowo stosowana do debuga
        {
            Utils utils = new Utils();

            int numberOfAttackers = 5;
            int numberOfDefenders = 5;

            for (int i = 0; i < numberOfAttackers; i++)
            {
                Participants.Add(new Character("Atakujący " + i, true));
            }
            
            /*
            for (int i = 0; i < numberOfDefenders; i++)
            {
                Participants.Add(new Character("Obrońca " + i, false));
            } 
            */
            

            
            // BOSS

            Character bossAss = new Character("Pomniejszy demon",                     // Name
                                            utils.RandomNumber(150, 225),           // HitPoints
                                            1,                                       // ManaPoints                                            
                                            utils.RandomNumber(12, 16),                                       // Strength
                                            utils.RandomNumber(20, 30),                                       // Dexterity
                                            utils.RandomNumber(1, 6),                                     // Toughness
                                            utils.RandomNumber(10, 15),                                        // Initiative
                                            new Weapon(utils.RandomNumber(8, 10),                           // BaseAttackPower
                                                       utils.RandomNumber(2, 3),                            // DiceSides
                                                       utils.RandomNumber(3, 8),                            // DiceRolls
                                                       utils.RandomNumber(3, 6),                             // AttacksPerTurn
                                                       utils.RandomNumber(5, 10)),                           // CriticalChance
                                            true,                                     // isAlive
                                            false);                                   // isAttacker
            Participants.Add(bossAss);

            Character bossAss2 = new Character("Piekielny golem",                     // Name
                                           utils.RandomNumber(500, 700),           // HitPoints
                                           1,                                       // ManaPoints                                          
                                           utils.RandomNumber(40, 50),                                       // Strength
                                           utils.RandomNumber(5, 8),                                       // Dexterity
                                           utils.RandomNumber(20, 30),                                       // Toughness
                                           utils.RandomNumber(25, 45),                                        // Initiative
                                           new Weapon(utils.RandomNumber(10, 16),                           // BaseAttackPower
                                                      utils.RandomNumber(2, 3),                            // DiceSides
                                                      utils.RandomNumber(10, 12),                            // DiceRolls
                                                      utils.RandomNumber(1, 3),                             // AttacksPerTurn
                                                      utils.RandomNumber(2, 6)),                           // CriticalChance
                                           true,                                     // isAlive
                                           false);                                   // isAttacker
            Participants.Add(bossAss2);

            Character boss = new Character("Michał, Lord Ciemności",        // Name
                                            utils.RandomNumber(400, 600),           // HitPoints
                                            1,                                       // ManaPoints                                            
                                            utils.RandomNumber(20, 30),                                       // Strength
                                            utils.RandomNumber(15, 20),                                       // Dexterity
                                            utils.RandomNumber(10, 20),                                       // Toughness
                                            utils.RandomNumber(15, 25),                                        // Initiative
                                            new Weapon(utils.RandomNumber(18, 25),                           // BaseAttackPower
                                                       utils.RandomNumber(2, 4),                            // DiceSides
                                                       utils.RandomNumber(10, 16),                            // DiceRolls
                                                       utils.RandomNumber(4, 6),                             // AttacksPerTurn
                                                       utils.RandomNumber(8, 15)),                           // CriticalChance
                                            true,                  // isAlive
                                            false);                // isAttacker
            Participants.Add(boss);
            

        }
    }
}
