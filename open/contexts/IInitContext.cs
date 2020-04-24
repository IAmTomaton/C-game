using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame.Contexts
{
    interface IInitContext
    {
        void AddTexture(string name, string path);
    }
}
