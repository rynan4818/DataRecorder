using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRecorder.Models
{
    public class BeatmapDataEntity
    {
		/// <summary>
		/// 曲名
		/// </summary>
		public string songName { get; set; } = null;

		/// <summary>
		/// 曲のサブタイトル
		/// </summary>
		public string songSubName { get; set; } = null;

		/// <summary>
		/// 曲の作者
		/// </summary>
		public string songAuthorName { get; set; } = null;

		/// <summary>
		/// 譜面の作者
		/// </summary>
		public string levelAuthorName { get; set; } = null;

		/// <summary>
		/// 譜面ID(SHA-1)
		/// </summary>
		public string songHash { get; set; } = null;

		/// <summary>
		/// 譜面ID(Raw)
		/// </summary>
		public string levelId { get; set; } = null;

		/// <summary>
		/// 曲のBPM
		/// </summary>
		public float songBPM { get; set; }

		/// <summary>
		/// 譜面のNJS
		/// </summary>
		public float noteJumpSpeed { get; set; }

		/// <summary>
		/// 譜面開始オフセット値(取得出来ていない？)
		/// </summary>
		public long songTimeOffset { get; set; } = 0;

		/// <summary>
		/// 譜面の長さ[ms]
		/// </summary>
		public long length { get; set; } = 0;

		/// <summary>
		/// 譜面プレイ開始時の時間。再開時に更新。(UNIX time[ms])
		/// </summary>
		public long start { get; set; } = 0;

		/// <summary>
		/// 一時停止時の時間(UNIX time[ms])
		/// </summary>
		public long paused { get; set; } = 0;

		/// <summary>
		/// 譜面の難易度
		/// </summary>
		public string difficulty { get; set; } = null;

		/// <summary>
		/// 譜面のノーツ数
		/// </summary>
		public int notesCount { get; set; } = 0;

		/// <summary>
		/// 譜面の爆弾数
		/// </summary>
		public int bombsCount { get; set; } = 0;

		/// <summary>
		/// 譜面の壁の数
		/// </summary>
		public int obstaclesCount { get; set; } = 0;

		/// <summary>
		/// 現在のModでの最大スコア
		/// </summary>
		public int maxScore { get; set; } = 0;

		/// <summary>
		/// 現在のModでの最大ランク
		/// </summary>
		public string maxRank { get; set; } = "E";

		/// <summary>
		/// 譜面の要求環境
		/// </summary>
		public string environmentName { get; set; } = null;
	}
}
