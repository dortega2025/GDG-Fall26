using Unity.VisualScripting;
using UnityEngine;

public class enemyTakeDamage : MonoBehaviour
{
    [SerializeField] float myHealth;
    [SerializeField] float defence;
    [SerializeField] float invinciblityTime;
    [SerializeField] bool lockDamage;
    [SerializeField] bool dontDestoryOn0Hp;

    [SerializeField] bool activateFunction; //Must be attached to same obj as this script
    [SerializeField] string functionName; //Must not need an argument

    [SerializeField] bool instantiateOnDestroy;
    [SerializeField] GameObject prefab;

    [Header("Sounds")]
    [SerializeField] GameObject deathToon;
    [SerializeField] GameObject OnHitSound;
    [SerializeField] GameObject OnInvincibilityHitSound;

    private float timeToVincibility = 0;
    float damage = -1;
    float defenceShred = 1;
    float defenceIgnore = 0;

    private GameObject player;

    void Start()
    {
        FindPlayer();
    }

    void FixedUpdate(){
        if (timeToVincibility > 0) {timeToVincibility--;}
    }

    public void TakeDamage(float DMG, float DShred, float DIgnore)
    {
        if (timeToVincibility <= 0)
        {
            if (lockDamage) {myHealth --;}
            else{myHealth -= DMG/((defence - DIgnore)/DShred);}

            if (OnHitSound != null)
            {
                Instantiate(OnHitSound);
            }

            
            timeToVincibility = invinciblityTime;

            if (myHealth <= 0)
            {
                if (instantiateOnDestroy)
                {
                    Instantiate(prefab, transform.position, transform.rotation);
                }

                if (activateFunction)
                {
                    gameObject.SendMessage(functionName);
                }

                if (!dontDestoryOn0Hp)
                {
                    Destroy(gameObject);
                }

                if (deathToon != null)
                {
                    Instantiate(deathToon);
                }
            }
        }
        else
        {
            if (OnInvincibilityHitSound != null)
            {
                Instantiate(OnInvincibilityHitSound);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "PBullet")
        {
            DoDamagePrep(collider.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "PBullet")
        {
            DoDamagePrep(collider.gameObject);
        }
    }

    void DoDamagePrep(GameObject damageSource)
    {
        if (damage == -1)
            {
            P_Bullet_M bulletMDamage = damageSource.GetComponent<P_Bullet_M>();
            BulletsKybrick bulletKDamage = damageSource.gameObject.GetComponent<BulletsKybrick>();
            Bullet Bullet = damageSource.gameObject.GetComponent<Bullet>();

            if (bulletMDamage != null) {damage = bulletMDamage.damage; defenceShred = bulletMDamage.defenceShred; defenceIgnore = bulletMDamage.defenceIgnore;} 
            //else if (bulletKDamage != null) {damage = PlayerInputKybrick.Atk;}
            else if (Bullet != null) {damage = Bullet.damage;}
            Debug.Log(damage);
        }
        TakeDamage(damage, defenceShred, defenceIgnore);
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
