Shader "BB_RimSpecularEmissive"
{
	Properties 
	{
_rimcolor("_rimcolor", Color) = (0.4477612,1,0.8841457,1)
_specularpower("_specularpower", Range(0,10) ) = 1
_rimemmisive("_rimemmisive", Range(0,5) ) = 1
_emmisivemap("_emmisivemap", 2D) = "black" {}
_speculargloss("_speculargloss", 2D) = "white" {}
_diffusecolor("_diffusecolor", Color) = (0,0,0,1)
_diffusemap("_diffusemap", 2D) = "gray" {}
_normalmap("_normalmap", 2D) = "bump" {}

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 2.0


float4 _rimcolor;
float _specularpower;
float _rimemmisive;
sampler2D _emmisivemap;
sampler2D _speculargloss;
float4 _diffusecolor;
sampler2D _diffusemap;
sampler2D _normalmap;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}

			inline half4 LightingBlinnPhongEditor_DirLightmap (EditorSurfaceOutput s, fixed4 color, fixed4 scale, half3 viewDir, bool surfFuncWritesNormal, out half3 specColor)
			{
				UNITY_DIRBASIS
				half3 scalePerBasisVector;
				
				half3 lm = DirLightmapDiffuse (unity_DirBasis, color, scale, s.Normal, surfFuncWritesNormal, scalePerBasisVector);
				
				half3 lightDir = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
				half3 h = normalize (lightDir + viewDir);
			
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular * 128.0);
				
				// specColor used outside in the forward path, compiled out in prepass
				specColor = lm * _SpecColor.rgb * s.Gloss * spec;
				
				// spec from the alpha component is used to calculate specular
				// in the Lighting*_Prepass function, it's not used in forward
				return half4(lm, spec);
			}
			
			struct Input {
				float2 uv_diffusemap;
float2 uv_normalmap;
float2 uv_emmisivemap;
float3 viewDir;
float2 uv_speculargloss;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D0=tex2D(_diffusemap,(IN.uv_diffusemap.xyxy).xy);
float4 Multiply3=_diffusecolor * Tex2D0;
float4 Tex2DNormal0=float4(UnpackNormal( tex2D(_normalmap,(IN.uv_normalmap.xyxy).xy)).xyz, 1.0 );
float4 Tex2D2=tex2D(_emmisivemap,(IN.uv_emmisivemap.xyxy).xy);
float4 Fresnel1_1_NoInput = float4(0,0,1,1);
float4 Fresnel1=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel1_1_NoInput.xyz ) )).xxxx;
float4 Pow1=pow(Fresnel1,_rimemmisive.xxxx);
float4 Multiply1=Pow1 * _rimcolor;
float4 Multiply2=Tex2D2 * Multiply1;
float4 Tex2D1=tex2D(_speculargloss,(IN.uv_speculargloss.xyxy).xy);
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
float4 Pow0=pow(Fresnel0,_specularpower.xxxx);
float4 Multiply0=Pow0 * _rimcolor;
float4 Multiply6=Tex2D1 * Multiply0;
float4 Master0_3_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply3;
o.Normal = Tex2DNormal0;
o.Emission = Multiply2;
o.Gloss = Multiply6;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}