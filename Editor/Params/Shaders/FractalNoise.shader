Shader "Hidden/Subtexture/FractalNoise"
{
	Properties
    {
        _UVScale( "UV Scale", Vector) = ( 1, 1, 1, 1)
        _UVOffset( "UV Offset", Vector) = ( 0, 0, 0, 0)
        _Brightness( "Brightness", Range( 0, 1)) = 0.5
        _Octave( "Octave", Range( 1, 8)) = 4
        _Amplitude0123( "Amplitude0123", Vector) = (0.5, 0.25, 0.125, 0.0625)
        _Amplitude4567( "Amplitude4567", Vector) = (0.03125, 0.015625, 0.0078125, 0.00390625)
        _Lacunarity0123( "Lacunarity0123", Vector) = (2.0, 2.0, 2.0, 2.0)
        _Lacunarity4567( "Lacunarity4567", Vector) = (2.0, 2.0, 2.0, 2.0)
        _Rotation0123( "Rotation0123", Vector) = (0.0, 0.0, 0.0, 0.0)
        _Rotation4567( "Rotation4567", Vector) = (0.0, 0.0, 0.0, 0.0)
        _Shift01( "Shift01", Vector) = (0.0, 0.0, 0.0, 0.0)
        _Shift23( "Shift23", Vector) = (0.0, 0.0, 0.0, 0.0)
        _Shift45( "Shift45", Vector) = (0.0, 0.0, 0.0, 0.0)
        _Shift67( "Shift67", Vector) = (0.0, 0.0, 0.0, 0.0)
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
				UNITY_DEFINE_INSTANCED_PROP( float, _Brightness)
				UNITY_DEFINE_INSTANCED_PROP( int, _Octave)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Amplitude0123)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Amplitude4567)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Lacunarity0123)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Lacunarity4567)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Rotation0123)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Rotation4567)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Shift01)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Shift23)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Shift45)
				UNITY_DEFINE_INSTANCED_PROP( float4, _Shift67)
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
				float brightness = UNITY_ACCESS_INSTANCED_PROP( Props, _Brightness);
				int octave = UNITY_ACCESS_INSTANCED_PROP( Props, _Octave);
				float4 amplitude0123 = UNITY_ACCESS_INSTANCED_PROP( Props, _Amplitude0123);
				float4 amplitude4567 = UNITY_ACCESS_INSTANCED_PROP( Props, _Amplitude4567);
				float4 lacunarity0123 = UNITY_ACCESS_INSTANCED_PROP( Props, _Lacunarity0123);
				float4 lacunarity4567 = UNITY_ACCESS_INSTANCED_PROP( Props, _Lacunarity4567);
				float4 rotation0123 = UNITY_ACCESS_INSTANCED_PROP( Props, _Rotation0123);
				float4 rotation4567 = UNITY_ACCESS_INSTANCED_PROP( Props, _Rotation4567);
				float4 shift01 = UNITY_ACCESS_INSTANCED_PROP( Props, _Shift01);
				float4 shift23 = UNITY_ACCESS_INSTANCED_PROP( Props, _Shift23);
				float4 shift45 = UNITY_ACCESS_INSTANCED_PROP( Props, _Shift45);
				float4 shift67 = UNITY_ACCESS_INSTANCED_PROP( Props, _Shift67);
				
				float amplitude[8];
				amplitude[0] = amplitude0123.x;
				amplitude[1] = amplitude0123.y;
				amplitude[2] = amplitude0123.z;
				amplitude[3] = amplitude0123.w;
				amplitude[4] = amplitude4567.x;
				amplitude[5] = amplitude4567.y;
				amplitude[6] = amplitude4567.z;
				amplitude[7] = amplitude4567.w;
				
				float lacunarity[8];
				lacunarity[0] = lacunarity0123.x;
				lacunarity[1] = lacunarity0123.y;
				lacunarity[2] = lacunarity0123.z;
				lacunarity[3] = lacunarity0123.w;
				lacunarity[4] = lacunarity4567.x;
				lacunarity[5] = lacunarity4567.y;
				lacunarity[6] = lacunarity4567.z;
				lacunarity[7] = lacunarity4567.w;
				
				float rotation[8];
				rotation[0] = rotation0123.x;
				rotation[1] = rotation0123.y;
				rotation[2] = rotation0123.z;
				rotation[3] = rotation0123.w;
				rotation[4] = rotation4567.x;
				rotation[5] = rotation4567.y;
				rotation[6] = rotation4567.z;
				rotation[7] = rotation4567.w;
				
				float2 shift[8];
				shift[0] = shift01.xy;
				shift[1] = shift01.zw;
				shift[2] = shift23.xy;
				shift[3] = shift23.zw;
				shift[4] = shift45.xy;
				shift[5] = shift45.zw;
				shift[6] = shift67.xy;
				shift[7] = shift67.zw;
				
				float c = fBm( i.uv0 * uvScale + uvOffset, octave, amplitude, lacunarity, rotation, shift) + brightness;
				return fixed4( c.xxx, 1);
			}
            ENDCG
        }
    }
}
