using UnityEngine;

public class GetPosition : MonoBehaviour
{
    public enum spawnPosType {OnPlayer, OnPlayerOffset, AtRandomPositionList, AtRandomPositionRange};
    [Header("Spawn Options")]
    [SerializeField] public spawnPosType spawnType;
    [SerializeField] public Vector3 offsetFromPlayer;
    [SerializeField] public Vector3[] spawnPositions; //First two work for random range
    [SerializeField] public bool adjustToBeRelitiveToSelf;

    private GameObject player;

    void Start()
    {
        FindPlayer();
    }
    public Vector3 FindLoctionWithDefault()
    {
        return FindLoction(spawnType , offsetFromPlayer , spawnPositions, adjustToBeRelitiveToSelf);
    }
    
    public Vector3 FindLoction(spawnPosType spawnType, Vector3 offsetFromPlayer, Vector3[] spawnPositions, bool adjustToBeRelitiveToSelf)
    {
        Vector3 spawnPos = new Vector3(0,0,0);
        switch (spawnType)
        {
            case spawnPosType.OnPlayer:
                spawnPos = player.transform.position;
            break;
            case spawnPosType.OnPlayerOffset:
                spawnPos = player.transform.position + offsetFromPlayer;
            break;
            case spawnPosType.AtRandomPositionList:
                spawnPos = spawnPositions[UnityEngine.Random.Range(0 , spawnPositions.Length)];
                if (adjustToBeRelitiveToSelf) {spawnPos += transform.position;}
            break;
            case spawnPosType.AtRandomPositionRange:
                spawnPos.x = UnityEngine.Random.Range(spawnPositions[0].x, spawnPositions[1].x);
                spawnPos.y = UnityEngine.Random.Range(spawnPositions[0].y, spawnPositions[1].y);
                spawnPos.z = UnityEngine.Random.Range(spawnPositions[0].z, spawnPositions[1].z);
                if (adjustToBeRelitiveToSelf) {spawnPos += transform.position;}
            break;
        }
        return spawnPos;
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
