using System;
using DataRecorder.Enums;

namespace DataRecorder.Models
{
    /// <summary>
    /// �Q�[�����̃X�e�[�^�X���Ǘ�����N���X�ł��B�Q�[������1�̃C���X�^���X�����쐬����܂���B
    /// </summary>
    public class GameStatus
    {
        #region // �v���p�e�B
        // �N���X�Ǘ��p

        /// <summary>
        /// �m�[�c���̃X�R�A�i�[�p�z��̌��݂̃C���f�b�N�X
        /// </summary>
        public int noteIndex { get; set; } = 0;

        /// <summary>
        /// �G�l���M�[�ω��i�[�p�z��̌��݂̃C���f�b�N�X
        /// </summary>
        public int energyIndex { get; set; } = 0;

        /// <summary>
        /// ���ʃf�[�^�p�z��̌��݂̃C���f�b�N�X
        /// </summary>
        public int mapIndex { get; set; } = 0;

        /// <summary>
        /// �m�[�c���̃X�R�A�i�[�p�z��̍ŏI�C���f�b�N�X
        /// </summary>
        public int noteEndIndex { get; set; } = 0;

        /// <summary>
        /// �G�l���M�[�ω��i�[�p�z��̍ŏI�C���f�b�N�X
        /// </summary>
        public int energyEndIndex { get; set; } = 0;

        /// <summary>
        /// ���ʃf�[�^�p�z��̍ŏI�C���f�b�N�X
        /// </summary>
        public int mapEndIndex { get; set; } = 0;

        /// <summary>
        /// noteID�����p
        /// </summary>
        public int lastNoteId { get; set; } = 0;

        // �L�^�p

        /// <summary>
        /// ���ʊJ�n����[�Ǝ�] (UNIX time[ms])
        /// </summary>
        public long startTime { get; set; } = 0;

        /// <summary>
        /// ���ʏI������[�Ǝ�] (UNIX time[ms])
        /// </summary>
        public long endTime { get; set; } = 0;

        /// <summary>
        /// �N���A����[�Ǝ�]
        /// </summary>
        public BeatSaberEvent? cleared { get; set; } = null;

        /// <summary>
        /// �I������[�Ǝ�]
        /// </summary>
        public int endFlag { get; set; } = 0;

        /// <summary>
        /// pause��[�Ǝ�]
        /// </summary>
        public int pauseCount { get; set; } = 0;

        /// <summary>
        /// StatusObject[Game] mode : �}���`�v���[���[
        /// </summary>
        public bool multiplayer { get; set; } = false;

        /// <summary>
        /// StatusObject[Game] mode : �p�[�e�B���[�h
        /// </summary>
        public bool partyMode { get; set; } = false;

        /// <summary>
        /// StatusObject[Game] mode : Campaign���[�h
        /// </summary>
        public bool campaign { get; set; } = false;

        /// <summary>
        /// StatusObject[Game] mode : �Q�[�����[�h
        /// </summary>
        public string mode { get; set; } = null;

        /// <summary>
        /// StatusObject[Game] scene : �Q�[���V�[��
        /// </summary>
        public BeatSaberScene? scene { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] �Ȗ�
        /// </summary>
        public string songName { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] �Ȃ̃T�u�^�C�g��
        /// </summary>
        public string songSubName { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] �Ȃ̍��
        /// </summary>
        public string songAuthorName { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] ���ʂ̍��
        /// </summary>
        public string levelAuthorName { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] ����ID(SHA-1)
        /// </summary>
        public string songHash { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] ����ID(Raw)
        /// </summary>
        public string levelId { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] �Ȃ�BPM
        /// </summary>
        public float songBPM { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] ���ʂ�NJS
        /// </summary>
        public float noteJumpSpeed { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] ���ʊJ�n�I�t�Z�b�g�l(�擾�o���Ă��Ȃ��H)
        /// </summary>
        public long songTimeOffset { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] ���ʂ̒���[ms]
        /// </summary>
        public long length { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] ���ʃv���C�J�n���̎��ԁB�ĊJ���ɍX�V�B(UNIX time[ms])
        /// </summary>
        public long start { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] �ꎞ��~���̎���(UNIX time[ms])
        /// </summary>
        public long paused { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] ���ʂ̓�Փx
        /// </summary>
        public string difficulty { get; set; } = null;

        /// <summary>
        /// StatusObject[BeatMap] ���ʂ̃m�[�c��
        /// </summary>
        public int notesCount { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] ���ʂ̔��e��
        /// </summary>
        public int bombsCount { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] ���ʂ̕ǂ̐�
        /// </summary>
        public int obstaclesCount { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] ���݂�Mod�ł̍ő�X�R�A
        /// </summary>
        public int maxScore { get; set; } = 0;

