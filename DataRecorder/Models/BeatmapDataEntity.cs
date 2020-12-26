using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRecorder.Models
{
    public class BeatmapDataEntity
    {
		// TODO: コメントのsummary化

		/// <summary>
		/// 曲名
		/// </summary>
		public string songName { get; set; } = null;

		/// <summary>
		/// 曲のサブタイトル
		/// </summary>
		public string songSubName { get; set; } = null;

		public string songAuthorName { get; set; } = null;                   //曲の作者
		public string levelAuthorName { get; set; } = null;                  //譜面の作者
		public string songHash { get; set; } = null;                         //譜面ID(SHA-1ハッシュ値)
		public string levelId { get; set; } = null;                          //譜面のRawレベル。全て難易度で同じ
		public float songBPM { get; set; }                                  //曲のBPM
		public float noteJumpSpeed { get; set; }                            //譜面のNJS
		public long songTimeOffset { get; set; } = 0;                        //譜面開始オフセット値(取得出来ていない？)
		public long length { get; set; } = 0;                                //譜面の長さ[ms]
		public long start { get; set; } = 0;                                 //譜面プレイ開始時の時間。再開時に更新。(UNIX time[ms])
		public long paused { get; set; } = 0;                                //一時停止時の時間(UNIX time[ms])
		public string difficulty { get; set; } = null;                       //譜面の難易度
		public int notesCount { get; set; } = 0;                             //譜面のノーツ数
		public int bombsCount { get; set; } = 0;                             //譜面の爆弾数
		public int obstaclesCount { get; set; } = 0;                         //譜面の壁の数
		public int maxScore { get; set; } = 0;                               //現在のModでの最大スコア
		public string maxRank { get; set; } = "E";                           //現在のModでの最大ランク
		public string environmentName { get; set; } = null;                  //譜面の要求環境
	}
}
