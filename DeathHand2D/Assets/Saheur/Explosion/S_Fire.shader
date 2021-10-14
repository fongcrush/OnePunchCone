// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Fire"
{
	Properties
	{
		_Tex1("Tex1", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" {}
		_Base_Scale("Base_Scale", Float) = 0.14
		_Inner_Scale("Inner_Scale", Float) = -0.1
		_Inner_Color("Inner_Color", Color) = (1,0.3985968,0.2971698,0)
		_Base_Color("Base_Color", Color) = (1,0.2538727,0,0)
		_Base_Inten("Base_Inten", Float) = 10
		_Inner_Inten("Inner_Inten", Float) = 6
		_Dissolve("Dissolve", 2D) = "white" {}
		_Dissolve_Value("Dissolve_Value", Range( 0 , 1)) = 0.5
		_Line_Scale("Line_Scale", Float) = 0.05
		_Line_Color("Line_Color", Color) = (1,0.2644757,0,0)
		_Line_Inten("Line_Inten", Float) = 5
		[Toggle]_ToggleSwitch1("Toggle Switch1", Float) = 1
		_Tex1_UV("Tex1_UV", Vector) = (1,1,0,0)
		_Tex1_Pan("Tex1_Pan", Vector) = (0,0,0,0)
		_Dis_Tex1("Dis_Tex1", 2D) = "white" {}
		_Dis_Str("Dis_Str", Range( 0 , 1)) = 0.1
		_Dis_Pan("Dis_Pan", Vector) = (0,1,0,0)
		_Mask_Power("Mask_Power", Float) = 0
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
			float4 vertexColor : COLOR;
		};

		uniform float4 _Inner_Color;
		uniform sampler2D _Tex1;
		uniform float2 _Tex1_Pan;
		uniform float2 _Tex1_UV;
		uniform sampler2D _Dis_Tex1;
		uniform float2 _Dis_Pan;
		uniform float _Dis_Str;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform float _Mask_Power;
		uniform float _Base_Scale;
		uniform float _Inner_Scale;
		uniform float _Inner_Inten;
		uniform float4 _Base_Color;
		uniform float _Base_Inten;
		uniform float4 _Line_Color;
		uniform sampler2D _Dissolve;
		uniform float4 _Dissolve_ST;
		uniform float _ToggleSwitch1;
		uniform float _Dissolve_Value;
		uniform float _Line_Scale;
		uniform float _Line_Inten;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord53 = i.uv_texcoord * _Tex1_UV;
			float2 panner55 = ( 1.0 * _Time.y * _Tex1_Pan + uv_TexCoord53);
			float2 panner62 = ( 1.0 * _Time.y * _Dis_Pan + i.uv_texcoord);
			float4 tex2DNode1 = tex2D( _Tex1, ( panner55 + ( tex2D( _Dis_Tex1, panner62 ).r * _Dis_Str ) ) );
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float temp_output_4_0 = ( tex2DNode1.r * pow( tex2D( _Mask, uv_Mask ).r , _Mask_Power ) );
			float4 temp_output_18_0 = ( ( _Base_Color * ( 1.0 - step( temp_output_4_0 , _Base_Scale ) ) * tex2DNode1.r ) * _Base_Inten );
			float2 uv_Dissolve = i.uv_texcoord * _Dissolve_ST.xy + _Dissolve_ST.zw;
			float4 tex2DNode25 = tex2D( _Dissolve, uv_Dissolve );
			float ifLocalVar24 = 0;
			if( tex2DNode25.r >= (( _ToggleSwitch1 )?( i.uv2_tex4coord2.x ):( _Dissolve_Value )) )
				ifLocalVar24 = 1.0;
			else
				ifLocalVar24 = 0.0;
			float ifLocalVar31 = 0;
			if( tex2DNode25.r >= ( (( _ToggleSwitch1 )?( i.uv2_tex4coord2.x ):( _Dissolve_Value )) + _Line_Scale ) )
				ifLocalVar31 = 1.0;
			else
				ifLocalVar31 = 0.0;
			o.Emission = ( ( ( ( _Inner_Color * ( 1.0 - step( temp_output_4_0 , ( _Base_Scale - _Inner_Scale ) ) ) * tex2DNode1.r ) * _Inner_Inten ) + temp_output_18_0 ) + ( _Line_Color * ( ifLocalVar24 - ifLocalVar31 ) * _Line_Inten ) ).rgb;
			o.Alpha = ( saturate( ( temp_output_18_0 * ifLocalVar24 ) ) * i.vertexColor.a ).r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.uv2_tex4coord2;
				o.customPack2.xyzw = v.texcoord1;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.uv2_tex4coord2 = IN.customPack2.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
-99;580;1920;498;1857.73;188.9267;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-2381.338,23.15292;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;64;-2273.338,202.1529;Inherit;False;Property;_Dis_Pan;Dis_Pan;19;0;Create;True;0;0;0;False;0;False;0,1;0.2,-1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;62;-2086.338,103.1529;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;56;-2095.138,-101.0471;Inherit;False;Property;_Tex1_UV;Tex1_UV;15;0;Create;True;0;0;0;False;0;False;1,1;1,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;60;-1852.338,109.1529;Inherit;True;Property;_Dis_Tex1;Dis_Tex1;17;0;Create;True;0;0;0;False;0;False;-1;64774c75677d29d4ea1fe1815b8d63cc;64774c75677d29d4ea1fe1815b8d63cc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;61;-1833.338,297.1529;Inherit;False;Property;_Dis_Str;Dis_Str;18;0;Create;True;0;0;0;False;0;False;0.1;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;57;-1767.338,-31.84708;Inherit;False;Property;_Tex1_Pan;Tex1_Pan;16;0;Create;True;0;0;0;False;0;False;0,0;-0.1,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;53;-1880.748,-195.6594;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1508.338,234.1529;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;55;-1553.338,-86.84708;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1306.748,-22.75941;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1207.73,410.0733;Inherit;False;Property;_Mask_Power;Mask_Power;20;0;Create;True;0;0;0;False;0;False;0;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1324,211;Inherit;True;Property;_Mask;Mask;2;0;Create;True;0;0;0;False;0;False;-1;0616238a82f7e524095a0ec40a481714;f510b1a00e97521479d09c7e184bc2f9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1087,-28;Inherit;True;Property;_Tex1;Tex1;1;0;Create;True;0;0;0;False;0;False;-1;12201dd52bf81834694a4be2226da444;67752a7ad85a5604a802f18061513cca;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;67;-916.7305,203.0733;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-704,155;Inherit;False;Property;_Base_Scale;Base_Scale;3;0;Create;True;0;0;0;False;0;False;0.14;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-756,-124;Inherit;False;Property;_Inner_Scale;Inner_Scale;4;0;Create;True;0;0;0;False;0;False;-0.1;-0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-710.4594,-24.05213;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;-513,-191;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;27;-1059.05,566.2958;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-1092.05,475.2959;Inherit;False;Property;_Dissolve_Value;Dissolve_Value;10;0;Create;True;0;0;0;False;0;False;0.5;0.484;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;3;-362,52;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-573.3537,966.5632;Inherit;False;Property;_Line_Scale;Line_Scale;11;0;Create;True;0;0;0;False;0;False;0.05;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;6;-354,-257;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;50;-460.1754,443.1179;Inherit;False;Property;_ToggleSwitch1;Toggle Switch1;14;0;Create;True;0;0;0;False;0;False;1;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;13;-245.4397,-86.22827;Inherit;False;Property;_Base_Color;Base_Color;6;0;Create;True;0;0;0;False;0;False;1,0.2538727,0,0;1,0.2538727,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;14;-121.4397,94.77173;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-385.017,623.1729;Inherit;False;Constant;_Float1;Float 1;12;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;39.37233,98.1833;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;12;-247.4397,-401.2283;Inherit;False;Property;_Inner_Color;Inner_Color;5;0;Create;True;0;0;0;False;0;False;1,0.3985968,0.2971698,0;1,0.3985968,0.2971698,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;-946.3324,737.5447;Inherit;True;Property;_Dissolve;Dissolve;9;0;Create;True;0;0;0;False;0;False;-1;64774c75677d29d4ea1fe1815b8d63cc;64774c75677d29d4ea1fe1815b8d63cc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-360.1642,971.3107;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;262.9118,166.2592;Inherit;False;Property;_Base_Inten;Base_Inten;7;0;Create;True;0;0;0;False;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-386.017,694.1729;Inherit;False;Constant;_Float0;Float 0;12;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;15;-104.4397,-199.2283;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;31;-189.588,936.6642;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;24;-173.9207,466.0217;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;261.9118,-114.7408;Inherit;False;Property;_Inner_Inten;Inner_Inten;8;0;Create;True;0;0;0;False;0;False;6;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;95.5603,-227.2283;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;435.9118,81.25922;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;946.0927,185.9654;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;235.3085,916.3279;Inherit;False;Property;_Line_Inten;Line_Inten;13;0;Create;True;0;0;0;False;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;167.2709,697.6829;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;44;202.3085,523.3279;Inherit;False;Property;_Line_Color;Line_Color;12;0;Create;True;0;0;0;False;0;False;1,0.2644757,0,0;0.754717,0.2122449,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;432.9118,-211.7408;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;439.3086,706.3279;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;65;1218.825,279.7723;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;723.9118,-99.74078;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;51;1320.604,154.4138;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;1037.613,-68.76772;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;1500.53,156.6979;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1692.061,-72.33142;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;S_Fire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;62;0;63;0
WireConnection;62;2;64;0
WireConnection;60;1;62;0
WireConnection;53;0;56;0
WireConnection;58;0;60;1
WireConnection;58;1;61;0
WireConnection;55;0;53;0
WireConnection;55;2;57;0
WireConnection;52;0;55;0
WireConnection;52;1;58;0
WireConnection;1;1;52;0
WireConnection;67;0;2;1
WireConnection;67;1;68;0
WireConnection;4;0;1;1
WireConnection;4;1;67;0
WireConnection;7;0;5;0
WireConnection;7;1;8;0
WireConnection;3;0;4;0
WireConnection;3;1;5;0
WireConnection;6;0;4;0
WireConnection;6;1;7;0
WireConnection;50;0;26;0
WireConnection;50;1;27;1
WireConnection;14;0;3;0
WireConnection;9;0;13;0
WireConnection;9;1;14;0
WireConnection;9;2;1;1
WireConnection;38;0;50;0
WireConnection;38;1;39;0
WireConnection;15;0;6;0
WireConnection;31;0;25;1
WireConnection;31;1;38;0
WireConnection;31;2;30;0
WireConnection;31;3;30;0
WireConnection;31;4;29;0
WireConnection;24;0;25;1
WireConnection;24;1;50;0
WireConnection;24;2;30;0
WireConnection;24;3;30;0
WireConnection;24;4;29;0
WireConnection;10;0;12;0
WireConnection;10;1;15;0
WireConnection;10;2;1;1
WireConnection;18;0;9;0
WireConnection;18;1;19;0
WireConnection;47;0;18;0
WireConnection;47;1;24;0
WireConnection;41;0;24;0
WireConnection;41;1;31;0
WireConnection;17;0;10;0
WireConnection;17;1;20;0
WireConnection;42;0;44;0
WireConnection;42;1;41;0
WireConnection;42;2;45;0
WireConnection;22;0;17;0
WireConnection;22;1;18;0
WireConnection;51;0;47;0
WireConnection;46;0;22;0
WireConnection;46;1;42;0
WireConnection;66;0;51;0
WireConnection;66;1;65;4
WireConnection;0;2;46;0
WireConnection;0;9;66;0
ASEEND*/
//CHKSM=D9295A3C67BF13FB51A25BBD9BD9F382AB38F431