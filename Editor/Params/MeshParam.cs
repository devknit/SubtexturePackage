
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
		kQuad,
		kPrefab,
	}
	[System.Serializable]
	public sealed class MeshParam : BaseParam
	{
		public MeshParam() : base( false)
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
					m_BoundsMaterial = new Material( shader);
				}
			}
			if( m_BoundsMesh == null)
			{
				m_BoundsMesh = new Mesh();
				
				m_BoundsMesh.SetVertices( new Vector3[]
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
				m_BoundsMesh.SetColors( new Color[]
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
				m_BoundsMesh.SetIndices( new int[]
				{
					0, 1, 2, 3, 4, 5, 6, 7,
					0, 2, 1, 3, 4, 6, 5, 7,
					0, 4, 1, 5, 2, 6, 3, 7,
				}, MeshTopology.Lines, 0);
			}
			if( m_DynamicMesh == null)
			{
				m_DynamicMesh = new Mesh();
				
				m_DynamicMesh.vertices = new Vector3[]
				{
					new Vector3( m_Width * -0.5f, m_Height * -0.5f, 0),
					new Vector3( m_Width * -0.5f, m_Height *  0.5f, 0),
					new Vector3( m_Width *  0.5f, m_Height * -0.5f, 0),
					new Vector3( m_Width *  0.5f, m_Height *  0.5f, 0)
				};
				m_DynamicMesh.colors = new Color[]
				{
					m_Color,
					m_Color,
					m_Color,
					m_Color
				};
				m_DynamicMesh.triangles = new int[]
				{
					0, 1, 2, 
					1, 3, 2
				};
				
				List<Vector4> texcoord;
				texcoord = new List<Vector4>();
				texcoord.Add( new Vector4( 0, 0, m_Texcoords0.z, m_Texcoords0.w));
				texcoord.Add( new Vector4( 0, 1, m_Texcoords0.z, m_Texcoords0.w));
				texcoord.Add( new Vector4( 1, 0, m_Texcoords0.z, m_Texcoords0.w));
				texcoord.Add( new Vector4( 1, 1, m_Texcoords0.z, m_Texcoords0.w));
				m_DynamicMesh.SetUVs( 0, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( m_Texcoords1);
				texcoord.Add( m_Texcoords1);
				texcoord.Add( m_Texcoords1);
				texcoord.Add( m_Texcoords1);
				m_DynamicMesh.SetUVs( 1, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( m_Texcoords2);
				texcoord.Add( m_Texcoords2);
				texcoord.Add( m_Texcoords2);
				texcoord.Add( m_Texcoords2);
				m_DynamicMesh.SetUVs( 2, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( m_Texcoords3);
				texcoord.Add( m_Texcoords3);
				texcoord.Add( m_Texcoords3);
				texcoord.Add( m_Texcoords3);
				m_DynamicMesh.SetUVs( 3, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( m_Texcoords4);
				texcoord.Add( m_Texcoords4);
				texcoord.Add( m_Texcoords4);
				texcoord.Add( m_Texcoords4);
				m_DynamicMesh.SetUVs( 4, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( m_Texcoords5);
				texcoord.Add( m_Texcoords5);
				texcoord.Add( m_Texcoords5);
				texcoord.Add( m_Texcoords5);
				m_DynamicMesh.SetUVs( 5, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( m_Texcoords6);
				texcoord.Add( m_Texcoords6);
				texcoord.Add( m_Texcoords6);
				texcoord.Add( m_Texcoords6);
				m_DynamicMesh.SetUVs( 6, texcoord);
				
				texcoord = new List<Vector4>();
				texcoord.Add( m_Texcoords7);
				texcoord.Add( m_Texcoords7);
				texcoord.Add( m_Texcoords7);
				texcoord.Add( m_Texcoords7);
				m_DynamicMesh.SetUVs( 7, texcoord);
				
				m_DynamicMesh.RecalculateNormals();
				m_DynamicMesh.RecalculateBounds();
				m_DynamicMesh.MarkDynamic();
			}
		}
		public override void OnDisable()
		{
			base.OnDisable();
			
			if( m_GameObject != null)
			{
				GameObject.DestroyImmediate( m_GameObject);
				m_GameObject = null;
			}
			if( m_BoundsObject != null)
			{
				GameObject.DestroyImmediate( m_BoundsObject);
				m_BoundsObject = null;
			}
			if( m_BoundsMaterial != null)
			{
				Material.DestroyImmediate( m_BoundsMaterial);
				m_BoundsMaterial = null;
			}
			if( m_BoundsMesh != null)
			{
				Mesh.DestroyImmediate( m_BoundsMesh);
				m_BoundsMesh = null;
			}
			if( m_DynamicMesh != null)
			{
				Mesh.DestroyImmediate( m_DynamicMesh);
				m_DynamicMesh = null;
			}
		}
		public override int OnGUI( PreviewRenderUtility context, BaseParam[] param)
		{
			int refreshCount = 0;
			
			OnPUI( "Mesh", false, 
				() =>
				{
					if( m_GameObject != null)
					{
						GameObject.DestroyImmediate( m_GameObject);
						m_GameObject = null;
					}
					if( m_BoundsObject != null)
					{
						GameObject.DestroyImmediate( m_BoundsObject);
						m_BoundsObject = null;
					}
					m_MeshType = MeshType.kQuad;
					m_Width = 1;
					m_Height = 1;
					m_Size = 1;
					m_Color = Color.white;
					m_Texcoords0 = Vector4.zero;
					m_Texcoords1 = Vector4.zero;
					m_Texcoords2 = Vector4.zero;
					m_Texcoords3 = Vector4.zero;
					m_Texcoords4 = Vector4.zero;
					m_Texcoords5 = Vector4.zero;
					m_Texcoords6 = Vector4.zero;
					m_Texcoords7 = Vector4.zero;
					
					m_DynamicMesh.vertices = new Vector3[]
					{
						new Vector3( m_Width * -0.5f, m_Height * -0.5f, 0),
						new Vector3( m_Width * -0.5f, m_Height *  0.5f, 0),
						new Vector3( m_Width *  0.5f, m_Height * -0.5f, 0),
						new Vector3( m_Width *  0.5f, m_Height *  0.5f, 0)
					};
					m_DynamicMesh.colors = new Color[]
					{
						m_Color,
						m_Color,
						m_Color,
						m_Color
					};
					List<Vector4> texcoord;
					
					texcoord = new List<Vector4>();
					texcoord.Add( new Vector4( 0, 0, m_Texcoords0.z, m_Texcoords0.w));
					texcoord.Add( new Vector4( 0, 1, m_Texcoords0.z, m_Texcoords0.w));
					texcoord.Add( new Vector4( 1, 0, m_Texcoords0.z, m_Texcoords0.w));
					texcoord.Add( new Vector4( 1, 1, m_Texcoords0.z, m_Texcoords0.w));
					m_DynamicMesh.SetUVs( 0, texcoord);
					
					texcoord = new List<Vector4>();
					texcoord.Add( m_Texcoords1);
					texcoord.Add( m_Texcoords1);
					texcoord.Add( m_Texcoords1);
					texcoord.Add( m_Texcoords1);
					m_DynamicMesh.SetUVs( 1, texcoord);
					
					texcoord = new List<Vector4>();
					texcoord.Add( m_Texcoords2);
					texcoord.Add( m_Texcoords2);
					texcoord.Add( m_Texcoords2);
					texcoord.Add( m_Texcoords2);
					m_DynamicMesh.SetUVs( 2, texcoord);
					
					texcoord = new List<Vector4>();
					texcoord.Add( m_Texcoords3);
					texcoord.Add( m_Texcoords3);
					texcoord.Add( m_Texcoords3);
					texcoord.Add( m_Texcoords3);
					m_DynamicMesh.SetUVs( 3, texcoord);
					
					texcoord = new List<Vector4>();
					texcoord.Add( m_Texcoords4);
					texcoord.Add( m_Texcoords4);
					texcoord.Add( m_Texcoords4);
					texcoord.Add( m_Texcoords4);
					m_DynamicMesh.SetUVs( 4, texcoord);
					
					texcoord = new List<Vector4>();
					texcoord.Add( m_Texcoords5);
					texcoord.Add( m_Texcoords5);
					texcoord.Add( m_Texcoords5);
					texcoord.Add( m_Texcoords5);
					m_DynamicMesh.SetUVs( 5, texcoord);
					
					texcoord = new List<Vector4>();
					texcoord.Add( m_Texcoords6);
					texcoord.Add( m_Texcoords6);
					texcoord.Add( m_Texcoords6);
					texcoord.Add( m_Texcoords6);
					m_DynamicMesh.SetUVs( 6, texcoord);
					
					texcoord = new List<Vector4>();
					texcoord.Add( m_Texcoords7);
					texcoord.Add( m_Texcoords7);
					texcoord.Add( m_Texcoords7);
					texcoord.Add( m_Texcoords7);
					m_DynamicMesh.SetUVs( 7, texcoord);
				}, 
				() =>
				{
					var meshTypeValue = (MeshType)EditorGUILayout.EnumPopup( "Type", m_MeshType);
					if( m_MeshType.Equals( meshTypeValue) == false)
					{
						Record( "Change Mesh Type");
						
						if( param[ (int)PreParamType.kAnimation] is AnimationParam animationParam)
						{
							animationParam.Dispose();
						}
						if( m_GameObject != null)
						{
							GameObject.DestroyImmediate( m_GameObject);
							m_GameObject = null;
						}
						if( m_BoundsObject != null)
						{
							GameObject.DestroyImmediate( m_BoundsObject);
							m_BoundsObject = null;
						}
						m_MeshType = meshTypeValue;
					}
					if( m_MeshType == MeshType.kAssets)
					{
						var assetMeshValue = EditorGUILayout.ObjectField( "Mesh", m_AssetMesh, typeof( Mesh), false) as Mesh;
						if( m_AssetMesh != assetMeshValue)
						{
							Record( "Change Mesh Assets");
							m_AssetMesh = assetMeshValue;
						}
					}
					else if( m_MeshType == MeshType.kQuad)
					{
						bool forceSquareValue = EditorGUILayout.Toggle( "Force square", m_ForceSquare);
						if( m_ForceSquare.Equals( forceSquareValue) == false)
						{
							Record( "Change Force square");
							if( forceSquareValue != false)
							{
								m_Width = m_Size;
								m_Height = m_Size;
							}
							m_ForceSquare = forceSquareValue;
						}
						if( m_ForceSquare == false)
						{
							EditorGUI.BeginChangeCheck();
							int widthValue = EditorGUILayout.IntSlider( "Width", m_Width, 1, 8196);
							if( EditorGUI.EndChangeCheck() != false && m_Width.Equals( widthValue) == false)
							{
								Record( "Change Width");
								m_Width = widthValue;
								m_Size = widthValue;
								
								m_DynamicMesh.vertices = new Vector3[]
								{
									new Vector3( m_Width * -0.5f, m_Height * -0.5f, 0),
									new Vector3( m_Width * -0.5f, m_Height *  0.5f, 0),
									new Vector3( m_Width *  0.5f, m_Height * -0.5f, 0),
									new Vector3( m_Width *  0.5f, m_Height *  0.5f, 0)
								};
							}
							EditorGUI.BeginChangeCheck();
							int heightValue = EditorGUILayout.IntSlider( "Height", m_Height, 1, 8196);
							if( EditorGUI.EndChangeCheck() != false && m_Height.Equals( heightValue) == false)
							{
								Record( "Change Height");
								m_Height = heightValue;
								m_Size = heightValue;
								
								m_DynamicMesh.vertices = new Vector3[]
								{
									new Vector3( m_Width * -0.5f, m_Height * -0.5f, 0),
									new Vector3( m_Width * -0.5f, m_Height *  0.5f, 0),
									new Vector3( m_Width *  0.5f, m_Height * -0.5f, 0),
									new Vector3( m_Width *  0.5f, m_Height *  0.5f, 0)
								};
							}
						}
						else
						{
							EditorGUI.BeginChangeCheck();
							int sizeValue = EditorGUILayout.IntSlider( "Size", m_Size, 1, 8196);
							if( EditorGUI.EndChangeCheck() != false && m_Size.Equals( sizeValue) == false)
							{
								Record( "Change Size");
								m_Size = sizeValue;
								m_Width = m_Size;
								m_Height = m_Size;
								
								m_DynamicMesh.vertices = new Vector3[]
								{
									new Vector3( m_Width * -0.5f, m_Height * -0.5f, 0),
									new Vector3( m_Width * -0.5f, m_Height *  0.5f, 0),
									new Vector3( m_Width *  0.5f, m_Height * -0.5f, 0),
									new Vector3( m_Width *  0.5f, m_Height *  0.5f, 0)
								};
							}
						}
						Color colorValue = EditorGUILayout.ColorField( "Color", m_Color);
						if( m_Color.Equals( colorValue) == false)
						{
							Record( "Change Vertex Color");
							m_DynamicMesh.colors = new Color[]
							{
								colorValue,
								colorValue,
								colorValue,
								colorValue
							};
							m_Color = colorValue;
						}
						List<Vector4> texcoord;
						Vector4 uv;
						
						Vector2 texcoordZW = new Vector2( m_Texcoords0.z, m_Texcoords0.w);
						Vector2 uvZW = EditorGUILayout.Vector2Field( "TexCoord0.zw", texcoordZW);
						if( texcoordZW.Equals( uvZW) == false)
						{
							Record( "Change TexCoord0");
							texcoord = new List<Vector4>();
							texcoord.Add( new Vector4( 0, 0, uvZW.x, uvZW.y));
							texcoord.Add( new Vector4( 0, 1, uvZW.x, uvZW.y));
							texcoord.Add( new Vector4( 1, 0, uvZW.x, uvZW.y));
							texcoord.Add( new Vector4( 1, 1, uvZW.x, uvZW.y));
							m_DynamicMesh.SetUVs( 0, texcoord);
							m_Texcoords0 = new Vector4( 0, 0, uvZW.x, uvZW.y);
						}
						uv = EditorGUILayout.Vector4Field( "TexCoord1", m_Texcoords1);
						if( m_Texcoords1.Equals( uv) == false)
						{
							Record( "Change TexCoord1");
							texcoord = new List<Vector4>();
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							m_DynamicMesh.SetUVs( 1, texcoord);
							m_Texcoords1 = uv;
						}
						uv = EditorGUILayout.Vector4Field( "TexCoord2", m_Texcoords2);
						if( m_Texcoords2.Equals( uv) == false)
						{
							Record( "Change TexCoord2");
							texcoord = new List<Vector4>();
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							m_DynamicMesh.SetUVs( 2, texcoord);
							m_Texcoords2 = uv;
						}
						uv = EditorGUILayout.Vector4Field( "TexCoord3", m_Texcoords3);
						if( m_Texcoords3.Equals( uv) == false)
						{
							Record( "Change TexCoord3");
							texcoord = new List<Vector4>();
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							m_DynamicMesh.SetUVs( 3, texcoord);
							m_Texcoords3 = uv;
						}
						uv = EditorGUILayout.Vector4Field( "TexCoord4", m_Texcoords4);
						if( m_Texcoords4.Equals( uv) == false)
						{
							Record( "Change TexCoord4");
							texcoord = new List<Vector4>();
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							m_DynamicMesh.SetUVs( 4, texcoord);
							m_Texcoords4 = uv;
						}
						uv = EditorGUILayout.Vector4Field( "TexCoord5", m_Texcoords5);
						if( m_Texcoords5.Equals( uv) == false)
						{
							Record( "Change TexCoord5");
							texcoord = new List<Vector4>();
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							m_DynamicMesh.SetUVs( 5, texcoord);
							m_Texcoords5 = uv;
						}
						uv = EditorGUILayout.Vector4Field( "TexCoord6", m_Texcoords6);
						if( m_Texcoords6.Equals( uv) == false)
						{
							Record( "Change TexCoord6");
							texcoord = new List<Vector4>();
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							m_DynamicMesh.SetUVs( 6, texcoord);
							m_Texcoords6 = uv;
						}
						uv = EditorGUILayout.Vector4Field( "TexCoord7", m_Texcoords7);
						if( m_Texcoords7.Equals( uv) == false)
						{
							Record( "Change TexCoord7");
							texcoord = new List<Vector4>();
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							texcoord.Add( uv);
							m_DynamicMesh.SetUVs( 7, texcoord);
							m_Texcoords7 = uv;
						}
					}
					else if( m_MeshType == MeshType.kPrefab)
					{
						var prefabValue = EditorGUILayout.ObjectField( "Prefab", m_Prefab, typeof( GameObject), false) as GameObject;
						if( m_RequestPrefab != null)
						{
							prefabValue = m_RequestPrefab;
							m_RequestPrefab = null;
							GUI.changed = true;
						}
						if( m_Prefab != prefabValue)
						{
							Record( "Change Prefab");
							
							if( param[ (int)PreParamType.kAnimation] is AnimationParam animationParam)
							{
								animationParam.Dispose();
							}
							if( m_GameObject != null)
							{
								GameObject.DestroyImmediate( m_GameObject);
								m_GameObject = null;
							}
							if( m_BoundsObject != null)
							{
								GameObject.DestroyImmediate( m_BoundsObject);
								m_BoundsObject = null;
							}
							m_Prefab = prefabValue;
						}
						bool m_AutoViewFitValue = EditorGUILayout.Toggle( "Auto view fit", m_AutoViewFit);
						if( m_AutoViewFit.Equals( m_AutoViewFitValue) == false)
						{
							m_AutoViewFit = m_AutoViewFitValue;
						}
						bool showBoundsValue = EditorGUILayout.Toggle( "Show BoundingBox", m_ShowBounds);
						if( m_ShowBounds.Equals( showBoundsValue) == false)
						{
							if( m_BoundsObject != null)
							{
								m_BoundsObject.SetActive( showBoundsValue);
							}
							m_ShowBounds = showBoundsValue;
						}
					}
					if( m_Prefab != null)
					{
						if( m_GameObject == null)
						{
							m_GameObject = context.InstantiatePrefabInScene( m_Prefab);
							
							if( param[ (int)PreParamType.kCamera] is CameraParam cameraParam)
							if( param[ (int)PreParamType.kTexture] is TextureParam textureParam)
							if( param[ (int)PreParamType.kTransform] is TransformParam transformParam)
							{
								if( m_AutoViewFit != false)
								{
									cameraParam.ToPreset( CameraPreset.kFit, textureParam, transformParam, this);
								}
							}
						}
						if( m_BoundsObject == null)
						{
							m_BoundsObject = new GameObject( "Bounds", typeof( MeshFilter), typeof( MeshRenderer));
							context.AddSingleGO( m_BoundsObject);
							m_BoundsObject.GetComponent<MeshFilter>().sharedMesh = m_BoundsMesh;
							m_BoundsObject.GetComponent<MeshRenderer>().sharedMaterial = m_BoundsMaterial;
							m_BoundsObject.transform.SetParent( m_GameObject.transform, false);
							m_BoundsObject.SetActive( m_ShowBounds);
						}
					}
				});
			return refreshCount;
		}
		public bool TryGetBoundingBox( out Bounds bounds)
		{
			switch( m_MeshType)
			{
				case MeshType.kPrefab:
				{
					if( m_GameObject != null)
					{
						bounds = GetBoundingBox( m_GameObject);
						return true;
					}
					break;
				}
			}
			bounds = new Bounds( Vector3.zero, Vector3.zero);
			return false;
		}
		static Bounds GetBoundingBox( GameObject m_GameObject)
		{
			var renderers = m_GameObject.GetComponentsInChildren<Renderer>( true);
			var bounds = new Bounds();
			
			foreach( var renderer in renderers)
			{
				Bounds? meshBounds = null;
				
				switch( renderer)
				{
					case MeshRenderer meshRenderer:
					{
						var filter = renderer.GetComponent<MeshFilter>();
						if( filter != null)
						{
							meshBounds = filter.mesh.bounds;
						}
						break;
					}
					case SkinnedMeshRenderer skinnedMeshRenderer:
					{
						var mesh = new Mesh();
						skinnedMeshRenderer.BakeMesh( mesh, true);
						meshBounds = mesh.bounds;
						Mesh.DestroyImmediate( mesh);
						break;
					}
				}
				if( meshBounds.HasValue != false)
				{
					Bounds transformBounds = RotateBounds( meshBounds.Value, renderer.transform.rotation);
					Vector3 boundsCenter = transformBounds.center + renderer.transform.position;
					Vector3 boundsSize = Vector3.Scale( transformBounds.size, renderer.transform.lossyScale);
					
					if( bounds.size == Vector3.zero)
					{
						// 元バウンドのサイズがゼロの場合はバウンドを作り直す
						bounds = new Bounds( boundsCenter, Vector3.zero);
					}
					bounds.Encapsulate( new Bounds( boundsCenter, boundsSize));
				}
			}
			Vector3 position = m_GameObject.transform.position;
			Vector3 scale = m_GameObject.transform.lossyScale;
			return new Bounds( 
				new Vector3(
					(bounds.center.x - position.x) / scale.x,
					(bounds.center.y - position.y) / scale.y,
					(bounds.center.z - position.z) / scale.z), 
				new Vector3(
					bounds.size.x / scale.x,
					bounds.size.y / scale.y,
					bounds.size.z / scale.z));
		}
		static Bounds RotateBounds( Bounds bounds, Quaternion quaternion)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			
			var vertices = new Vector3[]
			{
				new Vector3( min.x, min.y, min.z),
				new Vector3( min.x, min.y, max.z),
				new Vector3( min.x, max.y, min.z),
				new Vector3( min.x, max.y, max.z),
				new Vector3( max.x, min.y, min.z),
				new Vector3( max.x, min.y, max.z),
				new Vector3( max.x, max.y, min.z),
				new Vector3( max.x, max.y, max.z),
			};
			min = new Vector3( float.MaxValue, float.MaxValue, float.MaxValue);
			max = new Vector3( float.MinValue, float.MinValue, float.MinValue);
			
			foreach( Vector3 vertex in vertices)
			{
				var v = quaternion * vertex;
				min = Vector3.Min( min, v);
				max = Vector3.Max( max, v);
			}
			Vector3 size = max - min;
			return new Bounds( min + size * 0.5f, size);
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
			
			if( m_GameObject != null)
			{
				m_GameObject.transform.localPosition = transformParam.LocalPosition;
				m_GameObject.transform.localEulerAngles = transformParam.LocalRotation;
				m_GameObject.transform.localScale = transformParam.LocalScale;
			}
			if( TryGetBoundingBox( out bounds) != false)
			{
				m_BoundsObject.transform.localPosition = bounds.center;
				m_BoundsObject.transform.localScale = bounds.size;
			}
		}
		public Mesh RenderMesh
		{
			get
			{
				switch( m_MeshType)
				{
					case MeshType.kAssets: return m_AssetMesh;
					case MeshType.kQuad: return m_DynamicMesh;
				}
				return null;
			}
		}
		public MeshType MeshType
		{
			get{ return m_MeshType; }
		}
		public GameObject Prefab
		{
			get{ return m_Prefab; }
		}
		public GameObject GameObject
		{
			get{ return m_GameObject; }
		}
		public GameObject RequestPrefab
		{
			get{ return m_RequestPrefab; }
			internal set{ m_RequestPrefab = value; }
		}
		[SerializeField]
		MeshType m_MeshType = MeshType.kQuad;
		[SerializeField]
		bool m_ForceSquare = true;
		[SerializeField]
		int m_Width = 1;
		[SerializeField]
		int m_Height = 1;
		[SerializeField]
		int m_Size = 1;
		[SerializeField]
		Color m_Color = Color.white;
		[SerializeField]
		Vector4 m_Texcoords0 = Vector4.zero;
		[SerializeField]
		Vector4 m_Texcoords1 = Vector4.zero;
		[SerializeField]
		Vector4 m_Texcoords2 = Vector4.zero;
		[SerializeField]
		Vector4 m_Texcoords3 = Vector4.zero;
		[SerializeField]
		Vector4 m_Texcoords4 = Vector4.zero;
		[SerializeField]
		Vector4 m_Texcoords5 = Vector4.zero;
		[SerializeField]
		Vector4 m_Texcoords6 = Vector4.zero;
		[SerializeField]
		Vector4 m_Texcoords7 = Vector4.zero;
		[SerializeField]
		Material m_BoundsMaterial = default;
		[SerializeField]
		Mesh m_BoundsMesh = default;
		[SerializeField]
		Mesh m_DynamicMesh = default;
		[SerializeField]
		Mesh m_AssetMesh = default;
		[SerializeField]
		GameObject m_BoundsObject = default;
		[SerializeField]
		bool m_AutoViewFit = true;
		[SerializeField]
		bool m_ShowBounds = false;
		[SerializeField]
		GameObject m_Prefab = default;
		[SerializeField]
		GameObject m_GameObject = default;
		[System.NonSerialized]
		GameObject m_RequestPrefab;
	}
}