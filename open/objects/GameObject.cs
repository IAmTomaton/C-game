using Cgame.Contexts;
using OpenTK;

namespace Cgame
{
    abstract class GameObject
    {
        /// <summary>
        /// Спрайт объекта.
        /// </summary>
        public Sprite Sprite { get; protected set; }
        /// <summary>
        /// Масса обекта.
        /// Если Mass = 0, то объект считается статичным и не смещается при коллизии.
        /// </summary>
        public float Mass { get; protected set; }
        /// <summary>
        /// Указывает слой для столкновений.
        /// default: Layers.Base
        /// </summary>
        public Layers Layer { get; protected set; } = Layers.Base;
        /// <summary>
        /// Коллайдер объекта.
        /// Если экземпляр объекта был добавлен в пространство без коллайдера, то он никогда не будет участвовать в столкновениях.
        /// </summary>
        public Collider Collider { get; protected set; }
        public Vector3 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }

        /// <summary>
        /// На данный момент не нужен, но чтобы не дописывать его вызов во всех потомках когда он понадобится он тут есть.
        /// </summary>
        public GameObject() { }

        /// <summary>
        /// Вызывается единственный раз для экземпляра, сразу после добавления в пространство.
        /// </summary>
        /// <param name="updateContext"></param>
        virtual public void Start(IUpdateContext updateContext) { }

        /// <summary>
        /// Вызывается на каждом обновлении пространства.
        /// </summary>
        /// <param name="updateContext"></param>
        virtual public void Update(IUpdateContext updateContext) { }

        /// <summary>
        /// Вызывается если произошла коллизия.
        /// </summary>
        /// <param name="updateContext"></param>
        /// <param name="other">Объект с которым произошла коллизия.</param>
        virtual public void Collision(IUpdateContext updateContext, GameObject other) { }
    }
}
