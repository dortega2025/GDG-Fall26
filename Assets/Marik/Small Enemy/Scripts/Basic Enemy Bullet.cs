using UnityEngine;

public class BasicEnemyBullet : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rigBod;

    public GameObject player;

    public float lifeSpan;
    public float velocity;
    public float gravitationalEffect;
    public float homingPower;
    public bool destroyedByObstacles;
    public bool friendlyFire;
    public bool collidesWithFriendly;
    public bool collidesWithFriendlyBullets;
    public bool collidesWithPlayerBullets;

    private Vector2 ogVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ogVelocity = rigBod.linearVelocity;

        rigBod.gravityScale = gravitationalEffect;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Homeing();

        CollisionsCheck();

        if (lifeSpan > 0) {lifeSpan--;} else {Destroy(gameObject);}

    }

    void OnDestroy()
    {
        //TODO explotion
    }

    private void CollisionsCheck()
    {
        
    }

    private void Homeing()
    {
        if (player.transform.position.x > transform.position.x)
        {
            rigBod.linearVelocity = new Vector2(ogVelocity.x + homingPower, ogVelocity.y);
        }
        else
        {
            rigBod.linearVelocity = new Vector2(ogVelocity.x - homingPower, ogVelocity.y);
        }

        if (player.transform.position.y > transform.position.y)
        {
            rigBod.linearVelocity = new Vector2(ogVelocity.x, ogVelocity.y + homingPower);
        }
        else
        {
            rigBod.linearVelocity = new Vector2(ogVelocity.x, ogVelocity.y - homingPower);
        }
    }

}