using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame.Contexts
{
    interface IUpdateContext
    {
        void AddObject(GameObject gameObject);
        IEnumerable<T> FindObject<T>()
            where T : GameObject;
        void DeleteObject(GameObject gameObject);
        double DelayTime { get; }
    }
}
