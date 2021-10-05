
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
			handle = window;
			
			if( renderer == null)
			{
				renderer = new PreviewRenderUtility();
			}
			if( preParams == null)
			{
				preParams = new BaseParam[]
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
			if( batchPrefabs == null)
			{
				batchPrefabs = new List<GameObject>();
			}
			if( batchPrefabList == null)
			{
				batchPrefabList = new ReorderableList(
					batchPrefabs, typeof( GameObject), true, true, true, true);
				batchPrefabList.onAddCallback += OnBatchPrefabAdd;
				batchPrefabList.onRemoveCallback += OnBatchPrefabRemove;
				batchPrefabList.drawElementCallback += OnBatchPrefabGUI;
				batchPrefabList.elementHeightCallback += OnBatchPrefabHeight;
				batchPrefabList.drawHeaderCallback = (rect) =>
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
								batchPrefabs.Add( gameObject);
							}
						}
					});
					Rect clearRect = rect;
					clearRect.xMin -= 64 - clearRect.width;
					if( GUI.Button( clearRect, "Clear", EditorStyles.toolbarButton) != false)
					{
						batchPrefabs.Clear();
					}
					if( preParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
					{
						if( meshParam.meshType == MeshType.kPrefab)
						{
							if( string.IsNullOrEmpty( batchExportPath) != false)
							{
								batchIndex = -1;
								
								for( i0 = 0; i0 < batchPrefabs.Count; ++i0)
								{
									if( batchPrefabs[ i0] != null)
									{
										batchIndex = i0;
										break;
									}
								}
							}
							EditorGUI.BeginDisabledGroup( batchIndex < 0 || meshParam.Opend == false);
							{
								Rect runRect = rect;
								runRect.xMin -= 128 - runRect.width;
								runRect.xMax -= 64;
								if( GUI.Button( runRect, "Export", EditorStyles.toolbarButton) != false)
								{
									string path = EditorUtility.SaveFolderPanel( "出力先", Application.dataPath, string.Empty);
									if( string.IsNullOrEmpty( path) == false && System.IO.Directory.Exists( path) != false)
									{
										batchExportPath = path;
									}
								}
							}
							EditorGUI.EndDisabledGroup();
						}
					}
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
			if( batchPrefabList != null)
			{
				batchPrefabList = null;
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
		void Batch()
		{
			if( string.IsNullOrEmpty( batchExportPath) != false || refreshCount > 0)
			{
				return;
			}
			if( preParams[ (int)PreParamType.kMesh] is MeshParam meshParam)
			{
				if( batchIndex < batchPrefabs.Count)
				{
					if( meshParam.prefab == batchPrefabs[ batchIndex])
					{
						string assetPath = AssetDatabase.GetAssetPath( meshParam.prefab);
						string assetFileName = System.IO.Path.GetFileName( assetPath);
						string exportPath = System.IO.Path.Combine( batchExportPath, assetFileName).Replace( "\\", "/");
						Export( ExportFormat.kPng, exportPath);
						++batchIndex;
					}
				}
				if( batchIndex < batchPrefabs.Count)
				{
					if( meshParam.requestPrefab == null)
					{
						if( EditorUtility.DisplayCancelableProgressBar( "Batching", 
							$"Exporting... {batchIndex+1}/{batchPrefabs.Count}", 
							(float)batchIndex / (float)batchPrefabs.Count) != false)
						{
							EditorUtility.ClearProgressBar();
							batchExportPath = string.Empty;
						}
						else
						{
							do
							{
								if( batchPrefabs[ batchIndex] != null)
								{
									meshParam.requestPrefab = batchPrefabs[ batchIndex];
									refreshCount = Mathf.Max( 1, refreshCount);
									break;
								}
							}
							while( ++batchIndex < batchPrefabs.Count);
						}
					}
				}
				if( batchIndex >= batchPrefabs.Count)
				{
					EditorUtility.DisplayProgressBar( "Batching", 
						$"Exporting... {batchIndex}/{batchPrefabs.Count}", 
						(float)batchIndex / (float)batchPrefabs.Count);
					EditorUtility.DisplayDialog( "Batch", "Complete", "OK");
					EditorUtility.ClearProgressBar();
					batchExportPath = string.Empty;
				}
			}
		}
		public void Update()
		{
			if( enabled == false)
			{
				return;
			}
			Batch();
			
			if( previewForceUpdate != false || refreshCount > 0)
			{
				if( --refreshCount < 0)
				{
					refreshCount = 0;
				}
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
			handle.Record( label);
		}
		
		[SerializeField]
		bool previewForceUpdate = false;
		[SerializeField]
		FilterMode previewFilterMode = FilterMode.Bilinear;
		
		[SerializeField]
		Vector2 inspectorScrollPosition = Vector2.zero;
		[SerializeField]
		Vector2 batchScrollPosition = Vector2.zero;
		[SerializeReference]
		BaseParam[] preParams = default;
		[SerializeField]
		List<PostProcessParam> postProcessParams = default;
		[SerializeField]
		List<GameObject> batchPrefabs = default;
		
		[System.NonSerialized]
		bool enabled;
		[System.NonSerialized]
		Window handle;
		[System.NonSerialized]
		bool refresh;
		[System.NonSerialized]
		int refreshCount;
		[System.NonSerialized]
		PreviewRenderUtility renderer;
		[System.NonSerialized]
		RenderTexture previewTexture;
		[System.NonSerialized]
		ReorderableList postProcessList;
		[System.NonSerialized]
		ReorderableList batchPrefabList;
		[System.NonSerialized]
		string batchExportPath;
		[System.NonSerialized]
		int batchIndex = -1;
	}
}
