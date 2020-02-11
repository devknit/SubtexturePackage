
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

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
	//	kProceduralRing,
	}
	[System.Serializable]
	public sealed class MaterialParam : BaseParam
	{
		public override void OnEnable( Window window, bool opened)
		{
			base.OnEnable( window, opened);
			
			if( materialRandomNoise == null)
			{
				materialRandomNoise = new MaterialRandomNoise();
			}
			materialRandomNoise.OnEnable( window);
			
			if( materialBlockNoise == null)
			{
				materialBlockNoise = new MaterialBlockNoise();
			}
			materialBlockNoise.OnEnable( window);
			
			if( materialValueNoise == null)
			{
				materialValueNoise = new MaterialValueNoise();
			}
			materialValueNoise.OnEnable( window);
			
			if( materialPerlinNoise == null)
			{
				materialPerlinNoise = new MaterialPerlinNoise();
			}
			materialPerlinNoise.OnEnable( window);
			
			if( materialFractalNoise == null)
			{
				materialFractalNoise = new MaterialFractalNoise();
			}
			materialFractalNoise.OnEnable( window);
			
			if( materialCellularNoise == null)
			{
				materialCellularNoise = new MaterialCellularNoise();
			}
			materialCellularNoise.OnEnable( window);
			
			if( materialVoronoiNoise == null)
			{
				materialVoronoiNoise = new MaterialVoronoiNoise();
			}
			materialVoronoiNoise.OnEnable( window);
			
			if( materialCirclePattern == null)
			{
				materialCirclePattern = new MaterialCirclePattern();
			}
			materialCirclePattern.OnEnable( window);
			
			ChangeDynamicMaterial( materialType);
		}
		public override void OnDisable()
		{
			if( materialRandomNoise != null)
			{
				materialRandomNoise.OnDisable();
			}
			if( materialBlockNoise != null)
			{
				materialBlockNoise.OnDisable();
			}
			if( materialValueNoise != null)
			{
				materialValueNoise.OnDisable();
			}
			if( materialPerlinNoise != null)
			{
				materialPerlinNoise.OnDisable();
			}
			if( materialFractalNoise != null)
			{
				materialFractalNoise.OnDisable();
			}
			if( materialCellularNoise != null)
			{
				materialCellularNoise.OnDisable();
			}
			if( materialVoronoiNoise != null)
			{
				materialVoronoiNoise.OnDisable();
			}
			if( materialCirclePattern != null)
			{
				materialCirclePattern.OnDisable();
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
				if( materialType != type)
				{
					Record( "Change Material Type");
					ChangeDynamicMaterial( type);
					materialType = type;
				}
				switch( materialType)
				{
					case MaterialType.kAssets:
					{
						var material = EditorGUILayout.ObjectField( "Material", assetMaterial, typeof( Material), false) as Material;
						if( assetMaterial != material)
						{
							Record( "Change Asset Material");
							assetMaterial = material;
						}
						break;
					}
					default:
					{
						if( materialProperties != null)
						{
							materialProperties.OnGUI();
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
			switch( type)
			{
				case MaterialType.kRandomNoise:
				{
					materialProperties = materialRandomNoise;
					break;
				}
				case MaterialType.kBlockNoise:
				{
					materialProperties = materialBlockNoise;
					break;
				}
				case MaterialType.kValueNoise:
				{
					materialProperties = materialValueNoise;
					break;
				}
				case MaterialType.kPerlinNoise:
				{
					materialProperties = materialPerlinNoise;
					break;
				}
				case MaterialType.kFractalNoise:
				{
					materialProperties = materialFractalNoise;
					break;
				}
				case MaterialType.kCellularNoise:
				{
					materialProperties = materialCellularNoise;
					break;
				}
				case MaterialType.kVoronoiNoise:
				{
					materialProperties = materialVoronoiNoise;
					break;
				}
				case MaterialType.kCirclePattern:
				{
					materialProperties = materialCirclePattern;
					break;
				}
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
		[SerializeField]
		MaterialRandomNoise materialRandomNoise;
		[SerializeField]
		MaterialBlockNoise materialBlockNoise;
		[SerializeField]
		MaterialValueNoise materialValueNoise;
		[SerializeField]
		MaterialPerlinNoise materialPerlinNoise;
		[SerializeField]
		MaterialFractalNoise materialFractalNoise;
		[SerializeField]
		MaterialCellularNoise materialCellularNoise;
		[SerializeField]
		MaterialVoronoiNoise materialVoronoiNoise;
		[SerializeField]
		MaterialCirclePattern materialCirclePattern;
		[SerializeField]
		Material assetMaterial;
		
		MaterialBase materialProperties;
		Material dynamicMaterial;
	}
}