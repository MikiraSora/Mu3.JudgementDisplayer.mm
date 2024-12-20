﻿using MonoMod;
using MU3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DpPatches.JudgementDisplayer.Patches.MU3
{
    [MonoModPatch("global::MU3.SystemUI")]
    internal class SystemUIEx : SystemUI
    {
        protected extern void orig_Awake();
        protected void Awake()
        {
            orig_Awake();
            PatchLog.WriteLine("GOOD");

            this.gameObject.AddComponent<ImGuiPluginHook>();
            this.gameObject.AddComponent<ImGuiDemoWindow>();
        }
    }
}
