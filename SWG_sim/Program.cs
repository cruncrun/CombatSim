using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWG_sim
{
    class Program
    {
        static void Main(string[] args)
        {            
            Character character = new Character();
            Battle battle = new Battle(character.GetCharacters(3));
            battle.GenerateBattleReport();

            ConsoleWriter cw = new ConsoleWriter();
            cw.GenerateBattleReport(battle); 

            Console.ReadKey();
        }
    }
}
