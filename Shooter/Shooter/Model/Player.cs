using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter.Model
{
    public class Player
    {

        public Animation PlayerAnimation;
        public Vector2 Position;
        private bool active;
        private int health;

        // Get the width of the player ship
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        // Initialize the player
        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;

            // Set the player health
            Health = 100;
        }

        // Update the player animation
        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}

