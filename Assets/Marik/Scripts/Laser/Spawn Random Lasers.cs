using Unity.VisualScripting;
using UnityEngine;

public class SpawnRandomLasers : MonoBehaviour
{
    [SerializeField] private float Cooldown; 
    [SerializeField] private GetPosition positionGetter; 
    [SerializeField] private LaserRemoteControl laserControl; 

    [Header("Spawn Options")]
    [SerializeField] private bool lockToVertical; 
    [SerializeField] private bool lockToHorizontal; 
    [SerializeField] private bool spawnNearVertical; 

    private float currCooldown;    
    
    void FixedUpdate()
    {
        if (currCooldown <= 0)
        {
            currCooldown = Cooldown;
            Vector3 spawnPos = positionGetter.FindLoctionWithDefault();
            laserControl.CreateLaser(spawnPos, null, lockToVertical, lockToHorizontal, true, false, spawnNearVertical);
        }
        else
        {
            currCooldown --;
        }
    }
}
