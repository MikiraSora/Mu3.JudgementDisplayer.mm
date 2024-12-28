using ImGuiNET;
using ImPlotNET;
using MU3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.UI
{
    internal class WindowManagerMonoBehavior : SingletonMonoBehaviour<WindowManagerMonoBehavior>
    {
        public void Update()
        {
            Singleton<WindowManager>.instance.DoGUI();
        }
    }
}
