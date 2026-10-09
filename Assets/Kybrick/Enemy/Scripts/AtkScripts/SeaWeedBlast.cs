using Unity.VisualScripting;
using UnityEngine;

public class SeaWeedBlast : MonoBehaviour
{
    [SerializeField] private float MinSizeY;
    [SerializeField] private float ScaleRateY;
    [SerializeField] private int Dmg;
    [SerializeReference] private GameObject Center;
    [SerializeField] private float TimeToBlast;
    [SerializeReference] private Sprite Beam;
    private SpriteRenderer Render;
    [SerializeReference] private BoxCollider2D Collider;
    Player player;
    float x;
    float y;
    bool MaxY;



    void Start()
    {
        Render = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GameObject found = GameObject.Find("GeneratorOfBeam");
        y = transform.localScale.y;
        x = transform.localScale.x;
        Center.transform.position = found.transform.position;
        Center.transform.parent = found.transform;
        Collider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(Dmg);
        }
    }
    void Update()
    {
        TimeToBlast -= Time.deltaTime;
        if (TimeToBlast < 0)
        {
            Collider.enabled = true;
            Render.sprite = Beam;

            if (transform.localScale.y >= MinSizeY)
            {
                y -= ScaleRateY * Time.deltaTime;
            }
            else
            {
                MaxY = true;
            }
            transform.localScale = new Vector3(x, y, 0);

            if (MaxY)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
