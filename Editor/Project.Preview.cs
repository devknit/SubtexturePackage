
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Experimental.Rendering;
using System.Collections.Generic;

namespace Subtexture
{
	public sealed partial class Project
	{
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
					
					if( GUILayout.Button( "View", EditorStyles.toolbarButton, GUILayout.Width( 50)) != false)
					{
						if( preParams[ (int)PreParamType.kTexture] is TextureParam textureParam)
						if( preParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
						if( preParams[ (int)PreParamType.kTransform] is TransformParam transformParam)
						if( preParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
						{
							var context = new GenericMenu();
							
							context.AddItem( new GUIContent( "Fit"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kFit, textureParam, transformParam, meshParam);
								refresh = true;
							});
							context.AddItem( new GUIContent( "Front"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kFront, textureParam, transformParam, meshParam);
								refresh = true;
							});
							context.AddItem( new GUIContent( "Back"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kBack, textureParam, transformParam, meshParam);
								refresh = true;
							});
							context.AddItem( new GUIContent( "Top"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kTop, textureParam, transformParam, meshParam);
								refresh = true;
							});
							context.AddItem( new GUIContent( "Bottom"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kBottom, textureParam, transformParam, meshParam);
								refresh = true;
							});
							context.AddItem( new GUIContent( "Left"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kLeft, textureParam, transformParam, meshParam);
								refresh = true;
							});
							context.AddItem( new GUIContent( "Right"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kRight, textureParam, transformParam, meshParam);
								refresh = true;
							});
							context.ShowAsContext();
						}
					}
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
					MouseControl( previewRect);
				}
			}
			GUILayout.EndArea();
		}
		void MouseControl( Rect rect)
		{
			Event ev = Event.current;
			
			switch( ev.type)
			{
				case EventType.MouseDrag:
				{
					if( rect.Contains( ev.mousePosition) != false)
					{
						switch( ev.button)
						{
							case 0:
							{
								if( ev.alt != false)
								{
									if( preParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
									{
										cameraParam.OrbitControl( renderer.camera, ev.delta);
										refresh = true;
									}
								}
								break;
							}
							case 1:
							{
								if( ev.alt != false)
								{
									if( preParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
									{
										cameraParam.ZoomControl( renderer.camera, (ev.delta.x * -1.0f - ev.delta.y) * 0.5f);
										refresh = true;
									}
								}
								break;
							}
							case 2:
							{
								if( ev.alt == false)
								{
									if( preParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
									{
										Vector2 size = cameraParam.GetSurfaceArea( rect.width / rect.height);
										Vector2 delta = ev.delta;
										delta.x *= size.x / rect.width;
										delta.y *= size.y / rect.height; 
										cameraParam.MoveControl( renderer.camera, delta);
										refresh = true;
									}
								}
								else
								{
									if( preParams[ (int)PreParamType.kLight] is LightParam lightParam)
									{
										lightParam.OrbitControl( renderer.lights[ 0], ev.delta);
										refresh = true;
									}
								}
								break;
							}
						}
					}
					break;
				}
				case EventType.ScrollWheel:
				{
					if( rect.Contains( ev.mousePosition) != false)
					{
						if( ev.alt == false)
						{
							if( preParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
							{
								cameraParam.ZoomControl( renderer.camera, ev.delta.y * 0.1f);
								refresh = true;
							}
						}
					}
					break;
				}
			}
		}
	}
}
