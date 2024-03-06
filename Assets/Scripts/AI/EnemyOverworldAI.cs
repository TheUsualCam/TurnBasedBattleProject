using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FSM;

public class EnemyOverworldAI : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public float seekDistance;
    public float seekSpeed;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    private int waypointNum = 0;
    private float currentDistance;
    private NavMeshAgent agent;
    private float agentSpeed;
    private GameObject player;
    private StateMachine enemyOverworld_fsm;
    private Vector3 direction;
    private RaycastHit hit;
    private bool canSeePlayer = false;
    private bool battleStarted = false;
    private BattleSystem battleSystem;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agentSpeed = agent.speed;
        agent.SetDestination(waypoints[waypointNum].position);
        player = GameObject.Find("PlayerOverworld");
        battleSystem = GameObject.Find("Battle Environment").GetComponent<BattleSystem>();

        enemyOverworld_fsm = new StateMachine(this);

        enemyOverworld_fsm.AddState("Patrolling", new State(onLogic: (state) => CheckIfDestinationReached()));
        enemyOverworld_fsm.AddState("Seeking", new State(onLogic: (state) => Seek()));

        enemyOverworld_fsm.AddTransition(new Transition(
            "Patrolling",
            "Seeking",
            (transition) => currentDistance <= seekDistance && canSeePlayer
        ));

        enemyOverworld_fsm.AddTransition(new Transition(
            "Seeking",
            "Patrolling",
            (transition) => !canSeePlayer
        ));

        enemyOverworld_fsm.SetStartState("Patrolling");
        enemyOverworld_fsm.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        currentDistance = Vector3.Distance(player.transform.position, transform.position);
        
        if(Physics.Raycast(transform.position, transform.forward * 100, out hit))
        {
            if(hit.collider.tag == "Player")
            {
                canSeePlayer = true;
            }
        }

        if(currentDistance > seekDistance)
        {
            canSeePlayer = false;
            agent.speed = agentSpeed;
        }
        
        enemyOverworld_fsm.OnLogic();
    }

    public void CheckIfDestinationReached()
    {
        if (transform.position.x == agent.pathEndPosition.x && transform.position.z == agent.pathEndPosition.z)
        {
            if (waypointNum >= waypoints.Count - 1)
            {
                waypointNum = 0;
            }
            else
            {
                waypointNum++;
            }

            StartCoroutine(WaitBeforeMovingToNextWaypoint());
        }
    }

    public void Seek()
    {
        agent.speed = seekSpeed;
        agent.SetDestination(player.transform.position);
    }

    IEnumerator WaitBeforeMovingToNextWaypoint()
    {
        yield return new WaitForSeconds(3.0f);

        agent.SetDestination(waypoints[waypointNum].position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player && !battleStarted)
        {
            battleStarted = true;
            battleSystem.StartBattle(this.gameObject, enemiesToSpawn);
        }
    }

    public void ResetBattleStarted()
    {
        battleStarted = false;
    }
}
