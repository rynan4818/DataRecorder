using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRecorder.Models
{
    public class NoteDataEntity
    {
		public string bs_event { get; set; } = "";               //イベント名
		public long time { get; set; } = 0;                      //イベント発生時間(UNIX time[ms])
		public long? cutTime { get; set; } = null;               //noteCut時間
																 // Performance
		public int score { get; set; } = 0;                      //現在のスコア
		public int currentMaxScore { get; set; } = 0;            //現在処理したノーツで達成可能な最高スコア
		public string rank { get; set; } = "E";                  //現在のランク
		public int passedNotes { get; set; } = 0;                //現在処理したノーツ数
		public int hitNotes { get; set; } = 0;                   //現在のヒットしたノーツ数
		public int missedNotes { get; set; } = 0;                //現在のミスしたノーツ・爆弾数
		public int lastNoteScore { get; set; } = 0;              //未実装
		public int passedBombs { get; set; } = 0;                //現在処理した爆弾数
		public int hitBombs { get; set; } = 0;                   //現在のヒットした爆弾数
		public int combo { get; set; } = 0;                      //現在のコンボ数
		public int maxCombo { get; set; } = 0;                   //現在取得した最大コンボ数
		public int multiplier { get; set; } = 0;                 //現在のコンボ乗数[1,2,4,8]
		public float multiplierProgress { get; set; } = 0;       //現在のコンボ乗数の進行[0..1]
		public int? batteryEnergy { get; set; } = 1;             //現在のバッテリー寿命の残り。バッテリーエネルギーとインスタ障害が無効になっている場合はnull。

		// NoteCut
		public int? noteID { get; set; } = null;                 //ノーツ番号
		public string noteType { get; set; } = null;             //"NoteA" | "NoteB" | "GhostNote" | "Bomb" //ノーツの種類
		public string noteCutDirection { get; set; } = null;     //"Up" | "Down" | "Left" | "Right" | "UpLeft" | "UpRight" | "DownLeft" | "DownRight" | "Any" | "None" //ノーツの矢印種類
		public int? noteLine { get; set; } = null;               //左から右へのノーツの水平位置[0..3] 
		public int? noteLayer { get; set; } = null;              //下から上へのノーツの垂直位置[0..2] 
		public bool? speedOK { get; set; } = null;               //カット速度は十分に速かった
		public bool? directionOK { get; set; } = null;           //正しい方向でノーツがカットされた。爆弾の場合はnull。
		public bool? saberTypeOK { get; set; } = null;           //正しいセイバーでノーツがカットされた。爆弾の場合はnull。
		public bool? wasCutTooSoon { get; set; } = null;         //ノーツのカットが早すぎる
		public int? initialScore { get; set; } = null;           //カット前のスイングのスコアとノーツ中心カットの合計[max85]。爆弾の場合はnull。
		public int? beforeScore { get; set; } = null;            //カット前のスイングのスコア[max70]。爆弾の場合はnull。
		public int? afterScore { get; set; } = null;             //カット後のスイングのスコア[max30]。爆弾の場合はnull。
		public int? cutDistanceScore { get; set; } = null;       //ノーツ中心カットのスコア[max15]。  爆弾の場合はnull。
		public int? finalScore { get; set; } = null;             //カット全体の乗数なしのスコア。爆弾の場合はnull。
		public int? cutMultiplier { get; set; } = null;          //カット時のコンボ乗数
		public float? saberSpeed { get; set; } = null;           //ノーツがカットされたときのセイバーの速度
		public float? saberDirX { get; set; } = null;            //ノーツがカットされたときにセイバーが移動した方向[X]
		public float? saberDirY { get; set; } = null;            //同上[Y]
		public float? saberDirZ { get; set; } = null;            //同上[Z]
		public string saberType { get; set; } = null;            //"SaberA" | "SaberB" 、 このノーツをカットするために使用されるセイバー
		public float? swingRating { get; set; } = null;          //ゲームのスイング評価。カット前の評価。爆弾の場合は-1。
		public float? swingRatingFullyCut { get; set; } = null;  //ゲームのスイング評価。カット後の評価を使用します。爆弾の場合は-1。
		public float? timeDeviation { get; set; } = null;        //ノーツをカットするのに最適な時間からの秒単位の時間オフセット
		public float? cutDirectionDeviation { get; set; } = null;//度単位の完全なカット角度からのオフセット
		public float? cutPointX { get; set; } = null;            //ノーツの中心に最も近いカット平面上のポイントの位置
		public float? cutPointY { get; set; } = null;            //同上[Y]
		public float? cutPointZ { get; set; } = null;            //同上[Z]
		public float? cutNormalX { get; set; } = null;           //カットする理想的な平面の法線
		public float? cutNormalY { get; set; } = null;           //同上[Y]
		public float? cutNormalZ { get; set; } = null;           //同上[Z]
		public float? cutDistanceToCenter { get; set; } = null;  //ノーツの中心からカット平面までの距離
		public float? timeToNextBasicNote { get; set; } = null;  //次のノーツまでの時間（秒）
	}
}
