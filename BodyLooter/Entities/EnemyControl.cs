using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using XnaModel = Microsoft.Xna.Framework.Graphics.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using Engine;

namespace BodyLooter.Entities
{
    using Mod = AModel;

    public class EnemyControl : GameComponent, IBeginable, ILoadContent
    {
        Player PlayerRef;
        XnaModel DroneModel;
        XnaModel ShotModel;

        List<AirDrone> Drones = new List<AirDrone>();

        public EnemyControl(Game game, Player player) : base(game)
        {
            PlayerRef = player;
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            Services.AddBeginable(this);
            Services.AddLoadable(this);

            base.Initialize();
        }

        public void LoadContent()
        {
            DroneModel = Game.Content.Load<XnaModel>("AirDrone");
            ShotModel = Game.Content.Load<XnaModel>("cube");
        }

        public void BeginRun()
        {
            Drones.Add(new AirDrone(Game, PlayerRef));
            Drones.Last().SetModel(DroneModel);
            Drones.Last().GunShot.SetModel(ShotModel);
            Drones.Last().Position.X = 1200;
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
