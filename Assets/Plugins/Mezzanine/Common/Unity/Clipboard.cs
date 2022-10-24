using UnityEngine;

namespace Mz.Unity
{
    public static class Clipboard
    {
        // See: https://answers.unity.com/questions/1144378/copy-to-clipboard-with-a-button-unity-53-solution.html
        public static void CopyToClipboard(this string value)
        {
            var textEditor = new TextEditor();
            textEditor.text = value;
            textEditor.SelectAll();
            textEditor.Copy();
        }
    }
}