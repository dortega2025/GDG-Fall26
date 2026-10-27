using UnityEngine;

public class EnterTrigger : MonoBehaviour
{
    [SerializeReference] private GameObject ActivateThis;
     public GameObject CheckFor;
    public bool entered;
    void Start()
    {
        CheckFor = GameObject.Find("Player ");
        ActivateThis.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject ==CheckFor)
        {
            entered = true;
            ActivateThis.SetActive(true);
        }
    }
   
}
