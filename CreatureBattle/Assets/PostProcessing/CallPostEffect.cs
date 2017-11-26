using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallPostEffect : MonoBehaviour {

    public Shader shader;
    Material material;
    private void Start()
    {
        material = new Material(shader);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
