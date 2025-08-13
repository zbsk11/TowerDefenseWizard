using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float speed = 10f;
    private Enemy target;

    public Enemy Target { get => target; set => target = value; }

    void Update()
    {
        if(Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
        if(go.CompareTag("Enemy"))
        {
            go.GetComponent<Enemy>().Health -= damage;
            Destroy(gameObject);
        }
    }
}
