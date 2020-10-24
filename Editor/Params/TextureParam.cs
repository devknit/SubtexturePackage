
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public sealed class TextureParam : BaseParam
	{
		public TextureParam() : base( true)
		{
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			OnPUI( "Screen Texture", false, () =>
			{
				bool forceSquareValue = EditorGUILayout.Toggle( "Force square", forceSquare);
				if( forceSquare.Equals( forceSquareValue) == false)
				{
					Record( "Change Force square");
					if( forceSquareValue != false)
					{
						width = size;
						height = size;
					}
					forceSquare = forceSquareValue;
				}
				if( forceSquare == false)
				{
					int widthValue = EditorGUILayout.IntPopup( "Width", width, kResolutionLabels, kResolutions);
					if( width.Equals( widthValue) == false)
					{
						Record( "Change Width");
						width = widthValue;
						size = widthValue;
					}
					int heightValue = EditorGUILayout.IntPopup( "Height", height, kResolutionLabels, kResolutions);
					if( height.Equals( heightValue) == false)
					{
						Record( "Change Height");
						height = heightValue;
						size = heightValue;
					}
				}
				else
				{
					int sizeValue = EditorGUILayout.IntPopup( "Size", size, kResolutionLabels, kResolutions);
					if( size.Equals( sizeValue) == false)
					{
						Record( "Change Size");
						size = sizeValue;
						width = size;
						height = size;
					}
				}
			});
			return 0;
		}
		static readonly int[] kResolutions = new int[]
		{
			1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192
		};
		static readonly string[] kResolutionLabels = new string[]
		{
			"1", "2", "4", "8", "16", "32", "64", "128", "256", "512", "1024", "2048", "4096", "8192"
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
		public int width = 512;
		[SerializeField]
		public int height = 512;
		[SerializeField]
		public int size = 512;
	}
}