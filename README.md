## Kalon

![](https://github.com/Dewera/Kalon/workflows/Continuous%20Integration/badge.svg)

A humanlike cursor movement library that uses randomised bezier curves for path generation

---

### Caveats

- The time taken to generate a randomised path differs based on many factors and, hence, the provided delay applies exclusively to the movement of the cursor, rather than the total runtime of the method

---

### Getting started

The example below demonstrates a basic implementation of the library that moves the cursor to the point (185, 235) in 800 milliseconds

```c#
var point = new Point(185, 235);

var delay = TimeSpan.FromMilliseconds(800);

CursorMover.MoveCursor(point, delay);
```

---

### CursorMover Class

#### Methods

```c#
MoveCursor(Point, TimeSpan)
```

Moves the cursor to a point

---
