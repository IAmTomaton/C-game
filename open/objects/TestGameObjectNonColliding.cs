using Cgame.Contexts;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cgame
{
    class TestGameObjectWithoutCollider : GameObject
    {
        public static int c = 0;
        
        public TestGameObjectWithoutCollider() : base()
        {
            Sprite = new Sprite(this, "base");
            Mass = 1;
        }

        public override void Update(IUpdateContext updateContext)
        {
            c++;
            base.Update(updateContext);
        }
    }
}
