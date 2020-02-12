
using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.Rendering;

namespace Subtexture
{
	[System.Serializable]
	public class Project
	{
		public void OnEnable( Window window)
		{
			handle = window;
			
			if( textureParam == null)
			{
				textureParam = new TextureParam();
			}
			textureParam.OnEnable( window, true);
			
			if( cameraParam == null)
			{
				cameraParam = new CameraParam();
			}
			cameraParam.OnEnable( window, false);
			
			if( transformParam == null)
			{
				transformParam = new TransformParam();
			}
			transformParam.OnEnable( window, true);
			
			if( meshParam == null)
			{
				meshParam = new MeshParam();
			}
			meshParam.OnEnable( window, false);
			
			if( materialParam == null)
			{
				materialParam = new MaterialParam();
			}
			materialParam.OnEnable( window, true);
			
			refresh = true;
		}
		public void OnDisable()
		{
			if( renderer != null)
			{
				renderer.Cleanup();
				renderer = null;
			}
			if( image != null)
			{
				RenderTexture.DestroyImmediate( image);
				image = null;
			}
			if( material != null)
			{
				Material.DestroyImmediate( material);
				material = null;
			}
			if( materialParam != null)
			{
				materialParam.OnDisable();
			}
			if( meshParam != null)
			{
				meshParam.OnDisable();
			}
			if( transformParam != null)
			{
				transformParam.OnDisable();
			}
			if( cameraParam != null)
			{
				cameraParam.OnDisable();
			}
			if( textureParam != null)
			{
				textureParam.OnDisable();
			}
		}
		public void Update()
		{
			if( previewForceUpdate != false)
			{
				refresh = true;
				handle.Repaint();
			}
		}
		public void OnToolbarGUI()
		{
			EditorGUI.BeginDisabledGroup( image == null);
			{
				if( GUILayout.Button( "Export", EditorStyles.toolbarButton, GUILayout.Width( 70)) != false)
				{
					Export();
				}
			}
			EditorGUI.EndDisabledGroup();
		}
		public void OnPreviewGUI( Rect rect)
		{
			GUILayout.BeginArea( rect);
			{
				EditorGUILayout.BeginHorizontal( EditorStyles.toolbar, GUILayout.ExpandWidth( true));
				{
					GUILayout.FlexibleSpace();
					
					previewForceUpdate = GUILayout.Toggle( 
						previewForceUpdate, "Sync", EditorStyles.toolbarButton, GUILayout.Width( 50));
					
					previewFilterMode = (FilterMode)EditorGUILayout.EnumPopup( 
						previewFilterMode, EditorStyles.toolbarPopup, GUILayout.Width( 70));
				}
				EditorGUILayout.EndHorizontal();
			
				if( image != null && image.IsCreated() != false)
				{
					Rect previewRect = EditorGUILayout.GetControlRect( GUILayout.ExpandWidth( true), GUILayout.ExpandHeight( true));
					float previewAspect = previewRect.width / previewRect.height;
					float imageAspect = (float)image.width / (float)image.height;
					float offset, scale;
					
					if( previewAspect < 1.0f)
					{
						offset = (1.0f - previewAspect) * previewRect.height * 0.5f;
						previewRect.yMin += offset;
						previewRect.yMax -= offset;
					}
					else if( previewAspect > 1.0f)
					{
						offset = (previewAspect - 1.0f) * previewRect.height * 0.5f;
						previewRect.xMin += offset;
						previewRect.xMax -= offset;
					}
					if( image.width < image.height)
					{
						scale = Mathf.Min( 
							Mathf.Abs( previewRect.width / image.width),
							Mathf.Abs( previewRect.height / image.height));
						offset = ((float)image.height - (float)image.width) * scale * 0.5f;
						previewRect.xMin += offset;
						previewRect.xMax -= offset;
					}
					else if( imageAspect > 1.0f)
					{
						scale = Mathf.Min( 
							Mathf.Abs( previewRect.width / image.width),
							Mathf.Abs( previewRect.height / image.height));
						offset = ((float)image.width - (float)image.height) * scale * 0.5f;
						previewRect.yMin += offset;
						previewRect.yMax -= offset;
					}
					image.filterMode = previewFilterMode;
					
					var checker = EditorGUIUtility.Load( 
						EditorGUIUtility.isProSkin ? "textureCheckerDark" : "textureChecker");
					if( checker is Texture textureChecker)
					{	
						var texCoords = new Rect( 0, 0, previewRect.width * 0.02f, previewRect.height * 0.02f);
						GUI.DrawTextureWithTexCoords( previewRect, textureChecker, texCoords, false);
					}
					GUI.DrawTexture( previewRect, image);
				}
			}
			GUILayout.EndArea();
		}
		public void OnInspectorGUI( Rect rect)
		{
			GUILayout.BeginArea( rect);
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.BeginVertical();
					{
						scrollPosition = EditorGUILayout.BeginScrollView( scrollPosition);
						{
							EditorGUI.BeginChangeCheck();
							
							textureParam.OnGUI();
							cameraParam.OnGUI();
							transformParam.OnGUI();
							meshParam.OnGUI();
							materialParam.OnGUI();
					
							if( EditorGUI.EndChangeCheck() != false)
							{
								refresh = true;
							}
							if( refresh != false && meshParam.RenderMesh != null && materialParam.RenderMaterial != null)
							{
								Refresh();
							}
						}
						EditorGUILayout.EndScrollView();
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
		void Export()
		{
			string path = EditorUtility.SaveFilePanel( "Subtexture", Application.dataPath, "", "png");
			if( string.IsNullOrEmpty( path) == false)
			{
				var texture = new Texture2D( image.width, image.height, TextureFormat.RGBA32, false, false);
				var current = RenderTexture.active;
				RenderTexture.active = image;
				texture.ReadPixels( new Rect( 0, 0, image.width, image.height), 0, 0);
				texture.Apply();
				RenderTexture.active = current;
				byte[] bytes = texture.EncodeToPNG();
				Texture.DestroyImmediate( texture);
				Debug.Log(path);
				System.IO.File.WriteAllBytes( path, bytes);
			}
		}
		void Refresh()
		{
			if( renderer == null)
			{
				renderer = new PreviewRenderUtility();
			}
			renderer.BeginPreview( textureParam.RenderRect, GUIStyle.none);
			cameraParam.Apply( renderer.camera);
			renderer.DrawMesh( meshParam.RenderMesh, transformParam.LocalMatrix, materialParam.RenderMaterial, 0);
			renderer.camera.Render();
			if( renderer.EndPreview() is RenderTexture renderTexture)
			{
				RenderTexture currentTexture = RenderTexture.active;
				
				/* postprocess param */
				if( image != null)
				{
					if( image.width != renderTexture.width || image.height != renderTexture.height)
					{
						RenderTexture.DestroyImmediate( image);
						image = null;
					}
				}
				if( image == null)
				{
					GraphicsFormat format = renderer.camera.allowHDR ? GraphicsFormat.R16G16B16A16_SFloat : GraphicsFormat.R8G8B8A8_UNorm;
					image = new RenderTexture( renderTexture.width, renderTexture.height, 16, format);
					image.hideFlags = HideFlags.HideAndDontSave;
				}
				if( material == null)
				{
					string newShaderPath = AssetDatabase.GUIDToAssetPath( "2b154a50c25ca3a4c9c4512a996093b1");
					
					if( string.IsNullOrEmpty( newShaderPath) == false)
					{
						if( AssetDatabase.LoadAssetAtPath<Shader>( newShaderPath) is Shader shader)
						{
							material = new Material( shader);
						}
					}
				}
				Graphics.Blit( renderTexture, image, material);
				RenderTexture.active = currentTexture;
			}
			refresh = false;
		}
		public void Record( string label)
		{
			handle.Record( label);
		}
		
		[SerializeField]
		bool previewForceUpdate = false;
		[SerializeField]
		FilterMode previewFilterMode = FilterMode.Point;
		
		[SerializeField]
		Vector2 scrollPosition = Vector2.zero;
		[SerializeField]
		TextureParam textureParam = default;
		[SerializeField]
		CameraParam cameraParam = default;
		[SerializeField]
		TransformParam transformParam = default;
		[SerializeField]
		MeshParam meshParam = default;
		[SerializeField]
		MaterialParam materialParam = default;
		
		[System.NonSerialized]
		PreviewRenderUtility renderer;
		[System.NonSerialized]
		bool refresh;
		[System.NonSerialized]
		Window handle;
		
		[System.NonSerialized]
		RenderTexture image;
		[System.NonSerialized]
		Material material;
	}
}
