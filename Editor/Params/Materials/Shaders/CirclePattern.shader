Shader "Hidden/Subtexture/CirclePattern"
{
	Properties
    {
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
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "NoiseCG.cginc"
            
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
			float2 tile( float2 v, float zoom)
			{
				return frac( v * zoom);
			}
			float2 brickTile( float2 v, float zoom)
			{
			    v *= zoom;

			    v.x += step( 1.0, fmod( v.y, 2.0)) * 0.5;

			    return frac( v);
			}
			float2 rotate( float2 v, float radian)
			{
				float c = cos( radian);
				float s = sin( radian);
				return mul( float2x2( c, s, -s, c), v - 0.5) + 0.5;
			}
			float circlePattern( float2 v, float radius, float smoothEdges)
			{
				v -= 0.5;
				return 1.0 - smoothstep( radius - smoothEdges, radius, dot( v, v) * 4.0);
			}
			float squarePattern( float2 v, float2 size, float smoothEdges)
			{
			    size = (0.5).xx - size * 0.5;
			    float2 aa = (smoothEdges * 0.5).xx;
			    float2 uv = smoothstep( size, size + aa, v);
			    uv *= smoothstep( size, size + aa, (1.0).xx - v);
			    return uv.x * uv.y;
			}
			fixed4 frag( VertexOutput i) : COLOR
			{
				UNITY_SETUP_INSTANCE_ID( i);
				float2 uvScale = UNITY_ACCESS_INSTANCED_PROP( Props, _UVScale);
				float2 uvOffset = UNITY_ACCESS_INSTANCED_PROP( Props, _UVOffset);
				float2 uv = i.uv0 * uvScale + uvOffset;
				
				uv = brickTile( uv, 1.0);
				uv = rotate( uv, 3.141592 * 0.25);
				
				float c = circlePattern( uv, 1.0, 0.1);
			//	float c = squarePattern( uv, float2( 0.75, 0.75),  0.1);
				
				return fixed4( c.xxx, 1);
			}
            ENDCG
        }
    }
}
