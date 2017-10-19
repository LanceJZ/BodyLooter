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

    public class Background : GameComponent, IBeginable, ILoadContent, IUpdateableComponent
    {
        Player PlayerRef;
        XnaModel GroundModel;
        XnaModel BuildingModel;
        public List<Mod> GroundBlocks = new List<Mod>();
        public List<Mod> BuildingBlocks = new List<Mod>();
        List<float> GroundHeight = new List<float>();
        public float GroundTop = -250;
        public float GroundBottom = -450;
        public float GroundLeft = -600 * 3;
        public float GroundRight = 600 * 3;
        public int GroundWidth = 24 * 3;

        public Background(Game game, Player player) : base(game)
        {
            PlayerRef = player;

            game.Components.Add(this);
        }

        public override void Initialize()
        {
            Services.AddBeginable(this);
            Services.AddLoadable(this);

            for (int i =0; i < GroundWidth; i++)
            {
                float prevous = 0;

                if (i > 0)
                {
                    prevous = GroundHeight[i - 1];
                }

                GroundHeight.Add(MathHelper.Clamp(prevous + Services.RandomMinMax(-5, 5), -25, 25));
            }

            Vector3 pos = Vector3.Zero;

            for (int i = 0; i < 6; i++)
            {
                for (int ii = 0; ii < GroundWidth; ii++)
                {
                    GroundBlocks.Add(new Mod(Game));

                    pos.X = GroundLeft + 25 + (ii * 50);
                    pos.Y = GroundBottom - 50 + (i * 50) + GroundHeight[ii];
                    pos.Z = 25;
                    GroundBlocks.Last().Position = pos;
                    GroundBlocks.Last().Moveable = false;

                    if (i == 5)
                    {
                        PlayerRef.Ground.Add(new PositionedObject(Game));
                        PlayerRef.Ground.Last().Position = pos;
                        PlayerRef.Ground.Last().Radius = 70;
                        PlayerRef.Ground.Last().WidthHeight = new Vector2(50, 55.5f);

                        BuildingBlocks.Add(new Mod(Game));
                        BuildingBlocks.Last().Position = pos + new Vector3(0, 50, -50);
                    }
                }
            }

            base.Initialize();
        }

        public void LoadContent()
        {
            GroundModel = PlayerRef.Load("GroundBlock");
            BuildingModel = PlayerRef.Load("Building");
        }

        public void BeginRun()
        {
            foreach(Mod block in GroundBlocks)
            {
                block.SetModel(GroundModel);
            }

            foreach(Mod block in BuildingBlocks)
            {
                block.SetModel(BuildingModel);
            }
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public void NewGame()
        {
            for (int i = 0; i < GroundWidth; i++)
            {
                GroundHeight[i] = 0;
            }

            for (int i = 0; i < GroundWidth; i++)
            {
                float prevous = 0;

                if (i > 0)
                {
                    prevous = GroundHeight[i - 1];
                }

                GroundHeight[i] = (MathHelper.Clamp(prevous + Services.RandomMinMax(-5, 5), -25, 25));
            }

            Vector3 pos = Vector3.Zero;

            for (int i = 0; i < 6; i++)
            {
                for (int ii = 0; ii < GroundWidth; ii++)
                {
                    pos.X = GroundLeft + 25 + (ii * 50);
                    pos.Y = GroundBottom - 50 + (i * 50) + GroundHeight[ii];
                    pos.Z = 25;

                    GroundBlocks[ii + (i * GroundWidth)].Position = pos;

                    if (i == 5)
                    {
                        PlayerRef.Ground[ii].Position = pos;
                        PlayerRef.Ground[ii].Radius = 70;
                        PlayerRef.Ground[ii].WidthHeight = new Vector2(50, 55.5f);

                        BuildingBlocks[ii].Position = pos + new Vector3(0, 50, -50);
                    }
                }
            }
        }
    }
}
