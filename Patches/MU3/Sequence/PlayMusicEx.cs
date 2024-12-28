using DpPatches.JudgementDisplayer.Managers;
using DpPatches.JudgementDisplayer.Patches.MU3.Notes;
using DpPatches.JudgementDisplayer.UI;
using MonoMod;
using MU3.Battle;
using MU3.Game;
using MU3.Notes;
using MU3.Sequence;
using MU3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.Patches.MU3.Sequence
{
    [MonoModPatch("global::MU3.Sequence.PlayMusic")]
    internal class PlayMusicEx : PlayMusic
    {
        private extern void orig_Enter_Play();
        private void Enter_Play()
        {
            orig_Enter_Play();

            PlayingManager.Instance.CurrentJudgementItems.Clear();
            PlayingManager.Instance.ClearRecords();
            PlayingManager.Instance.SetRuntimeJudgementDisplayerVisible(true);
        }

        private void Leave_Play()
        {
            PlayingManager.Instance.SetRuntimeJudgementDisplayerVisible(false);
        }
    }
}
