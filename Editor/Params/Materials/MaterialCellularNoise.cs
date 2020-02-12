
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialCellularNoise : MaterialBaseUvProperties
	{
		public override bool OnGUI()
		{
			bool ret = base.OnGUI();
			
			float timeValue = EditorGUILayout.FloatField( "Time", time);
			if( time.Equals( timeValue) == false)
			{
				Record( "Change Time");
				time = timeValue;
				ret = true;
			}
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			base.OnUpdateMaterial();
			materialCache.SetFloat( "_TimePosition", time);
		}
		protected override string GetShaderGuid()
		{
			return "b68038a3be357504db49ab1958ec5ffe";
		}
		
		[SerializeField]
		float time = 0.0f;
	}
}