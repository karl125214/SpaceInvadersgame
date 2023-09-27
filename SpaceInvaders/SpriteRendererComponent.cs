﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_CsLo;
using System.Numerics;

namespace SpaceInvaders
{
    internal class SpriteRendererComponent
    {
        Texture sprite;
        Color debugColor;
        Transformcomponent transform_ref;
        Collisioncomponent collision_ref;

        float scale;
        Vector2 drawOffset;


        public SpriteRendererComponent(Texture image, Color color, Transformcomponent transform, Collisioncomponent collision)
        {
            sprite = image;
            debugColor = color;
            transform_ref = transform;
            collision_ref = collision;


            float scaleX = collision_ref.size.X / sprite.width;
            float scaleY = collision_ref.size.Y / sprite.height;
            scale = Math.Min(scaleX, scaleY);


            float xOffset = collision_ref.size.X - scale * sprite.width;
            float yOffset = collision_ref.size.Y - scale * sprite.height;
            drawOffset = new Vector2(xOffset, yOffset);
        }

        public void Draw()
        {
            Raylib.DrawTextureEx(sprite, transform_ref.position + drawOffset, 0.0f, scale, Raylib.WHITE);

            Raylib.DrawRectangleLines((int)transform_ref.position.X, (int)transform_ref.position.Y, (int)collision_ref.size.X, (int)collision_ref.size.Y, debugColor);
        }
    }
}
