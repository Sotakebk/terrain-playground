Shader "Playground/Heightmap Shader"
{
    Properties
    {
        _MainTex ("Triplanar", 2D) = "white" {}
        [NoScaleOffset]_HeightTex("Height Map", 2D) = "black" {}
        _HeightScale("Height displacement multiplier", float) = 1
        [NoScaleOffset]_GradientTex("Gradient Map", 2D) = "white" {}
        [NoScaleOffset]_LineTex("Height Map", 2D) = "white" {}
        _LineScale("Lines per one unit of height", float) = 1
        [PerRendererData]_Offset("Offset", Vector) = (0,0,0,0) // specific per renderer
    }

    SubShader
    {
        Tags {
            "LightMode" = "ForwardBase"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct u2v
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0; // gradient map UV, x = height (0, 1), y = dot(normal, vector.up)
                float4 vertex : SV_POSITION; // screenspace position
                float4 diffuse : COLOR; // lightning color
                float3 worldpos : TEXCOORD1; // world space position
                float3 normal : TEXCOORD2; // world space normal
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _HeightTex;
            float4 _Offset;
            float4 _HeightTex_TexelSize;
            float _HeightScale;
            sampler2D _GradientTex;
            sampler2D _LineTex;
            float _LineScale;

            v2f vert(u2v v)
            {
                v2f o;

                float mult_to_tex = 1 / _HeightTex_TexelSize.z;

                float2 UV = v.vertex.xz + _Offset.xy;
                UV *= mult_to_tex;
                UV += _HeightTex_TexelSize.xy * 0.5;
                float h = tex2Dlod(_HeightTex, float4(UV, 0, 0));

                // displace in world coordinates
                v.vertex.y += h * _HeightScale;

                // get dX, dY of the surface
                float xP = tex2Dlod(_HeightTex, float4(UV.x + mult_to_tex, UV.y, 0, 0));
                float xN = tex2Dlod(_HeightTex, float4(UV.x - mult_to_tex, UV.y, 0, 0));

                float yP = tex2Dlod(_HeightTex, float4(UV.x, UV.y + mult_to_tex, 0, 0));
                float yN = tex2Dlod(_HeightTex, float4(UV.x, UV.y - mult_to_tex, 0, 0));
                /*
                float3 dxV = float3(2, (xP - xN) * _HeightScale, 0);
                float3 dyV = float3(0, (yP - yN) * _HeightScale, 2);
                float3 norm = normalize(cross(dyV, dxV));
                */
                float3 norm = float3(-2 * ((xP - xN) * _HeightScale), 4, -2 * ((yP - yN) * _HeightScale));
                norm = normalize(norm);
                // useful for testing
                //norm = UnityObjectToWorldNormal(norm);
                o.normal = norm;
                o.worldpos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;

                float dsin = max(0, dot(norm, _WorldSpaceLightPos0.xyz));
                o.diffuse = dsin * _LightColor0;
                o.diffuse.rgb += ShadeSH9(float4(norm, 1));

                o.uv = float2(h, norm.y); // norm.y == dot(norm, float(0,1,0))
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(norm);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // diffuse
                fixed4 col = i.diffuse;
                // gradient color
                col = tex2D(_GradientTex, i.uv);
                // line texture
                col *= tex2D(_LineTex, float2(i.uv.x * _HeightScale * _LineScale, i.uv.y));
                // triplanar
                i.normal = abs(i.normal);
                fixed4 tripl = tex2D(_MainTex, TRANSFORM_TEX(i.worldpos.xy, _MainTex)) * i.normal.z;
                tripl += tex2D(_MainTex, TRANSFORM_TEX(i.worldpos.xz, _MainTex)) * i.normal.y;
                tripl += tex2D(_MainTex, TRANSFORM_TEX(i.worldpos.yz, _MainTex)) * i.normal.x;
                col *= tripl/length(i.normal);

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}