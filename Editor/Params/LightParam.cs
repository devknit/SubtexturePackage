﻿
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
		public override void OnGUI( BaseParam[] param)
		{
			OnPUI( "Light", false, () =>
			{
				Vector3 localPositionValue = EditorGUILayout.Vector3Field( "Position", localPosition);
				if( localPosition.Equals( localPositionValue) == false)
				{
					Record( "Change Position");
					localPosition = localPositionValue;
				}
				Vector3 localRotationValue = EditorGUILayout.Vector3Field( "Rotation", localRotation);
				if( localRotation.Equals( localRotationValue) == false)
				{
					Record( "Change Rotation");
					localRotation = localRotationValue;
				}
				Color lightColorValue = EditorGUILayout.ColorField( "Light Color", lightColor);
				if( lightColor.Equals( lightColorValue) == false)
				{
					Record( "Change LightColor");
					lightColor = lightColorValue;
				}
				Color ambientColorValue = EditorGUILayout.ColorField( "Ambient Color", ambientColor);
				if( ambientColor.Equals( ambientColorValue) == false)
				{
					Record( "Change AmbientColor");
					ambientColor = ambientColorValue;
				}
				float intensityValue = EditorGUILayout.FloatField( "Intensity", intensity);
				if( intensity.Equals( intensityValue) == false)
				{
					Record( "Change Intensity");
					intensity = intensityValue;
				}
				LightShadows shadowsValue = (LightShadows)EditorGUILayout.EnumPopup( "Shadows", shadows);
				if( shadows.Equals( shadowsValue) == false)
				{
					Record( "Change Shadows");
					shadows = shadowsValue;
				}
			});
		}
		public Color Apply( Light light)
		{
			light.transform.position = localPosition;
			light.transform.rotation = Quaternion.Euler( localRotation);
			light.color = lightColor;
			light.intensity = intensity;
			light.shadows = shadows ;
			
			return ambientColor;
		}
		
		[SerializeField]
		Vector3 localPosition = Vector3.zero;
		[SerializeField]
		Vector3 localRotation = new Vector3( 50, -30, 0);
		[SerializeField]
		Color ambientColor = new Color32( 54, 58, 66, 255);
		[SerializeField]
		Color lightColor = new Color32( 255, 244, 214, 255);
		[SerializeField]
		float intensity = 1.0f;
		[SerializeField]
		LightShadows shadows  = LightShadows.None;
	}
}