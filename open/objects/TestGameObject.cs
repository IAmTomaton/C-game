using Cgame.Contexts;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cgame.Objects
{
    class TestGameObject : GameObject
    {
        private readonly float cooldown = 1;
        private float timer;
        private bool c;

        public TestGameObject() : base()
        {
            Sprite = new Sprite(this, "base");
            Layer = Layers.Object;
            Collider = new Collider(this, 64, 64);
            Mass = 1;
        }

        public override void Start(IUpdateContext updateContext)
        {
            base.Start(updateContext);
        }

        public override void Update(IUpdateContext updateContext)
        {
            base.Update(updateContext);
            timer -= updateContext.DelayTime;
        }

        public override void Collision(IUpdateContext updateContext, GameObject other)
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
            base.Collision(updateContext, other);
        }
    }
}
