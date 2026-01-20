
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	public enum CameraPreset
	{
		kFit,
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
		public void MoveControl( Camera camera, Vector2 value)
		{
			var translate = 
				(camera.transform.right * -value.x)
				+ (camera.transform.up * value.y);
			
			m_LocalPosition += translate;
			m_LookAtPosition += translate;
			camera.transform.localPosition = m_LocalPosition;
			camera.transform.LookAt( m_LookAtPosition);
		}
		public void OrbitControl( Camera camera, Vector2 value)
		{
			camera.transform.RotateAround( m_LookAtPosition, Vector3.up, value.x);
			camera.transform.RotateAround( m_LookAtPosition, camera.transform.right, value.y);
			m_LocalPosition = camera.transform.localPosition;
			m_LocalRotation = camera.transform.localEulerAngles;
		}
		public void ZoomControl( Camera camera, float value, BaseParam[] param)
		{
			if( m_Orthographic == false)
			{
				Vector3 direction = m_LookAtPosition - m_LocalPosition;
				float distance = Mathf.Max( 1e-4f, direction.magnitude + value * 0.5f);
				camera.transform.localPosition = m_LocalPosition = m_LookAtPosition - (direction.normalized * distance);
			}
			else if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
			{
				float size = textureParam.Size;
				size += value * 2.0f;
				textureParam.Size = Mathf.RoundToInt( size);
			}
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			OnPUI( "Camera", false, 
				() =>
				{
					m_LocalPosition = new Vector3( 0, 0, -1);
					m_LocalRotation = Vector3.zero;
					m_LookAtPosition = Vector3.zero;
					m_BackgroundColor = Color.clear;
					m_Orthographic = true;
					m_OrthographicSize = 1.0f;
					m_FieldOfView = Camera.HorizontalToVerticalFieldOfView( 45.0f, 1.0f);
					m_NearClipPlane = 0.03f;
					m_FarClipPlane = 1000.0f;
					m_ViewportRect = new Rect( 0, 0, 1, 1);
				},
				() =>
				{
					int projection = m_Orthographic ? 1 : 0;
					projection = EditorGUILayout.Popup( "Projection", projection, kProjections);
					bool orthographicValue = (projection == 0) ? false : true;
					if( m_Orthographic.Equals( orthographicValue) == false)
					{
						Record( "Change Projection");
						m_Orthographic = orthographicValue;
						m_LocalPosition = new Vector3( 0, 0, -1);
						m_LocalRotation = Vector3.zero;
						m_LookAtPosition = Vector3.zero;
					}
					if( m_Orthographic == false)
					{
						float fieldOfViewValue = EditorGUILayout.Slider( " Field of View", m_FieldOfView, 1e-5f, 179.0f);
						if( m_FieldOfView.Equals( fieldOfViewValue) == false)
						{
							Record( "Change Field of View");
							m_FieldOfView = fieldOfViewValue;
						}
					}
					else if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
					{
						m_OrthographicSize = textureParam.Size;
					}
					
					Color backgroundColorValue = EditorGUILayout.ColorField( "Background", m_BackgroundColor);
					if( m_BackgroundColor.Equals( backgroundColorValue) == false)
					{
						Record( "Change Background");
						m_BackgroundColor = backgroundColorValue;
					}
					
					float nearClipPlaneValue = EditorGUILayout.FloatField( "Near Clipping Planes", m_NearClipPlane);
					if( nearClipPlaneValue < 0.1f)
					{
						nearClipPlaneValue = 0.1f;
					}
					if( nearClipPlaneValue >= 3.402823e+37f)
					{
						nearClipPlaneValue = 3.402823e+37f;
					}
					if( m_NearClipPlane.Equals( nearClipPlaneValue) == false)
					{
						Record( "Change Near Clipping Planes");
						m_NearClipPlane = nearClipPlaneValue;
					}
					
					float farClipPlaneValue = EditorGUILayout.FloatField( "Far Clipping Planes", m_FarClipPlane);
					if( farClipPlaneValue < m_NearClipPlane)
					{
						farClipPlaneValue = m_NearClipPlane + 0.01f;
					}
					if( m_FarClipPlane.Equals( farClipPlaneValue) == false)
					{
						Record( "Change Far Clipping Planes");
						m_FarClipPlane = farClipPlaneValue;
					}
					
					Rect viewportRectValue = EditorGUILayout.RectField( "Viewport Rect", m_ViewportRect);
					if( m_ViewportRect.Equals( viewportRectValue) == false)
					{
						Record( "Change Far Clipping Planes");
						m_ViewportRect = viewportRectValue;
					}
					
					Vector3 lookAtPositionValue = EditorGUILayout.Vector3Field( "LookAt", m_LookAtPosition);
					if( m_LookAtPosition.Equals( lookAtPositionValue) == false)
					{
						Record( "Change LookAt");
						m_LookAtPosition = lookAtPositionValue;
					}
					Vector3 localPositionValue = EditorGUILayout.Vector3Field( "Position", m_LocalPosition);
					if( m_LocalPosition.Equals( localPositionValue) == false)
					{
						Record( "Change Position");
						m_LocalPosition = localPositionValue;
					}
					Vector3 localRotationValue = EditorGUILayout.Vector3Field( "Rotation", m_LocalRotation);
					if( m_LocalRotation.Equals( localRotationValue) == false)
					{
						Record( "Change Rotation");
						m_LocalRotation = localRotationValue;
					}
				});
			return 0;
		}
		public void Apply( Camera camera)
		{
			camera.transform.position = m_LocalPosition;
			camera.transform.rotation = Quaternion.Euler( m_LocalRotation);
			
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.backgroundColor = m_BackgroundColor;
			camera.orthographic = m_Orthographic;
			if( m_Orthographic == false)
			{
				camera.fieldOfView = m_FieldOfView;
			}
			else
			{
				camera.orthographicSize = m_OrthographicSize * 0.5f + 0.005f;
			}
			camera.nearClipPlane = m_NearClipPlane;
			camera.farClipPlane = m_FarClipPlane;
			camera.rect = m_ViewportRect;
		}
		public void ToPreset( CameraPreset preset, TextureParam textureParam, TransformParam transformParam, MeshParam meshParam)
		{
			float aspecct = (float)textureParam.Width / (float)textureParam.Height;
			Bounds bounds;
				
			if( meshParam.TryGetBoundingBox( out bounds) != false)
			{
				if( m_Orthographic == false)
				{
					switch( preset)
					{
						case CameraPreset.kFit:
						{
							float length;
							float fov;
							
							if( bounds.extents.x < bounds.extents.y)
							{
								length = bounds.extents.y;
								fov = m_FieldOfView;
							}
							else
							{
								length = bounds.extents.x;
								fov = Camera.VerticalToHorizontalFieldOfView( m_FieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad) + bounds.extents.z;
							if( distance <= 0.0f)
							{
								return;
							}
							Vector3 direction = m_LookAtPosition - m_LocalPosition;
							
							if( Mathf.Approximately( direction.magnitude, 0.0f) != false)
							{
								return;
							}
							m_LookAtPosition = bounds.center;
							direction = Vector3.Normalize( direction);
							m_LocalPosition = m_LookAtPosition - direction * distance;
							m_LocalRotation = Quaternion.LookRotation( direction).eulerAngles;
							m_NearClipPlane = distance - 0.01f;
							m_FarClipPlane = m_NearClipPlane + bounds.size.z + 0.02f;
							break;
						}
						case CameraPreset.kFront:
						{
							float length;
							float fov;
							
							if( bounds.extents.x < bounds.extents.y)
							{
								length = bounds.extents.y;
								fov = m_FieldOfView;
							}
							else
							{
								length = bounds.extents.x;
								fov = Camera.VerticalToHorizontalFieldOfView( m_FieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float z = -(distance + bounds.extents.z - bounds.center.z);
							m_LocalPosition = new Vector3( bounds.center.x, bounds.center.y, z);
							m_LocalRotation = Vector3.zero;
							m_LookAtPosition = bounds.center;
							m_NearClipPlane = distance - 0.01f;
							m_FarClipPlane = m_NearClipPlane + bounds.size.z + 0.02f;
							break;
						}
						case CameraPreset.kBack:
						{
							float length;
							float fov;
							
							if( bounds.extents.x < bounds.extents.y)
							{
								length = bounds.extents.y;
								fov = m_FieldOfView;
							}
							else
							{
								length = bounds.extents.x;
								fov = Camera.VerticalToHorizontalFieldOfView( m_FieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float z = distance + bounds.extents.z + bounds.center.z;
							m_LocalPosition = new Vector3( bounds.center.x, bounds.center.y, z);
							m_LocalRotation = new Vector3( 0, 180, 0);
							m_LookAtPosition = bounds.center;
							m_NearClipPlane = distance - 0.01f;
							m_FarClipPlane = m_NearClipPlane + bounds.size.z + 0.02f;
							break;
						}
						case CameraPreset.kTop:
						{
							float length;
							float fov;
							
							if( bounds.extents.x < bounds.extents.z)
							{
								length = bounds.extents.z;
								fov = m_FieldOfView;
							}
							else
							{
								length = bounds.extents.x;
								fov = Camera.VerticalToHorizontalFieldOfView( m_FieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float y = distance + bounds.extents.y + bounds.center.y;
							m_LocalPosition = new Vector3( bounds.center.x, y, bounds.center.z);
							m_LocalRotation = new Vector3( 90, 0, 0);
							m_LookAtPosition = bounds.center;
							m_NearClipPlane = distance - 0.01f;
							m_FarClipPlane = m_NearClipPlane + bounds.size.y + 0.02f;
							break;
						}
						case CameraPreset.kBottom:
						{
							float length;
							float fov;
							
							if( bounds.extents.x < bounds.extents.z)
							{
								length = bounds.extents.z;
								fov = m_FieldOfView;
							}
							else
							{
								length = bounds.extents.x;
								fov = Camera.VerticalToHorizontalFieldOfView( m_FieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float y = -(distance + bounds.extents.y - bounds.center.y);
							m_LocalPosition = new Vector3( bounds.center.x, y, bounds.center.z);
							m_LocalRotation = new Vector3( -90, 0, 0);
							m_LookAtPosition = bounds.center;
							m_NearClipPlane = distance - 0.01f;
							m_FarClipPlane = m_NearClipPlane + bounds.size.y + 0.02f;
							break;
						}
						case CameraPreset.kLeft:
						{
							float length;
							float fov;
							
							if( bounds.extents.z < bounds.extents.y)
							{
								length = bounds.extents.y;
								fov = m_FieldOfView;
							}
							else
							{
								length = bounds.extents.z;
								fov = Camera.VerticalToHorizontalFieldOfView( m_FieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float x = -(distance + bounds.extents.x - bounds.center.x);
							m_LocalPosition = new Vector3( x, bounds.center.y, bounds.center.z);
							m_LocalRotation = new Vector3( 0, 90, 0);
							m_LookAtPosition = bounds.center;
							m_NearClipPlane = distance - 0.01f;
							m_FarClipPlane = m_NearClipPlane + bounds.size.x + 0.02f;
							break;
						}
						case CameraPreset.kRight:
						{
							float length;
							float fov;
							
							if( bounds.extents.z < bounds.extents.y)
							{
								length = bounds.extents.y;
								fov = m_FieldOfView;
							}
							else
							{
								length = bounds.extents.z;
								fov = Camera.VerticalToHorizontalFieldOfView( m_FieldOfView, aspecct);
							}
							float distance = length / Mathf.Tan( fov * 0.5f * Mathf.Deg2Rad);
							float x = distance + bounds.extents.x + bounds.center.x;
							m_LocalPosition = new Vector3( x, bounds.center.y, bounds.center.z);
							m_LocalRotation = new Vector3( 0, -90, 0);
							m_LookAtPosition = bounds.center;
							m_NearClipPlane = distance - 0.01f;
							m_FarClipPlane = m_NearClipPlane + bounds.size.x + 0.02f;
							break;
						}
					}
				}
				else
				{
					const float nearLength = 1.00f;
					
					switch( preset)
					{
						case CameraPreset.kFront:
						{
							m_LocalPosition = new Vector3( bounds.center.x, bounds.center.y, -(bounds.extents.z - bounds.center.z + nearLength));
							m_LocalRotation = Vector3.zero;
							m_LookAtPosition = bounds.center;
							m_OrthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.x, aspecct), bounds.extents.y);
							m_NearClipPlane = nearLength;
							m_FarClipPlane = m_NearClipPlane + bounds.size.z + 0.001f;
							break;
						}
						case CameraPreset.kBack:
						{
							m_LocalPosition = new Vector3( bounds.center.x, bounds.center.y, bounds.extents.z + bounds.center.z + nearLength);
							m_LocalRotation = new Vector3( 0, 180, 0);
							m_LookAtPosition = bounds.center;
							m_OrthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.x, aspecct), bounds.extents.y);
							m_NearClipPlane = nearLength;
							m_FarClipPlane = m_NearClipPlane + bounds.size.z + 0.001f;
							break;
						}
						case CameraPreset.kTop:
						{
							m_LocalPosition = new Vector3( bounds.center.x, bounds.extents.y + bounds.center.y + nearLength, bounds.center.z);
							m_LocalRotation = new Vector3( 90, 0, 0);
							m_LookAtPosition = bounds.center;
							m_OrthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.x, aspecct), bounds.extents.z);
							m_NearClipPlane = nearLength;
							m_FarClipPlane = m_NearClipPlane + bounds.size.y + 0.001f + 100;
							break;
						}
						case CameraPreset.kBottom:
						{
							m_LocalPosition = new Vector3( bounds.center.x, -(bounds.extents.y - bounds.center.y + nearLength), bounds.center.z);
							m_LocalRotation = new Vector3( -90, 0, 0);
							m_LookAtPosition = bounds.center;
							m_OrthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.x, aspecct), bounds.extents.z);
							m_NearClipPlane = nearLength;
							m_FarClipPlane = m_NearClipPlane + bounds.size.y + 0.001f;
							break;
						}
						case CameraPreset.kLeft:
						{
							m_LocalPosition = new Vector3( -(bounds.extents.x - bounds.center.x + nearLength), bounds.center.y, bounds.center.z);
							m_LocalRotation = new Vector3( 0, 90, 0);
							m_LookAtPosition = bounds.center;
							m_OrthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.z, aspecct), bounds.extents.y);
							m_NearClipPlane = nearLength;
							m_FarClipPlane = m_NearClipPlane + bounds.size.y + 0.001f;
							break;
						}
						case CameraPreset.kRight:
						{
							m_LocalPosition = new Vector3( bounds.extents.x - bounds.center.x + nearLength, bounds.center.y, bounds.center.z);
							m_LocalRotation = new Vector3( 0, -90, 0);
							m_LookAtPosition = bounds.center;
							m_OrthographicSize = Mathf.Max( Camera.HorizontalToVerticalFieldOfView( bounds.extents.z, aspecct), bounds.extents.y);
							m_NearClipPlane = nearLength;
							m_FarClipPlane = m_NearClipPlane + bounds.size.y + 0.001f;
							break;
						}
					}
				}
				transformParam.LocalPosition = Vector3.zero;
				transformParam.LocalRotation = Vector3.zero;
				transformParam.LocalScale = Vector3.one;
				
				m_NearClipPlane -= 10.0f;
				m_FarClipPlane += 100.0f;
				
				if( m_NearClipPlane < 0.1f)
				{
					m_NearClipPlane = 0.1f;
				}
				if( m_NearClipPlane >= 3.402823e+37f)
				{
					m_NearClipPlane = 3.402823e+37f;
				}
				if( m_FarClipPlane < m_NearClipPlane)
				{
					m_FarClipPlane = m_NearClipPlane + 0.01f;
				}
			}
		}
		public Vector2 GetSurfaceArea( float aspect)
		{
			float distance = Mathf.Abs( Vector3.Distance( m_LookAtPosition, m_LocalPosition));
			float xFieldOfView = Camera.VerticalToHorizontalFieldOfView( m_FieldOfView, aspect);
			return new Vector2( 
				distance * Mathf.Tan( xFieldOfView * 0.5f * Mathf.Deg2Rad) * 2.0f,
				distance * Mathf.Tan( m_FieldOfView * 0.5f * Mathf.Deg2Rad) * 2.0f);
		}
		static readonly string[] kProjections = new string[]
		{
			"Perspective",
			"Orthographic"
		};
		[SerializeField]
		Color m_BackgroundColor = Color.clear;
		[SerializeField]
		bool m_Orthographic = true;
		[SerializeField]
		float m_OrthographicSize = 1.0f;
		[SerializeField]
		float m_FieldOfView = Camera.HorizontalToVerticalFieldOfView( 45.0f, 1.0f);
		[SerializeField]
		float m_NearClipPlane = 0.03f;
		[SerializeField]
		float m_FarClipPlane = 1000.0f;
		[SerializeField]
		Rect m_ViewportRect = new Rect( 0, 0, 1, 1);
		[SerializeField]
		Vector3 m_LocalPosition = new Vector3( 0, 0, -1);
		[SerializeField]
		Vector3 m_LocalRotation = Vector3.zero;
		[SerializeField]
		Vector3 m_LookAtPosition = Vector3.zero;
	}
}