
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public class TextureParam : BaseParam
	{
		public TextureParam() : base( "Texture")
		{
		}
		protected override void OnParamGUI()
		{
			forceSquare = EditorGUILayout.Toggle( "Force square", forceSquare);
			if( forceSquare == false)
			{
				width = EditorGUILayout.IntPopup( "Width", width, kResolutionLabels, kResolutions);
				height = EditorGUILayout.IntPopup( "Height", height, kResolutionLabels, kResolutions);
			}
			else
			{
				int size = width > height ? width : height;
				size = EditorGUILayout.IntPopup( "Size", size, kResolutionLabels, kResolutions);
				width = size;
				height = size;
			}
		}
		static readonly int[] kResolutions = new int[]
		{
			1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096
		};
		static readonly string[] kResolutionLabels = new string[]
		{
			"1", "2", "4", "8", "16", "32", "64", "128", "256", "512", "1024", "2048", "4096"
		};
		float GetScaleFactor()
	    {
	        float scaleFacX = Mathf.Max( Mathf.Min( width * 2, 1024), width) / width;
	        float scaleFacY = Mathf.Max( Mathf.Min( height * 2, 1024), height) / height;
	        return Mathf.Min( scaleFacX, scaleFacY) * EditorGUIUtility.pixelsPerPoint;
	    }
		public Rect RenderRect
		{
			get
			{
				var rect = new Rect( 0, 0, width, height);
				float scaleFac = GetScaleFactor();
				
				if( scaleFac != 1.0f)
				{
					scaleFac = 1.0f / scaleFac;
					rect.width *= scaleFac;
					rect.height *= scaleFac;
				}
				return rect;
			}
		}
		
		[SerializeField]
		bool forceSquare = true;
		[SerializeField]
		public int width = 256;
		[SerializeField]
		public int height = 256;
	}
}