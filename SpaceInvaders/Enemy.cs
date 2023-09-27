using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_CsLo;
namespace SpaceInvaders




{
    internal class Enemy
    {
        public Transformcomponent transform;
        public Collisioncomponent collision;
        SpriteRendererComponent spriteRenderer;
        public bool active;
        public int scoreValue;
        public Enemy(Vector2 startPosition, Vector2 direction, float speed, int size, Texture image, int score)
        {
            transform = new Transformcomponent(startPosition, direction, speed);

            collision = new Collisioncomponent(new Vector2(size, size));

            spriteRenderer = new SpriteRendererComponent(image, Raylib.RED, transform, collision);

            active = true;

            scoreValue = score;
        }
        internal void Draw()
        {
            if (active)
            {
                spriteRenderer.Draw();

                
            }
            
        }

        internal void Update()
        {
            if (active)
            {
                float deltaTime = Raylib.GetFrameTime();
                transform.position += transform.direction * transform.speed * deltaTime;
            }
        

          
        }
    }
}
