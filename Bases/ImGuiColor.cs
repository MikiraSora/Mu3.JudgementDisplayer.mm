using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DpPatches.JudgementDisplayer.Bases
{
    internal struct ImGuiColor
    {
        public ImGuiColor(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public static implicit operator uint(ImGuiColor d) => (uint)(d.A << 24 | d.B << 16 | d.G << 8 | d.R);
        public static explicit operator ImGuiColor(uint abgr)
        {
            var a = (byte)(abgr >> 24);
            var b = (byte)((abgr >> 16) & 0xFF);
            var g = (byte)((abgr >> 8) & 0xFF);
            var r = (byte)(abgr & 0xFF);

            return new()
            {
                R = r,
                B = b,
                A = a,
                G = g
            };
        }


    }
}
