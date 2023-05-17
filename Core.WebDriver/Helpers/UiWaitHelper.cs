using System;
using System.Diagnostics;
using System.Threading;

namespace Taf.UI.Core.Helpers
{
    public class UiWaitHelper
    {
        /// <summary>
        /// Wait for a condition to become true (within specifiied timeout)
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="timeoutInSec"></param>
        /// <param name="pollingIntInMs"></param>
        /// <returns>true - if a condition is met, else - false</returns>
        public static bool Wait(Func<bool> condition, int timeoutInSec, int pollingIntInMs=200) =>

            WaitInMs(condition, timeoutInSec * 1000, pollingIntInMs);
        

        public static bool WaitInMs(Func<bool> condition, int timeoutInMs, int pollingIntInMs = 200)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (!condition())
            {
                int sleepInMs = Math.Min(timeoutInMs - (int)stopwatch.ElapsedMilliseconds, pollingIntInMs);

                if (sleepInMs < 0)
                {
                    return false;
                }

                Thread.Sleep(sleepInMs);
            }

            return true;
        }
    }
}
