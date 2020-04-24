using Cgame.Contexts;
using Cgame.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    class Space : IInitContext, IUpdateContext
    {
        private readonly List<GameObject> objects = new List<GameObject>();
        private readonly TextureLibrary textureLibrary;

        public double DelayTime { get; private set; }

        public Space(TextureLibrary textureLibrary)
        {
            this.textureLibrary = textureLibrary;
            InitGameObjects();

            var test = new TestGameObject("base");
            AddObject(test);
        }

        public void Update(double delayTime)
        {
            DelayTime = delayTime;
            objects.ForEach(obj => obj.Update(this));
        }

        public IEnumerable<Sprite> GetSprites()
        {
            return objects.Select(obj => obj.Sprite).Where(sprite => !(sprite is null));
        }

        private void InitGameObjects()
        {
            GameObject.Init(this);
            var ourtype = typeof(GameObject);
            var list = Assembly.GetAssembly(ourtype).GetTypes().Where(type => type.IsSubclassOf(ourtype));
            foreach (var item in list)
            {
                item.GetMethod("Init")?.Invoke(null, new object[] { this });
            }
        }

        public void AddTexture(string name, string path)
        {
            textureLibrary.AddTexture(name, path);
        }

        public void AddObject(GameObject gameObject)
        {
            if (!objects.Contains(gameObject))
            {
                objects.Add(gameObject);
                gameObject.Start(this);
            }
        }

        public IEnumerable<T> FindObject<T>()
            where T: GameObject
        {
            return objects.Select(obj => obj as T).Where(obj => !(obj is null));
        }

        public void DeleteObject(GameObject gameObject)
        {
            objects.Remove(gameObject);
        }
    }
}
