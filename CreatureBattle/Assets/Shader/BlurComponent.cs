using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurComponent : MonoBehaviour {

    [SerializeField]
    Shader shader;
    Material material;
    Texture prevTexture;
    private void Start()
    {
        material = new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetTexture("_SubTex", prevTexture);
        Graphics.Blit(source, destination, material);
        prevTexture = destination;
    }
}
