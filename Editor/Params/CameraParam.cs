
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public sealed class CameraParam : BaseParam
	{
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
		public override void OnGUI()
		{
			OnPUI( "Camera", () =>
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
				if( nearClipPlaneValue < 0.01)
				{
					nearClipPlaneValue = 0.01f;
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
			});
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
		float fieldOfView = 60.0f;
		[SerializeField]
		float nearClipPlane = 0.03f;
		[SerializeField]
		float farClipPlane = 1000.0f;
		[SerializeField]
		Rect rect = new Rect( 0, 0, 1, 1);
	}
}