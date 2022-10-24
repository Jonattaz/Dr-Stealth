namespace Mz.Models
{
    public class ModelChange
    {
        public ModelChange(
            string propertyPath,
            object valueCurrent,
            object valuePrevious
        )
        {
            PropertyPath = propertyPath;
            ValueCurrent = valueCurrent;
            ValuePrevious = valuePrevious;
        }

        public string PropertyPath { get; }
        public object ValueCurrent { get; set; }
        public object ValuePrevious { get; }
    }
}