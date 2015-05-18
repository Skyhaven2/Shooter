using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Shooter.Model;

namespace Shooter.Controller
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Player player;
        // Keyboard states used to determine key presses
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        // Gamepad states used to determine button presses
        private GamePadState currentGamePadState;
        private GamePadState previousGamePadState;

        // A movement speed for the player
        private float playerMoveSpeed;
        // Image used to display the static background
        private Texture2D mainBackground;

        // Parallaxing Layers
        private ParallaxingBackground bgLayer1;
        private ParallaxingBackground bgLayer2;

        // Enemies
        private Texture2D enemyTexture;
        private List<Enemy> enemies;

        // The rate at which the enemies appear
        private TimeSpan enemySpawnTime;
        private TimeSpan previousSpawnTime;

        // A random number generator
        private Random random;

        private Texture2D projectileTexture;
        private Texture2D projectileTexture1;
        private Texture2D projectileTexture2;
        private List<Projectile> projectiles;
        private List<Projectile2> projectiles2;
        private List<Projectile3> projectiles3;

        // The rate of fire of the player laser
        private TimeSpan fireTime;
        private TimeSpan fireTime2;
        private TimeSpan fireTime3;
        private TimeSpan previousFireTime;

        private Texture2D explosionTexture;
        private List<Animation> explosions;

        // The sound that is played when a laser is fired
        private SoundEffect laserSound;

        // The sound used when the player or an enemy dies
        private SoundEffect explosionSound;

        // The music played during gameplay
        private Song gameplayMusic;

        //Number that holds the player score
        int score;
        // The font used to display UI elements
        private SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();

            playerMoveSpeed = 8.0f;

            // Initialize the enemies list
            enemies = new List<Enemy>();

            // Set the time keepers to zero
            previousSpawnTime = TimeSpan.Zero;

            // Used to determine how fast enemy respawns
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);

            // Initialize our random number generator
            random = new Random();

            projectiles = new List<Projectile>();
            projectiles2 = new List<Projectile2>();
            projectiles3 = new List<Projectile3>();

            // Set the laser to fire every quarter second
            fireTime = TimeSpan.FromSeconds(1f);
            fireTime2 = TimeSpan.FromSeconds(.5f);
            fireTime3 = TimeSpan.FromSeconds(.25f);

            explosions = new List<Animation>();

            //Set player's score to zero
            score = 0;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the player resources
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("Images/shipAnimation");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y
            + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player.Initialize(playerAnimation, playerPosition);

            // Load the parallaxing background
            bgLayer1.Initialize(Content, "Images/bgLayer1", GraphicsDevice.Viewport.Width, -1);
            bgLayer2.Initialize(Content, "Images/bgLayer2", GraphicsDevice.Viewport.Width, -2);

            enemyTexture = Content.Load<Texture2D>("Images/mineAnimation");

            projectileTexture = Content.Load<Texture2D>("Images/laser");
            projectileTexture1 = Content.Load<Texture2D>("Images/projectile1");
            projectileTexture2 = Content.Load<Texture2D>("Images/Shuriken");

            explosionTexture = Content.Load<Texture2D>("Images/explosion");

            // Load the music
            gameplayMusic = Content.Load<Song>("sound/gameMusic");

            // Load the laser and explosion sound effect
            laserSound = Content.Load<SoundEffect>("sound/laserFire");
            explosionSound = Content.Load<SoundEffect>("sound/explosion");

            // Load the score font
            font = Content.Load<SpriteFont>("Images/gameFont");

            // Start the music right away
            PlayMusic(gameplayMusic);

            mainBackground = Content.Load<Texture2D>("Images/mainbackground");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // Save the previous state of the keyboard and game pad so we can determinesingle key/button presses
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            // Read the current state of the keyboard and gamepad and store it
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);


            //Update the player
            UpdatePlayer(gameTime);
            bgLayer1.Update();
            bgLayer2.Update();
            // Update the enemies
            UpdateEnemies(gameTime);

            // Update the collision
            UpdateCollision();

            // Update the projectiles
            UpdateProjectiles();

            // Update the explosions
            UpdateExplosions(gameTime);


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void AddEnemy()
        {
            // Create the animation object
            Animation enemyAnimation = new Animation();

            // Initialize the animation with the correct animation information
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);

            // Randomly generate the position of the enemy
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next(100, GraphicsDevice.Viewport.Height - 100));

            // Create an enemy
            Enemy enemy = new Enemy();

            // Initialize the enemy
            enemy.Initialize(enemyAnimation, position);

            // Add the enemy to the active enemies list
            enemies.Add(enemy);
        }

        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);
        }

        private void AddProjectile(Vector2 position)
        {
            Projectile projectile = new Projectile();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture1, position, 0);
            projectiles.Add(projectile);
            Projectile projectile2 = new Projectile();
            projectile2.Initialize(GraphicsDevice.Viewport, projectileTexture1, position, 1);
            projectiles.Add(projectile2);
            Projectile projectile3 = new Projectile();
            projectile3.Initialize(GraphicsDevice.Viewport, projectileTexture1, position, 2);
            projectiles.Add(projectile3);
            Projectile projectile4 = new Projectile();
            projectile4.Initialize(GraphicsDevice.Viewport, projectileTexture1, position, 3);
            projectiles.Add(projectile4);
            Projectile projectile5 = new Projectile();
            projectile5.Initialize(GraphicsDevice.Viewport, projectileTexture1, position, 4);
            projectiles.Add(projectile5);
            Projectile projectile6 = new Projectile();
            projectile6.Initialize(GraphicsDevice.Viewport, projectileTexture1, position, 5);
            projectiles.Add(projectile6);
            Projectile projectile7 = new Projectile();
            projectile7.Initialize(GraphicsDevice.Viewport, projectileTexture1, position, 6);
            projectiles.Add(projectile7);
            Projectile projectile8 = new Projectile();
            projectile8.Initialize(GraphicsDevice.Viewport, projectileTexture1, position, 7);
            projectiles.Add(projectile8);


        }

        private void AddProjectile2(Vector2 position)
        {
            Projectile2 projectile = new Projectile2();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture, position, 0);
            projectiles2.Add(projectile);
            Projectile2 projectile1 = new Projectile2();
            projectile1.Initialize(GraphicsDevice.Viewport, projectileTexture, position, 1);
            projectiles2.Add(projectile1);
            Projectile2 projectile2 = new Projectile2();
            projectile2.Initialize(GraphicsDevice.Viewport, projectileTexture, position, 2);
            projectiles2.Add(projectile2);
            Projectile2 projectile3 = new Projectile2();
            projectile3.Initialize(GraphicsDevice.Viewport, projectileTexture, position, 3);
            projectiles2.Add(projectile3);
            Projectile2 projectile4 = new Projectile2();
            projectile4.Initialize(GraphicsDevice.Viewport, projectileTexture, position, 4);
            projectiles2.Add(projectile4);
        }

        private void AddProjectile3(Vector2 position)
        {
            Projectile3 projectile = new Projectile3();
            projectile.Initialize(GraphicsDevice.Viewport, projectileTexture2, position);
            projectiles3.Add(projectile);
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            // Spawn a new enemy enemy every 1.5 seconds
            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                // Add an Enemy
                AddEnemy();
            }

            // Update the Enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);

                if (enemies[i].Active == false)
                {
                    // If not active and health <= 0
                    if (enemies[i].Health <= 0)
                    {
                        // Add an explosion
                        AddExplosion(enemies[i].Position);
                        // Play the explosion sound
                        explosionSound.Play();
                        //Add to the player's score
                        score += enemies[i].Value;
                    }
                    enemies.RemoveAt(i);
                }
            }
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            player.Update(gameTime);

            // Get Thumbstick Controls
            player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
            player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

            // Use the Keyboard / Dpad
            if (currentKeyboardState.IsKeyDown(Keys.Left) ||
            currentGamePadState.DPad.Left == ButtonState.Pressed)
            {
                player.Position.X -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Right) ||
            currentGamePadState.DPad.Right == ButtonState.Pressed)
            {
                player.Position.X += playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) ||
            currentGamePadState.DPad.Up == ButtonState.Pressed)
            {
                player.Position.Y -= playerMoveSpeed;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) ||
            currentGamePadState.DPad.Down == ButtonState.Pressed)
            {
                player.Position.Y += playerMoveSpeed;
            }

            // Make sure that the player does not go out of bounds
            player.Position.X = MathHelper.Clamp(player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
            player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);

            // Fire only every interval we set as the fireTime
            if (gameTime.TotalGameTime - previousFireTime > fireTime)
            {
                if (currentGamePadState.Buttons.A == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Q))
                {
                    // Reset our current time
                    previousFireTime = gameTime.TotalGameTime;

                    // Add the projectile, but add it to the front and center of the player
                    AddProjectile(player.Position + new Vector2(player.Width / 2, 0));
                    // Play the laser sound
                    laserSound.Play();
                }
            }
            if(gameTime.TotalGameTime - previousFireTime > fireTime2)
            {
                if (currentGamePadState.Triggers.Right > .9f || currentKeyboardState.IsKeyDown(Keys.W))
                {
                    // Reset our current time
                    previousFireTime = gameTime.TotalGameTime;

                    // Add the projectile, but add it to the front and center of the player
                    AddProjectile2(player.Position + new Vector2(player.Width / 2, 0));
                    // Play the laser sound
                    laserSound.Play();
                }
            }
            if (gameTime.TotalGameTime - previousFireTime > fireTime3)
            {
                if (currentGamePadState.Triggers.Left > .9f || currentKeyboardState.IsKeyDown(Keys.E))
                {
                    // Reset our current time
                    previousFireTime = gameTime.TotalGameTime;

                    // Add the projectile, but add it to the front and center of the player
                    AddProjectile3(player.Position + new Vector2(player.Width / 2, 0));
                    // Play the laser sound
                    laserSound.Play();
                }
            }

            // reset score if player health goes to zero
            if (player.Health <= 0)
            {
                player.Health = 100;
                score = 0;
            }
        }

        private void UpdateProjectiles()
        {
            // Update the Projectiles
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                projectiles[i].Update();

                if (projectiles[i].Active == false)
                {
                    projectiles.RemoveAt(i);
                }
            }

            // Update the Projectiles2
            for (int i = projectiles2.Count - 1; i >= 0; i--)
            {
                projectiles2[i].Update();

                if (projectiles2[i].Active == false)
                {
                    projectiles2.RemoveAt(i);
                }
            }

            // Update the Projectiles3
            for (int i = projectiles3.Count - 1; i >= 0; i--)
            {
                projectiles3[i].Update();

                if (projectiles3[i].Active == false)
                {
                    projectiles3.RemoveAt(i);
                }
            }
            
        }

        private void UpdateCollision()
        {
            // Use the Rectangle's built-in intersect function to 
            // determine if two objects are overlapping
            Rectangle rectangle1;
            Rectangle rectangle2;

            // Only create the rectangle once for the player
            rectangle1 = new Rectangle((int)player.Position.X,
            (int)player.Position.Y,
            player.Width,
            player.Height);

            // Do the collision between the player and the enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                rectangle2 = new Rectangle((int)enemies[i].Position.X,
                (int)enemies[i].Position.Y,
                enemies[i].Width,
                enemies[i].Height);

                // Determine if the two objects collided with each
                // other
                if (rectangle1.Intersects(rectangle2))
                {
                    // Subtract the health from the player based on
                    // the enemy damage
                    player.Health -= enemies[i].Damage;

                    // Since the enemy collided with the player
                    // destroy it
                    enemies[i].Health = 0;

                    // If the player health is less than zero we died
                    if (player.Health <= 0)
                        player.Active = false;
                }

            }

            // Projectile vs Enemy Collision
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    // Create the rectangles we need to determine if we collided with each other
                    rectangle1 = new Rectangle((int)projectiles[i].Position.X -
                    projectiles[i].Width / 2, (int)projectiles[i].Position.Y -
                    projectiles[i].Height / 2, projectiles[i].Width, projectiles[i].Height);

                    rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
                    (int)enemies[j].Position.Y - enemies[j].Height / 2,
                    enemies[j].Width, enemies[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        enemies[j].Health -= projectiles[i].Damage;
                        projectiles[i].Active = false;
                    }
                }
            }

            for (int i = 0; i < projectiles2.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    // Create the rectangles we need to determine if we collided with each other
                    rectangle1 = new Rectangle((int)projectiles2[i].Position.X -
                    projectiles2[i].Width / 2, (int)projectiles2[i].Position.Y -
                    projectiles2[i].Height / 2, projectiles2[i].Width, projectiles2[i].Height);

                    rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
                    (int)enemies[j].Position.Y - enemies[j].Height / 2,
                    enemies[j].Width, enemies[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        enemies[j].Health -= projectiles2[i].Damage;
                        projectiles2[i].Active = false;
                    }
                }
            }

            for (int i = 0; i < projectiles3.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    // Create the rectangles we need to determine if we collided with each other
                    rectangle1 = new Rectangle((int)projectiles3[i].Position.X -
                    projectiles3[i].Width / 2, (int)projectiles3[i].Position.Y -
                    projectiles3[i].Height / 2, projectiles3[i].Width, projectiles3[i].Height);

                    rectangle2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
                    (int)enemies[j].Position.Y - enemies[j].Height / 2,
                    enemies[j].Width, enemies[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        enemies[j].Health -= projectiles3[i].Damage;
                        projectiles3[i].Active = false;
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkViolet);

            // TODO: Add your drawing code here
            // Start drawing
            spriteBatch.Begin();

            spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);

            // Draw the moving background
            bgLayer1.Draw(spriteBatch);
            bgLayer2.Draw(spriteBatch);
            // Draw the Player
            player.Draw(spriteBatch);

            // Draw the Enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(spriteBatch);
            }

            // Draw the Projectiles
            for (int i = 0; i < projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }

            // Draw the Projectiles2
            for (int i = 0; i < projectiles2.Count; i++)
            {
                projectiles2[i].Draw(spriteBatch);
            }

            // Draw the Projectiles3
            for (int i = 0; i < projectiles3.Count; i++)
            {
                projectiles3[i].Draw(spriteBatch);
            }

            // Draw the explosions
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(spriteBatch);
            }

            // Draw the score
            spriteBatch.DrawString(font, "score: " + score, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
            // Draw the player health
            spriteBatch.DrawString(font, "health: " + player.Health, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + 30), Color.White);

            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void PlayMusic(Song song)
        {
            // Due to the way the MediaPlayer plays music,
            // we have to catch the exception. Music will play when the game is not tethered
            try
            {
                // Play the music
                MediaPlayer.Play(song);

                // Loop the currently playing song
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }
    }
}
