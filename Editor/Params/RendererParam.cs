
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Subtexture
{
	[System.Serializable]
	public sealed class RendererParam : BaseParam
	{
		public RendererParam() : base( "Renderer")
		{
		}
		public override void OnEnable( EditorWindow window, bool opened)
		{
			base.OnEnable( window, opened);
			
			if( meshParam == null)
			{
				meshParam = new MeshParam();
			}
			meshParam.OnEnable( window, false);
			
			if( materialParam == null)
			{
				materialParam = new MaterialParam();
			}
			materialParam.OnEnable( window, true);
		}
		public override void OnDisable()
		{
			if( materialParam != null)
			{
				materialParam.OnDisable();
				materialParam = null;
			}
			if( meshParam != null)
			{
				meshParam.OnDisable();
				meshParam = null;
			}
			base.OnDisable();
		}
		protected override void OnParamGUI()
		{
			localPosition = EditorGUILayout.Vector3Field( "Position", localPosition);
			localRotation = EditorGUILayout.Vector3Field( "Rotation", localRotation);
			localScale = EditorGUILayout.Vector3Field( "Scale", localScale);
			
			meshParam.meshType = (MeshType)EditorGUILayout.EnumPopup( "Mesh Type", meshParam.meshType);
			materialParam.materialType = (MaterialType)EditorGUILayout.EnumPopup( "Material Type", materialParam.materialType);
		}
		protected override void OnAfterGUI()
		{
			meshParam.OnGUI();
			materialParam.OnGUI();
		}
		public Matrix4x4 LocalMatrix
		{
			get
			{
				return Matrix4x4.TRS( localPosition, Quaternion.Euler( localRotation), localScale);
			}
		}
		public Mesh RenderMesh
		{
			get{ return meshParam.RenderMesh; }
		}
		public Material RenderMaterial
		{
			get{ return materialParam.RenderMaterial; }
		}
		
		[SerializeField]
		Vector3 localPosition = Vector3.zero;
		[SerializeField]
		Vector3 localRotation = Vector3.zero;
		[SerializeField]
		Vector3 localScale = Vector3.one;
		
		[SerializeField]
		MeshParam meshParam;
		
		[SerializeField]
		MaterialParam materialParam;
		
		
	}
}