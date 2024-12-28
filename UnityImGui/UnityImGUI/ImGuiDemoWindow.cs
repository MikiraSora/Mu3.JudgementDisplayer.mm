using UnityEngine;
using ImGuiNET;
using ImPlotNET;

public class ImGuiDemoWindow : MonoBehaviour
{
    private bool showImGuiDemoWindow;
    private bool showImPlotDemoWindow;

    public void Awake()
    {
        Cursor.visible = true;
    }

    public void Update()
    {
        ImGui.ShowDemoWindow(ref showImGuiDemoWindow);
        ImPlot.ShowDemoWindow(ref showImPlotDemoWindow);
    }
}
