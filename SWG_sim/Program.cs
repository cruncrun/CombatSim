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
            Battle battle = new Battle();
            battle.Fight(battle);
            Console.ReadKey();
        }
    }
}
