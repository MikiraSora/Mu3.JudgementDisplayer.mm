using System;
using ImGuiNET.FXCompatible.System.Numerics;
using ImGuiNET.FXCompatible.System.Runtime.CompilerServices;
using System.Text;

namespace ImGuiNET
{
    public unsafe partial struct ImFontAtlasCustomRect
    {
        public uint ID;
        public ushort Width;
        public ushort Height;
        public ushort X;
        public ushort Y;
        public float GlyphAdvanceX;
        public Vector2 GlyphOffset;
        public ImFont* Font;
    }
    public unsafe partial struct ImFontAtlasCustomRectPtr
    {
        public ImFontAtlasCustomRect* NativePtr { get; }
        public ImFontAtlasCustomRectPtr(ImFontAtlasCustomRect* nativePtr) => NativePtr = nativePtr;
        public ImFontAtlasCustomRectPtr(IntPtr nativePtr) => NativePtr = (ImFontAtlasCustomRect*)nativePtr;
        public static implicit operator ImFontAtlasCustomRectPtr(ImFontAtlasCustomRect* nativePtr) => new ImFontAtlasCustomRectPtr(nativePtr);
        public static implicit operator ImFontAtlasCustomRect* (ImFontAtlasCustomRectPtr wrappedPtr) => wrappedPtr.NativePtr;
        public static implicit operator ImFontAtlasCustomRectPtr(IntPtr nativePtr) => new ImFontAtlasCustomRectPtr(nativePtr);
        public ref uint ID => ref Unsafe.AsRef<uint>(&NativePtr->ID);
        public ref ushort Width => ref Unsafe.AsRef<ushort>(&NativePtr->Width);
        public ref ushort Height => ref Unsafe.AsRef<ushort>(&NativePtr->Height);
        public ref ushort X => ref Unsafe.AsRef<ushort>(&NativePtr->X);
        public ref ushort Y => ref Unsafe.AsRef<ushort>(&NativePtr->Y);
        public ref float GlyphAdvanceX => ref Unsafe.AsRef<float>(&NativePtr->GlyphAdvanceX);
        public ref Vector2 GlyphOffset => ref Unsafe.AsRef<Vector2>(&NativePtr->GlyphOffset);
        public ImFontPtr Font => new ImFontPtr(NativePtr->Font);
        public void Destroy()
        {
            ImGuiNative.ImFontAtlasCustomRect_destroy(NativePtr);
        }
        public bool IsPacked()
        {
            byte ret = ImGuiNative.ImFontAtlasCustomRect_IsPacked(NativePtr);
            return ret != 0;
        }
    }
}