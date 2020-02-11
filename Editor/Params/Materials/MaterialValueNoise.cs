
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialValueNoise : MaterialBase
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
			return "c213afc6e8574a540b30cd6ef7c038ef";
		}
		
		[SerializeField]
		Vector2 uvScale = new Vector2( 8, 8);
		[SerializeField]
		Vector2 uvOffset = Vector2.zero;
	}
}