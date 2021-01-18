using System;

namespace DataRecorder.Models
{
    /// <summary>
    /// HTTPStatusからまるこぴ
    /// ゲーム中のステータスを管理するクラスです。ゲーム中に1つのインスタンスしか作成されません。
    /// </summary>
    public class GameStatus
    {
        // プロパティ
        /// <summary>
        /// StatusObject[Game] mode : パーティモード
        /// </summary>
        public bool partyMode { get; set; } = false;

        /// <summary>
        /// StatusObject[Game] mode : ゲームモード
        /// </summary>
        public string mode { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] 曲名
        /// </summary>
        public string songName { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] 曲のサブタイトル
        /// </summary>
        public string songSubName { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] 曲の作者
        /// </summary>
        public string songAuthorName { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] 譜面の作者
        /// </summary>
        public string levelAuthorName { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] 譜面ID(SHA-1)
        /// </summary>
        public string songHash { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] 譜面ID(Raw)
        /// </summary>
        public string levelId { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] 曲のBPM
        /// </summary>
        public float songBPM { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 譜面のNJS
        /// </summary>
        public float noteJumpSpeed { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 譜面開始オフセット値(取得出来ていない？)
        /// </summary>
        public long songTimeOffset { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 譜面の長さ[ms]
        /// </summary>
        public long length { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 譜面プレイ開始時の時間。再開時に更新。(UNIX time[ms])
        /// </summary>
        public long start { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 一時停止時の時間(UNIX time[ms])
        /// </summary>
        public long paused { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 譜面の難易度
        /// </summary>
        public string difficulty { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] 譜面のノーツ数
        /// </summary>
        public int notesCount { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 譜面の爆弾数
        /// </summary>
        public int bombsCount { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 譜面の壁の数
        /// </summary>
        public int obstaclesCount { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 現在のModでの最大スコア
        /// </summary>
        public int maxScore { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] 現在のModでの最大ランク
        /// </summary>
        public string maxRank { get; set; } = "E";

        /// <summary>
        /// StatusObject[BeatMap] 譜面の要求環境
        /// </summary>
        public string environmentName { get; set; } = null;

        /// <summary>
        /// StatusObject[Performance] 現在のスコア
        /// </summary>
        public int score { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在のノーツ数で達成可能な最大スコア
        /// </summary>
        public int currentMaxScore { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在のランク
        /// </summary>
        public string rank { get; set; } = "E";

        /// <summary>
        /// StatusObject[Performance] 現在処理したノーツ数
        /// </summary>
        public int passedNotes { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在ヒットしたノーツ数
        /// </summary>
        public int hitNotes { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在ミスしたノーツ数
        /// </summary>
        public int missedNotes { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] （未取得）
        /// </summary>
        public int lastNoteScore { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在処理した爆弾数
        /// </summary>
        public int passedBombs { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在ヒットした爆弾数
        /// </summary>
        public int hitBombs { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在のコンボ数
        /// </summary>
        public int combo { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在の最大コンボ数
        /// </summary>
        public int maxCombo { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在のコンボ乗数
        /// </summary>
        public int multiplier { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在のコンボ乗数の進行具合
        /// </summary>
        public float multiplierProgress { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] 現在のバッテリエネルギー残量
        /// </summary>
        public int batteryEnergy { get; set; } = 1;

        /// <summary>
        /// StatusObject[Performance] 現在のエネルギー残量
        /// </summary>
        public float energy { get; set; } = 0;

        /// <summary>
        /// StatusObject[Mods] Mod乗数
        /// </summary>
        public float modifierMultiplier { get; set; } = 1f;

        /// <summary>
        /// StatusObject[Mods] 壁の有無
        /// </summary>
        public string modObstacles { get; set; } = "All";

        /// <summary>
        /// StatusObject[Mods] ノーミス
        /// </summary>
        public bool modInstaFail { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] ノーファウル
        /// </summary>
        public bool modNoFail { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] バッテリエネルギー
        /// </summary>
        public bool modBatteryEnergy { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] 最大バッテリ残量(DB未記録)
        /// </summary>
        public int batteryLives { get; set; } = 1;

        /// <summary>
        /// StatusObject[Mods] 消える矢印
        /// </summary>
        public bool modDisappearingArrows { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] 爆弾無し
        /// </summary>
        public bool modNoBombs { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] 曲の速度
        /// </summary>
        public string modSongSpeed { get; set; } = "Normal";

        /// <summary>
        /// StatusObject[Mods] 曲の速度のMod乗数
        /// </summary>
        public float songSpeedMultiplier { get; set; } = 1f;

        /// <summary>
        /// StatusObject[Mods] 矢印無し
        /// </summary>
        public bool modNoArrows { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] ゴーストノーツ
        /// </summary>
        public bool modGhostNotes { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] セイバークラッシュで失敗？（Hidden)
        /// </summary>
        public bool modFailOnSaberClash { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] 厳密な角度(Hidden)
        /// </summary>
        public bool modStrictAngles { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] Does something (Hidden)
        /// </summary>
        public bool modFastNotes { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] 静的ライト
        /// </summary>
        public bool staticLights { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] 左利き
        /// </summary>
        public bool leftHanded { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] プレイヤーの高さ
        /// </summary>
        public float playerHeight { get; set; } = 1.7f;

        /// <summary>
        /// StatusObject[Player settings] ノーツカット音量
        /// </summary>
        public float sfxVolume { get; set; } = 0.7f;

        /// <summary>
        /// StatusObject[Player settings] ノーツカット時の破片有無
        /// </summary>
        public bool reduceDebris { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] テキストとHUD無し
        /// </summary>
        public bool noHUD { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] Advanced HUD
        /// </summary>
        public bool advancedHUD { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] 失敗時に自動リスタート
        /// </summary>
        public bool autoRestart { get; set; } = false;

        // 定数
        /// <summary>
        /// noteScore配列初期化サイズ (必要な配列サイズはノーツ数＋爆弾数)
        /// </summary>
        private const int initNoteScoreSize = 3000;

        /// <summary>
        /// noteScore配列追加サイズ
        /// </summary>
        private const int addNoteScoreSize = 500;

        /// <summary>
        /// エネルギー変化格納用配列初期化サイズ
        /// </summary>
        private const int initEnergyDataSize = 1000;

        /// <summary>
        /// エネルギー変化格納用配列追加サイズ
        /// </summary>
        private const int addEnergyDataSize = 500;

        // メンバ変数
        /// <summary>
        /// ノーツ毎のスコア格納用配列
        /// </summary>
        private NoteDataEntity[] noteScores = new NoteDataEntity[initNoteScoreSize];

        /// <summary>
        /// エネルギー変化格納用配列
        /// </summary>
        private EnergyDataEntity[] energyDatas = new EnergyDataEntity[initEnergyDataSize];

        /// <summary>
        /// ノーツ毎のスコア格納用配列の初期化済みサイズ
        /// </summary>
        private int initNoteSize = 0;

        /// <summary>
        /// エネルギー変化格納用配列の初期化済みサイズ
        /// </summary>
        private int initEnergySize = 0;

        // メソッド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GameStatus()
        {
            NoteInit();
            EnergyInit();
        }

        /// <summary>
        /// ノーツ毎のスコア格納用配列全体初期化
        /// </summary>
        private void NoteInit()
        {
            while (initNoteSize < noteScores.Length) {
                noteScores[initNoteSize] = new NoteDataEntity();
                initNoteSize++;
            }
        }

        /// <summary>
        /// エネルギー変化格納用配列全体初期化
        /// </summary>
        private void EnergyInit()
        {
            while (initEnergySize < energyDatas.Length) {
                energyDatas[initEnergySize] = new EnergyDataEntity();
                initEnergySize++;
            }
        }

        /// <summary>
        /// エネルギー変化格納用配列のリサイズ
        /// </summary>
        public void EnergyResize()
        {
            Array.Resize(ref energyDatas, initEnergySize + addEnergyDataSize);
        }

        /// <summary>
        /// ノーツ毎のスコア格納用配列のリサイズ
        /// </summary>
        public void NoteResize(int size)
        {
            Array.Resize(ref noteScores, size + addNoteScoreSize);
        }



        public void ResetMapInfo()
        {
            this.songName = null;
            this.songSubName = null;
            this.songAuthorName = null;
            this.levelAuthorName = null;
            this.songHash = null;
            this.levelId = null;
            this.songBPM = 0f;
            this.noteJumpSpeed = 0f;
            this.songTimeOffset = 0;
            this.length = 0;
            this.start = 0;
            this.paused = 0;
            this.difficulty = null;
            this.notesCount = 0;
            this.obstaclesCount = 0;
            this.maxScore = 0;
            this.maxRank = "E";
            this.environmentName = null;
        }

        public void ResetPerformance()
        {
            this.score = 0;
            this.currentMaxScore = 0;
            this.rank = "E";
            this.passedNotes = 0;
            this.hitNotes = 0;
            this.missedNotes = 0;
            this.lastNoteScore = 0;
            this.passedBombs = 0;
            this.hitBombs = 0;
            this.combo = 0;
            this.maxCombo = 0;
            this.multiplier = 0;
            this.multiplierProgress = 0;
            this.batteryEnergy = 1;
            this.energy = 0;
        }

        public void ResetNoteCut()
        {
            this.noteID = -1;
            this.noteType = null;
            this.noteCutDirection = null;
            this.speedOK = false;
            this.directionOK = false;
            this.saberTypeOK = false;
            this.wasCutTooSoon = false;
            this.initialScore = -1;
            this.finalScore = -1;
            this.cutDistanceScore = -1;
            this.cutMultiplier = 0;
            this.saberSpeed = 0;
            this.saberDirX = 0;
            this.saberDirY = 0;
            this.saberDirZ = 0;
            this.saberType = null;
            this.swingRating = 0;
            this.timeDeviation = 0;
            this.cutDirectionDeviation = 0;
            this.cutPointX = 0;
            this.cutPointY = 0;
            this.cutPointZ = 0;
            this.cutNormalX = 0;
            this.cutNormalY = 0;
            this.cutNormalZ = 0;
            this.cutDistanceToCenter = 0;
        }
    }
}
