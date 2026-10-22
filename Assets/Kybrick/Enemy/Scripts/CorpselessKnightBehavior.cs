using NUnit.Framework;
using UnityEngine;

public class CorpselessKnightBehavior : MonoBehaviour
{
    [SerializeField] private float Hp;
    [SerializeField] private float InvulMax;
    [SerializeField] private float AtkTimer;

    [SerializeField] private GameObject AtkObA;//Green Flame Nodes
    [SerializeField] private GameObject AtkGenA;
    [SerializeField] private int ProbabilityA;
    [SerializeField] private float AtkCoolDwnA;

    [SerializeField] private GameObject AtkObB;//GreenFlame blades
    [SerializeField] private GameObject AtkGenB;
    [SerializeField] private int ProbabilityB;
    [SerializeField] private float AtkCoolDwnB;

    [SerializeField] private GameObject AtkObC;//Green Flame Blast
    [SerializeField] private GameObject AtkGenC;
    [SerializeField] private int ProbabilityC;
    [SerializeField] private float AtkCoolDwnC;

    [SerializeField] private GameObject AtkObD;//Green Flame Chains
    [SerializeField] private GameObject AtkGenD;
    [SerializeField] private int ProbabilityD;
    [SerializeField] private float AtkCoolDwnD;
    private int RNG;
    void Update()
    {
        if (AtkTimer - 1 * Time.deltaTime >= 0)
        {
            AtkTimer -= 1 * Time.deltaTime;
        }
        else
        {
            AtkTimer = 0;
        }

        if (AtkTimer == 0)
        {
            ProbRoll();
        }

    }

    public void ProbRoll()
    {
        RNG = Random.Range(1, 100);
        if (RNG >= 0 && RNG <= ProbabilityA)
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
        //RNG = 1- 2 2= r 1=l
        //
        AtkTimer = AtkCoolDwnC;
    }
    public void AttackD()
    {
        //RNG = 1 - NodeSpawn#
        //GameOBject Spawn1 = GameObject.Find(NodeSpawn(clone) + RNG);
        //Instantiate(AtkObD,Spawn1);
        //repeat as many times as you want. 
        AtkTimer = AtkCoolDwnD;
    }
    public void TakeDamage(int DMG)
    {
        Hp -= DMG;
    }
}