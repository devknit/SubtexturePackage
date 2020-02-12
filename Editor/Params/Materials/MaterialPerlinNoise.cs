
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialPerlinNoise : MaterialBaseUvProperties
	{
		public override bool OnGUI()
		{
			bool ret = base.OnGUI();
			
			float brightnessValue = EditorGUILayout.Slider( "Brightness", brightness, 0.0f, 1.0f);
			if( brightness.Equals( brightnessValue) == false)
			{
				Record( "Change Brightness");
				brightness = brightnessValue;
				ret = true;
			}
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			base.OnUpdateMaterial();
			materialCache.SetFloat( "_Brightness", brightness);
		}
		protected override string GetShaderGuid()
		{
			return "b2423f437ef759643be61fd6902a848e";
		}
		
		[SerializeField]
		float brightness = 0.5f;
	}
}