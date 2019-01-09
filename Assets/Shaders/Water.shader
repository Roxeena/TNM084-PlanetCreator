﻿Shader "Custom/Water" {
	Properties {
		_WaterColor ("Color of the water", Color) = (0,0,1,1)
    _WaveColor("Color of the waves od the water", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
    _Amount("Amount", Range(0,0.2)) = 0.01
    _WaveAmount("Amount of wave color", Range(0,1)) = 0.5
    _Freq("Frequency", Range(5,20)) = 10.0
    _WaterSpeed("Speed of the water", Range(0,1)) = 0.2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _WaterColor;
    fixed4 _WaveColor;
    float _Amount;
    float _WaveAmount;
    float _Freq;
    float _WaterSpeed;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

    float3 mod289(float3 x) {
      return x - floor(x * (1.0 / 289.0)) * 289.0;
    }

    float4 mod289(float4 x) {
      return x - floor(x * (1.0 / 289.0)) * 289.0;
    }

    float4 permute(float4 x) {
      return mod289(((x*34.0) + 1.0)*x);
    }

    float4 taylorInvSqrt(float4 r)
    {
      return 1.79284291400159 - 0.85373472095314 * r;
    }

    float snoise(float3 v)
    {
      const float2  C = float2(1.0 / 6.0, 1.0 / 3.0);
      const float4  D = float4(0.0, 0.5, 1.0, 2.0);

      // First corner
      float3 i = floor(v + dot(v, C.yyy));
      float3 x0 = v - i + dot(i, C.xxx);

      // Other corners
      float3 g = step(x0.yzx, x0.xyz);
      float3 l = 1.0 - g;
      float3 i1 = min(g.xyz, l.zxy);
      float3 i2 = max(g.xyz, l.zxy);

      //   x0 = x0 - 0.0 + 0.0 * C.xxx;
      //   x1 = x0 - i1  + 1.0 * C.xxx;
      //   x2 = x0 - i2  + 2.0 * C.xxx;
      //   x3 = x0 - 1.0 + 3.0 * C.xxx;
      float3 x1 = x0 - i1 + C.xxx;
      float3 x2 = x0 - i2 + C.yyy; // 2.0*C.x = 1/3 = C.y
      float3 x3 = x0 - D.yyy;      // -1.0+3.0*C.x = -0.5 = -D.y

                                   // Permutations
      i = mod289(i);
      float4 p = permute(permute(permute(
        i.z + float4(0.0, i1.z, i2.z, 1.0))
        + i.y + float4(0.0, i1.y, i2.y, 1.0))
        + i.x + float4(0.0, i1.x, i2.x, 1.0));

      // Gradients: 7x7 points over a square, mapped onto an octahedron.
      // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
      float n_ = 0.142857142857; // 1.0/7.0
      float3  ns = n_ * D.wyz - D.xzx;

      float4 j = p - 49.0 * floor(p * ns.z * ns.z);  //  mod(p,7*7)

      float4 x_ = floor(j * ns.z);
      float4 y_ = floor(j - 7.0 * x_);    // mod(j,N)

      float4 x = x_ * ns.x + ns.yyyy;
      float4 y = y_ * ns.x + ns.yyyy;
      float4 h = 1.0 - abs(x) - abs(y);

      float4 b0 = float4(x.xy, y.xy);
      float4 b1 = float4(x.zw, y.zw);

      //float4 s0 = float4(lessThan(b0,0.0))*2.0 - 1.0;
      //float4 s1 = float4(lessThan(b1,0.0))*2.0 - 1.0;
      float4 s0 = floor(b0)*2.0 + 1.0;
      float4 s1 = floor(b1)*2.0 + 1.0;
      float4 sh = -step(h, float4(0.0, 0.0, 0.0, 0.0));

      float4 a0 = b0.xzyw + s0.xzyw*sh.xxyy;
      float4 a1 = b1.xzyw + s1.xzyw*sh.zzww;

      float3 p0 = float3(a0.xy, h.x);
      float3 p1 = float3(a0.zw, h.y);
      float3 p2 = float3(a1.xy, h.z);
      float3 p3 = float3(a1.zw, h.w);

      //Normalise gradients
      float4 norm = taylorInvSqrt(float4(dot(p0, p0), dot(p1, p1), dot(p2, p2), dot(p3, p3)));
      p0 *= norm.x;
      p1 *= norm.y;
      p2 *= norm.z;
      p3 *= norm.w;

      // Mix final noise value
      float4 m = max(0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0);
      m = m * m;
      return 42.0 * dot(m*m, float4(dot(p0, x0), dot(p1, x1),
        dot(p2, x2), dot(p3, x3)));
    }


    void vert(inout appdata_full v) {
      if (_Amount > 0.001f && _Freq > 0.01f)
      {
        v.vertex.x += (_Amount * sin(_Freq * v.vertex.x * _Time.y * _WaterSpeed) + 0.5f * _Amount * sin(_Freq * 2.0f * v.vertex.x * _Time.y * _WaterSpeed));
        v.vertex.y += (_Amount * sin(_Freq * v.vertex.y * _Time.y * _WaterSpeed) + 0.5f * _Amount * sin(_Freq * 2.0f * v.vertex.y * _Time.y * _WaterSpeed));
        v.vertex.z += (_Amount * sin(_Freq * v.vertex.z * _Time.y * _WaterSpeed) + 0.5f * _Amount * sin(_Freq * 2.0f * v.vertex.z * _Time.y * _WaterSpeed));
      }
    }

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _WaterColor;
      float4 c = _WaterColor; 
      float3 temp = float3(IN.uv_MainTex.x, IN.uv_MainTex.y, _Time.y * _WaterSpeed);
      c.rgb = lerp(c.rgb, _WaveColor.rgb, _WaveAmount * snoise(_Freq * temp));

			o.Albedo = c.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
