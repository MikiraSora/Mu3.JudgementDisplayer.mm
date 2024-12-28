using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.UI
{
    internal interface IWindow
    {
        void OnAfterLoad();
        void OnBeforeUnload();
        void OnGUI();
    }
}
