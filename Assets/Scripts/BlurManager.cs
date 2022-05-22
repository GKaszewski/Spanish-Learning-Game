using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurManager : MonoBehaviour {
    [SerializeField]
    public Camera _blurCamera;

    [SerializeField]
    private Material _blurMaterial;

    private void Start() {
        if (_blurCamera.targetTexture != null) _blurCamera.targetTexture.Release();
        _blurCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, 1);
        _blurMaterial.SetTexture("_RenText", _blurCamera.targetTexture);
    }
}
