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

namespace RymdSkjutare
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RymdSkjutare : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D asteroid, bullet, spaceship;
        Vector2 spaceshipPosition;
        float spaceshipRotation;
        bool canShoot = true;
        float spawnTimer = 500;
        Rectangle spaceshipRectangle;

        SpriteFont font;

        List<Bullet> blist = new List<Bullet>();
        List<Asteroid> alist = new List<Asteroid>();

        static Random r = new Random();

        public RymdSkjutare()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void ShootBullet()
        {
            //if (blist.Count <= 5)
            {
                blist.Add(new Bullet(bullet, spaceshipPosition, GetDirection(spaceshipPosition, MouseLocation)));
            }
        }

        public Vector2 MouseLocation
        {
            get { return new Vector2(Mouse.GetState().X, Mouse.GetState().Y); } 
        }

        public float GetAngle(Vector2 origin, Vector2 destination)
        {
            return (float)Math.Atan2(destination.Y - origin.Y, destination.X - origin.X);
        }

        public Vector2 GetDirection(Vector2 origin, Vector2 destination)
        {
            return new Vector2(destination.X - origin.X, destination.Y - origin.Y);
        }

        public void SpawnRandomAsteroid()
        {
            Vector2 pos = Vector2.Zero;
            switch (r.Next(0, 4))
            {
                case 0:
                    pos = new Vector2(graphics.PreferredBackBufferWidth, r.Next(0, graphics.PreferredBackBufferHeight)); 
                    alist.Add(new Asteroid(asteroid, pos, GetDirection(pos, spaceshipPosition)));
                    break;
                case 1:
                    pos = new Vector2(r.Next(0, graphics.PreferredBackBufferWidth),-spaceship.Height);
                    alist.Add(new Asteroid(asteroid, pos, GetDirection(pos, spaceshipPosition)));
                    break;
                case 2:
                    pos = new Vector2(-spaceship.Width, r.Next(0, graphics.PreferredBackBufferHeight));
                    alist.Add(new Asteroid(asteroid, pos, GetDirection(pos, spaceshipPosition)));
                    break;
                case 3:
                    pos = new Vector2(r.Next(0, graphics.PreferredBackBufferWidth), graphics.PreferredBackBufferHeight);
                    alist.Add(new Asteroid(asteroid, pos, GetDirection(pos, spaceshipPosition)));
                    break;
                default:
                    break;
            }
        }

        protected override void Initialize()
        {
            spaceshipPosition = new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2);
            IsMouseVisible = true;

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
            asteroid = Content.Load<Texture2D>("asteroid");
            bullet = Content.Load<Texture2D>("BULLET");
            spaceship = Content.Load<Texture2D>("spaceship");

            spaceshipRectangle = new Rectangle((int)spaceshipPosition.X, (int)spaceshipPosition.Y, spaceship.Width, spaceship.Height);
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
                this.Exit();

            if (GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed
                || (Keyboard.GetState().IsKeyDown(Keys.Space) && canShoot))
            {
                canShoot = false;
                ShootBullet();
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                canShoot = true;
            }
            foreach (Bullet b in blist)
            {
                b.Update();
            }

            foreach (Asteroid a in alist)
            {
                a.Update();
            }

            for (int i = 0; i < blist.Count; i++)
            {
                for (int j = 0; j < alist.Count; j++)
                {
                    if (blist[i].Box.Intersects(alist[j].Box))
                    {
                        blist.RemoveAt(i);
                        alist.RemoveAt(j);
                    }
                }	
            }

            for (int j = 0; j < alist.Count; j++)
            {
                if (spaceshipRectangle.Intersects(alist[j].Box))
                {
                    this.Exit();
                }
            }

            spawnTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (spawnTimer <= 0)
            {
                SpawnRandomAsteroid();
                spawnTimer = 1000;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(spaceship, spaceshipPosition, null, Color.White, 
                GetAngle(spaceshipPosition, MouseLocation) + (float)Math.PI/2, 
                new Vector2(spaceship.Width/2, spaceship.Height/2), 1f,SpriteEffects.None, 0f);

            foreach (Bullet b in blist)
            {
                spriteBatch.Draw(bullet, b.Position, Color.White);
            }

            foreach (Asteroid a in alist)
            {
                spriteBatch.Draw(asteroid, a.Position, Color.White);
            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
