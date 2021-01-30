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
        /// pause , resume イベント記録
        /// </summary>
        void PauseEventAdd(BeatSaberEvent bs_event);

        /// <summary>
        /// プレイデータの記録
        /// </summary>
        void PlayDataAdd();
    }
}
