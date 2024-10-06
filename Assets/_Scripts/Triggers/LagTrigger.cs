using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagTrigger : MonoBehaviour
{
    [SerializeField] private HitlagState hitlagState;
    private bool _lagTrigger;

    public bool lagTrigger
    {
        get { return _lagTrigger; }    
        set { _lagTrigger = value; }
    }

    //Falta saber onde o lag vai ser verdadeiro - no hitbox?
    //Onde o hitlag vai ser chamado?
    //

    public void OnLag(StateMachine<CharacterState> machine)
    {
        if(_lagTrigger)
        {
            machine.TransitionTo(hitlagState);
        }
    }

}
