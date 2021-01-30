using DataRecorder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace DataRecorder.Interfaces
{
    public interface IRepository
    {
        /// <summary>
        /// プレイデータのデータベース書き込みフラグ
        /// </summary>
        bool playDataAddFlag { get; set; }

        /// <summary>
        /// pause , resume イベント書き込みフラグ
        /// </summary>
        BeatSaberEvent? pauseEventAddFlag { get; set; }
    }
}
