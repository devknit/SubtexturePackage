Shader "Hidden/Subtexture/PerlinNoise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            #include "UnityCG.cginc"
            #include "NoiseCG.cginc"

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
				float c = perlinNoise( i.uv0 * 8) + 0.5;
			//	float c = fBm( i.uv0 * 8, 4, 0.5, 2.0, 0.5, 0, float2( 0, 0)) + 0.5;
				return fixed4( c, c, c, 1);
			}
            ENDCG
        }
    }
}
