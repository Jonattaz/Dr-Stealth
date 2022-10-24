using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(MzInfo))]
[InitializeOnLoad]
public class MzInfoEditor : Editor 
{
	private bool _isInitialized;
	private MzInfo _target;
	private Color _colorHighlight;
	private Color _colorVeryDark;
	private GUIStyle _styleHeading;
	private GUIStyle _styleBody;
	private GUIStyle _styleLink;

	private void _Initialize()
	{
		if (_isInitialized) return;

		_target = (MzInfo) target;

		_colorHighlight = _target.colorHighlight;
		_colorVeryDark = new Color(41.0f / 255.0f, 41.0f / 255.0f, 40.0f / 255.0f, 1.0f);

		_styleBody = new GUIStyle(EditorStyles.label) { richText = true, wordWrap = true, fontSize = 14 };
		_styleHeading = new GUIStyle(_styleBody) { fontSize = 18, fontStyle = FontStyle.Bold} ;

		_styleLink = new GUIStyle(_styleBody)
		{
			wordWrap = false,
			normal = { textColor = _colorHighlight },
			fontStyle = FontStyle.Bold,
			stretchWidth = false
		};

		_isInitialized = true;
	}
	
	protected override void OnHeaderGUI()
	{
		_Initialize();

		var bannerAreaWidth = EditorGUIUtility.currentViewWidth;
		var bannerScaleFactor = Mathf.Min(bannerAreaWidth / (256f + 512f), 1f);
		var bannerAreaLeftWidth = Mathf.Min(512f * bannerScaleFactor, 512f);
		var bannerAreaHeight = Mathf.Min(256f * bannerScaleFactor, 256f);
		var bannerImageRightWidth = 512f * bannerScaleFactor;

		var styleBanner = new GUIStyle(GUI.skin.label) { margin = new RectOffset(0, 0, 0, 0) };
		var textureBackground = Mz.Unity.UI.MakeTexture((int)bannerAreaWidth, (int)bannerAreaHeight, _colorVeryDark);

		EditorGUILayout.BeginHorizontal(styleBanner, GUILayout.Height(bannerAreaHeight));
			GUI.DrawTexture(new Rect(0, 0, bannerAreaWidth, bannerAreaHeight), textureBackground, ScaleMode.ScaleToFit);
			GUI.DrawTexture(new Rect(0, 0, bannerAreaLeftWidth, bannerAreaHeight), _target.bannerLeft, ScaleMode.ScaleToFit);
			GUI.DrawTexture(new Rect(EditorGUIUtility.currentViewWidth - bannerImageRightWidth, 0, bannerImageRightWidth, bannerAreaHeight), _target.bannerRight);
			
			GUILayout.Label(" ", GUILayout.Width(bannerAreaWidth), GUILayout.Height(bannerAreaHeight));
		EditorGUILayout.EndHorizontal();
	}

	public override void OnInspectorGUI()
	{
		_Initialize();

		var bannerAreaWidth = EditorGUIUtility.currentViewWidth;
		var scaleFactor = Mathf.Min(bannerAreaWidth / (256f + 512f), 1f);
		var marginLeft = (int)(36 * scaleFactor);
		
		_styleHeading.margin = new RectOffset(marginLeft, 20, 36, 12);
		_styleBody.margin = new RectOffset(marginLeft, 20, 6, 6);
		_styleLink.margin = new RectOffset(marginLeft, 20, 6, 6);

		if (!string.IsNullOrEmpty(_target.demoScene))
		{
			GUILayout.Label("Demo", _styleHeading);

			var styleButton = new GUIStyle(EditorStyles.miniButton)
			{
				margin = new RectOffset(marginLeft, marginLeft, 0, 0)
			};
			
			if (GUILayout.Button("play demo", styleButton))
			{
				if (!EditorApplication.isPlaying)
				{
					// The editor is not currently in play mode,
					// so the target scene in the editor and play it.
					
					EditorSceneManager.OpenScene(_target.demoScene);
					EditorApplication.isPlaying = true;
				}
				else
				{
					// The editor is currently in play mode.
					
					// Is it playing the correct demo scene?
					// If not, ask the use to stop play and try again.
					// Otherwise, there's nothing else to do.
					var sceneActive = SceneManager.GetActiveScene();
					if (sceneActive.path != _target.demoScene)
					{
						Debug.Log("Another scene is already playing. Please stop play mode and try again.");
					}
				}
			}
		}

		if (!string.IsNullOrEmpty(_target.demoCode))
		{
			if (_LinkLabel(new GUIContent("demo source code"), _styleLink))
			{
				var scriptFile = (MonoScript)AssetDatabase.LoadAssetAtPath(_target.demoCode, typeof(MonoScript));
				if (scriptFile != null)
				{
					AssetDatabase.OpenAsset(scriptFile);
				}
			}
		}

		foreach (var section in _target.sections)
		{
			if (!string.IsNullOrEmpty(section.heading))
			{
				GUILayout.Label(section.heading, _styleHeading);
			}

			if (!string.IsNullOrEmpty(section.text))
			{
				if (section.heading == "Quick Start") _QuickStartSectionTextArea(section.text, marginLeft);
				else GUILayout.Label(section.text, _styleBody);
			}

			if (string.IsNullOrEmpty(section.linkText)) continue;
			if (_LinkLabel(new GUIContent(section.linkText), _styleLink))
			{
				Application.OpenURL(section.url);
			}
		}
		
		GUILayout.Label("\n\n_____\n\nThanks for your support!", _styleBody);
		if (!string.IsNullOrEmpty(_target.assetStoreUrl))
		{
			GUILayout.Label("\nIf you find this asset useful,\nplease don't forget to post a rating.", _styleBody);
			if (_LinkLabel(new GUIContent("rate & review"), _styleLink))
			{
				Application.OpenURL(_target.assetStoreUrl);
			}
		}
		
		GUILayout.Label("\n\nVictor\n\n\n", _styleBody);
	}

	private void _QuickStartSectionTextArea(string text, int marginLeft)
	{
		var styleSetupBlock = new GUIStyle(GUI.skin.label) { padding = new RectOffset(20, 20, 20, 20), margin = new RectOffset(marginLeft, marginLeft, 0, 0)};
		styleSetupBlock.normal.background = Mz.Unity.UI.MakeTexture(1, 1, new Color(255.0f, 255.0f, 255.0f, 0.2f));
		var styleSetupBody = new GUIStyle(EditorStyles.label) { richText = true, wordWrap = true, fontSize = 14 };

		EditorGUILayout.BeginHorizontal(styleSetupBlock);
			GUILayout.Label(text, styleSetupBody);
		EditorGUILayout.EndHorizontal();
	}

	private bool _LinkLabel (GUIContent label, GUIStyle styleLink = null, params GUILayoutOption[] options)
	{
		var position = GUILayoutUtility.GetRect(label, _styleLink, options);

		Handles.BeginGUI ();
		Handles.color = _styleLink.normal.textColor;
		Handles.DrawLine (new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
		Handles.color = Color.white;
		Handles.EndGUI ();

		EditorGUIUtility.AddCursorRect (position, MouseCursor.Link);

		return GUI.Button (position, label, styleLink ?? _styleLink);
	}
}
