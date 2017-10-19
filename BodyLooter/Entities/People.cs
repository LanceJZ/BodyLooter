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
    using Mod = Engine.AModel;

    public class People : GameComponent, IBeginable, IUpdateableComponent, ILoadContent
    {
        Player PlayerRef;
        XnaModel BodyModel;
        List<Person> Bodies = new List<Person>();

        public People(Game game, Player player) : base(game)
        {
            PlayerRef = player;
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            Services.AddBeginable(this);
            Services.AddLoadable(this);

            for (int i = 0; i < 5; i++)
            {
                Bodies.Add(new Person(Game, PlayerRef));
            }

            base.Initialize();
        }

        public void LoadContent()
        {
            BodyModel = PlayerRef.Load("Body");

            foreach (Person body in Bodies)
            {
                body.LoadContent();
                body.SetModel(BodyModel);
            }
        }

        public void BeginRun()
        {
            NewGame();
        }

        public override void Update(GameTime gameTime)
        {
            int count = 0;

            foreach(Mod body in Bodies)
            {
                if (body.Active)
                    count++;
            }

            if (count < 2)
            {
                SpawnBody();
            }

            base.Update(gameTime);
        }

        public void NewGame()
        {
            int spot = 0;

            float start = Services.RandomMinMax(-1000, -800);

            foreach (Person body in Bodies)
            {
                body.Position = new Vector3(start + (spot * Services.RandomMinMax(350, 450)), 450, 0);
                body.NewGame();
                spot++;
            }
        }

        void SpawnBody()
        {
            foreach(Person body in Bodies)
            {
                if (!body.Active)
                {
                    body.Position = new Vector3(Services.RandomMinMax(-1000, 1000), 450, 0);
                    body.NewGame();
                    break;
                }
            }
        }
    }
}
