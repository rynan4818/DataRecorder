using DataRecorder.Configuration;
using DataRecorder.Models;
using System.IO;
using System.Text;

namespace DataRecorder.DataBases
{
    public static class DebugNoteId
    {
        public static void Test(GameStatus test)
        {
            test.lastNoteId = 0;
            var b = new StringBuilder();
            b.AppendLine("#NOTE DATA#");
            for (test.noteIndex = 0; test.noteIndex < test.noteEndIndex; test.noteIndex++)
            {
                var data = test.NoteDataGet();
                b.AppendLine($"{data.noteTime}:{data.noteLine}:{data.noteLayer}:{data.colorType}:{data.noteCutDirection}:{data.gameplayType}:{test.GetNoteId()}");
            }
            b.AppendLine("#MAP DATA#");
            for (test.mapIndex = 0; test.mapIndex < test.mapEndIndex; test.mapIndex++)
            {
                var data = test.MapDataGet();
                b.AppendLine($"{data.time}:{data.lineIndex}:{data.noteLineLayer}:{data.colorType}:{data.cutDirection}:{data.gameplayType}");
            }
            b.AppendLine("#DATA END#");
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(PluginConfig.Instance.DBFilePath),"GameStatusTest.txt"), b.ToString());
        }
    }
}
