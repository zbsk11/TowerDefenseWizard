using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private float originalAcceleration;
    private float originalAngularSpeed;
    private float originalSpeed;
    private bool speedDivided = false;

    private static int numCounter = 1;

    protected GameController gameController;
    protected NavMeshAgent agent;
    private int number;

    [SerializeField] private int reward = 5;
    [SerializeField] private int health = 1;

    public int Health { get => health; set => health = value; }
    public int Number { get => number; set => number = value; }
    public bool SpeedDivided { get => speedDivided; set => speedDivided = value; }

    void Start()
    {
        Number = numCounter;
        numCounter++;

        gameController = FindObjectOfType<GameController>();
        agent = GetComponent<NavMeshAgent>();

        agent.destination = GameController.Finish;

        originalAcceleration = agent.acceleration;
        originalAngularSpeed = agent.angularSpeed;
        originalSpeed = agent.speed;
    }

    public IEnumerator DivideSpeed(float divider)
    {
        agent.acceleration = originalAcceleration / divider;
        agent.angularSpeed = originalAngularSpeed / divider;
        agent.speed = originalSpeed / divider;

        SpeedDivided = true;

        yield return new WaitForSeconds(1);

        if(agent != null) RestoreSpeed();
    }

    public void RestoreSpeed()
    {
        agent.acceleration = originalAcceleration;
        agent.angularSpeed = originalAngularSpeed;
        agent.speed = originalSpeed;

        SpeedDivided = false;
    }

    private void Update()
    {
        if(agent.remainingDistance < 2)
        {
            Destroy(gameObject);
            GameController.PlayerHealth--;
        }

        if(health <= 0)
        {
            Destroy(gameObject);
            gameController.Money += reward;
        }
    }
}
