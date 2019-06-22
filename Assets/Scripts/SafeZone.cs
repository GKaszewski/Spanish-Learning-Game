using UnityEngine;

public class SafeZone : MonoBehaviour{
    private float timer;
    private Transform player;
    public float maxTime = 5f;

    private void Awake(){
        player = GameObject.FindWithTag("Player").transform;
    }
    private void Update(){
        if(timer >= maxTime){
            player.position += Vector3.right * 5f;
        }
    }
    private void OnTriggerStay(Collider other){
        Debug.Log("Something is in me!");
        if(other.transform == player){
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.transform == player){
            //timer = 0;
        }
    }
}