using System;
using DataRecorder.Enums;

namespace DataRecorder.Models
{
    /// <summary>
    /// ゲーム中のステータスを管理するクラスです。ゲーム中に1つのインスタンスしか作成されません。
    /// </summary>
    public class GameStatus
    {
        #region // プロパティ
        /// <summary>
        /// ノーツ毎のスコア格納用配列の現在のインデックス
        /// </summary>
        public int noteIndex { get; set; } = 0;

        /// <summary>
        /// エネルギー変化格納用配列の現在のインデックス
        /// </summary>
        public int energyIndex { get; set; } = 0;

        /// <summary>
        /// 譜面データ用配列の現在のインデックス
        /// </summary>
        public int mapIndex { get; set; } = 0;

        /// <summary>
        /// ノーツ毎のスコア格納用配列の最終インデックス
        /// </summary>
        public int noteEndIndex { get; set; } = 0;

        /// <summary>
        /// エネルギー変化格納用配列の最終インデックス
        /// </summary>
        public int energyEndIndex { get; set; } = 0;

        /// <summary>
        /// 譜面データ用配列の最終インデックス
        /// </summary>
        public int mapEndIndex { get; set; } = 0;

        /// <summary>
        /// 譜面開始時刻 (UNIX time[ms])
        /// </summary>
        public long startTime { get; set; } = 0;

        /// <summary>
        /// 譜面終了時刻 (UNIX time[ms])
        /// </summary>
        public long endTime { get; set; } = 0;

        /// <summary>
        /// クリア条件
        /// </summary>
        public BeatSaberEvent? cleared { get; set; } = null;

        /// <summary>
        /// 終了条件
        /// </summary>
        public int endFlag { get; set; } = 0;

        /// <summary>
        /// pause回数
        /// </summary>
        public int pauseCount { get; set; } = 0;

        /// <summary>
        /// StatusObject[Game] mode : パーティモード
        /// </summary>
        public bool partyMode { get; set; } = false;

        /// <summary>
        /// StatusObject[Game] mode : ゲームモード
        /// </summary>
        public string mode { get; set; } = null;

        /// <summary>
        /// StatusObject[Game] mode : ゲームシーン
        /// </summary>
        public BeatSaberScene? scene { get; set; } = null;

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
        public RankModel.Rank maxRank { get; set; } = RankModel.Rank.E;

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
        public RankModel.Rank rank { get; set; } = RankModel.Rank.E;

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
        public GameplayModifiers.EnabledObstacleType modObstacles { get; set; } = GameplayModifiers.EnabledObstacleType.All;

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
        public GameplayModifiers.SongSpeed modSongSpeed { get; set; } = GameplayModifiers.SongSpeed.Normal;

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

        /// <summary>
        /// noteID検索用
        /// </summary>
        public int lastNoteId { get; set; } = 0;
        #endregion
        #region // 定数
        /// <summary>
        /// noteScore配列初期化サイズ (必要な配列サイズはノーツ数＋爆弾数)
        /// </summary>
        private const int defaultNoteScoreSize = 3000;

        /// <summary>
        /// noteScore配列追加サイズ
        /// </summary>
        private const int addNoteScoreSize = 500;

        /// <summary>
        /// 譜面データ用配列初期化サイズ
        /// </summary>
        private const int defaultMapDataSize = 3000;

        /// <summary>
        /// 譜面データ用配列追加サイズ
        /// </summary>
        private const int addMapDataSize = 500;

        /// <summary>
        /// エネルギー変化格納用配列初期化サイズ
        /// </summary>
        private const int defaultEnergyDataSize = 3000;

        /// <summary>
        /// エネルギー変化格納用配列追加サイズ
        /// </summary>
        private const int addEnergyDataSize = 500;
        #endregion
        #region // メンバ変数
        /// <summary>
        /// ノーツ毎のスコア格納用配列
        /// </summary>
        private NoteDataEntity[] noteScores = new NoteDataEntity[defaultNoteScoreSize];

        /// <summary>
        /// エネルギー変化格納用配列
        /// </summary>
        private EnergyDataEntity[] energyDatas = new EnergyDataEntity[defaultEnergyDataSize];

        /// <summary>
        /// 譜面データ用配列
        /// </summary>
        private MapDataEntity[] mapDatas = new MapDataEntity[defaultMapDataSize];

        /// <summary>
        /// ノーツ毎のスコア格納用配列の初期化済み数
        /// </summary>
        private int noteScoresInitCount = 0;

        /// <summary>
        /// エネルギー変化格納配列の初期化済み数
        /// </summary>
        private int energyDataInitCount = 0;

        /// <summary>
        /// 譜面データ用配列の初期化済み数
        /// </summary>
        private int mapDatasInitCount = 0;

