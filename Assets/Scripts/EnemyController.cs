using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public GameObject player;
    Vector3 previousTargetPosition;

    void Start()
    {
        previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);
        player = GameObject.FindGameObjectWithTag("Player");
        this.GetComponent<NavMeshAgent>().destination = player.transform.position;
        StartCoroutine(FollowTarget(100.0f, player.transform));
    }

    private IEnumerator FollowTarget(float range, Transform target)
    {
        while (Vector3.SqrMagnitude(transform.position - target.position) > 0.1f)
        {
            // did target move more than at least a minimum amount since last destination set?
            if (Vector3.SqrMagnitude(previousTargetPosition - target.position) > 1.0f)
            {
                Debug.Log("Recalculate");
                this.GetComponent<NavMeshAgent>().SetDestination(target.position);
                previousTargetPosition = target.position;
            }
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }
}
