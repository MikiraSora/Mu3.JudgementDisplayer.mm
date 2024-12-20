using MonoMod;
using MU3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MU3.User.UserLimitBreakItemSet;

namespace DpPatches.JudgementDisplayer.Patches.MU3.Util
{
    //[MonoModPatch("global::MU3.Util.Mode`2")]
    internal class ModeEx<TClass, TEnum> : Mode<TClass, TEnum> where TEnum : struct, IConvertible
    {
        private int current_;
        private static Dictionary<int, string> nameMap;

        static ModeEx()
        {
            nameMap = new Dictionary<int, string>();
            foreach (var val in Enum.GetValues(typeof(TEnum)))
                nameMap[Convert.ToInt32(val)] = val.ToString();
        }

        public ModeEx(TClass parent) : base(parent)
        {
        }

        public extern void orig_set(TEnum state);

        public void set(TEnum state)
        {
            if (current_.GetHashCode() != state.GetHashCode())
            {
                var name = typeof(TEnum).FullName.Replace("+", ".");
                var r = "";
                PatchLog.WriteLine($"Mode[{name}] {nameMap[current_]} -> {state} {r}");
            }
            orig_set(state);
        }
    }
}
