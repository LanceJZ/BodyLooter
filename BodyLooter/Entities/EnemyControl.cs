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
        SoundEffect EngineSound;
        SoundEffect GunshotSound;

        Player PlayerRef;
        XnaModel DroneModel;
        XnaModel ShotModel;

        List<AirDrone> Drones = new List<AirDrone>();

        int NextSpawn = 3;

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
            DroneModel = PlayerRef.Load("AirDrone");
            ShotModel = PlayerRef.Load("cube");
            EngineSound = PlayerRef.LoadSoundEffect("AirdroneEngine");
            GunshotSound = PlayerRef.LoadSoundEffect("GunShot");
        }

        public void BeginRun()
        {
            SpawnDrone();
        }

        public override void Update(GameTime gameTime)
        {
            if (PlayerRef.Points > NextSpawn)
            {
                SpawnDrone();
                NextSpawn += 2;
            }

            base.Update(gameTime);
        }

        public void NewGame()
        {
            foreach(AirDrone ad in Drones)
            {
                ad.Active = false;
                ad.EngineSound.Stop();
            }

            SpawnDrone();
        }

        void SpawnDrone()
        {
            bool spawnNew = true;

            foreach (AirDrone ad in Drones)
            {
                if (!ad.Active)
                {
                    ad.Spawn();
                    spawnNew = false;
                    break;
                }
            }

            if (spawnNew)
            {
                Drones.Add(new AirDrone(Game, PlayerRef));
                Drones.Last().SetModel(DroneModel);
                Drones.Last().GunShot.SetModel(ShotModel);
                Drones.Last().GunShotSound = GunshotSound;
                Drones.Last().EngineSound = EngineSound.CreateInstance();
                Drones.Last().Spawn();
            }
        }
    }
}
