using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRecorder.Util
{
    public static class Utility
    {
        public static long GetCurrentTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public static bool NoteDataEquals(NoteData a, NoteData b)
        {
            return a.time == b.time && a.lineIndex == b.lineIndex && a.noteLineLayer == b.noteLineLayer && a.colorType == b.colorType && a.cutDirection == b.cutDirection && a.duration == b.duration;
        }
    }
}
