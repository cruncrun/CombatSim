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
            PrintBattleSummary(battle.Participants);
        }

        private void PrintTurnDetails(List<Turn> turns)
        {
            foreach (Turn turn in turns)
            {
                TurnNumberMessage(turn.TurnIterator);
                TurnActionsMessage(turn.ActionList);
            }
        }

        private void TurnActionsMessage(List<Action> actionList)
        {
            foreach (Action action in actionList)
            {
                if (action.IsAccurate)
                {
                    if (action.IsCritical)
                    {
                        CriticalHitMessage(action);
                    }
                    else
                    {
                        RegularHitMesage(action);
                    }
                    if (!action.Opponent_EOTValues.IsAlive)
                    {
                        DeathMessage(action.Opponent.Name);
                    }
                }
                else
                {
                    MissedAttackMessage(action);
                }
            }
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
                new Formatter(action.Opponent.Name, GetCharacterNameColor(action.Opponent)),
                new Formatter(action.Opponent_EOTValues.RemainingHitPoints, Color.White),
                new Formatter(action.Opponent.HitPoints, Color.White)
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
                new Formatter(action.Opponent.Name, GetCharacterNameColor(action.Opponent)),
                new Formatter(action.Opponent_EOTValues.RemainingHitPoints, Color.White),
                new Formatter(action.Opponent.HitPoints, Color.White)
            };
            Console.WriteLineFormatted(baseText, Color.HotPink, elements);
        }

        private void MissedAttackMessage(Action action)
        {
            string baseText = "{0} próbuje zaatakować {1}, ale nie trafia...";
            Formatter[] elements = new Formatter[]
            {
                new Formatter(action.Character.Name, GetCharacterNameColor(action.Character)),
                new Formatter(action.Opponent.Name, GetCharacterNameColor(action.Opponent))
            };
            Console.WriteLineFormatted(baseText, Color.DimGray, elements);            
        }

        private void PrintBattleSummary(List<Character> participants)
        {
            foreach (var character in participants)
            {
                string baseText = "{0} \t{1}/{2}\tZabitych wrogów: {3}. \tZadane obrażenia: {4}, otrzymane obrażenia: {5}";
                Formatter[] elements = new Formatter[]
                {
                    new Formatter(character.Name, GetCharacterNameColor(character)),
                    new Formatter(character.RemainingHitPoints, Color.LightGray),
                    new Formatter(character.HitPoints, Color.LightGray),
                    new Formatter(character.KillCount, Color.LightGray),
                    new Formatter(character.DamageDone, Color.LightGray),
                    new Formatter(character.DamageTaken, Color.LightGray)
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
