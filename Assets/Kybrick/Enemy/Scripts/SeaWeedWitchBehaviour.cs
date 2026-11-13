using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
    [SerializeReference] private GameObject Door;
    [SerializeReference] private GameObject Self;
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
        if (TimerA == 0 && Speed != OldSpd)
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
        Instantiate(AtkObA, AtkGenA.transform.localPosition, AtkGenA.transform.localRotation);
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
        if (Hp <= 0)
        {
            Destroy(Door);
            Destroy(Self);
        }
    }
    ////////
    /// [SerializeReference] public GameObject SpawnNextRoomPoint;
    //[SerializeReference] public GameObject RoomStart;
    //[SerializeReference] public GameObject Hall;
    //[SerializeReference] private GameObject Placeholder;
    //[SerializeReference] public GameObject[] Possibilities;
    //[SerializeReference] public Collider2D CheckZone;
    //public bool Terminate;
    //public bool StartHall;
    //public bool TranHall;
    //private GameObject Chosen;
    //int Check;
    //int DeathCount;
    //void Start()
    //{
    //    if (TranHall)
    //    {
    //        Check = 0;
    //        while (Check <= Possibilities.Length - 1)
    //        {
    //            FindBosses();
    //            Debug.Log(Possibilities[Check]);
    //            Check++;
    //            Debug.Log(Check);
    //        }
    //        if (Check >= Possibilities.Length - 1)
    //        {
    //            Debug.Log("Transition");

    //            Transition();

    //        }
    //    }
    //    else
    //    {
    //        Roll();
    //        if (Terminate == true && TranHall == false)
    //        {
    //            Instantiate(Hall, SpawnNextRoomPoint.transform.position, SpawnNextRoomPoint.transform.rotation);
    //        }
    //        if (Terminate == false)
    //        {
    //            Instantiate(Chosen, SpawnNextRoomPoint.transform.position, SpawnNextRoomPoint.transform.rotation);
    //            Terminate = true;
    //        }

    //    }

    //}
    //void FindBosses()
    //{
    //    GameObject Find = GameObject.Find((Possibilities[Check].name + "(Clone)"));
    //    if (Find != null)
    //    {

    //        Possibilities[Check] = Placeholder;
    //        Debug.Log(Possibilities[Check]);
    //    }
    //}
    //void Transition()
    //{
    //    Roll();
    //    if (Terminate == false)
    //    {
    //        Instantiate(Chosen, SpawnNextRoomPoint.transform.position, SpawnNextRoomPoint.transform.rotation);
    //    }
    //}
    //void Roll()
    //{
    //    if (DeathCount > 25)
    //    {
    //        Debug.Log("DeadLoop");
    //        Terminate = true;
    //    }
    //    else
    //    {
    //        int RNG = Random.Range(0, Possibilities.Length - 1);
    //        Chosen = Possibilities[RNG];
    //        if (Chosen == Placeholder)
    //        {
    //            Debug.Log(DeathCount);
    //            DeathCount++;
    //            Roll();
    //        }
    //    }
    //}


}
