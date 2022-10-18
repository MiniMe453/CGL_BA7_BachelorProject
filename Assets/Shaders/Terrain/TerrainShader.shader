// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/MainTerrainTexture"
{
	Properties
	{
		_MacroColor("MacroColor", 2D) = "white" {}
		_DetailNormal("DetailNormal", 2D) = "bump" {}
		_DetailColor("DetailColor", 2D) = "white" {}
		_Float0("Float 0", Float) = 1
		_Float2("Float 1", Float) = 5000
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _DetailNormal;
		uniform float _Float2;
		uniform sampler2D _DetailColor;
		uniform sampler2D _MacroColor;
		uniform float _Float0;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_Float2).xx;
			float2 uv_TexCoord9 = i.uv_texcoord * temp_cast_0;
			o.Normal = UnpackNormal( tex2D( _DetailNormal, uv_TexCoord9 ) );
			float2 temp_cast_1 = (_Float0).xx;
			float2 uv_TexCoord2 = i.uv_texcoord * temp_cast_1;
			o.Albedo = ( tex2D( _DetailColor, uv_TexCoord9 ) * CalculateContrast(1.25,tex2D( _MacroColor, uv_TexCoord2 )) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18935
-1895;31;1839;914;1166.852;361.7927;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;4;-1656.536,-18.70551;Inherit;False;Property;_Float0;Float 0;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-1439.852,356.2073;Inherit;False;Property;_Float2;Float 1;4;0;Create;True;0;0;0;False;0;False;5000;5000;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1099.536,17.29449;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1090.901,314.4155;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-729.3359,-27.08493;Inherit;True;Property;_MacroColor;MacroColor;0;0;Create;True;0;0;0;False;0;False;-1;20d36a52cbc00ed4aad635ac3f33bc23;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-680.8521,-292.7927;Inherit;True;Property;_DetailColor;DetailColor;2;0;Create;True;0;0;0;False;0;False;-1;7870e9ab37965fc4ca8c711d3a1d9b1c;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;13;-347.8521,-25.79269;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;1.25;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;8;-806.7651,265.7772;Inherit;True;Property;_DetailNormal;DetailNormal;1;0;Create;True;0;0;0;False;0;False;-1;dcbd74f16aae50d43b4c89a814e2948d;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-38.85205,-105.7927;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;318,77;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/MainTerrainTexture;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;4;0
WireConnection;9;0;10;0
WireConnection;1;1;2;0
WireConnection;11;1;9;0
WireConnection;13;1;1;0
WireConnection;8;1;9;0
WireConnection;12;0;11;0
WireConnection;12;1;13;0
WireConnection;0;0;12;0
WireConnection;0;1;8;0
ASEEND*/
//CHKSM=D958B6FAB4B420A48981FC0D47A6E6E5C502161C