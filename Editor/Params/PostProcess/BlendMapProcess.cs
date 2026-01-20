
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	public enum ColorBlendOp
	{
		None,
		Override,
		Multiply,
		Darken,
		ColorBurn,
		LinearBurn,
		Lighten,
		Screen,
		ColorDodge,
		LinearDodge,
		Overlay,
		HardLight,
		VividLight,
		LinearLight,
		PinLight,
		HardMix,
		Difference,
		Exclusion,
		Substract,
		Division,
		Hue,
		Saturation,
		Luminosity,
		Color
	}
	public enum ColorBlendRatioSource
	{
		Value,
		AlphaBlendOp,
		OneMinusAlphaBlendOp,
		BaseAlpha,
		OneMinusBaseAlpha,
		BlendAlpha,
		OneMinusBlendAlpha,
		BaseColorValue,
		OneMinusBaseColorValue,
		BlendColorValue,
		OneMinusBlendColorValue
	}
	public enum AlphaBlendOp
	{
		None,
		Override,
		Multiply,
		Add,
		Substract,
		ReverseSubstract,
		Maximum
	}
	[System.Serializable]
	public class BlendMapProcess : PostProcessBase
	{
		bool OnTextureGUI( Rect rect)
		{
			bool ret = false;
			
			Rect textureRect = new Rect( rect.x, rect.y, rect.width, 64);
			
			var newTexture = EditorGUI.ObjectField( textureRect, "Texture", m_Texture, typeof( Texture2D), false) as Texture2D;
			if( m_Texture != newTexture)
			{
				Record( "Change Blend Map");
				m_Texture = newTexture;
				ret = true;
			}
			
			float labelWidth = 64;
			float x = rect.x + 15.0f;
			float y = rect.y + 64.0f - EditorGUIUtility.singleLineHeight;
			float w = rect.width - (15.0f + 64.0f + 2.0f) - labelWidth;
			if( w < 70.0f)
			{
				w = 70.0f;
			}
			
			var offsetPrefixRect = new Rect( 
				x, y, labelWidth, EditorGUIUtility.singleLineHeight);
			EditorGUI.PrefixLabel( offsetPrefixRect, new GUIContent( "Offset"));
			
			Rect offsetRect = new Rect( 
				x + labelWidth, y, w, EditorGUIUtility.singleLineHeight);
			Vector2 newOffset = EditorGUI.Vector2Field( offsetRect, string.Empty, m_Offset);
			if( m_Offset.Equals( newOffset) == false)
			{
				Record( "Change Offset");
				m_Offset = newOffset;
				ret = true;
			}
			
			y -= EditorGUIUtility.singleLineHeight + 2.0f;
			
			var tilingPrefixRect = new Rect( 
				x, y, labelWidth, EditorGUIUtility.singleLineHeight);
			EditorGUI.PrefixLabel( tilingPrefixRect, new GUIContent( "Tiling"));
			
			Rect tilingRect = new Rect( 
				x + labelWidth, y, w, EditorGUIUtility.singleLineHeight);
			Vector2 newTiling = EditorGUI.Vector2Field( tilingRect, string.Empty, m_Tiling);
			if( m_Tiling.Equals( newTiling) == false)
			{
				Record( "Change Tiling");
				m_Tiling = newTiling;
				ret = true;
			}
			return ret;
		}
		
		public override bool OnGUI( Rect rect)
		{
			bool ret = false;
			
			if( OnTextureGUI( rect) != false)
			{
				ret = true;
			}
			Rect elementRect = new Rect( rect.x, rect.y + 66, rect.width, EditorGUIUtility.singleLineHeight);
			
			var newColorBlendOp = (ColorBlendOp)EditorGUI.EnumPopup( elementRect, " Color Blend Op", m_ColorBlendOp);
			if( m_ColorBlendOp != newColorBlendOp)
			{
				Record( "Change Color Blend Op");
				m_ColorBlendOp = newColorBlendOp;
				ret = true;
			}
			elementRect.y += elementRect.height + 2;
			
			var newColorBlendRatioSource = (ColorBlendRatioSource)EditorGUI.EnumPopup( elementRect, " Color Blend Ratio Source", m_ColorBlendRatioSource);
			if( m_ColorBlendRatioSource != newColorBlendRatioSource)
			{
				Record( "Change Color Blend Ratio Source");
				m_ColorBlendRatioSource = newColorBlendRatioSource;
				ret = true;
			}
			elementRect.y += elementRect.height + 2;
			
			var newColorBlendRatioValue = EditorGUI.FloatField( elementRect, " Color Blend Ratio Value", m_ColorBlendRatioValue);
			if( m_ColorBlendRatioValue != newColorBlendRatioValue)
			{
				Record( "Change Color Blend Ratio Value");
				m_ColorBlendRatioValue = newColorBlendRatioValue;
				ret = true;
			}
			elementRect.y += elementRect.height + 2;
			
			var newAlphaBlendOp = (AlphaBlendOp)EditorGUI.EnumPopup( elementRect, " Alpha Blend Op", m_AlphaBlendOp);
			if( m_AlphaBlendOp != newAlphaBlendOp)
			{
				Record( "Change Alpha Blend Op");
				m_AlphaBlendOp = newAlphaBlendOp;
				ret = true;
			}
			elementRect.y += elementRect.height + 2;
			
			var newAlphaBlendRatioValue = EditorGUI.FloatField( elementRect, " Alpha Blend Ratio Value", m_AlphaBlendRatioValue);
			if( m_AlphaBlendRatioValue != newAlphaBlendRatioValue)
			{
				Record( "Change Alpha Blend Ratio Value");
				m_AlphaBlendRatioValue = newAlphaBlendRatioValue;
				ret = true;
			}
			return ret;
		}
		public override float GetHeight()
		{
			float height = (EditorGUIUtility.singleLineHeight + 2) * 5;
			
			height += 64 + 2;
			
			return height;
		}
		public override Material OnUpdateMaterial()
		{
			if( Material != null)
			{
				Material.SetVector( "_UVScale", new Vector4( m_Tiling.x, m_Tiling.y, 0.0f, 0.0f));
				Material.SetVector( "_UVOffset", new Vector4( m_Offset.x, m_Offset.y, 0.0f, 0.0f));
				Material.SetTexture( "_BlendTex", m_Texture);
				Material.SetFloat( "_ColorBlendRatio1", m_ColorBlendRatioValue);
				Material.SetFloat( "_AlphaBlendRatio1", m_AlphaBlendRatioValue);
				
				string[] keywords = Material.shaderKeywords;
				
				for( int i0 = 0; i0 < keywords.Length; ++i0)
				{
					Material.DisableKeyword( keywords[ i0]);
				}
				Material.EnableKeyword( string.Format( $"_COLORBLENDOP1_{m_ColorBlendOp.ToString().ToUpper()}"));
				Material.EnableKeyword( string.Format( $"_COLORBLENDSRC1_{m_ColorBlendRatioSource.ToString().ToUpper()}"));
				Material.EnableKeyword( string.Format( $"_ALPHABLENDOP1_{m_AlphaBlendOp.ToString().ToUpper()}"));
			}
			return base.OnUpdateMaterial();
		}
		protected override string GetShaderGuid()
		{
			return "85e57f9688bf8f348b206f87e9ea9504";
		}
		
		[SerializeField]
		Texture2D m_Texture;
		[SerializeField]
		Vector2 m_Tiling = Vector2.one;
		[SerializeField]
		Vector2 m_Offset = Vector2.zero;
		[SerializeField]
		ColorBlendOp m_ColorBlendOp;
		[SerializeField]
		ColorBlendRatioSource m_ColorBlendRatioSource;
		[SerializeField]
		float m_ColorBlendRatioValue = 1.0f;
		[SerializeField]
		AlphaBlendOp m_AlphaBlendOp;
		[SerializeField]
		float m_AlphaBlendRatioValue = 1.0f;
	}
}