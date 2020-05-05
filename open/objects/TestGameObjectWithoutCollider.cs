using Cgame.Contexts;

namespace Cgame
{
    class TestGameObjectWithoutCollider : GameObject
    {
        public TestGameObjectWithoutCollider() : base()
        {
            Sprite = new Sprite(this, "base");
        }
    }
}
