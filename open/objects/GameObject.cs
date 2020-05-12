using Cgame.Contexts;
using OpenTK;
using System;

namespace Cgame
{
    /*abstract class IShootable : GameObject
    {}

    abstract class IKillingShootable : GameObject
    {}*/

    interface IShootable:GameObject { }
    interface IKilling : GameObject { }
    
    //now we can only have one abstract class, but i'd better had several interfaces(?)
    //it's is not really important but i did

    public interface GameObject
    {
        //GameObject Copy();
        /// <summary>
        /// Спрайт объекта.
        /// </summary>
        Sprite Sprite { get;set; }
        /// <summary>
        /// Масса обекта.
        /// Если Mass = 0, то объект считается статичным и не смещается при коллизии.
        /// </summary>
        float Mass { get;set; }
        /// <summary>
        /// Указывает слой для столкновений.
        /// default: Layers.Base
        /// </summary>
        Layers Layer { get; set; }
        /// <summary>
        /// Коллайдер объекта.
        /// Если экземпляр объекта был добавлен в пространство без коллайдера, то он никогда не будет участвовать в столкновениях.
        /// </summary>
        Collider Collider { get; set; }
        Vector3 Position { get; set; }
        Vector2 Velocity { get; set; }
        float Angle { get; set; }

        /// <summary>
        /// На данный момент не нужен, но чтобы не дописывать его вызов во всех потомках когда он понадобится он тут есть.
        /// </summary>
        //public GameObject() { }

        /// <summary>
        /// Вызывается единственный раз для экземпляра, сразу после добавления в пространство.
        /// </summary>
        /// <param name="updateContext"></param>
        void Start(IUpdateContext updateContext);

        /// <summary>
        /// Вызывается на каждом обновлении пространства.
        /// </summary>
        /// <param name="updateContext"></param>
        void Update(IUpdateContext updateContext);

        /// <summary>
        /// Вызывается если произошла коллизия.
        /// </summary>
        /// <param name="updateContext"></param>
        /// <param name="other">Объект с которым произошла коллизия.</param>
        void Collision(IUpdateContext updateContext, GameObject other);
    }
}
