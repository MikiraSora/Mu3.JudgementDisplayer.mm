using DpPatches.JudgementDisplayer.Bases;
using ImGuiNET;
using ImGuiNET.FXCompatible.System.Numerics;
using ImPlotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.UI.Windows
{
    internal class ResultJudgementDisplayerTypeAWindow : CommonWindowBase
    {
        private float windowWidth;
        private float windowHeight;
        private int splitCount;
        private string[] judgeNames;
        private int[] data;
        private bool showDebugWindow;

        private int maxY = 0;

        private float offsetX = 350f;
        private float offsetY = -181f;

        private uint blackColor = ImGui.ColorConvertFloat4ToU32(new Vector4(0, 0, 0, 1));
        private ImPlotColormap colormap;

        internal enum Timing
        {
            Fast,
            PerfectFast,
            PerfectLate,
            Late,
            Miss
        }

        public ResultJudgementDisplayerTypeAWindow()
        {
            windowWidth = 340f;
            windowHeight = 135f;

            splitCount = 40;
            judgeNames = ["Fast", "GFast", "GLate", "Late", "Miss"];
            data = new int[judgeNames.Length * (splitCount + 0)];

            var colorArray = new uint[] { new ImGuiColor(93, 173, 226, 255), new ImGuiColor(249, 231, 159, 255), new ImGuiColor(249, 231, 159, 255), new ImGuiColor(245, 176, 65, 255), new ImGuiColor(125, 125, 125, 255) };
            colormap = ImPlot.AddColormap("JudgementColorMap", ref colorArray[0], colorArray.Length);
        }

        public override void OnGUI()
        {
            var io = ImGui.GetIO();
            var screenSize = io.DisplaySize;

            ImGui.SetNextWindowPos(new(screenSize.X * 0.5f + offsetX, screenSize.Y * 0.5f + offsetY), ImGuiCond.Always, new(0.5f, 0.5f));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(2, 2));
            ImGui.PushStyleColor(ImGuiCol.FrameBg, blackColor);
            ImGui.PushStyleColor(ImGuiCol.Border, blackColor);

            if (ImGui.Begin("Judgement", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoSavedSettings))
            {
                ImPlot.PushColormap(colormap);
                ImPlot.PushStyleVar(ImPlotStyleVar.PlotPadding, new Vector2(0, 0));

                if (ImPlot.BeginPlot("Judgement", new(windowWidth, windowHeight), ImPlotFlags.NoTitle | ImPlotFlags.NoInputs | ImPlotFlags.NoMenus))
                {
                    ImPlot.SetupLegend(ImPlotLocation.South, ImPlotLegendFlags.Outside | ImPlotLegendFlags.Horizontal);
                    ImPlot.SetupAxes(null, null, ImPlotAxisFlags.AutoFit | ImPlotAxisFlags.NoDecorations, ImPlotAxisFlags.AutoFit | ImPlotAxisFlags.NoDecorations);
                    ImPlot.SetupAxisLimitsConstraints(ImAxis.Y1, -maxY, maxY);
                    ImPlot.SetupAxisLimitsConstraints(ImAxis.X1, 0, splitCount);
                    ImPlot.PlotBarGroups(judgeNames, ref data[0], judgeNames.Length, splitCount, 1f, 0, ImPlotBarGroupsFlags.Stacked);

                    ImPlot.EndPlot();
                }

                ImPlot.PopStyleVar();
                ImPlot.PopColormap();

                ImGui.End();
            }

            ImGui.PopStyleColor();
            ImGui.PopStyleColor();
            ImGui.PopStyleVar();

            //OnGUIDebug();
        }

        private void OnGUIDebug()
        {
            UnityEngine.Cursor.visible = true;

            if (ImGui.Begin("Debug OngekiPlaytimeJudgementDisplayerWindow", ref showDebugWindow))
            {
                var io = ImGui.GetIO();

                ImGui.SliderFloat("WindowOffsetX", ref offsetX, -io.DisplaySize.X * 0.5f, io.DisplaySize.X * 0.5f);
                ImGui.SliderFloat("WindowOffsetY", ref offsetY, -io.DisplaySize.Y * 0.5f, io.DisplaySize.Y * 0.5f);
                ImGui.SliderFloat("WindowWidth", ref windowWidth, 100, 500);
                ImGui.SliderFloat("WindowHeight", ref windowHeight, 100, 500);

                ImGui.End();
            }
        }

        internal void UpdateRecordList(List<JudgementRecordItem> recordList)
        {
            Array.Clear(data, 0, data.Length);
            maxY = 0;

            if (recordList.Count == 0)
                return;

            var min = recordList.Min(x => x.TrigFrame);
            var max = recordList.Max(x => x.TrigFrame) + 1;
            var splitPerFrame = (max - min) / splitCount;

            int GetIndex(Timing result, int frameIdx) => (data.Length / judgeNames.Length * (int)result) + frameIdx;

            foreach (var item in recordList)
            {
                if (item.AttackType is MU3.Battle.AttackNoteType.SideHold || item.AttackType is MU3.Battle.AttackNoteType.TapHold)
                    continue;

                var frameIdx = (int)((item.TrigFrame - min) / splitPerFrame);
                var timing = item.CurFrame > item.TrigFrame ? Timing.Late : Timing.Fast;

                switch (item.Judge)
                {
                    case MU3.Notes.Judge.Great://todo
                    case MU3.Notes.Judge.Perfect:
                        timing = timing == Timing.Fast ? Timing.PerfectFast : Timing.PerfectLate;
                        break;
                    case MU3.Notes.Judge.Miss:
                        timing = Timing.Miss;
                        break;
                    default:
                        break;
                }

                var splitIdx = GetIndex(timing, frameIdx);
                var appendValue = timing switch
                {
                    Timing.Fast or Timing.PerfectFast => 1,
                    Timing.Late or Timing.PerfectLate => -1,
                    Timing.Miss => -1,
                    _ => 0
                };
                data[splitIdx] += appendValue;
            }

            maxY = Math.Abs(data.Max());
            PatchLog.WriteLine($"UpdateRecordList() offset:{string.Join(",", recordList.Select(x => (x.CurFrame - x.TrigFrame).ToString()).ToArray())}");
            PatchLog.WriteLine($"UpdateRecordList() maxY:{maxY}, data:{string.Join(",", data.Select(x => x.ToString()).ToArray())}");
        }
    }
}
