Shader "Hidden/Subtexture/VoronoiNoise"
{
	Properties
    {
        _UVScale( "UV Scale", Vector) = ( 1, 1, 1, 1)
        _UVOffset( "UV Offset", Vector) = ( 0, 0, 0, 0)
        _TimePosition( "Time", Float) = 0
        _Swizzle( "Swizzle", Range( 0, 9)) = 0
        _ColorScale( "Color Scale", Vector) = ( 1, 1, 1, 1)
        _SmoothEdges( "Smooth Edges", Vector) = ( 0.05, 0.07, 0, 0)
    }
    SubShader
    {
        Tags
        {
			"RenderType" = "Opaque"
		}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "NoiseCG.cginc"
            
            UNITY_INSTANCING_BUFFER_START( Props)
				UNITY_DEFINE_INSTANCED_PROP( float2, _UVScale)
				UNITY_DEFINE_INSTANCED_PROP( float2, _UVOffset)
				UNITY_DEFINE_INSTANCED_PROP( float, _TimePosition)
				UNITY_DEFINE_INSTANCED_PROP( int, _Swizzle)
				UNITY_DEFINE_INSTANCED_PROP( float3, _ColorScale)
				UNITY_DEFINE_INSTANCED_PROP( float2, _SmoothEdges)
            UNITY_INSTANCING_BUFFER_END( Props)

            struct VertexInput
			{
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				fixed4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct VertexOutput
			{
				float4 position : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				fixed4 vertexColor : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			void vert( VertexInput v, out VertexOutput o)
			{
				UNITY_SETUP_INSTANCE_ID( v);
                UNITY_TRANSFER_INSTANCE_ID( v, o);
				o.position = UnityObjectToClipPos( v.vertex);
				o.uv0 = v.uv0;
				o.vertexColor = v.vertexColor;
			}
			fixed4 frag( VertexOutput i) : COLOR
			{
				UNITY_SETUP_INSTANCE_ID( i);
				float2 uvScale = UNITY_ACCESS_INSTANCED_PROP( Props, _UVScale);
				float2 uvOffset = UNITY_ACCESS_INSTANCED_PROP( Props, _UVOffset);
				float t = UNITY_ACCESS_INSTANCED_PROP( Props, _TimePosition);
				float swizzle = UNITY_ACCESS_INSTANCED_PROP( Props, _Swizzle);
				float3 colorScale = UNITY_ACCESS_INSTANCED_PROP( Props, _ColorScale);
				float2 smoothEdges = UNITY_ACCESS_INSTANCED_PROP( Props, _SmoothEdges);
				
				float4 c = float4( voronoiNoise2( i.uv0 * uvScale + uvOffset, t), 1.0);
				float smooth = smoothstep( smoothEdges.x, smoothEdges.y, c.z);
				
				if( swizzle == 0)
				{
					c.xyz = c.xxx;
				}
				else if( swizzle == 1)
				{
					c.xyz = c.yyy;
				}
				else if( swizzle == 2)
				{
					c.xyz = c.zzz;
				}
				else if( swizzle == 3)
				{
					c.xyz = 1.0 - c.zzz;
				}
				else if( swizzle == 4)
				{
					c.xyz = c.wxy;
				}
				else if( swizzle == 5)
				{
					c.xyz = c.wyx;
				}
				else if( swizzle == 6)
				{
					c.xyz = c.xwy;
				}
				else if( swizzle == 7)
				{
					c.xyz = c.ywx;
				}
				else if( swizzle == 8)
				{
					c.xyz = c.xyw;
				}
				else if( swizzle == 9)
				{
					c.xyz = c.yxw;
				}
				c.xyz = saturate( c.xyz * colorScale * smooth);
				return c;
			}
            ENDCG
        }
    }
}
