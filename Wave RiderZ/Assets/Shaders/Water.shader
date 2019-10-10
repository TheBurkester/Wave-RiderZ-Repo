// Shader created with Shader Forge v1.37 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.37;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:2865,x:33720,y:32567,varname:node_2865,prsc:2|diff-3998-OUT,spec-8242-OUT,gloss-1813-OUT,normal-2297-OUT,emission-6596-OUT,transm-4896-OUT,alpha-5628-OUT,refract-840-OUT,voffset-5516-OUT;n:type:ShaderForge.SFN_Slider,id:1813,x:32425,y:32806,ptovrint:False,ptlb:Glossiness,ptin:_Glossiness,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.95,max:5;n:type:ShaderForge.SFN_Vector1,id:8242,x:32603,y:32702,varname:node_8242,prsc:2,v1:0;n:type:ShaderForge.SFN_Color,id:3029,x:32080,y:32361,ptovrint:False,ptlb:Colour (Deep),ptin:_ColourDeep,varname:node_3029,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0.2862745,c3:0.2156863,c4:1;n:type:ShaderForge.SFN_Color,id:8492,x:32080,y:32541,ptovrint:False,ptlb:Colour (Shallow),ptin:_ColourShallow,varname:node_8492,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.4156863,c2:0.7882354,c3:1,c4:1;n:type:ShaderForge.SFN_Lerp,id:3998,x:32466,y:32490,varname:node_3998,prsc:2|A-3029-RGB,B-8492-RGB,T-1885-OUT;n:type:ShaderForge.SFN_Fresnel,id:1885,x:32316,y:32700,varname:node_1885,prsc:2|NRM-5582-OUT,EXP-2711-OUT;n:type:ShaderForge.SFN_NormalVector,id:5582,x:32080,y:32700,prsc:2,pt:True;n:type:ShaderForge.SFN_ValueProperty,id:9570,x:31859,y:32924,ptovrint:False,ptlb:Color Frensnel,ptin:_ColorFrensnel,varname:node_9570,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1.336;n:type:ShaderForge.SFN_ConstantClamp,id:2711,x:32142,y:32898,varname:node_2711,prsc:2,min:0,max:4|IN-9570-OUT;n:type:ShaderForge.SFN_Tex2d,id:4237,x:29360,y:34500,varname:node_4237,prsc:2,ntxv:0,isnm:False|UVIN-4252-OUT,TEX-6590-TEX;n:type:ShaderForge.SFN_Tex2d,id:7804,x:29360,y:34680,varname:node_7804,prsc:2,ntxv:0,isnm:False|UVIN-8445-OUT,TEX-6590-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:6590,x:29127,y:34771,ptovrint:False,ptlb:Normal Map,ptin:_NormalMap,varname:node_6590,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Lerp,id:8576,x:30149,y:34980,varname:node_8576,prsc:2|A-1539-OUT,B-5209-OUT,T-8212-OUT;n:type:ShaderForge.SFN_Slider,id:8212,x:29654,y:35144,ptovrint:False,ptlb:Normal Blend Strength,ptin:_NormalBlendStrength,varname:node_8212,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:5;n:type:ShaderForge.SFN_Time,id:1874,x:30585,y:33969,varname:node_1874,prsc:2;n:type:ShaderForge.SFN_FragmentPosition,id:5764,x:29411,y:32157,varname:node_5764,prsc:2;n:type:ShaderForge.SFN_Append,id:9700,x:29686,y:32117,varname:node_9700,prsc:2|A-5764-X,B-5764-Z;n:type:ShaderForge.SFN_Divide,id:4544,x:29844,y:32254,varname:node_4544,prsc:2|A-9700-OUT,B-1890-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1890,x:29637,y:32345,ptovrint:False,ptlb:UV Scale,ptin:_UVScale,varname:node_1890,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Set,id:3415,x:30042,y:32254,varname:_worldUV,prsc:2|IN-4544-OUT;n:type:ShaderForge.SFN_Get,id:7141,x:29452,y:33477,varname:node_7141,prsc:2|IN-3415-OUT;n:type:ShaderForge.SFN_Set,id:7239,x:30628,y:33293,varname:_UV1,prsc:2|IN-9280-OUT;n:type:ShaderForge.SFN_Set,id:9320,x:30688,y:33591,varname:_UV2,prsc:2|IN-3117-OUT;n:type:ShaderForge.SFN_Vector4Property,id:8138,x:29718,y:33310,ptovrint:False,ptlb:UV 1 Animator,ptin:_UV1Animator,varname:node_8138,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_Vector4Property,id:9873,x:29721,y:33821,ptovrint:False,ptlb:UV 2 Animator,ptin:_UV2Animator,varname:node_9873,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0,v2:0,v3:0,v4:0;n:type:ShaderForge.SFN_ComponentMask,id:9625,x:29996,y:33572,varname:node_9625,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-2395-OUT;n:type:ShaderForge.SFN_ComponentMask,id:5191,x:29996,y:33741,varname:node_5191,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-8179-OUT;n:type:ShaderForge.SFN_Time,id:959,x:29721,y:33982,varname:node_959,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1043,x:29996,y:33398,varname:node_1043,prsc:2|A-8138-Y,B-959-TSL;n:type:ShaderForge.SFN_Multiply,id:9319,x:29996,y:33274,varname:node_9319,prsc:2|A-8138-X,B-959-TSL;n:type:ShaderForge.SFN_Add,id:848,x:30252,y:33398,varname:node_848,prsc:2|A-1043-OUT,B-9625-G;n:type:ShaderForge.SFN_Add,id:1760,x:30252,y:33269,varname:node_1760,prsc:2|A-9319-OUT,B-9625-R;n:type:ShaderForge.SFN_Append,id:9280,x:30447,y:33325,varname:node_9280,prsc:2|A-1760-OUT,B-848-OUT;n:type:ShaderForge.SFN_Add,id:1996,x:30252,y:33556,varname:node_1996,prsc:2|A-5191-R,B-5793-OUT;n:type:ShaderForge.SFN_Add,id:4111,x:30252,y:33706,varname:node_4111,prsc:2|A-5191-G,B-680-OUT;n:type:ShaderForge.SFN_Multiply,id:5793,x:29996,y:33920,varname:node_5793,prsc:2|A-9873-X,B-959-TSL;n:type:ShaderForge.SFN_Multiply,id:680,x:29996,y:34079,varname:node_680,prsc:2|A-9873-Y,B-959-TSL;n:type:ShaderForge.SFN_Append,id:3117,x:30447,y:33591,varname:node_3117,prsc:2|A-1996-OUT,B-4111-OUT;n:type:ShaderForge.SFN_Multiply,id:2395,x:29718,y:33477,varname:node_2395,prsc:2|A-7141-OUT,B-90-OUT;n:type:ShaderForge.SFN_Multiply,id:8179,x:29718,y:33625,varname:node_8179,prsc:2|A-7141-OUT,B-4277-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4277,x:29434,y:33810,ptovrint:False,ptlb:UV  Tiling 2,ptin:_UVTiling2,varname:node_4277,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Get,id:4252,x:29106,y:34638,varname:node_4252,prsc:2|IN-7239-OUT;n:type:ShaderForge.SFN_Get,id:8445,x:29052,y:34991,varname:node_8445,prsc:2|IN-9320-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8578,x:29432,y:33128,ptovrint:False,ptlb:node_8578,ptin:_node_8578,varname:node_8578,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:5682,x:29432,y:33215,ptovrint:False,ptlb:node_5682,ptin:_node_5682,varname:node_5682,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:90,x:29434,y:33727,ptovrint:False,ptlb:UV Tiling,ptin:_UVTiling,varname:node_90,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:3710,x:29770,y:34537,varname:node_3710,prsc:2|A-9205-OUT,B-2658-OUT;n:type:ShaderForge.SFN_Slider,id:2658,x:29258,y:35109,ptovrint:False,ptlb:Normal map 1 strength,ptin:_Normalmap1strength,varname:node_2658,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:5;n:type:ShaderForge.SFN_ComponentMask,id:9205,x:29541,y:34500,varname:node_9205,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-4237-RGB;n:type:ShaderForge.SFN_Vector1,id:7824,x:29764,y:34741,varname:node_7824,prsc:2,v1:1;n:type:ShaderForge.SFN_Append,id:1539,x:29929,y:34651,varname:node_1539,prsc:2|A-3710-OUT,B-7824-OUT;n:type:ShaderForge.SFN_ComponentMask,id:8495,x:29541,y:34680,varname:node_8495,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-7804-RGB;n:type:ShaderForge.SFN_Multiply,id:7647,x:29764,y:34898,varname:node_7647,prsc:2|A-8495-OUT,B-2678-OUT;n:type:ShaderForge.SFN_Slider,id:2678,x:29240,y:35008,ptovrint:False,ptlb:Normal map 2 strength,ptin:_Normalmap2strength,varname:node_2678,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:5;n:type:ShaderForge.SFN_Append,id:5209,x:29929,y:34881,varname:node_5209,prsc:2|A-7647-OUT,B-7824-OUT;n:type:ShaderForge.SFN_Set,id:5899,x:30461,y:34970,varname:_normalMapping,prsc:2|IN-8576-OUT;n:type:ShaderForge.SFN_Get,id:2297,x:32704,y:32848,varname:node_2297,prsc:2|IN-5899-OUT;n:type:ShaderForge.SFN_TexCoord,id:3962,x:31797,y:33496,varname:node_3962,prsc:2,uv:1,uaff:False;n:type:ShaderForge.SFN_Time,id:4263,x:31585,y:33582,varname:node_4263,prsc:2;n:type:ShaderForge.SFN_Multiply,id:8817,x:31810,y:33686,varname:node_8817,prsc:2|A-4263-T,B-6611-OUT;n:type:ShaderForge.SFN_Slider,id:6611,x:31428,y:33765,ptovrint:False,ptlb:Wave Speed,ptin:_WaveSpeed,varname:node_6611,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:0.1774391,max:5;n:type:ShaderForge.SFN_Panner,id:4714,x:32045,y:33537,varname:node_4714,prsc:2,spu:1,spv:1|UVIN-3962-UVOUT,DIST-8817-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:3275,x:32003,y:33857,ptovrint:False,ptlb:Wave Texture,ptin:_WaveTexture,varname:node_3275,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3010,x:32340,y:33622,varname:node_3010,prsc:2,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-4714-UVOUT,TEX-3275-TEX;n:type:ShaderForge.SFN_NormalVector,id:4185,x:32269,y:33815,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:5516,x:32778,y:33613,varname:node_5516,prsc:2|A-3010-RGB,B-2550-OUT;n:type:ShaderForge.SFN_Multiply,id:4896,x:32725,y:32955,varname:node_4896,prsc:2|A-9659-RGB,B-3010-RGB;n:type:ShaderForge.SFN_Color,id:9659,x:32330,y:33055,ptovrint:False,ptlb:Wave Color,ptin:_WaveColor,varname:node_9659,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Slider,id:6237,x:32063,y:33241,ptovrint:False,ptlb:Wave Brightness,ptin:_WaveBrightness,varname:node_6237,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:5;n:type:ShaderForge.SFN_Multiply,id:6596,x:32725,y:33103,varname:node_6596,prsc:2|A-6237-OUT,B-3010-RGB;n:type:ShaderForge.SFN_ValueProperty,id:5628,x:33366,y:32882,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_5628,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:840,x:33516,y:33142,varname:node_840,prsc:2|A-3541-OUT,B-7731-OUT;n:type:ShaderForge.SFN_ComponentMask,id:3541,x:33308,y:33262,varname:node_3541,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-5547-RGB;n:type:ShaderForge.SFN_Tex2d,id:5547,x:33133,y:33255,ptovrint:False,ptlb:Texture noise,ptin:_Texturenoise,varname:node_5547,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-9871-UVOUT;n:type:ShaderForge.SFN_Slider,id:7731,x:33199,y:33504,ptovrint:False,ptlb:Refraction,ptin:_Refraction,varname:node_7731,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-5,cur:2,max:20;n:type:ShaderForge.SFN_Panner,id:9871,x:32876,y:33268,varname:node_9871,prsc:2,spu:0.1,spv:0.1|UVIN-4929-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:4929,x:32712,y:33297,varname:node_4929,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Multiply,id:2550,x:32625,y:33851,varname:node_2550,prsc:2|A-4185-OUT,B-297-OUT;n:type:ShaderForge.SFN_ValueProperty,id:297,x:32343,y:34106,ptovrint:False,ptlb:Waves Amplitude,ptin:_WavesAmplitude,varname:node_297,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:30;proporder:5547-7731-3029-8492-9570-1813-6590-2658-2678-8212-8138-9873-1890-90-4277-6611-3275-9659-6237-5628-297;pass:END;sub:END;*/