        #endregion
        #region // コンストラクタ
        public GameStatus()
        {
            // ノーツ・エネルギー・譜面格納変数の初期化
            for (int i = 0; i < defaultNoteScoreSize; ++i)
                this.noteScores[i] = new NoteDataEntity();
            this.noteScoresInitCount = defaultNoteScoreSize;
            for (int i = 0; i < defaultEnergyDataSize; ++i)
                this.energyDatas[i] = new EnergyDataEntity();
            this.energyDataInitCount = defaultEnergyDataSize;
            for (int i = 0; i < defaultMapDataSize; ++i)
                this.mapDatas[i] = new MapDataEntity();
            this.mapDatasInitCount = defaultMapDataSize;
        }
        #endregion
        #region // パブリックメソッド
        /// <summary>
        /// ノーツ毎のスコア格納用配列から現在のインデックスの内容を取り出す
        /// </summary>
        public NoteDataEntity NoteDataGet()
        {
            if (this.noteScores[this.noteIndex] == null) {
                this.noteScores[this.noteIndex] = new NoteDataEntity();
                this.noteScoresInitCount = this.noteIndex + 1;
            }
            return this.noteScores[this.noteIndex];
        }

        /// <summary>
        /// エネルギー変化格納用配列から現在のインデックスの内容を取り出す
        /// </summary>
        public EnergyDataEntity EnergyDataGet()
        {
            if (this.energyDatas[this.energyIndex] == null) {
                this.energyDatas[this.energyIndex] = new EnergyDataEntity();
                this.energyDataInitCount = this.energyIndex + 1;
            }
            return this.energyDatas[this.energyIndex];
        }

        /// <summary>
        /// 譜面データ用配列から現在のインデックスの内容を取り出す
        /// </summary>
        public MapDataEntity MapDataGet()
        {
            if (this.mapDatas[this.mapIndex] == null) {
                this.mapDatas[this.mapIndex] = new MapDataEntity();
                this.mapDatasInitCount = this.mapIndex + 1;
            }
            return this.mapDatas[this.mapIndex];
        }

        /// <summary>
        /// ノーツ毎のスコア格納用配列のインデックスをインクリメント
        /// </summary>
        public void NoteDataIndexUp()
        {
            var performance = this.NoteDataGet();
            performance.score = this.score;
            performance.currentMaxScore = this.currentMaxScore;
            performance.rank = this.rank;
            performance.passedNotes = this.passedNotes;
            performance.hitNotes = this.hitNotes;
            performance.missedNotes = this.missedNotes;
            performance.lastNoteScore = this.lastNoteScore;
            performance.passedBombs = this.passedBombs;
            performance.hitBombs = this.hitBombs;
            performance.combo = this.combo;
            performance.maxCombo = this.maxCombo;
            performance.multiplier = this.multiplier;
            performance.multiplierProgress = this.multiplierProgress;
            performance.batteryEnergy = this.batteryEnergy;
            this.noteIndex++;
            if (this.noteIndex >= this.noteScores.Length)
                this.NoteDataResize(this.noteScores.Length);
            this.noteEndIndex++;
        }

        /// <summary>
        /// エネルギー変化格納用配列のインデックスをインクリメント
        /// </summary>
        public void EnergyDataIndexUp()
        {
            this.energyIndex++;
            if (this.energyIndex >= this.energyDatas.Length)
                this.EnergyDataResize(this.energyDatas.Length);
            this.energyEndIndex++;
        }

