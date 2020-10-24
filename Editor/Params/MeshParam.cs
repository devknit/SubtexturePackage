
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System.Linq;
using System.Collections.Generic;

namespace Subtexture
{
	public enum MeshType
	{
		kAssets,
		kDynamic,
		kPrefab
	}
	[System.Serializable]
	public sealed class MeshParam : BaseParam
	{
		public MeshParam() : base( true)
		{
		}
		public override void OnEnable( Window window)
		{
			base.OnEnable( window);
			
			string newShaderPath = AssetDatabase.GUIDToAssetPath( "2848d84731c5f6c4683867a9fafecacc");
			
			if( string.IsNullOrEmpty( newShaderPath) == false)
			{
				if( AssetDatabase.LoadAssetAtPath<Shader>( newShaderPath) is Shader shader)
				{
					boundsMaterial = new Material( shader);
				}
			}
			if( boundsMesh == null)
			{
				boundsMesh = new Mesh();
				
				boundsMesh.SetVertices( new Vector3[]
				{
		            new Vector3( -0.5f, -0.5f, -0.5f),
					new Vector3(  0.5f, -0.5f, -0.5f),
					new Vector3( -0.5f,  0.5f, -0.5f),
					new Vector3(  0.5f,  0.5f, -0.5f),
					new Vector3( -0.5f, -0.5f,  0.5f),
					new Vector3(  0.5f, -0.5f,  0.5f),
					new Vector3( -0.5f,  0.5f,  0.5f),
					new Vector3(  0.5f,  0.5f,  0.5f),
		        });
		        boundsMesh.SetColors( new Color[]
		        {
					Color.green,
					Color.green,
					Color.green,
					Color.green,
					Color.green,
					Color.green,
					Color.green,
					Color.green,
				});
				boundsMesh.SetIndices( new int[]
				{
					0, 1, 2, 3, 4, 5, 6, 7,
					0, 2, 1, 3, 4, 6, 5, 7,
					0, 4, 1, 5, 2, 6, 3, 7,
				}, MeshTopology.Lines, 0);
			}
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
			
			if( gameObject != null)
			{
				GameObject.DestroyImmediate( gameObject);
				gameObject = null;
			}
			if( boundsObject != null)
			{
				GameObject.DestroyImmediate( boundsObject);
				boundsObject = null;
			}
			if( boundsMaterial != null)
			{
				Material.DestroyImmediate( boundsMaterial);
				boundsMaterial = null;
			}
			if( boundsMesh != null)
			{
				Mesh.DestroyImmediate( boundsMesh);
				boundsMesh = null;
			}
			if( dynamicMesh != null)
			{
				Mesh.DestroyImmediate( dynamicMesh);
				dynamicMesh = null;
			}
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			int refreshCount = 0;
			
			OnPUI( "Mesh", false, () =>
			{
				var meshTypeValue = (MeshType)EditorGUILayout.EnumPopup( "Type", meshType);
				if( meshType.Equals( meshTypeValue) == false)
				{
					Record( "Change Mesh Type");
					
					if( param[ (int)PreParamType.kAnimation] is AnimationParam animationParam)
					{
						animationParam.Dispose();
					}
					if( gameObject != null)
					{
						GameObject.DestroyImmediate( gameObject);
						gameObject = null;
					}
					if( boundsObject != null)
					{
						GameObject.DestroyImmediate( boundsObject);
						boundsObject = null;
					}
					meshType = meshTypeValue;
				}
				if( meshType == MeshType.kAssets)
				{
					var assetMeshValue = EditorGUILayout.ObjectField( "Mesh", assetMesh, typeof( Mesh), false) as Mesh;
					if( assetMesh != assetMeshValue)
					{
						Record( "Change Mesh Assets");
						assetMesh = assetMeshValue;
					}
				}
				else if( meshType == MeshType.kDynamic)
				{
					Color vertexColorValue = EditorGUILayout.ColorField( "Vertex Color", vertexColor);
					if( vertexColor.Equals( vertexColorValue) == false)
					{
						Record( "Change Vertex Color");
						dynamicMesh.colors = new Color[]
				        {
							vertexColorValue,
							vertexColorValue,
							vertexColorValue,
							vertexColorValue
						};
						vertexColor = vertexColorValue;
					}
					List<Vector4> texcoord;
					Vector4 uv;
					
					Vector2 texcoordZW = new Vector2( texcoords0.z, texcoords0.w);
					Vector2 uvZW = EditorGUILayout.Vector2Field( "TexCoord0.zw", texcoordZW);
					if( texcoordZW.Equals( uvZW) == false)
					{
						Record( "Change TexCoord0");
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
						Record( "Change TexCoord1");
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
						Record( "Change TexCoord2");
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
						Record( "Change TexCoord3");
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
						Record( "Change TexCoord4");
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
						Record( "Change TexCoord5");
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
						Record( "Change TexCoord6");
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
						Record( "Change TexCoord7");
						texcoord = new List<Vector4>();
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						texcoord.Add( uv);
						dynamicMesh.SetUVs( 7, texcoord);
						texcoords7 = uv;
					}
				}
				else if( meshType == MeshType.kPrefab)
				{
					var prefabValue = EditorGUILayout.ObjectField( "Prefab", prefab, typeof( GameObject), false) as GameObject;
					if( requestPrefab != null)
					{
						prefabValue = requestPrefab;
						requestPrefab = null;
						GUI.changed = true;
					}
					if( prefab != prefabValue)
					{
						Record( "Change Prefab");
						
						if( param[ (int)PreParamType.kAnimation] is AnimationParam animationParam)
						{
							animationParam.Dispose();
						}
						if( gameObject != null)
						{
							GameObject.DestroyImmediate( gameObject);
							gameObject = null;
						}
						if( boundsObject != null)
						{
							GameObject.DestroyImmediate( boundsObject);
							boundsObject = null;
						}
						prefab = prefabValue;
					}
					bool showBoundsValue = EditorGUILayout.Toggle( "Show BoundingBox", showBounds);
					if( showBounds.Equals( showBoundsValue) == false)
					{
						boundsObject.SetActive( showBoundsValue);
						showBounds = showBoundsValue;
					}
				}
				if( prefab != null)
				{
					if( gameObject == null)
					{
						gameObject = context.InstantiatePrefabInScene( prefab);
						
						if( param[ (int)PreParamType.kCamera] is CameraParam cameraParam)
						if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
						if( param[ (int)PreParamType.kTransform] is TransformParam transformParam)
						{
							cameraParam.ToPreset( CameraPreset.kFit, textureParam, transformParam, this);
						}
					}
					if( boundsObject == null)
					{
						boundsObject = new GameObject( "Bounds", typeof( MeshFilter), typeof( MeshRenderer));
						context.AddSingleGO( boundsObject);
						boundsObject.GetComponent<MeshFilter>().sharedMesh = boundsMesh;
						boundsObject.GetComponent<MeshRenderer>().sharedMaterial = boundsMaterial;
						boundsObject.transform.SetParent( gameObject.transform, false);
						boundsObject.SetActive( showBounds);
					}
				}
			});
			return refreshCount;
		}
		public bool TryGetBoundingBox( out Bounds bounds)
		{
			switch( meshType)
			{
				case MeshType.kPrefab:
				{
					if( gameObject != null)
					{
						bounds = new Bounds();
						
						Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
						
						for( int i0 = 0; i0 < renderers.Length; ++i0)
						{
							switch( renderers[ i0])
							{
								case MeshRenderer meshRenderer:
								case SkinnedMeshRenderer skinnedMeshRenderer:
								{
									bounds.Encapsulate( renderers[ i0].bounds);
									break;
								}
							}
						}
						return true;
					}
					break;
				}
			}
			bounds = new Bounds( Vector3.zero, Vector3.zero);
			return false;
		}
		public bool TryGetBoundingSphere( out BoundingSphere sphere)
		{
			Bounds bounds;
			
			if( TryGetBoundingBox( out bounds) != false)
			{
				sphere = new BoundingSphere( bounds.center, ((bounds.max - bounds.min) / 2.0f).magnitude);
				return true;
			}
			sphere = new BoundingSphere( Vector3.zero, 0.0f);
			return false;
		}
		public void Update( PreviewRenderUtility context, TransformParam transformParam)
		{
			Bounds bounds;
			
			if( gameObject != null)
			{
				gameObject.transform.localPosition = transformParam.localPosition;
				gameObject.transform.localEulerAngles = transformParam.localRotation;
				gameObject.transform.localScale = transformParam.localScale;
			}
			if( TryGetBoundingBox( out bounds) != false)
			{
				boundsObject.transform.localPosition = bounds.center;
				boundsObject.transform.localScale = bounds.size;
			}
		}
		public Mesh RenderMesh
		{
			get
			{
				switch( meshType)
				{
					case MeshType.kAssets: return assetMesh;
					case MeshType.kDynamic: return dynamicMesh;
				}
				return null;
			}
		}
		
		[SerializeField]
		public MeshType meshType = MeshType.kPrefab;
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
		Material boundsMaterial = default;
		[SerializeField]
		Mesh boundsMesh = default;
		[SerializeField]
		Mesh dynamicMesh = default;
		[SerializeField]
		Mesh assetMesh = default;
		[SerializeField]
		GameObject boundsObject = default;
		[SerializeField]
		bool showBounds = false;
		[SerializeField]
		public GameObject prefab = default;
		[SerializeField]
		public GameObject gameObject = default;
		[System.NonSerialized]
		public GameObject requestPrefab;
	}
}