using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_CsLo;

namespace SpaceInvaders
{
    internal class Bullet
    {
        public Transformcomponent transform;
        public Collisioncomponent collision;
        public bool isActive;
        Texture Image;

        //BULLET 
        public Bullet(Vector2 startPosition, Vector2 direction, float speed, int size, Texture bulletImage)
        {
            Image = bulletImage;
           Reset(startPosition, direction, speed, size);
        }

        public void Update()
        {
            transform.position += transform.direction * transform.speed * Raylib.GetFrameTime();


        }

        // BULLET DRAW
        public void Draw()
        {
            Raylib.DrawTextureEx(Image, transform.position,0, 0.3f, Raylib.WHITE);
            
        }

        //VOID RESET
        public void Reset(Vector2 startPosition, Vector2 direction, float speed, int size)
        {
            collision = new Collisioncomponent(new Vector2(size, size));
            transform = new Transformcomponent(startPosition, direction, speed);

            transform.position = startPosition;
            transform.direction = direction;
            transform.speed = speed;
            

            isActive = true;
        }
    }
}
