using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cgame.Contexts;
using OpenTK;
using OpenTK.Input;

namespace Cgame
{
    class Bullet:GameObject
    {
        private static float defaultSpeedX = 5f;
        private Vector3 startPosition;
        private float range;

        public Sprite Sprite { get; set; }
        public float Mass { get; set; }
        public Layers Layer { get; set; }
        public Collider Collider { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        public Bullet(Vector2 direction, Vector3 start, float range, float speed=2f) : base()
        {
            this.range = range;
            Sprite = new Sprite(this, "bullet");
            Layer = Layers.Player;
            Collider = new Collider(this, 16, 16);
            Mass = 0.05f;
            Position = start+new Vector3(50,0,0);
            var normalized = direction.Normalized();
            Velocity = new Vector2(normalized.X*speed, normalized.Y*speed);
        }

        public void Start(IUpdateContext updateContext)
        {
            startPosition = Position;
            //base.Start(updateContext);
        }

        public void Update(IUpdateContext updateContext)
        {
            if ((Position - startPosition).Length >= range)
                updateContext.objectsToDelete.Add(this);
            //base.Update(updateContext);
        }

        public void Collision(IUpdateContext updateContext, GameObject other)
        {
            if (other is IShootable)
                updateContext.objectsToDelete.Add(other);
            //base.Collision(updateContext, other);
        }
    }
}
