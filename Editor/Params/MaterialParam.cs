
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	public enum MaterialType
	{
		kMaterialAssets,
		kTextureAsset,
		kRandomNoise,
		kBlockNoise,
		kValueNoise,
		kPerlinNoise,
		kFractalNoise,
		kCellularNoise,
		kVoronoiNoise,
		kCirclePattern,
	}
	[System.Serializable]
	public sealed class MaterialParam : BaseParam
	{
		public MaterialParam() : base( true)
		{
		}
		public override void OnEnable( Window window)
		{
			base.OnEnable( window);
			
			if( m_Materials == null)
			{
				m_Materials = new MaterialBase[]
				{
					null,
					new MaterialTexture(),
					new MaterialRandomNoise(),
					new MaterialBlockNoise(),
					new MaterialValueNoise(),
					new MaterialPerlinNoise(),
					new MaterialFractalNoise(),
					new MaterialCellularNoise(),
					new MaterialVoronoiNoise(),
					new MaterialCirclePattern(),
				};
			}
			for( int i0 = 0; i0 < m_Materials.Length; ++i0)
			{
				m_Materials[ i0]?.OnEnable( window);
			}
			ChangeDynamicMaterial( m_MaterialType);
		}
		public override void OnDisable()
		{
			if( m_DynamicMaterial != null)
			{
				m_DynamicMaterial = null;
			}
			if( m_MaterialProperties != null)
			{
				m_MaterialProperties.Dispose();
				m_MaterialProperties = null;
			}
			if( m_Materials != null)
			{
				for( int i0 = 0; i0 < m_Materials.Length; ++i0)
				{
					m_Materials[ i0]?.OnDisable();
				}
			}
			base.OnDisable();
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			if( param[ (int)PreParamType.kMesh] is MeshParam meshParam)
			{
				if( meshParam.MeshType == MeshType.kPrefab)
				{
					return 0;
				}
			}
			OnPUI( "Material", false, 
				() =>
				{
					if( m_MaterialType != MaterialType.kTextureAsset)
					{
						m_MaterialType = MaterialType.kTextureAsset;
						ChangeDynamicMaterial( m_MaterialType);
					}
					if( m_Materials != null)
					{
						for( int i0 = 0; i0 < m_Materials.Length; ++i0)
						{
							m_Materials[ i0]?.Reset();
							m_Materials[ i0]?.OnUpdateMaterial();
						}
					};
					m_AssetMaterial = null;
				}, 
				() =>
				{
					var type = (MaterialType)EditorGUILayout.EnumPopup( "Type", m_MaterialType);
					if( m_MaterialType.Equals( type) == false)
					{
						Record( "Change Material Type");
						ChangeDynamicMaterial( type);
						m_MaterialType = type;
					}
					switch( m_MaterialType)
					{
						case MaterialType.kMaterialAssets:
						{
							var assetMaterialValue = EditorGUILayout.ObjectField( "Material", m_AssetMaterial, typeof( Material), false) as Material;
							if( m_AssetMaterial != assetMaterialValue)
							{
								Record( "Change Asset Material");
								m_AssetMaterial = assetMaterialValue;
							}
							break;
						}
						default:
						{
							if( m_MaterialProperties != null)
							{
								if( m_MaterialProperties.OnGUI() != false)
								{
									m_MaterialProperties.OnUpdateMaterial();
								}
							}
							break;
						}
					}
				});
			return 0;
		}
		void ChangeDynamicMaterial( MaterialType type)
		{
			if( m_DynamicMaterial != null)
			{
				m_DynamicMaterial = null;
			}
			if( m_MaterialProperties != null)
			{
				m_MaterialProperties.Dispose();
				m_MaterialProperties = null;
			}
			m_MaterialProperties = m_Materials[ (int)type];
			
			if( m_MaterialProperties != null)
			{
				m_DynamicMaterial = m_MaterialProperties.Create();
				m_MaterialProperties.OnUpdateMaterial();
			}
		}
		public Material RenderMaterial
		{
			get{ return (m_MaterialType == MaterialType.kMaterialAssets)? m_AssetMaterial : m_DynamicMaterial; }
		}
		
		[SerializeField]
		MaterialType m_MaterialType = MaterialType.kTextureAsset;
		[SerializeReference]
		MaterialBase[] m_Materials;
		[SerializeField]
		Material m_AssetMaterial;
		
		MaterialBase m_MaterialProperties;
		Material m_DynamicMaterial;
	}
}