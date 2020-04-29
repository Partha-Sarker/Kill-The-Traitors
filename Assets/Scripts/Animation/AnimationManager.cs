using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float layerWeightSwitchTime = .2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetLayerWeight(int index, float target)
    {
        StartCoroutine(ChangeAnimatorLayerWeight(index, animator.GetLayerWeight(index), target));
    }

    IEnumerator ChangeAnimatorLayerWeight(int index, float source, float target)
    {
        float startTime = Time.time;
        float weight;
        while (Time.time < startTime + layerWeightSwitchTime)
        {
            weight = Mathf.Lerp(source, target, (Time.time - startTime) / layerWeightSwitchTime);
            animator.SetLayerWeight(index, weight);
            yield return null;
        }
        animator.SetLayerWeight(index, target);
    }
}
