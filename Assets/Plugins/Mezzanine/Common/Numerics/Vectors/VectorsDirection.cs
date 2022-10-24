namespace Mz.Numerics
{
    public partial class Vectors
    {
        public static MzVector Up => new MzVector(0, 1, 0);
        public static MzVector Down => new MzVector(0, -1, 0);
        public static MzVector Left => new MzVector(-1, 0, 0);
        public static MzVector Right => new MzVector(1, 0, 0);
        public static MzVector ForwardRightHanded => new MzVector(0, 0, -1);
        public static MzVector ForwardLeftHanded => new MzVector(0, 0, 1);
        public static MzVector BackwardRightHanded => new MzVector(0, 0, 1);
        public static MzVector BackwardLeftHanded => new MzVector(0, 0, -1);
        public static MzVector Forward => ForwardLeftHanded;
        public static MzVector Backward => BackwardLeftHanded;
        
        public static MzVector From(Direction direction, MzVector forward = default) 
        {
            if (forward.IsEmpty) forward = new MzVector(0f, 1f, 0f);
            var vector = new MzVector(0.0f, 0.0f);
            switch (direction) 
            {
                case Numerics.Direction.None:
                    return new MzVector(0f, 0f, 0f);
                case Numerics.Direction.Forward:
                    if (forward.X > Numbers.Epsilon) return new MzVector(1f, 0f, 0f);
                    else if (forward.Y > Numbers.Epsilon) return new MzVector(0f, 1f, 0f);
                    else if (forward.Z > Numbers.Epsilon) return new MzVector(0f, 0f, 1f);
                    return new MzVector(0f, 0f, 0f);
                case Numerics.Direction.ForwardRight:
                    if (forward.X > Numbers.Epsilon) return new MzVector(1f, -1f, 0f);
                    else if (forward.Y > Numbers.Epsilon) return new MzVector(1f, 1f, 0f);
                    else if (forward.Z > Numbers.Epsilon) return new MzVector(1f, 0f, 1f);
                    return new MzVector(0f, 0f, 0f);
                case Numerics.Direction.ForwardLeft:
                    if (forward.X > Numbers.Epsilon) return new MzVector(1f, 1f, 0f);
                    else if (forward.Y > Numbers.Epsilon) return new MzVector(-1f, 1f, 0f);
                    else if (forward.Z > Numbers.Epsilon) return new MzVector(-1f, 0f, 1f);
                    return new MzVector(0f, 0f, 0f);
                case Numerics.Direction.Backward:
                    if (forward.X > Numbers.Epsilon) return new MzVector(-1f, 0f, 0f);
                    else if (forward.Y > Numbers.Epsilon) return new MzVector(0f, -1f, 0f);
                    else if (forward.Z > Numbers.Epsilon) return new MzVector(0f, 0f, -1f);
                    return new MzVector(0f, 0f, 0f);
                case Numerics.Direction.BackwardRight:
                    if (forward.X > Numbers.Epsilon) return new MzVector(-1f, -1f, 0f);
                    else if (forward.Y > Numbers.Epsilon) return new MzVector(1f, -1f, 0f);
                    else if (forward.Z > Numbers.Epsilon) return new MzVector(1f, 0f, -1f);
                    return new MzVector(0f, 0f, 0f);
                case Numerics.Direction.BackwardLeft:
                    if (forward.X > Numbers.Epsilon) return new MzVector(-1f, 1f, 0f);
                    else if (forward.Y > Numbers.Epsilon) return new MzVector(-1f, -1f, 0f);
                    else if (forward.Z > Numbers.Epsilon) return new MzVector(-1f, 0f, -1f);
                    return new MzVector(0f, 0f, 0f);
                case Numerics.Direction.Right:
                    if (forward.X > Numbers.Epsilon) return new MzVector(0f, -1f, 0f);
                    else if (forward.Y > Numbers.Epsilon) return new MzVector(1f, 0f, 0f);
                    else if (forward.Z > Numbers.Epsilon) return new MzVector(1f, 0f, 0f);
                    return new MzVector(0f, 0f, 0f);
                case Numerics.Direction.Left:
                    if (forward.X > Numbers.Epsilon) return new MzVector(0f, 1f, 0f);
                    else if (forward.Y > Numbers.Epsilon) return new MzVector(-1f, 0f, 0f);
                    else if (forward.Z > Numbers.Epsilon) return new MzVector(-1f, 0f, 0f);
                    return new MzVector(0f, 0f, 0f);
                default:
                    return new MzVector(0f, 0f, 0f);
            }
        }
        
        /// <summary>
        /// Calculate and return the direction unit vector from point a to point b.
        /// </summary>
        /// <returns>The normalised direction unit mzVector between point a and point b.</returns>
        public static MzVector Direction(MzVector a, MzVector b)
        {
            return (b - a).Normalized;
        }
    }
}