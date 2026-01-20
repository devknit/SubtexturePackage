
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public abstract class PostProcessBase
	{
		public void OnEnable( Window window)
		{
			m_Handle = window;
		}
		public void OnDisable()
		{
		}
		public void Record( string label)
		{
			m_Handle.Record( label);
		}
		public Material Create()
		{
			string newShaderGuid = GetShaderGuid();
			
			if( string.IsNullOrEmpty( newShaderGuid) == false)
			{
				string newShaderPath = AssetDatabase.GUIDToAssetPath( newShaderGuid);
				
				if( string.IsNullOrEmpty( newShaderPath) == false)
				{
					if( AssetDatabase.LoadAssetAtPath<Shader>( newShaderPath) is Shader shader)
					{
						m_Material = new Material( shader);
						return m_Material;
					}
				}
			}
			return null;
		}
		public void Dispose()
		{
			if( m_Material != null)
			{
				Material.DestroyImmediate( m_Material);
				m_Material = null;
			}
		}
		public virtual bool OnGUI( Rect rect)
		{
			return false;
		}
		public virtual float GetHeight()
		{
			return 0.0f;
		}
		public virtual Material OnUpdateMaterial()
		{
			return m_Material;
		}
		protected abstract string GetShaderGuid();
		
		protected Material Material
		{
			get{ return m_Material; }
		}
		protected const float kIntervalHeight = 2.0f;
		
		[System.NonSerialized]
		Window m_Handle;
		[System.NonSerialized]
		Material m_Material;
	}
}