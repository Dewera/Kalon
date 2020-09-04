using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kalon
{
    internal sealed class Movement
    {
        internal TimeSpan Delay { get; }

        internal IEnumerable<Point> Points { get; }

        internal Movement(TimeSpan delay, IEnumerable<Point> points)
        {
            Delay = delay;

            Points = points;
        }
    }
}