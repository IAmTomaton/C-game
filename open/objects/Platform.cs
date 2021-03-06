﻿using Cgame.Core;
using OpenTK;

namespace Cgame.objects
{
    class Platform : GameObject
    {
        public Platform()
        {
            Sprite = new Sprite(this, "platform");
            Collider = new Collider(this, 64, 256);
            Layer = Layers.Object;
            Mass = 0;
        }

        public Platform(Vector3 pos) : this()
        {
            Position = pos;
        }

        public Platform(GameObjectParameter parameter) : this(parameter.Position) { }
    }
}
