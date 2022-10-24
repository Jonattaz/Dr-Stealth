using System.Text;
using UnityEngine;
using System.Timers;
using Mz.Models;
using Mz.App;

using Mz.DemoDataModels.Models;
// Use the namespace below instead for the XML demo.
// using Mz.DemoDataModels.Models.Xml;

namespace Mz.DemoDataModels
{
    public class MzBehaviour : MzBehaviourBase<CoreDemoDataModels> {}
    
    public class CoreDemoDataModels : CoreBase
    { 
        public Screens.Index Screens { get; private set; }

        // This is where we'll reference our main data model for our game.
        // We can have multiple models, and even nested models, so there's 
        // nothing special about this particular property... it's just
        // an example implementation.
        //
        // Take a look at Assets/Plugins/Mezzanine/DataModels/Demo/Models/ModelGame.cs
        // for the actual implementation of the data model, which is just a standard
        // C# class with properties that map to your desired data objects.
        public ModelGame Model { get; set; }

        private Timer _timer;
        private Screens.Home.Packs.Gallery.Pack.Index _pack;

        protected override void OnInitializeStarted()
        {
            //===== START: Demo setup code, nothing to see here.
            // Colors
            Core.Colors.Dark = new Color(33f / 255f, 34f / 255f, 35f / 255f);
            Core.Colors.Light = new Color(192f / 255f, 190f / 255f, 187f / 255f);
            Core.Colors.Highlight = new Color(1f, 0f, 0f, 1f);
            
            // Fonts
            Core.Fonts.MainSemibold = Resources.Load<Font>("Fonts/Heebo-Medium");
            Core.Fonts.MainBold = Resources.Load<Font>("Fonts/Heebo-Bold");
            Core.Fonts.Icon = Resources.Load<Font>("Fonts/FiraMono-Medium");
            
            // Canvas
            Core.Canvas.BackgroundColor = Core.Colors.VeryDark;

            // Screens
            Screens = Core.Canvas.Add<Screens.Index>("Screens");
            //===== END: Demo setup code.

            // Data Model
            //
            // Take a look at Demo/Resources/Packs/index.txt
            // for the JSON formatted initial data.
            // Or, Demo/Resources/Packs/index.txt for the XML.
            //
            // A couple of things to note here. First, Unity expects any files you're going to load 
            // dynamically to live in a folder named "Resources". You can have multiple such folders,
            // but they all need to be nested somewhere under the Assets folder.
            // Second, file extensions should be ".txt", regardless of what type of data the file actually holds.
            
            // Here we set up our save options to use the specified file name when saving.
            // By default, the file will be saved to the persistent data storage location provided by Unity.
            var saveFileName = "mz_data_models_demo_packs";
            var saveOptions = new ModelSaveOptions(saveFileName);
            // NOTE: If we wanted to use XML, rather than JSON, we could just uncomment the line below.
            // saveOptions.DataFormat = DataFormat.Xml;
            
            // If this is the first time the user has launched the app, we'll load our initial data from a file in the Resources folder.
            if (Core.IsFirstUse)
            {
                // The actual file for this particular demo is named "index.txt" and lives at "Demo/Resources/Packs/index.txt",
                // as noted above. Omit the ".txt" file extension here, when specifying the file name. "Packs", in this case,
                // is just an optional parent folder. If your data file lives at the root of the "Resources" folder, just
                // specify "" or null for the directoryPath argument.
                Model = Mz.Models.Model.LoadResource<ModelGame, GameData>("index", "Packs", saveOptions);
                // For an XML demo, use the line below instead (the only difference is the file name).
                // Model = Mz.Models.Model.LoadResource<ModelGame, GameData>("index_xml", "Packs", saveOptions);
                
                // If the user never invokes any changes to the data, no data file will be saved, 
                // and we won't find it the next time they launch the game. So, we'll run a 
                // manual save here, just in case.
                Model.Save();
            }
            
            // Otherwise, we'll grab the saved data from the persistent data storage location.
            else Model = Mz.Models.Model.Load<ModelGame, GameData>(saveFileName, saveOptions);
            // For this demo, we want to auto-save only when specific changes are made to the data.
            // So, we'll enable auto-saving in general. 
            Model.IsAutoSave = true;
            // We want to notify any listeners each time a change is made of any type, so 
            // we'll trigger "changed" events by default. We can always override this default
            // when we actually modify the data by indicating that we don't want our current changes
            // to trigger a changed event.
            Model.Defaults.IsTriggerChangedEvent = true;
            // We'll only auto-save on certain changes, which we'll specify when the change is made.
            // For this, we'll set Model.Defaults.IsTriggerAutoSave to false, so auto-save
            // events aren't always triggered by default. 
            Model.Defaults.IsTriggerAutoSave = false;
            
            Core.IsFirstUse = false;

            Screens.Home.Packs.Panels.Center.Gallery.LoadPacks(Model.Data.Packs);
            
            // React to changes in the Data Model
            Model.Changed += changedArgs =>
            {
                var indexPack = Model.Data.PackCurrent - 1;
                _pack = Screens.Home.Packs.Panels.Center.Gallery.Packs[indexPack];
                
                // Note that the UI is not directly touched here. We just pass
                // the newly updated data to the LoadData method, and the UI is
                // redrawn accordingly.
                _pack.LoadData(Model.Data.Packs[indexPack]);

                var stringBuilder = new StringBuilder();
                // If we were storing our data in a database, we could use this key
                // as a unique identifier.
                stringBuilder.AppendLine($"Changes to Model: {changedArgs.Model.Key}");
                foreach (var change in changedArgs.Changes)
                {
                    stringBuilder.AppendLine(" ");
                    // PropertyPath returns the precise path to a changed data property as a string.
                    // We could use this to respond to specific property changes in the data hierarchy.
                    var propertyPath = change.PropertyPath;
                    propertyPath = propertyPath.Replace("Mz.DemoDataModels.Models.", "");
                    stringBuilder.AppendLine($"{propertyPath}");
                    // For each property changed, we get both the new current value and the previous value.
                    stringBuilder.AppendLine($"value current: {change.ValueCurrent}, previous: {change.ValuePrevious}");
                }

                _pack.Content.Status.SetTextChanged(stringBuilder.ToString());
            };
            
            // React to auto-save events
            var savedCount = 0;
            Model.Saved += (model, serializeResult) =>
            {
                savedCount++;
                
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(" ");
                stringBuilder.AppendLine($"Model saved {savedCount} times");
                stringBuilder.AppendLine(" ");
                stringBuilder.AppendLine($"{serializeResult.FilePath}");

                if (_pack == null) return;
                _pack.Content.Status.SetTextSaved(stringBuilder.ToString());
            };
        }
        
        // This is called when a level item is clicked
        public void LoadLevel(int pack, int level)
        {
            // This is a custom method in the Data Model definition class
            // See the Demo/Models/ModelGame.cs file for details.
            Model.SetLevel(pack, level);
            
            pack = Model.Data.PackCurrent; // This will be properly constrained by the Model

            var indexPack = pack - 1;
            var indexLevel = level - 1;
            var levelValue = Model.Data.Packs[indexPack].Levels[indexLevel];
            var levelValueNew = levelValue + 1;
            if (levelValueNew > 3) levelValueNew = -1;
            
            // Change the value of the clicked level in the Data Model and trigger an auto-save
            // by passing true as the fourth parameter.
            Model.Set(data => data.Packs[indexPack].Levels[indexLevel], levelValueNew, true, true);
        }
    }
}