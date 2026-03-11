Shader "Custom/Waves"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Amplitude ("Amplitude", Float) = 0.05
        _Frequency ("Frequency", Float) = 1
        _Speed ("Speed", Float) = 1
        _Phase ("Phase", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Amplitude;
            float _Frequency;
            float _Speed;
            float _Phase;

            v2f vert (appdata_t v)
            {
                v2f o;

                float wave = sin(v.vertex.y * _Frequency + _Time.y * _Speed + _Phase) * _Amplitude;
                v.vertex.x += wave;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}