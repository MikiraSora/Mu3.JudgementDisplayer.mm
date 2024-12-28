using MU3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.UI
{
    internal class WindowManager : Singleton<WindowManager>
    {
        private List<IWindow> shownWindows = new List<IWindow>();
        private HashSet<Type> shownWindowTypes = new HashSet<Type>();

        internal void ShowWindow<T>(T window) where T : IWindow
        {
            var type = window.GetType();
            if (shownWindowTypes.Contains(type))
            {
                PatchLog.WriteLine($"ShowWindow() window {type.Name} has been shown already.");
                return;
            }

            shownWindows.Add(window);
            shownWindowTypes.Add(type);
            window.OnAfterLoad();
            //PatchLog.WriteLine($"ShowWindow() window {type.Name} shown.");
        }

        internal void HideWindow<T>(T window) where T : IWindow
        {
            var type = window.GetType();
            if (!shownWindowTypes.Contains(type))
            {
                PatchLog.WriteLine($"ShowWindow() window {type.Name} has been hiden already.");
                return;
            }

            window.OnBeforeUnload();
            shownWindows.Remove(window);
            shownWindowTypes.Remove(type);
            //PatchLog.WriteLine($"ShowWindow() window {type.Name} hiden.");
        }

        internal void DoGUI()
        {
            foreach (var window in shownWindows)
            {
                window.OnGUI();
            }
        }
    }
}
