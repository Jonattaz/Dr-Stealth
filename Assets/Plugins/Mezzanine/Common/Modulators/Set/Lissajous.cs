using Mz.Numerics;

namespace Mz.Modulators
{
    public class Lissajous : ModulatorBase
    {
        public Lissajous(float a = 2, float b = 3, float c = 0, float progressMin = -20, float progressMax = 20)
        {
            _a = a;
            _b = b;
            _c = c;
        }
        
        private float _a;
        private float _b;
        private float _c;
        private float _progressMin;
        private float _progressMax;

        public override IMetric Modulate(float progressNormalized)
        {
            var progress = _ScaleProgress(progressNormalized, 0, 6.5f, 0, 1);
            var x = Numbers.Sin(_a * progress + _c);
            var y = Numbers.Cos(_b * progress + _c);

            return new MzVector(x, y);
        }
    }
}