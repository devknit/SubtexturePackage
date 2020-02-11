
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialBase
	{
		public void OnEnable( Window window)
		{
			handle = window;
		}
		public void OnDisable()
		{
		}
		public void Record( string label)
		{
			handle.Record( label);
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
						materialCache = new Material( shader);
						return materialCache;
					}
				}
			}
			return null;
		}
		public void Dispose()
		{
			if( materialCache != null)
			{
				materialCache = null;
			}
		}
		public virtual void OnGUI()
		{
		}
		public virtual void OnUpdateMaterial()
		{
		}
		protected virtual string GetShaderGuid()
		{
			return string.Empty;
		}
		
		[System.NonSerialized]
		Window handle;
		[System.NonSerialized]
		protected Material materialCache;
	}
}