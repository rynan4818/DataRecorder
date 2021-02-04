using DataRecorder.Util;
using DataRecorder.Models;
using DataRecorder.Configuration;
using DataRecorder.Interfaces;
using DataRecorder.Enums;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;
using System.Threading;

namespace DataRecorder.DataBases
{
    /// <summary>
    /// SQLiteDataBaseへの接続を提供するクラスです。
    /// 使う場合はZenjectのDIコンテナから引っ張ってきてください。
    /// </summary>
    public class Repository : IRepository, IInitializable, IDisposable
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        /// <summary>
        /// プレイデータのデータベース書き込みフラグ
        /// </summary>
        public bool playDataAddFlag { get; set; } = false;

        /// <summary>
        /// pause , resume イベント書き込みフラグ
        /// </summary>
        public BeatSaberEvent? pauseEventAddFlag { get; set; } = null;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 定数
        /// <summary>
        /// データベース書き込みのタイムアウト時間[ms]
        /// </summary>
        private const int databaseTimeout = 10000;

        // データベースパラメータ用文字列
        private const string sTime = "time";
        private const string sEvent = "event";
        private const string sEndTime = "endTime";
        private const string sMenuTime = "menuTime";
        private const string sCleared = "cleared";
        private const string sEndFlag = "endFlag";
        private const string sPauseCount = "pauseCount";
        private const string sPluginVersion = "pluginVersion";
        private const string sGameVersion = "gameVersion";
        private const string sScene = "scene";
        private const string sMode = "mode";
        // BeatMap
        private const string sSongName = "songName";
        private const string sSongSubName = "songSubName";
        private const string sSongAuthorName = "songAuthorName";
        private const string sLevelAuthorName = "levelAuthorName";
        private const string sLength = "length";
        private const string sSongHash = "songHash";
        private const string sLevelId = "levelId";
        private const string sSongBPM = "songBPM";
        private const string sNoteJumpSpeed = "noteJumpSpeed";
        private const string sSongTimeOffset = "songTimeOffset";
        private const string sStart = "start";
        private const string sPaused = "paused";
        private const string sDifficulty = "difficulty";
        private const string sNotesCount = "notesCount";
        private const string sBombsCount = "bombsCount";
        private const string sObstaclesCount = "obstaclesCount";
        private const string sMaxScore = "maxScore";
        private const string sMaxRank = "maxRank";
        private const string sEnvironmentName = "environmentName";
        private const string sScorePercentage = "scorePercentage";
        // Performance
        private const string sRawScore = "rawScore";
        private const string sScore = "score";
        private const string sCurrentMaxScore = "currentMaxScore";
        private const string sRank = "rank";
        private const string sPassedNotes = "passedNotes";
        private const string sHitNotes = "hitNotes";
        private const string sMissedNotes = "missedNotes";
        private const string sLastNoteScore = "lastNoteScore";
        private const string sPassedBombs = "passedBombs";
        private const string sHitBombs = "hitBombs";
        private const string sCombo = "combo";
        private const string sMaxCombo = "maxCombo";
        private const string sComboMultiplier = "comboMultiplier";
        private const string sMultiplierProgress = "multiplierProgress";
        private const string sBatteryEnergyLevel = "batteryEnergyLevel";
        private const string sSoftFailed = "softFailed";
        // Mod
        private const string sMultiplier = "multiplier";
        private const string sObstacles = "obstacles";
        private const string sInstaFail = "instaFail";
        private const string sNoFail = "noFail";
        private const string sBatteryEnergy = "batteryEnergy";
        private const string sDisappearingArrows = "disappearingArrows";
        private const string sNoBombs = "noBombs";
        private const string sSongSpeed = "songSpeed";
        private const string sSongSpeedMultiplier = "songSpeedMultiplier";
        private const string sNoArrows = "noArrows";
        private const string sGhostNotes = "ghostNotes";
        private const string sFailOnSaberClash = "failOnSaberClash";
        private const string sStrictAngles = "strictAngles";
        private const string sFastNotes = "fastNotes";
        private const string sStaticLights = "staticLights";
        private const string sLeftHanded = "leftHanded";
        private const string sPlayerHeight = "playerHeight";
        private const string sReduceDebris = "reduceDebris";
        private const string sNoHUD = "noHUD";
        private const string sAdvancedHUD = "advancedHUD";
        private const string sAutoRestart = "autoRestart";
        // NoteCut
        private const string sCutTime = "cutTime";
        private const string sStartTime = "startTime";
        private const string sNoteID = "noteID";
        private const string sNoteType = "noteType";
        private const string sNoteCutDirection = "noteCutDirection";
        private const string sNoteLine = "noteLine";
        private const string sNoteLayer = "noteLayer";
        private const string sSpeedOK = "speedOK";
        private const string sDirectionOK = "directionOK";
        private const string sSaberTypeOK = "saberTypeOK";
        private const string sWasCutTooSoon = "wasCutTooSoon";
        private const string sInitialScore = "initialScore";
        private const string sBeforeScore = "beforeScore";
        private const string sAfterScore = "afterScore";
        private const string sCutDistanceScore = "cutDistanceScore";
        private const string sFinalScore = "finalScore";
        private const string sCutMultiplier = "cutMultiplier";
        private const string sSaberSpeed = "saberSpeed";
        private const string sSaberDirX = "saberDirX";
        private const string sSaberDirY = "saberDirY";
        private const string sSaberDirZ = "saberDirZ";
        private const string sSaberType = "saberType";
        private const string sSwingRating = "swingRating";
        private const string sSwingRatingFullyCut = "swingRatingFullyCut";
        private const string sTimeDeviation = "timeDeviation";
        private const string sCutDirectionDeviation = "cutDirectionDeviation";
        private const string sCutPointX = "cutPointX";
        private const string sCutPointY = "cutPointY";
        private const string sCutPointZ = "cutPointZ";
        private const string sCutNormalX = "cutNormalX";
        private const string sCutNormalY = "cutNormalY";
        private const string sCutNormalZ = "cutNormalZ";
        private const string sCutDistanceToCenter = "cutDistanceToCenter";
        private const string sTimeToNextBasicNote = "timeToNextBasicNote";
        private const string sEnergy = "energy";

