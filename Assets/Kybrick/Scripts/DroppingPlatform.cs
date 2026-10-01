using UnityEngine;

public class DroppingPlatform : MonoBehaviour
{
    public float Lifespan;
    public float Life;
    private bool PlayerContact;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           PlayerContact = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerContact = false;
        }
    }

    void Update()
    {
        if (PlayerContact)
        {
            Life += 1 * Time.deltaTime;
            if(Life >= Lifespan)
            {
                Destroy(gameObject);
            }
        }
        else if(Life - 1*Time.deltaTime>=0)
        {
            Life -= 1 * Time.deltaTime;
        }
    }

    
}
