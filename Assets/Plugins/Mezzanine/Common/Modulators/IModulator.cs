using Mz.Numerics;

namespace Mz.Modulators
{
    public interface IModulator
    {
        IMetric Modulate(float progressLinear);
    }
}