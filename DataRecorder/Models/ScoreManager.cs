using DataRecorder.Interfaces;
using DataRecorder.Util;
using DataRecorder.Enums;
using IPA.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SiraUtil.Zenject;

namespace DataRecorder.Models
{
    /// <summary>
    /// ゲーム中のスコアを記録するクラスです。
    /// 初期化と破棄はZenjectがいい感じにやってくれます。
    /// </summary>
    public class ScoreManager : IAsyncInitializable, IDisposable, ICutScoreBufferDidFinishReceiver
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
        /// <param name="noteController"></param>
        /// <param name="noteCutInfo"></param>
        public void OnNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
        {
            var noteData = noteCutInfo.noteData;

            if (noteData.scoringType == NoteData.ScoringType.Ignore)
            {
                if (noteData.gameplayType == NoteData.GameplayType.Bomb)
                {
                    // Bombs don't fire any other events: handle the bomb cut immediately
                    HandleCutStart(noteData, noteCutInfo, null);
                }

                return;
            }
            else
            {
                if (!noteCutInfo.allIsOK)
                {
                    // Handle bad cuts here so we don't have to store the noteCutInfo
                    // FIXME: this might result in Combo/ScoreController not getting a chance to process the bad cut, though at least ScoreController handles stuff in LateUpdate anyway, so...
                    HandleCutStart(noteData, noteCutInfo, null);
                }
            }
        }

        /// <summary>
        /// スコア変化のあるノーツカットイベント発生時
        /// </summary>
        /// <param name="scoringElement"></param>
        public void OnScoringForNoteStarted(ScoringElement scoringElement)
        {
            switch (scoringElement)
            {
                case GoodCutScoringElement goodCut: HandleGoodCutScoring(goodCut); break;
                case BadCutScoringElement badCut: HandleBadCutScoring(badCut); break;
                case MissScoringElement miss: HandleMissScoring(miss); break;
                default: throw new Exception($"Unknown ScoringElement type: {scoringElement.GetType()}");
            }
        }

        /// <summary>
        /// グッドカットイベント発生処理
        /// </summary>
        /// <param name="goodCut"></param>
        public void HandleGoodCutScoring(GoodCutScoringElement goodCut)
        {
            var notescore = this._gameStatus.NoteDataGet();
            notescore.cutMultiplier = goodCut.multiplier;

            HandleCutStart(goodCut.noteData, goodCut.cutScoreBuffer.noteCutInfo, goodCut.cutScoreBuffer);
        }

        /// <summary>
        /// バッドカットイベント発生処理
        /// </summary>
        /// <param name="badCut"></param>
        public void HandleBadCutScoring(BadCutScoringElement badCut)
        {
            // NOOP
        }

        /// <summary>
        /// ミスカットイベント発生処理
        /// </summary>
        /// <param name="miss"></param>
        public void HandleMissScoring(MissScoringElement miss)
        {
            // NOOP
        }

