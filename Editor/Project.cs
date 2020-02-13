
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace Subtexture
{
	public enum PreParamType
	{
		kTexture,
		kCamera,
		kTransform,
		kMesh,
		kMaterial
	}
	[System.Serializable]
	public class Project
	{
		public void OnEnable( Window window)
		{
			handle = window;
			
			if( preParams == null)
			{
				preParams = new BaseParam[]
				{
					new TextureParam(),
					new CameraParam(),
					new TransformParam(),
					new MeshParam(),
					new MaterialParam()
				};
			}
			for( int i0 = 0; i0 < preParams.Length; ++i0)
			{
				preParams[ i0].OnEnable( window);
			}
			if( postProcessParams == null)
			{
				postProcessParams = new List<PostProcessParam>();
			}
			foreach( PostProcessParam param in postProcessParams)
			{
				param.OnEnable( window);
			}
			
			refresh = true;
		}
		public void OnDisable()
		{
			if( renderer != null)
			{
				renderer.Cleanup();
				renderer = null;
			}
			if( postProcessParams != null)
			{
				foreach( PostProcessParam param in postProcessParams)
				{
					param.OnDisable();
				}
			}
			if( preParams != null)
			{
				for( int i0 = 0; i0 < preParams.Length; ++i0)
				{
					preParams[ i0].OnDisable();
				}
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
			EditorGUI.BeginDisabledGroup( previewTexture == null);
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
			
				if( previewTexture != null && previewTexture.IsCreated() != false)
				{
					Rect previewRect = EditorGUILayout.GetControlRect( GUILayout.ExpandWidth( true), GUILayout.ExpandHeight( true));
					float previewAspect = previewRect.width / previewRect.height;
					float textureAspect = (float)previewTexture.width / (float)previewTexture.height;
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
					if( previewTexture.width < previewTexture.height)
					{
						scale = Mathf.Min( 
							Mathf.Abs( previewRect.width / previewTexture.width),
							Mathf.Abs( previewRect.height / previewTexture.height));
						offset = ((float)previewTexture.height - (float)previewTexture.width) * scale * 0.5f;
						previewRect.xMin += offset;
						previewRect.xMax -= offset;
					}
					else if( textureAspect > 1.0f)
					{
						scale = Mathf.Min( 
							Mathf.Abs( previewRect.width / previewTexture.width),
							Mathf.Abs( previewRect.height / previewTexture.height));
						offset = ((float)previewTexture.width - (float)previewTexture.height) * scale * 0.5f;
						previewRect.yMin += offset;
						previewRect.yMax -= offset;
					}
					previewTexture.filterMode = previewFilterMode;
					
					var checker = EditorGUIUtility.Load( 
						EditorGUIUtility.isProSkin ? "textureCheckerDark" : "textureChecker");
					if( checker is Texture textureChecker)
					{	
						var texCoords = new Rect( 0, 0, previewRect.width * 0.02f, previewRect.height * 0.02f);
						GUI.DrawTextureWithTexCoords( previewRect, textureChecker, texCoords, false);
					}
					GUI.DrawTexture( previewRect, previewTexture);
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
							
							for( int i0 = 0; i0 < preParams.Length; ++i0)
							{
								preParams[ i0].OnGUI();
							}
							List<PostProcessParam> removeParams = null;
							
							foreach( PostProcessParam param in postProcessParams)
							{
								param.OnGUI();
								
								if( param.IsClose() != false)
								{
									if( removeParams == null)
									{
										removeParams = new List<PostProcessParam>();
									}
									removeParams.Add( param);
								}
							}
							if( removeParams != null)
							{
								foreach( PostProcessParam param in removeParams)
								{
									postProcessParams.Remove( param);
								}
							}
							EditorGUILayout.BeginHorizontal();
							{
								GUILayout.FlexibleSpace();
								{
									if( GUILayout.Button( "Add Post Processing"))
									{
										var postProcessParam = new PostProcessParam();
										postProcessParam.OnEnable( handle);
										postProcessParams.Add( postProcessParam);
										handle.Repaint();
									}
								}
								GUILayout.FlexibleSpace();
							}
							EditorGUILayout.EndHorizontal();
							
							if( EditorGUI.EndChangeCheck() != false)
							{
								refresh = true;
							}
							if( refresh != false)
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
				var texture = new Texture2D( previewTexture.width, previewTexture.height, TextureFormat.RGBA32, false, false);
				var current = RenderTexture.active;
				RenderTexture.active = previewTexture;
				texture.ReadPixels( new Rect( 0, 0, previewTexture.width, previewTexture.height), 0, 0);
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
			if( preParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
			{
				if( preParams[ (int)PreParamType.kMaterial] is MaterialParam materialParam)
				{
					Material renderMaterial = materialParam.RenderMaterial;
					Mesh renderMesh = meshParam.RenderMesh;
					
					if( renderMaterial != null && renderMesh != null)
					{
						if( preParams[ (int)PreParamType.kTexture] is TextureParam textureParam)
						{
							if( preParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
							{
								if( preParams[ (int)PreParamType.kTransform] is TransformParam transformParam)
								{
									Rect renderRect = textureParam.RenderRect;
									Matrix4x4 localMatrix = transformParam.LocalMatrix;
									
									if( renderer == null)
									{
										renderer = new PreviewRenderUtility();
									}
									renderer.BeginPreview( renderRect, GUIStyle.none);
									cameraParam.Apply( renderer.camera);
									renderer.DrawMesh( renderMesh, localMatrix, renderMaterial, 0);
									renderer.camera.Render();
									if( renderer.EndPreview() is RenderTexture renderTexture)
									{
										RenderTexture currentTexture = RenderTexture.active;
										
										previewTexture = renderTexture;
										
										foreach( PostProcessParam param in postProcessParams)
										{
											previewTexture = param.Blit( previewTexture);
										}
										RenderTexture.active = currentTexture;
									}
								}
							}
						}
					}
				}
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
		BaseParam[] preParams = default;
		[SerializeField]
		List<PostProcessParam> postProcessParams = default;
		
		[System.NonSerialized]
		PreviewRenderUtility renderer;
		[System.NonSerialized]
		bool refresh;
		[System.NonSerialized]
		Window handle;
		
		[System.NonSerialized]
		RenderTexture previewTexture;
	}
}
