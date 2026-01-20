
using UnityEngine;
using UnityEditor;

namespace Subtexture
{
	[System.Serializable]
	public sealed class TransformParam : BaseParam
	{
		public TransformParam() : base( false)
		{
		}
		public override void OnEnable( Window window)
		{
			base.OnEnable( window);
		}
		public override void OnDisable()
		{
			base.OnDisable();
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			
			OnPUI( "Transform", false, 
				() =>
				{
					m_LocalPosition = Vector3.zero;
					m_LocalRotation = Vector3.zero;
					m_LocalScale = Vector3.one;
				}, 
				() =>
				{
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
					Vector3 localScaleValue = EditorGUILayout.Vector3Field( "Scale", m_LocalScale);
					if( m_LocalScale.Equals( localScaleValue) == false)
					{
						Record( "Change Scale");
						m_LocalScale = localScaleValue;
					}
				});
			return 0;
		}
		public Matrix4x4 LocalMatrix
		{
			get
			{
				return Matrix4x4.TRS( m_LocalPosition, Quaternion.Euler( m_LocalRotation), m_LocalScale);
			}
		}
		public Vector3 LocalPosition
		{
			get{ return m_LocalPosition; }
			internal set{ m_LocalPosition = value; }
		}
		public Vector3 LocalRotation
		{
			get{ return m_LocalRotation; }
			internal set{ m_LocalRotation = value; }
		}
		public Vector3 LocalScale
		{
			get{ return m_LocalScale; }
			internal set{ m_LocalScale = value; }
		}
		[SerializeField]
		Vector3 m_LocalPosition = Vector3.zero;
		[SerializeField]
		Vector3 m_LocalRotation = Vector3.zero;
		[SerializeField]
		Vector3 m_LocalScale = Vector3.one;
	}
}