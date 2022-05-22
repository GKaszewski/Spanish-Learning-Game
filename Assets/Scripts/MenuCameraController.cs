using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour {

    [SerializeField]
    private float _camSpeed = 5f;

    private void Start() {
        LeanTween.rotateAround(gameObject, Vector3.up, 360f, _camSpeed).setLoopClamp();
    }

}
