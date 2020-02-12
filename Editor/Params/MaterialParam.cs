
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Subtexture
{
	public enum MaterialType
	{
		kAssets = -1,
		kRandomNoise,
		kBlockNoise,
		kValueNoise,
		kPerlinNoise,
		kFractalNoise,
		kCellularNoise,
		kVoronoiNoise,
		kCirclePattern,
	//	kProceduralRing,
	}
	[System.Serializable]
	public sealed class MaterialParam : BaseParam
	{
		public override void OnEnable( Window window, bool opened)
		{
			base.OnEnable( window, opened);
			
			if( materialList == null)
			{
				materialList = new List<MaterialBase>();
				materialList.Add( new MaterialRandomNoise());
				materialList.Add( new MaterialBlockNoise());
				materialList.Add( new MaterialValueNoise());
				materialList.Add( new MaterialPerlinNoise());
				materialList.Add( new MaterialFractalNoise());
				materialList.Add( new MaterialCellularNoise());
				materialList.Add( new MaterialVoronoiNoise());
				materialList.Add( new MaterialCirclePattern());
			}
			for( int i0 = 0; i0 < materialList.Count; ++i0)
			{
				materialList[ i0].OnEnable( window);
			}
			ChangeDynamicMaterial( materialType);
		}
		public override void OnDisable()
		{
			if( materialList != null)
			{
				for( int i0 = 0; i0 < materialList.Count; ++i0)
				{
					materialList[ i0].OnDisable();
				}
			}
			if( materialProperties != null)
			{
				materialProperties.Dispose();
				materialProperties = null;
			}
			if( dynamicMaterial != null)
			{
				Material.DestroyImmediate( dynamicMaterial);
				dynamicMaterial = null;
			}
			base.OnDisable();
		}
		public override void OnGUI()
		{
			OnPUI( "Material", () =>
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
				Material.DestroyImmediate( dynamicMaterial);
				dynamicMaterial = null;
			}
			if( materialProperties != null)
			{
				materialProperties.Dispose();
				materialProperties = null;
			}
			if( type != MaterialType.kAssets)
			{
				materialProperties = materialList[ (int)type];
			}
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
		public MaterialType materialType = MaterialType.kAssets;
		[SerializeReference]
		List<MaterialBase> materialList;
		[SerializeField]
		Material assetMaterial;
		
		MaterialBase materialProperties;
		Material dynamicMaterial;
	}
}