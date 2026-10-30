using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyBullet : MonoBehaviour
{

    [SerializeField] public Rigidbody2D rigBod;

    public GameObject player;
    public GameObject parent;

    public Small_Enemy_Basic_AI.HomeingTypes homeingType;

    public float lifeSpan;
    public float velocity;
    public float gravitationalEffect;
    public float homingPower;
    public float damage;

    public bool explodesOnDestruction;
    public bool explosionDamageFallsOff;
    public float explosionForce;
    public float explosionDamage;
    public float explosionRange; 

    public bool destroyedByObstacles;
    public bool friendlyFire;
    public bool collidesWithFriendly;
    public bool collidesWithFriendlyBullets;
    public bool collidesWithPlayerBullets;

    private GameObject healthLogic;
    private Health health;

    private Vector2 ogVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ogVelocity = rigBod.linearVelocity;

        rigBod.gravityScale = gravitationalEffect;

        healthLogic = GameObject.Find("HealthLogic");
        health = healthLogic.GetComponent<Health>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Homeing();

        if (lifeSpan > 0) {lifeSpan--;} else {Destroy(gameObject);}

    }

    void OnDestroy()
    {
        if (player == null)
        {
            return;
        }
        //Final blast
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (explodesOnDestruction && distanceFromPlayer <= explosionRange)
        {
            if (explosionDamageFallsOff)
            {
                explosionDamage /= distanceFromPlayer*(explosionDamage/explosionRange); 
                explosionForce /= distanceFromPlayer*(explosionForce/explosionRange); 
            }

            //player.TakeDamage(explosionDamage)

            //Applies force //TODO test
            Vector3 direction = new Vector3(player.transform.position.x, player.transform.position.y, 0f) - transform.position;
            direction.z = 0f;
            direction = direction.normalized;
            Debug.Log(direction);
            player.GetComponent<Rigidbody2D>().AddForce(direction * explosionForce, ForceMode2D.Impulse);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                health.TakeDamage(damage);
                Destroy(gameObject);
            break;

            case "EBullet":
                if (collidesWithFriendlyBullets) {Destroy(gameObject);}
            break;

            case "PBullet":
                if (collidesWithPlayerBullets) {Destroy(gameObject);}
            break;

            default:
                switch (LayerMask.LayerToName(collision.gameObject.layer))
                {
                    case "Enemies":
                        if (collision.gameObject == parent) {return;}
                        if (friendlyFire)
                        { 
                            collision.gameObject.GetComponent<Small_Enemy_Basic_AI>().TakeDamage(damage); // TakeDamage(damage);
                            Destroy(gameObject);
                        }
                        else if (collidesWithFriendly)
                        {
                            Destroy(gameObject);
                        }
                    break;

                    case "Ground":
                        if (destroyedByObstacles)
                        {
                            Destroy(gameObject);
                        }
                    break;
                }
            break;
        }
    }

    private void Homeing()
    {
        if (player.transform.position.x > transform.position.x)
        {
            if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Nudge) {rigBod.linearVelocityX += homingPower;}
            else if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Absolute) {rigBod.linearVelocity = new Vector2(ogVelocity.x + homingPower, ogVelocity.y);}
        }
        else
        {
            if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Nudge) {rigBod.linearVelocityX -= homingPower;}
            else if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Absolute) {rigBod.linearVelocity = new Vector2(ogVelocity.x - homingPower, ogVelocity.y);}
        }

        if (player.transform.position.y > transform.position.y)
        {
            if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Nudge) {rigBod.linearVelocityY += homingPower;}
            else if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Absolute) {rigBod.linearVelocity = new Vector2(ogVelocity.x, ogVelocity.y + homingPower);}
            else if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Turn) {rigBod.AddForceY(homingPower);}
        }
        else
        {
            if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Nudge) {rigBod.linearVelocityY -= homingPower;}
            else if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Absolute) {rigBod.linearVelocity = new Vector2(ogVelocity.x, ogVelocity.y - homingPower);}
            else if (homeingType == Small_Enemy_Basic_AI.HomeingTypes.Turn) {rigBod.AddForceY(-homingPower);}
        }

        // Rotates to match velocity 
        float angle = Mathf.Atan2(rigBod.linearVelocity.y, rigBod.linearVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle+90);
    }


}