using Cgame.Contexts;
using OpenTK;
using OpenTK.Input;

namespace Cgame.Objects
{
    class TestGameObjectWithCamera : GameObject
    {
        public TestGameObjectWithCamera() : base()
        {
            Sprite = new Sprite(this, "none");
            Collider = new Collider(this, 128, 128);
            Layer = Layers.Player;
            Mass = 1;
        }

        public override void Start(IUpdateContext updateContext)
        {
            updateContext.BindGameObjectToCamera(this);
            base.Start(updateContext);
        }

        public override void Update(IUpdateContext updateContext)
        {
            var up = Vector3.UnitY;
            var right = Vector3.UnitX;
            var input = updateContext.Keyboard;
            const float cameraSpeed = 300f;

            if (input.IsKeyDown(Key.W))
            {
                Position += up * cameraSpeed * updateContext.DelayTime;
            }
            if (input.IsKeyDown(Key.S))
            {
                Position -= up * cameraSpeed * updateContext.DelayTime;
            }
            if (input.IsKeyDown(Key.A))
            {
                Position -= right * cameraSpeed * updateContext.DelayTime;
            }
            if (input.IsKeyDown(Key.D))
            {
                Position += right * cameraSpeed * updateContext.DelayTime;
            }

            base.Update(updateContext);
        }
    }
}
