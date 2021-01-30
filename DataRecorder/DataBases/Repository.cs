using DataRecorder.Util;
using DataRecorder.Models;
using DataRecorder.Configuration;
using DataRecorder.Interfaces;
using DataRecorder.Enums;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

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
        }

        /// <summary>
        /// pause , resume イベント記録
        /// </summary>
        public void PauseEventAdd(BeatSaberEvent bs_event)
        {
            using (this._connection = new SQLiteConnection($"Data Source={PluginConfig.Instance.DBFile};Version=3;")) {
                this._connection.Open();
                try {
                    using (SQLiteCommand command = new SQLiteCommand(this._connection)) {
                        command.CommandText = "insert into MovieCutPause(time, event) values (@time, @event)";
                        command.Parameters.Add(new SQLiteParameter("@time", Utility.GetCurrentTime()));
                        command.Parameters.Add(new SQLiteParameter("@event", bs_event.GetDescription()));
                        var result = command.ExecuteNonQuery();
                        // データ更新できない場合
                        if (result != 1)
                            Logger.Error("DB NoteScore MovieCutPause Error");
                    }
                }
                catch (Exception e) {
                    Logger.Error(e);
                }
                finally {
                    this._connection.Close();
                }
            }
        }

        /// <summary>
        /// プレイデータの記録
        /// </summary>
        public void PlayDataAdd()
        {
            using (this._connection = new SQLiteConnection($"Data Source={PluginConfig.Instance.DBFile};Version=3;")) {
                this._connection.Open();
                SQLiteTransaction transaction = null;
                try {
                    using (SQLiteCommand command = new SQLiteCommand(this._connection)) {
                        var gameStatus = this._gameStatus;
                        command.CommandText = @"
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
                        #region // MovieCutRecordテーブルINSERT
                        //独自カラム
                        command.Parameters.Add(new SQLiteParameter("@startTime", gameStatus.startTime));
                        command.Parameters.Add(new SQLiteParameter("@endTime", gameStatus.endTime));
                        command.Parameters.Add(new SQLiteParameter("@menuTime", Utility.GetCurrentTime()));
                        command.Parameters.Add(new SQLiteParameter("@cleared", gameStatus.cleared.GetDescription()));
                        command.Parameters.Add(new SQLiteParameter("@endFlag", gameStatus.endFlag));
                        command.Parameters.Add(new SQLiteParameter("@pauseCount", gameStatus.pauseCount));
                        //gameステータス
                        command.Parameters.Add(new SQLiteParameter("@pluginVersion", Utility.GetPluginVersion()));
                        command.Parameters.Add(new SQLiteParameter("@gameVersion", Utility.GetGameVersion()));
                        command.Parameters.Add(new SQLiteParameter("@scene", gameStatus.scene.GetDescription()));
                        command.Parameters.Add(new SQLiteParameter("@mode", gameStatus.mode == null ? null : (gameStatus.partyMode ? "Party" : "Solo") + gameStatus.mode));
                        //beatmapステータス
                        command.Parameters.Add(new SQLiteParameter("@songName", gameStatus.songName));
                        command.Parameters.Add(new SQLiteParameter("@songSubName", gameStatus.songSubName));
                        command.Parameters.Add(new SQLiteParameter("@songAuthorName", gameStatus.songAuthorName));
                        command.Parameters.Add(new SQLiteParameter("@levelAuthorName", gameStatus.levelAuthorName));
                        command.Parameters.Add(new SQLiteParameter("@length", gameStatus.length));
                        command.Parameters.Add(new SQLiteParameter("@songHash", gameStatus.songHash));
                        command.Parameters.Add(new SQLiteParameter("@levelId", gameStatus.levelId));
                        command.Parameters.Add(new SQLiteParameter("@songBPM", gameStatus.songBPM));
                        command.Parameters.Add(new SQLiteParameter("@noteJumpSpeed", gameStatus.noteJumpSpeed));
                        command.Parameters.Add(new SQLiteParameter("@songTimeOffset", gameStatus.songTimeOffset));
                        if (gameStatus.start == 0)
                            command.Parameters.Add(new SQLiteParameter("@start", null));
                        else
                            command.Parameters.Add(new SQLiteParameter("@start", gameStatus.start));
                        if (gameStatus.paused == 0)
                            command.Parameters.Add(new SQLiteParameter("@paused", null));
                        else
                            command.Parameters.Add(new SQLiteParameter("@paused", gameStatus.paused));
                        command.Parameters.Add(new SQLiteParameter("@difficulty", gameStatus.difficulty));
                        command.Parameters.Add(new SQLiteParameter("@notesCount", gameStatus.notesCount));
                        command.Parameters.Add(new SQLiteParameter("@bombsCount", gameStatus.bombsCount));
                        command.Parameters.Add(new SQLiteParameter("@obstaclesCount", gameStatus.obstaclesCount));
                        command.Parameters.Add(new SQLiteParameter("@maxScore", gameStatus.maxScore));
                        command.Parameters.Add(new SQLiteParameter("@maxRank", gameStatus.maxRank.ToString()));
                        command.Parameters.Add(new SQLiteParameter("@environmentName", gameStatus.environmentName));
                        double scorePercentage;
                        if (gameStatus.currentMaxScore == 0)
                            scorePercentage = 0.0;
                        else
                            scorePercentage = double.Parse(String.Format("{0:F2}", ((double)gameStatus.score / (double)gameStatus.currentMaxScore) * 100.0));
                        command.Parameters.Add(new SQLiteParameter("@scorePercentage", scorePercentage));
                        //performanceステータス
                        command.Parameters.Add(new SQLiteParameter("@score", gameStatus.score));
                        command.Parameters.Add(new SQLiteParameter("@currentMaxScore", gameStatus.currentMaxScore));
                        command.Parameters.Add(new SQLiteParameter("@rank", RankModel.GetRankName(gameStatus.rank)));
                        command.Parameters.Add(new SQLiteParameter("@passedNotes", gameStatus.passedNotes));
                        command.Parameters.Add(new SQLiteParameter("@hitNotes", gameStatus.hitNotes));
                        command.Parameters.Add(new SQLiteParameter("@missedNotes", gameStatus.missedNotes));
                        command.Parameters.Add(new SQLiteParameter("@lastNoteScore", gameStatus.lastNoteScore));
                        command.Parameters.Add(new SQLiteParameter("@passedBombs", gameStatus.passedBombs));
                        command.Parameters.Add(new SQLiteParameter("@hitBombs", gameStatus.hitBombs));
                        command.Parameters.Add(new SQLiteParameter("@combo", gameStatus.combo));
                        command.Parameters.Add(new SQLiteParameter("@maxCombo", gameStatus.maxCombo));
                        //modステータス
                        command.Parameters.Add(new SQLiteParameter("@multiplier", gameStatus.modifierMultiplier));
                        if (gameStatus.modObstacles == GameplayModifiers.EnabledObstacleType.NoObstacles)
                            command.Parameters.Add(new SQLiteParameter("@obstacles", 0));
                        else
                            command.Parameters.Add(new SQLiteParameter("@obstacles", gameStatus.modObstacles.ToString()));
                        command.Parameters.Add(new SQLiteParameter("@instaFail", gameStatus.modInstaFail ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@noFail", gameStatus.modNoFail ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@batteryEnergy", gameStatus.modBatteryEnergy ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@disappearingArrows", gameStatus.modDisappearingArrows ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@noBombs", gameStatus.modNoBombs ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@songSpeed", gameStatus.modSongSpeed.ToString()));
                        command.Parameters.Add(new SQLiteParameter("@songSpeedMultiplier", gameStatus.songSpeedMultiplier));
                        command.Parameters.Add(new SQLiteParameter("@noArrows", gameStatus.modNoArrows ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@ghostNotes", gameStatus.modGhostNotes ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@failOnSaberClash", gameStatus.modFailOnSaberClash ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@strictAngles", gameStatus.modStrictAngles ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@fastNotes", gameStatus.modFastNotes ? 1 : 0));
                        //playerSettingsステータス
                        command.Parameters.Add(new SQLiteParameter("@staticLights", gameStatus.staticLights ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@leftHanded", gameStatus.leftHanded ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@playerHeight", gameStatus.playerHeight));
                        command.Parameters.Add(new SQLiteParameter("@reduceDebris", gameStatus.reduceDebris ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@noHUD", gameStatus.noHUD ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@advancedHUD", gameStatus.advancedHUD ? 1 : 0));
                        command.Parameters.Add(new SQLiteParameter("@autoRestart", gameStatus.autoRestart ? 1 : 0));
                        var result = command.ExecuteNonQuery();
                        // データ更新できない場合
                        if (result != 1) 
                            Logger.Error("DB NoteScore MovieCutRecord Error");
                        #endregion

                        #region // NoteScore
                        // トランザクションを開始します。
                        transaction = this._connection.BeginTransaction();
                        command.CommandText = @"
                                            INSERT INTO NoteScore(
                                                time,
                                                cutTime,
                                                startTime,
                                                event,
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
                        int endIndex = gameStatus.noteIndex;
                        gameStatus.lastNoteId = 0;
                        for (gameStatus.noteIndex = 0; gameStatus.noteIndex < endIndex; gameStatus.noteIndex++) {
                            var noteScore = gameStatus.NoteDataGet();
                            command.Parameters.Add(new SQLiteParameter("@time", noteScore.time));
                            command.Parameters.Add(new SQLiteParameter("@cutTime", noteScore.cutTime));
                            command.Parameters.Add(new SQLiteParameter("@startTime", gameStatus.startTime));
                            command.Parameters.Add(new SQLiteParameter("@event", noteScore.bs_event.GetDescription()));
                            command.Parameters.Add(new SQLiteParameter("@score", noteScore.score));
                            command.Parameters.Add(new SQLiteParameter("@currentMaxScore", noteScore.currentMaxScore));
                            command.Parameters.Add(new SQLiteParameter("@rank", RankModel.GetRankName(noteScore.rank)));
                            command.Parameters.Add(new SQLiteParameter("@passedNotes", noteScore.passedNotes));
                            command.Parameters.Add(new SQLiteParameter("@hitNotes", noteScore.hitNotes));
                            command.Parameters.Add(new SQLiteParameter("@missedNotes", noteScore.missedNotes));
                            command.Parameters.Add(new SQLiteParameter("@lastNoteScore", noteScore.lastNoteScore));
                            command.Parameters.Add(new SQLiteParameter("@passedBombs", noteScore.passedBombs));
                            command.Parameters.Add(new SQLiteParameter("@hitBombs", noteScore.hitBombs));
                            command.Parameters.Add(new SQLiteParameter("@combo", noteScore.combo));
                            command.Parameters.Add(new SQLiteParameter("@maxCombo", noteScore.maxCombo));
                            command.Parameters.Add(new SQLiteParameter("@multiplier", noteScore.multiplier));
                            command.Parameters.Add(new SQLiteParameter("@multiplierProgress", noteScore.multiplierProgress));
                            command.Parameters.Add(new SQLiteParameter("@batteryEnergy", noteScore.batteryEnergy));
                            command.Parameters.Add(new SQLiteParameter("@noteID", gameStatus.GetNoteId()));
                            string noteType = noteScore.colorType == ColorType.None ? "Bomb" : noteScore.colorType == ColorType.ColorA ? "NoteA" : noteScore.colorType == ColorType.ColorB ? "NoteB" : noteScore.colorType.ToString();
                            command.Parameters.Add(new SQLiteParameter("@noteType", noteType));
                            command.Parameters.Add(new SQLiteParameter("@noteCutDirection", noteScore.noteCutDirection.ToString()));
                            command.Parameters.Add(new SQLiteParameter("@noteLine", noteScore.noteLine));
                            command.Parameters.Add(new SQLiteParameter("@noteLayer", (int)noteScore.noteLayer));
                            command.Parameters.Add(new SQLiteParameter("@speedOK", noteScore.speedOK == true ? 1 : 0));
                            command.Parameters.Add(new SQLiteParameter("@directionOK", noteScore.directionOK == null ? null : (noteScore.directionOK == true ? 1 : (int?)0)));
                            command.Parameters.Add(new SQLiteParameter("@saberTypeOK", noteScore.saberTypeOK == null ? null : (noteScore.saberTypeOK == true ? 1 : (int?)0)));
                            command.Parameters.Add(new SQLiteParameter("@wasCutTooSoon", noteScore.wasCutTooSoon == true ? 1 : 0));
                            command.Parameters.Add(new SQLiteParameter("@initialScore", noteScore.initialScore));
                            command.Parameters.Add(new SQLiteParameter("@beforeScore", noteScore.finalScore - noteScore.cutDistanceScore));
                            command.Parameters.Add(new SQLiteParameter("@afterScore", noteScore.finalScore - noteScore.initialScore));
                            command.Parameters.Add(new SQLiteParameter("@cutDistanceScore", noteScore.cutDistanceScore));
                            command.Parameters.Add(new SQLiteParameter("@finalScore", noteScore.finalScore));
                            command.Parameters.Add(new SQLiteParameter("@cutMultiplier", noteScore.cutMultiplier));
                            command.Parameters.Add(new SQLiteParameter("@saberSpeed", noteScore.saberSpeed));
                            command.Parameters.Add(new SQLiteParameter("@saberDirX", noteScore.saberDirX));
                            command.Parameters.Add(new SQLiteParameter("@saberDirY", noteScore.saberDirY));
                            command.Parameters.Add(new SQLiteParameter("@saberDirZ", noteScore.saberDirZ));
                            command.Parameters.Add(new SQLiteParameter("@saberType", noteScore.saberType.ToString()));
                            command.Parameters.Add(new SQLiteParameter("@swingRating", noteScore.swingRating));
                            command.Parameters.Add(new SQLiteParameter("@swingRatingFullyCut", noteScore.swingRatingFullyCut));
                            command.Parameters.Add(new SQLiteParameter("@timeDeviation", noteScore.timeDeviation));
                            command.Parameters.Add(new SQLiteParameter("@cutDirectionDeviation", noteScore.cutDirectionDeviation));
                            command.Parameters.Add(new SQLiteParameter("@cutPointX", noteScore.cutPointX));
                            command.Parameters.Add(new SQLiteParameter("@cutPointY", noteScore.cutPointY));
                            command.Parameters.Add(new SQLiteParameter("@cutPointZ", noteScore.cutPointZ));
                            command.Parameters.Add(new SQLiteParameter("@cutNormalX", noteScore.cutNormalX));
                            command.Parameters.Add(new SQLiteParameter("@cutNormalY", noteScore.cutNormalY));
                            command.Parameters.Add(new SQLiteParameter("@cutNormalZ", noteScore.cutNormalZ));
                            command.Parameters.Add(new SQLiteParameter("@cutDistanceToCenter", noteScore.cutDistanceToCenter));
                            command.Parameters.Add(new SQLiteParameter("@timeToNextBasicNote", noteScore.timeToNextBasicNote));
                            result = command.ExecuteNonQuery();
                            // データ更新できない場合
                            if (result != 1) {
                                Logger.Error("DB NoteScore INSERT Error");
                                transaction.Rollback();
                                transaction = null;
                                break;
                            }
                        }
                        endIndex = gameStatus.energyIndex;
                        for (gameStatus.energyIndex = 0; gameStatus.energyIndex < endIndex; gameStatus.energyIndex++) {
                            var energyData = gameStatus.EnergyDataGet();
                            command.CommandText = "INSERT INTO EnergyChange(time, energy) VALUES (@time, @energy)";
                            command.Parameters.Add(new SQLiteParameter("@time", energyData.time));
                            command.Parameters.Add(new SQLiteParameter("@energy", energyData.energy));
                            result = command.ExecuteNonQuery();
                            // データ更新できない場合
                            if (result != 1) {
                                Logger.Error("DB EnergyChange INSERT Error");
                                transaction.Rollback();
                                transaction = null;
                                break;
                            }
                        }
                        if (transaction != null)
                            transaction.Commit();
                        #endregion
                    }
                }
                // 例外が発生した場合
                catch (Exception e) {
                    Logger.Error("DB NoteScore INSERT Error " + e.Message);
                    // トランザクションが有効な場合
                    if (transaction != null)
                        transaction.Rollback();
                }
                finally {
                    this._connection.Close();
                }
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
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
                        DbColumnCheck(command, "NoteScore", "beforeScore", "INTEGER");
                    }
                }
                catch (Exception e) {
                    Logger.Error(e);
                }
                finally {
                    this._connection.Close();
                }
            }
        }

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
