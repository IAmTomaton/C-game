using Cgame.Contexts;
using Cgame.Objects;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    class Space : IUpdateContext
    {
        public float DelayTime { get; private set; }
        public KeyboardState Keyboard { get; private set; }
        public MouseState Mouse { get; private set; }
        public Camera Camera { get; private set; }

        private readonly List<GameObject> globalObjects = new List<GameObject>();
        private readonly List<GameObject> localObjects = new List<GameObject>();

        public Space(Camera camera)
        {
            Camera = camera;
            var r = new Random();
            for (var i = 0; i < 310; i++)
            {
                var test2 = new TestGameObject
                {
                    Position = new Vector3(i * 128, 0, 0)
                };
                AddObject(test2);
            }
            var test = new TestGameObjectWhithCamera
            {
                Position = new Vector3(0, 0, 0)
            };
            AddObject(test);
        }

        public void BindGameObjectToCamera(GameObject gameObject)
        {
            Camera.GameObject = gameObject;
        }

        public void Update(float delayTime, KeyboardState keyboardState, MouseState mouseState)
        {
            DelayTime = delayTime;
            Keyboard = keyboardState;
            Mouse = mouseState;
            MoveGameObjects();
            UpdateGameObjects();
            IntersectionCheck();
        }

        private void MoveGameObjects()
        {
            foreach (var gameObject in globalObjects.Concat(localObjects))
                MoveGameObject(gameObject);
        }

        private void MoveGameObject(GameObject gameObject)
        {
            gameObject.Position += new Vector3(gameObject.Velocity.X * DelayTime, gameObject.Velocity.Y * DelayTime, 0);
        }

        private void UpdateGameObjects()
        {
            foreach (var gameObject in globalObjects.Concat(localObjects))
                gameObject.Update(this);
        }

        private void IntersectionCheck()
        {
            var objects = globalObjects.Concat(localObjects).ToList();
            for (var i = 0; i < objects.Count; i++)
                for (var j = i + 1; j < objects.Count; j++)
                {
                    if (objects[i].Collider is null || objects[j].Collider is null)
                        continue;
                    var collision = new Collision(objects[i].Collider, objects[j].Collider);
                    if (!collision.Collide)
                        continue;
                    var massSum = objects[i].Mass + objects[j].Mass;
                    var delta = objects[i].Mass == 0 ? Vector2.Zero : collision.Mtv * (objects[i].Mass / massSum) * collision.MtvLength;
                    objects[i].Position += new Vector3(delta);
                    delta = objects[j].Mass == 0 ? Vector2.Zero : collision.Mtv * (objects[j].Mass / massSum) * collision.MtvLength;
                    objects[j].Position -= new Vector3(delta);
                    objects[i].Collision(this, objects[j]);
                    objects[j].Collision(this, objects[i]);
                }
        }
        

        public IEnumerable<Sprite> GetSprites()
        {
            return globalObjects
                .Concat(localObjects)
                .Select(obj => obj.Sprite)
                .Where(sprite => !(sprite is null));
        }

        public void AddObject(GameObject gameObject)
        {
            if (ObjectExistence(gameObject))
                throw new Exception("Object cannot be added as local. It is already global.");
            if (GlobalObjectExistence(gameObject))
                throw new Exception("Object cannot be added as local. It is already local.");
            localObjects.Add(gameObject);
            gameObject.Start(this);
        }

        public void AddGlobalObject(GameObject gameObject)
        {
            if (ObjectExistence(gameObject))
                throw new Exception("Object cannot be added as global. It is already global.");
            if (GlobalObjectExistence(gameObject))
                throw new Exception("Object cannot be added as global. It is already local.");
            globalObjects.Add(gameObject);
            gameObject.Start(this);
        }

        public bool ObjectExistence(GameObject gameObject)
        {
            return localObjects.Contains(gameObject);
        }

        public bool GlobalObjectExistence(GameObject gameObject)
        {
            return globalObjects.Contains(gameObject);
        }

        public IEnumerable<T> FindObject<T>() where T: GameObject
        {
            return localObjects.Select(obj => obj as T).Where(obj => !(obj is null));
        }

        public IEnumerable<T> FindGlobalObject<T>() where T: GameObject
        {
            return globalObjects.Select(obj => obj as T).Where(obj => !(obj is null));
        }

        public void DeleteObject(GameObject gameObject)
        {
            if (!ObjectExistence(gameObject))
                throw new Exception("This local object does not exist in space.");
            localObjects.Remove(gameObject);
        }

        public void DeleteGlobalObject(GameObject gameObject)
        {
            if (!GlobalObjectExistence(gameObject))
                throw new Exception("This global object does not exist in space.");
            globalObjects.Remove(gameObject);
        }
    }
}
