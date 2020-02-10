// https://thebookofshaders.com/

#ifndef __SUBTEXTURE_NOISE_CGINC__
#define __SUBTEXTURE_NOISE_CGINC__

//https://thebookofshaders.com/10/?lan=jp
#if 0
float random( float2 v)
{
	return frac( sin( dot( v, float2( 12.9898, 78.233))) * 43758.5453);
}
#else
float random( float2 v)
{
	float a = frac( dot( v, float2( 2.067390879775102, 12.451168662908249))) - 0.5;
	float s = a * (6.182785114200511 + a * a * (-38.026512460676566 + a * a * 53.392573080032137));
	return frac( s * 43758.5453);
}
#endif
float2 random2( float2 v)
{
	v = float2( dot( v,float2( 127.1, 311.7)), dot( v, float2( 269.5, 183.3)));
	return -1.0 + 2.0 * frac( sin( v) * 43758.5453123);
}
float2 fade( float2 t)
{
	return t * t * t * (t * (t * 6.0 - 15.0) + 10.0);
//	return t * t * (3.0 - 2.0 * t);
}
float2 bilinear( float f0, float f1, float f2, float f3, float2 t)
{
    return lerp( lerp( f0, f1, t.x), lerp( f2, f3, t.x), t.y);
}

float blockNoise( float2 v)
{
    return random( floor( v));
}

//https://thebookofshaders.com/11/?lan=jp
float valueNoise( float2 v)
{
    float2 p = floor( v);
    float2 f = frac( v);

    float v00 = random( p + float2( 0, 0));
    float v10 = random( p + float2( 1, 0));
    float v01 = random( p + float2( 0, 1));
    float v11 = random( p + float2( 1, 1));
    
    //float2 t = f * f * (3.0 - 2.0 * f);
    float2 t = fade( f);

	return bilinear( v00, v10, v01, v11, t);
}

float perlinNoise( float2 v)
{
    float2 p = floor( v);
    float2 f = frac( v);
	float2 t = fade( f);

	float v00 = dot( random2( p + float2( 0, 0)), f - float2( 0, 0));
	float v10 = dot( random2( p + float2( 1, 0)), f - float2( 1, 0));
	float v01 = dot( random2( p + float2( 0, 1)), f - float2( 0, 1));
	float v11 = dot( random2( p + float2( 1, 1)), f - float2( 1, 1));
    
    return bilinear( v00, v10, v01, v11, t);
}
/* Fractal Brownian Motion */
//https://thebookofshaders.com/13/?lan=jp
float fBm( float2 v, int octave, float amplitude, float lacunarity, float gain, float angle, float2 shift)
{
	float n;
	float f = 0.0;
	float c = cos( angle);
	float s = sin( angle);
	float2x2 mat = float2x2( c, s, -s, c);
	
	for( int i0 = 0; i0 < octave; ++i0)
	{
		n = perlinNoise( v);
	//	n = abs( n);
	//	n = n * -1;
	//	n = n * n;
		f += amplitude * n;
		v = mul( mat, v) * lacunarity + shift;
		amplitude *= gain;
	}
	return f;
}
//https://thebookofshaders.com/12/?lan=jp
float cellularNoise( float2 v, float scale, float white, float t)
{
	v *= scale;
	
	float2 ist = floor( v);
	float2 fst = frac( v);
	float minDistance = 1.0;
	
	for( int y = -1; y <= 1; ++y)
	{
		for( int x = -1; x <= 1; ++x)
		{
			/* �}�X�̊�_ */
			float2 neighbor = float2( x, y);
			
			/* �}�X�̋N�_����ɂ����_�̍��W */
			float2 pt = random2( ist + neighbor);
			pt = 0.5 + 0.5 * sin( t + 6.2831 * pt);
			
			/* ��_�Ɠ_�̋��� */
			float distance = length( neighbor + pt - fst);
			
			/* �߂�������ۂ� */
			minDistance = min( minDistance, distance);
		}
	}
	return minDistance + (1.0 - step( white, minDistance));
}
float3 voronoiNoise( float2 v, float scale, float white, float t)
{
	v *= scale;
	
	float2 ist = floor( v);
	float2 fst = frac( v);
	float minDistance = 1.0;
	float2 minPoint;
	
	for( int y = -1; y <= 1; ++y)
	{
		for( int x = -1; x <= 1; ++x)
		{
			/* �}�X�̊�_ */
			float2 neighbor = float2( x, y);
			
			/* �}�X�̋N�_����ɂ����_�̍��W */
			float2 pt = random2( ist + neighbor);
			pt = 0.5 + 0.5 * sin( t + 6.2831 * pt);
			
			/* ��_�Ɠ_�̋��� */
			float distance = length( neighbor + pt - fst);
			
			/* �߂�������ۂ� */
			if( minDistance > distance)
			{
				minDistance = distance;
				minPoint = pt;
			}
		}
	}
	return float3( minDistance, minPoint);
}



#endif /* __SUBTEXTURE_NOISE_CGINC__ */
