using DpPatches.JudgementDisplayer.Managers;
using MonoMod;
using MU3.Battle;
using MU3.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DpPatches.JudgementDisplayer.Patches.MU3.Notes
{
    [MonoModPatch("global::MU3.Notes.NotesManager")]
    internal class NotesManagerEx : NotesManager
    {
        public extern void orig_setResultEffectAndScore(Judge judge, Timing timing, AttackNoteType attackType, Lanes lane, Vector3 posText, Vector3 posBomb, float frameDiff);
        public override void setResultEffectAndScore(Judge judge, Timing timing, AttackNoteType attackType, Lanes lane, Vector3 posText, Vector3 posBomb, float frameDiff)
        {
            orig_setResultEffectAndScore(judge, timing, attackType, lane, posText, posBomb, frameDiff);

            var curFrame = getCurrentFrame();
            var trigFrame = curFrame + frameDiff;

            PlayingManager.Instance.PostJudgeRecord(judge, timing, attackType, lane, curFrame, trigFrame);
        }
    }
}
