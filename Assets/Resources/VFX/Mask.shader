Shader "Custom/Mask"
{
    SubShader
    {
        Tags{"RenderType" = "Opaque"}
        ColorMask 0
        Pass {
            ZWrite On
            Color (1,1,1,1)
            Stencil
			{
				Ref 1
				ReadMask 255
				WriteMask 255
				Comp Always
				Pass Replace
			}

            // Stencil {
            //     Ref 1
            //     Comp Always
            //     Pass Replace
            // }
        }
    }
}
