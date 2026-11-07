using System.Linq;
using System.Security;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lancer_Boss : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] GameObject lesserCannon;
    [SerializeField] GameObject mainShieldPrefab;
    [SerializeField] GameObject rotationShieldPrefab;
    [SerializeField] Rigidbody2D rigBod;

    [Header("Heavy Attacks")]

    [Header("Medium Attacks")]

    [Header("Light Attacks")]

    [Header("Attack Orders")]
    [SerializeField] private string[] phase1AttackOrder;
    [SerializeField] private string[] phase2AttackOrder;

    //Player
    private GameObject healthLogic;
    private Health health;
    private GameObject player;

    //Shields
    private GameObject mainShield;
    private GameObject rotatingShield;
    private Vector3 rotatingShieldScale;

    //Attack related
    private bool isAttacking = false;
    private string currAttack;
    private string currAttackType;
    private int currAttackTypeSelect = 0;
    private int attackTimeKeeper;

    //Player related
    private Vector3 psudoPlayerPos;
    private PlayerInput playerScript;
    private float playerSpeed;

    //Speeds
    private float xSpeed;
    private float ySpeed = 5;

    void Start()
    {
        FindPlayer();
        SetUpPlayerLogic();
        ConstructSheildingArrays();
        
        playerScript = player.GetComponent<PlayerInput>();
        xSpeed = 10f;//= playerScript.speed;
    }

    void FixedUpdate()
    {
        psudoPlayerPos = player.transform.position + new Vector3(15, 5, 0);
        ControlSheildingArrays();

        Move();



        if (isAttacking)
        {
            attackTimeKeeper++;
        }
        else
        {
            MoveLesserCannon();

            if (UnityEngine.Random.Range(1, 100) == 1)
            {
                attackTimeKeeper = 0;
                SelectAttack();
                Debug.Log("Lancer prepairing an attack");
            }
        }
    }


    private void ConstructSheildingArrays()
    {
        mainShield = Instantiate(mainShieldPrefab);
        rotatingShield = Instantiate(rotationShieldPrefab);

        rotatingShieldScale = rotatingShield.transform.localScale;
    }
    private void ControlSheildingArrays()
    {
        if (mainShield != null)
        {
            mainShield.transform.position = transform.position;
        }
        else 
        {
            if (rotatingShield.transform.localScale.x > rotatingShieldScale.x - .2)
            {
                rotatingShield.transform.localScale = rotatingShield.transform.localScale - new Vector3(.01f, .01f, 0);
            }
            rotatingShield.transform.eulerAngles -= new Vector3( 0, 0, .5f);
        }

        rotatingShield.transform.position = transform.position;

        rotatingShield.transform.eulerAngles += new Vector3( 0, 0, 1);
    }

    private void SelectAttack()
    {return;
        //if (phase == 1){} 
        switch (phase1AttackOrder[currAttackTypeSelect])
        {
            case "Light":
            currAttackType = "Light";
                switch(UnityEngine.Random.Range(1, 4))
                {
                    case 1:
                        currAttack = "Rapid Direct Fire";
                    break;
                    case 2:
                        currAttack = "Rapid Round Fire";
                    break;
                    case 3:
                        currAttack = "Rapid Spread Fire";
                    break;
                }
            break;
            case "Medium":
            currAttackType = "Medium";
                switch (UnityEngine.Random.Range(1, 3))
                {
                    case 1:
                        currAttack = "Laser Barrage";
                    break;
                    case 2:
                        currAttack = "Rail Gun Burst";
                    break;
                }
            break;
            case "Heavy":
            currAttackType = "Heavy";
                switch(UnityEngine.Random.Range(1, 3))
                {
                    case 1:
                    case 2:
                    case 3:
                        currAttack = "Lancer Main Cannon";
                    break;
                    case 4:
                        currAttack = "Meltdown";
                    break;
                }
            break;
        }
    }
    private void ResetAfterAttack()
    {
        currAttack = null;
        currAttackType = null;
        isAttacking = false;

        currAttackTypeSelect ++;
        if (currAttackTypeSelect >= phase1AttackOrder.Length)
        {
            currAttackTypeSelect = 0;
        }
    }
    private void MoveLesserCannon()
    {
        float lesserCannonSpeed = 0;
        if (currAttack == null)
        {
            lesserCannonSpeed = 1; if (lesserCannon.transform.eulerAngles.z < 180) {lesserCannonSpeed *= -1;}
            if (math.abs(lesserCannon.transform.eulerAngles.z) < math.abs(lesserCannonSpeed + .5)) {lesserCannon.transform.eulerAngles = new Vector3 (0,0,0); return;}
            lesserCannon.transform.eulerAngles = lesserCannon.transform.eulerAngles + new Vector3(0, 0, lesserCannonSpeed) ;
        }
    }

    private void Move()
    {
        float tempXSpeed;
        float tempYSpeed; 
        if (currAttack == null || currAttackType == "Light") 
        {
            tempXSpeed = xSpeed/2.0f;
            tempYSpeed = ySpeed/2.0f;
            if (math.abs(psudoPlayerPos.x - transform.position.x) > 5) {tempXSpeed = xSpeed;}
            if (math.abs(psudoPlayerPos.x - transform.position.x) > 1)
            {
                if (psudoPlayerPos.x > transform.position.x)
                {
                    rigBod.linearVelocityX = tempXSpeed;
                }
                else
                {
                    rigBod.linearVelocityX = -tempXSpeed;
                }
            }
            else
            {
                rigBod.linearVelocityX = 0;
            }

            if (math.abs(psudoPlayerPos.y - transform.position.y) > 5) {tempYSpeed = ySpeed;}
            if (math.abs(psudoPlayerPos.y - transform.position.y) > 1)
            {
                if (psudoPlayerPos.y > transform.position.y)
                {
                    rigBod.linearVelocityY = tempYSpeed;
                }
                else
                {
                    rigBod.linearVelocityY = -tempYSpeed;
                }
            }
            else
            {
                rigBod.linearVelocityY = 0;
            }
        }
    
    }

    private void OnDestroy()
    {
        Destroy(rotatingShield);
    }
    private void FindPlayer()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        int len = players.Length;
        if (len > 1)
        {
            float closestDistance = -100;
            for(int i = 0; i < len; i++)
            {
                GameObject obj = players[i];
                float dist = Vector3.Distance(obj.transform.position, transform.position);
                if (dist < closestDistance || closestDistance == -100)
                {
                    closestDistance = dist;
                    player = obj;
                }
            }
        }
        else if (len == 1)
        {
            player = players[0];
        }
        else
        {
            Debug.Log("No player found");
        }
    }
    private void SetUpPlayerLogic() {
        healthLogic = GameObject.Find("HealthLogic");
        health = healthLogic.GetComponent<Health>();
    }
}
