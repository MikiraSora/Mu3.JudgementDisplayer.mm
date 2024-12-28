using DpPatches.JudgementDisplayer.Bases;
using DpPatches.JudgementDisplayer.Managers;
using ImGuiNET;
using ImGuiNET.FXCompatible.System.Numerics;
using MU3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DpPatches.JudgementDisplayer.UI.Windows
{
    internal class PlaytimeJudgementDisplayerWindow : CommonWindowBase
    {
        private ImGuiColor pbreakLineColor;
        private ImGuiColor pbreakBorderColor;
        private float pbreakWidthWeight;

        private ImGuiColor cbreakLineColor;
        private ImGuiColor cbreakBorderColor;
        private float cbreakWidthWeight;

        private ImGuiColor breakLineColor;
        private ImGuiColor breakBorderColor;
        private float breakWidthWeight;

        private ImGuiColor hitLineColor;
        private ImGuiColor hitBorderColor;
        private float hitWidthWeight;

        private ImGuiColor missLineColor;
        private ImGuiColor missBorderColor;
        private float missWidthWeight;
        private ImGuiNET.FXCompatible.System.Numerics.Vector2 charSize;
        private float windowWidth;
        private float windowHeight;
        private int borderHeight;
        private byte borderAlpha = 175;
        private byte lineAlpha = 200;

        private bool showDebugWindow = true;

        private float offsetY = -340;
        private float totalWindowSize;
        private float widthPixelPerFrame;

        public PlaytimeJudgementDisplayerWindow()
        {
            ImGuiColor RGBA(byte r, byte g, byte b, byte a) => new ImGuiColor(r, g, b, a);

            //pbreak
            pbreakLineColor = RGBA(249, 231, 159, lineAlpha);
            pbreakBorderColor = RGBA(247, 220, 111, borderAlpha);
            pbreakWidthWeight = 1f;
            //cbreak
            cbreakLineColor = RGBA(250, 215, 160, lineAlpha);
            cbreakBorderColor = RGBA(248, 196, 113, borderAlpha);
            cbreakWidthWeight = 2f;
            //break
            breakLineColor = RGBA(245, 176, 65, lineAlpha);
            breakBorderColor = RGBA(243, 156, 18, borderAlpha);
            breakWidthWeight = 4f;
            //hit
            hitLineColor = RGBA(93, 173, 226, lineAlpha);
            hitBorderColor = RGBA(52, 152, 219, borderAlpha);
            hitWidthWeight = 6f;
            //miss
            missLineColor = RGBA(213, 216, 220, lineAlpha);
            missBorderColor = RGBA(171, 178, 185, borderAlpha);
            missWidthWeight = 1f;

            charSize = ImGui.CalcTextSize("L");

            windowWidth = 250f;
            windowHeight = 20f;

            borderHeight = 8;

            totalWindowSize = pbreakWidthWeight + cbreakWidthWeight + breakWidthWeight + hitWidthWeight + missWidthWeight;
            widthPixelPerFrame = windowWidth / totalWindowSize;
        }

        public override void OnGUI()
        {
            var io = ImGui.GetIO();
            var screenSize = io.DisplaySize;

            if (PlayingManager.Instance.GetCurrentFrame() is not float currentFrame)
                return;

            var currentTime = TimeSpan.FromMilliseconds(currentFrame * 16.6666667f);

            ImGui.SetNextWindowPos(new(io.DisplaySize.X * 0.5f, io.DisplaySize.Y * 0.5f + offsetY), ImGuiCond.Always, new(0.5f, 0.5f));
            ImGui.SetNextWindowContentSize(new(windowWidth, windowHeight));
            ImGui.SetNextWindowBgAlpha(0.75f);
            if (ImGui.Begin("begin", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoResize))
            {
                var drawList = ImGui.GetWindowDrawList();
                var topLeftPos = ImGui.GetWindowPos();
                var size = ImGui.GetWindowSize();

                var bottomRightPos = topLeftPos + size;
                var centerX = (topLeftPos.X + bottomRightPos.X) / 2;
                var centerY = (topLeftPos.Y + bottomRightPos.Y) / 2;

                DrawBoarderA(drawList, centerX, centerY);
                DrawJudgementListA(currentTime, drawList, topLeftPos, bottomRightPos, centerX);

                ImGui.End();
            }

            //OnGUIDebug();
        }

        private void DrawJudgementListA(TimeSpan currentTime, ImDrawListPtr drawList, ImGuiNET.FXCompatible.System.Numerics.Vector2 topLeftPos, ImGuiNET.FXCompatible.System.Numerics.Vector2 bottomRightPos, float centerX)
        {
            void DrawJudgement(float value, double showProgress, JudgeResult judgeResult)
            {
                var color = default(ImGuiColor);
                var bias = 0f;
                var duration = 0f;
                switch (judgeResult)
                {
                    case JudgeResult.PlatinumBreak:
                        color = pbreakLineColor;
                        bias = 0;
                        duration = pbreakWidthWeight;
                        break;
                    case JudgeResult.CriticalBreak:
                        color = cbreakLineColor;
                        bias = 0 + pbreakWidthWeight;
                        duration = cbreakWidthWeight;
                        break;
                    case JudgeResult.Break:
                        color = breakLineColor;
                        bias = 0 + pbreakWidthWeight + cbreakWidthWeight;
                        duration = breakWidthWeight;
                        break;
                    case JudgeResult.Hit:
                        color = hitLineColor;
                        bias = 0 + pbreakWidthWeight + cbreakWidthWeight + breakWidthWeight;
                        duration = hitWidthWeight;
                        break;
                    case JudgeResult.Miss:
                        color = missLineColor;
                        bias = 0 + pbreakWidthWeight + cbreakWidthWeight + breakWidthWeight + hitWidthWeight + missWidthWeight / 2;
                        duration = 0;
                        value = -1;
                        break;
                    default:
                        break;
                }

                var x = centerX + Math.Sign(value) * (bias + duration * Math.Abs(value)) * widthPixelPerFrame / 2;

                color.A = (byte)(color.A * (showProgress switch
                {
                    <= 0.7 => 1,
                    _ => 1 - showProgress
                }));

                drawList.AddLine(new(x, topLeftPos.Y), new(x, bottomRightPos.Y), color, 2);
            }

            var items = PlayingManager.instance.CurrentJudgementItems;
            var durationMs = items.Duration.TotalMilliseconds;
            items.UpdateCurrentTime(currentTime);

            foreach (var judgement in items)
            {
                var showProgress = (float)((currentTime - judgement.ShowStartTime).TotalMilliseconds / durationMs);
                showProgress = Mathf.Clamp(showProgress, 0, 1);
                DrawJudgement(judgement.Value, showProgress, judgement.JudgeResult);
            }
        }

        private void DrawBoarderA(ImDrawListPtr drawList, float centerX, float centerY)
        {
            //draw border
            void DrawBorder(float offsetFrame, float widthFrame, uint aColor, uint bColor)
            {
                var offsetPixel = offsetFrame * widthPixelPerFrame;
                var widthPixel = widthFrame * widthPixelPerFrame;

                void DrawBorderCore(float startX, float width, uint color)
                {
                    var endX = startX + width / 2;
                    drawList.AddRectFilled(new(startX, centerY + borderHeight / 2), new(endX, centerY - borderHeight / 2), color, 0);
                }

                DrawBorderCore(centerX + offsetPixel, widthPixel, aColor);
                DrawBorderCore(centerX - offsetPixel - widthPixel / 2, widthPixel, bColor);
            }

            var startFrame = 0f;
            //pbreak
            DrawBorder(startFrame, pbreakWidthWeight, pbreakBorderColor, pbreakBorderColor);
            //cbreak
            startFrame = startFrame + pbreakWidthWeight / 2;
            DrawBorder(startFrame, cbreakWidthWeight, cbreakBorderColor, cbreakBorderColor);
            //break
            startFrame = startFrame + cbreakWidthWeight / 2;
            DrawBorder(startFrame, breakWidthWeight, breakBorderColor, breakBorderColor);
            //hit
            startFrame = startFrame + breakWidthWeight / 2;
            DrawBorder(startFrame, hitWidthWeight, hitBorderColor, hitBorderColor);
            //miss
            startFrame = startFrame + hitWidthWeight / 2;
            DrawBorder(startFrame, missWidthWeight, 0, 0);

            drawList.AddText(new(centerX + (widthPixelPerFrame * totalWindowSize / 2 + 2), centerY - charSize.Y / 2), hitLineColor, "F");
            drawList.AddText(new(centerX - (widthPixelPerFrame * totalWindowSize / 2 + 8), centerY - charSize.Y / 2), breakLineColor, "L");
        }

        private void OnGUIDebug()
        {
            if (showDebugWindow)
                Cursor.visible = true;

            if (ImGui.Begin("Debug OngekiPlaytimeJudgementDisplayerWindow", ref showDebugWindow))
            {
                var io = ImGui.GetIO();

                ImGui.SliderFloat("WindowOffsetY", ref offsetY, -io.DisplaySize.Y * 0.5f, io.DisplaySize.Y * 0.5f);
                ImGui.SliderFloat("WindowWidth", ref windowWidth, 100, 500);
                ImGui.SliderFloat("WindowHeight", ref windowHeight, 5, 50);

                ImGui.End();
            }
        }
    }
}
