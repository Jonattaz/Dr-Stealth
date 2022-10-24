using Mz.Numerics;

namespace Mz.Modulators
{
    public abstract class ModulatorBase : IModulator
    {
        protected float _ScaleProgress(float progress, float minTarget, float maxTarget, float minActual,float  maxActual) 
        {
            return (maxTarget - minTarget) * (progress - minActual) / (maxActual - minActual) + minTarget;
        }

        public abstract IMetric Modulate(float progressNormalized);
    }
}