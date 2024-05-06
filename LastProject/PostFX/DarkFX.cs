using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;

namespace LastProject
{
    class DarkFX : PostProcessingEffect
    {
        //Shader for a vignette (not working but still useful to make the setting dark)
        private static string fragmentShader = @" 

            #version 330 core

            in vec2 uv;
  
            uniform vec2 u_resolution;
            uniform sampler2D u_sampler2D;

            out vec4 out_color;

            const float outerRadius = .65, innerRadius = .3, intensity = .6;

            void main()
            {
                vec4 color = texture(u_sampler2D, uv);
                
                vec2 relativePosition = gl_FragCoord.xy / u_resolution - .5;
                float len = length(relativePosition);
                float vignette = smoothstep(outerRadius, innerRadius, len);
                color.rgb = mix(color.rgb, color.rgb * vignette, intensity);

                out_color = color;
            }

        ";

        public DarkFX() : base(fragmentShader) { }
    }
}
