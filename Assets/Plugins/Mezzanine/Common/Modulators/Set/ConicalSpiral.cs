using Mz.Numerics;

namespace Mz.Modulators
{
    public class ConicalSpiral : ModulatorBase
    {
        public ConicalSpiral(float windings = 6, float radius = 1.5f, float coneLength = 1)
        {
            _windings = windings;
            _radius = radius;
            _progressMin = 0;
            _progressMax = coneLength;
        }

        private float _windings;
        private float _radius;
        private float _progressMin;
        private float _progressMax;
        
        public override IMetric Modulate(float progressNormalized)
        {
            var progress = _ScaleProgress(progressNormalized, _progressMin, _progressMax, 0, 1);
            var x = progress * Numbers.Cos(_windings * 6 * progress) * _radius;
            var y = progress * Numbers.Sin(_windings * 6 * progress) * _radius;
            var z = progress;

            return new MzVector(x, y, z);
        }
        
    }
}