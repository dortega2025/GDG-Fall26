using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class BulletBehavior : MonoBehaviour
{

    [SerializeField] Rigidbody2D RigBod;
    private float desponeTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        desponeTimer = 0;
        RigBod.linearVelocity = new Vector2(10f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        desponeTimer += Time.deltaTime;
        if (desponeTimer >= 1)
        {
            Destroy(gameObject);
        }
    }
}
