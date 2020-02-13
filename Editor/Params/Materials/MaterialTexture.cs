
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialTexture : MaterialBaseUvProperties
	{
		public MaterialTexture()
		{
			uvScale = Vector2.one;
		}
		public override bool OnGUI()
		{
			bool ret = false;
			
			var textureValue = EditorGUILayout.ObjectField( "Texture", texture, typeof( Texture2D), false) as Texture2D;
			if( texture != textureValue)
			{
				Record( "Change Texture");
				texture = textureValue;
				ret = true;
			}
			if( base.OnGUI() != false)
			{
				ret = true;
			}
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			base.OnUpdateMaterial();
			materialCache.SetTexture( "_MainTex", texture);
		}
		protected override string GetShaderGuid()
		{
			return "ece542f89f44ed34d8d477b8fdbcb62f";
		}
		
		[SerializeField]
		Texture2D texture;
	}
}