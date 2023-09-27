using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_CsLo;
using System.Numerics;
using SpaceInvaders;
using System.Drawing;

namespace Invaders_Demo
{
    internal class Player
    {
        public Transformcomponent transform { get; private set; }
        public Collisioncomponent collision;

        //bullet time thing
        double shotInterval = 0.654321f; //SECONDS
        double lastShotTime;
        public bool active;

        List<Invaders.Cheats> activeCheats;
        //todo spriterenderer
        Texture image;
        public Player(Vector2 startPos, float speed, int size, Texture image, List<Invaders.Cheats> activeCheats)
        {

            transform = new Transformcomponent(startPos, new Vector2(0,0), speed);
            collision = new Collisioncomponent(new Vector2(size, size));

            this.image = image;
            this.activeCheats = activeCheats;
            active = true;

            lastShotTime = -shotInterval;
        }

        public bool Update()
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            Vector2 center = transform.position + collision.size / 2;
            

            float deltaTime = Raylib.GetFrameTime();
            //Player movement left and right
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
            {
                transform.position.X -= transform.speed * deltaTime;
            }
            else if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
            {
                transform.position.X += transform.speed * deltaTime;
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_E)) 
            {
                if (activeCheats.Contains(Invaders.Cheats.FastFire))
                {
                    
                    return true;
                }
                //Player shoots
                double timeNow = Raylib.GetTime();
                double timeSinceShot = Raylib.GetTime() - lastShotTime;
                if (timeSinceShot >= shotInterval)
                {
                    Console.WriteLine("Shots");
                    lastShotTime = timeNow;
                    return true;
                }
               

            }
            return false;
        }

        public void Draw()
        {
            float ScaleX = collision.size.X / image.width;
            float ScaleY = collision.size.Y / image.height;
            float scale = Math.Min(ScaleX, ScaleY);

            Raylib.DrawTextureEx(image, transform.position, 0.0f, scale, Raylib.WHITE);
            

            //Raylib.DrawTextureV(image, transform.position, Raylib.WHITE);

            Raylib.DrawRectangleLines((int) transform.position.X,(int) transform.position.Y, (int)collision.size.X, (int)collision.size.Y, Raylib.SKYBLUE);


        }
    }
}
