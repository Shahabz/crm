Shader "Shaders/Additive Culled" {
	Properties {
		_MainTex ("Base", 2D) = "white" {}
	}
	
	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		
		half4 _MainTex_ST;
						
		struct v2f {
			    float4 pos : SV_POSITION;
			    float4 uv : TEXCOORD0;
			    float4 primary:Color;
			};

			v2f vert (appdata_full v)
			{
				float _Dist=80;
				float nechuvstv=0.40;
				
			    v2f o;
			    float4 vPos = mul (UNITY_MATRIX_MV, v.vertex);
			    
			    float xsmeh=sin(_WorldSpaceCameraPos.z/120);
			 			    
			    float zOff = vPos.z/_Dist;
			    
			    float4	_QOffset=float4(14*xsmeh,-12,0,0);
			    
			    if(zOff<-nechuvstv)
			    {
			    	zOff+=nechuvstv;
			   		vPos += _QOffset*zOff*zOff;
			    }
			    
			    o.pos = mul (UNITY_MATRIX_P, vPos);
			    o.uv = v.texcoord;
			    o.primary=v.color;
			    return o;
			}
		
		fixed4 frag( v2f i ) : COLOR {	
			half4 col = tex2D(_MainTex, i.uv.xy);
			col*=i.primary;
			return col;
		}
	
	ENDCG
	
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		AlphaTest Greater .01
		ColorMask RGB
		Cull Back Lighting Off ZWrite Off Fog { Mode Off }
		//BindChannels {
		//	Bind "Color", color
		//	Bind "Vertex", vertex
		//	Bind "TexCoord", texcoord
		//}
		
	Pass {
	
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		//#pragma fragmentoption ARB_precision_hint_fastest 
		
		ENDCG
		 
		} 
		
	}
	FallBack Off
}