        /// <summary>
        /// カットイベント発生処理
        /// </summary>
        /// <param name="noteData"></param>
        /// <param name="noteCutInfo"></param>
        /// <param name="cutScoreBuffer"></param>
        public void HandleCutStart(NoteData noteData, NoteCutInfo noteCutInfo, IReadonlyCutScoreBuffer cutScoreBuffer)
        {
            var nowtime = Utility.GetCurrentTime();
            //Logger.Debug($"\tHandleCutStart\t{nowtime}\t{cutScoreBuffer?.ToString()}\t{noteData.gameplayType.ToString()}\t{noteData.scoringType.ToString()}\t{noteData.cutDirection}\t{noteData.time}\t{noteData.lineIndex}\t{noteData.noteLineLayer}\t{noteData.colorType}");

            var gameStatus = this._gameStatus;
            var notescore = gameStatus.NoteDataGet();

            if (cutScoreBuffer != null && noteData.gameplayType != NoteData.GameplayType.Bomb && noteCutInfo.allIsOK) {
                gameStatus.passedNotes++;
                gameStatus.hitNotes++;
                UpdateCurrentMaxScore();

                if (noteData.scoringType == NoteData.ScoringType.BurstSliderElement)
                {
                    this.SetNoteDataStatus(noteData);
                    this.SetNoteCutStatus(noteCutInfo, noteData, cutScoreBuffer, true);
                    var fixScore = ScoreModel.GetNoteScoreDefinition(noteData.scoringType).fixedCutScore;
                    notescore.initialScore = fixScore;
                    notescore.finalScore = fixScore;
                    notescore.cutDistanceScore = 0;
                    notescore.bs_event = BeatSaberEvent.NoteFullyCut;
                    notescore.cutTime = nowtime;
                    gameStatus.NoteDataIndexUp();
                }
                else
                {
                    noteCutMapping.TryAdd(cutScoreBuffer, new NoteFullyCutData(noteData, noteCutInfo, nowtime));
                    cutScoreBuffer.RegisterDidFinishReceiver(this);
                }
                return;
            }

            this.SetNoteDataStatus(noteData);
            this.SetNoteCutStatus(noteCutInfo, noteData, cutScoreBuffer, true);

            var fixedCutScore = ScoreModel.GetNoteScoreDefinition(noteData.scoringType).fixedCutScore;

            if (noteData.gameplayType == NoteData.GameplayType.Bomb)
            {
                notescore.initialScore = -1;
                notescore.finalScore = -1;
                notescore.cutDistanceScore = -1;

                gameStatus.passedBombs++;
                gameStatus.hitBombs++;

                notescore.bs_event = BeatSaberEvent.BombCut;
            }
            else
            {
                notescore.initialScore = fixedCutScore;
                notescore.finalScore = fixedCutScore;
                notescore.cutDistanceScore = 0;

                gameStatus.passedNotes++;
                gameStatus.missedNotes++;
                UpdateCurrentMaxScore();

                notescore.bs_event = BeatSaberEvent.NoteMissed;
            }
            // XXX: do this in good cut handler
            // notescore.cutMultiplier = multiplier;
            gameStatus.NoteDataIndexUp();
        }

        /// <summary>
        /// ノーツの正常カット時の点数計算完了イベント発生時
        /// </summary>
        /// <param name="csb"></param>
        public void HandleCutScoreBufferDidFinish(CutScoreBuffer csb)
        {
            csb.UnregisterDidFinishReceiver(this);

            var noteFullyCutData = noteCutMapping[csb];
            noteCutMapping.Remove(csb);
            //Logger.Debug($"\tHandleCutScoreBufferDidFinish\t{noteFullyCutData.noteCutTiming}\t{csb?.ToString()}\t{noteFullyCutData.noteData.gameplayType.ToString()}\t{noteFullyCutData.noteData.scoringType.ToString()}\t{noteFullyCutData.noteData.cutDirection}\t{noteFullyCutData.noteData.time}\t{noteFullyCutData.noteData.lineIndex}\t{noteFullyCutData.noteData.noteLineLayer}\t{noteFullyCutData.noteData.colorType}");

            var noteCutInfo = noteFullyCutData.noteCutInfo;

            SetNoteDataStatus(noteFullyCutData.noteData);
            SetNoteCutStatus(noteCutInfo, noteFullyCutData.noteData, csb, false);

            int beforeCutScore = csb.beforeCutScore;
            int afterCutScore = csb.afterCutScore;
            int cutDistanceScore = csb.centerDistanceCutScore;

            var notescore = this._gameStatus.NoteDataGet();
            notescore.initialScore = beforeCutScore + cutDistanceScore;
            notescore.finalScore = beforeCutScore + afterCutScore + cutDistanceScore;
            notescore.cutDistanceScore = cutDistanceScore;
            // XXX: do this in good cut handler
            // notescore.cutMultiplier = csb.multiplier;
            notescore.cutTime = noteFullyCutData.noteCutTiming;
            notescore.bs_event = BeatSaberEvent.NoteFullyCut;
            this._gameStatus.NoteDataIndexUp();
        }

