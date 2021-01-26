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
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "NoiseCG.cginc"
			
			float2 _UVScale;
			float2 _UVOffset;
			float _TimePosition;
			int _Swizzle;
			float3 _ColorScale;
			float2 _SmoothEdges;
			
			struct VertexInput
			{
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				fixed4 vertexColor : COLOR;
			};
			struct VertexOutput
			{
				float4 position : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				fixed4 vertexColor : COLOR;
			};
			void vert( VertexInput v, out VertexOutput o)
			{
				o.position = UnityObjectToClipPos( v.vertex);
				o.uv0 = v.uv0;
				o.vertexColor = v.vertexColor;
			}
			fixed4 frag( VertexOutput i) : COLOR
			{
				float4 c = float4( voronoiNoise2( i.uv0 * _UVScale + _UVOffset, _TimePosition), 1.0);
				float smooth = smoothstep( _SmoothEdges.x, _SmoothEdges.y, c.z);
				
				if( _Swizzle == 0)
				{
					c.xyz = c.xxx;
				}
				else if( _Swizzle == 1)
				{
					c.xyz = c.yyy;
				}
				else if( _Swizzle == 2)
				{
					c.xyz = c.zzz;
				}
				else if( _Swizzle == 3)
				{
					c.xyz = 1.0 - c.zzz;
				}
				else if( _Swizzle == 4)
				{
					c.xyz = c.wxy;
				}
				else if( _Swizzle == 5)
				{
					c.xyz = c.wyx;
				}
				else if( _Swizzle == 6)
				{
					c.xyz = c.xwy;
				}
				else if( _Swizzle == 7)
				{
					c.xyz = c.ywx;
				}
				else if( _Swizzle == 8)
				{
					c.xyz = c.xyw;
				}
				else if( _Swizzle == 9)
				{
					c.xyz = c.yxw;
				}
				c.xyz = saturate( c.xyz * _ColorScale * smooth);
				return c;
			}
			ENDCG
		}
	}
}
