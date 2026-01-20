
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
		kLight,
		kTransform,
		kMesh,
		kMaterial,
		kAnimation,
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
	public sealed partial class Project
	{
		public void OnEnable( Window window)
		{
			m_Handle = window;
			
			if( m_Renderer == null)
			{
				m_Renderer = new PreviewRenderUtility();
			}
			if( m_PreParams == null)
			{
				m_PreParams = new BaseParam[]
				{
					new TextureParam(),
					new CameraParam(),
					new LightParam(),
					new TransformParam(),
					new MeshParam(),
					new MaterialParam(),
					new AnimationParam()
				};
			}
			for( int i0 = 0; i0 < m_PreParams.Length; ++i0)
			{
				m_PreParams[ i0].OnEnable( window);
			}
			if( m_PostProcessParams == null)
			{
				m_PostProcessParams = new List<PostProcessParam>();
			}
			foreach( PostProcessParam param in m_PostProcessParams)
			{
				param.OnEnable( window);
			}
			if( m_PostProcessList == null)
			{
				m_PostProcessList = new ReorderableList(
					m_PostProcessParams,
					typeof( PostProcessParam),
					true, true, true, true);
				m_PostProcessList.onAddCallback += OnPostProcessAdd;
				m_PostProcessList.onRemoveCallback += OnPostProcessRemove;
				m_PostProcessList.drawElementCallback += OnPostProcessGUI;
				m_PostProcessList.elementHeightCallback += OnPostProcessHeight;
				m_PostProcessList.drawHeaderCallback = (rect) =>
				{
					EditorGUI.LabelField( rect, "Post Processing");
				};
			}
			if( m_BatchPrefabs == null)
			{
				m_BatchPrefabs = new List<GameObject>();
			}
			if( m_BatchPrefabList == null)
			{
				m_BatchPrefabList = new ReorderableList(
					m_BatchPrefabs, typeof( GameObject), true, true, true, true);
				m_BatchPrefabList.onAddCallback += OnBatchPrefabAdd;
				m_BatchPrefabList.onRemoveCallback += OnBatchPrefabRemove;
				m_BatchPrefabList.drawElementCallback += OnBatchPrefabGUI;
				m_BatchPrefabList.elementHeightCallback += OnBatchPrefabHeight;
				m_BatchPrefabList.drawHeaderCallback = (rect) =>
				{
					int i0;
					Rect labelRect = rect;
					labelRect.xMax -= 64 + 64;
					
					EditorGUI.LabelField( labelRect, "Batch Prefabs");
					DragAndDropArea( labelRect, (objs) =>
					{
						for( i0 = 0; i0 < objs.Length; ++i0)
						{
							if( objs[ i0] is GameObject gameObject)
							{
								m_BatchPrefabs.Add( gameObject);
							}
						}
					});
					Rect clearRect = rect;
					clearRect.xMin -= 64 - clearRect.width;
					if( GUI.Button( clearRect, "Clear", EditorStyles.toolbarButton) != false)
					{
						m_BatchPrefabs.Clear();
					}
					if( m_PreParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
					{
						if( meshParam.MeshType == MeshType.kPrefab)
						{
							if( string.IsNullOrEmpty( m_BatchExportPath) != false)
							{
								m_BatchIndex = -1;
								
								for( i0 = 0; i0 < m_BatchPrefabs.Count; ++i0)
								{
									if( m_BatchPrefabs[ i0] != null)
									{
										m_BatchIndex = i0;
										break;
									}
								}
							}
							EditorGUI.BeginDisabledGroup( m_BatchIndex < 0 || meshParam.Opend == false);
							{
								Rect runRect = rect;
								runRect.xMin -= 128 - runRect.width;
								runRect.xMax -= 64;
								if( GUI.Button( runRect, "Export", EditorStyles.toolbarButton) != false)
								{
									string path = EditorUtility.SaveFolderPanel( "出力先", Application.dataPath, string.Empty);
									if( string.IsNullOrEmpty( path) == false && System.IO.Directory.Exists( path) != false)
									{
										m_BatchExportPath = path;
									}
								}
							}
							EditorGUI.EndDisabledGroup();
						}
					}
				};
			}
			m_Refresh = true;
			m_Enabled = true;
		}
		public void OnDisable()
		{
			if( m_Renderer != null)
			{
				m_Renderer.Cleanup();
				m_Renderer = null;
			}
			if( m_PostProcessList != null)
			{
				m_PostProcessList = null;
			}
			if( m_BatchPrefabList != null)
			{
				m_BatchPrefabList = null;
			}
			if( m_PostProcessParams != null)
			{
				foreach( PostProcessParam param in m_PostProcessParams)
				{
					param.OnDisable();
				}
			}
			if( m_PreParams != null)
			{
				for( int i0 = 0; i0 < m_PreParams.Length; ++i0)
				{
					m_PreParams[ i0].OnDisable();
				}
			}
			m_Enabled = false;
		}
		void Batch()
		{
			if( string.IsNullOrEmpty( m_BatchExportPath) != false || m_RefreshCount > 0)
			{
				return;
			}
			if( m_PreParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
			{
				if( m_BatchIndex < m_BatchPrefabs.Count)
				{
					if( meshParam.Prefab == m_BatchPrefabs[ m_BatchIndex])
					{
						string assetPath = AssetDatabase.GetAssetPath( meshParam.Prefab);
						string assetFileName = System.IO.Path.GetFileName( assetPath);
						string exportPath = System.IO.Path.Combine( m_BatchExportPath, assetFileName).Replace( "\\", "/");
						Export( ExportFormat.kPng, exportPath);
						++m_BatchIndex;
					}
				}
				if( m_BatchIndex < m_BatchPrefabs.Count)
				{
					if( meshParam.RequestPrefab == null)
					{
						if( EditorUtility.DisplayCancelableProgressBar( "Batching", 
							$"Exporting... {m_BatchIndex+1}/{m_BatchPrefabs.Count}", 
							(float)m_BatchIndex / (float)m_BatchPrefabs.Count) != false)
						{
							EditorUtility.ClearProgressBar();
							m_BatchExportPath = string.Empty;
						}
						else
						{
							do
							{
								if( m_BatchPrefabs[ m_BatchIndex] != null)
								{
									meshParam.RequestPrefab = m_BatchPrefabs[ m_BatchIndex];
									m_RefreshCount = Mathf.Max( 1, m_RefreshCount);
									break;
								}
							}
							while( ++m_BatchIndex < m_BatchPrefabs.Count);
						}
					}
				}
				if( m_BatchIndex >= m_BatchPrefabs.Count)
				{
					EditorUtility.DisplayProgressBar( "Batching", 
						$"Exporting... {m_BatchIndex}/{m_BatchPrefabs.Count}", 
						(float)m_BatchIndex / (float)m_BatchPrefabs.Count);
					EditorUtility.DisplayDialog( "Batch", "Complete", "OK");
					EditorUtility.ClearProgressBar();
					m_BatchExportPath = string.Empty;
				}
			}
		}
		public void Update()
		{
			if( m_Enabled == false)
			{
				return;
			}
			Batch();
			
			if( m_PreviewForceUpdate != false || m_RefreshCount > 0)
			{
				if( --m_RefreshCount < 0)
				{
					m_RefreshCount = 0;
				}
				m_Refresh = true;
				m_Handle.Repaint();
			}
		}
		public void OnToolbarGUI()
		{
			if( m_Enabled == false)
			{
				return;
			}
			EditorGUI.BeginDisabledGroup( m_RreviewTexture == null);
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
		void DragAndDropArea( Rect rect, 
			System.Action<UnityEngine.Object[]> callback, 
			DragAndDropVisualMode visualMode=DragAndDropVisualMode.Generic)
		{
			Event ev = Event.current;
			
			switch( ev.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:
				{
					if( rect.Contains( ev.mousePosition) != false)
					{
						DragAndDrop.visualMode = visualMode;
						
						if( ev.type == EventType.DragPerform)
						{
							DragAndDrop.AcceptDrag();
							
							if( DragAndDrop.objectReferences.Length > 0)
							{
								callback?.Invoke( DragAndDrop.objectReferences);
							}
							DragAndDrop.activeControlID = 0;
						}
						ev.Use();
					}
					break;
				}
			}
		}
		public void Record( string label)
		{
			m_Handle.Record( label);
		}
		
		[SerializeField]
		bool m_PreviewForceUpdate = false;
		[SerializeField]
		FilterMode m_PreviewFilterMode = FilterMode.Bilinear;
		
		[SerializeField]
		Vector2 m_InspectorScrollPosition = Vector2.zero;
		[SerializeField]
		Vector2 m_BatchScrollPosition = Vector2.zero;
		[SerializeReference]
		BaseParam[] m_PreParams = default;
		[SerializeField]
		List<PostProcessParam> m_PostProcessParams = default;
		[SerializeField]
		List<GameObject> m_BatchPrefabs = default;
		
		[System.NonSerialized]
		bool m_Enabled;
		[System.NonSerialized]
		Window m_Handle;
		[System.NonSerialized]
		bool m_Refresh;
		[System.NonSerialized]
		int m_RefreshCount;
		[System.NonSerialized]
		PreviewRenderUtility m_Renderer;
		[System.NonSerialized]
		RenderTexture m_RreviewTexture;
		[System.NonSerialized]
		ReorderableList m_PostProcessList;
		[System.NonSerialized]
		ReorderableList m_BatchPrefabList;
		[System.NonSerialized]
		string m_BatchExportPath;
		[System.NonSerialized]
		int m_BatchIndex = -1;
	}
}
