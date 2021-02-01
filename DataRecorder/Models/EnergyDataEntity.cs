using DataRecorder.Enums;
using System;

namespace DataRecorder.Models
{
    /// <summary>
    /// エネルギー変化保持内容
    /// </summary>
    public class EnergyDataEntity
    {
        /// <summary>
        /// イベント発生時間(UNIX time[ms])
        /// </summary>
        public long time { get; set; } = 0;

        /// <summary>
        /// エネルギー値
        /// </summary>
        public float energy { get; set; } = 0;
    }
}
