
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialPerlinNoise : MaterialBase
	{
		public override void OnGUI()
		{
			bool update = false;
			
			Vector2 uvScaleValue = EditorGUILayout.Vector2Field( "UV Scale", uvScale);
			if( uvScale.Equals( uvScaleValue) == false)
			{
				Record( "Change UV Scale");
				uvScale = uvScaleValue;
				update = true;
			}
			Vector2 uvOffsetValue = EditorGUILayout.Vector2Field( "UV Offset", uvOffset);
			if( uvOffset.Equals( uvOffsetValue) == false)
			{
				Record( "Change UV Offset");
				uvOffset = uvOffsetValue;
				update = true;
			}
			float brightnessValue = EditorGUILayout.Slider( "Brightness", brightness, 0.0f, 1.0f);
			if( brightness.Equals( brightnessValue) == false)
			{
				Record( "Change Brightness");
				brightness = brightnessValue;
				update = true;
			}
			if( update != false)
			{
				OnUpdateMaterial();
			}
		}
		public override void OnUpdateMaterial()
		{
			materialCache.SetVector( "_UVScale", new Vector4( uvScale.x, uvScale.y, 1, 1));
			materialCache.SetVector( "_UVOffset", new Vector4( uvOffset.x, uvOffset.y, 1, 1));
			materialCache.SetFloat( "_Brightness", brightness);
		}
		protected override string GetShaderGuid()
		{
			return "b2423f437ef759643be61fd6902a848e";
		}
		
		[SerializeField]
		Vector2 uvScale = new Vector2( 8, 8);
		[SerializeField]
		Vector2 uvOffset = Vector2.zero;
		[SerializeField]
		float brightness = 0.5f;
	}
}