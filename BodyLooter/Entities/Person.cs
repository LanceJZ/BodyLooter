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

    public class Person : Mod
    {
        Player PlayerRef;
        Timer BodyCleanTimer;
        Numbers AmountDisplay;
        int BodyAmount = 10;
        bool IsInPlace;

        public Person(Game game, Player player) : base(game)
        {
            PlayerRef = player;
            AmountDisplay = new Numbers(game);
            BodyCleanTimer = new Timer(game);
            BodyCleanTimer.Reset(1);
        }

        public override void Initialize()
        {
            WidthHeight = new Vector2(40, 10);

            Velocity.Y = -40;
            Acceleration.Y = -100;

            base.Initialize();
        }

        public override void LoadContent()
        {
            AmountDisplay.LoadContent();
        }

        public override void BeginRun()
        {


            base.BeginRun();
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsInPlace)
            {
                foreach(PositionedObject block in PlayerRef.Ground)
                {
                    if (BoundingBox.Intersects(block.BoundingBox))
                    {
                        Velocity.Y = 0;
                        Acceleration.Y = 0;
                        IsInPlace = true;
                        AmountDisplay.ProcessNumber(BodyAmount, Position + new Vector3(0, 20, 100), 1);
                    }
                }
            }

            if (Active)
                CheckPlayerTouch();

            base.Update(gameTime);
        }

        void CheckPlayerTouch()
        {
            if (PlayerRef.Active)
            {
                if (BoundingBox.Intersects(PlayerRef.BoundingBox))
                {
                    if (BodyCleanTimer.Expired)
                    {
                        BodyCleanTimer.Reset();
                        BodyAmount--;

                        AmountDisplay.UpdateNumber(BodyAmount);

                        if (BodyAmount < 1)
                        {
                            Active = false;
                            PlayerRef.AddBody();
                            AmountDisplay.ShowNumbers(false);
                        }
                    }
                }
            }
        }
    }
}
