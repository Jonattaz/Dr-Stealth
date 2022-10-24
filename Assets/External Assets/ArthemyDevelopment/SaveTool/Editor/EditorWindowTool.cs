using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditorInternal;

namespace ArthemyDevelopment.Save
{
    public class EditorWindowTool : EditorWindow
    {

        GUIStyle foldoutStyle = null;
        Vector2 scrollPos;
        EditorWindow scrollWindow;

        bool B_isDecryptFile = true;
        bool B_isEncryptFile = true;

        [SerializeField] bool UseCustomDecryptKey = false;
        [SerializeField] bool UseCustomEncryptKey = false;
        [SerializeField] string S_CustomKey = "";

        AnimBool OtherAnimBool = new AnimBool();
        AnimBool FileAnimBool = new AnimBool();

        SerializedObject serializedObject;

        [MenuItem("Tools/ArthemyDevelopment/SaveTool/SaveEditor")]
        static void Init()
        {
            EditorWindow window = GetWindow(typeof(EditorWindowTool),
                false, "SaveTool", true);
            window.Show();
            window.minSize = new Vector2(400, 500);
        }

        void Awake()
        {
            scrollWindow = GetWindow(typeof(EditorWindowTool));
        }


        void OnGUI()
        {
            if (foldoutStyle == null)
            {
                foldoutStyle = new GUIStyle(EditorStyles.foldout)
                    { fontStyle = FontStyle.Bold };
                if (EditorGUIUtility.isProSkin)
                    foldoutStyle.normal.textColor = Color.white;
                else
                    foldoutStyle.normal.textColor = Color.black;
            }

            if (serializedObject == null)
            {
                serializedObject = new SerializedObject(this);
            }


            scrollPos = EditorGUILayout.BeginScrollView(scrollPos,
                false, false);

            EditorGUILayout.Space(10);
            
            #region Banner

            EditorGUILayout.BeginHorizontal();
			
            GUIEditorWindow.BannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerStart50px"), new Vector2(48,50), 10f);
			
            GUIEditorWindow.ExtendableBannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerExt"),50f,new Vector2(58,58+(EditorGUIUtility.currentViewWidth-116-179)/2));
			
            GUIEditorWindow.BannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerSaveTool"), new Vector2(179,50), 58+(EditorGUIUtility.currentViewWidth-116-179)/2);
			
            GUIEditorWindow.ExtendableBannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerExt"),50f,new Vector2(58+ 179+(EditorGUIUtility.currentViewWidth-116-179)/2,55));
			
            GUIEditorWindow.BannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerEnd"),new Vector2(48,50), EditorGUIUtility.currentViewWidth - 58f);
			
            EditorGUILayout.EndHorizontal();

            #endregion
            
            #region FileManagement

            EditorGUILayout.Space(60);
            using (new GUIEditorWindow.FoldoutScope(FileAnimBool, out var shouldDraw, "File Management"))
            {
                if (shouldDraw)
                {
                    EditorGUILayout.Space(3);
                    
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("If you need to decrypt or encrypt a save file you can do it with the options below, first you need to select the save file and then you will need to select te location to save the new file", GUIEditorWindow.GuiMessageStyle);
                    EditorGUILayout.EndVertical();
                    
                    #region DecryptFile
                    EditorGUILayout.Space(3);
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.Space(3);
                    B_isDecryptFile = EditorGUILayout.Foldout(B_isDecryptFile, "Decrypt save file", true, foldoutStyle);
                    EditorGUILayout.Space(3);

                    if (B_isDecryptFile)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("With the 'Open Save File' button you can open the file you want to decrypt, after selecting the file you will need to select where to save the new file, if a different encryption key that the one registered in the SaveDataPreference object was used, you can use the 'Use Custom Key' option", GUIEditorWindow.GuiMessageStyle);
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.Space(3);
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("Open save file", GUILayout.Width(110), GUILayout.ExpandWidth(false)))
                        {
                            DecryptFile(UseCustomDecryptKey);
                        }
                        EditorGUILayout.Space(3);
                        
                        UseCustomDecryptKey = EditorGUILayout.ToggleLeft("Use Custom Key", UseCustomDecryptKey,GUILayout.Width(115), GUILayout.ExpandWidth(false));
                        
                        if (UseCustomDecryptKey)
                        {
                            GUI.enabled = true;
                        }
                        else
                        {
                            GUI.enabled = false;
                        }

                        SerializedProperty CustomKey = serializedObject.FindProperty("S_CustomKey");
                        EditorGUILayout.PropertyField(CustomKey, GUIContent.none, GUILayout.Width(EditorGUIUtility.currentViewWidth-285));
                        if(S_CustomKey != CustomKey.stringValue)
                            S_CustomKey=CustomKey.stringValue;
                        GUI.enabled = true;

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(10);




                    }
                    EditorGUILayout.EndVertical();
                    
                    #endregion
                    
                    EditorGUILayout.Space(3);
                    
                    #region B_isEncryptFile
                    
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.Space(3);
                    B_isEncryptFile = EditorGUILayout.Foldout(B_isEncryptFile, "Encrypt save file", true, foldoutStyle);
                    EditorGUILayout.Space(3);

                    if (B_isEncryptFile)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.LabelField("With the 'Open Save File' button you can open the file you want to encrypt, after selecting the file you will need to select where to save the new file, if you want to use a different encryption key that the one registered in the SaveDataPreference object, you can use the 'Use Custom Key' option", GUIEditorWindow.GuiMessageStyle);
                        EditorGUILayout.EndVertical();
                        
                        EditorGUILayout.Space(3);
                        
                        EditorGUILayout.BeginHorizontal();
                        
                        if (GUILayout.Button("Open save file", GUILayout.Width(110), GUILayout.ExpandWidth(false)))
                        {
                            EncryptFile(UseCustomEncryptKey);
                        }
                        EditorGUILayout.Space(2);
                        
                        UseCustomEncryptKey = EditorGUILayout.ToggleLeft("Use Custom Key", UseCustomEncryptKey, GUILayout.Width(115), GUILayout.ExpandWidth(false));
                        
                        if (UseCustomEncryptKey)
                        {
                            GUI.enabled = true;
                        }
                        else
                        {
                            GUI.enabled = false;
                        }

                        SerializedProperty CustomKey = serializedObject.FindProperty("S_CustomKey");
                        EditorGUILayout.PropertyField(CustomKey, GUIContent.none, GUILayout.Width(EditorGUIUtility.currentViewWidth-285));
                        if(S_CustomKey != CustomKey.stringValue)
                            S_CustomKey=CustomKey.stringValue;
                        GUI.enabled = true;

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(10);




                    }
                    EditorGUILayout.EndVertical();
                    
