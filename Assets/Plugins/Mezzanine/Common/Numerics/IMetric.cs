namespace Mz.Numerics
{
    using NumericBaseType = System.Single;

    public interface IMetric
    {
        NumericBaseType[] Values { get; }
        IMetric Empty { get; }
    }
}