#ifndef __RAYMARCHER_HLSL__
#define __RAYMARCHER_HLSL__

typedef float4x4 Transform;
#define IdentityTransform float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1)
#define PI 3.14159265359
float4x4 RotationX(float theta) 
{
	float4x4 mat = 0.0f;
	mat[0] = float4(1, 0, 0, 0);
	mat[1] = float4(0, cos(theta), -sin(theta), 0);
	mat[2] = float4(0, sin(theta), cos(theta), 0);
	mat[3] = float4(0, 0, 0, 1);

	return mat;
}

float4x4 RotationY(float theta)
{
	float4x4 mat = 0.0f;
	mat[0] = float4(cos(theta), 0, -sin(theta), 0);
	mat[1] = float4(0, 1, 0, 0);
	mat[2] = float4(sin(theta), 0, cos(theta), 0);
	mat[3] = float4(0, 0, 0, 1);

	return mat;
}

float4x4 RotationZ(float theta)
{
	float4x4 mat = 0.0f;
	mat[0] = float4(cos(theta), -sin(theta), 0, 0);
	mat[1] = float4(sin(theta), cos(theta), 0, 0);
	mat[2] = float4(0, 0, 1, 0);
	mat[3] = float4(0, 0, 0, 1);

	return mat;
}

float4x4 CompositeRotation(float3 rotations) 
{
	float xr = rotations.x;
	float yr = rotations.y;
	float zr = rotations.z;

	float4x4 mx = RotationX(-xr);
	float4x4 my = RotationY(yr);
	float4x4 mz = RotationZ(-zr);

	return mul(mz, mul(mx, my));
	
}

Transform CreateTransform(float4 scale, float4 orientation, float4 translation) 
{
	float4x4 _scale = 0.0f;
	float4x4 _orientation = 0.0f;
	float4x4 _translation = 0.0f;

	_translation[0] = float4(1, 0, 0, -translation.x);
	_translation[1] = float4(0, 1, 0, -translation.y);
	_translation[2] = float4(0, 0, 1, -translation.z);
	_translation[3] = float4(0, 0, 0, 1);

	_scale[0] = float4(1/scale.x, 0, 0, 0);
	_scale[1] = float4(0, 1/scale.y, 0, 0);
	_scale[2] = float4(0, 0, 1/scale.z, 0);
	_scale[3] = float4(0, 0, 0, 1);

	float4x4 _rotations = CompositeRotation(orientation);

	return mul(mul(_rotations, _scale), _translation);
}

interface iSDF 
{
	//Returns  a distance and a hue
	float2 Evaluate(float3 pos);
};


class iRay 
{
	float3 ray_origin;
	float3 ray_direction;

	inline float3 Project(float t) {
		return (ray_origin + (ray_direction * t));
	}
};


//ray_direction is the normalized direction
float2 Raymarch(iSDF sdf, float3 ray_origin, float3 ray_direction, float bound)
{
	int count = 0;
	float epsilon = 0.0001f;
	float3 norm_ray = normalize(ray_direction);

	float2 d = 0;
	float t = 0;

	iRay ray;
	ray.ray_origin = ray_origin;
	ray.ray_direction = norm_ray;


	while (t < bound) {
		d = sdf.Evaluate(ray.Project(t));
		if (d.x < epsilon) {
			return float2(t, d.y);
		}
		else {

			t += d;
			count += 1;
		}
		if (t > bound) {
			return float2(bound, d.y);
		}
		if (count > 1000) {
			return float2(bound, d.y);
		}
	}

	return float2(bound, d.y);

}

//val1 and val2 are doubles (distance, hue)
float2 sdfUnion(float2 val1, float2 val2)
{
	return val1.x < val2.x ? val1 : val2;
}

//val1 and val2 are doubles (distance, hue)
float2 sdfIntersection(float2 val1, float2 val2)
{
	return val1.x > val2.x ? val1 : val2;
}

float3 GetNormal(iSDF sdf, float3 pos) 
{
	static const float2 eps = float2(0.0001f, 0.0f);
	
	return normalize(float3(sdf.Evaluate(pos + eps.xyyy).x - sdf.Evaluate(pos - eps.xyyy).x,
					 sdf.Evaluate(pos + eps.yxyy).x - sdf.Evaluate(pos - eps.yxyy).x,
					 sdf.Evaluate(pos + eps.yyxy).x - sdf.Evaluate(pos - eps.yyxy).x//,
					 //sdf.Evaluate(pos + eps.yyyx) - sdf.Evaluate(pos - eps.yyyx)
					));
}

//uv is [-.5, .5] for u and v
float3 GetRays(float3 ray_origin, float2 uv, float2 planedistace) {

	return 0;
}



class SphereSDF : iSDF
{
	Transform t;
	float radius;
	float hue;
	float2 Evaluate(float3 pos) {
		float4 homo_pos = float4(pos, 1.0);
		pos = mul(t, homo_pos);
		return float2(length(pos.xyz) - radius, hue);
	}
};

class TorusSDF : iSDF
{
	Transform t;
	float2 radius;
	float hue;
	float2 Evaluate(float3 pos) {
		float4 homo_pos = float4(pos, 1.0);
		pos = mul(t, homo_pos);
		float2 q = float2(length(pos.xz) - radius.x, pos.y);
		return float2(length(q) - radius.y, hue);
	}
};

class PlaneSDF : iSDF
{
	Transform t;
	float3 n;
	float hue;
	float2 Evaluate(float3 pos) {
		float4 homo_pos = float4(pos, 1.0);
		pos = mul(t, homo_pos);
		return float2(dot(pos, n.xyz), hue);
	}
};

class PyramidSDF : iSDF 
{
	Transform t;
	float2 h;
	float hue;

	//float2 Evaluate(float3 pos) {
	//	float4 homo_pos = float4(pos, 1.0f);
	//	float3 p = mul(t, homo_pos);
	//	//float m2 = h * h + 0.25;
	//	float m2 = h * h + 0.25;

	//	p.xz = abs(p.xz);
	//	p.xz = (p.z > p.x) ? p.zx : p.xz;
	//	p.xz -= 0.5;

	//	float3 q = float3(p.z, h * p.y - 0.5 * p.x, h * p.x + 0.5 * p.y);

	//	float s = max(-q.x, 0.0);
	//	float t = clamp((q.y - 0.5 * p.z) / (m2 + 0.25), 0.0, 1.0);

	//	float a = m2 * (q.x + s) * (q.x + s) + q.y * q.y;
	//	float b = m2 * (q.x + 0.5 * t) * (q.x + 0.5 * t) + (q.y - m2 * t) * (q.y - m2 * t);

	//	float d2 = min(q.y, -q.x * m2 - q.y * 0.5) > 0.0 ? 0.0 : min(a, b);

	//	return float2(sqrt((d2 + q.z * q.z) / m2) * sign(max(q.z, -p.y)), hue);
	//}
	
	float2 Evaluate(float3 pos) {
		float4 homo_pos = float4(pos, 1.0f);
		float3 p = mul(t, homo_pos);
		float3 q = abs(p);
		return max(q.z - h.y, max(q.x * 0.866025 + p.y * 0.5, -p.y) - h.x * 0.5);
	}
};


class RectangleSDF : iSDF
{
	Transform t;
	float3 b;
	float hue;
	float2 Evaluate(float3 pos) 
	{
		float4 homo_pos = float4(pos, 1.0);
		pos = mul(t, homo_pos);
		float3 q = abs(pos) - b;
		return float2(length(max(q, 0.0)) + min(max(q.x, max(q.y, q.z)), 0.0), hue);
	}
};
#endif //__RAYMARCHER_HLSL__