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

    public class Shot : Mod
    {
        Timer LifeTimer;

        public Shot(Game game) : base(game)
        {
            LifeTimer = new Timer(game);
        }

        public override void Initialize()
        {
            Active = false;
            Radius = 2;
            Scale = 2;

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
            if (LifeTimer.Expired)
                Active = false;

            base.Update(gameTime);
        }

        public void Spawn(Vector3 position, Vector3 velocity, float life)
        {
            Position = position;
            Velocity = velocity;
            Active = true;
            LifeTimer.Reset(life);
        }
    }
}
