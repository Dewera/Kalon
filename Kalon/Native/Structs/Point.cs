using System.Runtime.InteropServices;

namespace Kalon.Native.Structs;

[StructLayout(LayoutKind.Explicit, Size = 8)]
internal readonly record struct Point([field: FieldOffset(0x0)] int X, [field: FieldOffset(0x4)] int Y);