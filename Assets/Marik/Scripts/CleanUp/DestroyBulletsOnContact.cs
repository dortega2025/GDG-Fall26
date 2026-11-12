using UnityEngine;

public class DestroyBulletsOnContact : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "PBullet" || tag == "EBullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
