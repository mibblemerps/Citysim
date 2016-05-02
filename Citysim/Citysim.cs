using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using Citysim.Map;
using Citysim.Views;
using Citysim.Map.Tiles;
using Citysim.Map.Generators;
using Dongs.Suckable;

namespace Citysim
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Citysim : Game
    {
        public const string VERSION = "0.0.1+alpha";
        public Dong dong = new Dong();
        
        if(dong.sucked) {
            Console.WriteLine("dong sucked");
        } else {
            Console.WriteLine("dong not sucked");
        }

        public static Citysim instance;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public bool debug = false;

        public SpriteFont font;
        public BitmapFont gameFont;

        public ViewRegistry viewRegistry;
        public TileRegistry tileRegistry;
        public Generator generator;

        public Camera camera = new Camera();

        public City city;

        public Citysim()
        {
            instance = this;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 580;
            graphics.PreferredBackBufferWidth = 900;
            Content.RootDirectory = "Content";
        }
        
        public void ToggleFullscreen()
        {
            if (graphics.IsFullScreen)
            {
                graphics.IsFullScreen = false;
                graphics.PreferredBackBufferWidth = 900;
                graphics.PreferredBackBufferHeight = 580;
            }
            else
            {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            graphics.ApplyChanges();
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
            city.world = generator.Generate(city.world, 50, 50, new Random());
            
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load font.
            font = Content.Load<SpriteFont>("arial");
            gameFont = Content.Load<BitmapFont>("squares");

            // Load tile textures
            tileRegistry.LoadTextures();

            // Load view content
            viewRegistry.LoadContent(Content);
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

            if (KeyboardHelper.IsKeyPressed(Keys.F11))
                ToggleFullscreen();

            // Update camera
            camera.Update(this, gameTime);

            // Update views
            viewRegistry.Update(this, gameTime);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

            // Render views
            viewRegistry.Render(this, spriteBatch, gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
