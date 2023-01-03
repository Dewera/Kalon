using System.Runtime.InteropServices;
using Kalon.Tests.Native.Structs;

namespace Kalon.Tests.Native.PInvoke;

internal static partial class User32
{
    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GetCursorPos(out Point point);
}