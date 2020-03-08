
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
			
			var newTexture = EditorGUI.ObjectField( textureRect, "Texture", texture, typeof( Texture2D), false) as Texture2D;
			if( texture != newTexture)
			{
				Record( "Change Blend Map");
				texture = newTexture;
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
			Vector2 newOffset = EditorGUI.Vector2Field( offsetRect, string.Empty, offset);
			if( offset.Equals( newOffset) == false)
			{
				Record( "Change Offset");
				offset = newOffset;
				ret = true;
			}
			
			y -= EditorGUIUtility.singleLineHeight + 2.0f;
			
			var tilingPrefixRect = new Rect( 
				x, y, labelWidth, EditorGUIUtility.singleLineHeight);
			EditorGUI.PrefixLabel( tilingPrefixRect, new GUIContent( "Tiling"));
			
			Rect tilingRect = new Rect( 
				x + labelWidth, y, w, EditorGUIUtility.singleLineHeight);
			Vector2 newTiling = EditorGUI.Vector2Field( tilingRect, string.Empty, tiling);
			if( tiling.Equals( newTiling) == false)
			{
				Record( "Change Tiling");
				tiling = newTiling;
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
			
			var newColorBlendOp = (ColorBlendOp)EditorGUI.EnumPopup( elementRect, " Color Blend Op", colorBlendOp);
			if( colorBlendOp != newColorBlendOp)
			{
				Record( "Change Color Blend Op");
				colorBlendOp = newColorBlendOp;
				ret = true;
			}
			elementRect.y += elementRect.height + 2;
			
			var newColorBlendRatioSource = (ColorBlendRatioSource)EditorGUI.EnumPopup( elementRect, " Color Blend Ratio Source", colorBlendRatioSource);
			if( colorBlendRatioSource != newColorBlendRatioSource)
			{
				Record( "Change Color Blend Ratio Source");
				colorBlendRatioSource = newColorBlendRatioSource;
				ret = true;
			}
			elementRect.y += elementRect.height + 2;
			
			var newColorBlendRatioValue = EditorGUI.FloatField( elementRect, " Color Blend Ratio Value", colorBlendRatioValue);
			if( colorBlendRatioValue != newColorBlendRatioValue)
			{
				Record( "Change Color Blend Ratio Value");
				colorBlendRatioValue = newColorBlendRatioValue;
				ret = true;
			}
			elementRect.y += elementRect.height + 2;
			
			var newAlphaBlendOp = (AlphaBlendOp)EditorGUI.EnumPopup( elementRect, " Alpha Blend Op", alphaBlendOp);
			if( alphaBlendOp != newAlphaBlendOp)
			{
				Record( "Change Alpha Blend Op");
				alphaBlendOp = newAlphaBlendOp;
				ret = true;
			}
			elementRect.y += elementRect.height + 2;
			
			var newAlphaBlendRatioValue = EditorGUI.FloatField( elementRect, " Alpha Blend Ratio Value", alphaBlendRatioValue);
			if( alphaBlendRatioValue != newAlphaBlendRatioValue)
			{
				Record( "Change Alpha Blend Ratio Value");
				alphaBlendRatioValue = newAlphaBlendRatioValue;
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
			materialCache.SetVector( "_UVScale", new Vector4( tiling.x, tiling.y, 0.0f, 0.0f));
			materialCache.SetVector( "_UVOffset", new Vector4( offset.x, offset.y, 0.0f, 0.0f));
			materialCache.SetTexture( "_BlendTex", texture);
			materialCache.SetFloat( "_ColorBlendRatio1", colorBlendRatioValue);
			materialCache.SetFloat( "_AlphaBlendRatio1", alphaBlendRatioValue);
			
			string[] keywords = materialCache.shaderKeywords;
			
			for( int i0 = 0; i0 < keywords.Length; ++i0)
			{
				materialCache.DisableKeyword( keywords[ i0]);
			}
			materialCache.EnableKeyword( string.Format( $"_COLORBLENDOP1_{colorBlendOp.ToString().ToUpper()}"));
			materialCache.EnableKeyword( string.Format( $"_COLORBLENDSRC1_{colorBlendRatioSource.ToString().ToUpper()}"));
			materialCache.EnableKeyword( string.Format( $"_ALPHABLENDOP1_{alphaBlendOp.ToString().ToUpper()}"));
			
			return base.OnUpdateMaterial();
		}
		protected override string GetShaderGuid()
		{
			return "85e57f9688bf8f348b206f87e9ea9504";
		}
		
		[SerializeField]
		Texture2D texture;
		[SerializeField]
		Vector2 tiling = Vector2.one;
		[SerializeField]
		Vector2 offset = Vector2.zero;
		[SerializeField]
		ColorBlendOp colorBlendOp;
		[SerializeField]
		ColorBlendRatioSource colorBlendRatioSource;
		[SerializeField]
		float colorBlendRatioValue = 1.0f;
		[SerializeField]
		AlphaBlendOp alphaBlendOp;
		[SerializeField]
		float alphaBlendRatioValue = 1.0f;
	}
}