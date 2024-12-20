using MU3.Collab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace DpPatches.JudgementDisplayer
{
    public static class PatchLog
    {
        public const string FilePath = "dpJudgementLog.log";
        private static readonly DateTime beginTime;
        private static object locker = new object();

        static PatchLog()
        {
            try
            {
                File.Delete(FilePath);
                beginTime = DateTime.Now;
            }
            catch { }
        }

        public static void WriteLine(string msg)
        {
            var passTime = DateTime.Now - beginTime;
            lock (locker)
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                File.AppendAllText(FilePath, $"{passTime:g}[{threadId:2}] {msg}{Environment.NewLine}");
            }
        }
    }
}