        /// <summary>
        /// 譜面データ用配列のインデックスをインクリメント
        /// </summary>
        public void MapDataIndexUp()
        {
            this.mapIndex++;
            if (this.mapIndex >= this.mapDatas.Length)
                this.MapDataResize(this.mapDatas.Length);
            this.mapEndIndex++;
        }
        /// <summary>
        /// ノーツ毎のスコア格納用配列のサイズ確認
        /// </summary>
        public void NoteDataSizeCheck()
        {
            if (this.notesCount + this.bombsCount >= this.noteScores.Length) {
                this.NoteDataResize(this.notesCount + this.bombsCount);
                while (this.noteScoresInitCount < this.noteScores.Length) {
                    if (this.noteScores[this.noteScoresInitCount] == null) {
                        this.noteScores[this.noteScoresInitCount] = new NoteDataEntity();
                    }
                    this.noteScoresInitCount++;
                }
            }
        }
        /// <summary>
        /// エネルギー変化格納用配列のサイズ確認
        /// </summary>
        public void EnergyDataSizeCheck()
        {
            if (this.notesCount + this.bombsCount >= this.energyDatas.Length) {
                this.EnergyDataResize(this.notesCount + this.bombsCount);
                while (this.energyDataInitCount < this.energyDatas.Length) {
                    if (this.energyDatas[this.energyDataInitCount] == null) {
                        this.energyDatas[this.energyDataInitCount] = new EnergyDataEntity();
                    }
                    this.energyDataInitCount++;
                }
            }
        }
        /// <summary>
        /// 譜面データ配列のサイズ確認
        /// </summary>
        public void MapDataSizeCheck()
        {
            if (this.notesCount + this.bombsCount >= this.mapDatas.Length) {
                this.MapDataResize(this.notesCount + this.bombsCount);
                while (this.mapDatasInitCount < this.mapDatas.Length) {
                    if (this.mapDatas[this.mapDatasInitCount] == null) {
                        this.mapDatas[this.mapDatasInitCount] = new MapDataEntity();
                    }
                    this.mapDatasInitCount++;
                }
            }
        }
        /// <summary>
        /// GameStatusのリセット
        /// </summary>
        public void ResetGameStatus()
        {
            this.startTime = 0;
            this.endTime = 0;
            this.cleared = null;
            this.endFlag = 0;
            this.pauseCount = 0;
            this.partyMode = false;
            this.mode = null;
            this.scene = null;
            this.songName = null;
            this.songSubName = null;
            this.songAuthorName = null;
            this.levelAuthorName = null;
            this.songHash = null;
            this.levelId = null;
            this.songBPM = 0;
            this.noteJumpSpeed = 0;
            this.songTimeOffset = 0;
            this.length = 0;
            this.start = 0;
            this.paused = 0;
            this.difficulty = null;
            this.notesCount = 0;
            this.bombsCount = 0;
            this.obstaclesCount = 0;
            this.maxScore = 0;
            this.maxRank = RankModel.Rank.E;
            this.environmentName = null;
            this.score = 0;
            this.currentMaxScore = 0;
            this.rank = RankModel.Rank.E;
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
            this.modifierMultiplier = 1f;
            this.modObstacles = GameplayModifiers.EnabledObstacleType.All;
            this.modInstaFail = false;
            this.modNoFail = false;
            this.modBatteryEnergy = false;
            this.batteryLives = 1;
            this.modDisappearingArrows = false;
            this.modNoBombs = false;
            this.modSongSpeed = GameplayModifiers.SongSpeed.Normal;
            this.songSpeedMultiplier = 1f;
            this.modNoArrows = false;
            this.modGhostNotes = false;
            this.modFailOnSaberClash = false;
            this.modStrictAngles = false;
            this.modFastNotes = false;
            this.staticLights = false;
            this.leftHanded = false;
            this.playerHeight = 1.7f;
            this.sfxVolume = 0.7f;
            this.reduceDebris = false;
            this.noHUD = false;
            this.advancedHUD = false;
            this.autoRestart = false;
            this.ResetNoteCut();
            this.ResetEnergy();
            this.ResetMap();
        }
        /// <summary>
        /// 現在のインデックスのNoteIDを取得する
        /// </summary>
        public int GetNoteId()
        {
            // Backwards compatibility for <1.12.1
            int noteID = -1;
            // Check the near notes first for performance
            for (int i = Math.Max(0, this.lastNoteId - 10); i < this.mapDatas.Length; i++) {
                if (NoteDataEquals(this.mapDatas[i], this.noteScores[noteIndex], this.modNoArrows)) {
                    noteID = i;
                    if (i > this.lastNoteId) this.lastNoteId = i;
                    break;
                }
            }
            // If that failed, check the rest of the notes in reverse order
            if (noteID == -1) {
                for (int i = Math.Max(0, this.lastNoteId - 11); i >= 0; i--) {
                    if (NoteDataEquals(this.mapDatas[i], this.noteScores[noteIndex], this.modNoArrows)) {
                        noteID = i;
                        break;
                    }
                }
            }
            return noteID;
        }
        #endregion
        #region // プライベートメソッド
        /// <summary>
        /// ノーツ毎のスコア格納用配列をリセット
        /// </summary>
        private void ResetNoteCut()
        {
            for (int i = 0; i < this.noteEndIndex; i++) {
                if (this.noteScores[i] == null) {
                    this.noteScores[i] = new NoteDataEntity();
                }
                else {
                    this.noteScores[i].bs_event = BeatSaberEvent.Menu;
                    this.noteScores[i].time = 0;
                    this.noteScores[i].noteTime = 0;
                    this.noteScores[i].duration = 0;
                    this.noteScores[i].cutTime = null;
                    this.noteScores[i].score = 0;
                    this.noteScores[i].currentMaxScore = 0;
                    this.noteScores[i].rank = RankModel.Rank.E;
                    this.noteScores[i].passedNotes = 0;
                    this.noteScores[i].hitNotes = 0;
                    this.noteScores[i].missedNotes = 0;
                    this.noteScores[i].lastNoteScore = 0;
                    this.noteScores[i].passedBombs = 0;
                    this.noteScores[i].hitBombs = 0;
                    this.noteScores[i].combo = 0;
                    this.noteScores[i].maxCombo = 0;
                    this.noteScores[i].multiplier = 0;
                    this.noteScores[i].multiplierProgress = 0;
                    this.noteScores[i].batteryEnergy = 1;
                    this.noteScores[i].colorType = ColorType.ColorA;
                    this.noteScores[i].noteCutDirection = null;
                    this.noteScores[i].noteLine = null;
                    this.noteScores[i].noteLayer = null;
                    this.noteScores[i].speedOK = false;
                    this.noteScores[i].directionOK = false;
                    this.noteScores[i].saberTypeOK = false;
                    this.noteScores[i].wasCutTooSoon = false;
                    this.noteScores[i].initialScore = -1;
                    this.noteScores[i].cutDistanceScore = -1;
                    this.noteScores[i].finalScore = -1;
                    this.noteScores[i].cutMultiplier = 0;
                    this.noteScores[i].saberSpeed = 0;
                    this.noteScores[i].saberDirX = 0;
                    this.noteScores[i].saberDirY = 0;
                    this.noteScores[i].saberDirZ = 0;
                    this.noteScores[i].saberType = null;
                    this.noteScores[i].swingRating = 0;
                    this.noteScores[i].swingRatingFullyCut = 0;
                    this.noteScores[i].timeDeviation = 0;
                    this.noteScores[i].cutDirectionDeviation = 0;
                    this.noteScores[i].cutPointX = 0;
                    this.noteScores[i].cutPointY = 0;
                    this.noteScores[i].cutPointZ = 0;
                    this.noteScores[i].cutNormalX = 0;
                    this.noteScores[i].cutNormalY = 0;
                    this.noteScores[i].cutNormalZ = 0;
                    this.noteScores[i].cutDistanceToCenter = 0;
                    this.noteScores[i].timeToNextBasicNote = 0;
                }
            }
            this.noteIndex = 0;
            this.noteEndIndex = 0;
            this.noteScoresInitCount = this.noteScores.Length;
        }