        /// <summary>
        /// StatusObject[BeatMap] ���݂�Mod�ł̍ő僉���N
        /// </summary>
        public RankModel.Rank maxRank { get; set; } = RankModel.Rank.E;

        /// <summary>
        /// StatusObject[BeatMap] ���ʂ̗v����
        /// </summary>
        public string environmentName { get; set; } = null;

        /// <summary>
        /// StatusObject[Performance] mod�搔�����̌��݂̃X�R�A
        /// </summary>
        public int rawScore { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݂̃X�R�A
        /// </summary>
        public int score { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݂̃m�[�c���ŒB���\�ȍő�X�R�A
        /// </summary>
        public int currentMaxScore { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݂̃����N
        /// </summary>
        public RankModel.Rank rank { get; set; } = RankModel.Rank.E;

        /// <summary>
        /// StatusObject[Performance] ���ݏ��������m�[�c��
        /// </summary>
        public int passedNotes { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݃q�b�g�����m�[�c��
        /// </summary>
        public int hitNotes { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݃~�X�����m�[�c��
        /// </summary>
        public int missedNotes { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] �i���擾�j
        /// </summary>
        public int lastNoteScore { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���ݏ����������e��
        /// </summary>
        public int passedBombs { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݃q�b�g�������e��
        /// </summary>
        public int hitBombs { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݂̃R���{��
        /// </summary>
        public int combo { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݂̍ő�R���{��
        /// </summary>
        public int maxCombo { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݂̃R���{�搔
        /// </summary>
        public int multiplier { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݂̃R���{�搔�̐i�s�
        /// </summary>
        public float multiplierProgress { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] ���݂̃o�b�e���G�l���M�[�c��
        /// </summary>
        public int batteryEnergy { get; set; } = 1;

        /// <summary>
        /// StatusObject[Performance] ���݂̃G�l���M�[�c��
        /// </summary>
        public float energy { get; set; } = 0;

        /// <summary>
        /// StatusObject[Performance] noFail����Fail������
        /// </summary>
        public bool softFailed { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] Mod�搔
        /// </summary>
        public float modifierMultiplier { get; set; } = 1f;

        /// <summary>
        /// StatusObject[Mods] �ǂ̗L��
        /// </summary>
        public GameplayModifiers.EnabledObstacleType modObstacles { get; set; } = GameplayModifiers.EnabledObstacleType.All;

        /// <summary>
        /// StatusObject[Mods] �m�[�~�X
        /// </summary>
        public bool modInstaFail { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �m�[�t�@�E��
        /// </summary>
        public bool modNoFail { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �o�b�e���G�l���M�[
        /// </summary>
        public bool modBatteryEnergy { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �ő�o�b�e���c��(DB���L�^)
        /// </summary>
        public int batteryLives { get; set; } = 1;

        /// <summary>
        /// StatusObject[Mods] ��������
        /// </summary>
        public bool modDisappearingArrows { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] ���e����
        /// </summary>
        public bool modNoBombs { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �Ȃ̑��x
        /// </summary>
        public GameplayModifiers.SongSpeed modSongSpeed { get; set; } = GameplayModifiers.SongSpeed.Normal;

        /// <summary>
        /// StatusObject[Mods] �Ȃ̑��x��Mod�搔
        /// </summary>
        public float songSpeedMultiplier { get; set; } = 1f;

        /// <summary>
        /// StatusObject[Mods] ��󖳂�
        /// </summary>
        public bool modNoArrows { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �S�[�X�g�m�[�c
        /// </summary>
        public bool modGhostNotes { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �Z�C�o�[�N���b�V���Ŏ��s�H�iHidden)
        /// </summary>
        public bool modFailOnSaberClash { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �����Ȋp�x[��萳�m�ȃJ�b�g���������߂��A�ő�΍���60�x����15�x�ɕύX]
        /// </summary>
        public bool modStrictAngles { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] Does something (Hidden)
        /// </summary>
        public bool modFastNotes { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �X���[���m�[�c
        /// </summary>
        public bool modSmallNotes { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �v�����[�h
        /// </summary>
        public bool modProMode { get; set; } = false;

        /// <summary>
        /// StatusObject[Mods] �T���[�h
        /// </summary>
        public bool modZenMode { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] �ÓI���C�g
        /// </summary>
        public bool staticLights { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] ������
        /// </summary>
        public bool leftHanded { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] �v���C���[�̍���
        /// </summary>
        public float playerHeight { get; set; } = 1.7f;

        /// <summary>
        /// StatusObject[Player settings] �m�[�c�J�b�g����
        /// </summary>
        public float sfxVolume { get; set; } = 0.7f;

        /// <summary>
        /// StatusObject[Player settings] �m�[�c�J�b�g���̔j�ЗL��
        /// </summary>
        public bool reduceDebris { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] �e�L�X�g��HUD����
        /// </summary>
        public bool noHUD { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] Advanced HUD
        /// </summary>
        public bool advancedHUD { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] ���s���Ɏ������X�^�[�g
        /// </summary>
        public bool autoRestart { get; set; } = false;

        /// <summary>
        /// StatusObject[Player settings] �g���C�����x
        /// </summary>
        public float saberTrailIntensity { get; set; } = 0.5f;

        /// <summary>
        /// StatusObject[Player settings] ���G�t�F�N�g
        /// </summary>
        public EnvironmentEffectsFilterPreset environmentEffects { get; set; } = EnvironmentEffectsFilterPreset.AllEffects;

        /// <summary>
        /// StatusObject[Player settings] �m�[�c�̃X�|�[�����ʂ��B��
        /// </summary>
        public bool hideNoteSpawningEffect { get; set; } = false;
        #endregion
        #region // �萔
        /// <summary>
        /// noteScore�z�񏉊����T�C�Y (�K�v�Ȕz��T�C�Y�̓m�[�c���{���e��)
        /// </summary>
        private const int defaultNoteScoreSize = 3000;

        /// <summary>
        /// noteScore�z��ǉ��T�C�Y
        /// </summary>
        private const int addNoteScoreSize = 500;

        /// <summary>
        /// ���ʃf�[�^�p�z�񏉊����T�C�Y
        /// </summary>
        private const int defaultMapDataSize = 3000;

        /// <summary>
        /// ���ʃf�[�^�p�z��ǉ��T�C�Y
        /// </summary>
        private const int addMapDataSize = 500;

        /// <summary>
        /// �G�l���M�[�ω��i�[�p�z�񏉊����T�C�Y
        /// </summary>
        private const int defaultEnergyDataSize = 3000;

        /// <summary>
        /// �G�l���M�[�ω��i�[�p�z��ǉ��T�C�Y
        /// </summary>
        private const int addEnergyDataSize = 500;
        #endregion
        #region // �����o�ϐ�
        /// <summary>
        /// �m�[�c���̃X�R�A�i�[�p�z��
        /// </summary>
        private NoteDataEntity[] noteScores = new NoteDataEntity[defaultNoteScoreSize];

        /// <summary>
        /// �G�l���M�[�ω��i�[�p�z��
        /// </summary>
        private EnergyDataEntity[] energyDatas = new EnergyDataEntity[defaultEnergyDataSize];

        /// <summary>
        /// ���ʃf�[�^�p�z��
        /// </summary>
        private MapDataEntity[] mapDatas = new MapDataEntity[defaultMapDataSize];

        /// <summary>
        /// �m�[�c���̃X�R�A�i�[�p�z��̏������ςݐ�
        /// </summary>
        private int noteScoresInitCount = 0;

        /// <summary>
        /// �G�l���M�[�ω��i�[�z��̏������ςݐ�
        /// </summary>
        private int energyDataInitCount = 0;

        /// <summary>
        /// ���ʃf�[�^�p�z��̏������ςݐ�
        /// </summary>
        private int mapDatasInitCount = 0;

        #endregion
        #region // �R���X�g���N�^
        public GameStatus()
        {
            // �m�[�c�E�G�l���M�[�E���ʊi�[�ϐ��̏�����
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
        #region // �p�u���b�N���\�b�h
        /// <summary>
        /// �m�[�c���̃X�R�A�i�[�p�z�񂩂猻�݂̃C���f�b�N�X�̓��e�����o��
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
        /// �G�l���M�[�ω��i�[�p�z�񂩂猻�݂̃C���f�b�N�X�̓��e�����o��
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
        /// ���ʃf�[�^�p�z�񂩂猻�݂̃C���f�b�N�X�̓��e�����o��
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
        /// �m�[�c���̃X�R�A�i�[�p�z��̃C���f�b�N�X���C���N�������g
        /// </summary>
        public void NoteDataIndexUp()
        {
            var performance = this.NoteDataGet();
            performance.rawScore = this.rawScore;
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
            performance.softFailed = this.softFailed;
            this.noteIndex++;
            if (this.noteIndex >= this.noteScores.Length)
                this.NoteDataResize(this.noteScores.Length);
            this.noteEndIndex++;
        }

        /// <summary>
        /// �G�l���M�[�ω��i�[�p�z��̃C���f�b�N�X���C���N�������g
        /// </summary>
        public void EnergyDataIndexUp()
        {
            this.energyIndex++;
            if (this.energyIndex >= this.energyDatas.Length)
                this.EnergyDataResize(this.energyDatas.Length);
            this.energyEndIndex++;
        }

        /// <summary>
        /// ���ʃf�[�^�p�z��̃C���f�b�N�X���C���N�������g
        /// </summary>
        public void MapDataIndexUp()
        {
            this.mapIndex++;
            if (this.mapIndex >= this.mapDatas.Length)
                this.MapDataResize(this.mapDatas.Length);
            this.mapEndIndex++;
        }
        /// <summary>
        /// �m�[�c���̃X�R�A�i�[�p�z��̃T�C�Y�m�F
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
        /// �G�l���M�[�ω��i�[�p�z��̃T�C�Y�m�F
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
        /// ���ʃf�[�^�z��̃T�C�Y�m�F
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
        /// GameStatus�̃��Z�b�g
        /// </summary>
        public void ResetGameStatus()
        {
            this.startTime = 0;
            this.endTime = 0;
            this.cleared = null;
            this.endFlag = 0;
            this.pauseCount = 0;
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
            this.rawScore = 0;
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
            this.softFailed = false;
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
            this.modSmallNotes = false;
            this.modProMode = false;
            this.modZenMode = false;
            this.staticLights = false;
            this.leftHanded = false;
            this.playerHeight = 1.7f;
            this.sfxVolume = 0.7f;
            this.reduceDebris = false;
            this.noHUD = false;
            this.advancedHUD = false;
            this.autoRestart = false;
            this.saberTrailIntensity = 0.5f;
            this.environmentEffects = EnvironmentEffectsFilterPreset.AllEffects;
            this.hideNoteSpawningEffect = false;
            this.ResetNoteCut();
            this.ResetEnergy();
            this.ResetMap();
        }
        /// <summary>
        /// ���݂̃C���f�b�N�X��NoteID���擾����
        /// </summary>
        public int GetNoteId()
        {
            // Backwards compatibility for <1.12.1
            int noteID = -1;
            // Check the near notes first for performance
            for (int i = Math.Max(0, this.lastNoteId - 10); i < this.mapEndIndex; i++) {
                if (NoteDataEquals(this.mapDatas[i])) {
                    noteID = i;
                    if (i > this.lastNoteId) this.lastNoteId = i;
                    break;
                }
            }
            // If that failed, check the rest of the notes in reverse order
            if (noteID == -1) {
                for (int i = Math.Max(0, this.lastNoteId - 11); i >= 0; i--) {
                    if (NoteDataEquals(this.mapDatas[i])) {
                        noteID = i;
                        break;
                    }
                }
            }
            return noteID;
        }
        #endregion
        #region // �v���C�x�[�g���\�b�h
        /// <summary>
        /// �m�[�c���̃X�R�A�i�[�p�z������Z�b�g
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
                    this.noteScores[i].scoringType = NoteData.ScoringType.Ignore;
                    this.noteScores[i].cutTime = null;
                    this.noteScores[i].rawScore = 0;
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
                    this.noteScores[i].softFailed = false;
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
        /// �G�l���M�[�ω��i�[�p�z������Z�b�g
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
        /// ���ʃf�[�^�z������Z�b�g
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
                }
            }
            this.mapIndex = 0;
            this.mapEndIndex = 0;
            this.mapDatasInitCount = this.mapDatas.Length;
        }

        /// <summary>
        /// �G�l���M�[�ω��i�[�p�z��̃��T�C�Y
        /// </summary>
        private void EnergyDataResize(int size)
        {
            Array.Resize(ref this.energyDatas, size + addEnergyDataSize);
        }

        /// <summary>
        /// �m�[�c���̃X�R�A�i�[�p�z��̃��T�C�Y
        /// </summary>
        private void NoteDataResize(int size)
        {
            Array.Resize(ref this.noteScores, size + addNoteScoreSize);
        }

        /// <summary>
        /// ���ʃf�[�^�p�z��̃��T�C�Y
        /// </summary>
        private void MapDataResize(int size)
        {
            Array.Resize(ref this.mapDatas, size + addMapDataSize);
        }

        /// <summary>
        /// ���ʃf�[�^�ƌ��݂̃C���f�b�N�X�̃m�[�c�f�[�^���r(noteID�擾�p)
        /// </summary>
        private bool NoteDataEquals(MapDataEntity a)
        {
            return (this.noteScores[noteIndex].scoringType == NoteData.ScoringType.BurstSliderElement && this.noteScores[noteIndex].noteTime - a.time < 0.1f || a.time == this.noteScores[noteIndex].noteTime) &&
                a.lineIndex == this.noteScores[noteIndex].noteLine &&
                a.noteLineLayer == this.noteScores[noteIndex].noteLayer &&
                a.colorType == this.noteScores[noteIndex].colorType &&
                (this.modNoArrows || this.noteScores[noteIndex].scoringType == NoteData.ScoringType.BurstSliderElement || a.cutDirection == this.noteScores[noteIndex].noteCutDirection);
        }
        #endregion
    }
}
