﻿using System.ComponentModel;
using System.Diagnostics;
using Kalon.Tests.Native.PInvoke;
using Kalon.Tests.Native.Structs;
using Xunit;

namespace Kalon.Tests;

public sealed class CursorMoverTests
{
    private readonly int _runtimeBuffer;

    public CursorMoverTests()
    {
        _runtimeBuffer = 100;
    }

    [Theory]
    [InlineData(0, 0, 20)]
    [InlineData(83, 27, 150)]
    [InlineData(27, 83, 880)]
    [InlineData(100, 100, 1200)]
    [InlineData(250, 400, 3500)]
    [InlineData(400, 250, 8000)]
    public void TestRuntime(int x, int y, int milliseconds)
    {
        var stopwatch = Stopwatch.StartNew();

        CursorMover.MoveCursor(x, y, TimeSpan.FromMilliseconds(milliseconds));
        stopwatch.Stop();

        // The runtime duration needs to be buffered to account for the movement initialisation

        Assert.InRange(stopwatch.ElapsedMilliseconds, milliseconds, milliseconds + _runtimeBuffer);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(83, 27)]
    [InlineData(27, 83)]
    [InlineData(100, 100)]
    [InlineData(250, 400)]
    [InlineData(400, 250)]
    public void TestMovement(int x, int y)
    {
        CursorMover.MoveCursor(x, y, TimeSpan.FromMilliseconds(1));

        if (!User32.GetCursorPos(out var currentCursorPosition))
        {
            throw new Win32Exception();
        }

        Assert.Equal(new Point(x, y), currentCursorPosition);
    }
}