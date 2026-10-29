using UnityEngine;

public class GreenFlameChain : MonoBehaviour
{
    [SerializeField] private float Life;
    [SerializeField] private float Speed;
    [SerializeField] private float RotSpeed;
    [SerializeField] private float ChaseDist;
    [SerializeField] private float ChaseMult;

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
        DIR = Player.transform.position - transform.position;
        if (Vector2.Distance(transform.position, Player.transform.position) >= ChaseDist)
        {
            RB.AddForce(DIR.normalized * Speed * ChaseMult * Time.deltaTime, ForceMode2D.Impulse);
            transform.Rotate(Vector3.forward, RotSpeed*ChaseMult);
        }
        else
        {
            RB.AddForce(DIR.normalized * Speed * Time.deltaTime, ForceMode2D.Impulse);
            transform.Rotate(Vector3.forward, RotSpeed);
        }
        
    }
}
