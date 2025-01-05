﻿using System.IO;
using System.Runtime.CompilerServices;
using DataRecorder.DataBases;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace DataRecorder.Configuration
{
    /// <summary>
    /// 設定を管理するクラス。
    /// 値が更新されると自動でjsonが更新されます。
    /// BeatSaber内から設定いじる時もここを更新します。
    /// </summary>
    internal class PluginConfig
    {
        /// <summary>
        /// デフォルトのデータベースファイルパスです。
        /// </summary>
        public static readonly string DefaultDBFilePath = Path.Combine(IPA.Utilities.UnityGame.UserDataPath, "DataRecorder", "beatsaber.db");

        public static PluginConfig Instance { get; set; }

        /// <summary>
        /// データベースのファイルパス
        /// </summary>
        public virtual string DBFilePath { get; set; } = DefaultDBFilePath;

        /// <summary>
        /// デバッグモード
        /// </summary>
        public virtual bool DebugMode { get; set; } = false;

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            // Do stuff when the config is changed.
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(PluginConfig other)
        {
            // This instance's members populated from other
        }
    }
}