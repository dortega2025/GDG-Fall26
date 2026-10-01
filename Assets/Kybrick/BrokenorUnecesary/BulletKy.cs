using UnityEngine;
using UnityEngine.InputSystem;

public class BulletKy : MonoBehaviour
{
    [SerializeField] private int Variation; //How much it is able to vary
    [SerializeField] private float Dmg;//Dmg dude 
    [SerializeField] private float Speed;//how fast it goes 
    [SerializeField] private float Amount; //how many are there
    [SerializeField] private float Lifespan; //how long they last
    [SerializeField] private float FallOff;//how much the speed is decreased by as they go 
    private Vector3 origin;
    private int RNG;//the random
    private float Life=0;
    private Rigidbody2D rb;
    void Start()
    {
        origin = transform.position;
        rb = GetComponent<Rigidbody2D>();
        RNG = Random.Range(0, Variation);
        Dmg += Dmg * RNG;
        RNG = Random.Range(0, Variation);
        Speed += Speed * RNG;
        RNG = Random.Range(-Variation, Variation);
        Amount += Amount * RNG;
        RNG = Random.Range(-Variation, Variation);
        Lifespan += Lifespan * RNG;
        RNG = Random.Range(-Variation, Variation);
        FallOff += FallOff * RNG;

        RNG = Random.Range(-Variation, Variation);
        float mousePosX = Mouse.current.position.x.ReadValue();
        float mousePosY = Mouse.current.position.y.ReadValue();
        mousePosX += RNG / 10;
        RNG = Random.Range(-Variation, Variation);
        mousePosY += RNG / 10;
        Vector3 mousePos = new Vector3(mousePosX, mousePosY, 0f);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 direction = mousePos - origin;
        rb.AddForce(direction.normalized * Speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        Life += 1*Time.deltaTime;
        if (Life >= Lifespan)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        
    }



}
