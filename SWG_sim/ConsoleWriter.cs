using System.Collections.Generic;
using System.Drawing;
using Console = Colorful.Console;
using Colorful;
using System;

namespace SWG_sim
{
    public class ConsoleWriter
    {
        public void GenerateBattleReport(Battle battle)
        {
            PrintParticipantDetails(battle.Participants);
            PrintTurnDetails(battle.Turns);
            PrintBattleResult(battle.BattleResult);
            PrintBattleSummary(battle.Participants);            
        }

        private void PrintBattleResult(Battle.BattleOutcome battleResult)
        {
            switch (battleResult)
            {
                case Battle.BattleOutcome.Draw:
                    Console.WriteLine("Walka zakończyła się remisem.", Color.Beige);
                    break;
                case Battle.BattleOutcome.AttackersWin:
                    Console.WriteLine("Walka zakończyła się zwycięstwem napastników.", Color.Beige);
                    break;
                case Battle.BattleOutcome.DefendersWin:
                    Console.WriteLine("Walka zakończyła się skuteczną obroną.", Color.Beige);
                    break;
                default:
                    break;
            }
            System.Console.WriteLine("\r\n");
        }

        private void PrintTurnDetails(List<Turn> turns)
        {
            foreach (Turn turn in turns)
            {
                TurnNumberMessage(turn.TurnIterator);
                TurnActionsMessage(turn.ActionList);
                System.Console.WriteLine("\r\n");
            }
        }

        private void TurnActionsMessage(List<Action> actionList)
        {
            foreach (Action action in actionList)
            {
                switch (action.ActionTypeId)
                {
                    case Action.ActionType.SingleTargetAttack:
                        if (action.AccurateHitCheck.IsAccurate)
                        {
                            if (action.CriticalHitCheck.IsCritical)
                            {
                                CriticalHitMessage(action);
                            }
                            else
                            {
                                RegularHitMesage(action);
                            }
                            if (!action.Target_EOTValues.IsAlive)
                            {
                                DeathMessage(action.Target.Name);
                            }
                        }
                        else
                        {
                            MissedAttackMessage(action);
                        }
                        break;
                    case Action.ActionType.MultiTargetAttack:
                        break;
                    case Action.ActionType.SingleTargetHealing:
                        RegularHealingMessage(action);
                        break;
                    case Action.ActionType.MultiTargetHealing:
                        break;
                    default:
                        break;
                }
            }
        }

        private void RegularHealingMessage(Action action)
        {
            if (action.Character != action.Target)
            {
                string baseText = "{0} leczy leśną magią {1} punktów życia. {2} ma {3}/{4} punktów życia.";
                Formatter[] elements = new Formatter[]
                {
                new Formatter(action.Character.Name, GetCharacterNameColor(action.Character)),
                new Formatter(action.HealingAmount, Color.Green),
                new Formatter(action.Target.Name, GetCharacterNameColor(action.Target)),
                new Formatter(action.Target_EOTValues.RemainingHitPoints, Color.White),
                new Formatter(action.Target.HitPoints, Color.White)
                };
                Console.WriteLineFormatted(baseText, Color.LightGreen, elements);
            }
            else if (action.Character == action.Target)
            {
                string baseText = "{0} leczy siebie samego, odzyskując {1} punktów życia. {2} ma {3}/{4} punktów życia.";
                Formatter[] elements = new Formatter[]
                {
                new Formatter(action.Character.Name, GetCharacterNameColor(action.Character)),
                new Formatter(action.HealingAmount, Color.Green),
                new Formatter(action.Target.Name, GetCharacterNameColor(action.Target)),
                new Formatter(action.Target_EOTValues.RemainingHitPoints, Color.White),
                new Formatter(action.Target.HitPoints, Color.White)
                };
                Console.WriteLineFormatted(baseText, Color.LightGreen, elements);
            }            
        }

        private void CriticalHealingMessage(Action action)
        {
            throw new NotImplementedException();
        }

        private void PrintParticipantDetails(List<Character> participants)
        {
            foreach (Character character in participants)
            {
                ParticipantDetails(character);
            }
        }

        private void DeathMessage(string name)
        {
            string baseText = name + " umiera na śmierć.";
            Console.WriteLine(baseText, Color.Red);
        }

        private void TurnNumberMessage(int turnIterator)
        {
            string baseText = "Tura " + turnIterator;
            Console.WriteLine(baseText, Color.LawnGreen);
        }

