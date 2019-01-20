Shader "Custom/Sun" {
	Properties {
    _Color1("Color 1", Color) = (1, 1, 0, 1)
    _Color2("Color 2", Color) = (1, 0, 0, 1)
    _Color1Ratio("Color 1 Ratio", Range(0,1)) = 0.5
    _ColorAmount("Amount color", Range(0, 1)) = 1.0
    _ColorFreq("Frequency for color", Range(5,100)) = 10.0
    _ColorEmmision("Color of emmision", Color) = (1,1,1,1)
    _EmmisionAmount("Amount of emmision", Range(0,1)) = 0.9
    _Speed("Speed of gas flow", Range(0,0.2)) = 0.1
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
    #pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color1;
    fixed4 _Color2;
    float _Color1Ratio;
    float _ColorAmount;
    float _ColorFreq;
    fixed4 _ColorEmmision;
    float _EmmisionAmount;
    float _Speed;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

    /***************************************************************************************************/
    // Cellular noise ("Worley noise") in 3D in GLSL. (Translated to HLSL by Malin Ejdbo 2019-01-20)
    // Copyright (c) Stefan Gustavson 2011-04-19. All rights reserved.
    // This code is released under the conditions of the MIT license.
    // See LICENSE file for details.
    // https://github.com/stegu/webgl-noise

    // Modulo 289 without a division (only multiplications)
    float3 mod289(float3 x) {
      return x - floor(x * (1.0 / 289.0)) * 289.0;
    }

    // Modulo 7 without a division
    float3 mod7(float3 x) {
      return x - floor(x * (1.0 / 7.0)) * 7.0;
    }

    // Permutation polynomial: (34x^2 + x) mod 289
    float3 permute(float3 x) {
      return mod289((34.0 * x + 1.0) * x);
    }

    // Cellular noise, returning F1 and F2 in a float2.
    // 3x3x3 search region for good F2 everywhere, but a lot
    // slower than the 2x2x2 version.
    // The code below is a bit scary even to its author,
    // but it has at least half decent performance on a
    // modern GPU. In any case, it beats any software
    // implementation of Worley noise hands down.

    float2 cellular(float3 P) {
      #define K 0.142857142857 // 1/7
      #define Ko 0.428571428571 // 1/2-K/2
      #define K2 0.020408163265306 // 1/(7*7)
      #define Kz 0.166666666667 // 1/6
      #define Kzo 0.416666666667 // 1/2-1/6*2
      #define jitter 1.0 // smaller jitter gives more regular pattern

      float3 Pi = mod289(floor(P));
      float3 Pf = frac(P) - 0.5;

      float3 Pfx = Pf.x + float3(1.0, 0.0, -1.0);
      float3 Pfy = Pf.y + float3(1.0, 0.0, -1.0);
      float3 Pfz = Pf.z + float3(1.0, 0.0, -1.0);

      float3 p = permute(Pi.x + float3(-1.0, 0.0, 1.0));
      float3 p1 = permute(p + Pi.y - 1.0);
      float3 p2 = permute(p + Pi.y);
      float3 p3 = permute(p + Pi.y + 1.0);

      float3 p11 = permute(p1 + Pi.z - 1.0);
      float3 p12 = permute(p1 + Pi.z);
      float3 p13 = permute(p1 + Pi.z + 1.0);

      float3 p21 = permute(p2 + Pi.z - 1.0);
      float3 p22 = permute(p2 + Pi.z);
      float3 p23 = permute(p2 + Pi.z + 1.0);

      float3 p31 = permute(p3 + Pi.z - 1.0);
      float3 p32 = permute(p3 + Pi.z);
      float3 p33 = permute(p3 + Pi.z + 1.0);

      float3 ox11 = frac(p11*K) - Ko;
      float3 oy11 = mod7(floor(p11*K))*K - Ko;
      float3 oz11 = floor(p11*K2)*Kz - Kzo; // p11 < 289 guaranteed

      float3 ox12 = frac(p12*K) - Ko;
      float3 oy12 = mod7(floor(p12*K))*K - Ko;
      float3 oz12 = floor(p12*K2)*Kz - Kzo;

      float3 ox13 = frac(p13*K) - Ko;
      float3 oy13 = mod7(floor(p13*K))*K - Ko;
      float3 oz13 = floor(p13*K2)*Kz - Kzo;

      float3 ox21 = frac(p21*K) - Ko;
      float3 oy21 = mod7(floor(p21*K))*K - Ko;
      float3 oz21 = floor(p21*K2)*Kz - Kzo;

      float3 ox22 = frac(p22*K) - Ko;
      float3 oy22 = mod7(floor(p22*K))*K - Ko;
      float3 oz22 = floor(p22*K2)*Kz - Kzo;

      float3 ox23 = frac(p23*K) - Ko;
      float3 oy23 = mod7(floor(p23*K))*K - Ko;
      float3 oz23 = floor(p23*K2)*Kz - Kzo;

      float3 ox31 = frac(p31*K) - Ko;
      float3 oy31 = mod7(floor(p31*K))*K - Ko;
      float3 oz31 = floor(p31*K2)*Kz - Kzo;

      float3 ox32 = frac(p32*K) - Ko;
      float3 oy32 = mod7(floor(p32*K))*K - Ko;
      float3 oz32 = floor(p32*K2)*Kz - Kzo;

      float3 ox33 = frac(p33*K) - Ko;
      float3 oy33 = mod7(floor(p33*K))*K - Ko;
      float3 oz33 = floor(p33*K2)*Kz - Kzo;

      float3 dx11 = Pfx + jitter * ox11;
      float3 dy11 = Pfy.x + jitter * oy11;
      float3 dz11 = Pfz.x + jitter * oz11;

      float3 dx12 = Pfx + jitter * ox12;
      float3 dy12 = Pfy.x + jitter * oy12;
      float3 dz12 = Pfz.y + jitter * oz12;

      float3 dx13 = Pfx + jitter * ox13;
      float3 dy13 = Pfy.x + jitter * oy13;
      float3 dz13 = Pfz.z + jitter * oz13;

      float3 dx21 = Pfx + jitter * ox21;
      float3 dy21 = Pfy.y + jitter * oy21;
      float3 dz21 = Pfz.x + jitter * oz21;

      float3 dx22 = Pfx + jitter * ox22;
      float3 dy22 = Pfy.y + jitter * oy22;
      float3 dz22 = Pfz.y + jitter * oz22;

      float3 dx23 = Pfx + jitter * ox23;
      float3 dy23 = Pfy.y + jitter * oy23;
      float3 dz23 = Pfz.z + jitter * oz23;

      float3 dx31 = Pfx + jitter * ox31;
      float3 dy31 = Pfy.z + jitter * oy31;
      float3 dz31 = Pfz.x + jitter * oz31;

      float3 dx32 = Pfx + jitter * ox32;
      float3 dy32 = Pfy.z + jitter * oy32;
      float3 dz32 = Pfz.y + jitter * oz32;

      float3 dx33 = Pfx + jitter * ox33;
      float3 dy33 = Pfy.z + jitter * oy33;
      float3 dz33 = Pfz.z + jitter * oz33;

      float3 d11 = dx11 * dx11 + dy11 * dy11 + dz11 * dz11;
      float3 d12 = dx12 * dx12 + dy12 * dy12 + dz12 * dz12;
      float3 d13 = dx13 * dx13 + dy13 * dy13 + dz13 * dz13;
      float3 d21 = dx21 * dx21 + dy21 * dy21 + dz21 * dz21;
      float3 d22 = dx22 * dx22 + dy22 * dy22 + dz22 * dz22;
      float3 d23 = dx23 * dx23 + dy23 * dy23 + dz23 * dz23;
      float3 d31 = dx31 * dx31 + dy31 * dy31 + dz31 * dz31;
      float3 d32 = dx32 * dx32 + dy32 * dy32 + dz32 * dz32;
      float3 d33 = dx33 * dx33 + dy33 * dy33 + dz33 * dz33;

      // Sort out the two smallest distances (F1, F2)

      #if 0
      // Cheat and sort out only F1
      float3 d1 = min(min(d11, d12), d13);
      float3 d2 = min(min(d21, d22), d23);
      float3 d3 = min(min(d31, d32), d33);
      float3 d = min(min(d1, d2), d3);
      d.x = min(min(d.x, d.y), d.z);
      return float2(sqrt(d.x)); // F1 duplicated, no F2 computed

      #else
      // Do it right and sort out both F1 and F2
      float3 d1a = min(d11, d12);
      d12 = max(d11, d12);
      d11 = min(d1a, d13); // Smallest now not in d12 or d13
      d13 = max(d1a, d13);
      d12 = min(d12, d13); // 2nd smallest now not in d13
      float3 d2a = min(d21, d22);
      d22 = max(d21, d22);
      d21 = min(d2a, d23); // Smallest now not in d22 or d23
      d23 = max(d2a, d23);
      d22 = min(d22, d23); // 2nd smallest now not in d23
      float3 d3a = min(d31, d32);
      d32 = max(d31, d32);
      d31 = min(d3a, d33); // Smallest now not in d32 or d33
      d33 = max(d3a, d33);
      d32 = min(d32, d33); // 2nd smallest now not in d33
      float3 da = min(d11, d21);
      d21 = max(d11, d21);
      d11 = min(da, d31); // Smallest now in d11
      d31 = max(da, d31); // 2nd smallest now not in d31
      d11.xy = (d11.x < d11.y) ? d11.xy : d11.yx;
      d11.xz = (d11.x < d11.z) ? d11.xz : d11.zx; // d11.x now smallest
      d12 = min(d12, d21); // 2nd smallest now not in d21
      d12 = min(d12, d22); // nor in d22
      d12 = min(d12, d31); // nor in d31
      d12 = min(d12, d32); // nor in d32
      d11.yz = min(d11.yz, d12.xy); // nor in d12.yz
      d11.y = min(d11.y, d12.z); // Only two more to go
      d11.y = min(d11.y, d11.z); // Done! (Phew!)
      return sqrt(d11.xy); // F1, F2
      #endif
    }
    /***********************************************************************/
   
		void surf (Input IN, inout SurfaceOutputStandard o) {
      fixed4 c = _Color1;
      float2 F = _ColorAmount * cellular(_ColorFreq*float3(IN.uv_MainTex.x, IN.uv_MainTex.y, _Time.y * _Speed)) + _ColorAmount * 0.5f * cellular(_ColorFreq*2.0f*float3(IN.uv_MainTex.x, IN.uv_MainTex.y, _Time.y * _Speed));
      c.rgb = lerp(c.rgb, _Color2, _Color1Ratio * (F.y + F.x) );
			o.Albedo = c.rgb;
      o.Emission = _ColorEmmision * _EmmisionAmount;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
