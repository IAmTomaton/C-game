using Cgame.Contexts;
using OpenTK;

namespace Cgame
{
    class TestGameObjectWithoutCollider:GameObject
    {
        public Sprite Sprite { get; set; }
        public float Mass { get; set; }
        public Layers Layer { get; set; }
        public Collider Collider { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        public TestGameObjectWithoutCollider() : base()
        {
            Sprite = new Sprite(this, "base");
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
