
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	public enum ColorComponent
	{
		kRed,
		kGreen,
		kBlue,
		kAlpha,
		kValue
	}
	[System.Serializable]
	public class SwizzleProcess : PostProcessBase
	{
		public override bool OnGUI( Rect rect)
		{
			bool ret = false;
			
			Rect elementRect = new Rect( rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
			
			for( int i0 = 0; i0 < 4; ++i0)
			{
				if( i0 > 0)
				{
					elementRect.y += elementRect.height + kIntervalHeight;
				}
				var component = (ColorComponent)EditorGUI.EnumPopup( 
					elementRect, kComponentNames[ i0], components[ i0]);
				if( components[ i0].Equals( component) == false)
				{
					Record( string.Format( $"Change Swizzle {kComponentNames[ i0]}"));
					components[ i0] = component;
					ret = true;
				}
				if( components[ i0] == ColorComponent.kValue)
				{
					elementRect.y += elementRect.height + kIntervalHeight;
					float value = EditorGUI.Slider( elementRect, 
						string.Format( $"{kComponentNames[ i0]} Value"), 
						componentValues[ i0], 0.0f, 1.0f);
					if( componentValues[ i0].Equals( value) == false)
					{
						Record( string.Format( $"Change Swizzle {kComponentNames[ i0]} Value"));
						componentValues[ i0] = value;
						ret = true;
					}
				}
			}
			return ret;
		}
		public override float GetHeight()
		{
			float height = (EditorGUIUtility.singleLineHeight + kIntervalHeight) * 4;
			
			for( int i0 = 0; i0 < 4; ++i0)
			{
				if( components[ i0] == ColorComponent.kValue)
				{
					height += EditorGUIUtility.singleLineHeight + kIntervalHeight;
				}
			}
			return height;
		}
		public override Material OnUpdateMaterial()
		{
			materialCache.SetVector( 
				"_SwizzleType", new Vector4( 
					(int)components[ 0],
					(int)components[ 1],
					(int)components[ 2],
					(int)components[ 3]));
			materialCache.SetVector( 
				"_SwizzleValue", new Vector4( 
					componentValues[ 0],
					componentValues[ 1],
					componentValues[ 2],
					componentValues[ 3]));
			return base.OnUpdateMaterial();
		}
		protected override string GetShaderGuid()
		{
			return "cfcb423e393b75a469ad749f8b662ba5";
		}
		
		static readonly string[] kComponentNames = new string[]
		{
			"Red", "Green", "Blue", "Alpha"
		};
		
		[SerializeField]
		ColorComponent[] components = new ColorComponent[]
		{
			ColorComponent.kRed,
			ColorComponent.kGreen,
			ColorComponent.kBlue,
			ColorComponent.kAlpha
		};
		[SerializeField]
		float[] componentValues = new float[]
		{
			0.0f, 0.0f, 0.0f, 0.0f
		};
	}
}