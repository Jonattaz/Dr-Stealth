using System;
using System.Xml.Serialization;
using Mz.Models;

namespace Mz.DemoDataModels.Models.Xml
{
    //===== These are the XML versions of the data models we've defined for our demo.
    //
    // These can be whatever you want, depending on the data you wish to store.
    //
    // When we're using XML, rather than JSON, our data models are going to be a bit
    // more complicated, since we need to use property attribute tags to describe how 
    // we want the data to flow into our models. Also, unlike JSON, XML doesn't support
    // arrays, so we need to structure our models a little differently.
    //
    // Internally, we use Microsoft's XmlSerializer class, which doesn't allow us to
    // to deserialize our data into private properties. This means we need to
    // declare our property setters as public, and we'll have to be careful not to 
    // use the setter directly to modify the data, which is a common source of bugs.
    // For this reason, I recommend using JSON, rather than XML, if you can. By marking
    // the setters as private, you ensure that the data won't accidentally be modified
    // directly. Your data object becomes effectively immutable, so  all changes are
    // constrained to a single channel that flows through your Model interface. This
    // will eliminate a host of potential bugs, and allow you to monitor all your data
    // changes for further debugging purposes. We can still take advantage of these 
    // benefits using XML, as long as we remember to only use the Set() method, rather
    // than the property setter to modify the data.
    //
    // 1) Tag your data classes with the [Serialize] attribute.
    // 
    // 2) Use attribute tags to describe the data. 
    //    See https://docs.microsoft.com/en-us/dotnet/standard/serialization/attributes-that-control-xml-serialization
    //
    // 3) Declare our property setters as public.

    [Serializable]
    public class GameData
    {
        [XmlArray("Packs")]
        [XmlArrayItem("Pack")]
        public PackData[] Packs { get; set; }
        
        [XmlAttribute("PackCurrent")]
        public int PackCurrent { get; set; }
        
        [XmlAttribute("LevelCurrent")]
        public int LevelCurrent { get; set; }
    }
    
    [Serializable]
    public class PackData
    {
        [XmlAttribute("Key")]
        public int Key { get; set; }
        
        [XmlAttribute("Label")]
        public string Label { get; set; }
        
        [XmlAttribute("IsPurchased")]
        public bool IsPurchased { get; set; }
        
        [XmlArray("Levels")]
        [XmlArrayItem("Level")]
        public int[] Levels { get; set; }
        
        [XmlAttribute("Description")]
        public string Description { get; set; }
    }
    
    //===== Below is a custom Model class that illustrates data validation techniques.
    // NOTE: You don't need to define custom model classes, but they can provide 
    // some convenient features in certain cases.
    
    public class ModelGame : Model<GameData>
    {
        // It's not necessary to define a custom Model class for our data,
        // But we're doing it here to demonstrate the use of a custom
        // validation method (SetLevel below) to update the data held by the model.
        
        // We can use custom methods on our data object to validate incoming data
        // before we commit it to our model. This one ensures that level and pack
        // information is constrained to game packs that are actually installed
        // for the current user.
        public void SetLevel(int packNumber, int level)
        {
            if (packNumber > Data.Packs.Length) packNumber = Data.Packs.Length;
            if (packNumber < 1) packNumber = 1;

            var packData = Data.Packs[packNumber - 1];

            if (level > packData.Levels.Length) level = packData.Levels.Length;
            if (level < 1) level = 1;

            // Change the data in the model, but don't trigger an auto-save yet.
            Set(data => data.PackCurrent, packNumber, false, false);
            Set(data => data.LevelCurrent, level, false, false);
        }
        
        // This is just a convenience method to allow us to easily increment 
        // our current game level.
        public void NextLevel()
        {
            SetLevel(Data.PackCurrent, Data.LevelCurrent + 1);
        }
    }
}