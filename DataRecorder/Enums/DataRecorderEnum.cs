using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRecorder.Enums
{
	/// <summary>
	/// イベントの名称と値を管理します。
	/// </summary>
    public enum BeatSaberEvent
    {
		[Description("menu")]
		Menu,
		[Description("songStart")]
		SongStart,
		[Description("obstacleEnter")]
		ObstacleEnter,
		[Description("obstacleExit")]
		ObstacleExit,
		[Description("pause")]
		Pause,
		[Description("resume")]
		Resume,
		[Description("bombCut")]
		BombCut,
		[Description("noteCut")]
		NoteCut,
		[Description("noteFullyCut")]
		NoteFullyCut,
		[Description("bombMissed")]
		BombMissed,
		[Description("noteMissed")]
		NoteMissed,
		[Description("scoreChanged")]
		ScoreChanged,
		[Description("finished")]
		Finished,
		[Description("failed")]
		Failed,
		[Description("beatmapEvent")]
		BeatmapEvent,
		[Description("energyChanged")]
		EnergyChanged
	}

	/// <summary>
	/// 変更されたプロパティを管理する列挙型
	/// クラスじゃないのでパフォーマンス面でHTTPStatusより良いはず。
	/// </summary>
	[Flags]
	public enum ChangedProperty
    {
		Game = 1,
		Beatmap = 1 << 1,
		Performance = 1 << 2,
		NoteCut = 1 << 3,
		Mod = 1 << 4,
		BeatmapEvent = 1 << 5,
		AllButNoteCut = Game | BeatmapEvent | Performance | Mod,
		PerformanceAndNoteCut = Performance | NoteCut
	}
}
