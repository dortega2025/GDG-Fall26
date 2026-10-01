using Unity.VisualScripting;
using UnityEngine;

public class StationaryEnemyAI : MonoBehaviour
{
    [SerializeField] private float Invul;// amount of frames they are invulnerable after recieving damage
    [SerializeField] private float Hp;//How many times they can be hit 
    [SerializeField] private float Dmg; // how much damage it will do on contact 
    [SerializeField] private float AtkSpd;// how quickly it can attack
    [SerializeField] private float AggroRange;// how close the player has to be for them to aggro
    [SerializeField] private float AtkRange;//how close the player has to be before it attacks
    public float InvulTimer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PBullet") && (InvulTimer == 0)) //check if invulnerable
        { 
            InvulTimer = Invul;//Set timer
            Hp -= 1;//reduce hp, need to change this out to find away to get the damage value from the bullet itself
            Destroy(collision.gameObject);// destroy bullet to stop it from gunking anything up
            if (Hp <= 0)//Check here if dead, so we dont wait until no longer invul
            {
                Destroy(gameObject);//destroy this homeboy!
            }
        }else if (collision.gameObject.CompareTag("PBullet"))
        {
            Destroy(collision.gameObject);//destroys bullet regardless
        }
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Bullet"))
    //    {

    //    }
    //}
   void Update()
    {   
        if (InvulTimer - 1 * Time.deltaTime >= 0)//if InvulTimer is over 0
        {
            InvulTimer -= 1 * Time.deltaTime;//reduce it

        }
        else// if Time.deltaTime would be too much to put it directly at 0
        {
            InvulTimer = 0;//manually set it to 0 to avoid infinte InvulTime
        }
    }

  }
