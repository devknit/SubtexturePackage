
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialBase
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
		public virtual void Reset()
		{
		}
		public virtual bool OnGUI()
		{
			return false;
		}
		public virtual void OnUpdateMaterial()
		{
		}
		protected virtual string GetShaderGuid()
		{
			return string.Empty;
		}
		protected bool AxisSyncVector2Field( string caption, ref Vector2 source, ref bool axisSync)
		{
			bool ret = false;
			
			bool axisSyncValue = EditorGUILayout.ToggleLeft( caption, axisSync);
			if( axisSync.Equals( axisSyncValue) == false)
			{
				Record( string.Format( $"Change {caption} Axis Sync"));
				axisSync = axisSyncValue;
				ret = true;
			}
			++EditorGUI.indentLevel;
			Vector2 sourceValue = EditorGUILayout.Vector2Field( string.Empty, source);
			--EditorGUI.indentLevel;
			if( source.Equals( sourceValue) == false)
			{
				if( axisSync != false)
				{
					if( source.x != sourceValue.x)
					{
						sourceValue.y = sourceValue.x;
					}
					else
					{
						sourceValue.x = sourceValue.y;
					}
				}
				Record( string.Format( $"Change {caption}"));
				source = sourceValue;
				ret = true;
			}
			return ret;
		}
		protected Material Material
		{
			get{ return m_Material;}
		}
		[System.NonSerialized]
		Window m_Handle;
		[System.NonSerialized]
		Material m_Material;
	}
}