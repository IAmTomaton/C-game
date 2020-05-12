using Cgame.Contexts;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame.objects
{
    class Obstacle : IKilling, IShootable
    {
        public Sprite Sprite { get ; set ; }
        public float Mass { get ; set ; }
        public Layers Layer { get ; set ; }
        public Collider Collider { get ; set ; }
        public Vector3 Position { get ; set ; }
        public Vector2 Velocity { get ; set ; }
        public float Angle { get ; set ; }

        public Obstacle()
        {
            Sprite = new Sprite(this, "triangle");
            Collider = new Collider(this, 48, 48);
            Layer = Layers.Object;
            Mass = 1;
        }

        public Obstacle(Vector3 pos):this()
        {
            Position = pos;
        }

        public void Collision(IUpdateContext updateContext, GameObject other)
        {
        }

        public void Start(IUpdateContext updateContext)
        {
        }

        public void Update(IUpdateContext updateContext)
        {
        }
    }
}
