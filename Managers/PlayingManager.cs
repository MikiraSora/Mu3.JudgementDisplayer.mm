using DpPatches.JudgementDisplayer.Bases;
using DpPatches.JudgementDisplayer.UI.Windows;
using MU3.Battle;
using MU3.Notes;
using MU3.Reader;
using MU3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.Managers
{
    internal class PlayingManager : Singleton<PlayingManager>
    {
        public static PlayingManager Instance => Singleton<PlayingManager>.instance;

        private JudgementItemCollection list;
        private PlaytimeJudgementDisplayerWindow window;
        private ResultJudgementDisplayerTypeAWindow resultTypeAWindow;
        private ResultJudgementDisplayerTypeBWindow resultTypeBWindow;
        private Dictionary<AttackNoteType, JudgementChecker> judgeCheckMap;
        private List<JudgementRecordItem> recordList;

        public PlayingManager()
        {
            list = new(TimeSpan.FromSeconds(5));
            window = new();
            resultTypeAWindow = new();
            resultTypeBWindow = new();
            recordList = new();

            judgeCheckMap = new Dictionary<AttackNoteType, JudgementChecker>();

            void DefaultJudgeChecker(AttackNoteType type) => judgeCheckMap[type] = new JudgementChecker(1f, 2f, 4f, 6f);
            foreach (AttackNoteType type in Enum.GetValues(typeof(AttackNoteType)))
                DefaultJudgeChecker(type);
            judgeCheckMap[AttackNoteType.CRTap] = new(2, 6, 6, 6);
            judgeCheckMap[AttackNoteType.Side] = new(1.5f, 3, 9, 9);
            judgeCheckMap[AttackNoteType.CRSide] = new(3, 9, 9, 9);
            judgeCheckMap[AttackNoteType.FlickL] = judgeCheckMap[AttackNoteType.FlickR] = new(5, 5, 10, 10);
            judgeCheckMap[AttackNoteType.CRFlickL] = judgeCheckMap[AttackNoteType.CRFlickR] = new(10, 10, 10, 10);
        }

        public JudgementItemCollection CurrentJudgementItems => list;

        public void ClearRecords()
        {
            recordList.Clear();
        }

        public void PostJudgeRecord(Judge judge, Timing timing, AttackNoteType attackType, Lanes lane, float curFrame, float trigFrame)
        {
            AddRuntimeDisplayer(judge, timing, attackType, lane, curFrame, trigFrame);
            AddStaticRecord(judge, timing, attackType, lane, curFrame, trigFrame);
        }

        private void AddStaticRecord(Judge judge, Timing timing, AttackNoteType attackType, Lanes lane, float curFrame, float trigFrame)
        {
            recordList.Add(new(judge, timing, attackType, lane, curFrame, trigFrame));
        }

        private void AddRuntimeDisplayer(Judge judge, Timing timing, AttackNoteType attackType, Lanes lane, float curFrame, float trigFrame)
        {
            var diffFrame = curFrame - trigFrame;
            if (!judgeCheckMap.TryGetValue(attackType, out var checker))
                return;

            //diffFrame = -diffFrame;

            checker.AdjustFrame(diffFrame, out var adjustedFrame, out var judgeResult);
            switch (attackType)
            {
                case AttackNoteType.TapHold:
                case AttackNoteType.SideHold:
                    break;
                default:
                    break;
            }

            if (judge == Judge.Miss)
                judgeResult = JudgeResult.Miss;

            list.AddJudgementItem(TimeSpan.FromMilliseconds(curFrame * 16.6666667f), adjustedFrame, judgeResult);
        }

        public float? GetCurrentFrame()
        {
            return SingletonMonoBehaviour<GameEngine>.instance?.notesManager?.getCurrentFrame();
        }

        public void SetRuntimeJudgementDisplayerVisible(bool isVisible)
        {
            if (isVisible)
            {
                window.Show();
            }
            else
            {
                window.Hide();
            }
        }

        public void SetResultJudgementDisplayerVisible(bool isVisible)
        {
            if (isVisible)
            {
                resultTypeAWindow.UpdateRecordList(recordList);
                resultTypeAWindow.Show();

                resultTypeBWindow.UpdateRecordList(recordList);
                resultTypeBWindow.Show();
            }
            else
            {
                resultTypeAWindow.Hide();
                resultTypeBWindow.Hide();
            }
        }
    }
}
