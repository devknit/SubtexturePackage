
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Experimental.Rendering;
using System.Collections.Generic;

namespace Subtexture
{
	public sealed partial class Project
	{
		public void OnInspectorGUI( Rect rect)
		{
			if( enabled == false)
			{
				return;
			}
			EditorGUI.BeginDisabledGroup( string.IsNullOrEmpty( batchExportPath) == false);
			GUILayout.BeginArea( rect);
			{
				EditorGUILayout.BeginHorizontal();
				{
					EditorGUILayout.BeginVertical();
					{
						inspectorScrollPosition = EditorGUILayout.BeginScrollView( inspectorScrollPosition);
						{
							EditorGUI.BeginChangeCheck();
							
							for( int i0 = 0; i0 < preParams.Length; ++i0)
							{
								refreshCount = Mathf.Max( refreshCount, preParams[ i0].OnGUI( renderer, preParams));
							}
							postProcessList.DoLayoutList();
							
							if( preParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
							{
								if( meshParam.meshType == MeshType.kPrefab)
								{
									batchPrefabList.DoLayoutList();
								}
							}
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
			EditorGUI.EndDisabledGroup();
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
		void OnBatchPrefabGUI( Rect rect, int index, bool isActive, bool isFocused)
		{
			batchPrefabs[ index] = EditorGUI.ObjectField( rect, batchPrefabs[ index], typeof( GameObject), false) as GameObject;
		}
		float OnBatchPrefabHeight( int index)
		{
			return EditorGUIUtility.singleLineHeight;
		}
		void OnBatchPrefabAdd( ReorderableList list)
		{
			batchPrefabs.Add( null);
		}
		void OnBatchPrefabRemove( ReorderableList list)
		{
			batchPrefabs.RemoveAt( list.index);
		}
		void Refresh()
		{
			if( preParams[ (int)PreParamType.kTexture] is TextureParam textureParam)
			if( preParams[ (int)PreParamType.kCamera] is CameraParam cameraParam)
			if( preParams[ (int)PreParamType.kLight] is LightParam lightParam)
			if( preParams[ (int)PreParamType.kTransform] is TransformParam transformParam)
			if( preParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
			if( meshParam.meshType == MeshType.kPrefab)
			{
				Rendering( textureParam.RenderRect, cameraParam, lightParam, (renderer) =>
				{
					meshParam.Update( renderer, transformParam);
				});
				if( preParams[ (int)PreParamType.kAnimation] is AnimationParam animationParam)
				{
					animationParam.Update();
				}
			}
			else if( preParams[ (int)PreParamType.kMaterial] is MaterialParam materialParam)
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
			refresh = false;
		}
		void Rendering( Rect renderRect, CameraParam cameraParam, LightParam lightParam, System.Action<PreviewRenderUtility> callback)
		{
			renderer.BeginPreview( renderRect, GUIStyle.none);
			cameraParam.Apply( renderer.camera);
			renderer.ambientColor = lightParam.Apply( renderer.lights[ 0]);
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
	}
}
