using Cgame.Contexts;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cgame
{
    abstract class GameObject
    {
        //TODO: реализовать этот класс

        public Sprite Sprite { get; protected set; }
        public Vector3 Position { get; set; }
        public double Angle { get; set; }

        public static void Init(IInitContext initContext)
        {
            
        }

        virtual public void Start(IUpdateContext updateContext)
        {

        }

        virtual public void Update(IUpdateContext updateContext)
        {

        }

        virtual public void Collision(IUpdateContext updateContext, GameObject other)
        {

        }
    }
}
