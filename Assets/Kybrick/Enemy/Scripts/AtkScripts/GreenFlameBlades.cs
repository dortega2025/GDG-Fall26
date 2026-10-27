using Unity.VisualScripting;
using UnityEngine;

public class GreenFlameBlades : MonoBehaviour
{
    [SerializeField] private float Life;
    [SerializeField] private float ActivateTime;
    [SerializeField] private float Speed;
    [SerializeReference] private Rigidbody2D RB;
    private GameObject Player;
    private float timer;
    private Vector3 DIR;
    private bool Activatable = true;
    void Start()
    {
        Player = GameObject.Find("Player ");
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
}
