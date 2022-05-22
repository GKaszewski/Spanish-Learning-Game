using UnityEngine;

public class Dancing : MonoBehaviour {
    private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
        var randomNumber = Random.Range(0, 3);
        switch (randomNumber) {
            case 0:
                _animator.Play("twerk");
                return;
            case 1:
                _animator.Play("dancing");
                return;
            case 2:
                _animator.Play("hiphop");
                return;
        }
    }
}
