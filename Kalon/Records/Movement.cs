using System.Drawing;

namespace Kalon.Records;

internal sealed record Movement(TimeSpan Delay, IEnumerable<Point> Points);