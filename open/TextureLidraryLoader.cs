using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    /// <summary>
    /// Класс загрузчика библиотеки текстур.
    /// </summary>
    static class TextureLidraryLoader
    {
        /// <summary>
        /// Список информации для загрузки текстур.
        /// Именно здесь и нужно загружать текстуру в соответствии с шаблоном:
        /// Texture("имя текстуры", "путь до файла")
        /// </summary>
        private static readonly List<Tuple<string, string>> textureTuples = new List<Tuple<string, string>>
        {
            Texture("base", "textures/base.bmp"),
            Texture("none", "textures/none.bmp"),
            Texture("player1", @"C:\Users\pervu\source\repos\C-game\open\textures\a.bmp"),
            Texture("player2", @"C:\Users\pervu\source\repos\C-game\open\textures\b.bmp"),
            Texture("bullet", @"C:\Users\pervu\source\repos\C-game\open\textures\bullet.bmp"),
            Texture("triangle", @"C:\Users\pervu\source\repos\C-game\open\textures\triangle.bmp"),
            Texture("platform", @"C:\Users\pervu\source\repos\C-game\open\textures\platform.bmp")
        };

        /// <summary>
        /// Создаёт библиотеку текстур и загружает в её текстуры.
        /// </summary>
        /// <param name="shader">Шейдер</param>
        /// <returns>Готовая библиотека текстур.</returns>
        public static TextureLibrary LoadTextureLibrary(Shader shader)
        {
            var textureLibrary = new TextureLibrary(shader);
            foreach (var texture in textureTuples)
            {
                textureLibrary.AddTexture(texture.Item1, texture.Item2);
            }
            return textureLibrary;
        }

        /// <summary>
        /// Создаёт тапл с информацией, для последующей загрузки текстуры.
        /// </summary>
        /// <param name="name">Имя текстуры.</param>
        /// <param name="path">Путь до файла изображения.</param>
        /// <returns></returns>
        public static Tuple<string, string> Texture(string name, string path)
        {
            return Tuple.Create(name, path);
        }
    }
}
