using Unity.VisualScripting;
using UnityEngine;

public class ChestMvmnt : MonoBehaviour
{
    [Header("Mvmnt")]
   [SerializeField] private bool ChestBusy;//if it is taking an action
    [SerializeField] private int ChestJob;//what action its taking 0=move 1=ChestAtk1 etc
    [SerializeField]private bool ChestWaiting;//if it is not moving
    [SerializeField]private float ChestWaitingTime;//how long it waits
    [SerializeField]private float ChestWaitingMax;//how long it waits
    [SerializeField]private float ChestDef;//How much damage it reduces from being hit
    [SerializeField] private float ChestSpd;//how fast it moves
    [SerializeField] private float ChestDistTol;
    [SerializeReference] private GameObject Chest;
    [SerializeReference] private GameObject CurrentChestPnt;// where they are at
    [SerializeReference] private GameObject NextChestPnt;// where they will go next
    [SerializeReference] private GameObject LastChestPnt;// where they were last
    float ChestDir;
    [Header("Atks")]
    [SerializeField] private float ChestChestAtkTimer;//how often it can take an action
    [SerializeField] private float ChestExtraTimer;//how often it can take an action
    [SerializeReference] private GameObject ChestAtkObA;//Green Flame chains
    [SerializeReference] private GameObject ChestAtkGenA; // if there is a specific spawner 
    [SerializeField] private int ChestProbA; // how likely it is to trigger
    [SerializeField] private float ChestAtkCoolDwnA;// how long they must wait to attk again
    [SerializeReference] private GameObject ChestAtkObB;//GreenFlame blast
    [SerializeReference] private GameObject ChestAtkGenB;
    [SerializeField] private int ChestProbB;
    [SerializeField] private float ChestAtkCoolDwnB;
    [SerializeReference] private GameObject ChestAtkObC;//Green Flame blad
    [SerializeReference] private GameObject ChestAtkGenC;
    [SerializeField] private int ChestProbC;
    [SerializeField] private float ChestAtkCoolDwnC;
    [SerializeReference] private GameObject ChestAtkObD;//Green Flame node
    [SerializeReference] private GameObject ChestAtkGenD;
    [SerializeField] private int ChestProbD;
    [SerializeField] private float ChestAtkCoolDwnD;

    public GameObject player;

    private int RNG;

