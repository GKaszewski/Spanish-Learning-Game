using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScore : MonoBehaviour {

    private void Start() {
        GameManager.Points = 0;
        GameManager.Guessed = 0;
        GameManager.Skipped = 0;
    }

}
