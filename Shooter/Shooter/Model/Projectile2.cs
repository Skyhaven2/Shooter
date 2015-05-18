﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Model
{
    class Projectile2
    {
        // Image representing the Projectile
        public Texture2D Texture;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position;

        // State of the Projectile
        public bool Active;

        // The amount of damage the projectile can inflict to an enemy
        public int Damage;

        // Represents the viewable boundary of the game
        Viewport viewport;

        // Get the width of the projectile ship
        public int Width
        {
            get { return Texture.Width; }
        }

        // Get the height of the projectile ship
        public int Height
        {
            get { return Texture.Height; }
        }

        // Determines how fast the projectile moves
        float projectileMoveSpeed;

        float projectileVerticalCurve;

        int type;


        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, int type)
        {
            Texture = texture;
            Position = position;
            this.viewport = viewport;

            Active = true;

            Damage = 2;

            projectileMoveSpeed = 20f;
            projectileVerticalCurve = 1f;
            this.type = type;
        }
        public void Update()
        {
            Position.X += projectileMoveSpeed;
            if(type == 0)
            {
                Position.Y += projectileVerticalCurve;
                projectileVerticalCurve *= 1.2f;
            }
            if (type == 1)
            {
                Position.Y += projectileVerticalCurve;
                projectileVerticalCurve *= 1.1f;
            }
            if (type == 3)
            {
                Position.Y -= projectileVerticalCurve;
                projectileVerticalCurve *= 1.1f;
            }
            if (type == 4)
            {
                Position.Y -= projectileVerticalCurve;
                projectileVerticalCurve *= 1.2f;
            }
           

            // Deactivate the bullet if it goes out of screen
            if (Position.X + Texture.Width / 2 > viewport.Width)
            {
                Active = false;
            }
            if (Position.Y + Texture.Height / 2 > viewport.Height)
            {
                Active = false;
            }
            if (Position.X + Texture.Width / 2 < 0)
            {
                Active = false;
            }
            if (Position.Y + Texture.Height / 2 < 0)
            {
                Active = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
            new Vector2(Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}