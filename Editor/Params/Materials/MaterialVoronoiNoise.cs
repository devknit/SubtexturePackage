
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialVoronoiNoise : MaterialBase
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
			float timeValue = EditorGUILayout.FloatField( "Time", time);
			if( time.Equals( timeValue) == false)
			{
				Record( "Change Time");
				time = timeValue;
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
			materialCache.SetFloat( "_TimePosition", time);
		}
		protected override string GetShaderGuid()
		{
			return "25a82b0086028984c9b934ab9644b05d";
		}
		
		[SerializeField]
		Vector2 uvScale = new Vector2( 8, 8);
		[SerializeField]
		Vector2 uvOffset = Vector2.zero;
		[SerializeField]
		float time = 0.0f;
	}
}