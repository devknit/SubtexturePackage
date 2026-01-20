
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialTexture : MaterialBaseUvProperties
	{
		public override void Reset()
		{
			base.Reset();
			m_Texture = null;
		}
		public override bool OnGUI()
		{
			bool ret = false;
			
			var textureValue = EditorGUILayout.ObjectField( "Texture", m_Texture, typeof( Texture2D), false) as Texture2D;
			if( m_Texture != textureValue)
			{
				Record( "Change Texture");
				m_Texture = textureValue;
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
			Material?.SetTexture( "_MainTex", m_Texture);
		}
		protected override string GetShaderGuid()
		{
			return "ece542f89f44ed34d8d477b8fdbcb62f";
		}
		
		[SerializeField]
		Texture2D m_Texture;
	}
}