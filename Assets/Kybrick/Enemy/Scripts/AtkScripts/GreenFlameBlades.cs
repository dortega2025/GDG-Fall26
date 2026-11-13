using Unity.VisualScripting;
using UnityEngine;

public class GreenFlameBlades : MonoBehaviour
{
    [SerializeField] private float Life;
    [SerializeField] private float ActivateTime;
    [SerializeField] private float Speed;
    [SerializeReference] private Rigidbody2D RB;
    [SerializeReference] private Health health;
    [SerializeReference] private GameObject HealthLogic;
    [SerializeReference] private GameObject Player;
    [SerializeReference] private PlayerInput PlayerI;

    private float timer;
    private Vector3 DIR;
    private bool Activatable = true;
    void Start()
    {
        Player = GameObject.Find("Player ");
        HealthLogic = GameObject.Find("HealthLogic");
        health = HealthLogic.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= Life)
        {
            Destroy(gameObject);
        }
        if(timer >= ActivateTime&&Activatable)
        {
             DIR = Player.transform.position - transform.position;
            float angle = Mathf.Atan2(DIR.y, DIR.x) * Mathf.Rad2Deg+90;
            angle = Mathf.Repeat(angle, 360);
            angle = angle - transform.rotation.z;
            Activatable = false;
        }
        else if(Activatable)
        {
            Vector3 DIR = Player.transform.position - transform.position;
            float angle = Mathf.Atan2(DIR.y, DIR.x) * Mathf.Rad2Deg;
            angle = Mathf.Repeat(angle, 360);
            angle = angle - transform.rotation.z;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
        }
        if (!Activatable)
        {
            RB.AddForce(DIR.normalized * Speed * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == Player)
        {
            Debug.Log("Heya You got hit by blades");
            health.TakeDamage(5);
        }
    }

}
