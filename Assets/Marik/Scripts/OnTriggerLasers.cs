using UnityEngine;

public class OnTriggerLasers : MonoBehaviour
{
    [SerializeField] private GameObject[] lasers;
    [SerializeField] public float timeToWarning;
    [SerializeField] public float timeToTrigger;
    [SerializeField] public float timeToKeepFiring;

    private LaserSummon[] laserScr;
    private bool active = false;

    void Start()
    {
        laserScr = new LaserSummon[lasers.Length];
        for(int i = 0; i < lasers.Length; i++)
        {
            laserScr[i] = lasers[i].GetComponent<LaserSummon>();
        }

        for(int i = 0; i < laserScr.Length; i++)
        {
            laserScr[i].controlScript2 = gameObject.GetComponent<OnTriggerLasers>();
            if (timeToKeepFiring > -1) {laserScr[i].timeToKeepFiring = timeToKeepFiring;}
        }
    }

    void FixedUpdate()
    {
        if (active)
        {
            timeToWarning--;
            timeToTrigger--;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            active = true;
            for(int i = 0; i < laserScr.Length; i ++) {if (laserScr[i].startOff) {laserScr[i].TurnOn();}}
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
