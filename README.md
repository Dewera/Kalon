## Kalon

A humanlike cursor movement library that uses randomised bezier curves for path generation

---

### Example

The image below demonstrates some generated movements between a set of points

![](https://user-images.githubusercontent.com/59136536/92340978-5a00e400-f110-11ea-9af1-2b0de502d4cb.png)

---

### Caveats

- The provided delay applies exclusively to the movement of the cursor rather than the total runtime of the method

---

### Getting started

The example below demonstrates a basic implementation of the library

```c#
var delay = TimeSpan.FromMilliseconds(0);
CursorMover.MoveCursor(0, 0, delay);
```

---

### CursorMover Class

Provides the functionality to move the cursor in a human realistic manner

```c#
public static class CursorMover
```

### Methods

Moves the cursor to a set of coordinates in a timespan

```c#
public static void MoveCursor(int, int, TimeSpan);
```
