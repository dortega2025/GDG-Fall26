using Unity.VisualScripting;
using UnityEngine;

public class MTransHall : MonoBehaviour
{
    [SerializeField] public GameObject roomCreator;
    [SerializeField] public GameObject relatedGameObject;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MRoomCreationControl control = relatedGameObject.GetComponent<MRoomCreationControl>();
            if (control == null) {return;}

            control.DestroyLastZone();
            control.BuildNextZone();

            Destroy(gameObject);
        }
    }
}
