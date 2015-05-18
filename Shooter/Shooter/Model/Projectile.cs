// Projectile.cs
//Using declarations
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Model
{
    public class Projectile
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
        private Viewport viewport;

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

        private int type;

        // Determines how fast the projectile moves
        float projectileMoveSpeed;


        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position, int type)
        {
            this.type = type;
            Texture = texture;
            Position = position;
            this.viewport = viewport;

            Active = true;

            Damage = 10;

            projectileMoveSpeed = 20f;
        }
        public void Update()
        {
            if (type == 0)
            {
                // Projectiles always move to the right
                Position.X += projectileMoveSpeed;
            }
            if (type == 1)
            {
                Position.X += projectileMoveSpeed;
                Position.Y += projectileMoveSpeed;
            }
            if (type == 2)
            {
                Position.Y += projectileMoveSpeed;
            }
            if (type == 3)
            {
                Position.Y += projectileMoveSpeed;
                Position.X -= projectileMoveSpeed;
            }
            if (type == 4)
            {
                Position.X -= projectileMoveSpeed;
            }
            if (type == 5)
            {
                Position.X -= projectileMoveSpeed;
                Position.Y -= projectileMoveSpeed;
            }
            if (type == 6)
            {
                Position.Y -= projectileMoveSpeed;
            }
            if (type == 7)
            {
                Position.Y -= projectileMoveSpeed;
                Position.X += projectileMoveSpeed;
            }

            //Random rng = new Random();

            //Position.Y += rng.Next(-30, 30);
            //Position.X += rng.Next(-30, 30);


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
