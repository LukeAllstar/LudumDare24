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
using Squared.Tiled;
using System.IO;

namespace LudumDare24Evolution
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D mainBackground;

        Texture2D mapSprites;
        Texture2D playerSprite;
        Map map;
        Vector2 viewportPosition;
        float jumpSpeed = 0;
        int timeSinceJump;
        float gravity = 0.8f;

        Point playerPosition = new Point(100, 100);
        Player player = new Player();
        Rectangle intersection;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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

            
            Content.RootDirectory = "Content/Graphics";
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

            mapSprites = Content.Load<Texture2D>("EvolutionTileset_3");
            playerSprite = Content.Load<Texture2D>("squirrel_1");
            //map = Map.Load(Path.Combine(Content.RootDirectory, "MapTest2.tmx"), Content);
            map = Map.Load(Path.Combine(Content.RootDirectory, "map3.tmx"), Content);

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

            KeyboardState keyState = Keyboard.GetState();
            float positionX = player.getPositionX();
            float positionY = player.getPositionY();

            positionY += gravity;

            if (keyState.IsKeyDown(Keys.Left))
            {
                player.setDirection(-1);
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                player.setDirection(1);
            }
            else
            {
                player.setDirection(0);
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                if (timeSinceJump != 0)
                {
                    player.changeRunstate(Runstate.JUMPING);
                }
            }


            // movement X axis
            positionX += (int)player.getMovespeed() * player.getDirection();


            // check if jump ends
            if (player.getRunstate() == Runstate.JUMPING)
            {
                timeSinceJump += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeSinceJump > 400)
                {
                    player.changeRunstate(Runstate.FALLING);
                    timeSinceJump = 0;
                }
            }


            if (player.getRunstate() == Runstate.JUMPING)
            {
                positionY -= (int)jumpSpeed;
            }
            else if (player.getRunstate() == Runstate.FALLING)
            {
                positionY += (int)jumpSpeed;
            }



            // collision detection
            Rectangle rec;
            intersection = new Rectangle(); // intersection reset
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map.Layers["Collision"].GetTile(x, y) != 0)
                    {
                        rec = Rectangle.Intersect(new Rectangle(x * map.TileWidth, y * map.TileHeight, map.TileWidth, map.TileHeight),
                        new Rectangle((int)positionX, (int)positionY, playerSprite.Width, playerSprite.Height));
                        if (rec.Width > 0 && rec.Height > 0)
                        {
                            intersection = rec;
                        }
                    }
                }
            }

            // check where the collision happened
            if (intersection.Width > 0 && intersection.Height > 0)
            {
                if (intersection.X <= player.getPositionX())
                {
                    // player right of collision
                    if (player.getDirection() == 0)
                        positionX = player.getPositionX();
                }
                else
                {
                    // player left of collision
                    if (player.getDirection() == 0)
                        positionX = player.getPositionX();
                }
                if (intersection.Y < player.getPositionY())
                {
                    // collision top
                    player.changeRunstate(Runstate.FALLING);
                    //positionY += (int)jumpSpeed;
                }
                else
                {
                    // collision bottom
                    //positionY -= gravity;
                    player.changeRunstate(Runstate.WALKING);
                }

            }



            player.updatePosition(positionX, positionY);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            spriteBatch.Begin();

            spriteBatch.Draw(playerSprite, new Rectangle(player.getPositionX(), player.getPositionY(), playerSprite.Width, playerSprite.Height), Color.White);
            map.Draw(spriteBatch, new Rectangle(0, 0, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height), viewportPosition);

            //intersection = new Rectangle(20, 20, 100, 100);
            var t = new Texture2D(GraphicsDevice, 1, 1);
            t.SetData(new[] { Color.White });
            spriteBatch.Draw(t, intersection, Color.Black);

            spriteBatch.End();



        }
    }
}
