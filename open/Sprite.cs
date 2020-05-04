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
        public string Texture { get { return textures[CurrentIndex]; } }
        public Vector3 Position => gameObject.Position;
        public float Angle => (float)MathHelper.DegreesToRadians(gameObject.Angle);
        public int CurrentIndex { get; private set; }
        public int Count => textures.Length;

        private readonly GameObject gameObject;
        private readonly string[] textures;

        public Sprite(GameObject gameObject, string texture)
        {
            textures = new string[] { texture };
            this.gameObject = gameObject;
        }

        public Sprite(GameObject gameObject, string[] textures)
        {
            this.textures = textures;
            this.gameObject = gameObject;
        }

        public void StepForward() => SetIndex(1);

        public void StepBack() => SetIndex(-1);

        public void SetIndex(int index)
        {
            CurrentIndex = (index + Count) % Count;
        }
    }
}
