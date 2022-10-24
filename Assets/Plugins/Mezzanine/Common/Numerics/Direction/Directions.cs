using System;

namespace Mz.Numerics
{
    /// <summary>
    /// Represents any combinations of the eight directions in a rectangular tileMap.
    /// </summary>
    [Flags]
    public enum Directions : int
    {
        /// <summary>
        /// No direction.
        /// </summary>
        None = 0,

        /// <summary>
        /// Step to the right (0 degrees).
        /// </summary>
        Right = 1 << (Direction.Right - 1),

        /// <summary>
        /// Step to the upper right. (45 degrees).
        /// </summary>
        UpRight = 1 << (Direction.UpRight - 1),

        /// <summary>
        /// Step upwards. (90 degrees).
        /// </summary>
        Up = 1 << (Direction.Up - 1),

        /// <summary>
        /// Step to the upper left (135 degrees).
        /// </summary>
        UpLeft = 1 << (Direction.UpLeft - 1),

        /// <summary>
        /// Step to the left (180 degrees).
        /// </summary>
        Left = 1 << (Direction.Left - 1),

        /// <summary>
        /// Step to the lower left (225 degrees).
        /// </summary>
        DownLeft = 1 << (Direction.DownLeft - 1),

        /// <summary>
        /// Downwards step. (270 degrees).
        /// </summary>
        Down = 1 << (Direction.Down - 1),

        /// <summary>
        /// Step to the lower right (315 degrees).
        /// </summary>
        DownRight = 1 << (Direction.DownRight - 1),

        /// <summary>
        /// All 2D directions.
        /// </summary>
        All2D = Right | UpRight | Up | UpLeft | Left | DownLeft | Down | DownRight,

        /// <summary>
        /// Step forward in Z space.
        /// </summary>
        Forward = 1 << (Direction.Forward - 1),

        /// <summary>
        /// Step forward and to the right (0 degrees).
        /// </summary>
        ForwardRight = 1 << (Direction.ForwardRight - 1),

        /// <summary>
        /// Step forward and to the upper right. (45 degrees).
        /// </summary>
        ForwardUpRight = 1 << (Direction.ForwardUpRight - 1),

        /// <summary>
        /// Step forward and upwards. (90 degrees).
        /// </summary>
        ForwardUp = 1 << (Direction.ForwardUp - 1),

        /// <summary>
        /// Step forward and to the upper left (135 degrees).
        /// </summary>
        ForwardUpLeft = 1 << (Direction.ForwardUpLeft - 1),

        /// <summary>
        /// Step forward and to the left (180 degrees).
        /// </summary>
        ForwardLeft = 1 << (Direction.ForwardLeft - 1),

        /// <summary>
        /// Step forward and to the lower left (225 degrees).
        /// </summary>
        ForwardDownLeft = 1 << (Direction.ForwardDownLeft - 1),

        /// <summary>
        /// Step forward and downward. (270 degrees).
        /// </summary>
        ForwardDown = 1 << (Direction.ForwardDown - 1),

        /// <summary>
        /// Step forward and to the lower right (315 degrees).
        /// </summary>
        ForwardDownRight = 1 << (Direction.ForwardDownRight - 1),

        /// <summary>
        /// Step backward in Z space.
        /// </summary>
        Backward = 1 << (Direction.Backward - 1),

        /// <summary>
        /// Step backward and to the right (0 degrees).
        /// </summary>
        BackwardRight = 1 << (Direction.BackwardRight - 1),

        /// <summary>
        /// Step backward and to the upper right. (45 degrees).
        /// </summary>
        BackwardUpRight = 1 << (Direction.BackwardUpRight - 1),

        /// <summary>
        /// Step backward and upwards. (90 degrees).
        /// </summary>
        BackwardUp = 1 << (Direction.BackwardUp - 1),

        /// <summary>
        /// Step backward and to the upper left (135 degrees).
        /// </summary>
        BackwardUpLeft = 1 << (Direction.BackwardUpLeft - 1),

        /// <summary>
        /// Step backward and to the left (180 degrees).
        /// </summary>
        BackwardLeft = 1 << (Direction.BackwardLeft - 1),

        /// <summary>
        /// Step backward and to the lower left (225 degrees).
        /// </summary>
        BackwardDownLeft = 1 << (Direction.BackwardDownLeft - 1),

        /// <summary>
        /// Step backward and downward. (270 degrees).
        /// </summary>
        BackwardDown = 1 << (Direction.BackwardDown - 1),

        /// <summary>
        /// Step backward and to the lower right (315 degrees).
        /// </summary>
        BackwardDownRight = 1 << (Direction.BackwardDownRight - 1),

        /// <summary>
        /// All 3D directions.
        /// </summary>
        All3D = Right | UpRight | Up | UpLeft | Left | DownLeft | Down | DownRight | 
                Forward | ForwardRight | ForwardUpRight | ForwardUp | ForwardUpLeft | ForwardLeft | ForwardDownLeft | ForwardDown | ForwardDownRight |
                Backward | BackwardRight | BackwardUpRight | BackwardUp | BackwardUpLeft | BackwardLeft | BackwardDownLeft | BackwardDown | BackwardDownRight,
        
        XAxis = Left | Right,
        YAxis = Up | Down,
        ZAxis = Backward | Forward,
    }
}