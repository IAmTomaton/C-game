using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    class TextureLidraryLoader
    {
        private static List<Tuple<string, string>> textureTuples = new List<Tuple<string, string>>
        {
            Texture("base", "textures/base.bmp"),
            Texture("none", "textures/none.bmp")
        };

        public static TextureLibrary LoadTextureLibrary(Shader shader)
        {
            var textureLibrary = new TextureLibrary(shader);
            foreach (var texture in textureTuples)
            {
                textureLibrary.AddTexture(texture.Item1, texture.Item2);
            }
            return textureLibrary;
        }

        public static Tuple<string, string> Texture(string name, string path)
        {
            return Tuple.Create(name, path);
        }
    }
}
