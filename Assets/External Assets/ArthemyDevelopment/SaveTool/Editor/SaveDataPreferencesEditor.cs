using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArthemyDevelopment.Save
{


    [CustomEditor(typeof(SaveDataPreferences))]
    public class SaveDataPreferencesEditor : Editor
    {
        private SaveDataPreferences data;
        private bool B_isEnctyption;

        public SerializedProperty
            FileName_Prop,
            FileFormat_Prop,
            Encrypt_Prop,
            EncryptKey_Prop;

        void OnEnable()
        {
            data = (SaveDataPreferences) target;
            Encrypt_Prop = serializedObject.FindProperty("EncryptSaveFile");
            FileName_Prop = serializedObject.FindProperty("fileName");
            FileFormat_Prop = serializedObject.FindProperty("fileFormat");
            EncryptKey_Prop = serializedObject.FindProperty("EncryptionKey");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            B_isEnctyption = data.EncryptSaveFile;
            
            EditorGUILayout.Space(10);
            

            #region Banner

            EditorGUILayout.BeginHorizontal();
			
            GUIEditorWindow.BannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerStart33px"), new Vector2(32,33), 5f);
			
            GUIEditorWindow.ExtendableBannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerExt"),33f,new Vector2(37,37+(EditorGUIUtility.currentViewWidth-74-186)/2));
			
            GUIEditorWindow.BannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerSaveDataPreference"), new Vector2(186,33), 38+(EditorGUIUtility.currentViewWidth-74-186)/2);
			
            GUIEditorWindow.ExtendableBannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerExt"),33f,new Vector2(37+ 185+(EditorGUIUtility.currentViewWidth-74-186)/2,32));
			
            GUIEditorWindow.BannerLogo(Resources.Load<Texture>("ArthemyDevelopment/Editor/BannerEnd"),new Vector2(32,33), EditorGUIUtility.currentViewWidth - 37f);
			
            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.Space(38);


            EditorGUILayout.PropertyField(FileName_Prop);
            EditorGUILayout.PropertyField(FileFormat_Prop);
            EditorGUILayout.PropertyField(Encrypt_Prop);
            GUI.enabled = false;
            
            if (B_isEnctyption)
                GUI.enabled = true;
            
            EditorGUILayout.PropertyField(EncryptKey_Prop);
            GUI.enabled = true;
            
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Clear Player Pref"))
            {
                if (PlayerPrefs.HasKey("IsSavedFile"))
                {
                    PlayerPrefs.DeleteKey("IsSavedFile");
                }
            }
                

            

            serializedObject.ApplyModifiedProperties();
            
            
            
            
        }

    }

}