using Cgame.Contexts;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame.Objects
{
    class TestGameObject : GameObject
    {
        public TestGameObject(string texture)
        {
            Sprite = new Sprite(this, texture);
        }

        public TestGameObject() { }

        public override void Start(IUpdateContext updateContext)
        {
            base.Start(updateContext);
        }

        public override void Update(IUpdateContext updateContext)
        {
            Angle += 100 * updateContext.DelayTime;
            Position = new Vector3(40, 0, 0) * Matrix3.CreateRotationZ((float)MathHelper.DegreesToRadians(Angle));

            base.Update(updateContext);
        }
    }
}
