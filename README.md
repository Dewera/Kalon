## Kalon

![](https://github.com/Dewera/Kalon/workflows/Continuous%20Integration/badge.svg)

A humanlike cursor movement library that uses randomised bezier curves for path generation

---

### Caveats

- The provided delay applies exclusively to the movement of the cursor rather than the total runtime of the method

---

### Getting started

The example below demonstrates a basic implementation of the library

```c#
var point = new Point(0, 0);

var delay = TimeSpan.FromMilliseconds(0);

CursorMover.MoveCursor(point, delay);
```

---

### CursorMover Class

Provides the functionality to move the cursor in a human realistic manner

```c#
public static class CursorMover
```

### Methods

Moves the cursor to a point

```c#
MoveCursor(Point, TimeSpan)
```