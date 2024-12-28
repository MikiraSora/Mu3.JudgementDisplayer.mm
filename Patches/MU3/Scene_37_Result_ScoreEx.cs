using DpPatches.JudgementDisplayer.Managers;
using MonoMod;
using MU3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.Patches.MU3
{
    [MonoModPatch("global::MU3.Scene_37_Result_Score")]
    internal class Scene_37_Result_ScoreEx : Scene_37_Result_Score
    {
        private extern void orig_TechWait_Init();
        private void TechWait_Init()
        {
            orig_TechWait_Init();
            PlayingManager.Instance.SetResultJudgementDisplayerVisible(true);
        }

        private extern void orig_TechReward_Init();
        private void TechReward_Init()
        {
            orig_TechReward_Init();
            PlayingManager.Instance.SetResultJudgementDisplayerVisible(false);
        }
    }
}
