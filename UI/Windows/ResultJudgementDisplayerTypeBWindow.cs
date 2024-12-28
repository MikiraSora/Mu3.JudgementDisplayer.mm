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
    internal class ResultJudgementDisplayerTypeBWindow : CommonWindowBase
    {
        private float windowWidth;
        private float windowHeight;
        private int splitCount;
        private int[] result;
        private string[] judgeNames;
        private bool showDebugWindow;

        private int maxY = 0;

        private float offsetX = 350f;
        private float offsetY = -181f;

        private uint blackColor = ImGui.ColorConvertFloat4ToU32(new Vector4(0, 0, 0, 1));
        private int[] resultIdx;
        private int minFrame;
        private int maxFrame;
        private float stepSize;
        private int fastRangeBias;
        private int fastRangeCount;
        private int goodRangeBias;
        private int goodRangeCount;
        private int lateRangeBias;
        private int lateRangeCount;
        private string[] xAxisLabel;
        private double[] xAxisX;

        internal enum Timing
        {
            Fast,
            PerfectFast,
            PerfectLate,
            Late,
            Miss
        }

        public ResultJudgementDisplayerTypeBWindow()
        {
            windowWidth = 340f;
            windowHeight = 135f;

            splitCount = 100 + 2;
            result = new int[splitCount];
            resultIdx = Enumerable.Range(0, splitCount).ToArray();

            fastRangeBias = 0;
            fastRangeCount = splitCount / 3;

            goodRangeBias = fastRangeBias + fastRangeCount;
            goodRangeCount = splitCount / 3 + splitCount % 3;

            lateRangeBias = goodRangeBias + goodRangeCount;
            lateRangeCount = splitCount / 3;

            xAxisLabel = new[] { "<Fast      ", "", "      Late>" };
            xAxisX = new[] { fastRangeCount, splitCount / 2d, lateRangeBias };
        }

        public override void OnGUI()
        {
            var io = ImGui.GetIO();
            var screenSize = io.DisplaySize;

            ImGui.SetNextWindowPos(new(screenSize.X * 0.5f + offsetX, screenSize.Y * 0.5f + offsetY), ImGuiCond.Always, new(0.5f, 0.5f));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(2, 2));
            ImGui.PushStyleColor(ImGuiCol.FrameBg, blackColor);
            ImGui.PushStyleColor(ImGuiCol.Border, blackColor);

            if (ImGui.Begin("Judgement", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoSavedSettings))
            {
                if (ImPlot.BeginPlot("Stock Prices", new Vector2(windowWidth, windowHeight), ImPlotFlags.NoTitle | ImPlotFlags.NoLegend | ImPlotFlags.NoInputs))
                {
                    ImPlot.SetupAxesLimits(0, splitCount, 0, maxY);
                    ImPlot.SetupAxis(ImAxis.Y1, string.Empty, ImPlotAxisFlags.NoTickLabels | ImPlotAxisFlags.NoGridLines);
                    ImPlot.SetupAxisTicks(ImAxis.X1, ref xAxisX[0], 3, xAxisLabel);

                    ImPlot.PushStyleVar(ImPlotStyleVar.FillAlpha, 0.5f);

                    ImPlot.PlotShaded("Fast", ref resultIdx[fastRangeBias], ref result[fastRangeBias], fastRangeCount, int.MinValue, ImPlotShadedFlags.None);
                    ImPlot.PlotShaded("Good", ref resultIdx[goodRangeBias], ref result[goodRangeBias], goodRangeCount, int.MinValue, ImPlotShadedFlags.None);
                    ImPlot.PlotShaded("Late", ref resultIdx[lateRangeBias], ref result[lateRangeBias], lateRangeCount, int.MinValue, ImPlotShadedFlags.None);

                    ImPlot.PopStyleVar();

                    ImPlot.EndPlot();
                }

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

            if (ImGui.Begin("Debug ResultJudgementDisplayerTypeBWindow", ref showDebugWindow))
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
            Array.Clear(result, 0, result.Length);
            maxY = 0;

            if (recordList.Count == 0)
                return;

            var minFrame = -10;
            var maxFrame = 10;
            var stepSize = (maxFrame - minFrame * 1.0f) / (splitCount - 1);

            int GetFrameRegionIndex(double frame)
            {
                if (frame < minFrame)
                    return 0;
                if (frame > maxFrame)
                    return splitCount - 1;

                var index = (int)((frame - minFrame) / stepSize);
                return index;
            }

            foreach (var item in recordList)
            {
                if (
                    item.AttackType is MU3.Battle.AttackNoteType.SideHold ||
                    item.AttackType is MU3.Battle.AttackNoteType.TapHold ||
                    item.Judge is MU3.Notes.Judge.Miss)
                    continue;

                var idx = GetFrameRegionIndex(item.CurFrame - item.TrigFrame);
                result[idx]++;
            }

            maxY = result.Max();
        }
    }
}
