﻿using Cgame.Contexts;
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
            for (var i = 0; i < 300; i++)
            {
                var test2 = new TestGameObject
                {
                    Position = new Vector3(i * 128, -128, 0)
                };
                AddLocalObject(test2);
            }
            for (var i = 0; i < 1800; i++)
            {
                var test2 = new TestGameObjectWithoutCollider
                {
                    Position = new Vector3(i * 128, 128, 0)
                };
                AddLocalObject(test2);
            }
            var test = new TestGameObjectWhithCamera
            {
                Position = new Vector3(0, 0, 0)
            };
            AddLocalObject(test);
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
            foreach (var gameObject in AllObgects)
                MoveGameObject(gameObject);
        }

        private void MoveGameObject(GameObject gameObject)
        {
            gameObject.Position += new Vector3(gameObject.Velocity.X * DelayTime, gameObject.Velocity.Y * DelayTime, 0);
        }

        private void UpdateGameObjects()
        {
            foreach (var gameObject in AllObgects)
                gameObject.Update(this);
        }

        private void IntersectionCheck()
        {
            var objects = CollidingObjects.ToList();
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
            return AllObgects
                .Select(obj => obj.Sprite)
                .Where(sprite => !(sprite is null));
        }

        public void AddLocalObject(GameObject gameObject) =>
            AddObjectTo(gameObject, localCollidingObjects, localNonCollidingObjects, "local");
        public void AddGlobalObject(GameObject gameObject) =>
            AddObjectTo(gameObject, globalCollidingObjects, globalNonCollidingObjects, "global");

        private void AddObjectTo(GameObject gameObject, List<GameObject> colliding, List<GameObject> nonColliding, string type)
        {
            if (LocalObjectExistence(gameObject))
                throw new Exception($"Object cannot be added as {type}. It is already global.");
            if (GlobalObjectExistence(gameObject))
                throw new Exception($"Object cannot be added as {type}. It is already local.");
            if (gameObject.Collider is null)
                nonColliding.Add(gameObject);
            else
                colliding.Add(gameObject);
            gameObject.Start(this);
        }

        public bool LocalObjectExistence(GameObject gameObject) => LocalObjects.Contains(gameObject);
        public bool GlobalObjectExistence(GameObject gameObject) => GlobalObjects.Contains(gameObject);

        public IEnumerable<T> FindLocalObject<T>() where T: GameObject => FindObjectIn<T>(LocalObjects);
        public IEnumerable<T> FindGlobalObject<T>() where T : GameObject => FindObjectIn<T>(GlobalObjects);

        private IEnumerable<T> FindObjectIn<T>(IEnumerable<GameObject> objects) where T : GameObject
        {
            return objects.Where(obj => obj is T).Select(obj => obj as T);
        }

        public void DeleteLocalObject(GameObject gameObject) =>
            DeleteObjectFrom(gameObject, localCollidingObjects, localNonCollidingObjects, "local");
        public void DeleteGlobalObject(GameObject gameObject) =>
            DeleteObjectFrom(gameObject, globalCollidingObjects, globalNonCollidingObjects, "global");

        private void DeleteObjectFrom(GameObject gameObject, List<GameObject> colliding, List<GameObject> nonColliding, string type)
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
            throw new Exception($"This {type} object does not exist in space.");
        }
    }
}
