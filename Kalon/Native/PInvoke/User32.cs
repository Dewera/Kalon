﻿using System.Drawing;
using System.Runtime.InteropServices;

namespace Kalon.Native.PInvoke;

internal static class User32
{
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool GetCursorPos(out Point point);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool SetCursorPos(int x, int y);
}