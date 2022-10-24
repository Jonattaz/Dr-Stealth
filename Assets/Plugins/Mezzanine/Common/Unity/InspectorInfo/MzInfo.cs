using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Info Asset", menuName = "Mezzanine/Data Objects/MzInfo")]
public class MzInfo : ScriptableObject
{
    public string assetKey;
    public string assetStoreUrl;

    public Color colorHighlight;
    
    public Texture2D bannerLeft;
    public Texture2D bannerRight;

    public string demoScene;
    public string demoCode;
    
    public Section[] sections;
	
    [Serializable]
    public class Section
    {
        public string heading;
        
        [TextArea(3, 10)]
        public string text;
        
        public string linkText;
        public string url;
    }
}