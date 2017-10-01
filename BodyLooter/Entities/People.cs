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
                Bodies.Last().Position = new Vector3(Services.RandomMinMax(-1000, -500) + i
                    * Services.RandomMinMax(300, 600), 450, 0);
            }

            base.Initialize();
        }

        public void LoadContent()
        {
            BodyModel = Game.Content.Load<XnaModel>("Body");

            foreach(Person body in Bodies)
            {
                body.LoadContent();
                body.SetModel(BodyModel);
            }
        }

        public void BeginRun()
        {
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }
    }
}