        // データベース書き込み用
        private const string sBomb = "Bomb";
        private const string sNoteA = "NoteA";
        private const string sNoteB = "NoteB";

        ///
        ///データベースSQL文
        ///
        // pause , resume イベント記録
        private const string sqlPauseEventAdd = "INSERT INTO MovieCutPause(time, event) VALUES (@time, @event)";
        // プレイデータの記録 BeatMap情報
        private const string sqlPlayDataAddBeatMap = @"
                            INSERT INTO MovieCutRecord(
                                startTime,
                                endTime,
                                menuTime,
                                cleared,
                                endFlag,
                                pauseCount,
                                pluginVersion,
                                gameVersion,
                                scene,
                                mode,
                                songName,
                                songSubName,
                                songAuthorName,
                                levelAuthorName,
                                songHash,
                                levelId,
                                songBPM,
                                noteJumpSpeed,
                                songTimeOffset,
                                start,
                                paused,
                                length,
                                difficulty,
                                notesCount,
                                bombsCount,
                                obstaclesCount,
                                maxScore,
                                maxRank,
                                environmentName,
                                scorePercentage,
                                rawScore,
                                score,
                                currentMaxScore,
                                rank,
                                passedNotes,
                                hitNotes,
                                missedNotes,
                                lastNoteScore,
                                passedBombs,
                                hitBombs,
                                combo,
                                maxCombo,
                                comboMultiplier,
                                multiplierProgress,
                                batteryEnergyLevel,
                                energy,
                                softFailed,
                                multiplier,
                                obstacles,
                                instaFail,
                                noFail,
                                batteryEnergy,
                                disappearingArrows,
                                noBombs,
                                songSpeed,
                                songSpeedMultiplier,
                                noArrows,
                                ghostNotes,
                                failOnSaberClash,
                                strictAngles,
                                fastNotes,
                                staticLights,
                                leftHanded,
                                playerHeight,
                                reduceDebris,
                                noHUD,
                                advancedHUD,
                                autoRestart
                            ) VALUES (
                                @startTime,
                                @endTime,
                                @menuTime,
                                @cleared,
                                @endFlag,
                                @pauseCount,
                                @pluginVersion,
                                @gameVersion,
                                @scene,
                                @mode,
                                @songName,
                                @songSubName,
                                @songAuthorName,
                                @levelAuthorName,
                                @songHash,
                                @levelId,
                                @songBPM,
                                @noteJumpSpeed,
                                @songTimeOffset,
                                @start,
                                @paused,
                                @length,
                                @difficulty,
                                @notesCount,
                                @bombsCount,
                                @obstaclesCount,
                                @maxScore,
                                @maxRank,
                                @environmentName,
                                @scorePercentage,
                                @rawScore,
                                @score,
                                @currentMaxScore,
                                @rank,
                                @passedNotes,
                                @hitNotes,
                                @missedNotes,
                                @lastNoteScore,
                                @passedBombs,
                                @hitBombs,
                                @combo,
                                @maxCombo,
                                @comboMultiplier,
                                @multiplierProgress,
                                @batteryEnergyLevel,
                                @energy,
                                @softFailed,
                                @multiplier,
                                @obstacles,
                                @instaFail,
                                @noFail,
                                @batteryEnergy,
                                @disappearingArrows,
                                @noBombs,
                                @songSpeed,
                                @songSpeedMultiplier,
                                @noArrows,
                                @ghostNotes,
                                @failOnSaberClash,
                                @strictAngles,
                                @fastNotes,
                                @staticLights,
                                @leftHanded,
                                @playerHeight,
                                @reduceDebris,
                                @noHUD,
                                @advancedHUD,
                                @autoRestart
                            )
        ";
        // プレイデータの記録 NoteCut情報
        private const string sqlPlayDataAddNoteCut = @"
                                                INSERT INTO NoteScore(
                                                    time,
                                                    cutTime,
                                                    startTime,
                                                    event,
                                                    rawScore,
                                                    score,
                                                    currentMaxScore,
                                                    rank,
                                                    passedNotes,
                                                    hitNotes,
                                                    missedNotes,
                                                    lastNoteScore,
                                                    passedBombs,
                                                    hitBombs,
                                                    combo,
                                                    maxCombo,
                                                    multiplier,
                                                    multiplierProgress,
                                                    batteryEnergy,
                                                    softFailed,
                                                    noteID,
                                                    noteType,
                                                    noteCutDirection,
                                                    noteLine,
                                                    noteLayer,
                                                    speedOK,
                                                    directionOK,
                                                    saberTypeOK,
                                                    wasCutTooSoon,
                                                    initialScore,
                                                    beforeScore,
                                                    afterScore,
                                                    cutDistanceScore,
                                                    finalScore,
                                                    cutMultiplier,
                                                    saberSpeed,
                                                    saberDirX,
                                                    saberDirY,
                                                    saberDirZ,
                                                    saberType,
                                                    swingRating,
                                                    swingRatingFullyCut,
                                                    timeDeviation,
                                                    cutDirectionDeviation,
                                                    cutPointX,
                                                    cutPointY,
                                                    cutPointZ,
                                                    cutNormalX,
                                                    cutNormalY,
                                                    cutNormalZ,
                                                    cutDistanceToCenter,
                                                    timeToNextBasicNote
                                                ) VALUES (
                                                    @time,
                                                    @cutTime,
                                                    @startTime,
                                                    @event,
                                                    @rawScore,
                                                    @score,
                                                    @currentMaxScore,
                                                    @rank,
                                                    @passedNotes,
                                                    @hitNotes,
                                                    @missedNotes,
                                                    @lastNoteScore,
                                                    @passedBombs,
                                                    @hitBombs,
                                                    @combo,
                                                    @maxCombo,
                                                    @multiplier,
                                                    @multiplierProgress,
                                                    @batteryEnergy,
                                                    @softFailed,
                                                    @noteID,
                                                    @noteType,
                                                    @noteCutDirection,
                                                    @noteLine,
                                                    @noteLayer,
                                                    @speedOK,
                                                    @directionOK,
                                                    @saberTypeOK,
                                                    @wasCutTooSoon,
                                                    @initialScore,
                                                    @beforeScore,
                                                    @afterScore,
                                                    @cutDistanceScore,
                                                    @finalScore,
                                                    @cutMultiplier,
                                                    @saberSpeed,
                                                    @saberDirX,
                                                    @saberDirY,
                                                    @saberDirZ,
                                                    @saberType,
                                                    @swingRating,
                                                    @swingRatingFullyCut,
                                                    @timeDeviation,
                                                    @cutDirectionDeviation,
                                                    @cutPointX,
                                                    @cutPointY,
                                                    @cutPointZ,
                                                    @cutNormalX,
                                                    @cutNormalY,
                                                    @cutNormalZ,
                                                    @cutDistanceToCenter,
                                                    @timeToNextBasicNote
                                                )
        ";
        // プレイデータの記録 エネルギー変化記録
        private const string sqlPlayDataAddEnergy = "INSERT INTO EnergyChange(time, energy) VALUES (@time, @energy)";
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            if (!File.Exists(PluginConfig.Instance.DBFile)) {
                if (!Directory.Exists(Path.GetDirectoryName(PluginConfig.DataBaseFilePath))) {
                    Directory.CreateDirectory(Path.GetDirectoryName(PluginConfig.DataBaseFilePath));
                }
                PluginConfig.Instance.DBFile = PluginConfig.DataBaseFilePath;
            }
            this.CreateTable();
            this.thread = new Thread(new ThreadStart(this.DbInsertEvent));
            this.thread.Start();
        }

        /// <summary>
        /// データベースのタイムアウト処理
        /// </summary>
        public void DbTimeoutCheck()
        {
            if (this.databaseInsertTime != null) {
                if (Utility.GetCurrentTime() - this.databaseInsertTime > databaseTimeout) {
                    Logger.Error("DB Timeout Error");
                    this._connection?.Dispose();
                    this.databaseInsertTime = null;
                    this.playDataAddFlag = false;
                    this.pauseEventAddFlag = null;
                }
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        /// <summary>
        /// 非同期用データベース書き込み処理
        /// </summary>
        private void DbInsertEvent()
        {
            while (true) {
                if (this.playDataAddFlag) this.PlayDataAdd();
                if (this.pauseEventAddFlag != null) this.PauseEventAdd(this.pauseEventAddFlag);
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// pause , resume イベント記録
        /// </summary>
        private void PauseEventAdd(BeatSaberEvent? bs_event)
        {
            Logger.Debug("PauseEventAdd call");
            this.databaseInsertTime = Utility.GetCurrentTime();
            using (this._connection = new SQLiteConnection($"Data Source={PluginConfig.Instance.DBFile};Version=3;")) {
                this._connection.Open();
                try {
                    using (SQLiteCommand command = new SQLiteCommand(this._connection)) {
                        command.CommandText = sqlPauseEventAdd;
                        command.Parameters.Add(sTime, System.Data.DbType.Int64);
                        command.Parameters[sTime].Value = Utility.GetCurrentTime();
                        command.Parameters.Add(sEvent, System.Data.DbType.String);
                        command.Parameters[sEvent].Value = bs_event.GetDescription();
                        var result = command.ExecuteNonQuery();
                        // データ更新できない場合
                        if (result != 1)
                            Logger.Error("DB NoteScore MovieCutPause Error");
                    }
                }
                catch (Exception e) {
                    Logger.Error(e);
                }
            }
            this.databaseInsertTime = null;
            this.pauseEventAddFlag = null;
        }

        /// <summary>
        /// プレイデータの記録
        /// </summary>
        private void PlayDataAdd()
        {
            int result;
            var gameStatus = this._gameStatus;
            long addStartTime = Utility.GetCurrentTime();
            Logger.Debug("PlayDataAdd call");
            this.databaseInsertTime = Utility.GetCurrentTime();
            using (this._connection = new SQLiteConnection($"Data Source={PluginConfig.Instance.DBFile};Version=3;")) {
                this._connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(this._connection)) {
                    try {
                        command.CommandText = sqlPlayDataAddBeatMap;
                        #region // MovieCutRecordテーブルINSERT
                        //独自カラム
                        command.Parameters.Add(sStartTime, DbType.Int64);
                        command.Parameters[sStartTime].Value = gameStatus.startTime;
                        command.Parameters.Add(sEndTime, DbType.Int64);
                        if (gameStatus.endTime == 0)
                            gameStatus.endTime = Utility.GetCurrentTime();
                        command.Parameters[sEndTime].Value = gameStatus.endTime;
                        command.Parameters.Add(sMenuTime, DbType.Int64);
                        command.Parameters[sMenuTime].Value = Utility.GetCurrentTime();
                        command.Parameters.Add(sCleared, DbType.String);
                        command.Parameters[sCleared].Value = gameStatus.cleared.GetDescription();
                        command.Parameters.Add(sEndFlag, DbType.Int32);
                        command.Parameters[sEndFlag].Value = gameStatus.endFlag;
                        command.Parameters.Add(sPauseCount, DbType.Int32);
                        command.Parameters[sPauseCount].Value = gameStatus.pauseCount;
                        //gameステータス
                        command.Parameters.Add(sPluginVersion, DbType.String);
                        command.Parameters[sPluginVersion].Value = Utility.GetPluginVersion();
                        command.Parameters.Add(sGameVersion, DbType.String);
                        command.Parameters[sGameVersion].Value = Utility.GetGameVersion();
                        command.Parameters.Add(sScene, DbType.String);
                        command.Parameters[sScene].Value = gameStatus.scene.GetDescription();
                        command.Parameters.Add(sMode, DbType.String);
                        command.Parameters[sMode].Value = gameStatus.mode == null ? null : (gameStatus.multiplayer ? "Multiplayer" : gameStatus.partyMode ? "Party" : "Solo") + gameStatus.mode;
                        //beatmapステータス
                        command.Parameters.Add(sSongName, DbType.String);
                        command.Parameters[sSongName].Value = gameStatus.songName;
                        command.Parameters.Add(sSongSubName, DbType.String);
                        command.Parameters[sSongSubName].Value = gameStatus.songSubName;
                        command.Parameters.Add(sSongAuthorName, DbType.String);
                        command.Parameters[sSongAuthorName].Value = gameStatus.songAuthorName;
                        command.Parameters.Add(sLevelAuthorName, DbType.String);
                        command.Parameters[sLevelAuthorName].Value = gameStatus.levelAuthorName;
                        command.Parameters.Add(sLength, DbType.Int64);
                        command.Parameters[sLength].Value = gameStatus.length;
                        command.Parameters.Add(sSongHash, DbType.String);
                        command.Parameters[sSongHash].Value = gameStatus.songHash;
                        command.Parameters.Add(sLevelId, DbType.String);
                        command.Parameters[sLevelId].Value = gameStatus.levelId;
                        command.Parameters.Add(sSongBPM, DbType.Single);
                        command.Parameters[sSongBPM].Value = gameStatus.songBPM;
                        command.Parameters.Add(sNoteJumpSpeed, DbType.Single);
                        command.Parameters[sNoteJumpSpeed].Value = gameStatus.noteJumpSpeed;
                        command.Parameters.Add(sSongTimeOffset, DbType.Int64);
                        command.Parameters[sSongTimeOffset].Value = gameStatus.songTimeOffset;
                        command.Parameters.Add(sStart, DbType.Int64);
                        command.Parameters.Add(sPaused, DbType.Int64);
                        if (gameStatus.start == 0)
                            command.Parameters[sStart].Value = null;
                        else
                            command.Parameters[sStart].Value = gameStatus.start;
                        if (gameStatus.paused == 0)
                            command.Parameters[sPaused].Value = null;
                        else
                            command.Parameters[sPaused].Value = gameStatus.paused;
                        command.Parameters.Add(sDifficulty, DbType.String);
                        command.Parameters[sDifficulty].Value = gameStatus.difficulty;
                        command.Parameters.Add(sNotesCount, DbType.Int32);
                        command.Parameters[sNotesCount].Value = gameStatus.notesCount;
                        command.Parameters.Add(sBombsCount, DbType.Int32);
                        command.Parameters[sBombsCount].Value = gameStatus.bombsCount;
                        command.Parameters.Add(sObstaclesCount, DbType.Int32);
                        command.Parameters[sObstaclesCount].Value = gameStatus.obstaclesCount;
                        command.Parameters.Add(sMaxScore, DbType.Int32);
                        command.Parameters[sMaxScore].Value = gameStatus.maxScore;
                        command.Parameters.Add(sMaxRank, DbType.String);
                        command.Parameters[sMaxRank].Value = gameStatus.maxRank.ToString();
                        command.Parameters.Add(sEnvironmentName, DbType.String);
                        command.Parameters[sEnvironmentName].Value = gameStatus.environmentName;
                        double scorePercentage;
                        if (gameStatus.currentMaxScore == 0)
                            scorePercentage = 0.0;
                        else
                            scorePercentage = double.Parse(String.Format("{0:F2}", ((double)gameStatus.score / (double)gameStatus.currentMaxScore) * 100.0));
                        command.Parameters.Add(sScorePercentage, DbType.Double);
                        command.Parameters[sScorePercentage].Value = scorePercentage;
                        //performanceステータス
                        command.Parameters.Add(sRawScore, DbType.Int32);
                        command.Parameters[sRawScore].Value = gameStatus.rawScore;
                        command.Parameters.Add(sScore, DbType.Int32);
                        command.Parameters[sScore].Value = gameStatus.score;
                        command.Parameters.Add(sCurrentMaxScore, DbType.Int32);
                        command.Parameters[sCurrentMaxScore].Value = gameStatus.currentMaxScore;
                        command.Parameters.Add(sRank, DbType.String);
                        command.Parameters[sRank].Value = RankModel.GetRankName(gameStatus.rank);
                        command.Parameters.Add(sPassedNotes, DbType.Int32);
                        command.Parameters[sPassedNotes].Value = gameStatus.passedNotes;
                        command.Parameters.Add(sHitNotes, DbType.Int32);
                        command.Parameters[sHitNotes].Value = gameStatus.hitNotes;
                        command.Parameters.Add(sMissedNotes, DbType.Int32);
                        command.Parameters[sMissedNotes].Value = gameStatus.missedNotes;
                        command.Parameters.Add(sLastNoteScore, DbType.Int32);
                        command.Parameters[sLastNoteScore].Value = gameStatus.lastNoteScore;
                        command.Parameters.Add(sPassedBombs, DbType.Int32);
                        command.Parameters[sPassedBombs].Value = gameStatus.passedBombs;
                        command.Parameters.Add(sHitBombs, DbType.Int32);
                        command.Parameters[sHitBombs].Value = gameStatus.hitBombs;
                        command.Parameters.Add(sCombo, DbType.Int32);
                        command.Parameters[sCombo].Value = gameStatus.combo;
                        command.Parameters.Add(sMaxCombo, DbType.Int32);
                        command.Parameters[sMaxCombo].Value = gameStatus.maxCombo;
                        command.Parameters.Add(sComboMultiplier, DbType.Int32);
                        command.Parameters[sComboMultiplier].Value = gameStatus.multiplier;
                        command.Parameters.Add(sMultiplierProgress, DbType.Single);
                        command.Parameters[sMultiplierProgress].Value = gameStatus.multiplierProgress;
                        command.Parameters.Add(sBatteryEnergyLevel, DbType.Int32);
                        command.Parameters[sBatteryEnergyLevel].Value = gameStatus.batteryEnergy;
                        command.Parameters.Add(sEnergy, DbType.Single);
                        command.Parameters[sEnergy].Value = gameStatus.energy;
                        command.Parameters.Add(sSoftFailed, DbType.Boolean);
                        command.Parameters[sSoftFailed].Value = gameStatus.softFailed;
                        //modステータス
                        command.Parameters.Add(sMultiplier, DbType.Single);
                        command.Parameters[sMultiplier].Value = gameStatus.modifierMultiplier;
                        command.Parameters.Add(sObstacles, DbType.String);
                        if (gameStatus.modObstacles == GameplayModifiers.EnabledObstacleType.NoObstacles)
                            command.Parameters[sObstacles].Value = 0;
                        else
                            command.Parameters[sObstacles].Value = gameStatus.modObstacles.ToString();
                        command.Parameters.Add(sInstaFail, DbType.Boolean);
                        command.Parameters[sInstaFail].Value = gameStatus.modInstaFail;
                        command.Parameters.Add(sNoFail, DbType.Boolean);
                        command.Parameters[sNoFail].Value = gameStatus.modNoFail;
                        command.Parameters.Add(sBatteryEnergy, DbType.Boolean);
                        command.Parameters[sBatteryEnergy].Value = gameStatus.modBatteryEnergy;
                        command.Parameters.Add(sDisappearingArrows, DbType.Boolean);
                        command.Parameters[sDisappearingArrows].Value = gameStatus.modDisappearingArrows;
                        command.Parameters.Add(sNoBombs, DbType.Boolean);
                        command.Parameters[sNoBombs].Value = gameStatus.modNoBombs;
                        command.Parameters.Add(sSongSpeed, DbType.String);
                        command.Parameters[sSongSpeed].Value = gameStatus.modSongSpeed.ToString();
                        command.Parameters.Add(sSongSpeedMultiplier, DbType.Single);
                        command.Parameters[sSongSpeedMultiplier].Value = gameStatus.songSpeedMultiplier;
                        command.Parameters.Add(sNoArrows, DbType.Boolean);
                        command.Parameters[sNoArrows].Value = gameStatus.modNoArrows;
                        command.Parameters.Add(sGhostNotes, DbType.Boolean);
                        command.Parameters[sGhostNotes].Value = gameStatus.modGhostNotes;
                        command.Parameters.Add(sFailOnSaberClash, DbType.Boolean);
                        command.Parameters[sFailOnSaberClash].Value = gameStatus.modFailOnSaberClash;
                        command.Parameters.Add(sStrictAngles, DbType.Boolean);
                        command.Parameters[sStrictAngles].Value = gameStatus.modStrictAngles;
                        command.Parameters.Add(sFastNotes, DbType.Boolean);
                        command.Parameters[sFastNotes].Value = gameStatus.modFastNotes;
                        //playerSettingsステータス
                        command.Parameters.Add(sStaticLights, DbType.Boolean);
                        command.Parameters[sStaticLights].Value = gameStatus.staticLights;
                        command.Parameters.Add(sLeftHanded, DbType.Boolean);
                        command.Parameters[sLeftHanded].Value = gameStatus.leftHanded;
                        command.Parameters.Add(sPlayerHeight, DbType.Single);
                        command.Parameters[sPlayerHeight].Value = gameStatus.playerHeight;
                        command.Parameters.Add(sReduceDebris, DbType.Boolean);
                        command.Parameters[sReduceDebris].Value = gameStatus.reduceDebris;
                        command.Parameters.Add(sNoHUD, DbType.Boolean);
                        command.Parameters[sNoHUD].Value = gameStatus.noHUD;
                        command.Parameters.Add(sAdvancedHUD, DbType.Boolean);
                        command.Parameters[sAdvancedHUD].Value = gameStatus.advancedHUD;
                        command.Parameters.Add(sAutoRestart, DbType.Boolean);
                        command.Parameters[sAutoRestart].Value = gameStatus.autoRestart;
                        result = command.ExecuteNonQuery();
                        if (result != 1)
                            Logger.Error("DB MovieCutRecord Error");
                    }
                    catch (Exception e) {
                        Logger.Error("DB MovieCutRecord Error:" + e.Message);
                    }
                    #endregion

                    #region // NoteScore
                    // トランザクションを開始します。
                    using (SQLiteTransaction transaction = this._connection.BeginTransaction()) {
                        try {
                            command.CommandText = sqlPlayDataAddNoteCut;
                            command.Parameters.Add(sTime, DbType.Int64);
                            command.Parameters.Add(sCutTime, DbType.Int64);
                            command.Parameters.Add(sStartTime, DbType.Int64);
                            command.Parameters.Add(sEvent, DbType.String);
                            command.Parameters.Add(sRawScore, DbType.Int32);
                            command.Parameters.Add(sScore, DbType.Int32);
                            command.Parameters.Add(sCurrentMaxScore, DbType.Int32);
                            command.Parameters.Add(sRank, DbType.String);
                            command.Parameters.Add(sPassedNotes, DbType.Int32);
                            command.Parameters.Add(sHitNotes, DbType.Int32);
                            command.Parameters.Add(sMissedNotes, DbType.Int32);
                            command.Parameters.Add(sLastNoteScore, DbType.Int32);
                            command.Parameters.Add(sPassedBombs, DbType.Int32);
                            command.Parameters.Add(sHitBombs, DbType.Int32);
                            command.Parameters.Add(sCombo, DbType.Int32);
                            command.Parameters.Add(sMaxCombo, DbType.Int32);
                            command.Parameters.Add(sMultiplier, DbType.Int32);
                            command.Parameters.Add(sMultiplierProgress, DbType.Single);
                            command.Parameters.Add(sBatteryEnergy, DbType.Int32);
                            command.Parameters.Add(sSoftFailed, DbType.Boolean);
                            command.Parameters.Add(sNoteID, DbType.Int32);
                            command.Parameters.Add(sNoteType, DbType.String);
                            command.Parameters.Add(sNoteCutDirection, DbType.String);
                            command.Parameters.Add(sNoteLine, DbType.Int32);
                            command.Parameters.Add(sNoteLayer, DbType.Int32);
                            command.Parameters.Add(sSpeedOK, DbType.Boolean);
                            command.Parameters.Add(sDirectionOK, DbType.Boolean);
                            command.Parameters.Add(sSaberTypeOK, DbType.Boolean);
                            command.Parameters.Add(sWasCutTooSoon, DbType.Boolean);
                            command.Parameters.Add(sInitialScore, DbType.Int32);
                            command.Parameters.Add(sBeforeScore, DbType.Int32);
                            command.Parameters.Add(sAfterScore, DbType.Int32);
                            command.Parameters.Add(sCutDistanceScore, DbType.Int32);
                            command.Parameters.Add(sFinalScore, DbType.Int32);
                            command.Parameters.Add(sCutMultiplier, DbType.Int32);
                            command.Parameters.Add(sSaberSpeed, DbType.Single);
                            command.Parameters.Add(sSaberDirX, DbType.Single);
                            command.Parameters.Add(sSaberDirY, DbType.Single);
                            command.Parameters.Add(sSaberDirZ, DbType.Single);
                            command.Parameters.Add(sSaberType, DbType.String);
                            command.Parameters.Add(sSwingRating, DbType.Single);
                            command.Parameters.Add(sSwingRatingFullyCut, DbType.Single);
                            command.Parameters.Add(sTimeDeviation, DbType.Single);
                            command.Parameters.Add(sCutDirectionDeviation, DbType.Single);
                            command.Parameters.Add(sCutPointX, DbType.Single);
                            command.Parameters.Add(sCutPointY, DbType.Single);
                            command.Parameters.Add(sCutPointZ, DbType.Single);
                            command.Parameters.Add(sCutNormalX, DbType.Single);
                            command.Parameters.Add(sCutNormalY, DbType.Single);
                            command.Parameters.Add(sCutNormalZ, DbType.Single);
                            command.Parameters.Add(sCutDistanceToCenter, DbType.Single);
                            command.Parameters.Add(sTimeToNextBasicNote, DbType.Single);
                            gameStatus.lastNoteId = 0;
                            NoteDataEntity noteScore;
                            for (gameStatus.noteIndex = 0; gameStatus.noteIndex < gameStatus.noteEndIndex; gameStatus.noteIndex++) {
                                this.databaseInsertTime = Utility.GetCurrentTime();
                                noteScore = gameStatus.NoteDataGet();
                                command.Parameters[sTime].Value = noteScore.time;
                                command.Parameters[sCutTime].Value = noteScore.cutTime;
                                command.Parameters[sStartTime].Value = gameStatus.startTime;
                                command.Parameters[sEvent].Value = noteScore.bs_event.GetDescription();
                                command.Parameters[sRawScore].Value = noteScore.rawScore;
                                command.Parameters[sScore].Value = noteScore.score;
                                command.Parameters[sCurrentMaxScore].Value = noteScore.currentMaxScore;
                                command.Parameters[sRank].Value = RankModel.GetRankName(noteScore.rank);
                                command.Parameters[sPassedNotes].Value = noteScore.passedNotes;
                                command.Parameters[sHitNotes].Value = noteScore.hitNotes;
                                command.Parameters[sMissedNotes].Value = noteScore.missedNotes;
                                command.Parameters[sLastNoteScore].Value = noteScore.lastNoteScore;
                                command.Parameters[sPassedBombs].Value = noteScore.passedBombs;
                                command.Parameters[sHitBombs].Value = noteScore.hitBombs;
                                command.Parameters[sCombo].Value = noteScore.combo;
                                command.Parameters[sMaxCombo].Value = noteScore.maxCombo;
                                command.Parameters[sMultiplier].Value = noteScore.multiplier;
                                command.Parameters[sMultiplierProgress].Value = noteScore.multiplierProgress;
                                command.Parameters[sBatteryEnergy].Value = noteScore.batteryEnergy;
                                command.Parameters[sSoftFailed].Value = noteScore.softFailed;
                                command.Parameters[sNoteID].Value = gameStatus.GetNoteId();
                                command.Parameters[sNoteType].Value = noteScore.colorType == ColorType.None ? sBomb : noteScore.colorType == ColorType.ColorA ? sNoteA : noteScore.colorType == ColorType.ColorB ? sNoteB : noteScore.colorType.ToString();
                                command.Parameters[sNoteCutDirection].Value = noteScore.noteCutDirection.ToString();
                                command.Parameters[sNoteLine].Value = noteScore.noteLine;
                                command.Parameters[sNoteLayer].Value = (int)noteScore.noteLayer;
                                command.Parameters[sSpeedOK].Value = noteScore.speedOK;
                                command.Parameters[sDirectionOK].Value = noteScore.directionOK;
                                command.Parameters[sSaberTypeOK].Value = noteScore.saberTypeOK;
                                command.Parameters[sWasCutTooSoon].Value = noteScore.wasCutTooSoon;
                                command.Parameters[sInitialScore].Value = noteScore.initialScore;
                                command.Parameters[sBeforeScore].Value = noteScore.initialScore - noteScore.cutDistanceScore;
                                command.Parameters[sAfterScore].Value = noteScore.finalScore - noteScore.initialScore;
                                command.Parameters[sCutDistanceScore].Value = noteScore.cutDistanceScore;
                                command.Parameters[sFinalScore].Value = noteScore.finalScore;
                                command.Parameters[sCutMultiplier].Value = noteScore.cutMultiplier;
                                command.Parameters[sSaberSpeed].Value = noteScore.saberSpeed;
                                command.Parameters[sSaberDirX].Value = noteScore.saberDirX;
                                command.Parameters[sSaberDirY].Value = noteScore.saberDirY;
                                command.Parameters[sSaberDirZ].Value = noteScore.saberDirZ;
                                command.Parameters[sSaberType].Value = noteScore.saberType.ToString();
                                command.Parameters[sSwingRating].Value = noteScore.swingRating;
                                command.Parameters[sSwingRatingFullyCut].Value = noteScore.swingRatingFullyCut;
                                command.Parameters[sTimeDeviation].Value = noteScore.timeDeviation;
                                command.Parameters[sCutDirectionDeviation].Value = noteScore.cutDirectionDeviation;
                                command.Parameters[sCutPointX].Value = noteScore.cutPointX;
                                command.Parameters[sCutPointY].Value = noteScore.cutPointY;
                                command.Parameters[sCutPointZ].Value = noteScore.cutPointZ;
                                command.Parameters[sCutNormalX].Value = noteScore.cutNormalX;
                                command.Parameters[sCutNormalY].Value = noteScore.cutNormalY;
                                command.Parameters[sCutNormalZ].Value = noteScore.cutNormalZ;
                                command.Parameters[sCutDistanceToCenter].Value = noteScore.cutDistanceToCenter;
                                command.Parameters[sTimeToNextBasicNote].Value = noteScore.timeToNextBasicNote;
                                result = command.ExecuteNonQuery();
                                if (result != 1) {
                                    Logger.Error("DB NoteScore INSERT Error");
                                    transaction.Rollback();
                                    break;
                                }
                            }
                        }
                        catch (Exception e) {
                            Logger.Error("DB NoteScore INSERT Error:" + e.Message);
                            transaction.Rollback();
                        }
                        try {
                            command.CommandText = sqlPlayDataAddEnergy;
                            command.Parameters.Add(sTime, DbType.Int64);
                            command.Parameters.Add(sEnergy, DbType.Single);
                            long beforeTime = 0;
                            EnergyDataEntity energyData;
                            for (gameStatus.energyIndex = 0; gameStatus.energyIndex < gameStatus.energyEndIndex; gameStatus.energyIndex++) {
                                this.databaseInsertTime = Utility.GetCurrentTime();
                                energyData = gameStatus.EnergyDataGet();
                                if (beforeTime == energyData.time)
                                    energyData.time += 1;
                                beforeTime = energyData.time;
                                command.Parameters[sTime].Value = energyData.time;
                                command.Parameters[sEnergy].Value = energyData.energy;
                                result = command.ExecuteNonQuery();
                                if (result != 1) {
                                    Logger.Error("DB EnergyChange INSERT Error");
                                    transaction.Rollback();
                                    break;
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception e) {
                            Logger.Error("DB EnergyChange INSERT Error:" + e.Message);
                            transaction.Rollback();
                        }
                        #endregion
                    }
                }
            }
            this._gameStatus.ResetGameStatus();
            this.playDataAddFlag = false;
            this.databaseInsertTime = null;
            Logger.Info($"Play data writing completed:{Utility.GetCurrentTime() - addStartTime}ms");
        }

        /// <summary>
        /// テーブルを作成します。
        /// </summary>
        private void CreateTable()
        {
            using (this._connection = new SQLiteConnection($"Data Source={PluginConfig.Instance.DBFile};Version=3;")) {
                this._connection.Open();
                try {
                    using (SQLiteCommand command = new SQLiteCommand(this._connection)) {
                        command.CommandText = @"
                            CREATE TABLE IF NOT EXISTS MovieCutRecord(
                                startTime INTEGER NOT NULL PRIMARY KEY,
                                endTime INTEGER,
                                menuTime INTEGER NOT NULL,
                                cleared TEXT,
                                endFlag INTEGER NOT NULL,
                                pauseCount INTEGER NOT NULL,
                                pluginVersion TEXT,
                                gameVersion TEXT,
                                scene TEXT,
                                mode TEXT,
                                songName TEXT,
                                songSubName TEXT,
                                songAuthorName TEXT,
                                levelAuthorName TEXT,
                                songHash TEXT,
                                levelId TEXT,
                                songBPM REAL,
                                noteJumpSpeed REAL,
                                songTimeOffset INTEGER,
                                start TEXT,
                                paused TEXT,
                                length INTEGER,
                                difficulty TEXT,
                                notesCount INTEGER,
                                bombsCount INTEGER,
                                obstaclesCount INTEGER,
                                maxScore INTEGER,
                                maxRank TEXT,
                                environmentName TEXT,
                                scorePercentage REAL,
                                rawScore INTEGER,
                                score INTEGER,
                                currentMaxScore INTEGER,
                                rank TEXT,
                                passedNotes INTEGER,
                                hitNotes INTEGER,
                                missedNotes INTEGER,
                                lastNoteScore INTEGER,
                                passedBombs INTEGER,
                                hitBombs INTEGER,
                                combo INTEGER,
                                maxCombo INTEGER,
                                comboMultiplier INTEGER,
                                multiplierProgress REAL,
                                batteryEnergyLevel INTEGER,
                                energy REAL,
                                softFailed INTEGER,
                                multiplier REAL,
                                obstacles TEXT,
                                instaFail INTEGER,
                                noFail INTEGER,
                                batteryEnergy INTEGER,
                                disappearingArrows INTEGER,
                                noBombs INTEGER,
                                songSpeed TEXT,
                                songSpeedMultiplier REAL,
                                noArrows INTEGER,
                                ghostNotes INTEGER,
                                failOnSaberClash INTEGER,
                                strictAngles INTEGER,
                                fastNotes INTEGER,
                                staticLights INTEGER,
                                leftHanded INTEGER,
                                playerHeight REAL,
                                reduceDebris INTEGER,
                                noHUD INTEGER,
                                advancedHUD INTEGER,
                                autoRestart INTEGER
                            );
                        ";
                        command.ExecuteNonQuery();
                        command.CommandText = @"
                            CREATE TABLE IF NOT EXISTS MovieCutPause(
                                time INTEGER NOT NULL PRIMARY KEY,
                                event TEXT
                            );
                        ";
                        command.ExecuteNonQuery();
                        command.CommandText = @"
                            CREATE TABLE IF NOT EXISTS EnergyChange(
                                time INTEGER NOT NULL PRIMARY KEY,
                                energy REAL
                            );
                        ";
                        command.ExecuteNonQuery();
                        command.CommandText = @"
                            CREATE TABLE IF NOT EXISTS NoteScore(
                                time INTEGER,
                                cutTime INTEGER,
                                startTime INTEGER,
                                event TEXT,
                                rawScore INTEGER,
                                score INTEGER,
                                currentMaxScore INTEGER,
                                rank TEXT,
                                passedNotes INTEGER,
                                hitNotes INTEGER,
                                missedNotes INTEGER,
                                lastNoteScore INTEGER,
                                passedBombs INTEGER,
                                hitBombs INTEGER,
                                combo INTEGER,
                                maxCombo INTEGER,
                                multiplier INTEGER,
                                multiplierProgress REAL,
                                batteryEnergy INTEGER,
                                softFailed INTEGER,
                                noteID INTEGER,
                                noteType TEXT,
                                noteCutDirection TEXT,
                                noteLine INTEGER,
                                noteLayer INTEGER,
                                speedOK INTEGER,
                                directionOK INTEGER,
                                saberTypeOK INTEGER,
                                wasCutTooSoon INTEGER,
                                initialScore INTEGER,
                                beforeScore INTEGER,
                                afterScore INTEGER,
                                cutDistanceScore INTEGER,
                                finalScore INTEGER,
                                cutMultiplier INTEGER,
                                saberSpeed REAL,
                                saberDirX REAL,
                                saberDirY REAL,
                                saberDirZ REAL,
                                saberType TEXT,
                                swingRating REAL,
                                swingRatingFullyCut REAL,
                                timeDeviation REAL,
                                cutDirectionDeviation REAL,
                                cutPointX REAL,
                                cutPointY REAL,
                                cutPointZ REAL,
                                cutNormalX REAL,
                                cutNormalY REAL,
                                cutNormalZ REAL,
                                cutDistanceToCenter REAL,
                                timeToNextBasicNote REAL
                            );
                        ";
                        command.ExecuteNonQuery();
                        DbColumnCheck(command, "MovieCutRecord", "levelId", "TEXT");
                        DbColumnCheck(command, "MovieCutRecord", "rawScore", "INTEGER");
                        DbColumnCheck(command, "MovieCutRecord", "comboMultiplier", "INTEGER");
                        DbColumnCheck(command, "MovieCutRecord", "multiplierProgress", "REAL");
                        DbColumnCheck(command, "MovieCutRecord", "batteryEnergyLevel", "INTEGER");
                        DbColumnCheck(command, "MovieCutRecord", "energy", "REAL");
                        DbColumnCheck(command, "MovieCutRecord", "softFailed", "INTEGER");
                        DbColumnCheck(command, "NoteScore", "beforeScore", "INTEGER");
                        DbColumnCheck(command, "NoteScore", "rawScore", "INTEGER");
                        DbColumnCheck(command, "NoteScore", "softFailed", "INTEGER");
                    }
                }
                catch (Exception e) {
                    Logger.Error(e);
                }
            }
        }

        /// <summary>
        /// テーブルに指定カラムが存在するかチェックし、無ければ追加する
        /// </summary>
        /// <param name="command">SQLiteのコマンドオブジェクト</param>
        /// <param name="table">対象テーブル名</param>
        /// <param name="column">対象カラム名</param>
        /// <param name="type">追加時の型</param>
        private void DbColumnCheck(SQLiteCommand command, string table, string column, string type)
        {
            command.CommandText = $"PRAGMA table_info('{table}');";
            bool column_check = true;
            using (SQLiteDataReader db_reader = command.ExecuteReader()) {
                while (db_reader.Read()) {
                    if (column == (string)db_reader["name"]) {
                        column_check = false;
                        break;
                    }
                }
            }
            if (column_check) {
                command.CommandText = $"ALTER TABLE {table} ADD COLUMN {column} {type};";
                command.ExecuteNonQuery();
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private SQLiteConnection _connection;
        private Thread thread;
        /// <summary>
        /// データベースの最終INSERT発行時間(通常時はnull)
        /// </summary>
        private long? databaseInsertTime = null;

        [Inject]
        private GameStatus _gameStatus;

        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        public Repository()
        {
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    this._connection?.Dispose();
                    this.thread?.Abort();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~Repository()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
