using Cgame.Contexts;
using Cgame.objects;
using Cgame.Objects;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cgame
{
    /// <summary>
    /// Класс хранящий логическое представление игры и взаимодействующий с ним.
    /// </summary>
    class Space : IUpdateContext
    {
        public float DelayTime { get; private set; }
        public KeyboardState Keyboard { get; private set; }
        public MouseState Mouse { get; private set; }
        /// <summary>
        /// Текущаяя камера пространства.
        /// </summary>
        public Camera Camera { get; private set; }

        public List<GameObject> objectsToDelete { get; set; }
        public List<GameObject> objectsToAdd { get; set; }

        private readonly List<GameObject> globalCollidingObjects = new List<GameObject>();
        private readonly List<GameObject> localCollidingObjects = new List<GameObject>();
        private readonly List<GameObject> globalNonCollidingObjects = new List<GameObject>();
        private readonly List<GameObject> localNonCollidingObjects = new List<GameObject>();

        private IEnumerable<GameObject> AllObgects => globalCollidingObjects
            .Concat(localCollidingObjects)
            .Concat(globalNonCollidingObjects)
            .Concat(localNonCollidingObjects);
        private IEnumerable<GameObject> CollidingObjects => globalCollidingObjects
            .Concat(localCollidingObjects);
        private IEnumerable<GameObject> LocalObjects => localCollidingObjects
            .Concat(localNonCollidingObjects);
        private IEnumerable<GameObject> GlobalObjects => globalCollidingObjects
            .Concat(globalNonCollidingObjects);

        public Space(Camera camera)
        {
            Camera = camera;
            objectsToDelete = new List<GameObject>();
            objectsToAdd = new List<GameObject>();
            /*for (int i = 128; i < 3000; i++)
            {
                var test = new TestGameObjectWithoutMass
                {
                    Position = new Vector3(i, -128, 0)
                };
                AddLocalObject(test);
            }*/
            var test = new Platform
            {
                Position = new Vector3(128, -128, 0)
            };
            AddLocalObject(test);
            var test1 = new TestGameObjectWithoutCollider
            {
                Position = new Vector3(128, 128, 0)
            };
            AddLocalObject(test1);
            /*var test2 = new TestGameObject
            {
                Position = new Vector3(128, 512, 0)
            };
            AddLocalObject(test2);*/
            var test3 = new TestGameObjectWithoutMass
            {
                Position = new Vector3(512, -512, 0)
            };
            AddLocalObject(test3);
            var testMain = new Player
            {
                Position = new Vector3(128, 0, 0)
            };
            AddLocalObject(testMain);
            var obstacle = new Obstacle
            {
                Position = new Vector3(188, -65, 0)
            };
            AddLocalObject(obstacle);
            var testShoot = new TestGameObject
            {
                Position = new Vector3(358, 0, 0)
            };
            AddLocalObject(testShoot);
            var testShoot1 = new TestGameObject
            {
                Position = new Vector3(558, 200, 0)
            };
            AddLocalObject(testShoot1);
        }

        public void BindGameObjectToCamera(GameObject gameObject)
        {
            Camera.GameObject = gameObject;
        }

        /// <summary>
        /// Обновляет внутренне представление пространства.
        /// </summary>
        /// <param name="delayTime">Промежуток времени прошедший с последнего обновления.</param>
        /// <param name="keyboardState"></param>
        /// <param name="mouseState"></param>
        public void Update(float delayTime, KeyboardState keyboardState, MouseState mouseState)
        {
            DelayTime = delayTime;
            Keyboard = keyboardState;
            Mouse = mouseState;
            MoveGameObjects();
            UpdateGameObjects();
            DeleteGameObjects();
            AddGameObjects();
            CollisionCheck();
        }

        private void AddGameObjects()
        {
            foreach (var toAdd in objectsToAdd)
            {
                AddLocalObject(toAdd);
            }
            objectsToAdd = new List<GameObject>();
        }

        private void DeleteGameObjects()
        {
            foreach (var toDelete in objectsToDelete)
            {
                DeleteLocalObject(toDelete);
            }
            objectsToDelete = new List<GameObject>();
        }

        /// <summary>
        /// Перемещает все игровые объекты в соответствии с их скоростью.
        /// </summary>
        private void MoveGameObjects()
        {
            foreach (var gameObject in AllObgects)
                MoveGameObject(gameObject);
        }

        /// <summary>
        /// Перемещает игровой объект в соответствии с его скоростью.
        /// </summary>
        private void MoveGameObject(GameObject gameObject)
        {
            gameObject.Position += new Vector3(gameObject.Velocity.X * DelayTime, gameObject.Velocity.Y * DelayTime, 0);
            //if (gameObject is Player)
            //    Console.WriteLine("after move " + gameObject.Position.ToString());
        }

        /// <summary>
        /// Обновляет все игровые объекты.
        /// </summary>
        private void UpdateGameObjects()
        {
            foreach (var gameObject in AllObgects)
                gameObject.Update(this);
        }

        /// <summary>
        /// Проверяет столкновения для всех сталкиваемых игровых объектах.
        /// </summary>
        private void CollisionCheck()
        {
            var objects = CollidingObjects.ToList();
            for (var i = 0; i < objects.Count; i++)
                for (var j = i + 1; j < objects.Count; j++)
                {
                    if (!LayerSettings.CheckCollision(objects[i].Layer, objects[j].Layer))
                        continue;
                    if (objects[i].Collider is null || objects[j].Collider is null)
                        continue;
                    var collision = new Collision(objects[i].Collider, objects[j].Collider);
                    if (!collision.Collide)
                        continue;
                    if (!objects[i].Collider.IsTrigger && !objects[j].Collider.IsTrigger)
                    {
                        var massSum = objects[i].Mass + objects[j].Mass;
                        DisplacementObjectAfterCollision(objects[i], massSum, collision, 1);
                        DisplacementObjectAfterCollision(objects[j], massSum, collision, -1);
                    }
                    objects[i].Collision(this, objects[j]);
                    objects[j].Collision(this, objects[i]);
                }
        }

        /// <summary>
        /// Перемещает столкнувшийся объект.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="massSum"></param>
        /// <param name="collision"></param>
        /// <param name="revers"></param>
        private void DisplacementObjectAfterCollision(GameObject gameObject, float massSum, Collision collision, int revers)
        {
            if (gameObject.Mass == 0)
                return;
            var ratio = massSum == gameObject.Mass ? 1 : (massSum - gameObject.Mass) / massSum;
            var delta = collision.Mtv * ratio * collision.MtvLength;
            gameObject.Position += new Vector3(delta) * revers;
            /*if (gameObject is Player)
            {
                Console.WriteLine("ratio " + ratio.ToString()+"delta "+delta.ToString());
                Console.WriteLine("after collision " + gameObject.Position.ToString() + "reverse" + revers.ToString());
            }*/
        }

        /// <summary>
        /// Возвращает последовательность спрайтов для отрисовки.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Sprite> GetSprites()
        {
            return AllObgects
                .Where(obj => !(obj.Sprite is null))
                .Select(obj => obj.Sprite);
        }

        public void AddLocalObject(GameObject gameObject) =>
            AddObjectTo(gameObject, localCollidingObjects, localNonCollidingObjects);
        public void AddGlobalObject(GameObject gameObject) =>
            AddObjectTo(gameObject, globalCollidingObjects, globalNonCollidingObjects);

        private void AddObjectTo(GameObject gameObject, List<GameObject> colliding, List<GameObject> nonColliding)
        {
            if (LocalObjectExistence(gameObject) || GlobalObjectExistence(gameObject))
                return;
            if (gameObject.Collider is null)
                nonColliding.Add(gameObject);
            else
                colliding.Add(gameObject);
            gameObject.Start(this);
        }

        public bool LocalObjectExistence(GameObject gameObject) => LocalObjects.Contains(gameObject);
        public bool GlobalObjectExistence(GameObject gameObject) => GlobalObjects.Contains(gameObject);

        //where T: GameObject
        public IEnumerable<T> FindLocalObject<T>() => FindObjectIn<T>(LocalObjects);
        public IEnumerable<T> FindGlobalObject<T>() => FindObjectIn<T>(GlobalObjects);

        private IEnumerable<T> FindObjectIn<T>(IEnumerable<GameObject> objects)
        {
            //return objects.Where(obj => obj is T).Select(obj => obj as T);
            return objects.Cast<T>();
        }

        public void DeleteLocalObject(GameObject gameObject) =>
            DeleteObjectFrom(gameObject, localCollidingObjects, localNonCollidingObjects);
        public void DeleteGlobalObject(GameObject gameObject) =>
            DeleteObjectFrom(gameObject, globalCollidingObjects, globalNonCollidingObjects);

        private void DeleteObjectFrom(GameObject gameObject, List<GameObject> colliding, List<GameObject> nonColliding)
        {
            if (colliding.Contains(gameObject))
            {
                colliding.Remove(gameObject);
                return;
            }
            if (nonColliding.Contains(gameObject))
            {
                nonColliding.Remove(gameObject);
                return;
            }
        }
    }
}
