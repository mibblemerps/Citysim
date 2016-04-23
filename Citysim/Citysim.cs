﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using Citysim.Map;
using Citysim.Views;
using Citysim.Map.Tiles;
using Citysim.Map.Generators;

namespace Citysim
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Citysim : Game
    {
        public const string VERSION = "0.0.1+alpha";

        public static Citysim instance;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public bool debug = false;

        public SpriteFont font;
        public SpriteFont gameFont;

        public ViewRegistry viewRegistry;
        public TileRegistry tileRegistry;
        public Generator generator;

        public Camera camera = new Camera();

        public City city;

        public Citysim()
        {
            instance = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            IsMouseVisible = true;

            // Tile registry.
            tileRegistry = new TileRegistry();
            Tile.Register(tileRegistry); // register game tiles

            // Generator
            generator = new Generator();
            Generator.RegisterGenerators(generator); // register game generators

            // View registry.
            viewRegistry = new ViewRegistry();
            ViewRegistry.RegisterViews(viewRegistry); // register game views

            // Load blank city
            city = new City();
            city.world = generator.Generate(city.world, 100, 100, new Random());
            
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load font.
            font = Content.Load<SpriteFont>("arial");

            // Load tile textures
            tileRegistry.LoadTextures();
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            // Update keyboard helper
            KeyboardHelper.Update();

            if (KeyboardHelper.IsKeyDown(Keys.Escape))
                Exit();

            // Update views
            viewRegistry.Update(this, gameTime);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Render views
            viewRegistry.Render(this, spriteBatch, gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}