Shader "Shader Forge/Water" {
    Properties {
        _Texturenoise ("Texture noise", 2D) = "white" {}
        _Refraction ("Refraction", Range(-5, 20)) = 2
        _ColourDeep ("Colour (Deep)", Color) = (0,0.2862745,0.2156863,1)
        _ColourShallow ("Colour (Shallow)", Color) = (0.4156863,0.7882354,1,1)
        _ColorFrensnel ("Color Frensnel", Float ) = 1.336
        _Glossiness ("Glossiness", Range(0, 5)) = 0.95
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Normalmap1strength ("Normal map 1 strength", Range(0, 5)) = 0
        _Normalmap2strength ("Normal map 2 strength", Range(0, 5)) = 0
        _NormalBlendStrength ("Normal Blend Strength", Range(0, 5)) = 0
        _UV1Animator ("UV 1 Animator", Vector) = (0,0,0,0)
        _UV2Animator ("UV 2 Animator", Vector) = (0,0,0,0)
        _UVScale ("UV Scale", Float ) = 1
        _UVTiling ("UV Tiling", Float ) = 0
        _UVTiling2 ("UV  Tiling 2", Float ) = 0
        _WaveSpeed ("Wave Speed", Range(-5, 5)) = 0.1774391
        _WaveTexture ("Wave Texture", 2D) = "white" {}
        _WaveColor ("Wave Color", Color) = (1,1,1,1)
        _WaveBrightness ("Wave Brightness", Range(0, 5)) = 1
        _Opacity ("Opacity", Float ) = 0.5
        _WavesAmplitude ("Waves Amplitude", Float ) = 30
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform float _Glossiness;
            uniform float4 _ColourDeep;
            uniform float4 _ColourShallow;
            uniform float _ColorFrensnel;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _NormalBlendStrength;
            uniform float _UVScale;
            uniform float4 _UV1Animator;
            uniform float4 _UV2Animator;
            uniform float _UVTiling2;
            uniform float _UVTiling;
            uniform float _Normalmap1strength;
            uniform float _Normalmap2strength;
            uniform float _WaveSpeed;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            uniform float4 _WaveColor;
            uniform float _WaveBrightness;
            uniform float _Opacity;
            uniform sampler2D _Texturenoise; uniform float4 _Texturenoise_ST;
            uniform float _Refraction;
            uniform float _WavesAmplitude;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 screenPos : TEXCOORD7;
                UNITY_FOG_COORDS(8)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD9;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #elif UNITY_SHOULD_SAMPLE_SH
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_4263 = _Time + _TimeEditor;
                float2 node_4714 = (o.uv1+(node_4263.g*_WaveSpeed)*float2(1,1));
                float4 node_3010 = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_4714, _WaveTexture),0.0,0));
                v.vertex.xyz += (node_3010.rgb*(v.normal*_WavesAmplitude));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_959 = _Time + _TimeEditor;
                float2 _worldUV = (float2(i.posWorld.r,i.posWorld.b)/_UVScale);
                float2 node_7141 = _worldUV;
                float2 node_9625 = (node_7141*_UVTiling).rg;
                float2 _UV1 = float2(((_UV1Animator.r*node_959.r)+node_9625.r),((_UV1Animator.g*node_959.r)+node_9625.g));
                float2 node_4252 = _UV1;
                float3 node_4237 = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_4252, _NormalMap)));
                float node_7824 = 1.0;
                float2 node_5191 = (node_7141*_UVTiling2).rg;
                float2 _UV2 = float2((node_5191.r+(_UV2Animator.r*node_959.r)),(node_5191.g+(_UV2Animator.g*node_959.r)));
                float2 node_8445 = _UV2;
                float3 node_7804 = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_8445, _NormalMap)));
                float3 _normalMapping = lerp(float3((node_4237.rgb.rg*_Normalmap1strength),node_7824),float3((node_7804.rgb.rg*_Normalmap2strength),node_7824),_NormalBlendStrength);
                float3 normalLocal = _normalMapping;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float4 node_9224 = _Time + _TimeEditor;
                float2 node_9871 = (i.uv0+node_9224.g*float2(0.1,0.1));
                float4 _Texturenoise_var = tex2D(_Texturenoise,TRANSFORM_TEX(node_9871, _Texturenoise));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (_Texturenoise_var.rgb.rg*_Refraction);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Glossiness;
                float perceptualRoughness = 1.0 - _Glossiness;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = 0.0;
                float specularMonochrome;
                float3 diffuseColor = lerp(_ColourDeep.rgb,_ColourShallow.rgb,pow(1.0-max(0,dot(normalDirection, viewDirection)),clamp(_ColorFrensnel,0,4))); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = max(0.0, NdotL );
                float4 node_4263 = _Time + _TimeEditor;
                float2 node_4714 = (i.uv1+(node_4263.g*_WaveSpeed)*float2(1,1));
                float4 node_3010 = tex2D(_WaveTexture,TRANSFORM_TEX(node_4714, _WaveTexture));
                float3 backLight = max(0.0, -NdotL ) * (_WaveColor.rgb*node_3010.rgb);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 NdotLWrap = max(0,NdotL);
                float nlPow5 = Pow5(1-NdotLWrap);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((forwardLight+backLight) + ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL)) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = (_WaveBrightness*node_3010.rgb);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,_Opacity),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform float4 _TimeEditor;
            uniform float _Glossiness;
            uniform float4 _ColourDeep;
            uniform float4 _ColourShallow;
            uniform float _ColorFrensnel;
            uniform sampler2D _NormalMap; uniform float4 _NormalMap_ST;
            uniform float _NormalBlendStrength;
            uniform float _UVScale;
            uniform float4 _UV1Animator;
            uniform float4 _UV2Animator;
            uniform float _UVTiling2;
            uniform float _UVTiling;
            uniform float _Normalmap1strength;
            uniform float _Normalmap2strength;
            uniform float _WaveSpeed;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            uniform float4 _WaveColor;
            uniform float _WaveBrightness;
            uniform float _Opacity;
            uniform sampler2D _Texturenoise; uniform float4 _Texturenoise_ST;
            uniform float _Refraction;
            uniform float _WavesAmplitude;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                float4 screenPos : TEXCOORD7;
                LIGHTING_COORDS(8,9)
                UNITY_FOG_COORDS(10)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float4 node_4263 = _Time + _TimeEditor;
                float2 node_4714 = (o.uv1+(node_4263.g*_WaveSpeed)*float2(1,1));
                float4 node_3010 = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_4714, _WaveTexture),0.0,0));
                v.vertex.xyz += (node_3010.rgb*(v.normal*_WavesAmplitude));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 node_959 = _Time + _TimeEditor;
                float2 _worldUV = (float2(i.posWorld.r,i.posWorld.b)/_UVScale);
                float2 node_7141 = _worldUV;
                float2 node_9625 = (node_7141*_UVTiling).rg;
                float2 _UV1 = float2(((_UV1Animator.r*node_959.r)+node_9625.r),((_UV1Animator.g*node_959.r)+node_9625.g));
                float2 node_4252 = _UV1;
                float3 node_4237 = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_4252, _NormalMap)));
                float node_7824 = 1.0;
                float2 node_5191 = (node_7141*_UVTiling2).rg;
                float2 _UV2 = float2((node_5191.r+(_UV2Animator.r*node_959.r)),(node_5191.g+(_UV2Animator.g*node_959.r)));
                float2 node_8445 = _UV2;
                float3 node_7804 = UnpackNormal(tex2D(_NormalMap,TRANSFORM_TEX(node_8445, _NormalMap)));
                float3 _normalMapping = lerp(float3((node_4237.rgb.rg*_Normalmap1strength),node_7824),float3((node_7804.rgb.rg*_Normalmap2strength),node_7824),_NormalBlendStrength);
                float3 normalLocal = _normalMapping;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 node_605 = _Time + _TimeEditor;
                float2 node_9871 = (i.uv0+node_605.g*float2(0.1,0.1));
                float4 _Texturenoise_var = tex2D(_Texturenoise,TRANSFORM_TEX(node_9871, _Texturenoise));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (_Texturenoise_var.rgb.rg*_Refraction);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float gloss = _Glossiness;
                float perceptualRoughness = 1.0 - _Glossiness;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float3 specularColor = 0.0;
                float specularMonochrome;
                float3 diffuseColor = lerp(_ColourDeep.rgb,_ColourShallow.rgb,pow(1.0-max(0,dot(normalDirection, viewDirection)),clamp(_ColorFrensnel,0,4))); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = dot( normalDirection, lightDirection );
                float3 forwardLight = max(0.0, NdotL );
                float4 node_4263 = _Time + _TimeEditor;
                float2 node_4714 = (i.uv1+(node_4263.g*_WaveSpeed)*float2(1,1));
                float4 node_3010 = tex2D(_WaveTexture,TRANSFORM_TEX(node_4714, _WaveTexture));
                float3 backLight = max(0.0, -NdotL ) * (_WaveColor.rgb*node_3010.rgb);
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 NdotLWrap = max(0,NdotL);
                float nlPow5 = Pow5(1-NdotLWrap);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((forwardLight+backLight) + ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL)) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * _Opacity,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _WaveSpeed;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            uniform float _WavesAmplitude;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4263 = _Time + _TimeEditor;
                float2 node_4714 = (o.uv1+(node_4263.g*_WaveSpeed)*float2(1,1));
                float4 node_3010 = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_4714, _WaveTexture),0.0,0));
                v.vertex.xyz += (node_3010.rgb*(v.normal*_WavesAmplitude));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _Glossiness;
            uniform float4 _ColourDeep;
            uniform float4 _ColourShallow;
            uniform float _ColorFrensnel;
            uniform float _WaveSpeed;
            uniform sampler2D _WaveTexture; uniform float4 _WaveTexture_ST;
            uniform float _WaveBrightness;
            uniform float _WavesAmplitude;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv1 : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_4263 = _Time + _TimeEditor;
                float2 node_4714 = (o.uv1+(node_4263.g*_WaveSpeed)*float2(1,1));
                float4 node_3010 = tex2Dlod(_WaveTexture,float4(TRANSFORM_TEX(node_4714, _WaveTexture),0.0,0));
                v.vertex.xyz += (node_3010.rgb*(v.normal*_WavesAmplitude));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float4 node_4263 = _Time + _TimeEditor;
                float2 node_4714 = (i.uv1+(node_4263.g*_WaveSpeed)*float2(1,1));
                float4 node_3010 = tex2D(_WaveTexture,TRANSFORM_TEX(node_4714, _WaveTexture));
                o.Emission = (_WaveBrightness*node_3010.rgb);
                
                float3 diffColor = lerp(_ColourDeep.rgb,_ColourShallow.rgb,pow(1.0-max(0,dot(normalDirection, viewDirection)),clamp(_ColorFrensnel,0,4)));
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, 0.0, specColor, specularMonochrome );
                float roughness = 1.0 - _Glossiness;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
