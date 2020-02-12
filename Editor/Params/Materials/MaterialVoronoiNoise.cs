
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class MaterialVoronoiNoise : MaterialBaseUvProperties
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
			int swizzleValue = EditorGUILayout.Popup( "Swizzle", swizzle, kSwizzleLabels);
			if( swizzle.Equals( swizzleValue) == false)
			{
				Record( "Change Swizzle");
				swizzle = swizzleValue;
				ret = true;
			}
			Vector3 colorScaleValue = EditorGUILayout.Vector3Field( "Color Scale", colorScale);
			if( colorScale.Equals( colorScaleValue) == false)
			{
				Record( "Change Color Scale");
				colorScale = colorScaleValue;
				ret = true;
			}
			Vector2 smoothEdgesValue = EditorGUILayout.Vector2Field( "Smooth Edges", smoothEdges);
			if( smoothEdges.Equals( smoothEdgesValue) == false)
			{
				Record( "Change Smooth Edges");
				smoothEdges = smoothEdgesValue;
				ret = true;
			}
			return ret;
		}
		public override void OnUpdateMaterial()
		{
			base.OnUpdateMaterial();
			materialCache.SetFloat( "_TimePosition", time);
			materialCache.SetInt( "_Swizzle", swizzle);
			materialCache.SetVector( "_ColorScale", new Vector4( colorScale.x, colorScale.y, colorScale.z, 1));
			materialCache.SetVector( "_SmoothEdges", new Vector4( smoothEdges.x, smoothEdges.y, 0, 0));
		}
		protected override string GetShaderGuid()
		{
			return "25a82b0086028984c9b934ab9644b05d";
		}
		
		static readonly string[] kSwizzleLabels = new string[]
		{
			"Gray 1",
			"Gray 2",
			"Gradation 1",
			"Gradation 2",
			"Red 1",
			"Red 2",
			"Green 1",
			"Green 2",
			"Blue 1",
			"Blue 2"
		};
		
		[SerializeField]
		float time = 0.0f;
		[SerializeField]
		int swizzle = 0;
		[SerializeField]
		Vector3 colorScale = Vector3.one;
		[SerializeField]
		Vector2 smoothEdges = new Vector2( 0.02f, 0.04f);
	}
}