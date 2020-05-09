using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Cgame.Contexts;
using OpenTK;
using OpenTK.Input;

namespace Cgame
{
    class Player:GameObject
    {
        private bool isAlive = true;
        private Vector2 vert_acc = new Vector2(0, -10);//-10
        private Vector2 horiz_acc = new Vector2(-10, 0);
        private static Vector2 up = Vector2.UnitY;
        private static Vector2 right = Vector2.UnitX;
        private bool isJumpingUp = false;
        private bool isFallingAfterJump = false;
        //private bool isAccelerating;
        private static float defaultSpeedX = 10;

        public Sprite Sprite { get; set; }
        public float Mass { get; set; }
        public Layers Layer { get; set; }
        public Collider Collider { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        public Player() : base()
        {
            Sprite = new Sprite(this, new[] { "player1", "player2"});
            Collider = new Collider(this, 48, 48);
            Layer = Layers.Player;
            Mass = 10;
            Velocity = new Vector2(defaultSpeedX,0);
        }

        
        public void Start(IUpdateContext updateContext)
        {
            updateContext.BindGameObjectToCamera(this);
            //base.Start(updateContext);
        }

        public void Collision(IUpdateContext updateContext, GameObject other)
        {
            if (other.Mass == 0)
            {
                if (isFallingAfterJump)
                {
                    isFallingAfterJump = false;
                }
            }
            if (other is IKilling)
            {
                Sprite.StepForward();
                Collider = null;
                Velocity =vert_acc = horiz_acc = new Vector2(0,0);
            }

            //base.Collision(updateContext, other);
        }

        /// <summary>
        /// Finds projection of the p to line. {p1;p2} defines line
        /// </summary>
        /// <param name="p">Point to project</param>
        /// <param name="p1">First point of line segment</param>
        /// <param name="p2">Second point of line segment</param>
        /// <returns></returns>
        public static Vector2 GetProjection(Vector2 p, Vector2 p1, Vector2 p2)
        {
            float fDenominator = (p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y);
            if (fDenominator == 0) // p1 and p2 are the same
                return p1;

            float t = (p.X * (p2.X - p1.X) - (p2.X - p1.X) * p1.X + p.Y * (p2.Y - p1.Y) - (p2.Y - p1.Y) * p1.Y) / fDenominator;

            return new Vector2(p1.X + (p2.X - p1.X) * t, p1.Y + (p2.Y - p1.Y) * t);
        }


        /// <summary>
        /// Raycast way of shooting
        /// </summary>
        public void ShootStraight(IUpdateContext updateContext, Vector2 shootDirection)
        {
            //potential problems in this coordinates(is neccessary to make zero minimum)
            var start = new Vector2(this.Position.X, this.Position.Y<0?0:this.Position.Y);
            if (updateContext.FindLocalObject<IShootable>().Count() != 0)
            {
                var objectsToShoot = updateContext.FindLocalObject<IShootable>();
                var circle = objectsToShoot.ElementAt(0).Collider;
                var projection = GetProjection(circle.Position, start, start + shootDirection);
                var x = projection.X;
                var y = projection.Y;
                //right way too
                /*var t = (circle.Position.X - start.X) / (rs.X - rs.Y);
                var x = start.X + rs.X * t;
                var y = start.Y + rs.Y * t;*/
                var d = Math.Sqrt((x - circle.Position.X) * (x - circle.Position.X) +
                    (y - circle.Position.Y) * (y - circle.Position.Y));
                var n = Math.Sqrt(Math.Abs(circle.Radius * circle.Radius - d * d));
                Vector2 res = new Vector2(x, y) - new Vector2((float)n * shootDirection.X, (float)n * shootDirection.Y);
                Console.WriteLine(res.ToString());
                if (res.Length <= 300)
                {
                    updateContext.objectsToDelete.Add(objectsToShoot.ElementAt(0));
                }
            }
        }

        public void SendBullet(IUpdateContext updateContext, Vector2 shootDirection,float range, float speed)
        {
            updateContext.objectsToAdd.Add(new Bullet(shootDirection, this.Position,range, speed));
        }

        public void Update(IUpdateContext updateContext)
        {
            var input = updateContext.Keyboard;
            var dt = updateContext.DelayTime;
            //problems with sprite changing
            //Sprite.StepForward();
            if (input.IsKeyDown(Key.Z))
                SendBullet(updateContext, new Vector2(1, 0), 300, 200);
                //ShootStraight(updateContext, new Vector2(1,0));
            if (input.IsKeyDown(Key.W) && !isJumpingUp && !isFallingAfterJump)
            {
                Velocity += 400 * up;
                isJumpingUp = true;
            }
            if (isJumpingUp && Velocity.Y == 0)
            {
                isJumpingUp = false;
                isFallingAfterJump = true;
            }
            if (Velocity.Y < -100)
            {
                Velocity = new Vector2(defaultSpeedX, -100);
            }
            //we may let user to move on his own
            /*if (input.IsKeyDown(Key.D))
            {
                speed += 40 * right; 
            }
            if (speed.X > 0)
                speed += horiz_acc;*/
            Velocity += vert_acc;
            Position += new Vector3(Velocity.X * dt, Velocity.Y * dt, 0);
            Console.WriteLine("speed "+ Velocity.ToString());
            Console.WriteLine("position " + Position.ToString());
            Console.WriteLine("acc " + vert_acc.ToString());
            //base.Update(updateContext);
        }
    }
}