        /// <summary>
        /// ノーツ(ボム含)のミス(素通り)イベント発生時
        /// </summary>
        /// <param name="noteData"></param>
        /// <param name="multiplier"></param>
        public void OnNoteWasMissed(NoteController noteController)
        {
            // XXX: outdated?
            // Event order: combo, multiplier, scoreController.noteWasMissed, (LateUpdate) scoreController.scoreDidChange

            var notescore = this._gameStatus.NoteDataGet();

            var noteData = noteController.noteData;

            SetNoteDataStatus(noteData);

            if (noteData.gameplayType == NoteData.GameplayType.Bomb) {
                this._gameStatus.passedBombs++;

                notescore.bs_event = BeatSaberEvent.BombMissed;
            }
            else {
                this._gameStatus.passedNotes++;
                this._gameStatus.missedNotes++;
                UpdateCurrentMaxScore();

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
        }

        /// <summary>
        /// コンボ数変更イベント発生時
        /// </summary>
        /// <param name="combo"></param>
        public void OnComboDidChange(int combo)
        {
            this._gameStatus.combo = combo;
            // public int ComboController#maxCombo
            this._gameStatus.maxCombo = (this._comboController as ComboController).maxCombo;
        }

        /// <summary>
        /// マルチプレイヤーモードでの終了時
        /// </summary>
        /// <param name="obj"></param>
        private void OnMultiplayerLevelFinished(MultiplayerLevelCompletionResults obj)
        {
            switch (obj.playerLevelEndReason) {
                case MultiplayerLevelCompletionResults.MultiplayerPlayerLevelEndReason.Cleared:
                    this.OnLevelFinished();
                    break;
                default:
                    this.OnLevelFailed();
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
                this._gameStatus.batteryEnergy = _gameEnergyCounter.batteryEnergy;
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
            UpdateCurrentMaxScore();
        }

        /// <summary>
        /// 譜面のフェイルイベント発生時
        /// </summary>
        public void OnLevelFailed()
        {
            this._gameStatus.cleared = BeatSaberEvent.Failed;
            this._gameStatus.endTime = Utility.GetCurrentTime();
            this._gameStatus.endFlag = 1;
            UpdateCurrentMaxScore();
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
        /// ノーツデータの情報取得
        /// </summary>
        /// <param name="noteData"></param>
        private void SetNoteDataStatus(NoteData noteData)
        {
            var notescore = this._gameStatus.NoteDataGet();

            // Backwards compatibility for <1.12.1
            notescore.noteTime = noteData.time;
            notescore.scoringType = noteData.scoringType;
            notescore.colorType = noteData.colorType;
            notescore.noteCutDirection = noteData.cutDirection;
            notescore.noteLine = noteData.lineIndex;
            notescore.noteLayer = noteData.noteLineLayer;
            // If long notes are ever introduced, this name will make no sense
            notescore.timeToNextBasicNote = noteData.timeToNextColorNote;
            notescore.time = Utility.GetCurrentTime();
        }
        /// <summary>
        /// ノーツカットの情報取得
        /// </summary>
        /// <param name="noteCutInfo"></param>
        /// <param name="initialCut"></param>
        private void SetNoteCutStatus(NoteCutInfo noteCutInfo, NoteData noteData, IReadonlyCutScoreBuffer cutScoreBuffer, bool initialCut = true)
        {
            var notescore = this._gameStatus.NoteDataGet();

            notescore.speedOK = noteCutInfo.speedOK;
            notescore.directionOK = noteCutInfo.directionOK;
            notescore.saberTypeOK = noteCutInfo.saberTypeOK;
            notescore.wasCutTooSoon = noteCutInfo.wasCutTooSoon;
            notescore.saberSpeed = noteCutInfo.saberSpeed;
            notescore.saberDirX = noteCutInfo.saberDir[0];
            notescore.saberDirY = noteCutInfo.saberDir[1];
            notescore.saberDirZ = noteCutInfo.saberDir[2];
            notescore.saberType = noteCutInfo.saberType;
            notescore.swingRating = cutScoreBuffer == null ? -1 : cutScoreBuffer.beforeCutSwingRating;
            notescore.swingRatingFullyCut = cutScoreBuffer == null ? -1 : initialCut ? 0 : cutScoreBuffer.afterCutSwingRating;
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
        /// <summary>
        /// コンボ乗数、最大スコア・ランクの更新
        /// </summary>
        private void UpdateModMultiplier()
        {
            this._gameStatus.modifierMultiplier = this._gameplayModifiersSO.GetTotalMultiplier(this._gameplayModifiersSO.CreateModifierParamsList(this._gameplayModifiers), this._gameEnergyCounter.energy);
            this._gameStatus.maxScore = this._scoreController.immediateMaxPossibleModifiedScore;
            this._gameStatus.maxRank = RankModelHelper.MaxRankForGameplayModifiers(this._gameplayModifiers, this._gameplayModifiersSO, this._gameEnergyCounter.energy);
        }

        /// <summary>
        /// 現在の最大スコア・ランクの更新
        /// </summary>
        private void UpdateCurrentMaxScore()
        {
            var gameStatus = this._gameStatus;

            // TODO: test
            // int currentMaxScoreBeforeMultiplier = ScoreModel.MaxRawScoreForNumberOfNotes(gameStatus.passedNotes);
            gameStatus.currentMaxScore = this._scoreController.immediateMaxPossibleModifiedScore; // gameplayModifiersSO.MaxModifiedScoreForMaxRawScore(currentMaxScoreBeforeMultiplier, gameplayModiferParamsList, gameplayModifiersSO, gameEnergyCounter.energy);

            gameStatus.rank = RankModel.GetRankForScore(gameStatus.rawScore, gameStatus.score, this._scoreController.immediateMaxPossibleMultipliedScore, gameStatus.currentMaxScore);
        }

        /// <summary>
        /// 曲スタートまでstart更新を待機
        /// </summary>
        /// <returns></returns>
        private async Task SongStartWait()
        {
            var songTime = this._audioTimeSource.songTime;
            var token = connectionClosed.Token;
            try
            {
                while (this._audioTimeSource.songTime <= songTime)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.Delay(10);
                }
            }
            catch (Exception)
            {
                return;
            }
            PracticeSettings practiceSettings = this._gameplayCoreSceneSetupData.practiceSettings;
            float songSpeedMul = this._gameplayCoreSceneSetupData.gameplayModifiers.songSpeedMul;
            this._gameStatus.start = Utility.GetCurrentTime() - (long)(this._audioTimeSource.songTime * 1000f / songSpeedMul);
            if (practiceSettings != null)
                this._gameStatus.start -= (long)(practiceSettings.startSongTime * 1000f / songSpeedMul);
            Logger.Debug("Song Start");
        }

        /// <summary>
        /// noteFullyCutイベントへのデータ渡し用
        /// </summary>
        private readonly struct NoteFullyCutData
        {
            public readonly NoteData noteData;
            public readonly NoteCutInfo noteCutInfo;
            public readonly long noteCutTiming;

            public NoteFullyCutData(NoteData noteData, NoteCutInfo noteCutInfo, long noteCutTiming)
            {
                this.noteData = noteData;
                this.noteCutInfo = noteCutInfo;
                this.noteCutTiming = noteCutTiming;
            }
        }

        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private bool disposedValue;
        private bool initializeError;

        private GameplayCoreSceneSetupData _gameplayCoreSceneSetupData;
        private PauseController _pauseController;
        private IScoreController _scoreController;
        private IComboController _comboController;
        private GameplayModifiers _gameplayModifiers;
        private IAudioTimeSource _audioTimeSource;
        private GameEnergyCounter _gameEnergyCounter;
        private MultiplayerLocalActivePlayerFacade _multiplayerLocalActivePlayerFacade;
        private ILevelEndActions _levelEndActions;
        private readonly Dictionary<IReadonlyCutScoreBuffer, NoteFullyCutData> noteCutMapping = new Dictionary<IReadonlyCutScoreBuffer, NoteFullyCutData>();
        private GameplayModifiersModelSO _gameplayModifiersSO;
        private IReadonlyBeatmapData _beatmapData;
        private BeatmapObjectManager _beatmapObjectManager;
        private readonly CancellationTokenSource connectionClosed = new CancellationTokenSource();
        private readonly IRepository _repository;
        private readonly GameStatus _gameStatus;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        private ScoreManager(
            IRepository repository,
            GameStatus gameStatus,
            GameplayCoreSceneSetupData gameplayCoreSceneSetupData,
            GameplayModifiers gameplayModifiers,
            IAudioTimeSource audioTimeSource,
            IComboController comboController,
            GameEnergyCounter gameEnergyCounter,
            BeatmapObjectManager beatmapObjectManager,
            IReadonlyBeatmapData readonlyBeatmapData,
            IScoreController score,
            DiContainer container)
        {
            this._repository = repository;
            this._gameStatus = gameStatus;
            this._gameplayCoreSceneSetupData = gameplayCoreSceneSetupData;
            this._gameplayModifiers = gameplayModifiers;
            this._audioTimeSource = audioTimeSource;
            this._comboController = comboController;
            this._gameEnergyCounter = gameEnergyCounter;
            this._beatmapObjectManager = beatmapObjectManager;
            this._beatmapData = readonlyBeatmapData;
            this._scoreController = score;
            if (this._scoreController is ScoreController scoreController)
                this._gameplayModifiersSO = scoreController.GetField<GameplayModifiersModelSO, ScoreController>("_gameplayModifiersModel");
            this._pauseController = container.TryResolve<PauseController>();
            this._levelEndActions = container.TryResolve<ILevelEndActions>();
            this._multiplayerLocalActivePlayerFacade = container.TryResolve<MultiplayerLocalActivePlayerFacade>();
        }
        public async Task InitializeAsync(CancellationToken token)
        {
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
            if (this._pauseController != null) {
                // public event Action PauseController#didPauseEvent;
                this._pauseController.didPauseEvent += this.OnGamePause;
                // public event Action PauseController#didResumeEvent;
                this._pauseController.didResumeEvent += this.OnGameResume;
            }
            // public event Action<int scoreBeforeMultiplier, int scoreAfterMultiplier> ScoreController#scoreDidChangeEvent
            this._scoreController.scoreDidChangeEvent += this.OnScoreDidChange;
            // public event Action<ScoringElement> ScoreController#scoringForNoteStartedEvent
            this._scoreController.scoringForNoteStartedEvent += this.OnScoringForNoteStarted;
            // public event Action<int multiplier, float progress [0..1]> ScoreController#multiplierDidChangeEvent
            this._scoreController.multiplierDidChangeEvent += this.OnMultiplierDidChange;

            // public event Action<int combo> ComboController#comboDidChangeEvent
            this._comboController.comboDidChangeEvent += this.OnComboDidChange;

            // public event NoteWasCutDelegate<NoteController, in NoteCutInfo> BeatmapObjectManager#noteWasCutEvent
            this._beatmapObjectManager.noteWasCutEvent += this.OnNoteWasCut;
            // public event Action<NoteController> BeatmapObjectManager#noteWasMissedEvent
            this._beatmapObjectManager.noteWasMissedEvent += this.OnNoteWasMissed;

            // public event Action GameEnergyCounter#gameEnergyDidReach0Event;
            this._gameEnergyCounter.gameEnergyDidReach0Event += this.OnEnergyDidReach0Event;
            // public GameEnergyCounter#gameEnergyDidChangeEvent<float> // energy
            this._gameEnergyCounter.gameEnergyDidChangeEvent += this.OnEnergyDidChange;

            if (this._multiplayerLocalActivePlayerFacade != null) {
                this._multiplayerLocalActivePlayerFacade.playerDidFinishEvent += this.OnMultiplayerLevelFinished;
            }
            if (this._levelEndActions != null) {
                this._levelEndActions.levelFinishedEvent += this.OnLevelFinished;
                this._levelEndActions.levelFailedEvent += this.OnLevelFailed;
            }
            //BeatMapデータの登録
            this._gameStatus.scene = BeatSaberScene.Song;
            var diff = this._gameplayCoreSceneSetupData.difficultyBeatmap;
            var level = diff.level;
            // Load the beatmap data if it hasn't been loaded already
            if (this._gameplayCoreSceneSetupData.transformedBeatmapData == null)
                await this._gameplayCoreSceneSetupData.LoadTransformedBeatmapDataAsync();
            var beatmapData = this._gameplayCoreSceneSetupData.transformedBeatmapData;

            this._gameStatus.mode = diff.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName;
            this._gameplayModifiers = this._gameplayCoreSceneSetupData.gameplayModifiers;
            var playerSettings = this._gameplayCoreSceneSetupData.playerSpecificSettings;
            var practiceSettings = this._gameplayCoreSceneSetupData.practiceSettings;

            float songSpeedMul = this._gameplayModifiers.songSpeedMul;
            if (practiceSettings != null)
                songSpeedMul = practiceSettings.songSpeedMul;

            // HTTPStatus 1.12.1以下との下位互換性のために、NoteDataからidへのマッピングを生成します。 [Generate NoteData to id mappings for backwards compatiblity with <1.12.1]
            foreach (var beatmapObjectData in this._beatmapData.allBeatmapDataItems.Where(x => x is NoteData || x is SliderData).OrderBy(x => x.time)) {
                if (beatmapObjectData is NoteData noteData) {
                    //Logger.Debug($"\t{noteData.gameplayType.ToString()}\t{noteData.scoringType.ToString()}\t{noteData.cutDirection}\t{noteData.time}\t{noteData.lineIndex}\t{noteData.noteLineLayer}\t{noteData.colorType}");
                    var mapdata = this._gameStatus.MapDataGet();
                    mapdata.time = noteData.time;
                    mapdata.lineIndex = noteData.lineIndex;
                    mapdata.noteLineLayer = noteData.noteLineLayer;
                    mapdata.colorType = noteData.colorType;
                    mapdata.cutDirection = noteData.cutDirection;
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
            this._gameStatus.songHash = Regex.IsMatch(level.levelID, "^custom_level_[0-9A-F]{40}", RegexOptions.IgnoreCase) && !level.levelID.EndsWith(" WIP") ? level.levelID.Substring(13, 40) : null;
            this._gameStatus.levelId = level.levelID;
            this._gameStatus.songTimeOffset = (long)(level.songTimeOffset * 1000f / songSpeedMul);
            this._gameStatus.length = (long)(level.beatmapLevelData.audioClip.length * 1000f / songSpeedMul);
            this._gameStatus.paused = 0;
            this._gameStatus.difficulty = diff.difficulty.Name();
            this._gameStatus.notesCount = beatmapData.cuttableNotesCount;
            this._gameStatus.bombsCount = beatmapData.bombsCount;
            this._gameStatus.obstaclesCount = beatmapData.obstaclesCount;
            this._gameStatus.environmentName = level.environmentInfo.sceneInfo.sceneName;

            this.UpdateModMultiplier();

            this._gameStatus.songSpeedMultiplier = songSpeedMul;
            this._gameStatus.batteryLives = this._gameEnergyCounter.batteryLives;

            this._gameStatus.modObstacles = this._gameplayModifiers.enabledObstacleType;
            this._gameStatus.modInstaFail = this._gameplayModifiers.instaFail;
            this._gameStatus.modNoFail = this._gameplayModifiers.noFailOn0Energy;
            this._gameStatus.modBatteryEnergy = this._gameplayModifiers.energyType == GameplayModifiers.EnergyType.Battery;
            this._gameStatus.modDisappearingArrows = this._gameplayModifiers.disappearingArrows;
            this._gameStatus.modNoBombs = this._gameplayModifiers.noBombs;
            this._gameStatus.modSongSpeed = this._gameplayModifiers.songSpeed;
            this._gameStatus.modNoArrows = this._gameplayModifiers.noArrows;
            this._gameStatus.modGhostNotes = this._gameplayModifiers.ghostNotes;
            this._gameStatus.modFailOnSaberClash = this._gameplayModifiers.failOnSaberClash;
            this._gameStatus.modStrictAngles = this._gameplayModifiers.strictAngles;
            this._gameStatus.modFastNotes = this._gameplayModifiers.fastNotes;
            this._gameStatus.modSmallNotes = this._gameplayModifiers.smallCubes;
            this._gameStatus.modProMode = this._gameplayModifiers.proMode;
            this._gameStatus.modZenMode = this._gameplayModifiers.zenMode;

            if (diff.difficulty == BeatmapDifficulty.ExpertPlus) {
                this._gameStatus.staticLights = playerSettings.environmentEffectsFilterExpertPlusPreset != EnvironmentEffectsFilterPreset.AllEffects;
                this._gameStatus.environmentEffects = playerSettings.environmentEffectsFilterExpertPlusPreset;
            }
            else {
                this._gameStatus.staticLights = playerSettings.environmentEffectsFilterDefaultPreset != EnvironmentEffectsFilterPreset.AllEffects;
                this._gameStatus.environmentEffects = playerSettings.environmentEffectsFilterDefaultPreset;
            }
            this._gameStatus.leftHanded = playerSettings.leftHanded;
            this._gameStatus.playerHeight = playerSettings.playerHeight;
            this._gameStatus.sfxVolume = playerSettings.sfxVolume;
            this._gameStatus.reduceDebris = playerSettings.reduceDebris;
            this._gameStatus.noHUD = playerSettings.noTextsAndHuds;
            this._gameStatus.advancedHUD = playerSettings.advancedHud;
            this._gameStatus.autoRestart = playerSettings.autoRestart;
            this._gameStatus.saberTrailIntensity = playerSettings.saberTrailIntensity;
            this._gameStatus.hideNoteSpawningEffect = playerSettings.hideNoteSpawnEffect;
            this._gameStatus.startTime = Utility.GetCurrentTime();
            this._gameStatus.cleared = BeatSaberEvent.Menu;
            this._gameStatus.NoteDataSizeCheck();
            this._gameStatus.EnergyDataSizeCheck();
            this._gameStatus.MapDataSizeCheck();
            await this.SongStartWait();
            Logger.Debug("Initialize end");
        }

        /// <summary>
        /// Zenjectによりゲーム終了後に勝手に呼ばれる関数です。
        /// <see cref="UnityEngine.MonoBehaviour"/>のOnDestroy()のような動きをします。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)

                    //各種イベントの削除
                    try {
                        this.connectionClosed.Cancel();
                        this._repository.playDataAddFlag = true;

                        // 終了前にプレイヤーがマップを離れることで解決しないAfterCutScoreBuffersの参照を解放。(Release references for AfterCutScoreBuffers that don't resolve due to player leaving the map before finishing.)
                        foreach (var noteCutItem in noteCutMapping)
                        {
                            // CutScoreBuffers are pooled. Remove the event listener just in case it never fires the event.
                            noteCutItem.Key.UnregisterDidFinishReceiver(this);
                        }
                        this.noteCutMapping?.Clear();

                        if (this._pauseController != null) {
                            this._pauseController.didPauseEvent -= this.OnGamePause;
                            this._pauseController.didResumeEvent -= this.OnGameResume;
                        }

                        if (this._scoreController != null) {
                            this._scoreController.scoreDidChangeEvent -= this.OnScoreDidChange;
                            this._scoreController.scoringForNoteStartedEvent -= this.OnScoringForNoteStarted;
                            this._scoreController.multiplierDidChangeEvent -= this.OnMultiplierDidChange;
                        }

                        if (_comboController != null)
                        {
                            this._comboController.comboDidChangeEvent -= this.OnComboDidChange;
                            this._comboController = null;
                        }

                        if (_beatmapObjectManager != null)
                        {
                            this._beatmapObjectManager.noteWasCutEvent -= this.OnNoteWasCut;
                            this._beatmapObjectManager.noteWasMissedEvent -= this.OnNoteWasMissed;
                            this._beatmapObjectManager = null;
                        }

                        if (this._multiplayerLocalActivePlayerFacade != null) {
                            this._multiplayerLocalActivePlayerFacade.playerDidFinishEvent -= this.OnMultiplayerLevelFinished;
                            this._multiplayerLocalActivePlayerFacade = null;
                        }

                        if (this._levelEndActions != null) {
                            this._levelEndActions.levelFinishedEvent -= this.OnLevelFinished;
                            this._levelEndActions.levelFailedEvent -= this.OnLevelFailed;
                        }
                        //CleanUpMultiplayer();

                        if (this._gameEnergyCounter != null) {
                            this._gameEnergyCounter.gameEnergyDidChangeEvent -= this.OnEnergyDidChange;
                            this._gameEnergyCounter.gameEnergyDidReach0Event -= this.OnEnergyDidReach0Event;
                        }

                    }
                    catch (Exception e) {
                        Logger.Error(e);
                    }
                    Logger.Debug("dispose end");
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                this.disposedValue = true;
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
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
