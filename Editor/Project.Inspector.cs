
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Subtexture
{
	public sealed partial class Project
	{
		public void OnInspectorGUI( Rect rect)
		{
			if( m_Enabled == false)
			{
				return;
			}
			EditorGUI.BeginDisabledGroup( string.IsNullOrEmpty( m_BatchExportPath) == false);
			GUILayout.BeginArea( rect);
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.BeginVertical();
					{
						m_InspectorScrollPosition = EditorGUILayout.BeginScrollView( m_InspectorScrollPosition);
						{
							EditorGUI.BeginChangeCheck();
							
							for( int i0 = 0; i0 < m_PreParams.Length; ++i0)
							{
								m_RefreshCount = Mathf.Max( m_RefreshCount, m_PreParams[ i0].OnGUI( m_Renderer, m_PreParams));
							}
							m_PostProcessList.DoLayoutList();
							
							if( m_PreParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
							{
								if( meshParam.MeshType == MeshType.kPrefab)
								{
									m_BatchPrefabList.DoLayoutList();
								}
							}
							if( EditorGUI.EndChangeCheck() != false)
							{
								m_Refresh = true;
							}
							if( m_Refresh != false)
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
			EditorGUI.EndDisabledGroup();
			GUILayout.EndArea();
		}
		void OnPostProcessGUI( Rect rect, int index, bool isActive, bool isFocused)
		{
			m_PostProcessParams[ index].OnElementGUI( rect);
		}
		float OnPostProcessHeight( int index)
		{
			return m_PostProcessParams[ index].GetElementHeight();
		}
		void OnPostProcessAdd( ReorderableList list)
		{
			var postProcessParam = new PostProcessParam();
			postProcessParam.OnEnable( m_Handle);
			m_PostProcessParams.Add( postProcessParam);
		}
		void OnPostProcessRemove( ReorderableList list)
		{
			var postProcessParam = m_PostProcessParams[ list.index];
			postProcessParam.OnDisable();
			m_PostProcessParams.Remove( postProcessParam);
		}
		void OnBatchPrefabGUI( Rect rect, int index, bool isActive, bool isFocused)
		{
			m_BatchPrefabs[ index] = EditorGUI.ObjectField( rect, m_BatchPrefabs[ index], typeof( GameObject), false) as GameObject;
		}
		float OnBatchPrefabHeight( int index)
		{
			return EditorGUIUtility.singleLineHeight;
		}
		void OnBatchPrefabAdd( ReorderableList list)
		{
			m_BatchPrefabs.Add( null);
		}
		void OnBatchPrefabRemove( ReorderableList list)
		{
			m_BatchPrefabs.RemoveAt( list.index);
		}
		void Refresh()
		{
			if( m_PreParams[ (int)PreParamType.kTexture] is TextureParam textureParam)
			if( m_PreParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
			if( m_PreParams[ (int)PreParamType.kLight] is LightParam lightParam)
			if( m_PreParams[ (int)PreParamType.kTransform] is TransformParam transformParam)
			if( m_PreParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
			if( meshParam.MeshType == MeshType.kPrefab)
			{
				Rendering( textureParam.RenderRect, cameraParam, lightParam, (renderer) =>
				{
					meshParam.Update( renderer, transformParam);
				});
				if( m_PreParams[ (int)PreParamType.kAnimation] is AnimationParam animationParam)
				{
					animationParam.Update();
				}
			}
			else if( m_PreParams[ (int)PreParamType.kMaterial] is MaterialParam materialParam)
			{
				Material renderMaterial = materialParam.RenderMaterial;
				Mesh renderMesh = meshParam.RenderMesh;
				
				if( renderMaterial != null && renderMesh != null)
				{
					Rendering( textureParam.RenderRect, cameraParam, lightParam, (renderer) =>
					{
						renderer.DrawMesh( renderMesh, transformParam.LocalMatrix, renderMaterial, 0);
					});
				}
			}
			m_Refresh = false;
		}
		void Rendering( Rect renderRect, CameraParam cameraParam, LightParam lightParam, System.Action<PreviewRenderUtility> callback)
		{
			m_Renderer.BeginPreview( renderRect, GUIStyle.none);
			cameraParam.Apply( m_Renderer.camera);
			m_Renderer.ambientColor = lightParam.Apply( m_Renderer.lights[ 0]);
			callback?.Invoke( m_Renderer);
			m_Renderer.camera.Render();
			if( m_Renderer.EndPreview() is RenderTexture renderTexture)
			{
				RenderTexture currentTexture = RenderTexture.active;
				
				m_RreviewTexture = renderTexture;
				
				foreach( PostProcessParam param in m_PostProcessParams)
				{
					m_RreviewTexture = param.Blit( m_RreviewTexture);
				}
				RenderTexture.active = currentTexture;
			}
		}
	}
}
