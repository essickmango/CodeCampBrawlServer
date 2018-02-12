using System;
using System.Diagnostics;
using System.Threading;

namespace MainPlugin
{
    public static class Clock
    {

        public static bool QuitRequested;
        public delegate void Background();
        public static event Background Tick;

        public const float DeltaTime = 0.025f;
        public const int DeltaTimeMili = 25;

        private static AutoResetEvent timerEvent = new AutoResetEvent(true);

        public static void StartBackgroundloop()
        {
            //framerate displayer
            double avg = 0;
            double max = 0;
            short count = 0;

            double lastFrame;
            double elapsed;
            double delta;

            Stopwatch timer = Stopwatch.StartNew();
            timer.Start();

            while (!QuitRequested)
            {

                lastFrame = timer.Elapsed.TotalMilliseconds;
                if (Tick != null) Tick();
                elapsed = timer.Elapsed.TotalMilliseconds;
                delta = elapsed - lastFrame;
                if (delta < DeltaTimeMili)
                {
                    timerEvent.WaitOne((int)(DeltaTimeMili - delta));
                }

                //framerate displayer
                count++;
                avg += delta;
                max = Math.Max(max, delta);
                if (count == 400)
                {
                    Server.Instance.Log("Avg Time in last 400 frames:" + (avg / count).ToString("0.#####"));
                    Server.Instance.Log("Max: " + max.ToString("0.#####"));
                    count = 0;
                    avg = 0;
                    max = 0;

                }


            }
        }

    
}
}
