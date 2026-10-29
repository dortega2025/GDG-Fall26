using UnityEngine;
using UnityEngine.UIElements;

public class HeadMvmt : MonoBehaviour
{
    [Header("Mvmnt")]
    public bool HeadBusy;//if it is taking an action
    public int HeadJob;//what action its taking 0=move 1=Atk1 etc
    public bool HeadWaiting;//if it is not moving
    public float HeadWaitingTime;//how long it waits
    public float HeadWaitingMax;//how long it waits
    public float HeadDef;//How much damage it reduces from being hit
    public float HeadSpd;//how fast it moves
    public float HeadDistTol;
    public GameObject Head;
    public GameObject Player;
    public GameObject CurrentPnt;// where they are at
    public GameObject NextPnt;// where they will go next
    public GameObject LastPnt;// where they were last
    float HeadDir;
    [Header("Atks")]
    public float HeadAtkTimer;//how often it can take an action
    public float HeadExtraTimer;//how often it can take an action
    [SerializeField] private GameObject AtkObA;//Green Flame chains
    [SerializeField] private GameObject AtkGenA; // if there is a specific spawner 
    [SerializeField] private int ProbabilityA; // how likely it is to trigger
    [SerializeField] private float AtkCoolDwnA;// how long they must wait to attk again
    [SerializeField] private GameObject AtkObB;//GreenFlame blast
    [SerializeField] private GameObject AtkGenB;
    [SerializeField] private int ProbabilityB;
    [SerializeField] private float AtkCoolDwnB;
    [SerializeField] private GameObject AtkObC;//Green Flame blad
    [SerializeField] private GameObject AtkGenC;
    [SerializeField] private int ProbabilityC;
    [SerializeField] private float AtkCoolDwnC;
    [SerializeField] private GameObject AtkObD;//Green Flame node
    [SerializeField] private GameObject AtkGenD;
    [SerializeField] private int ProbabilityD;
    [SerializeField] private float AtkCoolDwnD;

    [Header("Mvmnt")]
    [SerializeField] private bool ChestBusy;//if it is taking an action
    [SerializeField] private int ChestJob;//what action its taking 0=move 1=ChestAtk1 etc
    [SerializeField] private bool ChestWaiting;//if it is not moving
    [SerializeField] private float ChestWaitingTime;//how long it waits
    [SerializeField] private float ChestWaitingMax;//how long it waits
    [SerializeField] private float ChestDef;//How much damage it reduces from being hit
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

    private int RNG;

