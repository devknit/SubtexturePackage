
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialBaseUvProperties : MaterialBase
	{
		public override void Reset()
		{
			SetUvScale( 1.0f);
			m_UvScaleAxisSync = true;
			m_UvOffsetAxisSync = true;
			m_UvOffset = Vector2.zero;
		}
		public override bool OnGUI()
		{
			bool ret = false;
			
			if( AxisSyncVector2Field( "UV Scale", ref m_UvScale, ref m_UvScaleAxisSync) != false)
			{
				ret = true;
			}
			if( AxisSyncVector2Field( "UV Offset", ref m_UvOffset, ref m_UvOffsetAxisSync) != false)
			{
				ret = true;
			}
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			if( Material != null)
			{
				Material.SetVector( "_UVScale", new Vector4( m_UvScale.x, m_UvScale.y, 1, 1));
				Material.SetVector( "_UVOffset", new Vector4( m_UvOffset.x, m_UvOffset.y, 0, 0));
			}
		}
		protected void SetUvScale( float scale)
		{
			m_UvScale = new Vector2( scale, scale);
		}
		[SerializeField]
		bool m_UvScaleAxisSync = true;
		[SerializeField]
		Vector2 m_UvScale = Vector2.one;
		[SerializeField]
		bool m_UvOffsetAxisSync = false;
		[SerializeField]
		Vector2 m_UvOffset = Vector2.zero;
	}
}