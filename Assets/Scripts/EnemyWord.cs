using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWord : MonoBehaviour {
    public Word Word { get; private set; } = null;

    public void SetWord(Word word) => Word = word;
}
