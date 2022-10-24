using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AnimatedValues;
using UnityEditor;
using System;


namespace ArthemyDevelopment
{


    public class GUIEditorWindow
    {
	    
        public static GUIStyle GuiMessageStyle
		{
			get
			{
				GUIStyle messageStyle = new GUIStyle(GUI.skin.label);
				messageStyle.wordWrap = true;
				return messageStyle;
			}
		}

        public static void BannerLogo(Texture tex, Vector2 size, float position = 0f)
		{
			var rect = GUILayoutUtility.GetRect(0f, 0f);
			rect.width = size.x;
			rect.height = size.y;
			rect.x = position;
			GUILayout.Space(rect.height);
			GUI.DrawTexture(rect, tex);

			var e = Event.current;
			if (e.type != EventType.MouseUp)
			{
				return;
			}
			if (!rect.Contains(e.mousePosition))
			{
				return;
			}
		}

		public static void ExtendableBannerLogo(Texture tex, float height,Vector2 paddings = new Vector2())
		{
			var rect = GUILayoutUtility.GetRect(0f, 0f);

			rect.width = EditorGUIUtility.currentViewWidth - paddings.x - paddings.y;
			rect.x =paddings.x;
			
			rect.height = height;
			GUILayout.Space(rect.height);
			GUI.DrawTexture(rect, tex);

			var e = Event.current;
			if (e.type != EventType.MouseUp)
			{
				return;
			}
			if (!rect.Contains(e.mousePosition))
			{
				return;
			}
		}

		public static void FooterLogo(Vector2 size)
		{
			var rect = GUILayoutUtility.GetRect(0, 0);
			Texture tex;

			if (EditorGUIUtility.isProSkin)
			{
				tex = Resources.Load<Texture>("ArthemyDevelopment/Editor/ImagotipoW");
			}
			else
			{
				tex = Resources.Load<Texture>("ArthemyDevelopment/Editor/ImagotipoB");
			}
			
			rect.width = size.x;
			rect.height = size.y;
			float position = (EditorGUIUtility.currentViewWidth / 2) - (size.x / 2);
			rect.x = position;
			GUI.DrawTexture(rect, tex);
			
			var e = Event.current;
			if (e.type != EventType.MouseUp)
			{
				return;
			}
			if (!rect.Contains(e.mousePosition))
			{
				return;
			}


		}
		public readonly struct FoldoutScope : IDisposable
		{
			private readonly bool wasIndent;

			public FoldoutScope(AnimBool value, out bool shouldDraw, string label, bool indent = true, SerializedProperty toggle = null)
			{
				value.target = Foldout(value.target, label, toggle);
				shouldDraw = value.target;
				if(shouldDraw && indent)
				{
					Indent();
					wasIndent = true;

				}
				else
				{
					wasIndent = false;
				}
			}

			public void Dispose()
			{
				if (wasIndent)
					EndIndent();
				
			}
		}

		public static void HorizontalLine(float height = 1, float width = 1, Vector2 verticalMargin = new Vector2(), Vector2 horizontalMargin = new Vector2(), Color color = new Color())
		{
			GUILayout.Space(verticalMargin.x);

			Rect rect = EditorGUILayout.GetControlRect(false, height);
			if(width>-1)
			{
				float centerX = rect.width / 2;
				rect.width = width - horizontalMargin.y;
				rect.x = horizontalMargin.x;
			}
					
			color.a = .5f;
			EditorGUI.DrawRect(rect, color);

			GUILayout.Space(verticalMargin.y);
		}

		public static bool Foldout(bool value, string lable, SerializedProperty toggle = null)
		{
			bool _value;
			Color defaultColor = GUI.backgroundColor;
			if(EditorGUIUtility.isProSkin)
			{
				GUI.backgroundColor = Color.black;

			}
			else
			{
				GUI.backgroundColor = Color.grey;
			}
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			GUI.backgroundColor = defaultColor;
			EditorGUILayout.Space(3);
			EditorGUILayout.BeginHorizontal();

			GUIStyle customFoldout = new GUIStyle(EditorStyles.foldout);
			

			if(toggle != null && !toggle.boolValue )
			{
				EditorGUI.BeginDisabledGroup(true);
				_value = EditorGUILayout.Toggle(value, EditorStyles.foldout);
				EditorGUI.EndDisabledGroup();

				_value = false;
			}
			else
			{
				_value = EditorGUILayout.Toggle(value, EditorStyles.foldout);
			}

			if(toggle != null)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(toggle, GUIContent.none, GUILayout.Width(30));
				if (EditorGUI.EndChangeCheck() && toggle.boolValue)
					_value = true;
			}

			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space(3);
			EditorGUILayout.EndVertical();

			Rect rect = GUILayoutUtility.GetLastRect();
			
			
			GUIStyle customStyle = new GUIStyle(EditorStyles.largeLabel);
			customStyle.fontSize = 18;
			customStyle.fontStyle = FontStyle.Bold;
			customStyle.alignment = TextAnchor.MiddleCenter;
			if (EditorGUIUtility.isProSkin)
				customStyle.normal.textColor = Color.white;
			else
				customStyle.normal.textColor = Color.black;
			


			if(toggle!= null && !toggle.boolValue)
			{
				EditorGUI.BeginDisabledGroup(true);
			
				EditorGUI.LabelField(rect, lable, customStyle);
				
				EditorGUI.EndDisabledGroup();
			}
			else
			{
				EditorGUI.LabelField(rect, lable, customStyle);				
			}

			return _value;

		}

		public static void Indent()
		{
			EditorGUILayout.BeginHorizontal();
            GUILayout.Space(15);
            EditorGUILayout.BeginVertical();
		}

		public static void EndIndent()
		{
			
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}



	}

}