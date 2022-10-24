using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Mz.App;

namespace Mz.DemoDataModels.Screens.Home.Packs.Gallery
{
    public class Index : MzBehaviour
    {
        public List<Pack.Index> Packs { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Packs = new List<Pack.Index>();
            
            Rect.anchorMin = new Vector2(0, 0);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 1);

            var layoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = 0;
            layoutGroup.childAlignment = TextAnchor.UpperLeft;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = true;
        }

        public void LoadPacks(IEnumerable<Models.PackData> packs)
        {
            transform.Clear();
         
            foreach (var packData in packs)
            {
                var pack = Add<Pack.Index>($"{packData.Key}");
                pack.LoadData(packData);
                Packs.Add(pack);
            }
        }
    }
}