using System;

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
        /// 譜面データ用配列のインデックス
        /// </summary>
        public int mapIndex { get; set; } = 0;

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
        public string cleared { get; set; } = null;

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
        public string scene { get; set; } = null;

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
        #endregion
        #region // 定数
        /// <summary>
        /// noteScore配列初期化サイズ (必要な配列サイズはノーツ数＋爆弾数)
        /// </summary>
        private const int defaultNoteScoreSize = 1; //3000 //1-24 ノーツカット記録無し版用コメントアウト

        /// <summary>
        /// noteScore配列追加サイズ
        /// </summary>
        private const int addNoteScoreSize = 500;

        /// <summary>
        /// 譜面データ用配列初期化サイズ
        /// </summary>
        private const int defaultMapDataSize = 1; //3000 //1-24 ノーツカット記録無し版用コメントアウト

        /// <summary>
        /// 譜面データ用配列追加サイズ
        /// </summary>
        private const int addMapDataSize = 500;

        /// <summary>
        /// エネルギー変化格納用配列初期化サイズ
        /// </summary>
        private const int defaultEnergyDataSize = 1; //1000 //1-24 ノーツカット記録無し版用コメントアウト

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

        #endregion
        #region // コンストラクタ
        public GameStatus()
        {
            // ノーツ・エネルギー格納変数の初期化
            for (int i = 0; i < defaultNoteScoreSize; ++i)
                noteScores[i] = new NoteDataEntity();
            for (int i = 0; i < defaultEnergyDataSize; ++i)
                energyDatas[i] = new EnergyDataEntity();
        }
        #endregion
        #region // パブリックメソッド
        /// <summary>
        /// エネルギー変化格納用配列のリサイズ
        /// </summary>
        public void EnergyDataResize()
        {
            Array.Resize(ref energyDatas, energyDatas.Length + addEnergyDataSize);
        }

        /// <summary>
        /// ノーツ毎のスコア格納用配列のリサイズ
        /// </summary>
        public void NoteDataResize(int size)
        {
            Array.Resize(ref noteScores, size + addNoteScoreSize);
        }

        /// <summary>
        /// 譜面データ用配列のリサイズ
        /// </summary>
        public void MapDataResize(int size)
        {
            Array.Resize(ref mapDatas, size + addMapDataSize);
        }

        /// <summary>
        /// ノーツ毎のスコア格納用配列から現在のインデックスの内容を取り出す
        /// </summary>
        public NoteDataEntity NoteDataGet()
        {
            if (noteScores[noteIndex] == null)
                noteScores[noteIndex] = new NoteDataEntity();
            return noteScores[noteIndex];
        }

        /// <summary>
        /// エネルギー変化格納用配列から現在のインデックスの内容を取り出す
        /// </summary>
        public EnergyDataEntity EnergyDataGet()
        {
            if (energyDatas[energyIndex] == null)
                energyDatas[energyIndex] = new EnergyDataEntity();
            return energyDatas[energyIndex];
        }

        /// <summary>
        /// 譜面データ用配列から現在のインデックスの内容を取り出す
        /// </summary>
        public MapDataEntity MapDataGet()
        {
            if (mapDatas[mapIndex] == null)
                mapDatas[mapIndex] = new MapDataEntity();
            return mapDatas[mapIndex];
        }

        /// <summary>
        /// ノーツ毎のスコア格納用配列のインデックスをインクリメント
        /// </summary>
        public void NoteDataIndexUp()
        {
            noteIndex++;
            if (noteIndex >= noteScores.Length) {
                NoteDataResize(noteScores.Length);
            }
        }

        /// <summary>
        /// エネルギー変化格納用配列のインデックスをインクリメント
        /// </summary>
        public void EnergyDataIndexUp()
        {
            energyIndex++;
            if (energyIndex >= energyDatas.Length) {
                EnergyDataResize();
            }
        }

        /// <summary>
        /// 譜面データ用配列のインデックスをインクリメント
        /// </summary>
        public void MapDataIndexUp()
        {
            mapIndex++;
            if (mapIndex >= mapDatas.Length) {
                MapDataResize(mapDatas.Length);
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
            this.maxRank = "E";
            this.environmentName = null;
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
            this.modifierMultiplier = 1f;
            this.modObstacles = "All";
            this.modInstaFail = false;
            this.modNoFail = false;
            this.modBatteryEnergy = false;
            this.batteryLives = 1;
            this.modDisappearingArrows = false;
            this.modNoBombs = false;
            this.modSongSpeed = "Normal";
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
            ResetNoteCut();
            ResetEnergy();
        }
        #endregion
        #region // プライベートメソッド
        /// <summary>
        /// ノーツ毎のスコア格納用配列をリセット
        /// </summary>
        private void ResetNoteCut()
        {
            noteIndex = 0;
            for (int i = 0; i < noteScores.Length; i++) {
                if (noteScores[i] == null) {
                    noteScores[i] = new NoteDataEntity();
                }
                else {
                    noteScores[i].bs_event = "";
                    noteScores[i].time = 0;
                    noteScores[i].cutTime = 0;
                    noteScores[i].score = 0;
                    noteScores[i].currentMaxScore = 0;
                    noteScores[i].rank = "E";
                    noteScores[i].passedNotes = 0;
                    noteScores[i].hitNotes = 0;
                    noteScores[i].missedNotes = 0;
                    noteScores[i].lastNoteScore = 0;
                    noteScores[i].passedBombs = 0;
                    noteScores[i].hitBombs = 0;
                    noteScores[i].combo = 0;
                    noteScores[i].maxCombo = 0;
                    noteScores[i].multiplier = 0;
                    noteScores[i].multiplierProgress = 0;
                    noteScores[i].batteryEnergy = 1;
                    noteScores[i].noteID = null;
                    noteScores[i].noteType = null;
                    noteScores[i].noteCutDirection = null;
                    noteScores[i].noteLine = null;
                    noteScores[i].noteLayer = null;
                    noteScores[i].speedOK = null;
                    noteScores[i].directionOK = null;
                    noteScores[i].saberTypeOK = null;
                    noteScores[i].wasCutTooSoon = null;
                    noteScores[i].initialScore = null;
                    noteScores[i].beforeScore = null;
                    noteScores[i].afterScore = null;
                    noteScores[i].cutDistanceScore = null;
                    noteScores[i].finalScore = null;
                    noteScores[i].cutMultiplier = null;
                    noteScores[i].saberSpeed = null;
                    noteScores[i].saberDirX = null;
                    noteScores[i].saberDirY = null;
                    noteScores[i].saberDirZ = null;
                    noteScores[i].saberType = null;
                    noteScores[i].swingRating = null;
                    noteScores[i].swingRatingFullyCut = null;
                    noteScores[i].timeDeviation = null;
                    noteScores[i].cutDirectionDeviation = null;
                    noteScores[i].cutPointX = null;
                    noteScores[i].cutPointY = null;
                    noteScores[i].cutPointZ = null;
                    noteScores[i].cutNormalX = null;
                    noteScores[i].cutNormalY = null;
                    noteScores[i].cutNormalZ = null;
                    noteScores[i].cutDistanceToCenter = null;
                    noteScores[i].timeToNextBasicNote = null;
                }
            }
        }

        /// <summary>
        /// エネルギー変化格納用配列をリセット
        /// </summary>
        private void ResetEnergy()
        {
            energyIndex = 0;
            for (int i = 0; i < energyDatas.Length; i++) {
                if (energyDatas[i] == null) {
                    energyDatas[i] = new EnergyDataEntity();
                }
                else {
                    energyDatas[i].time = 0;
                    energyDatas[i].energy = 0;
                }
            }
        }

        #endregion
    }
}
