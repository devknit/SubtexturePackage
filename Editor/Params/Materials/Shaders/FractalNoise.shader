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
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "NoiseCG.cginc"
			
			float2 _UVScale;
			float2 _UVOffset;
			float _Brightness;
			int _Octave;
			float4 _Amplitude0123;
			float4 _Amplitude4567;
			float4 _Lacunarity0123;
			float4 _Lacunarity4567;
			float4 _Rotation0123;
			float4 _Rotation4567;
			float4 _Shift01;
			float4 _Shift23;
			float4 _Shift45;
			float4 _Shift67;
			
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
				float amplitude[8];
				amplitude[0] = _Amplitude0123.x;
				amplitude[1] = _Amplitude0123.y;
				amplitude[2] = _Amplitude0123.z;
				amplitude[3] = _Amplitude0123.w;
				amplitude[4] = _Amplitude4567.x;
				amplitude[5] = _Amplitude4567.y;
				amplitude[6] = _Amplitude4567.z;
				amplitude[7] = _Amplitude4567.w;
				
				float lacunarity[8];
				lacunarity[0] = _Lacunarity0123.x;
				lacunarity[1] = _Lacunarity0123.y;
				lacunarity[2] = _Lacunarity0123.z;
				lacunarity[3] = _Lacunarity0123.w;
				lacunarity[4] = _Lacunarity4567.x;
				lacunarity[5] = _Lacunarity4567.y;
				lacunarity[6] = _Lacunarity4567.z;
				lacunarity[7] = _Lacunarity4567.w;
				
				float rotation[8];
				rotation[0] = _Rotation0123.x;
				rotation[1] = _Rotation0123.y;
				rotation[2] = _Rotation0123.z;
				rotation[3] = _Rotation0123.w;
				rotation[4] = _Rotation4567.x;
				rotation[5] = _Rotation4567.y;
				rotation[6] = _Rotation4567.z;
				rotation[7] = _Rotation4567.w;
				
				float2 shift[8];
				shift[0] = _Shift01.xy;
				shift[1] = _Shift01.zw;
				shift[2] = _Shift23.xy;
				shift[3] = _Shift23.zw;
				shift[4] = _Shift45.xy;
				shift[5] = _Shift45.zw;
				shift[6] = _Shift67.xy;
				shift[7] = _Shift67.zw;
				
				float c = fBm( i.uv0 * _UVScale + _UVOffset, _Octave, amplitude, lacunarity, rotation, shift) + _Brightness;
				return fixed4( c.xxx, 1);
			}
			ENDCG
		}
	}
}
