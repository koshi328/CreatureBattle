using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionObject : MonoBehaviour {
    Material material;
    [SerializeField]
    static Camera projCam;
    static Texture projTex;
	// Use this for initialization
	void Start () {
        if (GetComponent<Terrain>() != null)
            material = GetComponent<Terrain>().materialTemplate;
        else
            material = GetComponent<Renderer>().material;
        if(!projCam)
        {
            projCam = GameObject.FindWithTag("ProjectionCamera").GetComponent<Camera>();
        }
        if(!projTex)
        {
            projTex = Resources.Load("PaintTex") as Texture;
        }
        Matrix4x4 projVP = projCam.projectionMatrix
    * projCam.worldToCameraMatrix;

        var biasMat = new Matrix4x4();
        biasMat.SetRow(0, new Vector4(0.5f, 0.0f, 0.0f, 0.5f));
        biasMat.SetRow(1, new Vector4(0.0f, 0.5f, 0.0f, 0.5f));
        biasMat.SetRow(2, new Vector4(0.0f, 0.0f, 0.5f, 0.5f));
        biasMat.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
        material.SetTexture("_ProjectionTex", projTex);
        material.SetMatrix("_ProjVP", biasMat * projVP);
    }
}