        /// <summary>
        /// エネルギー変化格納用配列をリセット
        /// </summary>
        private void ResetEnergy()
        {
            for (int i = 0; i < this.energyEndIndex; i++) {
                if (this.energyDatas[i] == null) {
                    this.energyDatas[i] = new EnergyDataEntity();
                }
                else {
                    this.energyDatas[i].time = 0;
                    this.energyDatas[i].energy = 0;
                }
            }
            this.energyIndex = 0;
            this.energyEndIndex = 0;
            this.energyDataInitCount = this.energyDatas.Length;
        }

        /// <summary>
        /// 譜面データ配列をリセット
        /// </summary>
        private void ResetMap()
        {
            for (int i = 0; i < this.mapEndIndex; i++) {
                if (this.mapDatas[i] == null) {
                    this.mapDatas[i] = new MapDataEntity();
                }
                else {
                    this.mapDatas[i].time = 0;
                    this.mapDatas[i].lineIndex = 0;
                    this.mapDatas[i].noteLineLayer = 0;
                    this.mapDatas[i].colorType = 0;
                    this.mapDatas[i].cutDirection = 0;
                    this.mapDatas[i].duration = 0;
                }
            }
            this.mapIndex = 0;
            this.mapEndIndex = 0;
            this.mapDatasInitCount = this.mapDatas.Length;
        }

        /// <summary>
        /// エネルギー変化格納用配列のリサイズ
        /// </summary>
        private void EnergyDataResize(int size)
        {
            Array.Resize(ref this.energyDatas, size + addEnergyDataSize);
        }

        /// <summary>
        /// ノーツ毎のスコア格納用配列のリサイズ
        /// </summary>
        private void NoteDataResize(int size)
        {
            Array.Resize(ref this.noteScores, size + addNoteScoreSize);
        }

        /// <summary>
        /// 譜面データ用配列のリサイズ
        /// </summary>
        private void MapDataResize(int size)
        {
            Array.Resize(ref this.mapDatas, size + addMapDataSize);
        }

        /// <summary>
        /// 譜面データとノーツデータを比較(noteID取得用)
        /// </summary>
        private static bool NoteDataEquals(MapDataEntity a, NoteDataEntity b, bool noArrows = false)
        {
            return a.time == b.noteTime && a.lineIndex == b.noteLine && a.noteLineLayer == b.noteLayer && a.colorType == b.colorType && (noArrows || a.cutDirection == b.noteCutDirection) && a.duration == b.duration;
        }
        #endregion
    }
}
