
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public abstract class PostProcessBase
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
				Material.DestroyImmediate( materialCache);
				materialCache = null;
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
		public virtual void OnUpdateMaterial()
		{
		}
		protected abstract string GetShaderGuid();
		
		[System.NonSerialized]
		Window handle;
		[System.NonSerialized]
		protected Material materialCache;
	}
}