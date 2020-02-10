
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public sealed class CameraParam : BaseParam
	{
		public CameraParam() : base( "Camera")
		{
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
		protected override void OnParamGUI()
		{
			localPosition = EditorGUILayout.Vector3Field( "Position", localPosition);
			
			localRotation = EditorGUILayout.Vector3Field( "Rotation", localRotation);
			
			backgroundColor = EditorGUILayout.ColorField( "Background", backgroundColor);
			
			int projection = orthographic ? 1 : 0;
			projection = EditorGUILayout.Popup( "Projection", projection, kProjections);
			orthographic = (projection == 0) ? false : true;
			
			if( orthographic == false)
			{
				fieldOfView = EditorGUILayout.Slider( " Field of View", fieldOfView, 1e-5f, 179.0f);
			}
			else
			{
				orthographicSize = EditorGUILayout.FloatField( "Size", orthographicSize);
			}
			
			nearClipPlane = EditorGUILayout.FloatField( "Near Clipping Planes", nearClipPlane);
			if( nearClipPlane < 0.01)
			{
				nearClipPlane = 0.01f;
			}
			if( nearClipPlane >= 3.402823e+37f)
			{
				nearClipPlane = 3.402823e+37f;
			}
			
			farClipPlane = EditorGUILayout.FloatField( "Far Clipping Planes", farClipPlane);
			if( farClipPlane < nearClipPlane)
			{
				farClipPlane = nearClipPlane + 0.01f;
			}
			
			rect = EditorGUILayout.RectField( "Viewport Rect", rect);
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