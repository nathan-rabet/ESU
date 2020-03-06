﻿// Copyright © 2014 Laurens Mathot
// Code Animo™ http://codeanimo.com
// License terms can be found at the bottom of this file.

Shader "QuantumTheory/VertexColors/Unity5/Diffuse Alpha2"{
	Properties {
		//_DiffuseIBL("Diffuse IBL", CUBE) = "" {}
		//_IBLBrightness("IBL Brightness", Float) = 1
		_Opacity("Transparency",Range(0,1))=1	
	}
	CGINCLUDE
	

	
	ENDCG
	
	SubShader {
		
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		 
    Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
		
		
		// Surface Shader Passes:
		CGPROGRAM
		#pragma surface surf Lambert addshadow  alpha:fade
		#pragma target 3.0
		#pragma glsl
		#include "QT_Cubemaps.cginc"
		
		float _Opacity;

		struct Input {
			float4 color : COLOR;
			float3 worldNormal;
		};
	
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = IN.color.rgb;	
			
			o.Alpha=_Opacity;
		}
		ENDCG
		
		// Vertex Lit:
		Pass {
			Tags { "LightMode" = "Vertex" "RenderType" = "Tranparent"
            "Queue" = "Transparent"}
			CGPROGRAM
			#pragma vertex vert
			#pragma multi_compile_fog
			#pragma fragment frag
			#pragma target 3.0
			#pragma glsl
			
			#include "UnityCG.cginc"
			#include "VertexLitIBL.cginc"
			
			
			
			ENDCG
		}
		// Vertex Lit Lightmapped:
		Pass {
			Tags { "LightMode" = "VertexLM""RenderType" = "Tranparent"
            "Queue" = "Transparent" }
			CGPROGRAM
			#pragma vertex vert
			#pragma multi_compile_fog
			#pragma fragment frag
			#pragma target 3.0
			#pragma glsl
			
			#include "UnityCG.cginc"
			#include "VertexLitIBL.cginc"
			
			ENDCG
		}
		Pass {
			Tags { "LightMode" = "VertexLMRGBM" }
			CGPROGRAM
			#pragma vertex vert
			#pragma multi_compile_fog
			#pragma fragment frag
			#pragma target 3.0
			#pragma glsl
			
			#include "UnityCG.cginc"
			#include "VertexLitIBL.cginc"
			
			ENDCG
		}
	}
	Fallback "QuantumTheory/VertexColors/VertexLit"
	
	
}

// Copyright © 2014 Laurens Mathot
// Code Animo™ http://codeanimo.com
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.