using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Kalon.Native.PInvoke;
using Kalon.Structures;

[assembly: CLSCompliant(true)]

namespace Kalon
{
    /// <summary>
    /// Provides the functionality to move the cursor in a human realistic manner
    /// </summary>
    public static class CursorMover
    {
        private static readonly Random _random;

        static CursorMover()
        {
            _random = new Random();
        }

        /// <summary>
        /// Moves the cursor to a point in a timespan
        /// </summary>
        public static void MoveCursor(Point point, TimeSpan timeSpan)
        {
            if (point.X < 0 || point.Y < 0)
            {
                throw new ArgumentException("The provided point was invalid");
            }

            if (timeSpan.TotalMilliseconds <= 0)
            {
                throw new ArgumentException("The provided timespan was invalid");
            }

            // Generate a randomised set of movements from the current cursor position to the point

            if (!User32.GetCursorPos(out var currentCursorPosition))
            {
                throw new Win32Exception();
            }

            var cursorMovements = GenerateMovements(currentCursorPosition, point, (int) timeSpan.TotalMilliseconds);

            // Perform the movements

            var stopwatch = Stopwatch.StartNew();

            foreach (var movement in cursorMovements)
            {
                if (movement.Points.Any(movementPoint => !User32.SetCursorPos(movementPoint.X, movementPoint.Y)))
                {
                    throw new Win32Exception();
                }

                while (stopwatch.ElapsedMilliseconds < movement.Delay.Milliseconds)
                {
                    // Wait
                }

                stopwatch.Restart();
            }
        }

        private static IEnumerable<Movement> GenerateMovements(Point start, Point end, int milliseconds)
        {
            IEnumerable<int> FisherYatesShuffle(IList<int> collection, int elements)
            {
                for (var elementIndex = 0; elementIndex < collection.Count; elementIndex += 1)
                {
                    var randomIndex = _random.Next(0, elementIndex);

                    var currentValue = collection[elementIndex];

                    collection[elementIndex] = collection[randomIndex];

                    collection[randomIndex] = currentValue;
                }

                return collection.Take(elements);
            }

            var pathPoints = GeneratePath(start, end).ToArray();

            if (milliseconds <= pathPoints.Length)
            {
                var pointsPerMovement = pathPoints.Length / milliseconds;

                // Determine the amount of remaining points that need to be distributed between the movements

                var remainingPoints = pathPoints.Length - milliseconds * pointsPerMovement;

                // Randomly distribute the remaining points using a Fisher Yates shuffle

                var distributionIndexes = FisherYatesShuffle(Enumerable.Range(0, milliseconds).ToArray(), remainingPoints).ToHashSet();

                // Initialise the movements

                var pointsUsed = 0;

                for (var movementIndex = 0; movementIndex < milliseconds; movementIndex += 1)
                {
                    var movementPoints = pointsPerMovement;

                    if (distributionIndexes.Contains(movementIndex))
                    {
                        movementPoints += 1;
                    }

                    yield return new Movement(TimeSpan.FromMilliseconds(1), pathPoints.Skip(pointsUsed).Take(movementPoints));

                    pointsUsed += movementPoints;
                }
            }

            else
            {
                var delayPerMovement = milliseconds / pathPoints.Length;

                // Determine the amount of remaining milliseconds that need to be distributed between the movements

                var remainingMilliseconds = milliseconds - pathPoints.Length * delayPerMovement;

                // Randomly distribute the remaining milliseconds using a Fisher Yates shuffle

                var distributionIndexes = FisherYatesShuffle(Enumerable.Range(0, pathPoints.Length).ToArray(), remainingMilliseconds).ToHashSet();

                // Initialise the movements

                for (var movementIndex = 0; movementIndex < pathPoints.Length; movementIndex += 1)
                {
                    var movementDelay = delayPerMovement;

                    if (distributionIndexes.Contains(movementIndex))
                    {
                        movementDelay += 1;
                    }

                    yield return new Movement(TimeSpan.FromMilliseconds(movementDelay), pathPoints.Skip(movementIndex).Take(1));
                }
            }
        }

        private static IEnumerable<Point> GeneratePath(Point start, Point end)
        {
            // Generate randomised control points with a displacement of 15% to 30% between the start and end points

            var arcMultipliers = new[] {-1, 1};

            var arcMultiplier = arcMultipliers[_random.Next(arcMultipliers.Length)];

            Point GenerateControlPoint()
            {
                var x = start.X + arcMultiplier * (Math.Abs(end.X - start.X) + 50) * 0.01 * _random.Next(15, 30);

                var y = start.Y + arcMultiplier * (Math.Abs(end.Y - start.Y) + 50) * 0.01 * _random.Next(15, 30);

                return new Point((int) x, (int) y);
            }

            var anchorPoints = new[] {start, GenerateControlPoint(), GenerateControlPoint(), end};

            // Generate 5000 points of a third order Bezier curve using De Casteljau's algorithm

            var binomialCoefficients = new[] {1, 3, 3, 1};

            yield return start;

            for (var pointIndex = 0; pointIndex < 4998; pointIndex += 1)
            {
                var tValue = pointIndex / 4998d;

                var x = 0d;

                var y = 0d;

                for (var anchorPointIndex = 0; anchorPointIndex < anchorPoints.Length; anchorPointIndex += 1)
                {
                    x += anchorPoints[anchorPointIndex].X * binomialCoefficients[anchorPointIndex] * Math.Pow(1 - tValue, 3 - anchorPointIndex) * Math.Pow(tValue, anchorPointIndex);

                    y += anchorPoints[anchorPointIndex].Y * binomialCoefficients[anchorPointIndex] * Math.Pow(1 - tValue, 3 - anchorPointIndex) * Math.Pow(tValue, anchorPointIndex);
                }

                yield return new Point((int) x, (int) y);
            }

            yield return end;
        }
    }
}