using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Caminar",true);
        animator.SetFloat("Forward", 1.5f);
    }
}
