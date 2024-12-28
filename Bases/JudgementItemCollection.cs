using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.Bases
{
    internal class JudgementItemCollection : IEnumerable<JudgementItem>
    {
        private readonly TimeSpan duration;
        private List<JudgementItem> items = new();

        public TimeSpan Duration => duration;

        public JudgementItemCollection(TimeSpan duration)
        {
            this.duration = duration;
        }

        public void AddJudgementItem(TimeSpan showStartTime, float value, JudgeResult judgeResult)
        {
            items.Add(new JudgementItem
            {
                ShowStartTime = showStartTime,
                Value = value,
                JudgeResult = judgeResult
            });

            //PatchLog.WriteLine($"add judge item {offset} at {showStartTime.TotalMilliseconds}ms");
        }

        public IEnumerator<JudgementItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public void UpdateCurrentTime(TimeSpan time)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if ((time - items[i].ShowStartTime) > duration)
                {
                    //PatchLog.WriteLine($"[{time.TotalMilliseconds}ms]remove judge item {items[i].Offset} at {items[i].ShowStartTime.TotalMilliseconds}ms");

                    items.RemoveAt(i);
                    i--;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            items.Clear();
        }
    }
}
