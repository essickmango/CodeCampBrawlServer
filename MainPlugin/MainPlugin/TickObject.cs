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
        


        public virtual void Dispose()
        {
            Game.GameTick -= Tick;
        }
    }

}
