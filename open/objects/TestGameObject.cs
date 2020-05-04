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
        public TestGameObject() : base()
        {
            Sprite = new Sprite(this, "base");
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
        }
    }
}
