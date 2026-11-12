using System.Linq;
using System.Security;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Lancer_Boss : MonoBehaviour
{
    [Header("Connections")]
    [SerializeField] GameObject lesserCannon;
    [SerializeField] GameObject mainShieldPrefab;
    [SerializeField] GameObject rotationShieldPrefab;
    [SerializeField] Rigidbody2D rigBod;
    

    [Header("Audio")]
    [SerializeField] GameObject LesserCannonGenFireSound;

    [SerializeField] GameObject alarmLoop;
    private GameObject spawnedAlarm;

    [SerializeField] GameObject phase2Alarm;
    [SerializeField] GameObject postMortomAlarm;

    [Header("Heavy Attacks")]

    [Header("Medium Attacks")]
    [SerializeField] private GameObject laserCallPrefab1;
    [SerializeField] private GameObject laserPrefab1;
    [SerializeField] private GameObject laserCallPrefab2;
    [SerializeField] private GameObject laserPrefab2;
    [SerializeField] private GameObject laserCallPrefab3;
    [SerializeField] private GameObject laserPrefab3;

    [Header("Light Attacks")]
    [SerializeField] private GameObject laserBolt;

    [Header("Other")]
    [SerializeField] private GameObject sunBeams;

    [Header("Attack Orders")]
    [SerializeField] private string[] phase1AttackOrder;
    [SerializeField] private string[] phase2AttackOrder;

    //
    int phase = 1;

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
    [SerializeField] private string currAttack;
    private string currAttackType = null;
    private string currSubType = null;
    private int currAttackTypeSelect = -1;
    private float lesserCannonSpeed = 0;
    private float minAttkBreak = 100; private float curAttkBreakTime = 200;
    //Track attack state
    private int attackTimeKeeper;
    private bool attackJustStart;
    //Attack helpers
    private GameObject helperObject1;

    //Player related
    private Vector3 psudoPlayerPos;
    private PlayerInput playerScript;
    private float playerSpeed;

    //Speeds
    private float xSpeed;
    private float ySpeed = 5;

    //Other
    private float spawnY;
    

    void Start()
    {
        FindPlayer();
        SetUpPlayerLogic();
        ConstructSheildingArrays();
        ResetAfterAttack();
        spawnY = transform.position.y;

        spawnedAlarm = Instantiate(alarmLoop);
        
        playerScript = player.GetComponent<PlayerInput>();
        xSpeed = 10f;//= playerScript.speed;
    }

    void FixedUpdate()
    {
        psudoPlayerPos = player.transform.position + new Vector3(15, 5, 0);
        ControlSheildingArrays();

        Move();

        RotateLesserCannon(); //Controls phase 2 lesser cannon attack too

        if (isAttacking)
        {
            attackTimeKeeper++;
            CommenceAttack();
        }
        else
        {
            curAttkBreakTime --;
            if (UnityEngine.Random.Range(1, 10) == 1 && curAttkBreakTime <= 0 && (lesserCannon.transform.eulerAngles == new Vector3 (0, 0, 0) || phase == 2 || curAttkBreakTime < -100))
            {
                attackTimeKeeper = 0;
                isAttacking = true;
                SelectAttack();
                Debug.Log("Lancer prepairing an attack");
            }
        }

        if (transform.position.y < spawnY - 500)
        {
            Destroy(gameObject);
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
                if (phase == 1) {PhaseShift();}
            }
            rotatingShield.transform.eulerAngles -= new Vector3( 0, 0, .5f);
        }

        rotatingShield.transform.position = transform.position;

        rotatingShield.transform.eulerAngles += new Vector3( 0, 0, 1);
    }


    private void SelectAttack()
    {
        switch (phase1AttackOrder[currAttackTypeSelect])
        {
            case "Light":
            currAttackType = "Light";
                switch(UnityEngine.Random.Range(1, 9))
                {
                    case 1:
                        currAttack = "Rapid Direct Fire";
                    break;
                    case >1 and < 6:
                        currAttack = "Rapid Round Fire";
                    break;
                    default:
                        currAttack = "Rapid Spread Fire";
                    break;
                }
            break;
            case "Medium":
            currAttackType = "Medium";
                switch (UnityEngine.Random.Range(1, 4))
                {
                    case 1:
                        currAttack = "Laser Quick Barrage";
                        currSubType = "STDLaser";
                    break;
                    case 2:
                        currAttack = "Relentless Laser Barrage";
                        currSubType = "STDLaser";
                    break;
                    case 3:
                        currAttack = "Burst Laser";
                        currSubType = "STDLaser";
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
    
    private void CommenceAttack()
    {
        switch (currAttack)
        {
            //Light
            case "Rapid Direct Fire": RapidDirectFire(); break;
            case "Rapid Round Fire": RapidRoundFire(); break;
            case "Rapid Spread Fire": RapidSpreadFire(); break;
            //Medium
            case "Laser Quick Barrage": LaserQuickBarrage(); break;
            case "Relentless Laser Barrage": RelentlessLasserBarrage(); break;
            case "Burst Laser": BurstLaser(); break;
            //Heavy
        }
    }
    //Light
    private void RapidDirectFire()
    {
        if (attackTimeKeeper > 50 && attackTimeKeeper % 15 == 5)
        {
            FireLesserCannon(laserBolt, 30);
        }
        if (attackTimeKeeper > 450)
        {
            ResetAfterAttack();
        }
    }
    private void RapidRoundFire()
    {
        if (attackTimeKeeper > 50 && attackTimeKeeper % 2 == 1)
        {
            FireLesserCannon(laserBolt, 25);
        }
        if (attackTimeKeeper > 500)
        {
            ResetAfterAttack();
        }
    }
    private void RapidSpreadFire()
    {
        if (attackTimeKeeper > 50 && attackTimeKeeper % 10 == 2)
        {
            FireLesserCannon(laserBolt, 25);
        }
        if (attackTimeKeeper > 500)
        {
            ResetAfterAttack();
        }
    }
    //Medium
    private void LaserQuickBarrage()
    {
        if (attackJustStart)
        {
            helperObject1 = Instantiate(laserCallPrefab1);
            attackJustStart = false;
            return;
        }

        if (attackTimeKeeper % 41 == 2)
        {
            helperObject1.GetComponent<LaserRemoteControl>().CreateLaser(Vector3.forward, laserPrefab1, false, false, true, false);
        }

        if (attackTimeKeeper > 500) {ResetAfterAttack();}
    }
    private void RelentlessLasserBarrage()
    {
        if (attackJustStart)
        {
            helperObject1 = Instantiate(laserCallPrefab2);
            attackJustStart = false;
            return;
        }

        if (attackTimeKeeper % 23 == 2)
        {
            helperObject1.GetComponent<LaserRemoteControl>().CreateLaser(Vector3.forward, laserPrefab2, false, false, true, false);
        }

        if (attackTimeKeeper > 500) {ResetAfterAttack();}
    }
    private void BurstLaser()
    {
        if (attackJustStart)
        {
            helperObject1 = Instantiate(laserCallPrefab3);
            attackJustStart = false;
            return;
        }

        if (attackTimeKeeper % 35 == 2)
        {
            if (helperObject1.GetComponent<LaserRemoteControl>().timeToTrigger < 35) {helperObject1 = Instantiate(laserCallPrefab3);}
            helperObject1.GetComponent<LaserRemoteControl>().CreateLaser(Vector3.forward, laserPrefab3, false, false, true, true, true);
        }

        if (attackTimeKeeper > 400) {ResetAfterAttack();}
    }
    //Heavy
    private void ResetAfterAttack()
    {
        currAttack = "";
        currAttackType = "";
        currSubType = null;
        isAttacking = false;
        lesserCannonSpeed = 0;
        curAttkBreakTime = minAttkBreak;

        attackJustStart = true;

        helperObject1 = null;

        currAttackTypeSelect ++;
        if (currAttackTypeSelect >= phase1AttackOrder.Length)
        {
            currAttackTypeSelect = 0;
        }
    }
    
    private void FireLesserCannon(GameObject projectile, float speed)
    {
        Instantiate(LesserCannonGenFireSound);
        GameObject newProjectile = Instantiate(projectile, lesserCannon.transform.position, lesserCannon.transform.rotation * quaternion.Euler(0, 0, 90));
        Vector3 r1 = lesserCannon.transform.rotation * Vector3.left;
        Vector2 r2 = new Vector2(r1.x, r1.y).normalized*speed;
        newProjectile.GetComponent<Rigidbody2D>().AddForce(r2, ForceMode2D.Impulse);
        
    }

    private void RotateLesserCannon()
    {
        Vector3 cannonPos = lesserCannon.transform.position;
        Vector3 playerPos = player.transform.position;
        Vector3 cannonAngle = lesserCannon.transform.eulerAngles;

        if (phase == 2)
        {
            if (lesserCannonSpeed < 100) {lesserCannonSpeed += .05f;}
            lesserCannon.transform.eulerAngles = lesserCannon.transform.eulerAngles + new Vector3(0, 0, lesserCannonSpeed);
            if (UnityEngine.Random.Range(0, 20) == 1)FireLesserCannon(sunBeams, 5);
            return;
        }

        if (currAttack == "" || currAttack == "Laser Quick Barrage")
        {
            lesserCannonSpeed = 2; if (lesserCannon.transform.eulerAngles.z < 180) {lesserCannonSpeed *= -1;}
            if (math.abs(lesserCannon.transform.eulerAngles.z) < math.abs(lesserCannonSpeed + .5)) {lesserCannon.transform.eulerAngles = new Vector3 (0,0,0); return;}
            if (lesserCannon.transform.eulerAngles.z != 0) {lesserCannon.transform.eulerAngles = lesserCannon.transform.eulerAngles + new Vector3(0, 0, lesserCannonSpeed);}
        }
        else if (currAttack == "Rapid Direct Fire")
        {
            lesserCannonSpeed = 1;
            Vector3 direction = playerPos - cannonPos;
            float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg + 180;
            lesserCannon.transform.eulerAngles = new Vector3(0, 0, angle);
        }
        else if (currAttack == "Rapid Round Fire")
        {
            lesserCannonSpeed += .1f;
            lesserCannon.transform.eulerAngles = lesserCannon.transform.eulerAngles + new Vector3(0, 0, lesserCannonSpeed);
        }
        else if (currAttack == "Rapid Spread Fire")
        {
            if (lesserCannonSpeed == 0) {lesserCannonSpeed = .05f;}
            if (cannonAngle.z > 90) {lesserCannonSpeed *= -1;}

            lesserCannon.transform.eulerAngles = lesserCannon.transform.eulerAngles + new Vector3(0, 0, lesserCannonSpeed);
        }
    }
    private void Move()
    {


        float tempXSpeed;
        float tempYSpeed; 
        if (currAttack == "" || currAttackType == "Light" || currAttackType == "Medium") 
        {
            tempXSpeed = xSpeed/2.0f;
            tempYSpeed = ySpeed/2.0f;
            if (currSubType == "STDLaser")
            {
                tempXSpeed = xSpeed+1;
                tempYSpeed = ySpeed+1;
            }
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

    private void PhaseShift()
    {
        phase++;
        phase1AttackOrder = phase2AttackOrder;
        ResetAfterAttack();
        Destroy(spawnedAlarm);
        spawnedAlarm = Instantiate(phase2Alarm);
    }

    public void NotBigSurprise()
    {
        rigBod.gravityScale = 10;
    }

    private void OnDestroy()
    {
        Destroy(spawnedAlarm);
        spawnedAlarm = Instantiate(postMortomAlarm);
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


