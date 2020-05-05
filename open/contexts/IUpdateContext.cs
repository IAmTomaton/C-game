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
        void AddLocalObject(GameObject gameObject);
        void AddGlobalObject(GameObject gameObject);
        IEnumerable<T> FindLocalObject<T>() where T : GameObject;
        IEnumerable<T> FindGlobalObject<T>() where T : GameObject;
        void DeleteLocalObject(GameObject gameObject);
        void DeleteGlobalObject(GameObject gameObject);
        bool LocalObjectExistence(GameObject gameObject);
        bool GlobalObjectExistence(GameObject gameObject);
        void BindGameObjectToCamera(GameObject gameObject);
        float DelayTime { get; }
        KeyboardState Keyboard { get; }
        MouseState Mouse { get; }
    }
}
