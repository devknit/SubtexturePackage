
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Subtexture
{
	public enum MeshType
	{
		kAssets,
		kDynamic
	}
	[System.Serializable]
	public sealed class MeshParam : BaseParam
	{
		public override void OnEnable( Window window, bool opened)
		{
			base.OnEnable( window, opened);
			
			if( dynamicMesh == null)
			{
				dynamicMesh = new Mesh();
				
				dynamicMesh.vertices = new Vector3[]
				{
		            new Vector3( -0.5f, -0.5f, 0),
					new Vector3( -0.5f,  0.5f, 0),
					new Vector3(  0.5f, -0.5f, 0),
					new Vector3(  0.5f,  0.5f, 0)
		        };
		        dynamicMesh.colors = new Color[]
		        {
					vertexColor,
					vertexColor,
					vertexColor,
					vertexColor
				};
		        dynamicMesh.triangles = new int[]
		        {
					0, 1, 2, 
					1, 3, 2
				};
				
				List<Vector4> texcoord;
				texcoord = new List<Vector4>();
				texcoord.Add( new Vector4( 0, 0, texcoords0.z, texcoords0.w));
				texcoord.Add( new Vector4( 0, 1, texcoords0.z, texcoords0.w));
				texcoord.Add( new Vector4( 1, 0, texcoords0.z, texcoords0.w));
				texcoord.Add( new Vector4( 1, 1, texcoords0.z, texcoords0.w));
				dynamicMesh.SetUVs( 0, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( texcoords1);
				texcoord.Add( texcoords1);
				texcoord.Add( texcoords1);
				texcoord.Add( texcoords1);
				dynamicMesh.SetUVs( 1, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( texcoords2);
				texcoord.Add( texcoords2);
				texcoord.Add( texcoords2);
				texcoord.Add( texcoords2);
				dynamicMesh.SetUVs( 2, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( texcoords3);
				texcoord.Add( texcoords3);
				texcoord.Add( texcoords3);
				texcoord.Add( texcoords3);
				dynamicMesh.SetUVs( 3, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( texcoords4);
				texcoord.Add( texcoords4);
				texcoord.Add( texcoords4);
				texcoord.Add( texcoords4);
				dynamicMesh.SetUVs( 4, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( texcoords5);
				texcoord.Add( texcoords5);
				texcoord.Add( texcoords5);
				texcoord.Add( texcoords5);
				dynamicMesh.SetUVs( 5, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( texcoords6);
				texcoord.Add( texcoords6);
				texcoord.Add( texcoords6);
				texcoord.Add( texcoords6);
				dynamicMesh.SetUVs( 6, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( texcoords7);
				texcoord.Add( texcoords7);
				texcoord.Add( texcoords7);
				texcoord.Add( texcoords7);
				dynamicMesh.SetUVs( 7, texcoord);
				
				dynamicMesh.RecalculateNormals();
				dynamicMesh.RecalculateBounds();
				dynamicMesh.MarkDynamic();
			}
		}
		public override void OnDisable()
		{
			base.OnDisable();
			
			if( dynamicMesh != null)
			{
				Mesh.DestroyImmediate( dynamicMesh);
				dynamicMesh = null;
			}
		}
		public override void OnGUI()
		{
			OnPUI( "Mesh", () =>
			{
				meshType = (MeshType)EditorGUILayout.EnumPopup( "Type", meshType);
				if( meshType == MeshType.kAssets)
				{
					assetMesh = EditorGUILayout.ObjectField( "Mesh", assetMesh, typeof( Mesh), false) as Mesh;
				}
				else if( meshType == MeshType.kDynamic)
				{
					Color color = EditorGUILayout.ColorField( "Vertex Color", vertexColor);
					if( vertexColor != color)
					{
						dynamicMesh.colors = new Color[]
				        {
							color,
							color,
							color,
							color
						};
						vertexColor = color;
					}
					List<Vector4> texcoord;
					Vector4 uv;
					
					Vector2 texcoordZW = new Vector2( texcoords0.z, texcoords0.w);
					Vector2 uvZW = EditorGUILayout.Vector2Field( "TexCoord0.zw", texcoordZW);
					if( texcoordZW.Equals( uvZW) == false)
					{
						texcoord = new List<Vector4>();
						texcoord.Add( new Vector4( 0, 0, uvZW.x, uvZW.y));
						texcoord.Add( new Vector4( 0, 1, uvZW.x, uvZW.y));
						texcoord.Add( new Vector4( 1, 0, uvZW.x, uvZW.y));
						texcoord.Add( new Vector4( 1, 1, uvZW.x, uvZW.y));
						dynamicMesh.SetUVs( 0, texcoord);
						texcoords0 = new Vector4( 0, 0, uvZW.x, uvZW.y);
					}
					uv = EditorGUILayout.Vector4Field( "TexCoord1", texcoords1);
					if( texcoords1.Equals( uv) == false)
					{
						texcoord = new List<Vector4>();
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						dynamicMesh.SetUVs( 1, texcoord);
						texcoords1 = uv;
					}
					uv = EditorGUILayout.Vector4Field( "TexCoord2", texcoords2);
					if( texcoords2.Equals( uv) == false)
					{
						texcoord = new List<Vector4>();
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						dynamicMesh.SetUVs( 2, texcoord);
						texcoords2 = uv;
					}
					uv = EditorGUILayout.Vector4Field( "TexCoord3", texcoords3);
					if( texcoords3.Equals( uv) == false)
					{
						texcoord = new List<Vector4>();
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						dynamicMesh.SetUVs( 3, texcoord);
						texcoords3 = uv;
					}
					uv = EditorGUILayout.Vector4Field( "TexCoord4", texcoords4);
					if( texcoords4.Equals( uv) == false)
					{
						texcoord = new List<Vector4>();
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						dynamicMesh.SetUVs( 4, texcoord);
						texcoords4 = uv;
					}
					uv = EditorGUILayout.Vector4Field( "TexCoord5", texcoords5);
					if( texcoords5.Equals( uv) == false)
					{
						texcoord = new List<Vector4>();
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						dynamicMesh.SetUVs( 5, texcoord);
						texcoords5 = uv;
					}
					uv = EditorGUILayout.Vector4Field( "TexCoord6", texcoords6);
					if( texcoords6.Equals( uv) == false)
					{
						texcoord = new List<Vector4>();
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						dynamicMesh.SetUVs( 6, texcoord);
						texcoords6 = uv;
					}
					uv = EditorGUILayout.Vector4Field( "TexCoord7", texcoords7);
					if( texcoords7.Equals( uv) == false)
					{
						texcoord = new List<Vector4>();
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						dynamicMesh.SetUVs( 7, texcoord);
						texcoords7 = uv;
					}
				}
			});
		}
		public Mesh RenderMesh
		{
			get{ return (meshType == MeshType.kAssets) ? assetMesh : dynamicMesh; }
		}
		
		[SerializeField]
		public MeshType meshType = MeshType.kDynamic;
		[SerializeField]
		Color vertexColor = Color.white;
		[SerializeField]
		Vector4 texcoords0 = Vector4.zero;
		[SerializeField]
		Vector4 texcoords1 = Vector4.zero;
		[SerializeField]
		Vector4 texcoords2 = Vector4.zero;
		[SerializeField]
		Vector4 texcoords3 = Vector4.zero;
		[SerializeField]
		Vector4 texcoords4 = Vector4.zero;
		[SerializeField]
		Vector4 texcoords5 = Vector4.zero;
		[SerializeField]
		Vector4 texcoords6 = Vector4.zero;
		[SerializeField]
		Vector4 texcoords7 = Vector4.zero;
		[SerializeField]
		Mesh dynamicMesh = default;
		[SerializeField]
		Mesh assetMesh = default;
	}
}