using Cgame.Contexts;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    abstract class GameObject
    {
        public Sprite Sprite { get; protected set; }
        public float Mass { get; protected set; }
        public Layers Layer { get; protected set; } = Layers.Base;
        public Collider Collider { get; protected set; }
        public Vector3 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        public GameObject() { }

        virtual public void Start(IUpdateContext updateContext) { }

        virtual public void Update(IUpdateContext updateContext) { }

        virtual public void Collision(IUpdateContext updateContext, GameObject other) { }
    }
}
