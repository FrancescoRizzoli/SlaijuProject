Shader "Custom/OutlineShader"
{
    Properties
    {
        _OutlineCol("Outline Color", Color) = (0, 0, 0, 1)
		_OutlineSize("Outline Size", Float) = 0.025
    }

    SubShader
    {
        Pass
		{
			Name "Outline"

			Cull Front

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct MeshData
			{
				float4 position : POSITION;
				float3 normal : NORMAL;
			};
			struct V2F
			{
				float4 position : POSITION;
			};

			float _OutlineSize;

			V2F vert(MeshData meshData)
			{
				V2F OUT;

				float4 vertPos = meshData.position;
				vertPos.xyz += meshData.normal * _OutlineSize;

				OUT.position = UnityObjectToClipPos(vertPos);

				return OUT;
			}

			float4 _OutlineCol;

			float4 frag(V2F IN) : SV_Target
			{
				return _OutlineCol;
			}
			ENDHLSL
		}
    }
}
