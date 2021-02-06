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
        /// <summary>
        /// 現在時刻のミリ秒単位のUNIX時間
        /// </summary>
        /// <returns>UNIXTIME[ms]</returns>
        public static long GetCurrentTime()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// プラグインのバージョン取得
        /// </summary>
        /// <returns>バージョン文字列:0.0.0</returns>
        public static string GetPluginVersion()
        {
            SemVer.Version pluginVersion = PluginManager.GetPlugin("DataRecorder").Version;
            return pluginVersion.ToString();
        }

        /// <summary>
        /// BeatSaberのゲームバージョン取得
        /// </summary>
        /// <returns>バージョン文字列:0.0.0</returns>
        public static string GetGameVersion()
        {
            return UnityGame.GameVersion.ToString();
        }
    }
}
