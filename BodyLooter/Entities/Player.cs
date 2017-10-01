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

        Numbers Score;
        Words ScoreText;

        int Points;

        public Player(Game game) : base(game)
        {
            Score = new Numbers(game);
            ScoreText = new Words(game);
        }

        public override void Initialize()
        {
            Scale = 2;
            Position.Y = -180;
            Radius = 13;
            WidthHeight = new Vector2(26, 12);

            Services.AddBeginable(this);
            base.Initialize();
        }

        public override void LoadContent()
        {
            SetModel(Game.Content.Load<XnaModel>("Drone"));
        }

        public override void BeginRun()
        {
            Score.ProcessNumber(Points, new Vector3(0, 400, 100), 4);
            ScoreText.ProcessWords("BODIES", new Vector3(0, 410, 100), 2);

            base.BeginRun();
        }

        public override void Update(GameTime gameTime)
        {
            if (Active)
            {
                Services.Camera.Position.X = Position.X;

                Score.Position.X = Position.X;
                Score.UpdatePosition();

                ScoreText.Position.X = Position.X - 180;
                ScoreText.UpdatePosition();

                CheckOnGround();
                GetInput();
            }

            base.Update(gameTime);
        }

        public void AddBody()
        {
            Points++;

            Score.UpdateNumber(Points);
        }

        void CheckOnGround()
        {
            Acceleration.Y = -100;

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
                if (Position.X > -1195)
                    Velocity.X = -50;
                else
                    Velocity.X = 0;
            }
            else if (KeyState.IsKeyDown(Keys.Right))
            {
                if (Position.X < 1199)
                    Velocity.X = 50;
                else
                    Velocity.X = 0;
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
