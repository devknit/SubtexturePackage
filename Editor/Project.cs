
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Experimental.Rendering;
using System.Collections.Generic;

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
	public enum ExportFormat
	{
		kPng,
		kExr16,
		kExr16ZIP,
		kExr16RLE,
		kExr32,
		kExr32ZIP,
		kExr32RLE,
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
			if( postProcessList == null)
			{
				postProcessList = new ReorderableList(
					postProcessParams,
					typeof( PostProcessParam),
					true, true, true, true);
				postProcessList.onAddCallback += OnPostProcessAdd;
				postProcessList.onRemoveCallback += OnPostProcessRemove;
				postProcessList.drawElementCallback += OnPostProcessGUI;
				postProcessList.elementHeightCallback += OnPostProcessHeight;
				postProcessList.drawHeaderCallback = (rect) =>
		        {
		            EditorGUI.LabelField( rect, "Post Processing");
		        };
			}
			refresh = true;
			enabled = true;
		}
		public void OnDisable()
		{
			if( renderer != null)
			{
				renderer.Cleanup();
				renderer = null;
			}
			if( postProcessList != null)
			{
				postProcessList = null;
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
			enabled = false;
		}
		public void Update()
		{
			if( enabled == false)
			{
				return;
			}
			if( previewForceUpdate != false)
			{
				refresh = true;
				handle.Repaint();
			}
		}
		public void OnToolbarGUI()
		{
			if( enabled == false)
			{
				return;
			}
			EditorGUI.BeginDisabledGroup( previewTexture == null);
			{
				if( GUILayout.Button( "Export", EditorStyles.toolbarButton, GUILayout.Width( 70)) != false)
				{
					var contextMenu = new GenericMenu();
					
					contextMenu.AddItem( new GUIContent( "PNG"), false, () =>
					{
						Export( ExportFormat.kPng);
					});
					contextMenu.AddItem( new GUIContent( "EXR/16bit"), false, () =>
					{
						Export( ExportFormat.kExr16);
					});
					contextMenu.AddItem( new GUIContent( "EXR/16bit ZIP"), false, () =>
					{
						Export( ExportFormat.kExr16ZIP);
					});
					contextMenu.AddItem( new GUIContent( "EXR/16bit RLE"), false, () =>
					{
						Export( ExportFormat.kExr16RLE);
					});
					contextMenu.AddItem( new GUIContent( "EXR/32bit"), false, () =>
					{
						Export( ExportFormat.kExr32);
					});
					contextMenu.AddItem( new GUIContent( "EXR/32bit ZIP"), false, () =>
					{
						Export( ExportFormat.kExr32ZIP);
					});
					contextMenu.AddItem( new GUIContent( "EXR/32bit RLE"), false, () =>
					{
						Export( ExportFormat.kExr32RLE);
					});
					contextMenu.ShowAsContext();
				}
			}
			EditorGUI.EndDisabledGroup();
		}
		public void OnPreviewGUI( Rect rect)
		{
			if( enabled == false)
			{
				return;
			}
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
					Rect previewRect = new Rect( 
						0, EditorStyles.toolbar.fixedHeight, rect.width, 
						rect.height - EditorStyles.toolbar.fixedHeight);
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
			if( enabled == false)
			{
				return;
			}
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
								preParams[ i0].OnGUI( preParams);
							}
							postProcessList.DoLayoutList();
							
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
		void OnPostProcessGUI( Rect rect, int index, bool isActive, bool isFocused)
		{
			postProcessParams[ index].OnElementGUI( rect);
		}
		float OnPostProcessHeight( int index)
		{
			return postProcessParams[ index].GetElementHeight();
		}
		void OnPostProcessAdd( ReorderableList list)
		{
			var postProcessParam = new PostProcessParam();
			postProcessParam.OnEnable( handle);
			postProcessParams.Add( postProcessParam);
		}
		void OnPostProcessRemove( ReorderableList list)
		{
			var postProcessParam = postProcessParams[ list.index];
			postProcessParam.OnDisable();
			postProcessParams.Remove( postProcessParam);
		}
		void Export( ExportFormat exportFormat)
		{
			string extension = string.Empty;
			
			switch( exportFormat)
			{
				case ExportFormat.kExr16:
				case ExportFormat.kExr16ZIP:
				case ExportFormat.kExr16RLE:
				case ExportFormat.kExr32:
				case ExportFormat.kExr32ZIP:
				case ExportFormat.kExr32RLE:
				{
					extension = "exr";
					break;
				}
				default: /* case ExportFormat.kPng: */
				{
					extension = "png";
					break;
				}
			}
			
			string path = EditorUtility.SaveFilePanel( "Subtexture", Application.dataPath, "", extension);
			if( string.IsNullOrEmpty( path) == false)
			{
				Texture2D texture = null;
				
				switch( exportFormat)
				{
					case ExportFormat.kExr16:
					case ExportFormat.kExr16ZIP:
					case ExportFormat.kExr16RLE:
					{
						texture = new Texture2D( previewTexture.width, previewTexture.height, TextureFormat.RGBAHalf, false, false);
						break;
					}
					case ExportFormat.kExr32:
					case ExportFormat.kExr32ZIP:
					case ExportFormat.kExr32RLE:
					{
						texture = new Texture2D( previewTexture.width, previewTexture.height, TextureFormat.RGBAFloat, false, false);
						break;
					}
					default: /* case ExportFormat.kPng: */
					{
						texture = new Texture2D( previewTexture.width, previewTexture.height, TextureFormat.RGBA32, false, false);
						break;
					}
				}
				if( texture != null)
				{
					var current = RenderTexture.active;
					RenderTexture.active = previewTexture;
					texture.ReadPixels( new Rect( 0, 0, previewTexture.width, previewTexture.height), 0, 0);
					texture.Apply();
					RenderTexture.active = current;
					byte[] bytes = null;
					
					if( exportFormat == ExportFormat.kPng)
					{
						bytes = texture.EncodeToPNG();
					}
					else
					{
						Texture2D.EXRFlags exrFlags = Texture2D.EXRFlags.None;
						
						switch( exportFormat)
						{
							case ExportFormat.kExr16ZIP:
							{
								exrFlags |= Texture2D.EXRFlags.CompressZIP;
								break;
							}
							case ExportFormat.kExr16RLE:
							{
								exrFlags |= Texture2D.EXRFlags.CompressRLE;
								break;
							}
							case ExportFormat.kExr32:
							{
								exrFlags |= Texture2D.EXRFlags.OutputAsFloat;
								break;
							}
							case ExportFormat.kExr32ZIP:
							{
								exrFlags |= Texture2D.EXRFlags.OutputAsFloat;
								exrFlags |= Texture2D.EXRFlags.CompressZIP;
								break;
							}
							case ExportFormat.kExr32RLE:
							{
								exrFlags |= Texture2D.EXRFlags.OutputAsFloat;
								exrFlags |= Texture2D.EXRFlags.CompressRLE;
								break;
							}
						}
						bytes = ImageConversion.EncodeToEXR( texture, exrFlags);
					}
					Texture.DestroyImmediate( texture);
					
					if( bytes != null)
					{
						System.IO.File.WriteAllBytes( path, bytes);
					}
				}
			}
		}
		void Refresh()
		{
			if( preParams[ (int)PreParamType.kTexture] is TextureParam textureParam)
			{
				if( preParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
				{
					if( preParams[ (int)PreParamType.kTransform] is TransformParam transformParam)
					{
						if( preParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
						{
							if( meshParam.meshType == MeshType.kPrefab)
							{
								Rendering( textureParam.RenderRect, cameraParam, (renderer) =>
								{
									meshParam.PrefabRender( renderer, transformParam.LocalMatrix);
								});
							}
							else if( preParams[ (int)PreParamType.kMaterial] is MaterialParam materialParam)
							{
								Material renderMaterial = materialParam.RenderMaterial;
								Mesh renderMesh = meshParam.RenderMesh;
								
								if( renderMaterial != null && renderMesh != null)
								{
									Rendering( textureParam.RenderRect, cameraParam, (renderer) =>
									{
										renderer.DrawMesh( renderMesh, transformParam.LocalMatrix, renderMaterial, 0);
									});
								}
							}
						}
					}
				}
			}
			refresh = false;
		}
		void Rendering( Rect renderRect, CameraParam cameraParam, System.Action<PreviewRenderUtility> callback)
		{
			if( renderer == null)
			{
				renderer = new PreviewRenderUtility();
			}
			renderer.BeginPreview( renderRect, GUIStyle.none);
			cameraParam.Apply( renderer.camera);
			callback?.Invoke( renderer);
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
		[SerializeReference]
		BaseParam[] preParams = default;
		[SerializeField]
		List<PostProcessParam> postProcessParams = default;
		
		[System.NonSerialized]
		bool enabled;
		[System.NonSerialized]
		Window handle;
		[System.NonSerialized]
		bool refresh;
		[System.NonSerialized]
		PreviewRenderUtility renderer;
		[System.NonSerialized]
		RenderTexture previewTexture;
		[System.NonSerialized]
		ReorderableList postProcessList;
	}
}
