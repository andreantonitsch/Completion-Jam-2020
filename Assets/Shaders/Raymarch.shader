﻿Shader "Unlit/Raymarch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _BackgroundColor("Background Color", Color) = (0,0,0,0)

        _SDFColor("SDF Color", Color) = (1,1,1,1)
        _RimColor("Rim Color", Color) = (1,1,1,1)
		_RimThreshold("Rim Threshold", Float) = 10.0

        _BoundDistance("BoundDistance", Float) = 10.0
        _FOV("FoV", Vector) = (60, 60, 0, 0)

		_JelloShape("Jello Shape", Vector) = (1,0,0,0)

        _JelloPosition("Jello Position", Vector) = (0,0,0,0)
        _JelloRotation("Jello Rotation", Vector) = (0,0,0,0)
        _JelloScale("Jello Scale", Vector) = (1,1,1,0)

		_JelloRoundFactor("JellowRoundFactor", Float) = 0.2

        _LightPos("Light Position", Vector) = (5, 10, 10, 0)
        _LightIntensity("Light Intensity", Float) = 10.0

        _Test("Test", Float) = 1.0
        [Toggle(DEBUG_MODE)] _Debug("Debug Mode",  Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ DEBUG_MODE
			#pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
			#include "Include/raymarcher.hlsl"
			#include "Include/hsv.hlsl"
			#include "Lighting.cginc"

			#include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;	
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			float4 _MainTex_ST, _RayOrigin, _CameraTarget;
			float4  _CameraPosition, _CameraOrientation;
			float4 _BackgroundColor, _SDFColor, _RimColor;

			float4 _JelloPosition, _JelloRotation, _JelloScale, _JelloShape;
			float _JelloRoundFactor;
			float4 _LightPos;
			float _Test;
            float _BoundDistance, _LightIntensity, _RimThreshold;
            float3 _FOV;

            
            float remap_range(float x, float2 old_range, float2 new_range) {
                x -= old_range.x;
                x /= (old_range.y - old_range.x);
                x *= (new_range.y - new_range.x);
                return x + new_range.x;
            }

			float4x4 Translation(float4 pos) 
			{
				float4x4 mat = 0;
				mat[0] = float4(1, 0, 0, pos.x);
				mat[1] = float4(0, 1, 0, pos.y);
				mat[2] = float4(0, 0, 1, pos.z);
				mat[3] = float4(0, 0, 0 , 1);
			
				return mat;
			}


            float4x4 Cam2World(float3 camera_orientation, float3 camera_position)
            {
                float4x4 _translation;
                _translation[0] = float4(1, 0, 0, camera_position.x);
                _translation[1] = float4(0, 1, 0, camera_position.y);
                _translation[2] = float4(0, 0, 1, camera_position.z);
                _translation[3] = float4(0, 0, 0, 1);

                float4x4 _rotation = CompositeRotation(camera_orientation);

                return mul(_translation, _rotation);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
				
                return o;
            }

            float3x3 set_camera(float3 ray_origin, float3 target, float camera_rotation) {
                float3 cw = normalize(target - ray_origin);
                float3 cp = float3(sin(camera_rotation), cos(camera_rotation), 0.0);
                float3 cu = normalize(cross(cw, cp));
                float3 cv = (cross(cu, cw));
                return float3x3(cu, cv, cw);
            }


            class ShapeMap : iSDF
            {
                Transform t;
                float2 Evaluate(float3 pos)
                {
                    SphereSDF sphere;
                    //TorusSDF torus;
					RectangleSDF cube;
					PyramidSDF pyramid;

					pyramid.h = 0.7f;
					pyramid.t = t;
					pyramid.hue = 90;
					cube.t = t;
					cube.b = 0.3;
					cube.hue = 70;

                    //Shape parameters
                    sphere.radius = .5;

					sphere.t = t;
					sphere.hue = 60.0;

                    float2 d = float2(_BoundDistance, 60);

					float2 d2 = sphere.Evaluate(pos) - _JelloRoundFactor;
					float2 d3 = cube.Evaluate(pos) - _JelloRoundFactor;
					float2 d4 = pyramid.Evaluate(pos + float3(0,0.4,0)) - _JelloRoundFactor;
					
					float d6 = _JelloShape.x * d2 + _JelloShape.y * d3 + _JelloShape.z * d4;

					//float2 d5 = lerp(lerp(d2, d3, abs(sin(_Time.x * 40))), d4, abs(sin(_Time.x * 60)));
					d = sdfUnion(d, d6);
					
                    return d;
                }

            };


			fixed4 frag(v2f i) : SV_Target
			{
				// Directional light direction
				float4 light_dir = normalize(float4(_WorldSpaceLightPos0));

				//depth texture depth
				float4 depth = tex2D(_CameraDepthTexture, i.uv).r;
				depth = LinearEyeDepth(depth);

				// Screenspace uv
                float2 uv = ((i.uv - .5f) *2) * _ScreenParams.xy / _ScreenParams.y;
                uv.x *= -1;
                float2 screen_pos = uv * _FOV;


                ///// Raymarch Camera Parameters
                float3 up = float3(0.0, 1.0, 0.0);
                float3 tgt = _CameraTarget;
                float3 dir = normalize(tgt - _CameraPosition);
                float3 cam_right = normalize(cross(dir, up ));
                float3 cam_up = -normalize(cross(cam_right, dir));
                float3 p = normalize(cam_right * screen_pos.x + cam_up * screen_pos.y + dir);
                

                //Jello transform
                //Transform t = CreateTransform(_JelloScale, _JelloRotation * float4((_Time.x) / 14.0, (_Time.x % 1000.0) / 10.0, (_Time.x % 1000.0) / 18.0, 1.0), _JelloPosition);
                Transform t = CreateTransform(_JelloScale, _JelloRotation, _JelloPosition);

				// Jello SDF parameters
                ShapeMap shapeMap;
                shapeMap.t = t;

                //Raymarch for depth and color
                float2 d = Raymarch(shapeMap, _CameraPosition, p, _BoundDistance);
				float3 shape_color = HSV2RGB(float3(d.y / 360.0, 1.0, 1.0));
				
				// Draw SDF shape if less than bounding volume
                float draw = d.x < _BoundDistance;

				// sdf pixel world position
                float4 world_pos = float4((_CameraPosition + d.x * p).xyz, 1.0f);

                //Shape normals
                //Gets normals in world space
                float3 normal = GetNormal(shapeMap, world_pos);

                
                //Jello Solid Lambertian lighting
                //float3 light_vec = _LightPos.xyz - world_pos.xyz;
				float3 light_vec = light_dir;
                float light_sqdist = dot(light_vec, light_vec);
                float3 light_direction = normalize(light_vec);
                float light_intensity = _LightIntensity;
                light_intensity *= draw ? dot(light_direction, normal) : 0.0f ;
				
				// Light intensity and color at the point
                float3 ilum_color = _LightColor0 * light_intensity;

                //Jello rim lighting
                float view_normal = dot(p, normal);
                float rim_dir_value = smoothstep(0.0, 1.0, max(0, dot(normal, normalize(- 2* cam_right +  cam_up))));
                float rim_intensity = remap_range(view_normal, float2(-1, 1), float2(0, _RimThreshold)) ;
                rim_intensity = smoothstep(-_RimThreshold, 1, view_normal/ rim_dir_value);
				//rim_intensity = view_normal / rim_dir_value > _RimThreshold;
                //rim_intensity = view_normal / rim_dir_value > _RimThreshold;
                
				
				// TODO Add Jello Specular Lighting
				float3 b_p_h = normalize(light_dir - p);
				float b_p = dot(b_p_h, normal);
				float specular_intensity = pow(b_p,_Test) * draw;

                // Color  + Lighting + Rim lighting
                float3 color = ( shape_color ) * (ilum_color);
                color = (rim_intensity ? _RimColor : color) + specular_intensity * ilum_color;



				// Scene Image
				float4 unity_cam = tex2D(_MainTex, i.uv);

#ifdef DEBUG_MODE //Draw Both scene and shapes

                //return float4((unity_cam.xyz + (max(normal, 0) * draw)) / (1+draw), 1);
                //return float4((unity_cam.xyz + (max(color, 0) * draw)) / (1+draw), 1);

                return float4((unity_cam.xyz + (color * draw)) / (1+draw), 1);
                return float4(unity_cam.xyz * draw, 1);
#endif
				// Draw closest between scene and shape
				if (d.x < depth.x)
					return float4(color, 1) * draw;
				else
					return unity_cam;


                //return d.x;
            }
            ENDCG
        }
    }
}