using UnityEngine;

public class GreenFlameNode : MonoBehaviour
{
    [SerializeReference] private GameObject Gen00;
    [SerializeReference] private GameObject Gen;
    [SerializeReference] private Rigidbody2D RB;
    [SerializeField] private float speed;
    [SerializeField] private float LifeSpan;
    [SerializeField] private float GravCheck;

    public int RandomMax;
    private int RNG;
    bool on = true;
    private Vector3 Dir;
    void Start()
    {
        RNG = Random.Range(-RandomMax, RandomMax);
        speed += RNG;
    }
    void Update()
    {

        LifeSpan -= Time.deltaTime;
        if (LifeSpan < GravCheck)
        {
            RB.gravityScale = 0;
            RB.angularVelocity = 0;
            RB.linearVelocity = new Vector2(0,0);
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        if (LifeSpan < 0)
        {
            Destroy(gameObject);
        }
    }
}
