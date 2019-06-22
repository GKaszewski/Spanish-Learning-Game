using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour{

    private Animator anim;
    private NavMeshAgent agent;
    private Transform player;
    private bool isAttacking = false;
    private float timer = 0;

    private void Awake(){
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        agent.SetDestination(player.position);
    }

    private void Update(){
        timer += Time.deltaTime;
        if(Vector3.Distance(transform.position, player.position) <= 2f){
            agent.isStopped = true;
            anim.SetFloat("speed", 0f);
            isAttacking = true;
        }else{
            agent.isStopped = false;
            agent.SetDestination(player.position);
            anim.SetFloat("speed", 1f);
        }

        if(isAttacking){
            if(timer >= 2f)
                Attack();
        }
    }

    private void Attack(){
        anim.SetTrigger("attack");
        player.GetComponent<PlayerHealth>().TakeDamage();
        isAttacking = false;
        timer = 0f;
    }

}