                    #endregion
                }

            }
            #endregion
            
            EditorGUILayout.Space(5);
            #region OtherOptions

            using (new GUIEditorWindow.FoldoutScope(OtherAnimBool, out var shouldDraw, "Other Options"))
            {
                if (shouldDraw)
                {
                    EditorGUILayout.Space(3);
                    
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("If you need to create a new SaveDataPreferences object, you can do it with the button below, if there is already an existing object, nothing will happen", GUIEditorWindow.GuiMessageStyle);
                    EditorGUILayout.EndVertical();
                    
                    EditorGUILayout.Space(3);
                    if(GUILayout.Button("Create Save Data Preferences"))
                    {
                        var asset = Resources.Load<SaveDataPreferences>("ArthemyDevelopment/SaveTool/SaveDataPreferences");

                        if (!asset)
                        {
                            asset = ScriptableObject.CreateInstance<SaveDataPreferences>();
                            string path = "Assets/Resources/ArthemyDevelopment/SaveTool/SaveDataPreferences.asset";
                            Directory.CreateDirectory(Path.GetDirectoryName(path));
                            AssetDatabase.CreateAsset(asset, path);
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }
            
            #endregion
            
            
            EditorGUILayout.Space(50);
            GUILayout.FlexibleSpace();
            GUIEditorWindow.FooterLogo(new Vector2(107,143));
            EditorGUILayout.Space(150);
            EditorGUILayout.EndScrollView();
            
        }
        #region Functions

        void EncryptFile(bool isCustKey)
        {
            string filepath = EditorUtility.OpenFilePanel("Select save file to open", Application.dataPath, SaveDataPreferences.current.fileFormat);

            if (!string.IsNullOrEmpty(filepath))
            {
                string encryptedFile= File.ReadAllText(filepath);
                byte[] jsonData;

                if(isCustKey)
                {
                    jsonData = Encryption.EncryptData(encryptedFile,S_CustomKey);
                }
                else
                {
                    jsonData = Encryption.EncryptData(encryptedFile);
                }
            
                string saveFile = EditorUtility.SaveFilePanel("Save the new file", Application.dataPath, "Encrypted Save File",SaveDataPreferences.current.fileFormat);

                if (!string.IsNullOrEmpty(saveFile))
                {
                    File.WriteAllBytes(saveFile, jsonData);
                }
                
            }
        }
        

        void DecryptFile(bool isCustKey)
        {
            string filepath = EditorUtility.OpenFilePanel("Select save file to open", Application.dataPath, SaveDataPreferences.current.fileFormat);

            if (!string.IsNullOrEmpty(filepath))
            {
                byte[] encryptedFile;
                string jsonData;
                encryptedFile = File.ReadAllBytes(filepath);

                if(isCustKey)
                {
                    jsonData = Encryption.DecryptData(encryptedFile,S_CustomKey);
                }
                else
                {
                    jsonData = Encryption.DecryptData(encryptedFile);
                }
            
                string saveFile = EditorUtility.SaveFilePanel("Save the new file", Application.dataPath, "Decrypted Save File",SaveDataPreferences.current.fileFormat);

                if (!string.IsNullOrEmpty(saveFile))
                {
                    File.WriteAllText(saveFile, jsonData);
                }
                
            }

        }
        
        
        #endregion
    }
}