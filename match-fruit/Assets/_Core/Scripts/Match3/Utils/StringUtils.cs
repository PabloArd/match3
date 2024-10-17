using System;

namespace Match3.Utils
{
    public static class StringUtils
    {
        public static string ToTimeFormat(this long totalSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalSeconds);
            return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        }
    }
}