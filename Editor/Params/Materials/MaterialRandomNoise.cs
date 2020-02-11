
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialRandomNoise : MaterialBase
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
			if( update != false)
			{
				OnUpdateMaterial();
			}
		}
		public override void OnUpdateMaterial()
		{
			materialCache.SetVector( "_UVScale", new Vector4( uvScale.x, uvScale.y, 1, 1));
			materialCache.SetVector( "_UVOffset", new Vector4( uvOffset.x, uvOffset.y, 1, 1));
		}
		protected override string GetShaderGuid()
		{
			return "a0bc3d9f3b361864d851388e6a7071ec";
		}
		
		[SerializeField]
		Vector2 uvScale = new Vector2( 8, 8);
		[SerializeField]
		Vector2 uvOffset = Vector2.zero;
	}
}