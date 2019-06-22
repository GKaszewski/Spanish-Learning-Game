using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour{

    private ObjectPooler pooler;
    private static System.Random rnd = new System.Random();
    private int currentEnemies;

    public List<Transform> spawnpoints;
    public int enemyCount;
    public float waveBreak = 10f;
    public float spawningRate = 5f;

    public int waves;
    public TMP_Text waveLabel;

    private void Awake(){
        pooler = ObjectPooler.SharedInstance;
        currentEnemies = enemyCount;
        CallSpawn();
    }

    private void CallSpawn(){
        InvokeRepeating("Spawn", 1f, spawningRate);
    }

    private void Update(){
        waveLabel.text = $"WAVES LEFT: {waves}";
        if(currentEnemies == 0){
            CancelInvoke();
            waves--;
            currentEnemies = enemyCount;
            StartCoroutine(Wait());
            Invoke("CallSpawn", 0f); 
        }

        if(waves == 0){
            CancelInvoke();
            Debug.Log("Wow, you've won!");
        }
    }

    private void Spawn(){
        Debug.Log("I'm spawning!");
        if(pooler.GetPooledObject(0) == null) Debug.Log("Object is null!");
        var enemy = pooler.GetPooledObject(0);
        enemy.transform.position = spawnpoints[rnd.Next(spawnpoints.Count)].position;
        enemy.SetActive(true);
        currentEnemies--;
    }

    private IEnumerator Wait(){
        yield return new WaitForSeconds(waveBreak);
    }
}