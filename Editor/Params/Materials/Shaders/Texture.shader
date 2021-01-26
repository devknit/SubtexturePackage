Shader "Hidden/Subtexture/PerlinNoise"
{
	Properties
	{
		_MainTex( "Base Map", 2D) = "white" {}
		_UVScale( "UV Scale", Vector) = ( 1, 1, 1, 1)
		_UVOffset( "UV Offset", Vector) = ( 0, 0, 0, 0)
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
			
			uniform sampler2D _MainTex;
			float2 _UVScale;
			float2 _UVOffset;
			
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
				return tex2D( _MainTex, i.uv0 * _UVScale + _UVOffset);
			}
			ENDCG
		}
	}
}
