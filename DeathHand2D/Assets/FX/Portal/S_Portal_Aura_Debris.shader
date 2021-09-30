// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Portal_Aura_Debris"
{
	Properties
	{
		_Tex01("Tex01", 2D) = "white" {}
		_Dissolve2("Dissolve2", 2D) = "white" {}
		_Dissolve1("Dissolve1", 2D) = "white" {}
		_Dissovle_Value("Dissovle_Value", Range( 0 , 1)) = 0.5405252
		[Toggle]_Dissolve("Dissolve", Float) = 0
		_Line_Scale("Line_Scale", Float) = 0.08
		_Line_Inten("Line_Inten", Float) = 3
		_Line_Color("Line_Color", Color) = (1,1,1,0)
		_Tex01_Pan("Tex01_Pan", Vector) = (0.65,0,0,0)
		_Tex01_UV("Tex01_UV", Vector) = (1,1,0,0)
		_Dis_Str("Dis_Str", Range( 0 , 1)) = 0.2442828
		_Dis_Tex1("Dis_Tex1", 2D) = "white" {}
		_Dis_Pan("Dis_Pan", Vector) = (0,0.8,0,0)
		_Power("Power", Float) = 2
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Color_Inten("Color_Inten", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord2( "", 2D ) = "white" {}
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
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv2_tex4coord2;
		};

		uniform sampler2D _Tex01;
		uniform float2 _Tex01_Pan;
		uniform float2 _Tex01_UV;
		uniform sampler2D _Dis_Tex1;
		uniform float2 _Dis_Pan;
		uniform float _Dis_Str;
		uniform sampler2D _TextureSample0;
		uniform float _Power;
		uniform float _Color_Inten;
		uniform sampler2D _Dissolve1;
		uniform float4 _Dissolve1_ST;
		uniform sampler2D _Dissolve2;
		uniform float4 _Dissolve2_ST;
		uniform float _Dissolve;
		uniform float _Dissovle_Value;
		uniform float4 _Line_Color;
		uniform float _Line_Scale;
		uniform float _Line_Inten;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord36 = i.uv_texcoord * _Tex01_UV;
			float2 panner38 = ( 1.0 * _Time.y * _Tex01_Pan + uv_TexCoord36);
			float2 panner44 = ( 1.0 * _Time.y * _Dis_Pan + i.uv_texcoord);
			float4 tex2DNode1 = tex2D( _Tex01, ( panner38 + ( tex2D( _Dis_Tex1, panner44 ).r * _Dis_Str ) ) );
			float2 CenteredUV15_g2 = ( i.uv_texcoord - float2( 0.5,0.5 ) );
			float2 break17_g2 = CenteredUV15_g2;
			float2 appendResult23_g2 = (float2(( length( CenteredUV15_g2 ) * 1.0 * 2.0 ) , ( atan2( break17_g2.x , break17_g2.y ) * ( 1.0 / 6.28318548202515 ) * 1.0 )));
			float temp_output_58_0 = saturate( ( pow( ( tex2DNode1.r * ( tex2DNode1.r * tex2D( _TextureSample0, appendResult23_g2 ).r ) ) , _Power ) * 2.0 ) );
			o.Emission = ( i.vertexColor * temp_output_58_0 * _Color_Inten ).rgb;
			float2 uv_Dissolve1 = i.uv_texcoord * _Dissolve1_ST.xy + _Dissolve1_ST.zw;
			float4 tex2DNode7 = tex2D( _Dissolve1, uv_Dissolve1 );
			float2 uv_Dissolve2 = i.uv_texcoord * _Dissolve2_ST.xy + _Dissolve2_ST.zw;
			float lerpResult6 = lerp( tex2DNode7.r , tex2D( _Dissolve2, uv_Dissolve2 ).r , tex2DNode7.r);
			float Dissolve_Tex13 = lerpResult6;
			float ifLocalVar3 = 0;
			if( Dissolve_Tex13 <= (( _Dissolve )?( i.uv2_tex4coord2.x ):( _Dissovle_Value )) )
				ifLocalVar3 = 0.0;
			else
				ifLocalVar3 = 1.0;
			float ifLocalVar17 = 0;
			if( Dissolve_Tex13 <= ( (( _Dissolve )?( i.uv2_tex4coord2.x ):( _Dissovle_Value )) + _Line_Scale ) )
				ifLocalVar17 = 0.0;
			else
				ifLocalVar17 = 1.0;
			o.Alpha = ( i.vertexColor.a * ( temp_output_58_0 * ifLocalVar3 * ( temp_output_58_0 + ( _Line_Color * ( ifLocalVar3 - ifLocalVar17 ) * _Line_Inten ) ) ) ).r;
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
-89;549;1906;606;-875.3223;232.3904;1;True;False
Node;AmplifyShaderEditor.Vector2Node;46;-819.9061,103.4773;Inherit;False;Property;_Dis_Pan;Dis_Pan;13;0;Create;True;0;0;0;False;0;False;0,0.8;0,0.8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-991.9579,-58.69127;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;40;-572.6389,-176.2386;Inherit;False;Property;_Tex01_UV;Tex01_UV;10;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;44;-671.9578,-0.6912823;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;39;-266.6389,-136.2386;Inherit;False;Property;_Tex01_Pan;Tex01_Pan;9;0;Create;True;0;0;0;False;0;False;0.65,0;0.65,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;43;-377.9575,-0.6912823;Inherit;True;Property;_Dis_Tex1;Dis_Tex1;12;0;Create;True;0;0;0;False;0;False;-1;64774c75677d29d4ea1fe1815b8d63cc;64774c75677d29d4ea1fe1815b8d63cc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;42;-371.9575,198.3087;Inherit;False;Property;_Dis_Str;Dis_Str;11;0;Create;True;0;0;0;False;0;False;0.2442828;0.2442828;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-391.6388,-267.2385;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;29;238.48,434.1934;Inherit;False;2240.448;1158.049;Comment;20;12;17;18;19;14;11;21;8;23;3;10;9;24;27;26;28;13;7;6;4;Dissolve;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-57.95758,87.30872;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;38;-84.63885,-197.2385;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;155.2526,-112.6787;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;567.4716,685.1933;Inherit;True;Property;_Dissolve2;Dissolve2;2;0;Create;True;0;0;0;False;0;False;-1;54197560a90d47a4785a58562b3df73f;54197560a90d47a4785a58562b3df73f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;578.4716,484.1934;Inherit;True;Property;_Dissolve1;Dissolve1;3;0;Create;True;0;0;0;False;0;False;-1;64774c75677d29d4ea1fe1815b8d63cc;64774c75677d29d4ea1fe1815b8d63cc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;51;-81.23131,255.8882;Inherit;True;Polar Coordinates;-1;;2;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;408.9485,1068.747;Inherit;False;1;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;52;358.0141,162.6848;Inherit;True;Property;_TextureSample0;Texture Sample 0;15;0;Create;True;0;0;0;False;0;False;-1;600d17dce8ce61145a579bb295101846;600d17dce8ce61145a579bb295101846;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;288.4801,962.6793;Inherit;False;Property;_Dissovle_Value;Dissovle_Value;4;0;Create;True;0;0;0;False;0;False;0.5405252;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;343.6355,-140.5402;Inherit;True;Property;_Tex01;Tex01;1;0;Create;True;0;0;0;False;0;False;-1;54197560a90d47a4785a58562b3df73f;54197560a90d47a4785a58562b3df73f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;6;963.473,639.1932;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;9;792.8502,898.8414;Inherit;False;Property;_Dissolve;Dissolve;5;0;Create;True;0;0;0;False;0;False;0;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;804.4873,-2.413177;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;1295.985,704.7999;Inherit;False;Dissolve_Tex;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;881.9763,1314.692;Inherit;False;Property;_Line_Scale;Line_Scale;6;0;Create;True;0;0;0;False;0;False;0.08;0.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;1092.806,-134.7302;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;1337.201,1476.242;Inherit;False;Constant;_Float3;Float 3;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;1337.201,1406.533;Inherit;False;Constant;_Float2;Float 2;6;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;21;1108.99,1279.651;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;14;1198.541,1125.795;Inherit;False;13;Dissolve_Tex;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;1245.447,960.8615;Inherit;False;Constant;_Float0;Float 0;6;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;1245.447,1030.571;Inherit;False;Constant;_Float1;Float 1;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;1290.806,-57.73018;Inherit;False;Property;_Power;Power;14;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;3;1489.837,849.404;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;49;1452.806,-137.7302;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;1706.35,-33.20973;Inherit;False;Constant;_Opacity;Opacity;15;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;17;1501.345,1280.605;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;24;1913.485,991.8979;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;27;1917.417,815.6098;Inherit;False;Property;_Line_Color;Line_Color;8;0;Create;True;0;0;0;False;0;False;1,1,1,0;0.352994,0,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;1898.348,-136.2089;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;1971.17,1228.011;Inherit;False;Property;_Line_Inten;Line_Inten;7;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;2242.927,1043.062;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;58;2144.906,-120.6022;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;61;2612.116,227.5675;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;2987.257,-4.183017;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;59;2136.906,-23.60223;Inherit;False;Property;_Color_Inten;Color_Inten;16;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;56;2280.917,-353.5109;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;3354.297,-36.06003;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;2517.505,-148.7023;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3613.04,-228.322;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;S_Portal_Aura_Debris;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;44;0;45;0
WireConnection;44;2;46;0
WireConnection;43;1;44;0
WireConnection;36;0;40;0
WireConnection;41;0;43;1
WireConnection;41;1;42;0
WireConnection;38;0;36;0
WireConnection;38;2;39;0
WireConnection;35;0;38;0
WireConnection;35;1;41;0
WireConnection;52;1;51;0
WireConnection;1;1;35;0
WireConnection;6;0;7;1
WireConnection;6;1;4;1
WireConnection;6;2;7;1
WireConnection;9;0;8;0
WireConnection;9;1;10;1
WireConnection;64;0;1;1
WireConnection;64;1;52;1
WireConnection;13;0;6;0
WireConnection;48;0;1;1
WireConnection;48;1;64;0
WireConnection;21;0;9;0
WireConnection;21;1;23;0
WireConnection;3;0;13;0
WireConnection;3;1;9;0
WireConnection;3;2;11;0
WireConnection;3;3;12;0
WireConnection;3;4;12;0
WireConnection;49;0;48;0
WireConnection;49;1;50;0
WireConnection;17;0;14;0
WireConnection;17;1;21;0
WireConnection;17;2;18;0
WireConnection;17;3;19;0
WireConnection;17;4;19;0
WireConnection;24;0;3;0
WireConnection;24;1;17;0
WireConnection;53;0;49;0
WireConnection;53;1;54;0
WireConnection;26;0;27;0
WireConnection;26;1;24;0
WireConnection;26;2;28;0
WireConnection;58;0;53;0
WireConnection;61;0;58;0
WireConnection;61;1;26;0
WireConnection;62;0;58;0
WireConnection;62;1;3;0
WireConnection;62;2;61;0
WireConnection;63;0;56;4
WireConnection;63;1;62;0
WireConnection;57;0;56;0
WireConnection;57;1;58;0
WireConnection;57;2;59;0
WireConnection;0;2;57;0
WireConnection;0;9;63;0
ASEEND*/
//CHKSM=6A7EA7E4816D9224C380959FF4EDE66C79777152