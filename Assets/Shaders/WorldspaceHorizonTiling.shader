// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/WorldspaceHorizonTiling"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BumpScale("Scale", float) = 1.0
        [Normal] _NormalMap("Normal Map", 2D) = "bump" {}

        _Height("Height Scale", Range(0.005, 0.08)) = 0.02
        _HeightMap("Height Map", 2D) = "black" {}

    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 300

        //Pass
        //{
        //    CGPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag               
        //    #include "UnityCG.cginc"

        //    sampler2D _NormalMap;
        //    sampler2D _HeightMap;
        //    half _BumpAmount;
        //    half _Height;

        //    struct appdata
        //    {
        //        float4 vertex : POSITION;
        //        float2 uv : TEXCOORD0;
        //    };

        //    struct v2f
        //    {
        //        float2 uv : TEXCOORD0;
        //        float4 vertex : SV_POSITION;
        //    };

        //    sampler2D _MainTex;
        //    float4 _MainTex_ST;

        //    v2f vert(appdata v)
        //    {
        //        v2f o;
        //        o.vertex = UnityObjectToClipPos(v.vertex);

        //        // Gets the xy position of the vertex in worldspace.
        //        float2 worldXY = mul(unity_ObjectToWorld, v.vertex).xz;
        //        // Use the worldspace coords instead of the mesh's UVs.
        //        o.uv = TRANSFORM_TEX(worldXY, _MainTex);

        //        return o;
        //    }

        //    fixed4 frag(v2f i) : SV_Target
        //    {
        //        fixed4 col = tex2D(_MainTex, i.uv);
        //        return col;
        //    }
        //    ENDCG
        //}       
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag               
            #include "UnityCG.cginc"

            sampler2D _NormalMap;
            sampler2D _HeightMap;
            half _BumpAmount;
            half _Height;

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
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Gets the xy position of the vertex in worldspace.
                float2 worldXY = mul(unity_ObjectToWorld, v.vertex).xz;
                // Use the worldspace coords instead of the mesh's UVs.
                o.uv = TRANSFORM_TEX(worldXY, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}