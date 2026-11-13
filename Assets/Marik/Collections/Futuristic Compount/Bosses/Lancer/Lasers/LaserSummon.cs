using Unity.VisualScripting;
using UnityEngine;

public class LaserSummon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprRenderer; 
    [SerializeField] private Sprite damageingLaser; 
    [SerializeField] private Collider2D hitBox; 

    [SerializeField] public LaserRemoteControl controlScript; 
    [SerializeField] private float timeToWarning; 
    [SerializeField] public float timeToTrigger; 
    [SerializeField] private float timeToKeepFiring; 

    [SerializeField] private AudioSource sound; 

    [SerializeField] private float damage; 
    private bool firing = false;

    private GameObject healthLogic;
    private Health health;
    private GameObject player;

    private bool fireFirstTrigger = true;

    void Start()
    {
        FindPlayer();

        healthLogic = GameObject.Find("HealthLogic");
        health = healthLogic.GetComponent<Health>();
    }

    void FixedUpdate()
    {
        //If timeToTrigger = -5, relies on control
        if (timeToTrigger > -5)
        {
            if (timeToWarning > 0)
            {
                timeToWarning--;
            }
            else if (timeToWarning > -3)
            {
                timeToWarning = -5;
                timeToWarning = -1;
                sprRenderer.color = Color.red;
            }

            if (timeToTrigger > 0)
            {
                timeToTrigger--;
            }
            else if (timeToTrigger > -3)
            {
                timeToTrigger = -5;
                sprRenderer.color = Color.cyan;
                firing = true;
                sprRenderer.sprite = damageingLaser;
            }
        }
        else
        {
            //if (controlScript == null) {return;}
            if (controlScript.timeToWarning < 5 && controlScript.timeToWarning > -5)
            {
                sprRenderer.color = Color.red;
            }

            if (controlScript.timeToTrigger < 5 && controlScript.timeToTrigger > -5)
            {
                sprRenderer.color = Color.cyan;
                sprRenderer.sprite = damageingLaser;
                firing = true;
            }
        }

        if (firing)
        {
            if (fireFirstTrigger)
            {
                fireFirstTrigger = false;
                sound.Play();
            }

            timeToKeepFiring--;
            if (timeToKeepFiring < 0)
            {
                Destroy(gameObject);
            }
            
        }
    }

    void OnTriggerStay2D(Collider2D colider)
    {
        if (firing)
        {

            if (colider.gameObject.tag == "Player")
            {
                health.TakeDamage(damage);
                Destroy(hitBox);
            }
        }
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


}

