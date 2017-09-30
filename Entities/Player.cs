using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using XnaModel = Microsoft.Xna.Framework.Graphics.Model;
using System.Collections.Generic;
using System;
using Engine;

namespace BodyLooter.Entities
{
    using Mod = AModel;

    public class Player : Mod
    {
        public List<PositionedObject> Ground = new List<PositionedObject>();

        KeyboardState KeyState;
        KeyboardState KeyStateOld;

        public Player(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            //Scale = 2;
            Position.Y = -150;
            Radius = 13;
            HeightWidth = new Vector2(26, 12);

            base.Initialize();
        }

        public override void LoadContent()
        {
            SetModel(Game.Content.Load<XnaModel>("Drone"));
        }

        public override void BeginRun()
        {

            base.BeginRun();
        }

        public override void Update(GameTime gameTime)
        {
            Services.Camera.Position.X = Position.X;

            CheckOnGround();
            GetInput();

            base.Update(gameTime);
        }

        void CheckOnGround()
        {
            Acceleration.Y = -200;

            foreach(PositionedObject block in Ground)
            {
                if (CirclesIntersect(block))
                {
                    if (BoundingBox.Intersects(block.BoundingBox))
                    {
                        if(Position.Y + 2 < block.BoundingBox.Y + block.BoundingBox.Height)
                        {
                            Acceleration.Y = 100;
                        }
                        else
                        {
                            Acceleration.Y = 0;
                            Velocity.Y = 0;
                        }
                    }
                }
            }
        }

        void GetInput()
        {
            KeyState = Keyboard.GetState();
            //Check keys after this. --------------

            if (KeyState.IsKeyDown(Keys.Left))
            {

                Velocity.X = -40;
            }
            else if (KeyState.IsKeyDown(Keys.Right))
            {

                Velocity.X = 40;
            }
            else
            {
                Velocity.X = 0;
            }

            //Check keys before this. ----------------
            KeyStateOld = KeyState;
        }
    }
}
