using MU3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.UI
{
    internal abstract class CommonWindowBase : IWindow
    {
        public virtual void OnAfterLoad() { }
        public virtual void OnBeforeUnload() { }

        public abstract void OnGUI();

        public void Show()
        {
            Singleton<WindowManager>.instance.ShowWindow(this);
        }

        public void Hide()
        {
            Singleton<WindowManager>.instance.HideWindow(this);
        }
    }
}
