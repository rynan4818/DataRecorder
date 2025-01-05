using DataRecorder.Enums;
using System;

namespace DataRecorder.Models
{
    /// <summary>
    /// noteID����p
    /// </summary>
    public class MapDataEntity
    {
        /// <summary>
        /// �m�[�c�̕��ʎ���
        /// </summary>
        public float time { get; set; } = 0;
        /// <summary>
        /// ������E�ւ̃m�[�c�̐����ʒu[0..3]
        /// </summary>
        public int lineIndex { get; set; } = 0;
        /// <summary>
        /// �m�[�c�̐����ʒu
        /// </summary>
        public NoteLineLayer noteLineLayer { get; set; } = NoteLineLayer.Base;
        /// <summary>
        /// �m�[�c�̐F
        /// </summary>
        public ColorType colorType { get; set; } = ColorType.None;
        /// <summary>
        /// �m�[�c�̃J�b�g����
        /// </summary>
        public NoteCutDirection cutDirection { get; set; } = NoteCutDirection.None;
        /// <summary>
        /// �m�[�c�̃Q�[���v���C�^�C�v
        /// </summary>
        public NoteData.GameplayType gameplayType { get; set; } = NoteData.GameplayType.Normal;
    }
}
