/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using System.IO;
using UnityEngine;
using Mz.Serializers;

namespace Mz.Unity
{
    public enum FileLocation
    {
        None,
        PersistentData,
        Resources
    }

    public class Files
    {
        public static string FileExtension => "txt";
        
        public TObject Deserialize<TObject>(
            string relativeFolderPath, 
            string fileName,
            FileLocation fileLocation = FileLocation.PersistentData,
            DataFormat dataFormat = DataFormat.Binary
        )
        {
            TObject dataObject;
            var relativeFilePath = Path.Combine(relativeFolderPath ?? "", fileName + "." + FileExtension);

            try
            {
                switch (fileLocation)
                {
                    case FileLocation.Resources:
                        // We can't specify the file extension here, and the actual extension must be ".txt"
                        relativeFilePath = Path.Combine(relativeFolderPath, fileName);
                        
                        try
                        {
                            var text = (TextAsset)Resources.Load(relativeFilePath);
                            var dataString = text.text;
                            Mz.Serializers.Data.DeserializeFromString(out dataObject, dataString, DataFormat.Json, true);
                            return dataObject;
                        } catch (Exception ex) {
                            Debug.Log($"Error retrieving data from resource file: {relativeFilePath}. {ex.Message}");
                        }

                        break;
                    case FileLocation.PersistentData:
                        if (Application.platform == RuntimePlatform.WebGLPlayer)
                        {
                            // Don't use sub-folders, if we're running on WebGL.
                            relativeFolderPath = _PathToName(relativeFolderPath);
                            fileName = relativeFolderPath + fileName;
                        }
                        
                        var outputFolderPath = Path.Combine(Application.persistentDataPath, relativeFolderPath ?? "");

                        try {
                            Mz.Serializers.Data.Deserialize(
                                out dataObject, 
                                fileName, 
                                FileExtension, 
                                outputFolderPath, 
                                false,
                                dataFormat,
                                true
                            );

                            return dataObject;
                        } catch (Exception ex) {
                            Debug.Log($"Error retrieving data from file: {relativeFilePath}. {ex.Message}");
                        }
                        break;
                    default:
                        return default;
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Unable to load file: {relativeFilePath }. {ex.Message}");
                return default;
            }

            return default;
        }

        public SerializeResult Serialize(
            object dataObject, 
            string relativeFolderPath, 
            string fileName,
            DataFormat dataFormat = DataFormat.Binary,
            bool isPrettyPrint = false
        ) {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                // Don't use sub-folders, if we're running on WebGL.
                relativeFolderPath = _PathToName(relativeFolderPath);
                fileName = relativeFolderPath + fileName;
            }
            
            var outputFolderPath = Path.Combine(Application.persistentDataPath, relativeFolderPath ?? "");
            
            try {
                return Mz.Serializers.Data.Serialize(
                    dataObject, 
                    fileName, 
                    FileExtension, 
                    outputFolderPath, 
                    false,
                    dataFormat,
                    isPrettyPrint
                );
            } catch (Exception ex) {
                Debug.Log($"Error saving data to file: {outputFolderPath + fileName + "." + FileExtension}. {ex.Message}");
                return new SerializeResult();
            }
        }

        private static string _PathToName(string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, '_');
        }
    }
}