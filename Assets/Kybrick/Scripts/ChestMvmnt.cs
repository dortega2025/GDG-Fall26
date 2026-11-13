using Unity.VisualScripting;
using UnityEngine;

public class ChestMvmnt : MonoBehaviour
{
    public bool Chest;
    public CorpselessKnightBehavior Corpse;
    public GameObject Body;

    public void Start()
    {
        Body = GameObject.Find("CorpslessKnight");
        Corpse = Body.GetComponent<CorpselessKnightBehavior>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PBullet"))
        {
            Corpse.TakeDamage(Chest);
        }
    }
}
