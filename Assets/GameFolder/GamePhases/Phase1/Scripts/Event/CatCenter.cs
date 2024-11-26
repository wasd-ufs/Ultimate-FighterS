using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCenter : CatBaseMoves
{

    [SerializeField]private Animator _animator;
    [SerializeField]private GameObject _skin;

    void Awake()
    {
        _skin = transform.GetChild(0).gameObject;
        _animator = GetComponentInChildren<Animator>();
        _skin.SetActive(false);
    }

    public override bool ShowAnimator()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Trigger_Center"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Execute()
    {
        _skin.SetActive(true);
        _animator.SetTrigger("Trigger_Center");
    }

    public override void Hide()
    {
        _skin.SetActive(false);
    }
}