        private void ParticipantDetails(Character character)
        {
            if (character.HealingChance == 100)
            {
                character.Name += "+";
            }
            // Line 1
            string baseTextFirstLine = "{0} \tI: {1}\tSTR: {2}\tDEX: {3}\tTGH: {4}\tDEF: {5}";
            Formatter[] elementsFirstLine = new Formatter[]
            {
                new Formatter(character.Name, GetCharacterNameColor(character)),
                new Formatter(character.Initiative, Color.White),
                new Formatter(character.Strength, Color.White),
                new Formatter(character.Dexterity, Color.White),
                new Formatter(character.Toughness, Color.White),
                new Formatter(character.DefencePoints, Color.White)
            };
            Console.WriteLineFormatted(baseTextFirstLine, Color.LightGray, elementsFirstLine);
            // Line 2
            string baseTextSecondLine = "Broń:\t{0}-{1} * {2}\tPancerz\tDEF: {3}\tI: {4}";
            Formatter[] elementsSecondLine = new Formatter[]
            {
                new Formatter(character.Weapon.MinimumDamage, Color.White),
                new Formatter(character.Weapon.MaximumDamage, Color.White),
                new Formatter(character.Weapon.AttacksPerTurn, Color.White),
                new Formatter(character.Armor.DefencePoints, Color.White),
                new Formatter(character.Armor.InitiativeModifier, Color.White)
            };
            Console.WriteLineFormatted(baseTextSecondLine, Color.LightGray, elementsSecondLine);
        }

        private void RegularHitMesage(Action action)
        {
            string baseText = "{0} zadaje atakiem {1} punktów obrażeń. {2} ma {3}/{4} punktów życia.";
            Formatter[] elements = new Formatter[]
            {
                new Formatter(action.Character.Name, GetCharacterNameColor(action.Character)),
                new Formatter(action.DamageAmount, Color.White),
                new Formatter(action.Target.Name, GetCharacterNameColor(action.Target)),
                new Formatter(action.Target_EOTValues.RemainingHitPoints, Color.White),
                new Formatter(action.Target.HitPoints, Color.White)
            };
            Console.WriteLineFormatted(baseText, Color.LightGray, elements);
        }

        private void CriticalHitMessage(Action action)
        {
            string baseText = "{0} trafia krytycznie zadając {1} punktów obrażeń. {2} ma {3}/{4} punktów życia.";
            Formatter[] elements = new Formatter[]
            {
                new Formatter(action.Character.Name, GetCharacterNameColor(action.Character)),
                new Formatter(action.DamageAmount, Color.DeepPink),
                new Formatter(action.Target.Name, GetCharacterNameColor(action.Target)),
                new Formatter(action.Target_EOTValues.RemainingHitPoints, Color.White),
                new Formatter(action.Target.HitPoints, Color.White)
            };
            Console.WriteLineFormatted(baseText, Color.HotPink, elements);
        }

        private void MissedAttackMessage(Action action)
        {
            string baseText = "{0} próbuje zaatakować {1}, ale nie trafia...";
            Formatter[] elements = new Formatter[]
            {
                new Formatter(action.Character.Name, GetCharacterNameColor(action.Character)),
                new Formatter(action.Target.Name, GetCharacterNameColor(action.Target))
            };
            Console.WriteLineFormatted(baseText, Color.DimGray, elements);
        }

        private void PrintBattleSummary(List<Character> participants)
        {
            foreach (var character in participants)
            {
                string baseText = "{0} \t{1}/{2}\tZabitych wrogów: {3}. DD: {4}, DT: {5}, HD: {6}, HT: {7}";
                Formatter[] elements = new Formatter[]
                {
                    new Formatter(character.Name, GetCharacterNameColor(character)),
                    new Formatter(character.RemainingHitPoints, Color.LightGray),
                    new Formatter(character.HitPoints, Color.LightGray),
                    new Formatter(character.KillCount, Color.LightGray),
                    new Formatter(character.DamageDone, Color.LightGray),
                    new Formatter(character.DamageTaken, Color.LightGray),
                    new Formatter(character.HealingDone, Color.LightGray),
                    new Formatter(character.HealingTaken, Color.LightGray)
                };
                Console.WriteLineFormatted(baseText, Color.LightGray, elements);
            }
        }

        private Color GetCharacterNameColor(Character character)
        {
            return character.IsAttacker ? Color.Tomato : Color.SteelBlue;
        }
    }
}
