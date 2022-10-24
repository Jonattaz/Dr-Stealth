namespace Mz.App
{
    public class Specs
    {
        public Specs(CoreBase core)
        {
            Core = core;
            GutterHorizontal = (int)(core.ScreenWidth * 0.125f);
        }

        public int ControlWidth = 270;
        public int ControlHeight = 120;
        public int ControlBarMarginVertical = 90;
        public int ControlBarHeight => ControlHeight + ControlBarMarginVertical * 2;
        public int GutterHorizontal { get; }

        public int Margin = 60;

        private CoreBase Core;
    }
}