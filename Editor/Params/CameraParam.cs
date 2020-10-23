
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	public enum Direction
	{
		kFront,
		kBack,
		kTop,
		kBottom,
		kLeft,
		kRight,
	}
	[System.Serializable]
	public sealed class CameraParam : BaseParam
	{
		public CameraParam() : base( false)
		{
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			OnPUI( "Camera", false, () =>
			{
				Vector3 localPositionValue = EditorGUILayout.Vector3Field( "Position", localPosition);
				if( localPosition.Equals( localPositionValue) == false)
				{
					Record( "Change Position");
					localPosition = localPositionValue;
				}
				Vector3 localRotationValue = EditorGUILayout.Vector3Field( "Rotation", localRotation);
				if( localRotation.Equals( localRotationValue) == false)
				{
					Record( "Change Rotation");
					localRotation = localRotationValue;
				}
				
				Color backgroundColorValue = EditorGUILayout.ColorField( "Background", backgroundColor);
				if( backgroundColor.Equals( backgroundColorValue) == false)
				{
					Record( "Change Background");
					backgroundColor = backgroundColorValue;
				}
				
				int projection = orthographic ? 1 : 0;
				projection = EditorGUILayout.Popup( "Projection", projection, kProjections);
				bool orthographicValue = (projection == 0) ? false : true;
				if( orthographic.Equals( orthographicValue) == false)
				{
					Record( "Change Projection");
					orthographic = orthographicValue;
				}
				
				if( orthographic == false)
				{
					float fieldOfViewValue = EditorGUILayout.Slider( " Field of View", fieldOfView, 1e-5f, 179.0f);
					if( fieldOfView.Equals( fieldOfViewValue) == false)
					{
						Record( "Change Field of View");
						fieldOfView = fieldOfViewValue;
					}
				}
				else
				{
					float orthographicSizeValue = EditorGUILayout.FloatField( "Size", orthographicSize);
					if( orthographicSize.Equals( orthographicSizeValue) == false)
					{
						Record( "Change Size");
						orthographicSize = orthographicSizeValue;
					}
				}
				
				float nearClipPlaneValue = EditorGUILayout.FloatField( "Near Clipping Planes", nearClipPlane);
				if( nearClipPlaneValue < 0.1f)
				{
					nearClipPlaneValue = 0.1f;
				}
				if( nearClipPlaneValue >= 3.402823e+37f)
				{
					nearClipPlaneValue = 3.402823e+37f;
				}
				if( nearClipPlane.Equals( nearClipPlaneValue) == false)
				{
					Record( "Change Near Clipping Planes");
					nearClipPlane = nearClipPlaneValue;
				}
				
				float farClipPlaneValue = EditorGUILayout.FloatField( "Far Clipping Planes", farClipPlane);
				if( farClipPlaneValue < nearClipPlane)
				{
					farClipPlaneValue = nearClipPlane + 0.01f;
				}
				if( farClipPlane.Equals( farClipPlaneValue) == false)
				{
					Record( "Change Far Clipping Planes");
					farClipPlane = farClipPlaneValue;
				}
				
				Rect rectValue = EditorGUILayout.RectField( "Viewport Rect", rect);
				if( rect.Equals( rectValue) == false)
				{
					Record( "Change Far Clipping Planes");
					rect = rectValue;
				}
				EditorGUILayout.BeginHorizontal();
				{
					if( GUILayout.Button( "Front") != false)
					{
						if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
						if( param[ (int)PreParamType.kTransform] is TransformParam transformParam)
						if( param[ (int)PreParamType.kMesh] is MeshParam meshParam)
						{
							ToDirection( Direction.kFront, textureParam, transformParam, meshParam);
						}
					}
					if( GUILayout.Button( "Back") != false)
					{
						if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
						if( param[ (int)PreParamType.kTransform] is TransformParam transformParam)
						if( param[ (int)PreParamType.kMesh] is MeshParam meshParam)
						{
							ToDirection( Direction.kBack, textureParam, transformParam, meshParam);
						}
					}
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				{
					if( GUILayout.Button( "Top") != false)
					{
						if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
						if( param[ (int)PreParamType.kTransform] is TransformParam transformParam)
						if( param[ (int)PreParamType.kMesh] is MeshParam meshParam)
						{
							ToDirection( Direction.kTop, textureParam, transformParam, meshParam);
						}
					}
					if( GUILayout.Button( "Bottom") != false)
					{
						if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
						if( param[ (int)PreParamType.kTransform] is TransformParam transformParam)
						if( param[ (int)PreParamType.kMesh] is MeshParam meshParam)
						{
							ToDirection( Direction.kBottom, textureParam, transformParam, meshParam);
						}
					}
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				{
					if( GUILayout.Button( "Left") != false)
					{
						if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
						if( param[ (int)PreParamType.kTransform] is TransformParam transformParam)
						if( param[ (int)PreParamType.kMesh] is MeshParam meshParam)
						{
							ToDirection( Direction.kLeft, textureParam, transformParam, meshParam);
						}
					}
					if( GUILayout.Button( "Right") != false)
					{
						if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
						if( param[ (int)PreParamType.kTransform] is TransformParam transformParam)
						if( param[ (int)PreParamType.kMesh] is MeshParam meshParam)
						{
							ToDirection( Direction.kRight, textureParam, transformParam, meshParam);
						}
					}
				}
				EditorGUILayout.EndHorizontal();
			});
			return 0;
		}
		public void Apply( Camera camera)
		{
			camera.transform.position = localPosition;
			camera.transform.rotation = Quaternion.Euler( localRotation);
			
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.backgroundColor = backgroundColor;
			camera.orthographic = orthographic;
			if( orthographic == false)
			{
				camera.fieldOfView = fieldOfView;
			}
			else
			{
				camera.orthographicSize = orthographicSize;
			}
			camera.nearClipPlane = nearClipPlane;
			camera.farClipPlane = farClipPlane;
			camera.rect = rect;
		}
		public void ToDirection( Direction direction, TextureParam textureParam, TransformParam transformParam, MeshParam meshParam)
		{
			float aspecct = (float)textureParam.width / (float)textureParam.height;
			Bounds bounds;
				
			if( meshParam.TryGetBoundingBox( out bounds) != false)
			{
				if( orthographic == false)
				{
					switch( direction)
					{
						case Direction.kFront:
						{
							float length;
							float fov;
							
							if( bounds.extents.x < bounds.extents.y)
							{
								length = bounds.extents.y;
								fov = fieldOfView;
							}
							else
							{
								length = bounds.extents.x;
								fov = Camera.VerticalToHorizontalFieldOfView( fieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float z = -(distance + bounds.extents.z - bounds.center.z);
							localPosition = new Vector3( bounds.center.x, bounds.center.y, z);
							localRotation = Vector3.zero;
							nearClipPlane = distance - 0.01f;
							farClipPlane = nearClipPlane + bounds.size.z + 0.02f;
							break;
						}
						case Direction.kBack:
						{
							float length;
							float fov;
							
							if( bounds.extents.x < bounds.extents.y)
							{
								length = bounds.extents.y;
								fov = fieldOfView;
							}
							else
							{
								length = bounds.extents.x;
								fov = Camera.VerticalToHorizontalFieldOfView( fieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float z = distance + bounds.extents.z + bounds.center.z;
							localPosition = new Vector3( bounds.center.x, bounds.center.y, z);
							localRotation = new Vector3( 0, 180, 0);
							nearClipPlane = distance - 0.01f;
							farClipPlane = nearClipPlane + bounds.size.z + 0.02f;
							break;
						}
						case Direction.kTop:
						{
							float length;
							float fov;
							
							if( bounds.extents.x < bounds.extents.z)
							{
								length = bounds.extents.z;
								fov = fieldOfView;
							}
							else
							{
								length = bounds.extents.x;
								fov = Camera.VerticalToHorizontalFieldOfView( fieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float y = distance + bounds.extents.y + bounds.center.y;
							localPosition = new Vector3( bounds.center.x, y, bounds.center.z);
							localRotation = new Vector3( 90, 0, 0);
							nearClipPlane = distance - 0.01f;
							farClipPlane = nearClipPlane + bounds.size.y + 0.02f;
							break;
						}
						case Direction.kBottom:
						{
							float length;
							float fov;
							
							if( bounds.extents.x < bounds.extents.z)
							{
								length = bounds.extents.z;
								fov = fieldOfView;
							}
							else
							{
								length = bounds.extents.x;
								fov = Camera.VerticalToHorizontalFieldOfView( fieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float y = -(distance + bounds.extents.y - bounds.center.y);
							localPosition = new Vector3( bounds.center.x, y, bounds.center.z);
							localRotation = new Vector3( -90, 0, 0);
							nearClipPlane = distance - 0.01f;
							farClipPlane = nearClipPlane + bounds.size.y + 0.02f;
							break;
						}
						case Direction.kLeft:
						{
							float length;
							float fov;
							
							if( bounds.extents.z < bounds.extents.y)
							{
								length = bounds.extents.y;
								fov = fieldOfView;
							}
							else
							{
								length = bounds.extents.z;
								fov = Camera.VerticalToHorizontalFieldOfView( fieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float x = -(distance + bounds.extents.x - bounds.center.x);
							localPosition = new Vector3( x, bounds.center.y, bounds.center.z);
							localRotation = new Vector3( 0, 90, 0);
							nearClipPlane = distance - 0.01f;
							farClipPlane = nearClipPlane + bounds.size.x + 0.02f;
							break;
						}
						case Direction.kRight:
						{
							float length;
							float fov;
							
							if( bounds.extents.z < bounds.extents.y)
							{
								length = bounds.extents.y;
								fov = fieldOfView;
							}
							else
							{
								length = bounds.extents.z;
								fov = Camera.VerticalToHorizontalFieldOfView( fieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float x = distance + bounds.extents.x + bounds.center.x;
							localPosition = new Vector3( x, bounds.center.y, bounds.center.z);
							localRotation = new Vector3( 0, -90, 0);
							nearClipPlane = distance - 0.01f;
							farClipPlane = nearClipPlane + bounds.size.x + 0.02f;
							break;
						}
					}
				}
				else
				{
					const float nearLength = 1.00f;
					
					switch( direction)
					{
						case Direction.kFront:
						{
							localPosition = new Vector3( bounds.center.x, bounds.center.y, -(bounds.extents.z - bounds.center.z + nearLength));
							localRotation = Vector3.zero;
							orthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.x, aspecct), bounds.extents.y) * 1.005f;
							nearClipPlane = nearLength;
							farClipPlane = nearClipPlane + bounds.size.z + 0.001f;
							break;
						}
						case Direction.kBack:
						{
							localPosition = new Vector3( bounds.center.x, bounds.center.y, bounds.extents.z + bounds.center.z + nearLength);
							localRotation = new Vector3( 0, 180, 0);
							orthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.x, aspecct), bounds.extents.y) * 1.005f;
							nearClipPlane = nearLength;
							farClipPlane = nearClipPlane + bounds.size.z + 0.001f;
							break;
						}
						case Direction.kTop:
						{
							localPosition = new Vector3( bounds.center.x, bounds.extents.y + bounds.center.y + nearLength, bounds.center.z);
							localRotation = new Vector3( 90, 0, 0);
							orthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.x, aspecct), bounds.extents.z) * 1.005f;
							nearClipPlane = nearLength;
							farClipPlane = nearClipPlane + bounds.size.y + 0.001f + 100;
							break;
						}
						case Direction.kBottom:
						{
							localPosition = new Vector3( bounds.center.x, -(bounds.extents.y - bounds.center.y + nearLength), bounds.center.z);
							localRotation = new Vector3( -90, 0, 0);
							orthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.x, aspecct), bounds.extents.z) * 1.005f;
							nearClipPlane = nearLength;
							farClipPlane = nearClipPlane + bounds.size.y + 0.001f;
							break;
						}
						case Direction.kLeft:
						{
							localPosition = new Vector3( -(bounds.extents.x - bounds.center.x + nearLength), bounds.center.y, bounds.center.z);
							localRotation = new Vector3( 0, 90, 0);
							orthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.z, aspecct), bounds.extents.y) * 1.005f;
							nearClipPlane = nearLength;
							farClipPlane = nearClipPlane + bounds.size.y + 0.001f;
							break;
						}
						case Direction.kRight:
						{
							localPosition = new Vector3( bounds.extents.x - bounds.center.x + nearLength, bounds.center.y, bounds.center.z);
							localRotation = new Vector3( 0, -90, 0);
							orthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.z, aspecct), bounds.extents.y) * 1.005f;
							nearClipPlane = nearLength;
							farClipPlane = nearClipPlane + bounds.size.y + 0.001f;
							break;
						}
					}
				}
				transformParam.localPosition = Vector3.zero;
				transformParam.localRotation = Vector3.zero;
				transformParam.localScale = Vector3.one;
				
				nearClipPlane -= 10.0f;
				farClipPlane += 100.0f;
				
				if( nearClipPlane < 0.1f)
				{
					nearClipPlane = 0.1f;
				}
				if( nearClipPlane >= 3.402823e+37f)
				{
					nearClipPlane = 3.402823e+37f;
				}
				if( farClipPlane < nearClipPlane)
				{
					farClipPlane = nearClipPlane + 0.01f;
				}
			}
		}
		static readonly string[] kProjections = new string[]
		{
			"Perspective",
			"Orthographic"
		};
		
		[SerializeField]
		Vector3 localPosition = new Vector3( 0, 0, -1);
		[SerializeField]
		Vector3 localRotation = Vector3.zero;
		[SerializeField]
		Color backgroundColor = Color.clear;
		[SerializeField]
		bool orthographic = true;
		[SerializeField]
		float orthographicSize = 0.5f;
		[SerializeField]
		float fieldOfView = Camera.HorizontalToVerticalFieldOfView( 45.0f, 1.0f);
		[SerializeField]
		float nearClipPlane = 0.03f;
		[SerializeField]
		float farClipPlane = 1000.0f;
		[SerializeField]
		Rect rect = new Rect( 0, 0, 1, 1);
	}
}