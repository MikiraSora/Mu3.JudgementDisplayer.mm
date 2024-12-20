using MonoMod;
using RD1.SSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.Patches.RD1.SSS
{
    //[MonoModPatch("global::RD1.SSS.StateMachine`2")]
    internal class StateMachineEx<TClass, TState> : StateMachine<TClass, TState> where TClass : class where TState : struct, IConvertible
    {
        public StateMachineEx(object methodHolder) : base(methodHolder)
        {

        }

        public extern void orig_GoNext(TState state);

        public void GoNext(TState state)
        {
            if (State.GetHashCode() != state.GetHashCode())
            {
                var name = typeof(TState).FullName.Replace("+", ".");
                var r = "";
                if (childState != null)
                    r = $"child:[{childState?.getStateName()}]";
                PatchLog.WriteLine($"State[{name}] {State} -> {state} {r}");
            }
            orig_GoNext(state);
        }
    }
}
