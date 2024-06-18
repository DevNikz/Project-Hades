using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityState {
    Strafing,
    Dashing,
    Idle
}

public enum EntityDirection {
    North,
    NorthEast,
    East,
    SouthEast,
    South,
    SouthWest,
    West,
    NorthWest,
    None
}

public enum AnimDirection {
    Up,
    Down,
    Left,
    Right
}
