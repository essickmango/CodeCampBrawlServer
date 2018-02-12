using System;

namespace MainPlugin
{

    public class TickObject: IDisposable
    {

        public Game Game;
        public uint FramesAlive;

        public virtual void Tick()
        {
            FramesAlive++;
        }

        public static TickObject Create(Game game)
        {
            TickObject t = new TickObject();
            t.Game = game;
            t.Game.GameTick += t.Tick;

            return t;
        }


        public virtual void Dispose()
        {
            Game.GameTick -= Tick;
        }
    }

}
