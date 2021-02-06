using DataRecorder.Interfaces;
using DataRecorder.Util;
using DataRecorder.Enums;
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
using System.Threading;

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
        /// <summary>
        /// Pauseイベント発生時
        /// </summary>
        public void OnGamePause()
        {
            this._gameStatus.cleared = BeatSaberEvent.Pause;
            this._gameStatus.paused = Utility.GetCurrentTime();
            this._gameStatus.endTime = this._gameStatus.paused;
            this._gameStatus.pauseCount++;
            while (this._repository.pauseEventAddFlag != null) {
                Thread.Sleep(1);
                this._repository.DbTimeoutCheck();
            }
            this._repository.pauseEventAddFlag = BeatSaberEvent.Pause;
        }

        /// <summary>
        /// PauseからContinueイベント発生時
        /// </summary>
        public void OnGameResume()
        {
            this._gameStatus.start = Utility.GetCurrentTime() - (long)(audioTimeSyncController.songTime * 1000f / this._gameStatus.songSpeedMultiplier);
            this._gameStatus.paused = 0;
            while (this._repository.pauseEventAddFlag != null) {
                Thread.Sleep(1);
                this._repository.DbTimeoutCheck();
            }
            this._repository.pauseEventAddFlag = BeatSaberEvent.Resume;
        }

        /// <summary>
        /// ノーツ(ボム含)のカットイベント発生時
        /// </summary>
        /// <param name="noteData"></param>
        /// <param name="noteCutInfo"></param>
        /// <param name="multiplier"></param>
        public void OnNoteWasCut(NoteData noteData, NoteCutInfo noteCutInfo, int multiplier)
        {
            // Event order: combo, multiplier, scoreController.noteWasCut, (LateUpdate) scoreController.scoreDidChange, afterCut, (LateUpdate) scoreController.scoreDidChange

            var gameStatus = this._gameStatus;
            var notescore = gameStatus.NoteDataGet();

            if (noteData.colorType != ColorType.None && noteCutInfo.allIsOK) {
                gameStatus.passedNotes++;
                gameStatus.hitNotes++;

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

                notescore.bs_event = BeatSaberEvent.BombCut;
            }
            else {
                gameStatus.passedNotes++;
                gameStatus.missedNotes++;

                notescore.bs_event = BeatSaberEvent.NoteMissed;
            }
            gameStatus.NoteDataIndexUp();
        }

        /// <summary>
        /// ノーツの正常カット時の点数計算完了イベント発生時
        /// </summary>
        /// <param name="acsb"></param>
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
            notescore.bs_event = BeatSaberEvent.NoteFullyCut;
            this._gameStatus.NoteDataIndexUp();

            acsb.didFinishEvent -= OnNoteWasFullyCut;
        }

        /// <summary>
        /// ノーツ(ボム含)のミス(素通り)イベント発生時
        /// </summary>
        /// <param name="noteData"></param>
        /// <param name="multiplier"></param>
        public void OnNoteWasMissed(NoteData noteData, int multiplier)
        {
            // Event order: combo, multiplier, scoreController.noteWasMissed, (LateUpdate) scoreController.scoreDidChange

            var notescore = this._gameStatus.NoteDataGet();

            SetNoteCutStatus(noteData);

            if (noteData.colorType == ColorType.None) {
                this._gameStatus.passedBombs++;

                notescore.bs_event = BeatSaberEvent.BombMissed;
            }
            else {
                this._gameStatus.passedNotes++;
                this._gameStatus.missedNotes++;

                notescore.bs_event = BeatSaberEvent.NoteMissed;
            }
            this._gameStatus.NoteDataIndexUp();
        }

        /// <summary>
        /// スコア変更イベント発生時
        /// </summary>
        /// <param name="scoreBeforeMultiplier"></param>
        /// <param name="scoreAfterMultiplier"></param>
        public void OnScoreDidChange(int scoreBeforeMultiplier, int scoreAfterMultiplier)
        {
            this._gameStatus.rawScore = scoreBeforeMultiplier;
            this._gameStatus.score = scoreAfterMultiplier;
            this.UpdateCurrentMaxScore();
        }

        /// <summary>
        /// コンボ数変更イベント発生時
        /// </summary>
        /// <param name="combo"></param>
        public void OnComboDidChange(int combo)
        {
            this._gameStatus.combo = combo;
            // public int ScoreController#maxCombo
            this._gameStatus.maxCombo = scoreController.maxCombo;
        }

        /// <summary>
        /// マルチプレイヤーモードでの終了時
        /// </summary>
        /// <param name="obj"></param>
        private void OnMultiplayerLevelFinished(LevelCompletionResults obj)
        {
            switch (obj.levelEndStateType) {
                case LevelCompletionResults.LevelEndStateType.Failed:
                    OnLevelFailed();
                    break;
                default:
                    OnLevelFinished();
                    break;
            }
        }

        /// <summary>
        /// エネルギー値変更イベント発生時
        /// </summary>
        /// <param name="energy"></param>
        public void OnEnergyDidChange(float energy)
        {
            if (this._gameStatus.softFailed == false) {
                this._gameStatus.batteryEnergy = gameEnergyCounter.batteryEnergy;
                this._gameStatus.energy = energy;

                var energyData = this._gameStatus.EnergyDataGet();
                energyData.energy = energy;
                energyData.time = Utility.GetCurrentTime();
                this._gameStatus.EnergyDataIndexUp();
            }
        }

        /// <summary>
        /// コンボ乗数変更イベント発生時
        /// </summary>
        /// <param name="multiplier"></param>
        /// <param name="multiplierProgress"></param>
        public void OnMultiplierDidChange(int multiplier, float multiplierProgress)
        {
            this._gameStatus.multiplier = multiplier;
            this._gameStatus.multiplierProgress = multiplierProgress;
        }

        /// <summary>
        /// 譜面のクリアイベント発生時
        /// </summary>
        public void OnLevelFinished()
        {
            if(this._gameStatus.softFailed == false)
                this._gameStatus.cleared = BeatSaberEvent.Finished;
            this._gameStatus.endTime = Utility.GetCurrentTime();
            this._gameStatus.endFlag = 1;
        }

        /// <summary>
        /// 譜面のフェイルイベント発生時
        /// </summary>
        public void OnLevelFailed()
        {
            this._gameStatus.cleared = BeatSaberEvent.Failed;
            this._gameStatus.endTime = Utility.GetCurrentTime();
            this._gameStatus.endFlag = 1;
        }

        /// <summary>
        /// noFail時のフェイルイベント発生時
        /// </summary>
        public void OnEnergyDidReach0Event()
        {
            if (this._gameStatus.modNoFail) {
                this._gameStatus.softFailed = true;
                this._gameStatus.cleared = BeatSaberEvent.SoftFailed;
                UpdateModMultiplier();
                UpdateCurrentMaxScore();
                long nowTime = Utility.GetCurrentTime();
                var energyData = this._gameStatus.EnergyDataGet();
                energyData.time = nowTime;
                energyData.energy = 0;
                this._gameStatus.EnergyDataIndexUp();
                energyData = this._gameStatus.EnergyDataGet();
                energyData.time = nowTime + 1;
                energyData.energy = -1;
                this._gameStatus.EnergyDataIndexUp();
            }
        }

        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        /// <summary>
        /// ノーツカットの情報取得
        /// </summary>
        /// <param name="noteData"></param>
        /// <param name="noteCutInfo"></param>
        /// <param name="initialCut"></param>
        private void SetNoteCutStatus(NoteData noteData, NoteCutInfo noteCutInfo = null, bool initialCut = true)
        {
            var gameStatus = this._gameStatus;
            var notescore = gameStatus.NoteDataGet();

            // Backwards compatibility for <1.12.1
            notescore.noteTime = noteData.time;
            notescore.duration = noteData.duration;
            notescore.colorType = noteData.colorType;
            notescore.noteCutDirection = noteData.cutDirection;
            notescore.noteLine = noteData.lineIndex;
            notescore.noteLayer = noteData.noteLineLayer;
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
                notescore.saberType = noteCutInfo.saberType;
                notescore.swingRating = noteCutInfo.swingRatingCounter == null ? -1 : noteCutInfo.swingRatingCounter.beforeCutRating;
                notescore.swingRatingFullyCut = noteCutInfo.swingRatingCounter == null ? -1 : initialCut ? 0 : noteCutInfo.swingRatingCounter.afterCutRating;
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
        /// <summary>
        /// コンボ乗数、最大スコア・ランクの更新
        /// </summary>
        private void UpdateModMultiplier()
        {
            this._gameStatus.modifierMultiplier = gameplayModifiersSO.GetTotalMultiplier(gameplayModifiers, gameEnergyCounter.energy);
            this._gameStatus.maxScore = gameplayModifiersSO.MaxModifiedScoreForMaxRawScore(ScoreModel.MaxRawScoreForNumberOfNotes(gameplayCoreSceneSetupData.difficultyBeatmap.beatmapData.cuttableNotesType), gameplayModifiers, gameplayModifiersSO, gameEnergyCounter.energy);
            this._gameStatus.maxRank = RankModelHelper.MaxRankForGameplayModifiers(gameplayModifiers, gameplayModifiersSO, gameEnergyCounter.energy);
        }

        /// <summary>
        /// 現在の最大スコア・ランクの更新
        /// </summary>
        private void UpdateCurrentMaxScore()
        {
            GameStatus gameStatus = this._gameStatus;
            int currentMaxScoreBeforeMultiplier = ScoreModel.MaxRawScoreForNumberOfNotes(gameStatus.passedNotes);
            gameStatus.currentMaxScore = gameplayModifiersSO.MaxModifiedScoreForMaxRawScore(currentMaxScoreBeforeMultiplier, gameplayModifiers, gameplayModifiersSO, gameEnergyCounter.energy);
            gameStatus.rank = RankModel.GetRankForScore(gameStatus.rawScore, gameStatus.score, currentMaxScoreBeforeMultiplier, gameStatus.currentMaxScore);
        }

        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private bool disposedValue;
        private bool initializeError;

        private GameplayCoreSceneSetupData gameplayCoreSceneSetupData;
        private PauseController pauseController;
        private ScoreController scoreController;
        private GameplayModifiers gameplayModifiers;
        private AudioTimeSyncController audioTimeSyncController;
        private GameSongController gameSongController;
        private GameEnergyCounter gameEnergyCounter;
        private MultiplayerLocalActivePlayerFacade multiplayerLocalActivePlayerFacade;
        private ILevelEndActions levelEndActions;
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
            Logger.Debug("Constractor call");
            this.initializeError = true;
            try {
                this.gameplayCoreSceneSetupData = container.Resolve<GameplayCoreSceneSetupData>();
                this.scoreController = container.Resolve<ScoreController>();
                this.gameplayModifiers = container.Resolve<GameplayModifiers>();
                this.audioTimeSyncController = container.Resolve<AudioTimeSyncController>();
                this.gameSongController = container.Resolve<GameSongController>();
                this.gameEnergyCounter = container.Resolve<GameEnergyCounter>();
                this.gameplayModifiersSO = this.scoreController.GetField<GameplayModifiersModelSO, ScoreController>("_gameplayModifiersModel");
            }
            catch (Exception e) {
                Logger.Error(e);
                return;
            }
            this.pauseController = container.TryResolve<PauseController>();
            this.levelEndActions = container.TryResolve<ILevelEndActions>();
            this.multiplayerLocalActivePlayerFacade = container.TryResolve<MultiplayerLocalActivePlayerFacade>();
            this.initializeError = false;
        }

        /// <summary>
        /// Zenjectにより<see cref="Constractor(DiContainer)"/>のあとに呼ばれる関数です。
        /// </summary>
        public void Initialize()
        {
            if (this.initializeError) return;
            //初期化処理
            while (this._repository.playDataAddFlag) {
                Thread.Sleep(1);
                this._repository.DbTimeoutCheck();
            }
            this._gameStatus.ResetGameStatus();

            //各種イベントの追加
            // FIXME: 曲が終わったときには、このすべての参照先をきれいにしておく必要があります。(FIXME: i should probably clean references to all this when song is over)

            // イベントリスナーの登録 (Register event listeners)
            // マルチプレイヤーでは PauseController が存在しない (PauseController doesn't exist in multiplayer)
            Logger.Info("scoreController=" + this.scoreController);
            if (this.pauseController == null) {
                this._gameStatus.multiplayer = true;
            }
            else {
                Logger.Info("pauseController=" + this.pauseController);
                // public event Action PauseController#didPauseEvent;
                this.pauseController.didPauseEvent += this.OnGamePause;
                // public event Action PauseController#didResumeEvent;
                this.pauseController.didResumeEvent += this.OnGameResume;
            }
            // public ScoreController#noteWasCutEvent<NoteData, NoteCutInfo, int multiplier> // AfterCutScoreBufferが作成された後に呼び出される (called after AfterCutScoreBuffer is created)
            this.scoreController.noteWasCutEvent += this.OnNoteWasCut;
            // public ScoreController#noteWasMissedEvent<NoteData, int multiplier>
            this.scoreController.noteWasMissedEvent += this.OnNoteWasMissed;
            // public ScoreController#scoreDidChangeEvent<int, int> // score
            this.scoreController.scoreDidChangeEvent += this.OnScoreDidChange;
            // public ScoreController#comboDidChangeEvent<int> // combo
            this.scoreController.comboDidChangeEvent += this.OnComboDidChange;
            // public ScoreController#multiplierDidChangeEvent<int, float> // multiplier, progress [0..1]
            this.scoreController.multiplierDidChangeEvent += this.OnMultiplierDidChange;
            // public event Action GameSongController#songDidFinishEvent;
            this.gameSongController.songDidFinishEvent += this.OnLevelFinished;
            // public event Action GameEnergyCounter#gameEnergyDidReach0Event;
            this.gameEnergyCounter.gameEnergyDidReach0Event += this.OnEnergyDidReach0Event;
            // public GameEnergyCounter#gameEnergyDidChangeEvent<float> // energy
            this.gameEnergyCounter.gameEnergyDidChangeEvent += this.OnEnergyDidChange;

            if (this.multiplayerLocalActivePlayerFacade != null) {
                this.multiplayerLocalActivePlayerFacade.playerDidFinishEvent += this.OnMultiplayerLevelFinished;
                this._gameStatus.multiplayer = true;
            }
            if (this.levelEndActions != null) {
                this.levelEndActions.levelFailedEvent += this.OnLevelFinished;
                this.levelEndActions.levelFailedEvent += this.OnLevelFailed;
            }
            //BeatMapデータの登録
            this._gameStatus.scene = BeatSaberScene.Song;

            IDifficultyBeatmap diff = gameplayCoreSceneSetupData.difficultyBeatmap;
            IBeatmapLevel level = diff.level;

            this._gameStatus.partyMode = Gamemode.IsPartyActive;
            this._gameStatus.mode = BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName;

            gameplayModifiers = gameplayCoreSceneSetupData.gameplayModifiers;
            PlayerSpecificSettings playerSettings = gameplayCoreSceneSetupData.playerSpecificSettings;
            PracticeSettings practiceSettings = gameplayCoreSceneSetupData.practiceSettings;

            float songSpeedMul = gameplayModifiers.songSpeedMul;
            if (practiceSettings != null) songSpeedMul = practiceSettings.songSpeedMul;

            // HTTPStatus 1.12.1以下との下位互換性のために、NoteDataからidへのマッピングを生成します。 [Generate NoteData to id mappings for backwards compatiblity with <1.12.1]
            var beatmapObjectsData = diff.beatmapData.beatmapObjectsData;
            foreach (BeatmapObjectData beatmapObjectData in beatmapObjectsData) {
                if (beatmapObjectData is NoteData noteData) {
                    var mapdata = this._gameStatus.MapDataGet();
                    mapdata.time = noteData.time;
                    mapdata.lineIndex = noteData.lineIndex;
                    mapdata.noteLineLayer = noteData.noteLineLayer;
                    mapdata.colorType = noteData.colorType;
                    mapdata.cutDirection = noteData.cutDirection;
                    mapdata.duration = noteData.duration;
                    this._gameStatus.MapDataIndexUp();
                }
            }

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

            this.UpdateModMultiplier();

            this._gameStatus.songSpeedMultiplier = songSpeedMul;
            this._gameStatus.batteryLives = gameEnergyCounter.batteryLives;

            this._gameStatus.modObstacles = gameplayModifiers.enabledObstacleType;
            this._gameStatus.modInstaFail = gameplayModifiers.instaFail;
            this._gameStatus.modNoFail = gameplayModifiers.noFailOn0Energy;
            this._gameStatus.modBatteryEnergy = gameplayModifiers.energyType == GameplayModifiers.EnergyType.Battery;
            this._gameStatus.modDisappearingArrows = gameplayModifiers.disappearingArrows;
            this._gameStatus.modNoBombs = gameplayModifiers.noBombs;
            this._gameStatus.modSongSpeed = gameplayModifiers.songSpeed;
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
            this._gameStatus.cleared = BeatSaberEvent.Menu;
            this._gameStatus.NoteDataSizeCheck();
            this._gameStatus.EnergyDataSizeCheck();
            this._gameStatus.MapDataSizeCheck();
            Logger.Debug("Initialize end");
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
                    try {
                        this._repository.playDataAddFlag = true;

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

                        if (this.multiplayerLocalActivePlayerFacade != null) {
                            this.multiplayerLocalActivePlayerFacade.playerDidFinishEvent -= this.OnMultiplayerLevelFinished;
                            this.multiplayerLocalActivePlayerFacade = null;
                        }

                        if (this.levelEndActions != null) {
                            this.levelEndActions.levelFailedEvent -= this.OnLevelFinished;
                            this.levelEndActions.levelFailedEvent -= this.OnLevelFailed;
                        }
                        //CleanUpMultiplayer();

                        if (this.gameEnergyCounter != null) {
                            this.gameEnergyCounter.gameEnergyDidChangeEvent -= this.OnEnergyDidChange;
                            this.gameEnergyCounter.gameEnergyDidReach0Event -= this.OnEnergyDidReach0Event;
                        }

                        if (this.gameSongController != null) {
                            this.gameSongController.songDidFinishEvent -= this.OnLevelFinished;
                        }

                    }
                    catch (Exception e) {
                        Logger.Error(e);
                    }
                    Logger.Debug("dispose end");
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
