namespace Mz.Numerics 
{
    /// <summary>
    /// Represents the eight directions of movement in a rectangular tileMap.
    /// </summary>
    public enum Direction : byte 
    {
        /// <summary>
        /// Unknown/no direction value. Steps in this direction have no effect.
        /// </summary>
        None = 0,
        /// <summary>
        /// Step to the right (0 degrees).
        /// </summary>
        Right = 1,
        /// <summary>
        /// Step to the upper right. (45 degrees).
        /// </summary>
        UpRight = 2,
        /// <summary>
        /// Step upwards. (90 degrees).
        /// </summary>
        Up = 3,
        /// <summary>
        /// Step to the upper left (135 degrees).
        /// </summary>
        UpLeft = 4,
        /// <summary>
        /// Step to the left (180 degrees).
        /// </summary>
        Left = 5,
        /// <summary>
        /// Step to the lower left (225 degrees).
        /// </summary>
        DownLeft = 6,
        /// <summary>
        /// Downwards step. (270 degrees).
        /// </summary>
        Down = 7,
        /// <summary>
        /// Step to the lower right (315 degrees).
        /// </summary>
        DownRight = 8,
        
        /// <summary>
        /// Step forward in Z space.
        /// </summary>
        Forward = 9,
        /// <summary>
        /// Step forward and to the right (0 degrees).
        /// </summary>
        ForwardRight = 10,
        /// <summary>
        /// Step forward and to the upper right. (45 degrees).
        /// </summary>
        ForwardUpRight = 11,
        /// <summary>
        /// Step forward and upwards. (90 degrees).
        /// </summary>
        ForwardUp = 12,
        /// <summary>
        /// Step forward and to the upper left (135 degrees).
        /// </summary>
        ForwardUpLeft = 13,
        /// <summary>
        /// Step forward and to the left (180 degrees).
        /// </summary>
        ForwardLeft = 14,
        /// <summary>
        /// Step forward and to the lower left (225 degrees).
        /// </summary>
        ForwardDownLeft = 15,
        /// <summary>
        /// Step forward and downward. (270 degrees).
        /// </summary>
        ForwardDown = 16,
        /// <summary>
        /// Step forward and to the lower right (315 degrees).
        /// </summary>
        ForwardDownRight = 17,
        
        /// <summary>
        /// Step backward in Z space.
        /// </summary>
        Backward = 18,
        /// <summary>
        /// Step backward and to the right (0 degrees).
        /// </summary>
        BackwardRight = 19,
        /// <summary>
        /// Step backward and to the upper right. (45 degrees).
        /// </summary>
        BackwardUpRight = 20,
        /// <summary>
        /// Step backward and upwards. (90 degrees).
        /// </summary>
        BackwardUp = 21,
        /// <summary>
        /// Step backward and to the upper left (135 degrees).
        /// </summary>
        BackwardUpLeft = 22,
        /// <summary>
        /// Step backward and to the left (180 degrees).
        /// </summary>
        BackwardLeft = 23,
        /// <summary>
        /// Step backward and to the lower left (225 degrees).
        /// </summary>
        BackwardDownLeft = 24,
        /// <summary>
        /// Step backward and downward. (270 degrees).
        /// </summary>
        BackwardDown = 25,
        /// <summary>
        /// Step backward and to the lower right (315 degrees).
        /// </summary>
        BackwardDownRight = 26,
    }
}