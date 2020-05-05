namespace Cgame
{
    class TestGameObjectWithoutMass : GameObject
    {
        public TestGameObjectWithoutMass() : base()
        {
            Sprite = new Sprite(this, "base");
            Collider = new Collider(this, 64, 64);
            Layer = Layers.Platform;
        }
    }
}