    public void Update()
    {
        Timers();
        if (!HeadWaiting)
        {
            HeadBusy = true;
            HeadJob = 0;
            HeadMvmntChoice();
        }
        if (!HeadWaiting && HeadBusy && HeadJob==0)
        {
            Head.transform.position = Vector2.MoveTowards(CurrentPnt.transform.position, NextPnt.transform.position, HeadSpd);
            if (DistCheck(Head,Player,HeadDistTol))
            {
                HeadWaiting = true;
                HeadBusy = false;
                HeadWaitingTime = HeadWaitingMax;
                CurrentPnt = NextPnt;
            }
        }

        if(HeadBusy && HeadJob == 1)
        {
            if(HeadDir == 0)
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position, NextPnt.transform.position, HeadSpd + HeadSpd / 2);
                if (DistCheck(Head, NextPnt, 1))
                {
                    HeadDir = 1;
                    NextPnt = GameObject.Find("HeadPos2");
                }
            }
            if (HeadDir == 1)
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position, NextPnt.transform.position, HeadSpd + HeadSpd / 2);
                GameObject N1 = GameObject.Find("HeadPos4");
                GameObject N2 = GameObject.Find("HeadPos1");
                GameObject N3 = GameObject.Find("HeadPos6");
                GameObject ATK = Instantiate(AtkObA, Head.transform);
                Vector2 HeadDirection;
                if (Head.transform.position.x == N1.transform.position.x)
                {
                    HeadDirection = Head.transform.position - N1.transform.position;
                    ATK.GetComponent<Rigidbody2D>().AddForce(HeadDirection.normalized * (1f * HeadSpd/2), ForceMode2D.Impulse);
                }
                else if(Head.transform.position.x == N2.transform.position.x)
                {
                    HeadDirection = Head.transform.position - N2.transform.position;
                    ATK.GetComponent<Rigidbody2D>().AddForce(HeadDirection.normalized * (1f * HeadSpd/2), ForceMode2D.Impulse);
                }
                else if(Head.transform.position.x == N3.transform.position.x)
                {
                    HeadDirection = Head.transform.position - N3.transform.position;
                    ATK.GetComponent<Rigidbody2D>().AddForce(HeadDirection.normalized * (1f * HeadSpd/2), ForceMode2D.Impulse);
                }
                if(DistCheck(Head, NextPnt, 1))
                {
                    HeadBusy = false;
                    HeadJob = 0;
                    HeadDir = 0;
                }
            }
        }

        if(HeadBusy && HeadJob == 2)
        {
            if(HeadDir == 0)
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position,LastPnt.transform.position, HeadSpd + HeadSpd/2);
                if (DistCheck(Head,LastPnt,1))
                {
                    HeadDir = 1;
                }
            }

            if(HeadDir == 1)
            {
                Head.transform.position = Vector2.MoveTowards(LastPnt.transform.position, NextPnt.transform.position, HeadSpd + HeadSpd / 2);
                if (DistCheck(Head, NextPnt, 1))
                {
                    HeadDir = 2;
                }
                HeadExtraTimer += Time.deltaTime;
            }

            if (HeadDir == 2)
            {
                Head.transform.position = Vector2.MoveTowards(NextPnt.transform.position, LastPnt.transform.position, HeadSpd + HeadSpd / 2);
                if (DistCheck(Head, LastPnt, 1))
                {
                    HeadDir = 1;
                }
                HeadExtraTimer += Time.deltaTime;
            }
            
            if(HeadExtraTimer > 10)
            {
                HeadDir = 0;
                HeadBusy = false;
                HeadJob = 0;
                HeadExtraTimer = 0;
                Head.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

        }

        if (HeadBusy && HeadJob == 3) 
        {
            if (DistCheck(Head, Player, 8))
            {
                Head.transform.Rotate(Player.transform.position, -HeadSpd);
                HeadExtraTimer += Time.deltaTime;
                switch (HeadExtraTimer)
                {
                    case 1:
                        Instantiate(AtkObC, Head.transform);
                        break;
                    case 2:
                        Instantiate(AtkObC, Head.transform);
                        break;
                    case 3:
                        Instantiate(AtkObC, Head.transform);
                        break;
                    case 4:
                        Instantiate(AtkObC, Head.transform);
                        break;
                    case 5:
                        Instantiate(AtkObC, Head.transform);
                        break;
                    case 6:
                        HeadExtraTimer = 0;
                        HeadBusy = false;
                        HeadJob = 0;
                        break;
                }
            }
            else
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position, Player.transform.position, HeadSpd + HeadSpd / 2);
            }
        }

        if(HeadBusy && HeadJob == 4)
        {
            GameObject Desti = GameObject.Find("HeadPos6");
            float angle = Head.transform.localRotation.z;
            RNG = Random.Range(1,4);
            angle -= (HeadSpd / RNG) * Time.deltaTime;
            if (DistCheck(Head, Desti, 1))
            {
                HeadExtraTimer += Time.deltaTime;
                Head.transform.localRotation = Quaternion.Euler(0, 0, angle);
                if(HeadExtraTimer == 6) 
                {
                    HeadExtraTimer = 0;
                    HeadBusy = false;
                    HeadJob = 0;
                }else if(HeadExtraTimer == 1|| HeadExtraTimer == 2 || HeadExtraTimer == 3 || HeadExtraTimer == 4 || HeadExtraTimer == 5)
                {
                    GameObject ATK =Instantiate(AtkObD, Head.transform);
                    Vector2 HeadDirection = Head.transform.position - AtkGenD.transform.position;
                    ATK.GetComponent<Rigidbody2D>().AddForce(HeadDirection.normalized * (1f * HeadSpd), ForceMode2D.Impulse);
                }
            }
            else
            {
              Head.transform.position = Vector2.MoveTowards(Head.transform.position, Desti.transform.position, HeadSpd + HeadSpd / 2);
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
        if (HeadWaiting && !HeadBusy)
        {
            HeadWaitingTime -= Time.deltaTime;
            if (HeadWaitingTime < 0) 
            {
                HeadWaitingTime = 0;
                HeadWaiting = false;
            }
        }
        
        if (HeadWaiting&&!HeadBusy)
        {
            HeadAtkTimer -= Time.deltaTime;
            if(HeadAtkTimer < 0)
            {
                HeadAtkTimer = 0;
                HeadAttackRoll();
            }
        }

    }
    public void HeadMvmntChoice()
    {
        GameObject Node;
        RNG = Random.Range(1, 9);
        Node = GameObject.Find("HeadPos" + RNG);
        LastPnt = CurrentPnt;
        NextPnt = Node;
    }
    public void HeadAttackRoll()
    {
        RNG = Random.Range(1, 100);
        if(RNG>= ProbabilityA && RNG < ProbabilityB)
        {
            HeadAtkA();
        }
        else if (RNG >= ProbabilityB && RNG < ProbabilityC)
        {
            HeadAtkB();
        }
        else if (RNG >= ProbabilityC && RNG < ProbabilityD)
        {
            HeadAtkC();
        }
        else
        {
            HeadAtkD();
        }

    }
    public void HeadAtkA()//Chains
    {
        HeadJob = 1;
        NextPnt = GameObject.Find("HeadPos7");
        HeadAtkTimer = AtkCoolDwnA;
    }
    public void HeadAtkB()//Blast
    {
        HeadJob = 2;
        GameObject N1 = LastPnt;
        GameObject N2 = NextPnt;
        RNG = Random.Range(1, 4);
        switch (RNG)
        {
            case 1:
                N1 = GameObject.Find("HeadPos2");
                N2 = GameObject.Find("HeadPos3");
                Head.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case 2:
                N1 = GameObject.Find("HeadPos8");
                N2 = GameObject.Find("HeadPos7");
                Head.transform.localRotation = Quaternion.Euler(0, 0, -90);
                break;
            case 3:
                N1 = GameObject.Find("HeadPos3");
                N2 = GameObject.Find("HeadPos7");
                break;
            case 4:
                N1 = GameObject.Find("HeadPos7");
                N2 = GameObject.Find("HeadPos3");
                break;
        }
        if(N1 != null && N2 != null)
        {
            LastPnt = N1;
            NextPnt = N2;
        }
        GameObject Atk = Instantiate(AtkObB, Head.transform);
        Atk.transform.parent = Head.transform;
                HeadAtkTimer = AtkCoolDwnB;
    }
    public void HeadAtkC()//Swords
    {
        HeadJob = 3;
        HeadAtkTimer = AtkCoolDwnC;
    }
    public void HeadAtkD()//Embers
    {
        HeadJob = 4;
        HeadAtkTimer = AtkCoolDwnD;
    }

}
