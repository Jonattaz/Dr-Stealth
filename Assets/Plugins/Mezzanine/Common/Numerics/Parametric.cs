namespace Mz.Numerics
{
    public partial class Numbers
    {
        public static float ParametricScale(float progress, float minTarget, float maxTarget, float minActual,float  maxActual) 
        {
            return (maxTarget - minTarget) * (progress - minActual) / (maxActual - minActual) + minTarget;
        }
    }
}