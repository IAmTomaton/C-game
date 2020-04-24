using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    class Sprite
    {
        //TODO: реализовать этот класс

        public string Texture { get; }
        public Vector3 Position => gameObject.Position;
        public float Angle => (float)MathHelper.DegreesToRadians(gameObject.Angle);

        private readonly GameObject gameObject;

        public Sprite(GameObject gameObject, string texture)
        {
            Texture = texture;
            this.gameObject = gameObject;
        }
    }
}
