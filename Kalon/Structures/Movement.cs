using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kalon.Structures
{
    internal sealed record Movement(TimeSpan Delay, IEnumerable<Point> Points);
}