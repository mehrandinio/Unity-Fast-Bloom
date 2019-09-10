Shader "Custom/Wireframe"
{
    Properties
    {
        _Smoothing ("Wireframe Smoothing", Range(0, 0.5)) = 0.01
		_Thickness ("Wireframe Thickness", Range(0, 0.5)) = 0.1
    }

    SubShader
    {
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        CGINCLUDE

        #include "UnityCG.cginc"

        struct VertexInput
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            float2 barycentric : TEXCOORD1;
        };

        struct VertexOutput
        {
            float4 vertex : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 barycentric : TEXCOORD1;
        };

        float _Smoothing;
        float _Thickness;

        float3 convertBarycentric (float2 barycentric)
        {
            float3 barys;
            barys.xy = barycentric;
            barys.z = 1 - barys.x - barys.y;
            return barys;
        }

        VertexOutput vert (VertexInput IN)
        {
            VertexOutput OUT;
            OUT.vertex = UnityObjectToClipPos(IN.vertex);
            OUT.uv = IN.uv;
            OUT.barycentric = convertBarycentric(IN.barycentric);
            return OUT;
        }

        float wireframe (float3 barys)
        {
            barys = smoothstep(_Thickness.xxx, (_Thickness + _Smoothing).xxx, barys);
	        return min(barys.x, min(barys.y, barys.z));
        }

        float4 frag (VertexOutput IN) : SV_Target
        {
            float3 color = float3(IN.uv, abs(cos(_Time.y)));
            float wireAlpha = wireframe(IN.barycentric);
            return float4(color, 1.0 - wireAlpha);
        }

        ENDCG

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDCG
        }
    }
}