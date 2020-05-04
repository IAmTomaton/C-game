using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace Cgame
{
    class TextureLibrary
    {
        private readonly Dictionary<string, Primitive> primitives = new Dictionary<string, Primitive>();
        private readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
        private readonly Shader shader;
        public int IndicesLength => indices.Length;

        private readonly float[] vertices =
        {
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f
        };

        private readonly uint[] indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        public TextureLibrary(Shader shader)
        {
            this.shader = shader;
        }

        public void AddTexture(string name, string path)
        {
            //texture
            var texture = new Texture(path);
            textures[name] = texture;

            //primitive
            var primitive = new Primitive(texture.Width, texture.Height, shader);
            primitives[name] = primitive;
        }

        public Texture GetTexture(string name)
        {
            return textures.ContainsKey(name) ? textures[name] : textures["base"];
        }

        public Primitive GetPrimitive(string name)
        {
            return primitives.ContainsKey(name) ? primitives[name] : primitives["base"];
        }
    }
}
