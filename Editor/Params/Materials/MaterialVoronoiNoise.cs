
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialVoronoiNoise : MaterialBaseUvProperties
	{
		public override void Reset()
		{
			base.Reset();
			SetUvScale( 8.0f);
			m_Time = 0.0f;
			m_Swizzle = 0;
			m_ColorScale = Vector3.one;
			m_SmoothEdges = new Vector2( 0.02f, 0.04f);
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
			int swizzleValue = EditorGUILayout.Popup( "Swizzle", m_Swizzle, kSwizzleLabels);
			if( m_Swizzle.Equals( swizzleValue) == false)
			{
				Record( "Change Swizzle");
				m_Swizzle = swizzleValue;
				ret = true;
			}
			Vector3 colorScaleValue = EditorGUILayout.Vector3Field( "Color Scale", m_ColorScale);
			if( m_ColorScale.Equals( colorScaleValue) == false)
			{
				Record( "Change Color Scale");
				m_ColorScale = colorScaleValue;
				ret = true;
			}
			Vector2 smoothEdgesValue = EditorGUILayout.Vector2Field( "Smooth Edges", m_SmoothEdges);
			if( m_SmoothEdges.Equals( smoothEdgesValue) == false)
			{
				Record( "Change Smooth Edges");
				m_SmoothEdges = smoothEdgesValue;
				ret = true;
			}
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			base.OnUpdateMaterial();
			
			if( Material != null)
			{
				Material.SetFloat( "_TimePosition", m_Time);
				Material.SetInt( "_Swizzle", m_Swizzle);
				Material.SetVector( "_ColorScale", new Vector4( m_ColorScale.x, m_ColorScale.y, m_ColorScale.z, 1));
				Material.SetVector( "_SmoothEdges", new Vector4( m_SmoothEdges.x, m_SmoothEdges.y, 0, 0));
			}
		}
		protected override string GetShaderGuid()
		{
			return "25a82b0086028984c9b934ab9644b05d";
		}
		static readonly string[] kSwizzleLabels = new string[]
		{
			"Gray 1",
			"Gray 2",
			"Gradation 1",
			"Gradation 2",
			"Red 1",
			"Red 2",
			"Green 1",
			"Green 2",
			"Blue 1",
			"Blue 2"
		};
		[SerializeField]
		float m_Time = 0.0f;
		[SerializeField]
		int m_Swizzle = 0;
		[SerializeField]
		Vector3 m_ColorScale = Vector3.one;
		[SerializeField]
		Vector2 m_SmoothEdges = new Vector2( 0.02f, 0.04f);
	}
}