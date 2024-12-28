using System;

namespace DpPatches.JudgementDisplayer.Bases
{
    internal struct JudgementItem
    {
        public TimeSpan ShowStartTime { get; set; }
        public float Value { get; set; }
        public JudgeResult JudgeResult { get; set; }
    }
}