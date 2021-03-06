﻿using Microsoft.Xna.Framework;
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

    enum CurrentHeading
    {
        Left,
        Right
    }

    public class AirDrone : Mod
    {
        public SoundEffectInstance EngineSound;
        public SoundEffect GunShotSound;

        Player PlayerRef;
        public Shot GunShot;

        CurrentHeading Heading;

        float Distance;

        public AirDrone(Game game, Player player) : base(game)
        {
            PlayerRef = player;
            GunShot = new Shot(game);
        }

        public override void Initialize()
        {
            Scale = 2;

            Distance = Services.RandomMinMax(50, 100);

            base.Initialize();
        }

        public override void LoadContent()
        {

        }

        public override void BeginRun()
        {

            base.BeginRun();
        }

        public override void Update(GameTime gameTime)
        {
            switch (Heading)
            {
                case CurrentHeading.Left:
                    Velocity.X = -100;

                    if (Position.X + Distance < PlayerRef.Position.X)
                    {
                        Heading = CurrentHeading.Right;
                        Distance = Services.RandomMinMax(150, 300);
                    }

                    break;

                case CurrentHeading.Right:
                    Velocity.X = 100;

                    if (Position.X - Distance > PlayerRef.Position.X)
                    {
                        Heading = CurrentHeading.Left;
                        Distance = Services.RandomMinMax(150, 300);
                    }

                    break;
            }

            if (PlayerRef.Active)
            {
                if (Position.X + 10 > PlayerRef.Position.X && Position.X - 10 < PlayerRef.Position.X)
                {
                    FireShot();
                }
            }

            CheckPlayerHit();

            base.Update(gameTime);
        }

        public void Spawn()
        {
            Position.X = 1200;
            EngineSound.Volume = 0.05f;
            EngineSound.IsLooped = true;
            EngineSound.Play();
            Active = true;
        }

        void CheckPlayerHit()
        {
            if (GunShot.Active)
            {
                if (GunShot.CirclesIntersect(PlayerRef))
                {
                    PlayerRef.Hit = true;
                    PlayerRef.Explode.Play(0.25f, 0.5f, 0);
                }
            }
        }

        void FireShot()
        {
            if (!GunShot.Active)
            {
                GunShot.Spawn(Position, new Vector3(0, -210, 0), 1.35f);
                GunShotSound.Play(0.15f, 0, 0);
            }
        }
    }
}
