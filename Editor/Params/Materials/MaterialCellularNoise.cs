
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialCellularNoise : MaterialBaseUvProperties
	{
		public override void Reset()
		{
			base.Reset();
			SetUvScale( 8.0f);
			m_Time = 0.0f;
		}
		public override bool OnGUI()
		{
			bool ret = base.OnGUI();
			
			float timeValue = EditorGUILayout.FloatField( "Time", m_Time);
			if( m_Time.Equals( timeValue) == false)
			{
				Record( "Change Time");
				m_Time = timeValue;
				ret = true;
			}
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			base.OnUpdateMaterial();
			Material?.SetFloat( "_TimePosition", m_Time);
		}
		protected override string GetShaderGuid()
		{
			return "b68038a3be357504db49ab1958ec5ffe";
		}
		[SerializeField]
		float m_Time = 0.0f;
	}
}