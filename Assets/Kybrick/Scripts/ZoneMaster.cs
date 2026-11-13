using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZoneMaster : MonoBehaviour
{
    ///////
    [Header("LoadOrder")]
    [SerializeField] private int ZoneNumber;
    [SerializeReference] private GameObject StartRoom;//The prefab that will always go first
    [SerializeReference] private List<GameObject> LoadOrder = new List<GameObject>();// The order in which rooms are going to be placed
    private int LoadListCheck;
    ///////
    [Header("Prefabs")]
    [SerializeReference] private List<GameObject> Prefabs = new List<GameObject>();//Every possible room
    [SerializeReference] private GameObject Transition;
    [SerializeReference] private GameObject THEULTIMATEPLACEHOLDER;
    private int PrefabListCheck;
    ///////
    [Header("Collect from Prefab")]
    [SerializeReference] private List<string> Blacklist = new List<string>();// The Zones that are not allowed
    private int BlackListListCheck;
    [SerializeReference] private RoomCode Room;//The room 
    [SerializeReference] private GameObject RoomPrefab;//The prefab of the room you are collecting.
    [SerializeReference] private GameObject SpawnNextRoom;//The place to spawn the next room;
    [SerializeReference] private GameObject PlayerCheck;//The new place to wait and see if the player gets there 
    [SerializeReference] private TriggerBoss PlayerCheckCode;// if the player check is active
    private int RNG; 
    
    public void Awake()
    {
    for (int i = 0; i <= ZoneNumber; i += 1)
    {
            if (Prefabs.Any<GameObject>())
            {
                RNG = Random.Range(0, Prefabs.Count - 1);
                RoomPrefab = Prefabs[RNG];
                Room = RoomPrefab.GetComponent<RoomCode>();
            }
            if (!Blacklist.Contains(Room.Zone) && !Room.BossRoom && !Room.TranHall)//Checks if it is not a tran hall and not a boss room and not in the blacklist
            {
                Blacklist.Add(Room.Zone);
                LoadOrder.Add(RoomPrefab);
                Prefabs.Remove(RoomPrefab);
                Debug.Log(RoomPrefab.name + "Was added to list at position" + LoadOrder.Count);

                BossCheck();


                Culling();
            }          
            }
        }
    public void BossCheck()
    {
        for (int b = 0; b <= Prefabs.Count-1; b += 1)//add the boss room
        {
            if (Prefabs.Any<GameObject>())
            {
                Debug.Log("ActiveBoss Loop");
                GameObject BossRoom = Prefabs[b];
                RoomCode BossCode = BossRoom.GetComponent<RoomCode>();
                if (BossCode.Zone == Room.Zone && BossCode.BossRoom)
                {
                    LoadOrder.Add(Prefabs[b]);
                    LoadOrder.Add(Transition);
                    Debug.Log(BossRoom.name + "Was added to list at position" + LoadOrder.Count);
                    b = Prefabs.Count;
                }
            }
        }
    }
    public void Culling()
    {
        

        for (int e = 0; e <= Prefabs.Count-1; e += 1)//Culls the blacklisted prefabs
        {
            if (Prefabs.Any<GameObject>())
            {
                GameObject CheckRoom = Prefabs[e];
                RoomCode CheckCode = CheckRoom.GetComponent<RoomCode>();
                if (Blacklist.Contains(CheckCode.Zone))
                {
                    Debug.Log(CheckRoom.name + "Was Removed from Prefabs");
                    Prefabs.Remove(Prefabs[e]);
                }
            }
        }
    }

    public void Start()
    {
        //Compacted Manual setting of the StartHall and the First Room.
        GameObject Build = LoadOrder[0];
        GameObject Begin = Instantiate(StartRoom);
        Begin.transform.position = new Vector2(0, 0);
        RoomCode Code = Begin.GetComponent<RoomCode>();
        GameObject Spawn = Code.EndRoom;
        Instantiate(Build);
        Build.transform.position = Spawn.transform.position;
        LoadOrder.RemoveAt(0);//remove so we can go down the build list.
        //Next BossRoom;
        Code = Build.GetComponent<RoomCode>();// get the last rooms code
        Build = LoadOrder[0];// get the next room
        Spawn = Code.EndRoom;
        Instantiate(Build);
        Build.transform.position = Spawn.transform.position;
        LoadOrder.RemoveAt(0);
        //Next TransitionHall.
        Code = Build.GetComponent<RoomCode>();
        Build = LoadOrder[0];
        Spawn = Code.EndRoom;

        Instantiate(Build);
        Build.transform.position = Spawn.transform.position;
        LoadOrder.RemoveAt(0);

        Code = Build.GetComponent<RoomCode>();
        PlayerCheck = Code.PlayerCheck;
        PlayerCheckCode = PlayerCheck.GetComponent<TriggerBoss>();

        RoomPrefab = LoadOrder[0];
        Room = RoomPrefab.GetComponent<RoomCode>();

    }
    public void Update()
    {
        if (LoadOrder.Any<GameObject>() == false)
        {
            Destroy(gameObject);
        }
        //So we can reference the exact build made in this update
        GameObject Build = RoomPrefab;
        RoomCode Code = Room;
        GameObject Spawn = Code.EndRoom;
        //

        if (PlayerCheckCode.activated == true)
        {
            for (int i = 0; i < 3; i += 1)// first is next room, then boss, then tranHall
            {
                //Create The Saved prefab
                Instantiate(Build);
                Build.transform.position = Spawn.transform.position;//Move it to the right spot

                LoadOrder.RemoveAt(0);//Remove it from load Order

                //Set New Prefab
                RoomPrefab = LoadOrder[0];
                Room = RoomPrefab.GetComponent<RoomCode>();

                //set the holding references
                Build = RoomPrefab;
                Code = Room;
                Spawn = Code.EndRoom;

            }

            PlayerCheck = Code.PlayerCheck;
            PlayerCheckCode = PlayerCheck.GetComponent<TriggerBoss>();
        }
    }
}
