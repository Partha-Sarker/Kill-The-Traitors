using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100, currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamge(int damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        GetComponent<Animator>().SetBool("isDead", true);
        GetComponent<Animator>().SetTrigger("die");

        if (transform.CompareTag("Player"))
            FindObjectOfType<GameManager>().LoadDeadPanel();
        else
        {
            FindObjectOfType<GameManager>().DieEnemy();
            Destroy(this.gameObject, 3);
        }
    }



    private void AfterDeath()
    {
    }
}
