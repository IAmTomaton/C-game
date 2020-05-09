using Cgame.Contexts;
using OpenTK;

namespace Cgame
{
    class TestGameObjectWithoutMass : GameObject
    {
        public Sprite Sprite { get; set; }
        public float Mass { get; set; }
        public Layers Layer { get; set; }
        public Collider Collider { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }


        public TestGameObjectWithoutMass() : base()
        {
            Sprite = new Sprite(this, "base");
            Collider = new Collider(this, 64, 64);
            Layer = Layers.Platform;
        }

        public void Start(IUpdateContext updateContext)
        {
        }

        public void Update(IUpdateContext updateContext)
        {
        }

        public void Collision(IUpdateContext updateContext, GameObject other)
        {
        }
    }
}
