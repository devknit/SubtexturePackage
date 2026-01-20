
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
			if( m_Enabled == false)
			{
				return;
			}
			GUILayout.BeginArea( rect);
			{
				EditorGUILayout.BeginHorizontal( EditorStyles.toolbar, GUILayout.ExpandWidth( true));
				{
					GUILayout.FlexibleSpace();
					
					m_PreviewForceUpdate = GUILayout.Toggle( 
						m_PreviewForceUpdate, "Sync", EditorStyles.toolbarButton, GUILayout.Width( 50));
					
					if( GUILayout.Button( "View", EditorStyles.toolbarButton, GUILayout.Width( 50)) != false)
					{
						if( m_PreParams[ (int)PreParamType.kTexture] is TextureParam textureParam)
						if( m_PreParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
						if( m_PreParams[ (int)PreParamType.kTransform] is TransformParam transformParam)
						if( m_PreParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
						{
							var context = new GenericMenu();
							
							context.AddItem( new GUIContent( "Fit"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kFit, textureParam, transformParam, meshParam);
								m_Refresh = true;
							});
							context.AddItem( new GUIContent( "Front"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kFront, textureParam, transformParam, meshParam);
								m_Refresh = true;
							});
							context.AddItem( new GUIContent( "Back"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kBack, textureParam, transformParam, meshParam);
								m_Refresh = true;
							});
							context.AddItem( new GUIContent( "Top"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kTop, textureParam, transformParam, meshParam);
								m_Refresh = true;
							});
							context.AddItem( new GUIContent( "Bottom"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kBottom, textureParam, transformParam, meshParam);
								m_Refresh = true;
							});
							context.AddItem( new GUIContent( "Left"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kLeft, textureParam, transformParam, meshParam);
								m_Refresh = true;
							});
							context.AddItem( new GUIContent( "Right"), false, () =>
							{
								cameraParam.ToPreset( CameraPreset.kRight, textureParam, transformParam, meshParam);
								m_Refresh = true;
							});
							context.ShowAsContext();
						}
					}
					m_PreviewFilterMode = (FilterMode)EditorGUILayout.EnumPopup( 
						m_PreviewFilterMode, EditorStyles.toolbarPopup, GUILayout.Width( 70));
				}
				EditorGUILayout.EndHorizontal();
				
				if( m_RreviewTexture != null && m_RreviewTexture.IsCreated() != false)
				{
					Rect previewRect = new Rect( 
						0, EditorStyles.toolbar.fixedHeight, rect.width, 
						rect.height - EditorStyles.toolbar.fixedHeight);
					float previewAspect = previewRect.width / previewRect.height;
					float textureAspect = (float)m_RreviewTexture.width / (float)m_RreviewTexture.height;
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
					if( m_RreviewTexture.width < m_RreviewTexture.height)
					{
						scale = Mathf.Min( 
							Mathf.Abs( previewRect.width / m_RreviewTexture.width),
							Mathf.Abs( previewRect.height / m_RreviewTexture.height));
						offset = ((float)m_RreviewTexture.height - (float)m_RreviewTexture.width) * scale * 0.5f;
						previewRect.xMin += offset;
						previewRect.xMax -= offset;
					}
					else if( textureAspect > 1.0f)
					{
						scale = Mathf.Min( 
							Mathf.Abs( previewRect.width / m_RreviewTexture.width),
							Mathf.Abs( previewRect.height / m_RreviewTexture.height));
						offset = ((float)m_RreviewTexture.width - (float)m_RreviewTexture.height) * scale * 0.5f;
						previewRect.yMin += offset;
						previewRect.yMax -= offset;
					}
					m_RreviewTexture.filterMode = m_PreviewFilterMode;
					
					var checker = EditorGUIUtility.Load( 
						EditorGUIUtility.isProSkin ? "textureCheckerDark" : "textureChecker");
					if( checker is Texture textureChecker)
					{	
						var texCoords = new Rect( 0, 0, previewRect.width * 0.02f, previewRect.height * 0.02f);
						GUI.DrawTextureWithTexCoords( previewRect, textureChecker, texCoords, false);
					}
					GUI.DrawTexture( previewRect, m_RreviewTexture);
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
									if( m_PreParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
									{
										cameraParam.OrbitControl( m_Renderer.camera, ev.delta);
										m_Refresh = true;
									}
								}
								break;
							}
							case 1:
							{
								if( ev.alt != false)
								{
									if( m_PreParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
									{
										cameraParam.ZoomControl( m_Renderer.camera, (ev.delta.x * -1.0f - ev.delta.y) * 0.5f, m_PreParams);
										m_Refresh = true;
									}
								}
								break;
							}
							case 2:
							{
								if( ev.alt == false)
								{
									if( m_PreParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
									{
										Vector2 size = cameraParam.GetSurfaceArea( rect.width / rect.height);
										Vector2 delta = ev.delta;
										delta.x *= size.x / rect.width;
										delta.y *= size.y / rect.height; 
										cameraParam.MoveControl( m_Renderer.camera, delta);
										m_Refresh = true;
									}
								}
								else
								{
									if( m_PreParams[ (int)PreParamType.kLight] is LightParam lightParam)
									{
										lightParam.OrbitControl( m_Renderer.lights[ 0], ev.delta);
										m_Refresh = true;
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
							if( m_PreParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
							{
								cameraParam.ZoomControl( m_Renderer.camera, ev.delta.y * 0.1f, m_PreParams);
								m_Refresh = true;
							}
						}
					}
					break;
				}
			}
		}
	}
}
