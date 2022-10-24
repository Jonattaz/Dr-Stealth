using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace ArthemyDevelopment.Save
{


    [System.Serializable]
    public class SaveDataPreferences : ScriptableObject
    {
        [SerializeField] string fileName = "fileName";
        [SerializeField] public string fileFormat = "savefile";
        [SerializeField] public bool EncryptSaveFile;
        [SerializeField] public string EncryptionKey = "encryptionkey";

        static SaveDataPreferences _current = null;

        public static SaveDataPreferences current
        {
            get
            {
                if (!_current)
                {
                    SaveDataPreferences asset = Resources.Load<SaveDataPreferences>("ArthemyDevelopment/SaveTool/SaveDataPreferences");
                    if (!asset)
                    {
                        asset = ScriptableObject.CreateInstance<SaveDataPreferences>();
                        string path = "Assets/Resources/ArthemyDevelopment/SaveTool/SaveDataPreferences.asset";
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        AssetDatabase.CreateAsset(asset, path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    _current = asset;
                }

                

                return _current;
            }


        }


        public string FileName()
        {
            var temp = fileName + "." + fileFormat;
            return temp;
        }
        
        public string FileName(int i)
        {
            var temp = fileName + i + "." + fileFormat;
            return temp;
        }
    }

}
