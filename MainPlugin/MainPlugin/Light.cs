using System;
using DarkRift;

namespace MainPlugin
{

    public class Light: TickObject, IDisposable
    {
        public Collider Collider;

        public static Light Create(Collider collider, Game game)
        {
            Light l = new Light();
            l.Collider = collider;
            game.Lights.Add(l);
            l.Game = game;

            return l;
        }


        public override void Dispose()
        {
            base.Dispose();
            Game.Lights.Remove(this);
        }
    }


    public class Arealight : Light
    {
        public ushort Id;

        public static Arealight CreateA(Collider collider, Game game, ushort type)
        {
            Arealight l = new Arealight();
            l.Collider = collider;
            game.Lights.Add(l);
            l.Game = game;
            l.Game.GameTick += l.Tick;

            l.Id = game.NextObjectId++;
            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(type);
            writer.Write(l.Id);
            writer.Write(collider.Transform.Position.x);
            writer.Write(collider.Transform.Position.y);

            game.SendMessageToAll(Message.Create((ushort)Tags.SpawnObject, writer));


            return l;
        }

        public override void Tick()
        {
            base.Tick();
            if (FramesAlive == 200)
            {
                Dispose();
            }
        }


        public override void Dispose()
        {
            base.Dispose();
            Game.Lights.Remove(this);
            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(Id);
            Game.SendMessageToAll(Message.Create((ushort)Tags.KillObject, writer));
        }
    }

}
