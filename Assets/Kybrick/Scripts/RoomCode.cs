using UnityEngine;

public class RoomCode : MonoBehaviour
{
    [SerializeReference] public GameObject SpawnNextRoomPoint;
    [SerializeReference] public GameObject RoomStart;
    [SerializeReference] public GameObject Hall;
    [SerializeReference] private GameObject Placeholder;
    [SerializeReference] public GameObject[] Possibilities;
    [SerializeReference] public Collider2D CheckZone;
    public bool Terminate;
    public bool StartHall;
    public bool TranHall;
    private GameObject Chosen;
    int Check;
    int DeathCount;
    void Start()
    {
        if (TranHall)
        {
            Check = 0;
            while (Check <= Possibilities.Length - 1)
            {
                FindBosses();
                Debug.Log(Possibilities[Check]);
                Check++;
                Debug.Log(Check);
            }
            if (Check >= Possibilities.Length - 1)
            {
                Debug.Log("Transition");

                Transition();

            }
        }
        else
        {
            Roll();
            if (Terminate == true && TranHall == false)
            {
                Instantiate(Hall, SpawnNextRoomPoint.transform.position, SpawnNextRoomPoint.transform.rotation);
            }
            if (Terminate == false)
            {
                Instantiate(Chosen, SpawnNextRoomPoint.transform.position, SpawnNextRoomPoint.transform.rotation);
                Terminate = true;
            }

        }
        
    }
        void FindBosses()
        {
            GameObject Find = GameObject.Find((Possibilities[Check].name + "(Clone)"));
            if (Find != null)
            {

                Possibilities[Check] = Placeholder;
                Debug.Log(Possibilities[Check]);
            }
        }
        void Transition()
        {
            Roll();
            if (Terminate == false)
            {
                Instantiate(Chosen, SpawnNextRoomPoint.transform.position, SpawnNextRoomPoint.transform.rotation);
            }
        }
        void Roll()
        {
            if (DeathCount > 25)
            {
                Debug.Log("DeadLoop");
                Terminate = true;
            }
            else
            {
                int RNG = Random.Range(0, Possibilities.Length - 1);
                Chosen = Possibilities[RNG];
                if (Chosen == Placeholder)
                {
                    Debug.Log(DeathCount);
                    DeathCount++;
                    Roll();
                }
            }
        }

    
}
