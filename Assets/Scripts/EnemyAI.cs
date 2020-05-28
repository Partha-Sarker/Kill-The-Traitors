using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;

    private void OnTriggerEnter(Collider other)
    {
        enemyController.OnTargetEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        enemyController.OnTargetExit(other);
    }
}
