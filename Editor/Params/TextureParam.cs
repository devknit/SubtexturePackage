
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
			OnPUI( "Screen Texture", false, null, () =>
			{
				bool forceSquareValue = EditorGUILayout.Toggle( "Force square", m_ForceSquare);
				if( m_ForceSquare.Equals( forceSquareValue) == false)
				{
					Record( "Change Force square");
					if( forceSquareValue != false)
					{
						m_Width = m_Size;
						m_Height = m_Size;
					}
					m_ForceSquare = forceSquareValue;
				}
				if( m_ForceSquare == false)
				{
					EditorGUI.BeginChangeCheck();
					int widthValue = EditorGUILayout.IntSlider( "Width", m_Width, 4, 8196);
					if( EditorGUI.EndChangeCheck() != false && m_Width.Equals( widthValue) == false)
					{
						Record( "Change Width");
						m_Width = widthValue;
					}
					EditorGUI.BeginChangeCheck();
					int heightValue = EditorGUILayout.IntSlider( "Height", m_Height, 4, 8196);
					if( EditorGUI.EndChangeCheck() != false && m_Height.Equals( heightValue) == false)
					{
						Record( "Change Height");
						m_Height = heightValue;
						m_Size = heightValue;
					}
				}
				else
				{
					EditorGUI.BeginChangeCheck();
					int sizeValue = EditorGUILayout.IntSlider( "Size", m_Size, 4, 8196);
					if( EditorGUI.EndChangeCheck() != false && m_Size.Equals( sizeValue) == false)
					{
						Record( "Change Size");
						m_Size = sizeValue;
						m_Width = m_Size;
						m_Height = m_Size;
					}
				}
			});
			return 0;
		}
		float GetScaleFactor()
		{
			float scaleFacX = Mathf.Max( Mathf.Min( m_Width, 1024), m_Width) / m_Width;
			float scaleFacY = Mathf.Max( Mathf.Min( m_Height, 1024), m_Height) / m_Height;
			return Mathf.Min( scaleFacX, scaleFacY) * EditorGUIUtility.pixelsPerPoint;
		}
		public Rect RenderRect
		{
			get
			{
				var rect = new Rect( 0, 0, m_Width, m_Height);
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
		public int Width
		{
			get{ return m_Width; }
		}
		public int Height
		{
			get{ return m_Height; }
		}
		public int Size
		{
			get{ return m_Size; }
			set
			{
				m_Size = Mathf.Max( 4, value);
				m_Height = m_Size;
				
				if( m_ForceSquare != false)
				{
					m_Width = m_Size;
				}
			}
		}
		[SerializeField]
		bool m_ForceSquare = true;
		[SerializeField]
		int m_Width = 512;
		[SerializeField]
		int m_Height = 512;
		[SerializeField]
		int m_Size = 512;
	}
}