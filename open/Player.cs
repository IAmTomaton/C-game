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
    class Player:GameObject
    {
        //написать таймер для смены картинки(?)
        private float speed = 300f;
        private float lives = 5;
        public Player() : base()
        {
            Sprite = new Sprite(this, new[] { "player1", "player2"});
            Collider = new Collider(this, 128, 128);
            Layer = Layers.Player;
            Mass = 1;
        }

        
        public override void Start(IUpdateContext updateContext)
        {
            updateContext.BindGameObjectToCamera(this);
            base.Start(updateContext);
        }

        //public void Shoot() точка для стрельбы

        public override void Update(IUpdateContext updateContext)
        {
            var up = Vector3.UnitY;
            var right = Vector3.UnitX;
            var input = updateContext.Keyboard;
            //Sprite.StepForward();

            if (input.IsKeyDown(Key.W))
            {
                Position += up * speed * updateContext.DelayTime;
            }
            if (input.IsKeyDown(Key.S))
            {
                Position -= up * speed * updateContext.DelayTime;
            }
            if (input.IsKeyDown(Key.A))
            {
                Position -= right * speed * updateContext.DelayTime;
            }
            if (input.IsKeyDown(Key.D))
            {
                Position += right * speed * updateContext.DelayTime;
            }

            base.Update(updateContext);
        }
    }
}
