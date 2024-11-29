using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRight : CatBaseMoves
{

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _skin;

    void Awake()
    {
        _skin.SetActive(false);
    }

    public override void Execute()
    {
        _skin.SetActive(true);
        _animator.Play("CatRightPunch",-1);
    }
    public override void Hide()
    {
        _skin.SetActive(false);
    }
}
