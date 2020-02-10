
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Subtexture
{
	public enum MaterialType
	{
		kAssets,
	//	kProceduralCircle,
	//	kProceduralRing,
	}
	[System.Serializable]
	public sealed class MaterialParam : BaseParam
	{
		public MaterialParam() : base( "Material")
		{
		}
		protected override void OnParamGUI()
		{
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
		}
		public Material RenderMaterial
		{
			get{ return assetMaterial; }
		}
		
		[SerializeField]
		public MaterialType materialType = MaterialType.kAssets;
		[SerializeField]
		Material assetMaterial;
	}
}