
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
			ChangeBlitMaterial( postProcessType);
		}
		public override void OnDisable()
		{
			if( renderTexture != null)
			{
				RenderTexture.DestroyImmediate( renderTexture);
				renderTexture = null;
			}
			if( blitMaterial != null)
			{
				Material.DestroyImmediate( blitMaterial);
				blitMaterial = null;
			}
			base.OnDisable();
		}
		public void OnElementGUI( Rect rect)
		{
			Rect typeRect = new Rect( rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
			var postProcessTypeValue = (PostProcessType)EditorGUI.EnumPopup( typeRect, "Type", postProcessType);
			if( postProcessType.Equals( postProcessTypeValue) == false)
			{
				Record( "Change PostProcess Type");
				postProcessType = postProcessTypeValue;
				ChangeBlitMaterial( postProcessType);
			}
		}
		public float GetElementHeight()
		{
			return EditorGUIUtility.singleLineHeight;
		}
		public RenderTexture Blit( RenderTexture source)
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
				renderTexture = new RenderTexture( source.width, source.height, source.depth, source.format);
				renderTexture.hideFlags = HideFlags.HideAndDontSave;
			}
			Graphics.Blit( source, renderTexture, blitMaterial);
			
			return renderTexture;
		}
		void ChangeBlitMaterial( PostProcessType type)
		{
			if( blitMaterial != null)
			{
				Material.DestroyImmediate( blitMaterial);
				blitMaterial = null;
			}
			string newShaderGuid = string.Empty;
			
			switch( type)
			{
				case PostProcessType.kNone:
				{
					newShaderGuid = "2b154a50c25ca3a4c9c4512a996093b1";
					break;
				}
				case PostProcessType.kOneMinus:
				{
					newShaderGuid = "442e234fb65d8cf498e37fa4a7d4947c";
					break;
				}
				case PostProcessType.kSwizzle:
				{
					newShaderGuid = "cfcb423e393b75a469ad749f8b662ba5";
					break;
				}
			}
			if( string.IsNullOrEmpty( newShaderGuid) == false)
			{
				string newShaderPath = AssetDatabase.GUIDToAssetPath( newShaderGuid);
				
				if( string.IsNullOrEmpty( newShaderPath) == false)
				{
					if( AssetDatabase.LoadAssetAtPath<Shader>( newShaderPath) is Shader shader)
					{
						blitMaterial = new Material( shader);
					}
				}
			}
		}
		
		[SerializeField]
		PostProcessType postProcessType = PostProcessType.kNone;
		
		[System.NonSerialized]
		RenderTexture renderTexture;
		[System.NonSerialized]
		Material blitMaterial;
	}
}