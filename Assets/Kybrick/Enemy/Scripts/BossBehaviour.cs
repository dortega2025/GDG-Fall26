using NUnit.Framework;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    
    [SerializeField] private float Hp;
    [SerializeField] private float InvulMax;
    [SerializeField] private float AtkTimer;

    [SerializeField] private GameObject AtkObA;
    [SerializeField] private GameObject AtkGenA;
    [SerializeField] private int ProbabilityA;
    [SerializeField] private float AtkCoolDwnA;

    [SerializeField] private GameObject AtkObB;
    [SerializeField] private GameObject AtkGenB;
    [SerializeField] private int ProbabilityB;
    [SerializeField] private float AtkCoolDwnB;

    [SerializeField] private GameObject AtkObC;
    [SerializeField] private GameObject AtkGenC;
    [SerializeField] private int ProbabilityC;
    [SerializeField] private float AtkCoolDwnC;
    private int RNG;
    void Update()
    {
        if (AtkTimer - 1*Time.deltaTime >= 0)
        {
            AtkTimer -= 1 * Time.deltaTime;
        }
        else
        {
            AtkTimer = 0;
        }

        if(AtkTimer == 0)
        {
            ProbRoll();
        }

    }

    public void ProbRoll()
    {
        RNG = Random.Range(1, 100);
        if(RNG >= 0 && RNG <= ProbabilityA)
        {
            AttackA();
        }
        else if (RNG > ProbabilityA && RNG <= ProbabilityB)
        {
            AttackB();
        }
        else if (RNG > ProbabilityB && RNG <= ProbabilityC)
        {
            AttackC();
        }
    }

    public void AttackA()
    {
        //insert what happens
        AtkTimer = AtkCoolDwnA;
    }
    public void AttackB()
    {
        //insert what happens
        AtkTimer = AtkCoolDwnB;
    }
    public void AttackC()
    {
        //insert what happens
        AtkTimer = AtkCoolDwnC;
    }
    public void TakeDamage(int DMG)
    {
        Hp -= DMG;
    }
}