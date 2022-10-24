namespace Mz.Numerics {
    public struct DirectionStep {
        public readonly sbyte X;
        public readonly sbyte Y;
        public readonly sbyte Z;

        public DirectionStep(sbyte x, sbyte y, sbyte z = 0) {
            X = x;
            Y = y;
            Z = z;
        }
    }
}