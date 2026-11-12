using Unity.Mathematics;
using UnityEngine;

public class LaserRemoteControl : MonoBehaviour
{
    [SerializeField] public float timeToWarning; 
    [SerializeField] public float timeToTrigger; 
    [SerializeField] public GameObject laserToSpawn; 

    private GameObject player;

    private float timeToDestroy;


    void Start()
    {
        FindPlayer();
        timeToDestroy = timeToTrigger*2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeToWarning--;
        timeToTrigger--;
        timeToDestroy--;

        if (timeToWarning < 0) {timeToWarning = -5;}
        if (timeToTrigger < 0) {timeToTrigger = -5;}
        if (timeToDestroy < 0) {Destroy(gameObject);}
    }

    public void CreateLaser(Vector3 spawnLoc, GameObject laser = null, bool lockVert = false, bool lockHor = false, bool lockOnPlayer = true, bool controlsLaser = true, bool forceNearVert = false)
    {
        if (laser == null) {laser = laserToSpawn;}

        quaternion spawnAngle;
        Vector3 spawnPos = spawnLoc;
        if (lockVert && lockHor)
        {
            int ran = UnityEngine.Random.Range(1, 3);
            if (ran == 1) {spawnAngle = quaternion.identity;}
            else {spawnAngle = Quaternion.Euler(0f, 0f, 90f);}
        }
        else if (lockVert) {spawnAngle = quaternion.identity;}
        else if (lockHor) {spawnAngle = Quaternion.Euler(0f, 0f, 90f);}
        else
        {
            float angleZ;
            if (forceNearVert) {angleZ = UnityEngine.Random.Range(-45, 45);}
            else {angleZ = UnityEngine.Random.Range(0, 459);}
            spawnAngle = Quaternion.Euler(0f, 0f, angleZ);
        }

        if (lockOnPlayer) {spawnLoc = player.transform.position;}
        spawnLoc.z -= 9;

        GameObject obj = Instantiate(laser, spawnLoc, spawnAngle);
        obj.GetComponent<LaserSummon>().controlScript = GetComponent<LaserRemoteControl>();
        if (controlsLaser) {obj.GetComponent<LaserSummon>().timeToTrigger = -10 ;}
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
            Debug.Log("No player found");
        }
    }
}
