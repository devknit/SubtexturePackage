Shader "Hidden/Subtexture/ValueNoise"
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
				float c = valueNoise( i.uv0 * 8);
				return fixed4( c, c, c, 1);
			}
            ENDCG
        }
    }
}
