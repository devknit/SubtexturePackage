
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
	//	kProceduralCircle,
	//	kProceduralRing,
	}
	[System.Serializable]
	public sealed class MaterialParam : BaseParam
	{
		public override void OnEnable( EditorWindow window, bool opened)
		{
			base.OnEnable( window, opened);
			ChangeDynamicMaterial( materialType);
		}
		public override void OnDisable()
		{
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
					ChangeDynamicMaterial( type);
					materialType = type;
				}
				switch( materialType)
				{
					case MaterialType.kAssets:
					{
						assetMaterial = EditorGUILayout.ObjectField( "Material", assetMaterial, typeof( Material), false) as Material;
						break;
					}
				#if false
					case MaterialType.kProceduralCircle:
					{
						break;
					}
					case MaterialType.kProceduralRing:
					{
						break;
					}
				#endif
				}
			});
		}
		void ChangeDynamicMaterial( MaterialType type)
		{
			string newShaderGuid = string.Empty;
			string newShaderPath = string.Empty;
			
			if( dynamicMaterial != null)
			{
				Material.DestroyImmediate( dynamicMaterial);
				dynamicMaterial = null;
			}
			switch( type)
			{
				case MaterialType.kRandomNoise:
				{
					newShaderGuid = "a0bc3d9f3b361864d851388e6a7071ec";
					break;
				}
				case MaterialType.kBlockNoise:
				{
					newShaderGuid = "4dde2b0e60992e84082aaa19cc0edef4";
					break;
				}
				case MaterialType.kValueNoise:
				{
					newShaderGuid = "c213afc6e8574a540b30cd6ef7c038ef";
					break;
				}
				case MaterialType.kPerlinNoise:
				{
					newShaderGuid = "b2423f437ef759643be61fd6902a848e";
					break;
				}
				case MaterialType.kFractalNoise:
				{
					newShaderGuid = "b649d9c025e729849bf5138c0dad8d6b";
					break;
				}
				case MaterialType.kCellularNoise:
				{
					newShaderGuid = "b68038a3be357504db49ab1958ec5ffe";
					break;
				}
				case MaterialType.kVoronoiNoise:
				{
					newShaderGuid = "25a82b0086028984c9b934ab9644b05d";
					break;
				}
			}
			if( string.IsNullOrEmpty( newShaderGuid) == false)
			{
				newShaderPath = AssetDatabase.GUIDToAssetPath( newShaderGuid);
			}
			if( string.IsNullOrEmpty( newShaderPath) == false)
			{
				if( AssetDatabase.LoadAssetAtPath<Shader>( newShaderPath) is Shader shader)
				{
					dynamicMaterial = new Material( shader);
				}
			}
		}
		public Material RenderMaterial
		{
			get{ return (materialType == MaterialType.kAssets)? assetMaterial : dynamicMaterial; }
		}
		
		[SerializeField]
		public MaterialType materialType = MaterialType.kAssets;
		[SerializeField]
		Material assetMaterial;
		
		Material dynamicMaterial;
	}
}