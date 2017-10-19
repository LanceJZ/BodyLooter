using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XnaModel = Microsoft.Xna.Framework.Graphics.Model;
using Engine;

namespace BodyLooter
{
    using EServices = Engine.Services;

    public class Game1 : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        Entities.Player Player;
        Entities.Background Background;
        Entities.EnemyControl Enemies;
        Entities.People People;

        Words[] GameOver = new Words[2];

        /// <summary>
        /// This game is for Dreamhack-jam. Apocalypse.
        /// The player controls a drone, and finds supplies on dead bodies left over from the apocalypse.
        /// The player drives the drone, side scrolling, over debris, and battles other drones.
        /// The player has no weapons, they must avoid the air drones.
        /// The air drones fire at player, and can shoot each other, as well as run into each other.
        /// </summary>
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.IsFullScreen = false;
            Graphics.SynchronizeWithVerticalRetrace = true;
            Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.PreferredBackBufferWidth = 1200;
            Graphics.PreferredBackBufferHeight = 900;
            Graphics.PreferMultiSampling = true; //Error in MonoGame 3.6 for DirectX, fixed for dev version.
            Graphics.PreparingDeviceSettings += SetMultiSampling;
            Graphics.ApplyChanges();
            Graphics.GraphicsDevice.RasterizerState = new RasterizerState(); //Must be after Apply Changes.
            IsFixedTimeStep = false;
            Content.RootDirectory = "Content";

            Player = new Entities.Player(this);
            Background = new Entities.Background(this, Player);
            Enemies = new Entities.EnemyControl(this, Player);
            People = new Entities.People(this, Player);

            GameOver[0] = new Words(this);
            GameOver[1] = new Words(this);
        }

        private void SetMultiSampling(object sender, PreparingDeviceSettingsEventArgs eventArgs)
        {
            PresentationParameters PresentParm = eventArgs.GraphicsDeviceInformation.PresentationParameters;
            PresentParm.MultiSampleCount = 8;
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Positive Y is Up. Positive X is Right.
            EServices.Initialize(Graphics, this, new Vector3(0, 0, 200), 0, 1000);
            // Setup lighting.
            EServices.DefuseLight = new Vector3(0.6f, 0.5f, 0.7f);
            EServices.LightDirection = new Vector3(-0.75f, -0.75f, -0.5f);
            EServices.SpecularColor = new Vector3(0.1f, 0, 0.5f);
            EServices.AmbientLightColor = new Vector3(0.25f, 0.25f, 0.25f); // Add some overall ambient light.

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void BeginRun()
        {
            EServices.BeginRun(); //This only happens once in a game.

            GameOver[0].ProcessWords("GAME", new Vector3(-400, 50, 100), 9.5f);
            GameOver[0].ShowWords(false);
            GameOver[1].ProcessWords("OVER", new Vector3(100, 50, 100), 9.5f);
            GameOver[1].ShowWords(false);

            base.BeginRun();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Player.Hit)
            {
                Player.Active = false;
                Player.Hit = false;

                GameOver[0].Position.X = EServices.Camera.Position.X - 400;
                GameOver[0].UpdatePosition();
                GameOver[0].ShowWords(true);
                GameOver[1].Position.X = EServices.Camera.Position.X + 100;
                GameOver[1].UpdatePosition();
                GameOver[1].ShowWords(true);
            }

            if (!Player.Active)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.N))
                {
                    NewGame();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        void NewGame()
        {
            Player.NewGame();
            Enemies.NewGame();
            People.NewGame();
            Background.NewGame();
            GameOver[0].ShowWords(false);
            GameOver[1].ShowWords(false);
        }
    }
}
