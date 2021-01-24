using DataRecorder.Interfaces;
using DataRecorder.Util;
using BS_Utils.Gameplay;
using IPA.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DataRecorder.Models
{
    /// <summary>
    /// ゲーム中のスコアを記録するクラスです。
    /// 初期化と破棄はZenjectがいい感じにやってくれます。
    /// </summary>
    public class ScoreManager : IInitializable, IDisposable
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド

        //public void OnMultiplayerStateChanged(MultiplayerController.State state)
        //{
        //	Logger.Info("multiplayer state = " + state);

        //	if (state == MultiplayerController.State.Intro) {
        //		// Gameplay controllers don't exist on the inisial load of GameCore, s owe need to delay it until later
        //		// XXX: check that this isn't fired too late
        //		//HandleSongStart();
        //	}
        //}

        //public void OnMultiplayerDisconnected(DisconnectedReason reason)
        //{
        //	CleanUpMultiplayer();

        //	// XXX: this should only be fired if we go from multiplayer lobby to menu and there's no scene transition because of it. gotta prevent duplicates too
        //	// HandleMenuStart();
        //}

        public void OnGamePause()
        {
            this._gameStatus.cleared = "pause";
            this._gameStatus.paused = Utility.GetCurrentTime();
            this._gameStatus.endTime = this._gameStatus.paused;
            this._gameStatus.pauseCount++;
            this._repository.PaseEventAdd(this._gameStatus.cleared);
        }

        public void OnGameResume()
        {
            this._gameStatus.start = Utility.GetCurrentTime() - (long)(audioTimeSyncController.songTime * 1000f / this._gameStatus.songSpeedMultiplier);
            this._gameStatus.paused = 0;
            this._repository.PaseEventAdd("resume");
        }

        public void OnNoteWasCut(NoteData noteData, NoteCutInfo noteCutInfo, int multiplier)
        {
            // Event order: combo, multiplier, scoreController.noteWasCut, (LateUpdate) scoreController.scoreDidChange, afterCut, (LateUpdate) scoreController.scoreDidChange

            var gameStatus = this._gameStatus;
            var notescore = this._gameStatus.NoteDataGet();

            if (noteData.colorType != ColorType.None && noteCutInfo.allIsOK) {
                gameStatus.passedNotes++;
                gameStatus.hitNotes++;
                notescore.passedNotes++;
                notescore.hitNotes++;

                List<CutScoreBuffer> list = (List<CutScoreBuffer>)afterCutScoreBuffersField.GetValue(scoreController);
                foreach (CutScoreBuffer acsb in list) {
                    if (noteCutInfoField.GetValue(acsb) == noteCutInfo) {
                        // public CutScoreBuffer#didFinishEvent<CutScoreBuffer>
                        noteCutMapping.TryAdd(noteCutInfo, noteData);
                        noteCutTiming.TryAdd(noteCutInfo, Utility.GetCurrentTime());
                        acsb.didFinishEvent += OnNoteWasFullyCut;
                        break;
                    }
                }
                return;
            }

            SetNoteCutStatus(noteData, noteCutInfo, true);

            int beforeCutScore = 0;
            int afterCutScore = 0;
            int cutDistanceScore = 0;

            ScoreModel.RawScoreWithoutMultiplier(noteCutInfo, out beforeCutScore, out afterCutScore, out cutDistanceScore);

            notescore.initialScore = beforeCutScore + cutDistanceScore;
            notescore.finalScore = -1;
            notescore.cutDistanceScore = cutDistanceScore;
            notescore.cutMultiplier = multiplier;

            if (noteData.colorType == ColorType.None) {
                gameStatus.passedBombs++;
                gameStatus.hitBombs++;
                notescore.passedBombs++;
                notescore.hitBombs++;

                notescore.bs_event = "bombCut";
            }
            else {
                gameStatus.passedNotes++;
                gameStatus.missedNotes++;
                notescore.passedNotes++;
                notescore.missedNotes++;

                notescore.bs_event = "noteMissed";
            }
            //gameStatus.NoteDataIndexUp();　//1-24 ノーツカット記録無し版用コメントアウト
        }

        public void OnNoteWasFullyCut(CutScoreBuffer acsb)
        {
            int beforeCutScore;
            int afterCutScore;
            int cutDistanceScore;

            NoteCutInfo noteCutInfo = (NoteCutInfo)noteCutInfoField.GetValue(acsb);

            if (noteCutMapping.TryRemove(noteCutInfo, out var noteData)) {
                SetNoteCutStatus(noteData, noteCutInfo, false);
            }

            // public static ScoreModel.RawScoreWithoutMultiplier(NoteCutInfo, out int beforeCutRawScore, out int afterCutRawScore, out int cutDistanceRawScore)
            ScoreModel.RawScoreWithoutMultiplier(noteCutInfo, out beforeCutScore, out afterCutScore, out cutDistanceScore);

            int multiplier = (int)cutScoreBufferMultiplierField.GetValue(acsb);

            var notescore = this._gameStatus.NoteDataGet();
            notescore.initialScore = beforeCutScore + cutDistanceScore;
            notescore.finalScore = beforeCutScore + afterCutScore + cutDistanceScore;
            notescore.cutDistanceScore = cutDistanceScore;
            notescore.cutMultiplier = multiplier;

            if (noteCutTiming.TryRemove(noteCutInfo, out var time)) {
                notescore.cutTime = time;
            }
            notescore.bs_event = "noteFullyCut";
            //this._gameStatus.NoteDataIndexUp();　//1-24 ノーツカット記録無し版用コメントアウト

            acsb.didFinishEvent -= OnNoteWasFullyCut;
        }

        private void SetNoteCutStatus(NoteData noteData, NoteCutInfo noteCutInfo = null, bool initialCut = true)
        {
            var gameStatus = this._gameStatus;

            //gameStatus.ResetNoteCut();

            var notescore = gameStatus.NoteDataGet();
            // Backwards compatibility for <1.12.1
            notescore.colorType = (int)noteData.colorType;
            notescore.noteType = noteData.colorType == ColorType.None ? "Bomb" : noteData.colorType == ColorType.ColorA ? "NoteA" : noteData.colorType == ColorType.ColorB ? "NoteB" : noteData.colorType.ToString();
            notescore.noteCutDirection = noteData.cutDirection.ToString();
            notescore.noteLine = noteData.lineIndex;
            notescore.noteLayer = (int)noteData.noteLineLayer;
            // If long notes are ever introduced, this name will make no sense
            notescore.timeToNextBasicNote = noteData.timeToNextColorNote;
            notescore.time = Utility.GetCurrentTime();

            if (noteCutInfo != null) {
                notescore.speedOK = noteCutInfo.speedOK;
                notescore.directionOK = noteCutInfo.directionOK;
                notescore.saberTypeOK = noteCutInfo.saberTypeOK;
                notescore.wasCutTooSoon = noteCutInfo.wasCutTooSoon;
                notescore.saberSpeed = noteCutInfo.saberSpeed;
                notescore.saberDirX = noteCutInfo.saberDir[0];
                notescore.saberDirY = noteCutInfo.saberDir[1];
                notescore.saberDirZ = noteCutInfo.saberDir[2];
                notescore.saberType = noteCutInfo.saberType.ToString();
                notescore.swingRating = noteCutInfo.swingRatingCounter == null ? -1 : noteCutInfo.swingRatingCounter.beforeCutRating;
                notescore.swingRatingFullyCut =  noteCutInfo.swingRatingCounter == null ? -1 : initialCut ? 0 : noteCutInfo.swingRatingCounter.afterCutRating;
                notescore.timeDeviation = noteCutInfo.timeDeviation;
                notescore.cutDirectionDeviation = noteCutInfo.cutDirDeviation;
                notescore.cutPointX = noteCutInfo.cutPoint[0];
                notescore.cutPointY = noteCutInfo.cutPoint[1];
                notescore.cutPointZ = noteCutInfo.cutPoint[2];
                notescore.cutNormalX = noteCutInfo.cutNormal[0];
                notescore.cutNormalY = noteCutInfo.cutNormal[1];
                notescore.cutNormalZ = noteCutInfo.cutNormal[2];
                notescore.cutDistanceToCenter = noteCutInfo.cutDistanceToCenter;
            }
        }

        public void OnNoteWasMissed(NoteData noteData, int multiplier)
        {
            // Event order: combo, multiplier, scoreController.noteWasMissed, (LateUpdate) scoreController.scoreDidChange

            var notescore = this._gameStatus.NoteDataGet();

            SetNoteCutStatus(noteData);

            if (noteData.colorType == ColorType.None) {
                this._gameStatus.passedBombs++;
                notescore.passedBombs++;

                notescore.bs_event = "bombMissed";
            }
            else {
                this._gameStatus.passedNotes++;
                this._gameStatus.missedNotes++;
                notescore.passedNotes++;
                notescore.missedNotes++;

                notescore.bs_event = "noteMissed";
            }
            //this._gameStatus.NoteDataIndexUp();　//1-24 ノーツカット記録無し版用コメントアウト
        }

        public void OnScoreDidChange(int scoreBeforeMultiplier, int scoreAfterMultiplier)
        {
            var gameStatus = this._gameStatus;
            var notescore = this._gameStatus.NoteDataGet();

            gameStatus.score = scoreAfterMultiplier;
            notescore.score = scoreAfterMultiplier;

            int currentMaxScoreBeforeMultiplier = ScoreModel.MaxRawScoreForNumberOfNotes(gameStatus.passedNotes);
            gameStatus.currentMaxScore = gameplayModifiersSO.MaxModifiedScoreForMaxRawScore(currentMaxScoreBeforeMultiplier, gameplayModifiers, gameplayModifiersSO, gameEnergyCounter.energy);
            notescore.currentMaxScore = gameStatus.currentMaxScore;

            RankModel.Rank rank = RankModel.GetRankForScore(scoreBeforeMultiplier, gameStatus.score, currentMaxScoreBeforeMultiplier, gameStatus.currentMaxScore);
            gameStatus.rank = RankModel.GetRankName(rank);
            notescore.rank = gameStatus.rank;
        }

        public void OnComboDidChange(int combo)
        {
            var notescore = this._gameStatus.NoteDataGet();

            this._gameStatus.combo = combo;
            notescore.combo = combo;
            // public int ScoreController#maxCombo
            this._gameStatus.maxCombo = scoreController.maxCombo;
            notescore.maxCombo = this._gameStatus.maxCombo;
        }

        public void OnEnergyDidChange(float energy)
        {
            var notescore = this._gameStatus.NoteDataGet();

            this._gameStatus.batteryEnergy = gameEnergyCounter.batteryEnergy;
            notescore.batteryEnergy = this._gameStatus.batteryEnergy;

            this._gameStatus.energy = energy;
            var energyData = this._gameStatus.EnergyDataGet();
            energyData.energy = energy;
            energyData.time = Utility.GetCurrentTime();
            // this._gameStatus.EnergyDataIndexUp(); //1-24 ノーツカット記録無し版用コメントアウト
        }

        public void OnMultiplierDidChange(int multiplier, float multiplierProgress)
        {
            var notescore = this._gameStatus.NoteDataGet();

            this._gameStatus.multiplier = multiplier;
            notescore.multiplier = multiplier;

            this._gameStatus.multiplierProgress = multiplierProgress;
            notescore.multiplierProgress = multiplierProgress;
        }

        public void OnLevelFinished()
        {
            this._gameStatus.cleared = "finished";
            this._gameStatus.endTime = Utility.GetCurrentTime();
            this._gameStatus.endFlag = 1;
        }

        public void OnLevelFailed()
        {
            this._gameStatus.cleared = "failed";
            this._gameStatus.endTime = Utility.GetCurrentTime();
            this._gameStatus.endFlag = 1;
        }

        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private bool disposedValue;

        private GameplayCoreSceneSetupData gameplayCoreSceneSetupData;
        private PauseController pauseController;
        private ScoreController scoreController;
        private GameplayModifiers gameplayModifiers;
        private AudioTimeSyncController audioTimeSyncController;
        private GameSongController gameSongController;
        private GameEnergyCounter gameEnergyCounter;
        private ConcurrentDictionary<NoteCutInfo, NoteData> noteCutMapping = new ConcurrentDictionary<NoteCutInfo, NoteData>();
        private ConcurrentDictionary<NoteCutInfo, long> noteCutTiming = new ConcurrentDictionary<NoteCutInfo, long>();
        private GameplayModifiersModelSO gameplayModifiersSO;

        /// protected NoteCutInfo CutScoreBuffer._noteCutInfo
        private FieldInfo noteCutInfoField = typeof(CutScoreBuffer).GetField("_noteCutInfo", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        /// protected List<CutScoreBuffer> ScoreController._cutScoreBuffers // contains a list of after cut buffers
        private FieldInfo afterCutScoreBuffersField = typeof(ScoreController).GetField("_cutScoreBuffers", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        /// private int CutScoreBuffer#_multiplier
        private FieldInfo cutScoreBufferMultiplierField = typeof(CutScoreBuffer).GetField("_multiplier", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        [Inject]
        private IRepository _repository;
        [Inject]
        private GameStatus _gameStatus;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        /// <summary>
        /// Zenjetにより最初に呼ばれる関数です。
        /// </summary>
        /// <param name="container"></param>
        [Inject]
        private void Constractor(DiContainer container)
        {
            try {
                pauseController = container.Resolve<PauseController>();
                scoreController = container.Resolve<ScoreController>();
                gameplayModifiers = container.Resolve<GameplayModifiers>();
                audioTimeSyncController = container.Resolve<AudioTimeSyncController>();
                gameSongController = container.Resolve<GameSongController>();
                gameEnergyCounter = container.Resolve<GameEnergyCounter>();
                gameplayModifiersSO = this.scoreController.GetField<GameplayModifiersModelSO, ScoreController>("_gameplayModifiersModel");
            }
            catch (Exception e) {
                Logger.Error(e);
                return;
            }

            //各種イベントの追加
            // FIXME: 曲が終わったときには、このすべての参照先をきれいにしておく必要があります。(FIXME: i should probably clean references to all this when song is over)
            gameplayCoreSceneSetupData = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData;

            Logger.Info("scoreController=" + scoreController);

            // イベントリスナーの登録 (Register event listeners)
            // マルチプレイヤーでは PauseController が存在しない (PauseController doesn't exist in multiplayer)
            if (pauseController != null) {
                Logger.Info("pauseController=" + pauseController);
                // public event Action PauseController#didPauseEvent;
                pauseController.didPauseEvent += this.OnGamePause;
                // public event Action PauseController#didResumeEvent;
                pauseController.didResumeEvent += this.OnGameResume;
            }
            // public ScoreController#noteWasCutEvent<NoteData, NoteCutInfo, int multiplier> // AfterCutScoreBufferが作成された後に呼び出される (called after AfterCutScoreBuffer is created)
            scoreController.noteWasCutEvent += this.OnNoteWasCut;
            // public ScoreController#noteWasMissedEvent<NoteData, int multiplier>
            scoreController.noteWasMissedEvent += this.OnNoteWasMissed;
            // public ScoreController#scoreDidChangeEvent<int, int> // score
            scoreController.scoreDidChangeEvent += this.OnScoreDidChange;
            // public ScoreController#comboDidChangeEvent<int> // combo
            scoreController.comboDidChangeEvent += this.OnComboDidChange;
            // public ScoreController#multiplierDidChangeEvent<int, float> // multiplier, progress [0..1]
            scoreController.multiplierDidChangeEvent += this.OnMultiplierDidChange;
            // public event Action GameSongController#songDidFinishEvent;
            gameSongController.songDidFinishEvent += this.OnLevelFinished;
            // public event Action GameEnergyCounter#gameEnergyDidReach0Event;
            gameEnergyCounter.gameEnergyDidReach0Event += this.OnLevelFailed;
            // public GameEnergyCounter#gameEnergyDidChangeEvent<float> // energy
            gameEnergyCounter.gameEnergyDidChangeEvent += this.OnEnergyDidChange;

            this._gameStatus.ResetGameStatus();
            this._gameStatus.scene = "Song";

            IDifficultyBeatmap diff = gameplayCoreSceneSetupData.difficultyBeatmap;
            IBeatmapLevel level = diff.level;

            this._gameStatus.partyMode = Gamemode.IsPartyActive;
            this._gameStatus.mode = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName;

            gameplayModifiers = gameplayCoreSceneSetupData.gameplayModifiers;
            PlayerSpecificSettings playerSettings = gameplayCoreSceneSetupData.playerSpecificSettings;
            PracticeSettings practiceSettings = gameplayCoreSceneSetupData.practiceSettings;

            float songSpeedMul = gameplayModifiers.songSpeedMul;
            if (practiceSettings != null) songSpeedMul = practiceSettings.songSpeedMul;
            float modifierMultiplier = gameplayModifiersSO.GetTotalMultiplier(gameplayModifiers, gameEnergyCounter.energy);

            this._gameStatus.songName = level.songName;
            this._gameStatus.songSubName = level.songSubName;
            this._gameStatus.songAuthorName = level.songAuthorName;
            this._gameStatus.levelAuthorName = level.levelAuthorName;
            this._gameStatus.songBPM = level.beatsPerMinute;
            this._gameStatus.noteJumpSpeed = diff.noteJumpMovementSpeed;
            // 13は "custom_level_"、40はSHA-1ハッシュの長さを表すマジックナンバー
            this._gameStatus.songHash = level.levelID.StartsWith("custom_level_") && !level.levelID.EndsWith(" WIP") ? level.levelID.Substring(13, 40) : null;
            this._gameStatus.levelId = level.levelID;
            this._gameStatus.songTimeOffset = (long)(level.songTimeOffset * 1000f / songSpeedMul);
            this._gameStatus.length = (long)(level.beatmapLevelData.audioClip.length * 1000f / songSpeedMul);
            this._gameStatus.start = Utility.GetCurrentTime() - (long)(audioTimeSyncController.songTime * 1000f / songSpeedMul);
            if (practiceSettings != null) this._gameStatus.start -= (long)(practiceSettings.startSongTime * 1000f / songSpeedMul);
            this._gameStatus.paused = 0;
            this._gameStatus.difficulty = diff.difficulty.Name();
            this._gameStatus.notesCount = diff.beatmapData.cuttableNotesType;
            this._gameStatus.bombsCount = diff.beatmapData.bombsCount;
            this._gameStatus.obstaclesCount = diff.beatmapData.obstaclesCount;
            this._gameStatus.environmentName = level.environmentInfo.sceneInfo.sceneName;

            this._gameStatus.maxScore = gameplayModifiersSO.MaxModifiedScoreForMaxRawScore(ScoreModel.MaxRawScoreForNumberOfNotes(diff.beatmapData.cuttableNotesType), gameplayModifiers, gameplayModifiersSO, gameEnergyCounter.energy);
            this._gameStatus.maxRank = RankModelHelper.MaxRankForGameplayModifiers(gameplayModifiers, gameplayModifiersSO, gameEnergyCounter.energy).ToString();

            this._gameStatus.modifierMultiplier = modifierMultiplier;
            this._gameStatus.songSpeedMultiplier = songSpeedMul;
            this._gameStatus.batteryLives = gameEnergyCounter.batteryLives;

            this._gameStatus.modObstacles = gameplayModifiers.enabledObstacleType.ToString();
            this._gameStatus.modInstaFail = gameplayModifiers.instaFail;
            this._gameStatus.modNoFail = gameplayModifiers.noFailOn0Energy;
            this._gameStatus.modBatteryEnergy = gameplayModifiers.energyType == GameplayModifiers.EnergyType.Battery;
            this._gameStatus.modDisappearingArrows = gameplayModifiers.disappearingArrows;
            this._gameStatus.modNoBombs = gameplayModifiers.noBombs;
            this._gameStatus.modSongSpeed = gameplayModifiers.songSpeed.ToString();
            this._gameStatus.modNoArrows = gameplayModifiers.noArrows;
            this._gameStatus.modGhostNotes = gameplayModifiers.ghostNotes;
            this._gameStatus.modFailOnSaberClash = gameplayModifiers.failOnSaberClash;
            this._gameStatus.modStrictAngles = gameplayModifiers.strictAngles;
            this._gameStatus.modFastNotes = gameplayModifiers.fastNotes;

            this._gameStatus.staticLights = playerSettings.staticLights;
            this._gameStatus.leftHanded = playerSettings.leftHanded;
            this._gameStatus.playerHeight = playerSettings.playerHeight;
            this._gameStatus.sfxVolume = playerSettings.sfxVolume;
            this._gameStatus.reduceDebris = playerSettings.reduceDebris;
            this._gameStatus.noHUD = playerSettings.noTextsAndHuds;
            this._gameStatus.advancedHUD = playerSettings.advancedHud;
            this._gameStatus.autoRestart = playerSettings.autoRestart;
            this._gameStatus.startTime = Utility.GetCurrentTime();
            this._gameStatus.cleared = "menu";
        }

        /// <summary>
        /// Zenjectにより<see cref="Constractor(DiContainer)"/>のあとに呼ばれる関数です。
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// Zenjectによりゲーム終了後に勝手に呼ばれる関数です。
        /// <see cref="UnityEngine.MonoBehaviour"/>のOnDestroy()のような動きをします。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)

                    //各種イベントの削除
                    Logger.Debug("dispose call");
                    try {
                        this._repository.PlayDataAdd();

                        // 終了前にプレイヤーがマップを離れることで解決しないAfterCutScoreBuffersの参照を解放。(Release references for AfterCutScoreBuffers that don't resolve due to player leaving the map before finishing.)
                        noteCutMapping?.Clear();
                        noteCutTiming?.Clear();

                        if (this.pauseController != null) {
                            this.pauseController.didPauseEvent -= this.OnGamePause;
                            this.pauseController.didResumeEvent -= this.OnGameResume;
                        }

                        if (this.scoreController != null) {
                            this.scoreController.noteWasCutEvent -= this.OnNoteWasCut;
                            this.scoreController.noteWasMissedEvent -= this.OnNoteWasMissed;
                            this.scoreController.scoreDidChangeEvent -= this.OnScoreDidChange;
                            this.scoreController.comboDidChangeEvent -= this.OnComboDidChange;
                            this.scoreController.multiplierDidChangeEvent -= this.OnMultiplierDidChange;
                        }

                        if (this.gameEnergyCounter != null) {
                            this.gameEnergyCounter.gameEnergyDidChangeEvent -= this.OnEnergyDidChange;
                            this.gameEnergyCounter.gameEnergyDidReach0Event -= this.OnLevelFailed;
                        }

                        if (this.gameSongController != null) {
                            this.gameSongController.songDidFinishEvent -= this.OnLevelFinished;
                        }

                    }
                    catch (Exception e) {
                        Logger.Error(e);
                    }
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~ScoreManager()
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
