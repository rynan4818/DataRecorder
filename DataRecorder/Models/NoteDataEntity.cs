using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRecorder.Models
{
	/// <summary>
	/// ノーツカット格納情報
	/// </summary>
	public class NoteDataEntity
	{
		/// <summary>
		/// [Game] イベント名
		/// </summary>
		public string bs_event { get; set; } = "";

		/// <summary>
		/// [Game] イベント発生時間(UNIX time[ms])
		/// </summary>
		public long time { get; set; } = 0;

		/// <summary>
		/// [Game] noteCut時間
		/// </summary>
		public long? cutTime { get; set; } = null;

		/// <summary>
		/// [Performance] 現在のスコア
		/// </summary>
		public int score { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在処理したノーツで達成可能な最高スコア
		/// </summary>
		public int currentMaxScore { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在のランク
		/// </summary>
		public string rank { get; set; } = "E";

		/// <summary>
		/// [Performance] 現在処理したノーツ数
		/// </summary>
		public int passedNotes { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在のヒットしたノーツ数
		/// </summary>
		public int hitNotes { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在のミスしたノーツ・爆弾数
		/// </summary>
		public int missedNotes { get; set; } = 0;

		/// <summary>
		/// [Performance] 未実装
		/// </summary>
		public int lastNoteScore { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在処理した爆弾数
		/// </summary>
		public int passedBombs { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在のヒットした爆弾数
		/// </summary>
		public int hitBombs { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在のコンボ数
		/// </summary>
		public int combo { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在取得した最大コンボ数
		/// </summary>
		public int maxCombo { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在のコンボ乗数[1,2,4,8]
		/// </summary>
		public int multiplier { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在のコンボ乗数の進行[0..1]
		/// </summary>
		public float multiplierProgress { get; set; } = 0;

		/// <summary>
		/// [Performance] 現在のバッテリー寿命の残り。バッテリーエネルギーとインスタ障害が無効になっている場合はnull。
		/// </summary>
		public int? batteryEnergy { get; set; } = 1;

		/// <summary>
		/// [NoteCut] ノーツ番号
		/// </summary>
		public int? noteID { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツの種類 "NoteA" | "NoteB" | "GhostNote" | "Bomb"
		/// </summary>
		public string noteType { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツの矢印種類 "Up" | "Down" | "Left" | "Right" | "UpLeft" | "UpRight" | "DownLeft" | "DownRight" | "Any" | "None"
		/// </summary>
		public string noteCutDirection { get; set; } = null;

		/// <summary>
		/// [NoteCut] 左から右へのノーツの水平位置[0..3]
		/// </summary>
		public int? noteLine { get; set; } = null;

		/// <summary>
		/// [NoteCut] 下から上へのノーツの垂直位置[0..2]
		/// </summary>
		public int? noteLayer { get; set; } = null;

		/// <summary>
		/// [NoteCut] カット速度は十分に速かった
		/// </summary>
		public bool? speedOK { get; set; } = null;

		/// <summary>
		/// [NoteCut] 正しい方向でノーツがカットされた。爆弾の場合はnull。
		/// </summary>
		public bool? directionOK { get; set; } = null;

		/// <summary>
		/// [NoteCut] 正しいセイバーでノーツがカットされた。爆弾の場合はnull。
		/// </summary>
		public bool? saberTypeOK { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツのカットが早すぎる
		/// </summary>
		public bool? wasCutTooSoon { get; set; } = null;

		/// <summary>
		/// [NoteCut] カット前のスイングのスコアとノーツ中心カットの合計[max85]。爆弾の場合はnull。
		/// </summary>
		public int? initialScore { get; set; } = null;

		/// <summary>
		/// [NoteCut] カット前のスイングのスコア[max70]。爆弾の場合はnull。
		/// </summary>
		public int? beforeScore { get; set; } = null;

		/// <summary>
		/// [NoteCut] カット後のスイングのスコア[max30]。爆弾の場合はnull。
		/// </summary>
		public int? afterScore { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツ中心カットのスコア[max15]。  爆弾の場合はnull。
		/// </summary>
		public int? cutDistanceScore { get; set; } = null;

		/// <summary>
		/// [NoteCut] カット全体の乗数なしのスコア。爆弾の場合はnull。
		/// </summary>
		public int? finalScore { get; set; } = null;

		/// <summary>
		/// [NoteCut] カット時のコンボ乗数
		/// </summary>
		public int? cutMultiplier { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツがカットされたときのセイバーの速度
		/// </summary>
		public float? saberSpeed { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツがカットされたときにセイバーが移動した方向[X]
		/// </summary>
		public float? saberDirX { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツがカットされたときにセイバーが移動した方向[Y]
		/// </summary>
		public float? saberDirY { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツがカットされたときにセイバーが移動した方向[Z]
		/// </summary>
		public float? saberDirZ { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツをカットするために使用されるセイバー "SaberA" | "SaberB"
		/// </summary>
		public string saberType { get; set; } = null;

		/// <summary>
		/// [NoteCut] ゲームのスイング評価。カット前の評価。爆弾の場合は-1。
		/// </summary>
		public float? swingRating { get; set; } = null;

		/// <summary>
		/// [NoteCut] ゲームのスイング評価。カット後の評価を使用します。爆弾の場合は-1。
		/// </summary>
		public float? swingRatingFullyCut { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツをカットするのに最適な時間からの秒単位の時間オフセット
		/// </summary>
		public float? timeDeviation { get; set; } = null;

		/// <summary>
		/// [NoteCut] 度単位の完全なカット角度からのオフセット
		/// </summary>
		public float? cutDirectionDeviation { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツの中心に最も近いカット平面上のポイントの位置[X]
		/// </summary>
		public float? cutPointX { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツの中心に最も近いカット平面上のポイントの位置[Y]
		/// </summary>
		public float? cutPointY { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツの中心に最も近いカット平面上のポイントの位置[Z]
		/// </summary>
		public float? cutPointZ { get; set; } = null;

		/// <summary>
		/// [NoteCut] カットする理想的な平面の法線[X]
		/// </summary>
		public float? cutNormalX { get; set; } = null;

		/// <summary>
		/// [NoteCut] カットする理想的な平面の法線[Y]
		/// </summary>
		public float? cutNormalY { get; set; } = null;

		/// <summary>
		/// [NoteCut] カットする理想的な平面の法線[Z]
		/// </summary>
		public float? cutNormalZ { get; set; } = null;

		/// <summary>
		/// [NoteCut] ノーツの中心からカット平面までの距離
		/// </summary>
		public float? cutDistanceToCenter { get; set; } = null;

		/// <summary>
		/// [NoteCut] 次のノーツまでの時間（秒）
		/// </summary>
		public float? timeToNextBasicNote { get; set; } = null;
	}
	/// <summary>
	/// NoteWasCut保持内容
	/// </summary>
	public class NoteWasCutDataEntity
	{
		/// <summary>
		/// NoteData格納用
		/// </summary>
		public NoteData noteData { get; set; }

		/// <summary>
		/// イベント発生時間(UNIX time[ms])
		/// </summary>
		public long time { get; set; } = 0;
	}
	/// <summary>
	/// エネルギー変化保持内容
	/// </summary>
	public class EnergyDataEntity
	{
		/// <summary>
		/// イベント発生時間(UNIX time[ms])
		/// </summary>
		public long time { get; set; } = 0;

		/// <summary>
		/// エネルギー値
		/// </summary>
		public float energy { get; set; } = 0;
	}
}
