namespace Mz.App.Fonts
{
    public class Size
    {
        public Size(CoreBase core)
        {
            Core = core;
            _fontSizeBase = (int) (_fontSizeOrigin * 3.5f);
        }
        
        public int Base
        {
            get => Core.ScaleValueInt(_fontSizeBase);
            set => _fontSizeBase = value;
        }

        private CoreBase Core;
        private int _fontSizeOrigin = 12;
        private int _fontSizeBase;
        
        // 12          Origin
        // 18 12*1.5
        // 24 12*2     Smaller1
        // 30 12*2.5   -
        // 36 12*3     Base     
        // 42 12*3.5   -
        // 48 12*4     Larger1
        // 54 12*4.5   -  
        // 60 12*5     Larger2
        // 66 12*5.5   -
        // 72 12*6     Larger3
        // 78 12*6.5   -
        // 84 12*7     Larger4
        // 90 12*7.5   -
        // 96 12*8     Larger5

        public int Larger1 => (int) Core.ScaleValue(_fontSizeOrigin * 4f);
        public int Larger2 => (int) Core.ScaleValue(_fontSizeOrigin * 5f);
        public int Larger3 => (int) Core.ScaleValue(_fontSizeOrigin * 6f);
        public int Larger4 => (int) Core.ScaleValue(_fontSizeOrigin * 7f);
        public int Larger5 => (int) Core.ScaleValue(_fontSizeOrigin * 8f);

        public int Smaller1 => (int) Core.ScaleValue(_fontSizeOrigin * 3f);
        public int Smaller2 => (int) Core.ScaleValue(_fontSizeOrigin * 2f);
        
        public int Icon1 => Larger1;
        public int Icon2 => Larger2;
        public int Icon3 => Larger3;
        public int Icon4 => Larger5;
    }
}