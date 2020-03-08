Shader "Hidden/Subtexture/PostProcess/BlendMap"
{
	Properties
	{
		_MainTex( "Base Map", 2D) = "white" {}
		_MultiTex( "Blend Map", 2D) = "white" {}
		_UVScale( "UV Scale", Vector) = ( 1, 1, 1, 1)
        _UVOffset( "UV Offset", Vector) = ( 0, 0, 0, 0)
		_MultiTexAlphaRemap( "Blend Map Alpha Remap Param", Vector) = (0.0, 1.0, 0.0, 1.0)
		[KeywordEnum(None, Override, Multiply, Darken, ColorBurn, LinearBurn, Lighten, Screen, ColorDodge, LinearDodge, Overlay, HardLight, VividLight, LinearLight, PinLight, HardMix, Difference, Exclusion, Substract, Division, Hue, Saturation, Luminosity, Color)]
		_COLORBLENDOP1( "Multi Map Color Blend Op", float) = 0
		[KeywordEnum(Value, AlphaBlendOp, OneMinusAlphaBlendOp, BaseAlpha, OneMinusBaseAlpha, BlendAlpha, OneMinusBlendAlpha, BaseColorValue, OneMinusBaseColorValue, BlendColorValue, OneMinusBlendColorValue)]
		_COLORBLENDSRC1( "Multi Map Color Blend Ratop Source", float) = 0
		_ColorBlendRatio1( "Multi Map Color Blend Ratio Value", float) = 1.0
		[KeywordEnum(None, Override, Multiply, Add, Substract, ReverseSubstract, Maximum)]
		_ALPHABLENDOP1( "Multi Map Alpha Blend Op", float) = 2
		_AlphaBlendRatio1( "Multi Map Alpha Blend Ratio Value", float) = 1.0
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
            #pragma shader_feature_local _COLORBLENDOP1_NONE _COLORBLENDOP1_OVERRIDE _COLORBLENDOP1_MULTIPLY _COLORBLENDOP1_DARKEN _COLORBLENDOP1_COLORBURN _COLORBLENDOP1_LINEARBURN _COLORBLENDOP1_LIGHTEN _COLORBLENDOP1_SCREEN _COLORBLENDOP1_COLORDODGE _COLORBLENDOP1_LINEARDODGE _COLORBLENDOP1_OVERLAY _COLORBLENDOP1_HARDLIGHT _COLORBLENDOP1_VIVIDLIGHT _COLORBLENDOP1_LINEARLIGHT _COLORBLENDOP1_PINLIGHT _COLORBLENDOP1_HARDMIX _COLORBLENDOP1_DIFFERENCE _COLORBLENDOP1_EXCLUSION _COLORBLENDOP1_SUBSTRACT _COLORBLENDOP1_DIVISION _COLORBLENDOP1_HUE _COLORBLENDOP1_SATURATION _COLORBLENDOP1_LUMINOSITY _COLORBLENDOP1_COLOR
			#pragma shader_feature_local _COLORBLENDSRC1_VALUE _COLORBLENDSRC1_ALPHABLENDOP _COLORBLENDSRC1_ONEMINUSALPHABLENDOP _COLORBLENDSRC1_BASEALPHA _COLORBLENDSRC1_ONEMINUSBASEALPHA _COLORBLENDSRC1_BLENDALPHA _COLORBLENDSRC1_ONEMINUSBLENDALPHA _COLORBLENDSRC1_BASECOLORVALUE _COLORBLENDSRC1_ONEMINUSBASECOLORVALUE _COLORBLENDSRC1_BLENDCOLORVALUE _COLORBLENDSRC1_ONEMINUSBLENDCOLORVALUE
			#pragma shader_feature_local _ALPHABLENDOP1_NONE _ALPHABLENDOP1_OVERRIDE _ALPHABLENDOP1_MULTIPLY _ALPHABLENDOP1_ADD _ALPHABLENDOP1_SUBSTRACT _ALPHABLENDOP1_REVERSESUBSTRACT _ALPHABLENDOP1_MAXIMUM
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            
            uniform sampler2D _MainTex;
            uniform sampler2D _BlendTex;
            UNITY_INSTANCING_BUFFER_START( Props)
            	UNITY_DEFINE_INSTANCED_PROP( float4, _MultiTexAlphaRemap)
				UNITY_DEFINE_INSTANCED_PROP( float,  _ColorBlendRatio1)
				UNITY_DEFINE_INSTANCED_PROP( float,  _AlphaBlendRatio1)
				UNITY_DEFINE_INSTANCED_PROP( float2, _UVScale)
				UNITY_DEFINE_INSTANCED_PROP( float2, _UVOffset)
            UNITY_INSTANCING_BUFFER_END( Props)

            struct VertexInput
			{
				float4 vertex : POSITION;
				float2 uv0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			struct VertexOutput
			{
				float4 position : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			void vert( VertexInput v, out VertexOutput o)
			{
				UNITY_SETUP_INSTANCE_ID( v);
                UNITY_TRANSFER_INSTANCE_ID( v, o);
				o.position = UnityObjectToClipPos( v.vertex);
				o.uv0 = v.uv0;
			}
			float3 RGBToHSV( float3 rgb)
			{
				float4 K = float4( 0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 P = lerp( float4( rgb.bg, K.wz), float4( rgb.gb, K.xy), step( rgb.b, rgb.g));
				float4 Q = lerp( float4( P.xyw, rgb.r), float4( rgb.r, P.yzx), step( P.x, rgb.r));
				float  D = Q.x - min( Q.w, Q.y);
				float  E = 1e-4;
				return float3( abs( Q.z + (Q.w - Q.y) / (6.0 * D + E)), D / (Q.x + E), Q.x);
			}
			float3 HSVToRGB( float3 hsv)
			{
				float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
				float3 P = abs( frac( hsv.xxx + K.xyz) * 6.0 - K.www);
				return hsv.z * lerp( K.xxx, saturate( P - K.xxx), hsv.y);
			}
			inline fixed3 BelndMultiply( fixed3 Base, fixed3 Blend)
			{
				return Base * Blend;
			}
			inline fixed3 BelndDarken( fixed3 Base, fixed3 Blend)
			{
				return min( Base, Blend);
			}
			inline fixed3 BelndColorBurn( fixed3 Base, fixed3 Blend)
			{
				return 1.0 - (1.0 - Base) / (Blend + 1e-12);
			}
			inline fixed3 BelndLinearBurn( fixed3 Base, fixed3 Blend)
			{
				return saturate( Base + Blend - 1.0);
			}
			inline fixed3 BelndLighten( fixed3 Base, fixed3 Blend)
			{
				return max( Base, Blend);
			}
			inline fixed3 BelndScreen( fixed3 Base, fixed3 Blend)
			{
				return 1.0 - (1.0 - Base) * (1.0 - Blend);
			}
			inline fixed3 BelndColorDodge( fixed3 Base, fixed3 Blend)
			{
				return Base / (1.0 - clamp( Blend, 1e-12, 0.999999));
			}
			inline fixed3 BelndLinearDodge( fixed3 Base, fixed3 Blend)
			{
				return saturate( Blend + Base);
			}
			inline fixed3 BelndOverlay( fixed3 Base, fixed3 Blend)
			{
				float3 result1 = 2.0 * Base * Blend;
				float3 result2 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
			    float3 zeroOrOne = step( Base, 0.5);
			    return result1 * zeroOrOne + (1 - zeroOrOne) * result2;
			}
			inline fixed3 BelndHardLight( fixed3 Base, fixed3 Blend)
			{
				float3 result1 = 2.0 * Base * Blend;
				float3 result2 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
				float3 zeroOrOne = step( Blend, 0.5);
				return result1 * zeroOrOne + (1 - zeroOrOne) * result2;
			}
			inline fixed3 BelndVividLight( fixed3 Base, fixed3 Blend)
			{
			    float3 result1 = BelndColorBurn( Base, 2.0 * Blend);
			    float3 result2 = BelndColorDodge( Base, 2.0 * (Blend - 0.5));
			    float3 zeroOrOne = step( Blend, 0.5);
			    return result1 * zeroOrOne + (1 - zeroOrOne) * result2;
			}
			inline fixed3 BelndLinearLight( fixed3 Base, fixed3 Blend)
			{
				float3 result1 = BelndLinearBurn( Base, 2.0 * Blend);
				float3 result2 = BelndLinearDodge( Base, 2.0 * (Blend - 0.5));
				float3 zeroOrOne = step( Blend, 0.5);
				return result1 * zeroOrOne + (1 - zeroOrOne) * result2;
			}
			inline fixed3 BelndPinLight( fixed3 Base, fixed3 Blend)
			{
				float3 result1 = BelndDarken( Base, 2.0 * Blend);
				float3 result2 = BelndLighten( Base, 2.0 * (Blend - 0.5));
				float3 zeroOrOne = step( Blend, 0.5);
				return result1 * zeroOrOne + (1 - zeroOrOne) * result2;
			}
			inline fixed3 BelndHardMix( fixed3 Base, fixed3 Blend)
			{
				return step( 1 - Base, Blend);
			}
			inline fixed3 BelndDifference( fixed3 Base, fixed3 Blend)
			{
				return abs( Base - Blend);
			}
			inline fixed3 BelndExclusion( fixed3 Base, fixed3 Blend)
			{
				return Base + Blend - 2.0 * Base * Blend;
			}
			inline fixed3 BelndSubstract( fixed3 Base, fixed3 Blend)
			{
				return saturate( Base - Blend);
			}
			inline fixed3 BelndDivision( fixed3 Base, fixed3 Blend)
			{
				return saturate( Base / (Blend + 1e-12));
			}
			inline fixed3 BelndHue( fixed3 Base, fixed3 Blend)
			{
				float3 hsvBase = RGBToHSV( Base);
				float3 hsvBlend = RGBToHSV( Blend);
				return HSVToRGB( float3( hsvBlend.x, hsvBase.yz));
			}
			inline fixed3 BelndSaturation( fixed3 Base, fixed3 Blend)
			{
				float3 hsvBase = RGBToHSV( Base);
				float3 hsvBlend = RGBToHSV( Blend);
				return HSVToRGB( float3( hsvBase.x, hsvBlend.y, hsvBase.z));
			}
			inline fixed3 BelndLuminosity( fixed3 Base, fixed3 Blend)
			{
				float3 hsvBase = RGBToHSV( Base);
				float3 hsvBlend = RGBToHSV( Blend);
				return HSVToRGB( float3( hsvBase.xy, hsvBlend.z));
			}
			inline fixed3 BelndColor( fixed3 Base, fixed3 Blend)
			{
				float3 hsvBase = RGBToHSV( Base);
				float3 hsvBlend = RGBToHSV( Blend);
				return HSVToRGB( float3( hsvBlend.xy, hsvBase.z));
			}
			inline fixed4 Blending( fixed4 Base, fixed4 Blend, float ColorRatio, float AlphaRatio)
			{
				fixed alpha = Base.a;
			#if   _ALPHABLENDOP1_OVERRIDE
				alpha = Blend.a;
			#elif _ALPHABLENDOP1_MULTIPLY
				alpha = Base.a * Blend.a;
			#elif _ALPHABLENDOP1_ADD
				alpha = Base.a + Blend.a;
			#elif _ALPHABLENDOP1_SUBSTRACT
				alpha = Base.a - Blend.a;
			#elif _ALPHABLENDOP1_REVERSESUBSTRACT
				alpha = Blend.a - Base.a;
			#elif _ALPHABLENDOP1_MAXIMUM
				alpha = max( Base.a, Blend.a);
			#endif
				alpha = saturate( lerp( Base.a, alpha, AlphaRatio));

			#if   _COLORBLENDSRC1_VALUE
			#elif _COLORBLENDSRC1_ALPHABLENDOP
				ColorRatio *= alpha;
			#elif _COLORBLENDSRC1_ONEMINUSALPHABLENDOP
				ColorRatio *= 1.0 - alpha;
			#elif _COLORBLENDSRC1_BASEALPHA
				ColorRatio *= Base.a;
			#elif _COLORBLENDSRC1_ONEMINUSBASEALPHA
				ColorRatio *= 1.0 - Base.a;
			#elif _COLORBLENDSRC1_BLENDALPHA
				ColorRatio *= Blend.a;
			#elif _COLORBLENDSRC1_ONEMINUSBLENDALPHA
				ColorRatio *= 1.0 - Blend.a;
			#elif _COLORBLENDSRC1_BASECOLORVALUE
				ColorRatio *= max( Base.r, max( Base.g, Base.b));
			#elif _COLORBLENDSRC1_ONEMINUSBASECOLORVALUE
				ColorRatio *= 1.0 - max( Base.r, max( Base.g, Base.b));
			#elif _COLORBLENDSRC1_BLENDCOLORVALUE
				ColorRatio *= max( Blend.r, max( Blend.g, Blend.b));
			#elif _COLORBLENDSRC1_ONEMINUSBLENDCOLORVALUE
				ColorRatio *= 1.0 - max( Blend.r, max( Blend.g, Blend.b));
			#endif

				fixed3 color = Base.rgb;
			#if   _COLORBLENDOP1_OVERRIDE
				color = Blend.rgb;
			#elif _COLORBLENDOP1_MULTIPLY
				color = BelndMultiply( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_DARKEN
				color = BelndDarken( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_COLORBURN
				color = BelndColorBurn( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_LINEARBURN
				color = BelndLinearBurn( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_LIGHTEN
				color = BelndLighten( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_SCREEN
				color = BelndScreen( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_COLORDODGE
				color = BelndColorDodge( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_LINEARDODGE
				color = BelndLinearDodge( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_OVERLAY
				color = BelndOverlay( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_HARDLIGHT
				color = BelndHardLight( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_VIVIDLIGHT
				color = BelndVividLight( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_LINEARLIGHT
				color = BelndLinearLight( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_PINLIGHT
				color = BelndPinLight( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_HARDMIX
				color = BelndHardMix( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_DIFFERENCE
				color = BelndDifference( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_EXCLUSION
				color = BelndExclusion( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_SUBSTRACT
				color = BelndSubstract( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_DIVISION
				color = BelndDivision( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_HUE
				color = BelndHue( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_SATURATION
				color = BelndSaturation( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_LUMINOSITY
				color = BelndLuminosity( Base.rgb, Blend.rgb);
			#elif _COLORBLENDOP1_COLOR
				color = BelndColor( Base.rgb, Blend.rgb);
			#endif
				return fixed4( max( 0.0, lerp( Base.rgb, color, ColorRatio)), alpha);
			}
			fixed4 frag( VertexOutput i) : COLOR
			{
				UNITY_SETUP_INSTANCE_ID( i);
				float2 uvScale = UNITY_ACCESS_INSTANCED_PROP( Props, _UVScale);
				float2 uvOffset = UNITY_ACCESS_INSTANCED_PROP( Props, _UVOffset);
				float colorBlendRatio = UNITY_ACCESS_INSTANCED_PROP( Props, _ColorBlendRatio1);
				float alphaBlendRatio = UNITY_ACCESS_INSTANCED_PROP( Props, _AlphaBlendRatio1);
				float4 color = tex2D( _MainTex, i.uv0);
				float4 value = tex2D( _BlendTex, i.uv0 * uvScale + uvOffset);
				return Blending( color, value, colorBlendRatio, alphaBlendRatio);
			}
            ENDCG
        }
    }
}
