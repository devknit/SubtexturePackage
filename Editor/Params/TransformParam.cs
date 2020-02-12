
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Subtexture
{
	[System.Serializable]
	public sealed class TransformParam : BaseParam
	{
		public override void OnEnable( Window window, bool opened)
		{
			base.OnEnable( window, opened);
		}
		public override void OnDisable()
		{
			base.OnDisable();
		}
		public override void OnGUI()
		{
			OnPUI( "Transform", () =>
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
				Vector3 localScaleValue = EditorGUILayout.Vector3Field( "Scale", localScale);
				if( localScale.Equals( localScaleValue) == false)
				{
					Record( "Change Scale");
					localScale = localScaleValue;
				}
			});
		}
		public Matrix4x4 LocalMatrix
		{
			get
			{
				return Matrix4x4.TRS( localPosition, Quaternion.Euler( localRotation), localScale);
			}
		}
		
		[SerializeField]
		Vector3 localPosition = Vector3.zero;
		[SerializeField]
		Vector3 localRotation = Vector3.zero;
		[SerializeField]
		Vector3 localScale = Vector3.one;
	}
}