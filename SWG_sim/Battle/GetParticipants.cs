using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    static class BossGenerator
    {
        static public Character GenerateBoss(int type)
        {
            Utils utils = new Utils();
            switch (type)
            {
                case 1: // Main Boss
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
                                            new Armor(),
                                            true,                  // isAlive
                                            false);                // isAttacker
                    return boss;

                case 2: //Damage Sponge
                    Character bossDamageSponge = new Character("Piekielny golem",                     // Name
                                          utils.RandomNumber(500, 700),           // HitPoints
                                          1,                                       // ManaPoints                                          
                                          utils.RandomNumber(40, 50),                                       // Strength
                                          utils.RandomNumber(5, 8),                                       // Dexterity
                                          utils.RandomNumber(40, 50),                                       // Toughness
                                          utils.RandomNumber(25, 45),                                        // Initiative
                                          new Weapon(utils.RandomNumber(10, 16),                           // BaseAttackPower
                                                     utils.RandomNumber(2, 3),                            // DiceSides
                                                     utils.RandomNumber(10, 12),                            // DiceRolls
                                                     utils.RandomNumber(1, 3),                             // AttacksPerTurn
                                                     utils.RandomNumber(2, 6)),                           // CriticalChance
                                          new Armor(),
                                          true,                                     // isAlive
                                          false);                                   // isAttacker
                    return bossDamageSponge;

                case 3: //Lesser Boss
                    Character bossLesser = new Character("Pomniejszy demon",                     // Name
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
                                            new Armor(),
                                            true,                                     // isAlive
                                            false);                                   // isAttacker
                    return bossLesser;

                default:
                    Character bossRandom = new Character("Esencja zniszczenia i śmierci",                     // Name
                                            utils.RandomNumber(15, 2250),           // HitPoints
                                            1,                                       // ManaPoints                                            
                                            utils.RandomNumber(1, 50),                                       // Strength
                                            utils.RandomNumber(1, 50),                                       // Dexterity
                                            utils.RandomNumber(1, 50),                                     // Toughness
                                            utils.RandomNumber(1, 50),                                        // Initiative
                                            new Weapon(utils.RandomNumber(1, 50),                           // BaseAttackPower
                                                       utils.RandomNumber(1, 50),                           // DiceSides
                                                       utils.RandomNumber(1, 50),                           // DiceRolls
                                                       utils.RandomNumber(1, 50),                            // AttacksPerTurn
                                                       utils.RandomNumber(1, 50)),                            // CriticalChance
                                            new Armor(),
                                            true,                                     // isAlive
                                            false);                                   // isAttacker
                    return bossRandom;
            }
        }

        static public Character GenerateCharacter(bool isAttacker, int i)
        {
            switch (isAttacker)
            {
                case true:
                    return new Character("Atakujący " + i, true);
                case false:
                    return new Character("Obrońca " + i, false);
                default:                    
            }
            
        }
    }
}
