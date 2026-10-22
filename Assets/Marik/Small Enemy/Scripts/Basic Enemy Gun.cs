using UnityEngine;

public class BasicEnemyGun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Fixed_Update()
    {
        
    }

    void RotateArm()
    {/*
        float playerPosX = player.current.position.x.ReadValue();
        float playerPosY = player.current.position.y.ReadValue();
        Vector3 playerPos = new Vector3(playerPosX, playerPosY, 0f);
        playerPos = camera.ScreenToWorldPoint(playerPos);
        Vector3 direction = playerPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        angle = Mathf.Repeat(angle, 360);
        angle -= transform.rotation.z;
        transform.eulerAngles = new Vector3(0, 0, angle);*/
    }
}
