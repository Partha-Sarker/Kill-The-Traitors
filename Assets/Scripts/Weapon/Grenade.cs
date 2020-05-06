using UnityEngine;

public class Grenade : IThrowable
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

    public override void StartThrowingPreparation()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        transform.parent = null;
        col1.enabled = true;
        col2.enabled = true;
    }
}
