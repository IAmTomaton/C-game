using Cgame.Contexts;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cgame.Objects
{
    class TestGameObject : IShootable
    {
        private readonly float cooldown = 1;
        private float timer;
        private bool c;

        public Sprite Sprite { get; set; }
        public float Mass { get; set; }
        public Layers Layer { get; set; }
        public Collider Collider { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        public TestGameObject() : base()
        {
            Sprite = new Sprite(this, "base");
            Layer = Layers.Object;
            Collider = new Collider(this, 64, 64);
            Mass = 1;
        }

        public void Start(IUpdateContext updateContext)
        {
            //base.Start(updateContext);
        }

        public void Update(IUpdateContext updateContext)
        {
            //base.Update(updateContext);
            timer -= updateContext.DelayTime;
        }

        public void Collision(IUpdateContext updateContext, GameObject other)
        {
            if (timer <= 0 && other is TestGameObjectWithCamera)
            {
                if (c)
                    Console.WriteLine("АТАШЁЛ!!!!");
                else
                    Console.WriteLine("ШтобТиСдох!!!!!");
                c = !c;
                timer = cooldown;
            }
            //base.Collision(updateContext, other);
        }
    }
}
