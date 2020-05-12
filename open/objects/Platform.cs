using Cgame.Contexts;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame.objects
{
    class Platform:GameObject
    {
        public Sprite Sprite { get; set; }
        public float Mass { get; set; }
        public Layers Layer { get; set; }
        public Collider Collider { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        public Platform()
        {
            Sprite = new Sprite(this, "platform");
            Collider = new Collider(this, 64, 256);
            Layer = Layers.Object;
            Mass = 0;
        }

        public Platform(Vector3 pos) : this()
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
