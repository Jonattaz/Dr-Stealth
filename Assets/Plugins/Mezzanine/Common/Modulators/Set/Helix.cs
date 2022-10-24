using Mz.Numerics;

namespace Mz.Modulators
{
    public class Helix : ModulatorBase
    {
        public Helix(float amplitude = 0.1f, float windingRadius = 1, float progressMin = -10, float progressMax = 10)
        {
            _amplitude = amplitude;
            _windingRadius = windingRadius;
            _progressMin = progressMin;
            _progressMax = progressMax;
        }
        
        private float _windingRadius = 1;
        private float _amplitude = 1;
        private float _progressMin;
        private float _progressMax;
        
        public override IMetric Modulate(float progressNormalized)
        {
            var progress = _ScaleProgress(progressNormalized, _progressMin, _progressMax, 0, 1);
            var x = Numbers.Cos(progress) * _windingRadius;
            var y = Numbers.Sin(progress) * _windingRadius;
            var z = _amplitude * progress;

            return new MzVector(x, y, z);
        }
        
    }
}