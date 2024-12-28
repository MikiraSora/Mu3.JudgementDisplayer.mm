using MU3.Battle;
using MU3.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.Bases
{
    public struct JudgementRecordItem
    {
        public JudgementRecordItem(Judge judge, Timing timing, AttackNoteType attackType, Lanes lane, float curFrame, float trigFrame)
        {
            Judge = judge;
            Timing = timing;
            AttackType = attackType;
            Lane = lane;
            CurFrame = curFrame;
            TrigFrame = trigFrame;
        }

        public Judge Judge { get; }
        public Timing Timing { get; }
        public AttackNoteType AttackType { get; }
        public Lanes Lane { get; }
        public float CurFrame { get; }
        public float TrigFrame { get; }
    }
}
