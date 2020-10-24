
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
		public void Record( string label)
		{
			handle.Record( label);
		}
		
		[SerializeField]
		bool previewForceUpdate = false;
		[SerializeField]
		FilterMode previewFilterMode = FilterMode.Bilinear;
		
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
		int refreshCount;
		[System.NonSerialized]
		PreviewRenderUtility renderer;
		[System.NonSerialized]
		RenderTexture previewTexture;
		[System.NonSerialized]
		ReorderableList postProcessList;
	}
}
