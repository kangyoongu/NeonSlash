using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SnapshotMode : MonoBehaviour
{
    private Shader neonShader;
    private Shader bloomShader;

    private SnapshotFilter filters;

    private int filterIndex = 0;

    private void Awake()
    {
        neonShader = Shader.Find("Snapshot/Neon");
        bloomShader = Shader.Find("Snapshot/Bloom");

        filters = new NeonFilter("Neon", Color.cyan, bloomShader, 
            new BaseFilter("", Color.white, neonShader));
    }
    // Delegate OnRenderImage() to a SnapshotFilter object.
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        filters.OnRenderImage(src, dst);
    }
}
