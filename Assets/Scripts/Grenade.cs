using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private GameObject blastEffect;
    [SerializeField] private float cookTime = 4, blastDestroyTime = 5, damageAreaRadius, damage;

    private void Start()
    {
        Invoke("Blast", cookTime);
    }

    private void Blast()
    {
        GameObject tempEffect = Instantiate(blastEffect, transform.position, Quaternion.identity);
        Destroy(tempEffect, blastDestroyTime);
        Destroy(this.gameObject);
    }
}
