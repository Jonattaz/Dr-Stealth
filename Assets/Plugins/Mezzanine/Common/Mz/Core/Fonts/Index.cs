using UnityEngine;

namespace Mz.App.Fonts
{
    public class Index
    {
        public Index(CoreBase core)
        {
            Size = new Size(core);
        }
        
        public Font Main { get; set; }
        public Font MainSemibold { get; set; }
        public Font MainBold { get; set; }
        public Font Icon { get; set; }
        
        public Size Size { get; }
    }
}
