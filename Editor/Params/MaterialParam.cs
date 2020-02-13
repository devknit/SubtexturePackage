
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Subtexture
{
	public enum MaterialType
	{
		kAssets,
		kRandomNoise,
		kBlockNoise,
		kValueNoise,
		kPerlinNoise,
		kFractalNoise,
		kCellularNoise,
		kVoronoiNoise,
		kCirclePattern,
		kPolygonPattern,
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
			
			if( materials == null)
			{
				materials = new MaterialBase[]
				{
					null,
					new MaterialRandomNoise(),
					new MaterialBlockNoise(),
					new MaterialValueNoise(),
					new MaterialPerlinNoise(),
					new MaterialFractalNoise(),
					new MaterialCellularNoise(),
					new MaterialVoronoiNoise(),
					new MaterialCirclePattern(),
					new MaterialPolygonPattern()
				};
			}
			for( int i0 = 0; i0 < materials.Length; ++i0)
			{
				materials[ i0]?.OnEnable( window);
			}
			ChangeDynamicMaterial( materialType);
		}
		public override void OnDisable()
		{
			if( dynamicMaterial != null)
			{
				dynamicMaterial = null;
			}
			if( materialProperties != null)
			{
				materialProperties.Dispose();
				materialProperties = null;
			}
			if( materials != null)
			{
				for( int i0 = 0; i0 < materials.Length; ++i0)
				{
					materials[ i0]?.OnDisable();
				}
			}
			base.OnDisable();
		}
		public override void OnGUI()
		{
			OnPUI( "Material", false, () =>
			{
				var type = (MaterialType)EditorGUILayout.EnumPopup( "Type", materialType);
				if( materialType.Equals( type) == false)
				{
					Record( "Change Material Type");
					ChangeDynamicMaterial( type);
					materialType = type;
				}
				switch( materialType)
				{
					case MaterialType.kAssets:
					{
						var assetMaterialValue = EditorGUILayout.ObjectField( "Material", assetMaterial, typeof( Material), false) as Material;
						if( assetMaterial != assetMaterialValue)
						{
							Record( "Change Asset Material");
							assetMaterial = assetMaterialValue;
						}
						break;
					}
					default:
					{
						if( materialProperties != null)
						{
							if( materialProperties.OnGUI() != false)
							{
								materialProperties.OnUpdateMaterial();
							}
						}
						break;
					}
				}
			});
		}
		void ChangeDynamicMaterial( MaterialType type)
		{
			if( dynamicMaterial != null)
			{
				dynamicMaterial = null;
			}
			if( materialProperties != null)
			{
				materialProperties.Dispose();
				materialProperties = null;
			}
			materialProperties = materials[ (int)type];
			
			if( materialProperties != null)
			{
				dynamicMaterial = materialProperties.Create();
				materialProperties.OnUpdateMaterial();
			}
		}
		public Material RenderMaterial
		{
			get{ return (materialType == MaterialType.kAssets)? assetMaterial : dynamicMaterial; }
		}
		
		[SerializeField]
		MaterialType materialType = MaterialType.kAssets;
		[SerializeReference]
		MaterialBase[] materials;
		[SerializeField]
		Material assetMaterial;
		
		MaterialBase materialProperties;
		Material dynamicMaterial;
	}
}