using Cgame.Contexts;
using Cgame.objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    class ConsoleListener
    {
        //should we use events in programm???
        public ConsoleListener()
        {
        }

        public void Update(IUpdateContext updateContext)
        {
            while (Console.KeyAvailable)
            {
                var command = Console.ReadLine();
                Process(command, updateContext);
                //Console.WriteLine(updateContext.FindLocalObject<Obstacle>().Count());
            }
            
        }

        void Process(string command, IUpdateContext uc)
        {
            var commandParts = command.Split();
            List<GameObject> listToAdd;
            switch (commandParts[0])
            {
                case "add":

            }
            if ( == "add")
                
           else listToAdd = uc.objectsToDelete;
        }
    }
}
