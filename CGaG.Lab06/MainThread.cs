using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CGaG.Lab06 {
    public class MainThread : Game {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        Color BackColor = new Color(30, 30, 30);

        KeyboardState keyboard;
        KeyboardState keyboardPrev = Keyboard.GetState( );

        Func<float, float, float>[ ] AllFunctions = new Func<float, float, float>[ ] {
            LabUtils.F1,
            LabUtils.F2,
            LabUtils.F3,
            LabUtils.F4,
            LabUtils.F5,
        };
        uint Func = 0;
        Vector2 XRange = new Vector2(-15f, 15f);
        Vector2 YRange = new Vector2(-15f, 15f);
        Vector2 Delta = new Vector2(0.25f, 0.25f);
        Tuple<VertexPositionColor[ ], short[ ]> ToDraw = null;

        BasicEffect Effect;
        Vector3 SphereCameraPosition = new Vector3(64f, 315f, 45f);

        public MainThread( ) {
            Graphics = new GraphicsDeviceManager(this);
            Window.Position = new Point(0, 0);
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 700;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Graphics.PreparingDeviceSettings += SetMultiSampling;
        }

        private void SetMultiSampling(Object sender, PreparingDeviceSettingsEventArgs e) {
            e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 4;
        }

        private void Window_ClientSizeChanged(Object sender, EventArgs e) {
            Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            Graphics.ApplyChanges( );
        }

        protected override void Initialize( ) {
            // TODO: Add initialization logic

            Effect = new BasicEffect(Graphics.GraphicsDevice);
            Effect.World = Matrix.Identity;
            Effect.VertexColorEnabled = true;

            base.Initialize( );
        }

        protected override void LoadContent( ) {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load content
        }

        protected override void UnloadContent( ) {
            // TODO: Unload any non ContentManager content
        }

        protected override void Update(GameTime gameTime) {
            keyboard = Keyboard.GetState( );
            if (keyboardPrev == null) {
                keyboardPrev = keyboard;
            }

            if (keyboard.IsKeyDown(Keys.Escape)) {
                Exit( );
            }

            // TODO: Add update logic
            SphereCameraPosition.Y +=
            (keyboard.IsKeyDown(Keys.Left) ? 1 : 0) -
            (keyboard.IsKeyDown(Keys.Right) ? 1 : 0);
            SphereCameraPosition.Z +=
                (keyboard.IsKeyDown(Keys.Up) ? 1 : 0) -
                (keyboard.IsKeyDown(Keys.Down) ? 1 : 0);
            Utils.Median(ref SphereCameraPosition.Z, -89f, 89f);

            if (keyboard.IsKeyDown(Keys.D1) && Func != 0) {
                Func = 0;
                ToDraw = null;
            }
            if (keyboard.IsKeyDown(Keys.D2) && Func != 1) {
                Func = 1;
                ToDraw = null;
            }
            if (keyboard.IsKeyDown(Keys.D3) && Func != 2) {
                Func = 2;
                ToDraw = null;
            }
            if (keyboard.IsKeyDown(Keys.D4) && Func != 3) {
                Func = 3;
                ToDraw = null;
            }
            if (keyboard.IsKeyDown(Keys.D5) && Func != 4) {
                Func = 4;
                ToDraw = null;
            }
            if (ToDraw == null) {
                ToDraw = Utils.BuildFunction(AllFunctions[Func], XRange, YRange, Delta);
            }

            Effect.View = Matrix.CreateLookAt(SphereCameraPosition.SphereToCart( ), Vector3.Zero, Vector3.Up);
            //if (Graphics.PreferredBackBufferWidth > Graphics.PreferredBackBufferHeight) {
            //    Effect.Projection = Matrix.CreateOrthographic(SphereCameraPosition.X * Graphics.PreferredBackBufferWidth / Graphics.PreferredBackBufferHeight, SphereCameraPosition.X, 0.1f, 100.0f);
            //} else {
            //    Effect.Projection = Matrix.CreateOrthographic(SphereCameraPosition.X, SphereCameraPosition.X * Graphics.PreferredBackBufferHeight / Graphics.PreferredBackBufferWidth, 0.1f, 100.0f);
            //}
            Effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), Graphics.PreferredBackBufferWidth / Graphics.PreferredBackBufferHeight, 0.1f, 1000.0f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(BackColor);

            // TODO: Add drawing code
            RasterizerState rasterizerState1 = new RasterizerState( );
            rasterizerState1.CullMode = CullMode.None;
            Graphics.GraphicsDevice.RasterizerState = rasterizerState1;
            foreach (EffectPass pass in Effect.CurrentTechnique.Passes) {
                pass.Apply( );
                this.DrawLineList(ToDraw.Item1, ToDraw.Item2);
            }

            base.Draw(gameTime);
        }
    }
}