    public void Update()
    {
        Timers();
        if (!ChestWaiting)
        {
            ChestBusy = true;
            ChestJob = 0;
            ChestMvmntChoice();
        }
        if (!ChestWaiting && ChestBusy && ChestJob == 0)
        {
            Chest.transform.position = Vector2.MoveTowards(CurrentChestPnt.transform.position, NextChestPnt.transform.position, ChestSpd);
            if (DistCheck(Chest, player, ChestDistTol))
            {
                ChestWaiting = true;
                ChestBusy = false;
                ChestWaitingTime = ChestWaitingMax;
                CurrentChestPnt = NextChestPnt;
            }
        }

        if (ChestBusy && ChestJob == 1)
        {
            if (ChestDir == 0)
            {
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, NextChestPnt.transform.position, ChestSpd + ChestSpd / 2);
                if (DistCheck(Chest, NextChestPnt, 1))
                {
                    ChestDir = 1;
                    NextChestPnt = GameObject.Find("ChestPos2");
                }
            }
            if (ChestDir == 1)
            {
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, NextChestPnt.transform.position, ChestSpd + ChestSpd / 2);
                GameObject N1 = GameObject.Find("ChestPos4");
                GameObject N2 = GameObject.Find("ChestPos1");
                GameObject N3 = GameObject.Find("ChestPos6");
                GameObject ChestAtk = Instantiate(ChestAtkObA, Chest.transform);
                Vector2 ChestDirection;
                if (Chest.transform.position.x == N1.transform.position.x)
                {
                    ChestDirection = Chest.transform.position - N1.transform.position;
                    ChestAtk.GetComponent<Rigidbody2D>().AddForce(ChestDirection.normalized * (1f * ChestSpd / 2), ForceMode2D.Impulse);
                }
                else if (Chest.transform.position.x == N2.transform.position.x)
                {
                    ChestDirection = Chest.transform.position - N2.transform.position;
                    ChestAtk.GetComponent<Rigidbody2D>().AddForce(ChestDirection.normalized * (1f * ChestSpd / 2), ForceMode2D.Impulse);
                }
                else if (Chest.transform.position.x == N3.transform.position.x)
                {
                    ChestDirection = Chest.transform.position - N3.transform.position;
                    ChestAtk.GetComponent<Rigidbody2D>().AddForce(ChestDirection.normalized * (1f * ChestSpd / 2), ForceMode2D.Impulse);
                }
                if (DistCheck(Chest, NextChestPnt, 1))
                {
                    ChestBusy = false;
                    ChestJob = 0;
                    ChestDir = 0;
                }
            }
        }

        if (ChestBusy && ChestJob == 2)
        {
            if (ChestDir == 0)
            {
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, LastChestPnt.transform.position, ChestSpd + ChestSpd / 2);
                if (DistCheck(Chest, LastChestPnt, 1))
                {
                    ChestDir = 1;
                }
            }

            if (ChestDir == 1)
            {
                Chest.transform.position = Vector2.MoveTowards(LastChestPnt.transform.position, NextChestPnt.transform.position, ChestSpd + ChestSpd / 2);
                if (DistCheck(Chest, NextChestPnt, 1))
                {
                    ChestDir = 2;
                }
                ChestExtraTimer += Time.deltaTime;
            }

            if (ChestDir == 2)
            {
                Chest.transform.position = Vector2.MoveTowards(NextChestPnt.transform.position, LastChestPnt.transform.position, ChestSpd + ChestSpd / 2);
                if (DistCheck(Chest, LastChestPnt, 1))
                {
                    ChestDir = 1;
                }
                ChestExtraTimer += Time.deltaTime;
            }

            if (ChestExtraTimer > 10)
            {
                ChestDir = 0;
                ChestBusy = false;
                ChestJob = 0;
                ChestExtraTimer = 0;
                Chest.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

        }

        if (ChestBusy && ChestJob == 3)
        {
            if (DistCheck(Chest, player, 8))
            {
                Chest.transform.Rotate(player.transform.position, -ChestSpd);
                ChestExtraTimer += Time.deltaTime;
                switch (ChestExtraTimer)
                {
                    case 1:
                        Instantiate(ChestAtkObC, Chest.transform);
                        break;
                    case 2:
                        Instantiate(ChestAtkObC, Chest.transform);
                        break;
                    case 3:
                        Instantiate(ChestAtkObC, Chest.transform);
                        break;
                    case 4:
                        Instantiate(ChestAtkObC, Chest.transform);
                        break;
                    case 5:
                        Instantiate(ChestAtkObC, Chest.transform);
                        break;
                    case 6:
                        ChestExtraTimer = 0;
                        ChestBusy = false;
                        ChestJob = 0;
                        break;
                }
            }
            else
            {
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, player.transform.position, ChestSpd + ChestSpd / 2);
            }
        }

        if (ChestBusy && ChestJob == 4)
        {
            GameObject Desti = GameObject.Find("ChestPos6");
            float angle = Chest.transform.localRotation.z;
            RNG = Random.Range(1, 4);
            angle -= (ChestSpd / RNG) * Time.deltaTime;
            if (DistCheck(Chest, Desti, 1))
            {
                ChestExtraTimer += Time.deltaTime;
                Chest.transform.localRotation = Quaternion.Euler(0, 0, angle);
                if (ChestExtraTimer == 6)
                {
                    ChestExtraTimer = 0;
                    ChestBusy = false;
                    ChestJob = 0;
                }
                else if (ChestExtraTimer == 1 || ChestExtraTimer == 2 || ChestExtraTimer == 3 || ChestExtraTimer == 4 || ChestExtraTimer == 5)
                {
                    GameObject ChestAtk = Instantiate(ChestAtkObD, Chest.transform);
                    Vector2 ChestDirection = Chest.transform.position - ChestAtkGenD.transform.position;
                    ChestAtk.GetComponent<Rigidbody2D>().AddForce(ChestDirection.normalized * (1f * ChestSpd), ForceMode2D.Impulse);
                }
            }
            else
            {
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, Desti.transform.position, ChestSpd + ChestSpd / 2);
            }
        }
    }
    public bool DistCheck(GameObject One, GameObject Two, float Dist)
    {
        bool Activate;
        if (Vector2.Distance(One.transform.position, Two.transform.position) <= Dist)
        {
            Activate = true;
        }
        else
        {
            Activate = false;
        }
        return Activate;
    }
    public void Timers()
    {
        if (ChestWaiting && !ChestBusy)
        {
            ChestWaitingTime -= Time.deltaTime;
            if (ChestWaitingTime < 0)
            {
                ChestWaitingTime = 0;
                ChestWaiting = false;
            }
        }

        if (ChestWaiting && !ChestBusy)
        {
            ChestChestAtkTimer -= Time.deltaTime;
            if (ChestChestAtkTimer < 0)
            {
                ChestChestAtkTimer = 0;
                ChestAttackRoll();
            }
        }

    }
    public void ChestMvmntChoice()
    {
        GameObject Node;
        RNG = Random.Range(1, 9);
        Node = GameObject.Find("ChestPos" + RNG);
        LastChestPnt = CurrentChestPnt;
        NextChestPnt = Node;
    }
    public void ChestAttackRoll()
    {
        RNG = Random.Range(1, 100);
        if (RNG >= ChestProbA && RNG < ChestProbB)
        {
            ChestChestAtkA();
        }
        else if (RNG >= ChestProbB && RNG < ChestProbC)
        {
            ChestChestAtkB();
        }
        else if (RNG >= ChestProbC && RNG < ChestProbD)
        {
            ChestChestAtkC();
        }
        else
        {
            ChestChestAtkD();
        }

    }
    public void ChestChestAtkA()//Chains
    {
        ChestJob = 1;
        NextChestPnt = GameObject.Find("ChestPos7");
        ChestChestAtkTimer = ChestAtkCoolDwnA;
    }
    public void ChestChestAtkB()//Blast
    {
        ChestJob = 2;
        GameObject N1 = LastChestPnt;
        GameObject N2 = NextChestPnt;
        RNG = Random.Range(1, 4);
        switch (RNG)
        {
            case 1:
                N1 = GameObject.Find("ChestPos2");
                N2 = GameObject.Find("ChestPos3");
                Chest.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case 2:
                N1 = GameObject.Find("ChestPos8");
                N2 = GameObject.Find("ChestPos7");
                Chest.transform.localRotation = Quaternion.Euler(0, 0, -90);
                break;
            case 3:
                N1 = GameObject.Find("ChestPos3");
                N2 = GameObject.Find("ChestPos7");
                break;
            case 4:
                N1 = GameObject.Find("ChestPos7");
                N2 = GameObject.Find("ChestPos3");
                break;
        }
        if (N1 != null && N2 != null)
        {
            LastChestPnt = N1;
            NextChestPnt = N2;
        }
        GameObject ChestAtk = Instantiate(ChestAtkObB, Chest.transform);
        ChestAtk.transform.parent = Chest.transform;
        ChestChestAtkTimer = ChestAtkCoolDwnB;
    }
    public void ChestChestAtkC()//Swords
    {
        ChestJob = 3;
        ChestChestAtkTimer = ChestAtkCoolDwnC;
    }
    public void ChestChestAtkD()//Embers
    {
        ChestJob = 4;
        ChestChestAtkTimer = ChestAtkCoolDwnD;
    }

}
