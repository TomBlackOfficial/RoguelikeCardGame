Shader "Custom/Dissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount", Range(0,1)) = 0
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)
        _ObjectYPos ("Object Y Position", Float) = 0
        _DissolveHeight ("Dissolve Height", Float) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        sampler2D _DissolveTex;
        half _DissolveAmount;
        fixed4 _EdgeColor;
        float _ObjectYPos;
        float _DissolveHeight;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_DissolveTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
            half dissolveTexValue = tex2D(_DissolveTex, IN.uv_DissolveTex).r;

            // Calculate dissolve factor based on Y position
            half dissolveFactor = saturate((IN.worldPos.y - _ObjectYPos) / _DissolveHeight + _DissolveAmount);

            clip(dissolveTexValue - dissolveFactor);
            if (dissolveTexValue < dissolveFactor + 0.1)
            {
                o.Albedo = _EdgeColor.rgb;
                o.Emission = _EdgeColor.rgb;
            }
            else
            {
                o.Albedo = mainTex.rgb;
                o.Alpha = mainTex.a;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}
