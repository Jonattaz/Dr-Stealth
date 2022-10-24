using System;
using Mz.Models;

namespace Mz.DemoDataModels.Models
{
    //===== These are the actual data models we've defined for our demo.
    //
    // These can be whatever you want, depending on the data you wish to store.
    //
    // There are only two stipulations for how you define your data:
    //
    // 1) Tag your data classes with the [Serialize] attribute.
    //
    // 2) Provide private setters for your properties, as shown,
    // in order to be able to modify the data dynamically later,
    // using the Model interface.
    //
    // By marking the setters as private, you ensure that the data won't
    // accidentally be modified directly. Your data object is now effectively
    // immutable, so  all changes are constrained to a single channel that flows
    // through your Model interface. This will eliminate a host of potential
    // bugs, and allow you to monitor all your data changes for further debugging
    // purposes. It will also allow you to respond to requested data changes and
    // decide when to actually apply the changes, react to changes in other
    // parts of your code, and automatically save the data to disk.

    [Serializable]
    public class GameData
    {
        public PackData[] Packs { get; private set; }
        public int PackCurrent { get; private set; }
        public int LevelCurrent { get; private set; }
    }
    
    [Serializable]
    public class PackData
    {
        public int Key { get; private set; }
        public string Label { get; private set; }
        public bool IsPurchased { get; private set; }
        public int[] Levels { get; private set; }
        public string Description { get; private set; }
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