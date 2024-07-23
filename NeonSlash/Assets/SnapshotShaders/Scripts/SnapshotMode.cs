﻿using UnityEngine;

public class SnapshotMode : MonoBehaviour
{
    private Shader neonShader;
    private Shader bloomShader;
    SnapshotFilter filter;
    private void Awake()
    {
        neonShader = Shader.Find("Snapshot/Neon");
        bloomShader = Shader.Find("Snapshot/Bloom");

        filter = new NeonFilter("Neon", Color.cyan, bloomShader, new BaseFilter("", Color.white, neonShader));
    }

    // Delegate OnRenderImage() to a SnapshotFilter object.
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        filter.OnRenderImage(src, dst);
    }
}
