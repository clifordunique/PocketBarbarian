// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Sprites/Character Sprite"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		//Outline
		_OutlineColor ("Outline Color", Color) = (1,1,1,1)
		//_OutlineWidth ("Outline Width", Range(0,4)) = 1
		_OverlayColor("Addative Overlay Color", Color) = (1,1,1,1)
		//Hidden
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile DUMMY PIXELSNAP_ON
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					half2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color, _OutlineColor;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color;
					#ifdef PIXELSNAP_ON
						OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				float4 _MainTex_TexelSize;

				fixed4 _OverlayColor;

				fixed4 frag(v2f IN) : COLOR
				{
					fixed4 col = tex2D(_MainTex,IN.texcoord);

					//Get color values of pixels adjacent
					float a = tex2D(_MainTex,IN.texcoord + fixed2(0, _MainTex_TexelSize.y)).a;
					a += tex2D(_MainTex, IN.texcoord + fixed2(0, -_MainTex_TexelSize.y)).a;
					a += tex2D(_MainTex, IN.texcoord + fixed2(-_MainTex_TexelSize.x,0)).a;
					a += tex2D(_MainTex, IN.texcoord + fixed2(_MainTex_TexelSize.x,0)).a;

					a = step(a,.5);//Clamp the a value
					col.rgb = lerp(_OutlineColor, col.rgb, col.a);
					col.a = step(a, col.a);

					col.rgb += _OverlayColor;
					col.rgb = min(col.rgb, 1);

					//Spit out a color
					return col;
				}

			ENDCG
			}
		}
}