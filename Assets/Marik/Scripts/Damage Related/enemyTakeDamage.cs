using Unity.VisualScripting;
using UnityEngine;

public class enemyTakeDamage : MonoBehaviour
{
    [SerializeField] float myHealth;
    [SerializeField] float defence;
    [SerializeField] float invinciblityTime;
    [SerializeField] bool lockDamage;

    [SerializeField] bool instantiateOnDestroy;
    [SerializeField] GameObject prefab;

    private float timeToVincibility = 0;
    float damage = -1;

    private GameObject player;

    void Start()
    {
        FindPlayer();
    }

    void FixedUpdate(){
        if (timeToVincibility > 0) {timeToVincibility--;}
    }

    public void TakeDamage(float DMG)
    {
        if (timeToVincibility <= 0)
        {
            if (lockDamage) {myHealth --;}
            else{myHealth -= DMG/defence;}
            
            timeToVincibility = invinciblityTime;

            if (myHealth <= 0)
            {
                if (instantiateOnDestroy)
                {
                    Instantiate(prefab);
                }
                Destroy(gameObject);
            }
        }
    }

    void OnTrigger(Collider2D collider)
    {
        if (collider.gameObject.tag == "PBullet")
        {
            if (damage == -1)
            {
                P_Bullet_M bulletMDamage = collider.gameObject.GetComponent<P_Bullet_M>();
                BulletsKybrick bulletKDamage = collider.gameObject.GetComponent<BulletsKybrick>();
                Bullet Bullet = collider.gameObject.GetComponent<Bullet>();

                if (bulletMDamage != null) {damage = bulletMDamage.damage;}
                //else if (bulletKDamage != null) {damage = PlayerInputKybrick.Atk;}
                //else if (Bullet != null) {damage = PlayerInput.Atk;}
            }
            TakeDamage(damage);
        }
    }

    private void FindPlayer()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        int len = players.Length;
        if (len > 1)
        {
            float closestDistance = -100;
            for(int i = 0; i < len; i++)
            {
                GameObject obj = players[i];
                float dist = Vector3.Distance(obj.transform.position, transform.position);
                if (dist < closestDistance || closestDistance == -100)
                {
                    closestDistance = dist;
                    player = obj;
                }
            }
        }
        else if (len == 1)
        {
            player = players[0];
        }
        else
        {
        }
    }
}
