﻿using Cgame.Core;
using Cgame.Interfaces;
using OpenTK;

namespace Cgame.objects
{
    class Obstacle : GameObject, IKilling, IShootable
    {
        public Obstacle()
        {
            Sprite = new Sprite(this, "triangle");
            Collider = new Collider(this, 48, 48);
            Layer = Layers.Object;
            Mass = 1;
        }

        public Obstacle(Vector3 pos):this()
        {
            Position = pos;
        }

        public Obstacle(GameObjectParameter parameter) : this(parameter.Position) 
        { }
    }
}
