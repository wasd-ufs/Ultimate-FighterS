using UnityEngine;
using UnityEngine.Serialization;

public class CatRight : CatBaseMoves
{
    [FormerlySerializedAs("_animator")] [SerializeField] private Animator animator;
    [FormerlySerializedAs("_skin")] [SerializeField] private GameObject skin;

    private void Awake()
    {
        skin.SetActive(false);
    }

    public override void Execute()
    {
        skin.SetActive(true);
        animator.Play("CatRightPunch", -1);
    }

    public override void Hide()
    {
        skin.SetActive(false);
    }
}