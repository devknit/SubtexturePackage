Shader "Hidden/Subtexture/PostProcess/Swizzle"
{
	Properties
	{
		_MainTex( "Base Map", 2D) = "white" {}
		_UVScale( "UV Scale", Vector) = ( 1, 1, 1, 1)
        _UVOffset( "UV Offset", Vector) = ( 0, 0, 0, 0)
        _SwizzleType( "Swizzle Type", Vector) = ( 0, 1, 2, 3)
        _SwizzleValue( "Swizzle Value", Vector) = ( 0, 0, 0, 0)
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
				UNITY_DEFINE_INSTANCED_PROP( int4, _SwizzleType)
				UNITY_DEFINE_INSTANCED_PROP( float4, _SwizzleValue)
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
				int4 swizzleType = UNITY_ACCESS_INSTANCED_PROP( Props, _SwizzleType);
				float4 swizzleValue = UNITY_ACCESS_INSTANCED_PROP( Props, _SwizzleValue);
				float4 c = tex2D( _MainTex, i.uv0 * uvScale + uvOffset);
				float4 color;
				
				if( swizzleType.x == 0)
				{
					color.x = c.x;
				}
				else if( swizzleType.x == 1)
				{
					color.x = c.y;
				}
				else if( swizzleType.x == 2)
				{
					color.x = c.z;
				}
				else if( swizzleType.x == 3)
				{
					color.x = c.w;
				}
				else
				{
					color.x = swizzleValue.x;
				}
				if( swizzleType.y == 0)
				{
					color.y = c.x;
				}
				else if( swizzleType.y == 1)
				{
					color.y = c.y;
				}
				else if( swizzleType.y == 2)
				{
					color.y = c.z;
				}
				else if( swizzleType.y == 3)
				{
					color.y = c.w;
				}
				else
				{
					color.y = swizzleValue.y;
				}
				if( swizzleType.z == 0)
				{
					color.z = c.x;
				}
				else if( swizzleType.z == 1)
				{
					color.z = c.y;
				}
				else if( swizzleType.z == 2)
				{
					color.z = c.z;
				}
				else if( swizzleType.z == 3)
				{
					color.z = c.w;
				}
				else
				{
					color.z = swizzleValue.z;
				}
				if( swizzleType.w == 0)
				{
					color.w = c.x;
				}
				else if( swizzleType.w == 1)
				{
					color.w = c.y;
				}
				else if( swizzleType.w == 2)
				{
					color.w = c.z;
				}
				else if( swizzleType.w == 3)
				{
					color.w = c.w;
				}
				else
				{
					color.w = swizzleValue.w;
				}
				return color;
			}
            ENDCG
        }
    }
}
