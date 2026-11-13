using UnityEngine;

public class RoomCode : MonoBehaviour
{
    [Header("Universals")]
    [SerializeField] public GameObject StartRoom;//Where this room starts.
    [SerializeField] public GameObject EndRoom;//Where is the next room spawning at
    [SerializeField] public string Zone;//What is the name of the boss' Zone
    [SerializeReference] public GameObject PlayerCheck;//What checks for the player to spawn the next area.
    [Header("Specifics")]
    [SerializeField] public bool BossRoom;
    [SerializeField] public bool TranHall;
}
