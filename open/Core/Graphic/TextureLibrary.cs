using Cgame.Core.Shaders;
using System.Collections.Generic;

namespace Cgame.Core.Graphic
{
    /// <summary>
    /// Класс библиотеки текстур.
    /// </summary>
    class TextureLibrary
    {
        private readonly Dictionary<string, Primitive> primitives = new Dictionary<string, Primitive>();
        private readonly Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
        private readonly Shader shader;
        /// <summary>
        /// Количество вершин в примитиве. Обычно 4.
        /// </summary>
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

        /// <summary>
        /// Добавляет в библиотеку текстру с указанным именем и изображением по указанному пути.
        /// </summary>
        /// <param name="name">Имя текстуры</param>
        /// <param name="path">Путь до файла изображения.</param>
        public void AddTexture(string name, string path)
        {
            //texture
            var texture = new Texture(path);
            textures[name] = texture;

            //primitive
            var primitive = new Primitive(texture.Width, texture.Height, shader);
            primitives[name] = primitive;
        }

        /// <summary>
        /// Возвращает текстуру с указанным именем.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Texture GetTexture(string name)
        {
            return textures.ContainsKey(name) ? textures[name] : textures["base"];
        }

        /// <summary>
        /// Возвращает примитив с указанным именем.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Primitive GetPrimitive(string name)
        {
            return primitives.ContainsKey(name) ? primitives[name] : primitives["base"];
        }
    }
}
