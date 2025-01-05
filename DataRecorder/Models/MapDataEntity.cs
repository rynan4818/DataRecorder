using DataRecorder.Enums;
using System;

namespace DataRecorder.Models
{
    /// <summary>
    /// noteID判定用
    /// </summary>
    public class MapDataEntity
    {
        /// <summary>
        /// ノーツの譜面時間
        /// </summary>
        public float time { get; set; } = 0;
        /// <summary>
        /// 左から右へのノーツの水平位置[0..3]
        /// </summary>
        public int lineIndex { get; set; } = 0;
        /// <summary>
        /// ノーツの水平位置
        /// </summary>
        public NoteLineLayer noteLineLayer { get; set; } = NoteLineLayer.Base;
        /// <summary>
        /// ノーツの色
        /// </summary>
        public ColorType colorType { get; set; } = ColorType.None;
        /// <summary>
        /// ノーツのカット方向
        /// </summary>
        public NoteCutDirection cutDirection { get; set; } = NoteCutDirection.None;
        /// <summary>
        /// ノーツのゲームプレイタイプ
        /// </summary>
        public NoteData.GameplayType gameplayType { get; set; } = NoteData.GameplayType.Normal;
    }
}
