
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialPerlinNoise : MaterialBaseUvProperties
	{
		public override void Reset()
		{
			base.Reset();
			SetUvScale( 8.0f);
			m_Brightness = 0.5f;
		}
		public override bool OnGUI()
		{
			bool ret = base.OnGUI();
			
			float brightnessValue = EditorGUILayout.Slider( "Brightness", m_Brightness, 0.0f, 1.0f);
			if( m_Brightness.Equals( brightnessValue) == false)
			{
				Record( "Change Brightness");
				m_Brightness = brightnessValue;
				ret = true;
			}
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			base.OnUpdateMaterial();
			Material?.SetFloat( "_Brightness", m_Brightness);
		}
		protected override string GetShaderGuid()
		{
			return "b2423f437ef759643be61fd6902a848e";
		}
		[SerializeField]
		float m_Brightness = 0.5f;
	}
}