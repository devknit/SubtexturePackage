
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Subtexture
{
	public class Window : MDIEditorWindow
	{
		[MenuItem ("Tools/Subtexture/Open")]	
		static void ShowWindow() 
		{
			CreateNewWindow<Window>().Show();	
		}
		protected override void OnEnable()
		{
			if( textureParam == null)
			{
				textureParam = new TextureParam();
			}
			textureParam.OnEnable( this, true);
			
			if( cameraParam == null)
			{
				cameraParam = new CameraParam();
			}
			cameraParam.OnEnable( this, false);
			
			if( rendererParam == null)
			{
				rendererParam = new RendererParam();
			}
			rendererParam.OnEnable( this, true);
			
			refresh = true;
		}
		protected override void OnDisable()
		{
			if( renderer != null)
			{
				renderer.Cleanup();
				renderer = null;
			}
			if( rendererParam != null)
			{
				rendererParam.OnDisable();
				rendererParam = null;
			}
			if( cameraParam != null)
			{
				cameraParam.OnDisable();
				cameraParam = null;
			}
			if( textureParam != null)
			{
				textureParam.OnDisable();
				textureParam = null;
			}
		}
		void Update()
		{
			if( previewForceUpdate != false)
			{
				refresh = true;
				Repaint();
			}
		}
		[EWSubWindow( "Preview", EWSubWindowIcon.Scene, true, SubWindowStyle.Preview)]
		void OnPreviewGUI( Rect rect)
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
					GUI.DrawTexture( previewRect, image);
				}
			}
			GUILayout.EndArea();
		}
		[EWSubWindow( "Inspector", EWSubWindowIcon.Inspector)]
		void OnInspectorGUI( Rect rect)
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
							rendererParam.OnGUI();
					
							EditorGUI.BeginDisabledGroup( image == null);
							{
								if( GUILayout.Button( "Export") != false)
								{
									Export();
								}
							}
							EditorGUI.EndDisabledGroup();
						
							if( EditorGUI.EndChangeCheck() != false)
							{
								refresh = true;
							}
							if( refresh != false && rendererParam.RenderMesh != null && rendererParam.RenderMaterial != null)
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
				DestroyImmediate( texture);
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
			renderer.DrawMesh( rendererParam.RenderMesh, rendererParam.LocalMatrix, rendererParam.RenderMaterial, 0);
			renderer.camera.Render();
			image = renderer.EndPreview() as RenderTexture;
			refresh = false;
		}
		
		[SerializeField]
		PreviewRenderUtility renderer;
		[SerializeField]
		bool previewForceUpdate = false;
		[SerializeField]
		FilterMode previewFilterMode = FilterMode.Point;
		
		[SerializeField]
		Vector2 scrollPosition;
		[SerializeField]
		TextureParam textureParam;
		[SerializeField]
		CameraParam cameraParam;
		[SerializeField]
		RendererParam rendererParam;
		
		RenderTexture image;
		bool refresh;
	}
}
