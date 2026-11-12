using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class DestroySelfWhenFarFromPlayer : MonoBehaviour
{
    [SerializeField] private float maxDist;
    public bool hold; //Cannot destroy untill hold lifted
    [SerializeField] private bool useDistToLiftHold; //Once player is in range, hold lifted
    [SerializeField] private float minDist; //Once player is in this range, hold lifted, if above is enabled
    [Header("Sound Related")]
    [SerializeField] private bool forceDestroy; //Doesn't allow audio to fade
    [SerializeField] private bool stopOnLoopDone; //stops looping

    private AudioSource audioPlayer;
    private GameObject player;
    void Start()
    {
        FindPlayer();
        audioPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!hold)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxDist)
            {
                if (audioPlayer == null || !audioPlayer.isPlaying || audioPlayer.volume <= .1 || forceDestroy)
                {
                    Destroy(gameObject);
                }
                else
                {
                    audioPlayer.volume -= .01f;
                    if (stopOnLoopDone) {audioPlayer.loop = false;}
                }
            }
        }
        else if (useDistToLiftHold)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < minDist)
            {
                hold = false;
            }
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
            Debug.Log("No player found");
        }
    }
}
