
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public sealed class LightParam : BaseParam
	{
		public LightParam() : base( false)
		{
		}
		public void OrbitControl( Light light, Vector2 value)
		{
			light.transform.RotateAround( Vector3.zero, Vector3.up, value.x);
			light.transform.RotateAround( Vector3.zero, light.transform.right, value.y);
			m_LocalPosition = light.transform.localPosition;
			m_LocalRotation = light.transform.localEulerAngles;
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			OnPUI( "Light", false, 
				() =>
				{
					m_LocalPosition = Vector3.zero;
					m_LocalRotation = new Vector3( 50, -30, 0);
					m_AmbientColor = new Color32( 54, 58, 66, 255);
					m_LightColor = new Color32( 255, 244, 214, 255);
					m_Intensity = 1.0f;
					m_Shadows  = LightShadows.None;
				}, 
				() =>
				{
					Vector3 localPositionValue = EditorGUILayout.Vector3Field( "Position", m_LocalPosition);
					if( m_LocalPosition.Equals( localPositionValue) == false)
					{
						Record( "Change Position");
						m_LocalPosition = localPositionValue;
					}
					Vector3 localRotationValue = EditorGUILayout.Vector3Field( "Rotation", m_LocalRotation);
					if( m_LocalRotation.Equals( localRotationValue) == false)
					{
						Record( "Change Rotation");
						m_LocalRotation = localRotationValue;
					}
					Color lightColorValue = EditorGUILayout.ColorField( "Light Color", m_LightColor);
					if( m_LightColor.Equals( lightColorValue) == false)
					{
						Record( "Change LightColor");
						m_LightColor = lightColorValue;
					}
					Color ambientColorValue = EditorGUILayout.ColorField( "Ambient Color", m_AmbientColor);
					if( m_AmbientColor.Equals( ambientColorValue) == false)
					{
						Record( "Change AmbientColor");
						m_AmbientColor = ambientColorValue;
					}
					float intensityValue = EditorGUILayout.FloatField( "Intensity", m_Intensity);
					if( m_Intensity.Equals( intensityValue) == false)
					{
						Record( "Change Intensity");
						m_Intensity = intensityValue;
					}
					LightShadows shadowsValue = (LightShadows)EditorGUILayout.EnumPopup( "Shadows", m_Shadows);
					if( m_Shadows.Equals( shadowsValue) == false)
					{
						Record( "Change Shadows");
						m_Shadows = shadowsValue;
					}
				});
			return 0;
		}
		public Color Apply( Light light)
		{
			light.transform.position = m_LocalPosition;
			light.transform.rotation = Quaternion.Euler( m_LocalRotation);
			light.color = m_LightColor;
			light.intensity = m_Intensity;
			light.shadows = m_Shadows ;
			
			return m_AmbientColor;
		}
		[SerializeField]
		Vector3 m_LocalPosition = Vector3.zero;
		[SerializeField]
		Vector3 m_LocalRotation = new Vector3( 50, -30, 0);
		[SerializeField]
		Color m_AmbientColor = new Color32( 54, 58, 66, 255);
		[SerializeField]
		Color m_LightColor = new Color32( 255, 244, 214, 255);
		[SerializeField]
		float m_Intensity = 1.0f;
		[SerializeField]
		LightShadows m_Shadows  = LightShadows.None;
	}
}