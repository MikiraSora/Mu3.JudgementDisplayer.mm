using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.Bases
{
    internal class JudgementChecker
    {
        private readonly float pbreakFrame;
        private readonly float cbreakFrame;
        private readonly float breakFrame;
        private readonly float hitFrame;

        public JudgementChecker(float pbreakFrame, float cbreakFrame, float breakFrame, float hitFrame)
        {
            this.pbreakFrame = pbreakFrame;
            this.cbreakFrame = cbreakFrame;
            this.breakFrame = breakFrame;
            this.hitFrame = hitFrame;
        }

        public void AdjustFrame(float realFrame, out float adjustedFrame, out JudgeResult judgeResult)
        {
            var min = 0f;
            var max = 0f;
            var absFrame = Math.Abs(realFrame);

            if (absFrame <= pbreakFrame)
            {
                judgeResult = JudgeResult.PlatinumBreak;
                min = 0f;
                max = pbreakFrame;
            }
            else if (absFrame <= cbreakFrame)
            {
                judgeResult = JudgeResult.CriticalBreak;
                min = pbreakFrame;
                max = cbreakFrame;
            }
            else if (absFrame <= breakFrame)
            {
                judgeResult = JudgeResult.Break;
                min = cbreakFrame;
                max = breakFrame;
            }
            else if (absFrame <= hitFrame)
            {
                judgeResult = JudgeResult.Hit;
                min = breakFrame;
                max = hitFrame;
            }
            else
            {
                judgeResult = JudgeResult.Miss;
            }

            adjustedFrame = judgeResult == JudgeResult.Miss ? 1 : (Math.Sign(realFrame) * (absFrame - min) / (max - min));
        }
    }
}
