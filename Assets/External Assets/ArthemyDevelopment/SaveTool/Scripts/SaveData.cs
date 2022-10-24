using System;
using System.Collections; 
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Text;

namespace ArthemyDevelopment.Save
{

public class SaveData 
{
    public  Dictionary<string, string> SavedData = new Dictionary<string, string>();

    public static bool SaveFileExist
    {
        get
        {
            if (PlayerPrefs.HasKey("IsSavedFile"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool SuccessLoad = false;


    #region AddValues

        public void SaveValue<T1>(T1 key, object value)
        {
            AddToDictionary(key.ToString(), value.ToString());
        }
        
        public void SaveValue<T1,T2>(T1 key, T2 _class) where T2 : ISaveClass
        {
            string jsonData = JsonUtility.ToJson(_class);
            AddToDictionary(key.ToString(), jsonData);
        }

        void AddToDictionary(string key, string value)
        {
            if (SavedData.ContainsKey(key))
            {
                SavedData[key] = value;
            }
            else
            {
                SavedData.Add(key, value);
            }
        }
        
#endregion

    #region GetValues

        public void LoadValue<T1, T2>(T1 key, out T2 s) where T2 : ISaveClass
        {
            T2 temp = JsonUtility.FromJson<T2>(SavedData[key.ToString()]);
            s = temp;
        }

        public void LoadValue<T>(T key, out string value)
        {
            value =  SavedData[key.ToString()];
        }

        public void LoadValue<T>(T key, out bool value)
        {
            value =  bool.Parse((SavedData[key.ToString()]));
        }
        
        public void LoadValue<T>(T key, out int value)
        {
            value =  int.Parse(SavedData[key.ToString()]);
        }
        
        public void LoadValue<T>(T key, out float value)
        {
            value =  float.Parse(SavedData[key.ToString()].Replace(',', '.'),  CultureInfo.InvariantCulture);
        }
        
        public void LoadValue<T>(T key, out Vector3 value)
        {
            string StoV = SavedData[key.ToString()];
            StoV = StoV.Substring(1, StoV.Length - 2);
            string[] values = StoV.Split(',');
            Vector3 temp = new Vector3(float.Parse(values[0], CultureInfo.InvariantCulture),float.Parse(values[1], CultureInfo.InvariantCulture),float.Parse(values[2], CultureInfo.InvariantCulture));
            value = temp;
        }
        
        public void LoadValue<T>(T key, out Vector2 value)
        {
            string StoV = SavedData[key.ToString()];
            StoV = StoV.Substring(1, StoV.Length - 2);
            string[] values = StoV.Split(',');
            Vector2 temp = new Vector2(float.Parse(values[0], CultureInfo.InvariantCulture),float.Parse(values[1], CultureInfo.InvariantCulture));
            value = temp;
        }

#endregion

    #region Debug
    
        public void PrintValues()
        {
            List<string> keyList = new List<string>(SavedData.Keys);
            List<string> valuesList = new List<string>(SavedData.Values);
            for (int i = 0; i < keyList.Count; i++)
            {
                Debug.Log(keyList[i]);
                Debug.Log(valuesList[i]);
            }
        }
        
    #endregion
    
    #region SaveAndLoad
    
    public void SaveDataFile()
    {
        if (!PlayerPrefs.HasKey("IsSavedFile"))
        {
            PlayerPrefs.SetInt("IsSavedFile", 1);
        }
        
        
        if (SaveDataPreferences.current.EncryptSaveFile)
        {
            string filePath = Application.persistentDataPath +"/"+ SaveDataPreferences.current.FileName();
            SaveFile SF = new SaveFile();
            SF.AddDataToFile(SavedData);
            string jsonData = JsonUtility.ToJson(SF, true);
            byte[] encryptedData = Encryption.EncryptData(jsonData);
            File.WriteAllBytes(filePath, encryptedData);
            Debug.Log(filePath);   
        }
        else
        {
            string filePath = Application.persistentDataPath +"/"+ SaveDataPreferences.current.FileName();
            SaveFile SF = new SaveFile();
            SF.AddDataToFile(SavedData);
            string jsonData = JsonUtility.ToJson(SF, true);
            File.WriteAllText(filePath, jsonData);
            Debug.Log(filePath);
        }
    }
    
    public void SaveDataFile(int index)
    {
        if (!PlayerPrefs.HasKey("IsSavedFile"))
        {
            Debug.Log("PlayerPref");
            PlayerPrefs.SetInt("IsSavedFile", 1);
        }
        if (SaveDataPreferences.current.EncryptSaveFile)
        {
            string filePath = Application.persistentDataPath +"/"+ SaveDataPreferences.current.FileName(index);
            SaveFile SF = new SaveFile();
            SF.AddDataToFile(SavedData);
            string jsonData = JsonUtility.ToJson(SF, true);
            byte[] encryptedJason = Encryption.EncryptData(jsonData);
            File.WriteAllBytes(filePath, encryptedJason);
            Debug.Log("The save file was successfully saved in " + filePath);   
        }
        else
        {
            string filePath = Application.persistentDataPath +"/"+ SaveDataPreferences.current.FileName(index);
            SaveFile SF = new SaveFile();
            SF.AddDataToFile(SavedData);
            string jsonData = JsonUtility.ToJson(SF, true);
            File.WriteAllText(filePath, jsonData);
            Debug.Log("The save file was successfully saved in " + filePath);
        }   
    }

    public void LoadDataFile()
    {
        if (PlayerPrefs.HasKey("IsSavedFile") && File.Exists(Application.persistentDataPath + "/" + SaveDataPreferences.current.FileName()))
        {
            SuccessLoad = true;
            if (SaveDataPreferences.current.EncryptSaveFile && File.Exists(Application.persistentDataPath + "/" + SaveDataPreferences.current.FileName()))
            {
                SaveFile SF = new SaveFile();
                string filePath = Application.persistentDataPath + "/" + SaveDataPreferences.current.FileName();
                byte[] encryptedJson = File.ReadAllBytes(filePath);
                string jsonData = Encryption.DecryptData(encryptedJson);
                SF = JsonUtility.FromJson<SaveFile>(jsonData);
                SF.ApplyData(out SavedData);

            }
            else
            {
                SaveFile SF = new SaveFile();
                string filePath = Application.persistentDataPath + "/" + SaveDataPreferences.current.FileName();
                string jsonData = File.ReadAllText(filePath);
                SF = JsonUtility.FromJson<SaveFile>(jsonData);
                SF.ApplyData(out SavedData);
            }
        }
        else
        {
            SuccessLoad = false;
        }
    }
    
    public void LoadDataFile(int index)
    {
        if (PlayerPrefs.HasKey("IsSavedFile") && File.Exists(Application.persistentDataPath + "/" + SaveDataPreferences.current.FileName(index)))
        {
            SuccessLoad = true;
            if (SaveDataPreferences.current.EncryptSaveFile)
            {
                SaveFile SF = new SaveFile();
                string filePath = Application.persistentDataPath +"/"+ SaveDataPreferences.current.FileName(index);
                byte[] encryptedJson = File.ReadAllBytes(filePath);
                string jsonData = Encryption.DecryptData(encryptedJson);
                SF = JsonUtility.FromJson<SaveFile>(jsonData);
                SF.ApplyData(out SavedData);

            }
            else
            {
                SaveFile SF = new SaveFile();
                string filePath = Application.persistentDataPath +"/"+ SaveDataPreferences.current.FileName(index);
                string jsonData = File.ReadAllText(filePath);
                SF = JsonUtility.FromJson<SaveFile>(jsonData);
                SF.ApplyData(out SavedData);
            }
        }
        else
        {
            SuccessLoad = false;
        }
    }

    

    #endregion
    


    }

    [System.Serializable]
    public class SaveFile
    {
        public List<string> keys;
        public List<string> values;

        public void ApplyData(out Dictionary<string, string> Dic)
        {
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            for (int i = 0; i < keys.Count; i++)
            {
                tempDic.Add(keys[i], values[i]);
            }
            
            Dic = tempDic;

        }

        public void AddDataToFile(Dictionary<string, string> Dic)
        {
            keys = new List<string>(Dic.Keys);
            values = new List<string>(Dic.Values);
        }

    }

    public interface ISaveClass
    {
        
    }



    
}
