using Cgame.Core;
using Cgame.Core.Graphic;
using Cgame.Core.Shaders;
using Ninject;

namespace Cgame
{
    class Program
    {
        static StandardKernel GetConteiner()
        {
            var conteiner = new StandardKernel();
            conteiner.Bind<Game>().ToSelf();
            conteiner.Bind<WindowSettings>().ToConstant(new WindowSettings(600, 600, "Igra ebat"));
            conteiner.Bind<TextureLibrary>().ToSelf();
            conteiner.Bind<Space>().ToSelf();
            conteiner.Bind<Camera>().ToSelf();
            return conteiner;
        }

        static void Main(string[] args)
        {
            var conteiner = GetConteiner();
            using (var game = conteiner.Get<Game>())
            {
                game.Run(60.0);
            }
        }
    }
}
