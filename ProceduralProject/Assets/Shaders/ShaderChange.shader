Shader "TobysShaders/Disolve"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Noise", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Threshold ("Threshold", Range(0,1)) = 0.5
        _TimeOffset ("Time Offset", Float) = 0.5
        _MusicChange ("Music Change", Float) = 0.0
        _Extrude ("Extrusion", Range(-5, 10)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opague"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows
        #pragma vertex vert
        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Threshold;
        float _TimeOffset;
        float _MusicChange;
        float _Extrude;

        void vert(inout appdata_base DATA){
            // move the vertex

            float wave = sin(_Time.y);

            DATA.vertex += float4(DATA.normal, 0) * _Extrude * wave;
        }

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {

            float4 noise = tex2D(_MainTex, IN.uv_MainTex);

            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;

            float t = .3 - (_MusicChange * 30);

                if(noise.r > t){
                c = float4(0,0,0,0);
                clip(-1);

                }

            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            if(noise.r > t -.1 && noise.r < t){
                o.Emission = float4 (1, .8, .2, 1);
            }


        }
        ENDCG
    }
    FallBack "Diffuse"
}
