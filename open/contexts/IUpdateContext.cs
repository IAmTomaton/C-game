using OpenTK.Input;
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
        void AddGlobalObject(GameObject gameObject);
        IEnumerable<T> FindObject<T>() where T : GameObject;
        IEnumerable<T> FindGlobalObject<T>() where T : GameObject;
        void DeleteObject(GameObject gameObject);
        void DeleteGlobalObject(GameObject gameObject);
        bool ObjectExistence(GameObject gameObject);
        bool GlobalObjectExistence(GameObject gameObject);
        void BindGameObjectToCamera(GameObject gameObject);
        float DelayTime { get; }
        KeyboardState Keyboard { get; }
        MouseState Mouse { get; }
    }
}
