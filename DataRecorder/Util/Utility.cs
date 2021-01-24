using IPA.Loader;
using IPA.Utilities;
using SemVer;
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

        public static string GetPluginVersion()
        {
            SemVer.Version pluginVersion = PluginManager.GetPlugin("DataRecorder").Version;
            return pluginVersion.ToString();
        }

        public static string GetGameVersion()
        {
            return UnityGame.GameVersion.ToString();
        }
    }
}
