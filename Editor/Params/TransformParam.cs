
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Subtexture
{
	[System.Serializable]
	public sealed class TransformParam : BaseParam
	{
		public override void OnEnable( EditorWindow window, bool opened)
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
				localPosition = EditorGUILayout.Vector3Field( "Position", localPosition);
				localRotation = EditorGUILayout.Vector3Field( "Rotation", localRotation);
				localScale = EditorGUILayout.Vector3Field( "Scale", localScale);
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