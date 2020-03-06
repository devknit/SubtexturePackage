
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	public enum PostProcessType
	{
		kNone,
		kOneMinus,
		kSwizzle,
	}
	[System.Serializable]
	public sealed class PostProcessParam : BaseParam
	{
		public PostProcessParam() : base( true)
		{
		}
		public override void OnEnable( Window window)
		{
			base.OnEnable( window);
			
			if( processes == null)
			{
				processes = new PostProcessBase[]
				{
					new NoneProcess(),
					new OneMinusProcess(),
					new SwizzleProcess()
				};
			}
			for( int i0 = 0; i0 < processes.Length; ++i0)
			{
				processes[ i0]?.OnEnable( window);
			}
			ChangeBlitMaterial( postProcessType);
		}
		public override void OnDisable()
		{
			if( renderTexture != null)
			{
				RenderTexture.DestroyImmediate( renderTexture);
				renderTexture = null;
			}
			if( processProperties != null)
			{
				processProperties.Dispose();
				processProperties = null;
			}
			base.OnDisable();
		}
		public void OnElementGUI( Rect rect)
		{
			Rect typeRect = new Rect( rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
			var postProcessTypeValue = (PostProcessType)EditorGUI.EnumPopup( typeRect, postProcessType);
			if( postProcessType.Equals( postProcessTypeValue) == false)
			{
				Record( "Change PostProcess Type");
				ChangeBlitMaterial( postProcessTypeValue);
				postProcessType = postProcessTypeValue;
			}
			if( processProperties != null)
			{
				if( processProperties.OnGUI( new Rect( 
					rect.x, rect.y + typeRect.height, 
					rect.width, rect.height - typeRect.height)) != false)
				{
					processProperties.OnUpdateMaterial();
				}
			}
		}
		public float GetElementHeight()
		{
			return EditorGUIUtility.singleLineHeight + (processProperties?.GetHeight() ?? 0);
		}
		public RenderTexture Blit( RenderTexture source)
		{
			if( blitMaterial != null)
			{
				if( renderTexture != null)
				{
					if( renderTexture.width != source.width || renderTexture.height != source.height)
					{
						RenderTexture.DestroyImmediate( renderTexture);
						renderTexture = null;
					}
				}
				if( renderTexture == null)
				{
					renderTexture = new RenderTexture( source.width, source.height, 32, RenderTextureFormat.ARGBFloat);
					renderTexture.hideFlags = HideFlags.HideAndDontSave;
				}
				Graphics.Blit( source, renderTexture, blitMaterial);
				
				return renderTexture;
			}
			return source;
		}
		void ChangeBlitMaterial( PostProcessType type)
		{
			if( blitMaterial != null)
			{
				blitMaterial = null;
			}
			if( processProperties != null)
			{
				processProperties.Dispose();
				processProperties = null;
			}
			processProperties = processes[ (int)type];
			
			if( processProperties != null)
			{
				blitMaterial = processProperties.Create();
				processProperties.OnUpdateMaterial();
			}
		}
		
		[SerializeField]
		PostProcessType postProcessType = PostProcessType.kNone;
		[SerializeReference]
		PostProcessBase[] processes;
		
		[System.NonSerialized]
		RenderTexture renderTexture;
		[System.NonSerialized]
		PostProcessBase processProperties;
		[System.NonSerialized]
		Material blitMaterial;
		
	}
}