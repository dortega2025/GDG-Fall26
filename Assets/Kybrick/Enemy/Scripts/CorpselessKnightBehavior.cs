using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class CorpselessKnightBehavior : MonoBehaviour
{
    [Header("Head")]
    [Header("Mvmnt")]
    [SerializeField] bool HeadBusy;//if it is taking an action
    [SerializeField] int HeadJob;//what action its taking 0=move 1=Atk1 etc
    [SerializeField] bool HeadWaiting;//if it is not moving
    [SerializeField] float HeadWaitingTime;//how long it waits
    [SerializeField] float HeadWaitingMax;//how long it waits
    [SerializeField] float HeadDef;//How much damage it reduces from being hit
    [SerializeField] float HeadSpd;//how fast it moves
    [SerializeField] float HeadDistTol;
    [SerializeField] GameObject Head;
    public GameObject Player;
    [SerializeField] GameObject CurrentPnt;// where they are at
    [SerializeField] GameObject NextPnt;// where they will go next
    [SerializeField] GameObject LastPnt;// where they were last
    float HeadDir=0;
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
    [SerializeField] private GameObject AtkGen00;
    [SerializeField] private int ProbabilityD;
    [SerializeField] private float AtkCoolDwnD;
    [Header("Chest")]
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
    float ChestDir=0;
    [Header("Atks")]
     [SerializeField]private float ChestChestAtkTimer;//how often it can take an action
    [SerializeField]private float ChestExtraTimer;//how often it can take an action
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
    [SerializeReference] private GameObject ChestAtkGen00 ;
    [SerializeField] private int ChestProbD;
    [SerializeField] private float ChestAtkCoolDwnD;


    private float count;
    private int RNG;
    public void Start()
    {
        ChestWaitingTime = ChestWaitingMax;
        HeadWaitingTime = HeadWaitingMax;
        HeadAtkTimer = 25;
        ChestChestAtkTimer = 25;
        Player = GameObject.Find("Player ");

    }
    public void Update()
    {
        Timers();
        if (!HeadWaiting&&!HeadBusy)
        {
            HeadBusy = true;
            HeadJob = 0;
            HeadMvmntChoice();
        }
        if (!HeadWaiting && HeadBusy && HeadJob == 0)
        {
            Head.transform.position = Vector2.MoveTowards(Head.transform.position, NextPnt.transform.position, HeadSpd *Time.deltaTime);
            if (DistCheck(Head, NextPnt, HeadDistTol))
            {
                HeadWaiting = true;
                HeadBusy = false;
                HeadWaitingTime = HeadWaitingMax;
                CurrentPnt = NextPnt;
            }
        }

        if (HeadBusy && HeadJob == 1)
        {
            if(DistCheck(Head, NextPnt, 0.1F))
            {
                HeadMvmntChoice();
                GameObject ATK = Instantiate(AtkObA);
                ATK.transform.position = Head.transform.position;
                count += 1;
            }
            else
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position, NextPnt.transform.position, (HeadSpd) * Time.deltaTime);
            }

            if (count >= 5)
            {
                HeadJob = 0;
                HeadBusy = false;
                count = 0;
            }
        }

        if (HeadBusy && HeadJob == 2)
        {
            if (HeadDir == 0)
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position, LastPnt.transform.position, (HeadSpd + HeadSpd / 2) * Time.deltaTime);
                if (DistCheck(Head, LastPnt, 1))
                {
                    HeadDir = 1;
                    GameObject Atk = Instantiate(AtkObB);
                    Atk.transform.localScale = new Vector3(0.44685F, 1, 1);
                    Atk.transform.rotation = AtkGenD.transform.rotation;
                    Atk.transform.position = AtkGenD.transform.position;
                    Atk.transform.parent = AtkGenD.transform;
                }
            }

            if (HeadDir == 1)
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position, NextPnt.transform.position, (HeadSpd - HeadSpd / 4) * Time.deltaTime);
                if (DistCheck(Head, NextPnt, 1))
                {
                    HeadDir = 2;
                }
                HeadExtraTimer += Time.deltaTime;
            }

            if (HeadDir == 2)
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position, LastPnt.transform.position, (HeadSpd - HeadSpd / 4) * Time.deltaTime);
                if (DistCheck(Head, LastPnt, 1))
                {
                    HeadDir = 1;
                }
                HeadExtraTimer += Time.deltaTime;
            }

            if (HeadExtraTimer > 10)
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
            if (DistCheck(Head, Player, 6))
            {

                float angle = Time.time * ((HeadSpd/5)*-1);
                var positionCenterObject = Player.transform.position;

                var x = positionCenterObject.x + Mathf.Cos(angle) * 6;
                var y = positionCenterObject.y + Mathf.Sin(angle) * 6;
                Head.transform.position = new Vector3(x, y); HeadExtraTimer -= Time.deltaTime;
                if(HeadExtraTimer < 0)
                {
                    GameObject ATK = Instantiate(AtkObC);
                    ATK.transform.position = Head.transform.position;
                    HeadExtraTimer = 0.25F;
                    count += 1;
                }
                if (count >= 4) 
                {
                    count = 0;
                    HeadBusy = false;
                    HeadJob = 0;
                    HeadExtraTimer = 0;
                    HeadWaitingTime = 0;
                }
              
            }
            else
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position, Player.transform.position, (HeadSpd) * Time.deltaTime);
            }
        }

        if (HeadBusy && HeadJob == 4)
        {
            GameObject Desti = GameObject.Find("HeadPos1");
            if (DistCheck(Head, Desti, 0.01F))
            {
                HeadExtraTimer -= Time.deltaTime;
                Head.transform.Rotate(Vector3.forward* HeadSpd*HeadSpd*2 * Time.deltaTime);
                if(HeadExtraTimer < 0)
                {
                    ShootNode(AtkGenD,Head,HeadExtraTimer,AtkGen00);
                    RNG = Random.Range(0, 4);
                    HeadExtraTimer = 0.75F + RNG / 2;
                    count += 1;
                    if (count >= 6)
                    {
                        HeadBusy = false;
                        HeadJob = 0;
                        HeadExtraTimer = 0;
                        count = 0;
                        Head.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }
                }
            }
            else
            {
                Head.transform.position = Vector2.MoveTowards(Head.transform.position, Desti.transform.position, (HeadSpd + HeadSpd / 2) * Time.deltaTime);
            }
        }
        if(!ChestWaiting&&!ChestBusy)
        {
            ChestBusy = true;
            ChestJob = 0;
            ChestMvmntChoice();
        }
        if (!ChestWaiting && ChestBusy && ChestJob == 0)
        {
            Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, NextChestPnt.transform.position, ChestSpd * Time.deltaTime);
            if (DistCheck(Chest, NextChestPnt, ChestDistTol))
            {
                ChestWaiting = true;
                ChestBusy = false;
                ChestWaitingTime = ChestWaitingMax;
                CurrentChestPnt = NextChestPnt;
            }
        }

        if (ChestBusy && ChestJob == 1)
        {
            if (DistCheck(Chest, NextChestPnt, 0.1F)) 
            {
                GameObject ATK = Instantiate(ChestAtkObA);
                ATK.transform.localScale = new Vector3(0.5F,1,1);
                ATK.transform.position = Chest.transform.position;
                count += 1;
            }
            else
            {
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, NextChestPnt.transform.position, ChestSpd*Time.deltaTime);
            }
            if(count >= 40)
            {
                ChestBusy = false;
                ChestJob = 0;
                count = 0;
                ChestWaitingTime = ChestAtkCoolDwnA;
            }
        }

        if (ChestBusy && ChestJob == 2)
        {
            Rigidbody2D RB = Chest.GetComponent<Rigidbody2D>();
            ChestExtraTimer += Time.deltaTime;
            if (ChestExtraTimer > 10)
            {
                ChestBusy = false;
                ChestJob = 0;
                ChestExtraTimer = 0;
                ChestWaitingTime = 0;
                Chest.transform.localRotation = Quaternion.Euler(0, 0, 0);
                RB.angularVelocity =0;
                RB.linearVelocity =new Vector2(0,0);
            }
            else
            {
                Vector3 DIR = Player.transform.position - Chest.transform.position;
                
                float angle = Mathf.Atan2(DIR.y, DIR.x) * Mathf.Rad2Deg-90;
                angle = Mathf.Repeat(angle, 360);
                angle = angle - transform.rotation.z;
                Chest.transform.rotation = Quaternion.Euler(0, 0, angle);
                RB.AddForce(DIR.normalized * Time.deltaTime * (ChestSpd + ChestSpd / 2), ForceMode2D.Impulse);
            }

        }

        if (ChestBusy && ChestJob == 3)
        {

            ChestExtraTimer -= Time.deltaTime;
            GameObject N1 = GameObject.Find("ChestPos3");
            GameObject N2 = GameObject.Find("ChestPos7");
            GameObject N3 = GameObject.Find("ChestPos8");
            GameObject N4 = GameObject.Find("ChestPos2");
            if (ChestDir == 0)
            {
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, N1.transform.position,(ChestSpd+ChestSpd/2)*Time.deltaTime);
                if (DistCheck(Chest, N1, 0.1F))
                {
                    ChestDir += 1;

                }
            }else if(ChestDir == 1)
            {
                if (ChestExtraTimer <= 0)
                {
                    GameObject ATK = Instantiate(AtkObC);
                    ATK.transform.position = Chest.transform.position;
                    ChestExtraTimer = 0.25F;
                }
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, N2.transform.position, (ChestSpd + ChestSpd / 2) * Time.deltaTime);
                if (DistCheck(Chest, N2, 0.1F))
                {
                    ChestDir += 1;
                }
            }
            else if (ChestDir == 2)
            {
                if (ChestExtraTimer <= 0)
                {
                    GameObject ATK = Instantiate(AtkObC);
                    ATK.transform.position = Chest.transform.position;
                    ChestExtraTimer = 0.175F;
                }
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, N3.transform.position, (ChestSpd + ChestSpd / 2) * Time.deltaTime);
                if (DistCheck(Chest, N3, 0.1F))
                {
                    ChestDir += 1;
                }
            }
            else if (ChestDir == 3)
            {
                if (ChestExtraTimer <= 0)
                {
                    GameObject ATK = Instantiate(AtkObC);
                    ATK.transform.position = Chest.transform.position;
                    ChestExtraTimer = 0.25F;
                }
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, N4.transform.position, (ChestSpd + ChestSpd / 2) * Time.deltaTime);
                if (DistCheck(Chest, N4, 0.1F))
                {
                    ChestDir += 1;
                }
            }
            else if(ChestDir == 4)
            {
                if (ChestExtraTimer <= 0)
                {
                    GameObject ATK = Instantiate(AtkObC);
                    ATK.transform.position = Chest.transform.position;
                    ChestExtraTimer = 0.25F;
                }
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, N1.transform.position, (ChestSpd + ChestSpd / 2) * Time.deltaTime);
                if (DistCheck(Chest, N1, 0.1F))
                {
                    ChestDir += 1;
                }
            }
            else
            {
                ChestBusy = false;
                ChestExtraTimer = 0;
                ChestJob = 0;
                ChestDir = 0;
            }
        }
        if (ChestBusy && ChestJob == 4)
        {
            GameObject Desti = GameObject.Find("ChestPos1");
            if (DistCheck(Chest, Desti, 0.01F))
            {
                ChestExtraTimer -= Time.deltaTime;
                Chest.transform.Rotate(Vector3.forward * ChestSpd * ChestSpd*ChestSpd  * Time.deltaTime);
                if (ChestExtraTimer < 0)
                {
                    ShootNode(ChestAtkGenD, Chest, ChestExtraTimer, ChestAtkGen00);
                    RNG = Random.Range(0, 4);
                    ChestExtraTimer = 0.35F + RNG / 2;
                    count += 1;
                    if (count >= 12)
                    {
                        ChestBusy = false;
                        ChestJob = 0;
                        ChestExtraTimer = 0;
                        count = 0;
                        Chest.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }
                }
            }
            else
            {
                Chest.transform.position = Vector2.MoveTowards(Chest.transform.position, Desti.transform.position, (ChestSpd + ChestSpd / 2) * Time.deltaTime);
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

        if (HeadWaiting && !HeadBusy)
        {
            HeadAtkTimer -= Time.deltaTime;
            if (HeadAtkTimer < 0)
            {
                HeadAtkTimer = 0;
                HeadAttackRoll();
            }
        }
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
    public void HeadMvmntChoice()
    {
        GameObject Node;
        RNG = Random.Range(1, 9);
        Node = GameObject.Find("HeadPos" + RNG);
        LastPnt = CurrentPnt;
        NextPnt = Node;
    }
    public void ChestAttackRoll()
    {
        RNG = Random.Range(1, 100);
        if (RNG >= ChestProbA && RNG < ChestProbB)
        {
            ChestJob = 1;
            ChestChestAtkA();
        }
        else if (RNG >= ChestProbB && RNG < ChestProbC)
        {
            ChestJob = 2;
            ChestChestAtkB();
        }
        else if (RNG >= ChestProbC && RNG < ChestProbD)
        { 
            ChestJob = 3;
            ChestChestAtkC();
        }
        else
        {
            ChestJob = 4;
            ChestChestAtkD();
        }

    }
    public void HeadAttackRoll()
    {
        RNG = Random.Range(1, 100);
        if (RNG >= ProbabilityA && RNG < ProbabilityB)
        {
            HeadJob = 2;
            HeadAtkA();
        }
        else if (RNG >= ProbabilityB && RNG < ProbabilityC)
        {
            HeadJob = 2;
            HeadAtkB();
        }
        else if (RNG >= ProbabilityC && RNG < ProbabilityD)
        {
            HeadJob = 3;
            HeadAtkC();
        }
        else
        {
            HeadJob = 4;
            HeadAtkD();
        }

    }
    public void HeadAtkA()//Chains
    {
        HeadJob = 1;
        NextPnt = GameObject.Find("HeadPos7");
        HeadBusy = true;
        HeadAtkTimer = AtkCoolDwnA;
    }
    public void HeadAtkB()//Blast
    {
        HeadJob = 2;
        RNG = Random.Range(1, 4);
        switch (RNG)
        {
            case 1:
                LastPnt = GameObject.Find("HeadPos2");
                NextPnt = GameObject.Find("HeadPos3");
                Head.transform.localRotation = Quaternion.Euler(0, 0, 90);
                break;
            case 2:
                LastPnt = GameObject.Find("HeadPos8");
                NextPnt = GameObject.Find("HeadPos7");
                Head.transform.localRotation = Quaternion.Euler(0, 0, -90);
                break;
            case 3:
                LastPnt = GameObject.Find("HeadPos2");
                NextPnt = GameObject.Find("HeadPos7");
                break;
            case 4:
                LastPnt = GameObject.Find("HeadPos7");
                NextPnt = GameObject.Find("HeadPos2");
                break;
        }
        
      
        HeadAtkTimer = AtkCoolDwnB;
        HeadBusy = true;
    }
    public void HeadAtkC()//Swords
    {
        HeadJob = 3;
        HeadBusy = true;
        HeadAtkTimer = AtkCoolDwnC;
    }
    public void HeadAtkD()//Embers
    {
        HeadJob = 4;
        HeadBusy = true;
        RNG = Random.Range(0, 4);
        HeadExtraTimer = 0.75F + RNG/2;
        HeadAtkTimer = AtkCoolDwnD;
    }
    
    public void ChestChestAtkA()//Chains
    {
        HeadBusy = true;
        ChestJob = 1;
        RNG = Random.Range(1, 4);
        if(RNG == 1||RNG==2)
        {
            NextChestPnt = GameObject.Find("ChestPos8");
        }
        else
        {
            NextChestPnt = GameObject.Find("ChestPos3");
        }       
        ChestBusy = true;
        ChestChestAtkTimer = ChestAtkCoolDwnA;
    }
    public void ChestChestAtkB()//Blast
    {
        ChestJob = 2;
        GameObject ChestAtk1 = Instantiate(ChestAtkObB,Chest.transform);
        GameObject ChestAtk2 = Instantiate(ChestAtkObB,Chest.transform);
        GameObject ChestAtk3 = Instantiate(ChestAtkObB, Chest.transform);

        ChestAtk1.transform.parent = Chest.transform;
        ChestAtk1.transform.localScale = new Vector3(0.675F, 1, 1);
        ChestAtk1.transform.localPosition = new Vector2(0, -2.35F);

        ChestAtk2.transform.parent = Chest.transform;
        ChestAtk2.transform.localScale = new Vector3(0.3264167F, 0.458002F, 1);
        ChestAtk2.transform.localRotation = Quaternion.Euler(0, 0, 270);
        ChestAtk2.transform.localPosition = new Vector2(-1.41F, 0.547F);

        ChestAtk3.transform.parent = Chest.transform;
        ChestAtk3.transform.localScale = new Vector3(0.2935825F, 0.458002F, 1);
        ChestAtk3.transform.localRotation = Quaternion.Euler(180, 0, 90);
        ChestAtk3.transform.localPosition = new Vector2(1.39F, 0.458002F);

        ChestBusy = true;
        ChestChestAtkTimer = ChestAtkCoolDwnB;
    }
    public void ChestChestAtkC()//Swords
    {
        ChestJob = 3;
        ChestBusy = true;
        ChestChestAtkTimer = ChestAtkCoolDwnC;
    }
    public void ChestChestAtkD()//Embers
    {
        ChestJob = 4;
        ChestBusy =true;
        RNG = Random.Range(0, 4);
        ChestExtraTimer = 0.75F + RNG / 2;
        ChestChestAtkTimer = ChestAtkCoolDwnD;
    }

    public void ShootNode(GameObject Gen,GameObject Body,float Timer,GameObject Gen00)
    {
        GameObject ATK;
        Vector3 direction = Gen00.transform.position - Gen.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 270;
        angle = Mathf.Repeat(angle, 360);
        angle = angle - transform.rotation.z;
        direction.z = 0;
        direction = direction.normalized;
        ATK = Instantiate(AtkObD, Body.transform.position, Quaternion.Euler(0, 0, angle));
        RNG = Random.Range(-2, 2);
        ATK.GetComponent<Rigidbody2D>().AddForce((direction * (RNG+HeadSpd)) * -1, ForceMode2D.Impulse);
    }
}