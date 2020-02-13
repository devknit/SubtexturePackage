Shader "Hidden/Subtexture/PostProcess/None"
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
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            UNITY_INSTANCING_BUFFER_START( Props)
				UNITY_DEFINE_INSTANCED_PROP( float2, _UVScale)
				UNITY_DEFINE_INSTANCED_PROP( float2, _UVOffset)
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
				float4 c = tex2D( _MainTex, i.uv0 * uvScale + uvOffset);
				return fixed4( c.xyz, c.w);
			}
            ENDCG
        }
    }
}
