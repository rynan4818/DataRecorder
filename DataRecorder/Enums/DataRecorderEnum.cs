using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRecorder.Enums
{
	/// <summary>
	/// イベントの名称と値を管理
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
		EnergyChanged,
		[Description("softFail")]
		SoftFailed
	}
	/// <summary>
	/// ゲームシーンの名称と値を管理
	/// </summary>
	public enum BeatSaberScene
    {
		[Description("Song")]
		Song
	}
	/// <summary>
	/// 拡張メソッド用クラス
	/// </summary>
	public static class EnumExtention
	{
		/// <summary>
		/// <see cref="Enum"/>の<see cref="DescriptionAttribute"/>属性に指定された文字列を取得する拡張メソッドです。
		/// </summary>
		/// <param name="value">文字列を取得したい<see cref="Enum"/></param>
		/// <returns></returns>
		public static string GetDescription(this Enum value)
		{
			if (value == null)
				return null;
			var field = value.GetType().GetField(value.ToString());
			if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute) {
				return attribute.Description;
			}
			else {
				return value.ToString();
			}
		}
	}
}
