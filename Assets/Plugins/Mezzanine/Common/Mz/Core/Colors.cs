using UnityEngine;

namespace Mz.App
{
    public class Colors
    {
        public Color VeryLight => new Color(1, 1, 1, 1);
        
        public Color Light { get; set; }
        
        public Color MediumLight3 => new Color(Light.r, Light.g, Light.b, 0.96f);
        public Color MediumLight2 => new Color(Light.r, Light.g, Light.b, 0.84f);
        public Color MediumLight => new Color(Light.r, Light.g, Light.b, 0.72f);
        public Color MediumLight0 => new Color(Light.r, Light.g, Light.b, 0.60f);
        
        public Color MediumDark0 => new Color(Light.r, Light.g, Light.b, 0.48f);
        public Color MediumDark => new Color(Light.r, Light.g, Light.b, 0.24f);
        public Color MediumDark2 => new Color(Light.r, Light.g, Light.b, 0.12f);
        public Color MediumDark3 => new Color(Light.r, Light.g, Light.b, 0.06f);
        
        public Color Dark { get; set; }
        
        public Color VeryDark => new Color(0, 0, 0,  1);
        
        public Color Highlight { get; set; }
    }
}
