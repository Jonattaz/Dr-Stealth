using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace Mz.Unity
{
    public static class UI
    {
        private static Color _defaultBackgroundColor;
        public static Color DefaultBackgroundColor
        {
            get
            {
                if (_defaultBackgroundColor.a > 0f) return _defaultBackgroundColor;
                var method = typeof(EditorGUIUtility)
                    .GetMethod("GetDefaultBackgroundColor", BindingFlags.NonPublic | BindingFlags.Static);
                if (method != null) _defaultBackgroundColor = (Color)method.Invoke(null, null);
                return _defaultBackgroundColor;
            }
        }
        
        private static GUIStyle _tintableStyle;
        private static GUIStyle TintableStyle {
            get {
                if (_tintableStyle != null) return _tintableStyle;
                _tintableStyle = new GUIStyle();
                _tintableStyle.normal.background = EditorGUIUtility.whiteTexture;
                _tintableStyle.stretchWidth = true;
                return _tintableStyle;
            }
        }
 
        // See: https://answers.unity.com/questions/430669/is-there-a-way-to-get-editor-background-color.html
        public static void DrawTintedRect(Rect rect, Color color) {
            // Only need to perform drawing during repaints!
            if (Event.current.type != EventType.Repaint) return;
            var restoreColor = GUI.color;
            GUI.color = color;
            TintableStyle.Draw(rect, false, false, false, false);
            GUI.color = restoreColor;
        }
 
        public static void DrawEmptyRect(Rect rect) {
            var backgroundColor = GetBackgroundColor();
            DrawTintedRect(rect, backgroundColor);
        }
        
        public static Color GetBackgroundColor()
        {
            var colorBackground32 = GetBackgroundColor32();
            return new Color(
                colorBackground32.r / 255f,
                colorBackground32.g / 255f,
                colorBackground32.b / 255f,
                colorBackground32.a / 255f
            );
        }

        public static Color32 GetBackgroundColor32()
        {
            return EditorGUIUtility.isProSkin
                ? new Color32(56, 56, 56, 255)
                : new Color32(194, 194, 194, 255);
        }
        
        /// <summary>
        /// Sets the icon and title of an editor window.
        /// See: https://code.google.com/archive/p/hounitylibs/source/default/source
        /// </summary>
        /// <param name="editor">Reference to the editor panel whose icon to set</param>
        /// <param name="title">Title</param>
        /// /// <param name="icon">Icon to apply</param>
        public static void SetEditorWindowTitle(EditorWindow editor, string title, Texture icon = null)
        {
            GUIContent titleContent;
            titleContent = _GetEditorWindowContent(editor);
            if (titleContent != null) {
                if (icon != null && titleContent.image != icon) titleContent.image = icon;
                if (title != null && titleContent.text != title) titleContent.text = title;
            }
        }
        
        private static GUIContent _GetEditorWindowContent(EditorWindow editor)
        {
            const BindingFlags bFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            PropertyInfo p = typeof(EditorWindow).GetProperty("cachedTitleContent", bFlags);
            if (p == null) return null;
            return p.GetValue(editor, null) as GUIContent;
        }
        
        public static Color GetTintedColor(Color color)
        {
            var colorBackground = GetBackgroundColor();
            var colorTinted = new Color(color.r * colorBackground.r, color.g * colorBackground.g, color.b * colorBackground.b, color.a);
            return colorTinted;
        }
        
        public static Color GetTintedColor(Color32 color)
        {
            color = new Color(color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
            var colorBackground = GetBackgroundColor();
            var colorTinted = new Color(color.r * colorBackground.r, color.g * colorBackground.g, color.b * colorBackground.b, color.a);
            return colorTinted;
        }
        
        public static Color32 GetTintedColor32(Color color)
        {
            var colorTinted = GetTintedColor(color);
            return ColorToColor32(colorTinted);
        }
        
        public static Color32 GetTintedColor32(Color32 color)
        {
            var colorTinted = GetTintedColor(color);
            return ColorToColor32(colorTinted);
        }

        public static Color32 ColorToColor32(Color color)
        {
            return new Color32(
                (byte)(color.r * 255f), 
                (byte)(color.g * 255f), 
                (byte)(color.b * 255f), 
                (byte)(color.a * 255f)
            );
        }

        public static Color Color32ToColor(Color32 color)
        {
            return new Color(
                color.r / 255f, 
                color.g / 255f, 
                color.b / 255f, 
                color.a / 255f
            );
        }
        
        public static byte[] ColorToByteArray(Color color)
        {
            return new byte[] {
                (byte)(color.r * 255f), 
                (byte)(color.g * 255f), 
                (byte)(color.b * 255f), 
                (byte)(color.a * 255f)
            };
        }
        
        public static byte[] Color32ToByteArray(Color32 color)
        {
            return new byte[] {
                color.r, 
                color.g, 
                color.b, 
                color.a
            };
        }

        public static Color ByteArrayToColor(byte[] bytes)
        {
            return new Color(
                bytes[0] / 255f, 
                bytes[1] / 255f, 
                bytes[2] / 255f, 
                bytes.Length > 3 ? bytes[3] / 255f : 1f
            );
        }
        
        public static Color ByteArrayToColor32(byte[] bytes)
        {
            return new Color32(
                bytes[0], 
                bytes[1], 
                bytes[2], 
                bytes.Length > 3 ? bytes[3] : (byte)255
            );
        }
        
        public static Texture2D MakeTexture(int width, int height, Color color, Texture2D recycleTexture = null)
        {
            var result = recycleTexture != null ? recycleTexture : new Texture2D(width, height);
            
            var pixel = new Color[result.width * result.height];
            for(var i = 0; i < pixel.Length; i++) pixel[i] = color;
            
            result.SetPixels(pixel);
            result.Apply();
 
            return result;
        }
    }
}