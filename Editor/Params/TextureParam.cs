
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public sealed class TextureParam : BaseParam
	{
		public override void OnGUI()
		{
			OnPUI( "Texture", () =>
			{
				bool forceSquareValue = EditorGUILayout.Toggle( "Force square", forceSquare);
				if( forceSquare.Equals( forceSquareValue) == false)
				{
					Record( "Change Force square");
					forceSquare = forceSquareValue;
				}
				if( forceSquare == false)
				{
					int widthValue = EditorGUILayout.IntPopup( "Width", width, kResolutionLabels, kResolutions);
					if( width.Equals( widthValue) == false)
					{
						Record( "Change Width");
						width = widthValue;
					}
					int heightValue = EditorGUILayout.IntPopup( "Height", height, kResolutionLabels, kResolutions);
					if( height.Equals( heightValue) == false)
					{
						Record( "Change Height");
						height = heightValue;
					}
				}
				else
				{
					int size = width > height ? width : height;
					int sizeValue = EditorGUILayout.IntPopup( "Size", size, kResolutionLabels, kResolutions);
					if( size.Equals( sizeValue) == false)
					{
						Record( "Change Size");
						width = size;
						height = size;
					}
				}
			});
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