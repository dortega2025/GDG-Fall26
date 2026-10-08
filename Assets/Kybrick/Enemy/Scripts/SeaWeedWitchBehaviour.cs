using Unity.VisualScripting;
using UnityEngine;

public class SeaWeedWitchBehaviour : MonoBehaviour
{

    [SerializeField] private float Hp;
    [SerializeField] private float InvulMax;
    [SerializeField] private float AtkTimer;
    [SerializeField] private float Speed;
    [SerializeField] private float TravelDistance;
    [SerializeField] private GameObject AtkObA;
    [SerializeField] private GameObject AtkGenA;
    [SerializeField] private int ProbabilityA;
    [SerializeField] private float AtkCoolDwnA;
    [SerializeReference] private GameObject NodeLU;//the left/up node
    [SerializeReference] private GameObject NodeRD;//the right/down node
    [SerializeField] private GameObject AtkObB;
    [SerializeField] private int ProbabilityB;
    [SerializeField] private float AtkCoolDwnB;
    [SerializeField] private GameObject AtkObC;
    [SerializeField] private int ProbabilityC;
    [SerializeField] private float AtkCoolDwnC;
    private int RNG;
    private bool Dir;
    private float TimerA;
    float Distance;//how close it is to next Nodule
    public float DistTolerance;//how close the platform can be before turning around
    private float OldSpd;
    void Start()
    {
        OldSpd = Speed;
    }
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

        if (TimerA - 1 * Time.deltaTime >= 0)
        {
            TimerA -= 1 * Time.deltaTime;
        }
        else
        {
            TimerA = 0;
        }
        if(TimerA == 0 && Speed != OldSpd)
        {
            Speed = OldSpd;
        }
    }
    private void FixedUpdate()
    {
        if (Dir == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, NodeLU.transform.position, Speed * Time.deltaTime);
            //moving to LU
            Distance = Vector2.Distance(transform.position, NodeLU.transform.position);
            //Checking distanct to LU
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, NodeRD.transform.position, Speed * Time.deltaTime);
            //moving to RD
            Distance = Vector2.Distance(transform.position, NodeRD.transform.position);
            //Checking distance to RD
        }

        if (Distance <= DistTolerance)
        {
            if (Dir == true)
            {
                Dir = false;
                //if going to LU go then go to RD
            }
            else
            {
                Dir = true;
                //else going to RD then go to LU
            }
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
         OldSpd = Speed;
        Speed = Speed / 3;
        TimerA = 6;
        Instantiate(AtkObA,AtkGenA.transform.localPosition,AtkGenA.transform.localRotation);
        AtkTimer = AtkCoolDwnA;
    }
    public void AttackB()
    {

        Instantiate(AtkObB, AtkGenA.transform.position, AtkGenA.transform.localRotation);
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
