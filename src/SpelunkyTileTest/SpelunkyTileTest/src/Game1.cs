using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Input;
using TileGenerator;
using Utility;

using SpelunkyTileTest.Data;

namespace SpelunkyTileTest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Tileset texture to draw with
        /// </summary>
        Texture2D tilesetTexture;

        // Managers
        TileManager tileManager;
        TileRectangleDrawer tileRectangleDrawer;
        DebugTileRectangleDrawer tileRectangleDebugDrawer;
        EdgeDecalManager edgeDecalManager;

        /// <summary>
        /// A toggle to draw debug overlay or not
        /// </summary>
        bool debugDraw = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            int tileSize = GlobalData.tileSize;

            //Initialise grid of tiles (create an array to fill window with 64x64 sized tiles)
            tileManager = new TileManager(graphics.GraphicsDevice.Viewport.Width / (float)tileSize, graphics.GraphicsDevice.Viewport.Height / (float)tileSize, tileSize);
            tileRectangleDrawer = new TileRectangleDrawer(tileSize);
            tileRectangleDebugDrawer = new DebugTileRectangleDrawer(tileSize);
            edgeDecalManager = new EdgeDecalManager(tileSize);

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

            LibContent.LoadContent(this);

            //Load the tileset texture and plug it into the tile generators
            tilesetTexture = Content.Load<Texture2D>(GlobalData.TILESET_IMAGE_NAME);
            tileRectangleDrawer.SetTexture(tilesetTexture);
            edgeDecalManager.SetTexture(tilesetTexture);
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

            //Update the mouse manager to update mouse input
            MouseManager.Update();

            //Get the mouse position
            int mousePosX, mousePosY;
            MouseManager.GetPosition(out mousePosX, out mousePosY);

            //Get the tile position under the mouse
            int mouseTilePosX = mousePosX / 64;
            int mouseTilePosY = mousePosY / 64;

            //Left button: Add a tile
            if (MouseManager.ButtonPressed(MouseButton.LEFT))
            {
                //Add a tile under mouse
                tileManager.AddTile(mouseTilePosX, mouseTilePosY);

                //Update the tile generators (regenerate them currently)
                tileRectangleDrawer.SetTileRectangles(TileRectangleCombiner.Combine(tileManager));
                tileRectangleDebugDrawer.SetTileRectangles(TileRectangleCombiner.Combine(tileManager));

                //Create the edge decals
                edgeDecalManager.CreateEdgeDecals(tileManager.GetAllTiles(), tileManager.Width, tileManager.Height, tileManager.TileSize);
            }

            //Right button: Remove a tile
            if (MouseManager.ButtonPressed(MouseButton.RIGHT))
            {
                //Remove the tile under mouse
                tileManager.RemoveTile(mouseTilePosX, mouseTilePosY);

                //Update the tile generators (regenerate them currently)
                tileRectangleDrawer.SetTileRectangles(TileRectangleCombiner.Combine(tileManager));
                tileRectangleDebugDrawer.SetTileRectangles(TileRectangleCombiner.Combine(tileManager));

                //Create the edge decals
                edgeDecalManager.CreateEdgeDecals(tileManager.GetAllTiles(), tileManager.Width, tileManager.Height, tileManager.TileSize);
            }

            //Middle button: Toggle debug drawing
            if (MouseManager.ButtonPressed(MouseButton.MIDDLE))
            {
                debugDraw = !debugDraw;
            }

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

            //Draw the tiles
            tileRectangleDrawer.Draw(spriteBatch);

            //Draw the debug overlay if applicable
            if (debugDraw)
            {
                tileRectangleDebugDrawer.Draw(spriteBatch);
            }

            //Draw the edge decals
            edgeDecalManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
