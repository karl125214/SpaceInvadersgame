using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    internal class Transformcomponent
    {
        public Vector2 position;
        public float speed;
        public Vector2 direction;
        public Transformcomponent(Vector2 position, Vector2 direction, float speed)
        {
            this.position = position;
            this.speed = speed;
            this.direction = direction;
        }
    }
}
