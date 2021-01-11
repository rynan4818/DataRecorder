using DataRecorder.Enums;
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
        /// <summary>
        /// 旧BeatSaberEventに対応するところ
        /// </summary>
        /// <param name="e"></param>
        private void OnStatusUpdated(BeatSaberEvent e)
        {
            // TODO : それぞれの処理に対応するメソッドの作成。たぶん_repositoryに対してなんかおこなうはず。もしくは曲終わるまでデータを蓄積させる？
            switch (e) {
                case BeatSaberEvent.Menu:
                    break;
                case BeatSaberEvent.SongStart:
                    break;
                case BeatSaberEvent.ObstacleEnter:
                    break;
                case BeatSaberEvent.ObstacleExit:
                    break;
                case BeatSaberEvent.Pause:
                    break;
                case BeatSaberEvent.Resume:
                    break;
                case BeatSaberEvent.BombCut:
                    break;
                case BeatSaberEvent.NoteCut:
                    break;
                case BeatSaberEvent.NoteFullyCut:
                    break;
                case BeatSaberEvent.BombMissed:
                    break;
                case BeatSaberEvent.NoteMissed:
                    break;
                case BeatSaberEvent.ScoreChanged:
                    break;
                case BeatSaberEvent.Finished:
                    break;
                case BeatSaberEvent.Failed:
                    break;
                case BeatSaberEvent.BeatmapEvent:
                    break;
				case BeatSaberEvent.EnergyChanged:
					break;
                default:
                    break;
            }
        }

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
			this._gameStatus.paused = Utility.GetCurrentTime();

			this.OnStatusUpdated(BeatSaberEvent.Pause);
		}

		public void OnGameResume()
		{
			this._gameStatus.start = Utility.GetCurrentTime() - (long)(audioTimeSyncController.songTime * 1000f / this._gameStatus.songSpeedMultiplier);
			this._gameStatus.paused = 0;

			this.OnStatusUpdated(BeatSaberEvent.Resume);
		}

		public void OnNoteWasCut(NoteData noteData, NoteCutInfo noteCutInfo, int multiplier)
		{
			// Event order: combo, multiplier, scoreController.noteWasCut, (LateUpdate) scoreController.scoreDidChange, afterCut, (LateUpdate) scoreController.scoreDidChange

			var _gameStatus = this._gameStatus;

			SetNoteCutStatus(noteData, noteCutInfo, true);

			int beforeCutScore = 0;
			int afterCutScore = 0;
			int cutDistanceScore = 0;

			ScoreModel.RawScoreWithoutMultiplier(noteCutInfo, out beforeCutScore, out afterCutScore, out cutDistanceScore);

			_gameStatus.initialScore = beforeCutScore + cutDistanceScore;
			_gameStatus.finalScore = -1;
			_gameStatus.cutDistanceScore = cutDistanceScore;
			_gameStatus.cutMultiplier = multiplier;

			if (noteData.colorType == ColorType.None) {
				_gameStatus.passedBombs++;
				_gameStatus.hitBombs++;

				this.OnStatusUpdated(BeatSaberEvent.BombCut);
			}
			else {
				_gameStatus.passedNotes++;

				if (noteCutInfo.allIsOK) {
					_gameStatus.hitNotes++;

					this.OnStatusUpdated(BeatSaberEvent.NoteCut);
				}
				else {
					_gameStatus.missedNotes++;

					this.OnStatusUpdated(BeatSaberEvent.NoteMissed);
				}
			}

			List<CutScoreBuffer> list = (List<CutScoreBuffer>)afterCutScoreBuffersField.GetValue(scoreController);

			foreach (CutScoreBuffer acsb in list) {
				if (noteCutInfoField.GetValue(acsb) == noteCutInfo) {
					// public CutScoreBuffer#didFinishEvent<CutScoreBuffer>
					noteCutMapping.TryAdd(noteCutInfo, noteData);

					acsb.didFinishEvent += OnNoteWasFullyCut;
					break;
				}
			}
		}

		public void OnNoteWasFullyCut(CutScoreBuffer acsb)
		{
			int beforeCutScore;
			int afterCutScore;
			int cutDistanceScore;

			NoteCutInfo noteCutInfo = (NoteCutInfo)noteCutInfoField.GetValue(acsb);
			NoteData noteData = noteCutMapping[noteCutInfo];

			noteCutMapping.TryRemove(noteCutInfo, out _);

			SetNoteCutStatus(noteData, noteCutInfo, false);

			// public static ScoreModel.RawScoreWithoutMultiplier(NoteCutInfo, out int beforeCutRawScore, out int afterCutRawScore, out int cutDistanceRawScore)
			ScoreModel.RawScoreWithoutMultiplier(noteCutInfo, out beforeCutScore, out afterCutScore, out cutDistanceScore);

			int multiplier = (int)cutScoreBufferMultiplierField.GetValue(acsb);

			this._gameStatus.initialScore = beforeCutScore + cutDistanceScore;
			this._gameStatus.finalScore = beforeCutScore + afterCutScore + cutDistanceScore;
			this._gameStatus.cutDistanceScore = cutDistanceScore;
			this._gameStatus.cutMultiplier = multiplier;

			this.OnStatusUpdated(BeatSaberEvent.NoteFullyCut);

			acsb.didFinishEvent -= OnNoteWasFullyCut;
		}

		private void SetNoteCutStatus(NoteData noteData, NoteCutInfo noteCutInfo = null, bool initialCut = true)
		{
			var gameStatus = this._gameStatus;

			gameStatus.ResetNoteCut();

			// Backwards compatibility for <1.12.1
			gameStatus.noteType = noteData.colorType == ColorType.None ? "Bomb" : noteData.colorType == ColorType.ColorA ? "NoteA" : noteData.colorType == ColorType.ColorB ? "NoteB" : noteData.colorType.ToString();
			gameStatus.noteCutDirection = noteData.cutDirection.ToString();
			gameStatus.noteLine = noteData.lineIndex;
			gameStatus.noteLayer = (int)noteData.noteLineLayer;
			// If long notes are ever introduced, this name will make no sense
			gameStatus.timeToNextBasicNote = noteData.timeToNextColorNote;

			if (noteCutInfo != null) {
				gameStatus.speedOK = noteCutInfo.speedOK;
				gameStatus.directionOK = noteCutInfo.directionOK;
				gameStatus.saberTypeOK = noteCutInfo.saberTypeOK;
				gameStatus.wasCutTooSoon = noteCutInfo.wasCutTooSoon;
				gameStatus.saberSpeed = noteCutInfo.saberSpeed;
				gameStatus.saberDirX = noteCutInfo.saberDir[0];
				gameStatus.saberDirY = noteCutInfo.saberDir[1];
				gameStatus.saberDirZ = noteCutInfo.saberDir[2];
				gameStatus.saberType = noteCutInfo.saberType.ToString();
				gameStatus.swingRating = noteCutInfo.swingRatingCounter == null ? -1 : initialCut ? noteCutInfo.swingRatingCounter.beforeCutRating : noteCutInfo.swingRatingCounter.afterCutRating;
				gameStatus.timeDeviation = noteCutInfo.timeDeviation;
				gameStatus.cutDirectionDeviation = noteCutInfo.cutDirDeviation;
				gameStatus.cutPointX = noteCutInfo.cutPoint[0];
				gameStatus.cutPointY = noteCutInfo.cutPoint[1];
				gameStatus.cutPointZ = noteCutInfo.cutPoint[2];
				gameStatus.cutNormalX = noteCutInfo.cutNormal[0];
				gameStatus.cutNormalY = noteCutInfo.cutNormal[1];
				gameStatus.cutNormalZ = noteCutInfo.cutNormal[2];
				gameStatus.cutDistanceToCenter = noteCutInfo.cutDistanceToCenter;
			}
		}

		public void OnNoteWasMissed(NoteData noteData, int multiplier)
		{
			// Event order: combo, multiplier, scoreController.noteWasMissed, (LateUpdate) scoreController.scoreDidChange

			this._gameStatus.batteryEnergy = gameEnergyCounter.batteryEnergy;

			SetNoteCutStatus(noteData);

			if (noteData.colorType == ColorType.None) {
				this._gameStatus.passedBombs++;

				this.OnStatusUpdated(BeatSaberEvent.BombMissed);
			}
			else {
				this._gameStatus.passedNotes++;
				this._gameStatus.missedNotes++;

				this.OnStatusUpdated(BeatSaberEvent.NoteMissed);
			}
		}

		public void OnScoreDidChange(int scoreBeforeMultiplier, int scoreAfterMultiplier)
		{
			var gameStatus = this._gameStatus;

			gameStatus.score = scoreAfterMultiplier;

			int currentMaxScoreBeforeMultiplier = ScoreModel.MaxRawScoreForNumberOfNotes(gameStatus.passedNotes);
			gameStatus.currentMaxScore = gameplayModifiersSO.MaxModifiedScoreForMaxRawScore(currentMaxScoreBeforeMultiplier, gameplayModifiers, gameplayModifiersSO);

			RankModel.Rank rank = RankModel.GetRankForScore(scoreBeforeMultiplier, gameStatus.score, currentMaxScoreBeforeMultiplier, gameStatus.currentMaxScore);
			gameStatus.rank = RankModel.GetRankName(rank);

			this.OnStatusUpdated(BeatSaberEvent.ScoreChanged);
		}

		public void OnComboDidChange(int combo)
		{
			this._gameStatus.combo = combo;
			// public int ScoreController#maxCombo
			this._gameStatus.maxCombo = scoreController.maxCombo;
		}

		public void OnEnergyDidChange(float energy)
		{
			this._gameStatus.energy = energy;
			this.OnStatusUpdated(BeatSaberEvent.EnergyChanged);
		}

		public void OnMultiplierDidChange(int multiplier, float multiplierProgress)
		{
			this._gameStatus.multiplier = multiplier;
			this._gameStatus.multiplierProgress = multiplierProgress;
		}

		public void OnLevelFinished()
		{
			this.OnStatusUpdated(BeatSaberEvent.Finished);
		}

		public void OnLevelFailed()
		{
			this.OnStatusUpdated(BeatSaberEvent.Failed);
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
                // gameplayCoreSceneSetupData = container.Resolve<GameplayCoreSceneSetupData>();  ここ不要？
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
			Logger.Info("0");

			// TODO:各種イベントの追加
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
			Logger.Info("1");

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
			float modifierMultiplier = gameplayModifiersSO.GetTotalMultiplier(gameplayModifiers);
			Logger.Info("2");

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

			this._gameStatus.maxScore = gameplayModifiersSO.MaxModifiedScoreForMaxRawScore(ScoreModel.MaxRawScoreForNumberOfNotes(diff.beatmapData.cuttableNotesType), gameplayModifiers, gameplayModifiersSO);
			this._gameStatus.maxRank = RankModelHelper.MaxRankForGameplayModifiers(gameplayModifiers, gameplayModifiersSO).ToString();
			Logger.Info("3");

			this._gameStatus.ResetPerformance();

			this._gameStatus.modifierMultiplier = modifierMultiplier;
			this._gameStatus.songSpeedMultiplier = songSpeedMul;
			this._gameStatus.batteryLives = gameEnergyCounter.batteryLives;

			this._gameStatus.modObstacles = gameplayModifiers.enabledObstacleType.ToString();
			this._gameStatus.modInstaFail = gameplayModifiers.instaFail;
			this._gameStatus.modNoFail = gameplayModifiers.noFail;
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
			Logger.Info("4");
			this.OnStatusUpdated(BeatSaberEvent.SongStart);
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
                    this._repository?.Dispose();

					//TODO: 各種イベントの削除

					Logger.Debug("dispose call");
					try {
						this._gameStatus.scene = "Menu"; // XXX: multiplayerController は常にこの前にクリーンアップされているので不可能(XXX: impossible because multiplayerController is always cleaned up before this)

						this._gameStatus?.ResetMapInfo();

						this._gameStatus?.ResetPerformance();

						// 終了前にプレイヤーがマップを離れることで解決しないAfterCutScoreBuffersの参照を解放。(Release references for AfterCutScoreBuffers that don't resolve due to player leaving the map before finishing.)
						noteCutMapping?.Clear();

						this.OnStatusUpdated(BeatSaberEvent.Menu);

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
