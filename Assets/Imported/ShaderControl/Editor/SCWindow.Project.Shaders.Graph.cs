/// <summary>
/// Shader Control - (C) Copyright 2016-2022 Ramiro Oliva (Kronnect)
/// </summary>
/// 
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

namespace ShaderControl {

    public partial class SCWindow : EditorWindow {

        const int SHADER_GRAPH_KEYWORD_DEFINITION_MULTI_COMPILE = 1;
        const int SHADER_GRAPH_KEYWORD_DEFINITION_SHADER_FEATURE = 0;
        const int SHADER_GRAPH_KEYWORD_SCOPE_GLOBAL = 1;
        const int SHADER_GRAPH_KEYWORD_SCOPE_LOCAL = 0;

        void ScanShaderGraph(SCShader shader) {

            // Reads shader
            string shaderContents = File.ReadAllText(shader.path, Encoding.UTF8);

            // Check shader graph version
            if (shaderContents.Contains("m_SGVersion")) {
                shader.shaderGraphVersion = 3;
                ScanShaderGraphV2(shader, shaderContents);
            } else if (shaderContents.Contains("m_Version")) {
                shader.shaderGraphVersion = 2;
                ScanShaderGraphV2(shader, shaderContents);
            } else {
                shader.shaderGraphVersion = 1;
                ScanShaderGraphV1(shader, shaderContents);
            }
        }



        void UpdateShaderGraph(SCShader shader) {
            // Unsupported; use the build view to exclude keywords from this shader graph
        }

        void ConvertToLocalGraph(SCKeyword keyword, SCShader shader) {
            if (shader.shaderGraphVersion == 1) {
                ConvertToLocalGraphV1(keyword, shader);
            } else {
                ConvertToLocalGraphV2(keyword, shader);
            }

        }
    }
}