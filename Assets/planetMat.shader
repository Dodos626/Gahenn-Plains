Shader "Custom/planetMat"
{
    Properties
    {
        _AtmosphereSize("AtmosphereSize", Float) = 5
        _Color("Color", Color) = (0, 0, 0, 0)
        _LightPosition ("Light Position", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            "DisableBatching"="False"
            // "ShaderGraphShader"="true"
            // "ShaderGraphTargetId"="UniversalLitSubTarget"
        }

        LOD 300
        Pass
        {
            Name "Universal Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
            Cull Back
            Blend One One, One One
            ZTest LEqual
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
   
            #include "UnityCG.cginc"


            float fresnelEffect(float3 Normal, float3 ViewDir, float Power)
            {
                return pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
            }

            float multFloat(float A, float B)
            {
                return A * B;
            }
            
            float4 multFloat4(float4 A, float4 B)
            {
                return A * B;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normalWorld : TEXCOORD2;
                float3 posWorld : TEXCOORD3;
                float3 worldSpaceViewDir : TEXCOORD4;
                float3 vertexNormal : TEXCOORD5;
            };

            // fresnel
            //R = max(0, min(1, bias + scale * (1.0 + I • N)power))


            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                float3 posWorld = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.posWorld = posWorld;
                o.normalWorld = normalize(mul(unity_ObjectToWorld, v.normalOS));
                o.vertexNormal = v.normalOS;
                //https://forum.unity.com/threads/math-behind-computing-world-space-view-direction.377631/
                o.worldSpaceViewDir = _WorldSpaceCameraPos.xyz  - posWorld.xyz ;
                return o;
            }

            fixed4 _Color;
            float _AtmosphereSize;
            float4 _LightPosition;



            fixed4 frag (v2f i) : SV_Target {

                // this is what unity uses
                float fresnel = fresnelEffect(i.normalWorld,  normalize(i.worldSpaceViewDir), _AtmosphereSize);

                float secondFresnel = multFloat(fresnel, _AtmosphereSize);
                float4 _multipliedColor = multFloat4(_Color, secondFresnel);

                float _lightPositionCalculation = dot(i.vertexNormal, _LightPosition);

                float _lightPositionFixed = multFloat(_lightPositionCalculation, -1.0);

                float _clampedLight = clamp(_lightPositionFixed, 0.0, 1.0);

                // un comment bellow to check that vertex normal is working 
                // return float4(i.vertexNormal, 1.0);
                // return _Color;
                // return _lightPositionFixed;
                return  multFloat4(_multipliedColor, _clampedLight); 
            }
            ENDCG
 
        }

    }
    FallBack "Diffuse"
}
