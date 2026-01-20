
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	public enum PostProcessType
	{
		kNone,
		kOneMinus,
		kSwizzle,
		kBlendMap,
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
			
			if( m_Processes == null)
			{
				m_Processes = new PostProcessBase[]
				{
					new NoneProcess(),
					new OneMinusProcess(),
					new SwizzleProcess(),
					new BlendMapProcess()
				};
			}
			for( int i0 = 0; i0 < m_Processes.Length; ++i0)
			{
				m_Processes[ i0]?.OnEnable( window);
			}
			ChangeBlitMaterial( m_PostProcessType);
		}
		public override void OnDisable()
		{
			if( m_RenderTexture != null)
			{
				RenderTexture.DestroyImmediate( m_RenderTexture);
				m_RenderTexture = null;
			}
			if( m_ProcessProperties != null)
			{
				m_ProcessProperties.Dispose();
				m_ProcessProperties = null;
			}
			base.OnDisable();
		}
		public void OnElementGUI( Rect rect)
		{
			rect.yMin += 2.0f;
			Rect typeRect = new Rect( rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
			var postProcessTypeValue = (PostProcessType)EditorGUI.EnumPopup( typeRect, m_PostProcessType);
			if( m_PostProcessType.Equals( postProcessTypeValue) == false)
			{
				Record( "Change PostProcess Type");
				ChangeBlitMaterial( postProcessTypeValue);
				m_PostProcessType = postProcessTypeValue;
			}
			if( m_ProcessProperties != null)
			{
				if( m_ProcessProperties.OnGUI( new Rect( 
					rect.x, rect.y + typeRect.height + 2.0f, 
					rect.width, rect.height - typeRect.height)) != false)
				{
					m_BlitMaterial = m_ProcessProperties.OnUpdateMaterial();
				}
			}
		}
		public float GetElementHeight()
		{
			return EditorGUIUtility.singleLineHeight + (m_ProcessProperties?.GetHeight() ?? 0) + 6.0f;
		}
		public RenderTexture Blit( RenderTexture source)
		{
			if( m_BlitMaterial != null)
			{
				if( m_RenderTexture != null)
				{
					if( m_RenderTexture.width != source.width || m_RenderTexture.height != source.height)
					{
						RenderTexture.DestroyImmediate( m_RenderTexture);
						m_RenderTexture = null;
					}
				}
				if( m_RenderTexture == null)
				{
					m_RenderTexture = new RenderTexture( source.width, source.height, 32, RenderTextureFormat.ARGBFloat);
					m_RenderTexture.hideFlags = HideFlags.HideAndDontSave;
				}
				Graphics.Blit( source, m_RenderTexture, m_BlitMaterial);
				
				return m_RenderTexture;
			}
			return source;
		}
		void ChangeBlitMaterial( PostProcessType type)
		{
			if( m_BlitMaterial != null)
			{
				m_BlitMaterial = null;
			}
			if( m_ProcessProperties != null)
			{
				m_ProcessProperties.Dispose();
				m_ProcessProperties = null;
			}
			m_ProcessProperties = m_Processes[ (int)type];
			
			if( m_ProcessProperties != null)
			{
				m_ProcessProperties.Create();
				m_BlitMaterial = m_ProcessProperties.OnUpdateMaterial();
			}
		}
		
		[SerializeField]
		PostProcessType m_PostProcessType = PostProcessType.kNone;
		[SerializeReference]
		PostProcessBase[] m_Processes;
		
		[System.NonSerialized]
		RenderTexture m_RenderTexture;
		[System.NonSerialized]
		PostProcessBase m_ProcessProperties;
		[System.NonSerialized]
		Material m_BlitMaterial;
		
	}
}