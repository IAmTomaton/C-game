using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Game(600, 600, "Igra ebat"))
            {
                game.Run(60.0);
            }
        }
    }
}
