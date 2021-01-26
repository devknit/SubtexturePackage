
#ifndef __SUBTEXTURE_NOISE_CGINC__
#define __SUBTEXTURE_NOISE_CGINC__

// https://thebookofshaders.com/
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
//#pragma exclude_renderers d3d11 gles

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
float fBm( float2 v, int octave, float amplitude[8], float lacunarity[8], float rotation[8], float2 shift[8])
{
	float n, c, s, f = 0.0;
	float2x2 m;
	
	for( int i0 = 0; i0 < octave; ++i0)
	{
		c = cos( rotation[ i0]);
		s = sin( rotation[ i0]);
		m = float2x2( c, s, -s, c);
		v = mul( m, v) * lacunarity[ i0] + shift[ i0];
		n = perlinNoise( v);
	//	n = abs( n);
	//	n = n * -1;
	//	n = n * n;
		f += amplitude[i0] * n;
		
	}
	return f;
}
//https://thebookofshaders.com/12/?lan=jp
float cellularNoise( float2 v, float t)
{
	float2 ist = floor( v);
	float2 fst = frac( v);
	float minDistance = 1.0;
	
	for( int y = -1; y <= 1; ++y)
	{
		for( int x = -1; x <= 1; ++x)
		{
			/* マスの基点 */
			float2 neighbor = float2( x, y);
			
			/* マスの起点を基準にした点の座標 */
			float2 pt = random2( ist + neighbor);
			pt = 0.5 + 0.5 * sin( t + 6.2831 * pt);
			
			/* 基点と点の距離 */
			float distance = length( neighbor + pt - fst);
			
			/* 近い距離を保つ */
			minDistance = min( minDistance, distance);
		}
	}
	return minDistance;
}
float3 voronoiNoise( float2 v, float t)
{
	float2 ist = floor( v);
	float2 fst = frac( v);
	float2 minPoint;
	float minDistance = 10.0;
	float distance;
	
	for( int y = -1; y <= 1; ++y)
	{
		for( int x = -1; x <= 1; ++x)
		{
			float2 neighbor = float2( x, y);
			float2 pt = random2( ist + neighbor);
			pt = 0.5 + 0.5 * sin( pt * 6.2831853 + t);
			distance = length( neighbor + pt - fst);
			
			if( minDistance > distance)
			{
				minDistance = distance;
				minPoint = pt;
			}
		}
	}
	return float3( minPoint, minDistance);
}
float3 voronoiNoise2( float2 v, float t)
{
	float2 ist = floor( v);
	float2 fst = frac( v);
	float2 minDifference;
	float2 minNeighbor;
	float2 minPoint;
	float minDistance = 10.0;
	float distance;
	int x, y;
	
	for( y = -1; y <= 1; ++y)
	{
		for( x = -1; x <= 1; ++x)
		{
			float2 neighbor = float2( x, y);
			float2 pt = random2( ist + neighbor);
			pt = 0.5 + 0.5 * sin( pt * 6.2831853 + t);
			float2 difference = neighbor + pt - fst;
			distance = length( difference);
			
			if( minDistance > distance)
			{
				minDifference = difference;
				minDistance = distance;
				minNeighbor = neighbor;
				minPoint = pt;
			}
		}
	}
	minDistance = 10.0;
	
	for( y = -2; y <= 2; ++y)
	{
		for( x = -2; x <= 2; ++x)
		{
			if( x == 0 && y == 0)
			{
				continue;
			}
			float2 neighbor = minNeighbor + float2( x, y);
			float2 pt = random2( ist + neighbor);
			pt = 0.5 + 0.5 * sin( pt * 6.2831853 + t);
			float2 difference = neighbor + pt - fst;
			distance = dot( 0.5 * (minDifference + difference), normalize( difference - minDifference));
			minPoint = pt;
			minDistance = min( minDistance, distance);
		}
	}
	return float3( minPoint, minDistance);
}
#endif /* __SUBTEXTURE_NOISE_CGINC__ */
