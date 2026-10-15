using Unity.VisualScripting;
using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    [SerializeReference] GameObject Door1;
    [SerializeReference] GameObject Door2;
    [SerializeReference] GameObject Doorcreator1;
    [SerializeReference] GameObject Doorcreator2;

    bool activated;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated && collision.CompareTag("Player"))
        {
            activated = true;
        }
    }

    private void FixedUpdate()
    {
        if (activated)
        {
            Instantiate(Door1,Doorcreator1.transform);
            Instantiate(Door2, Doorcreator2.transform);
        }
        else if (FindDoor2() != null)
        {
            Destroy(FindDoor2());
            Destroy(gameObject);
        }
    }
    private GameObject FindDoor2()
    {
        GameObject Door = GameObject.Find(Door2 + "(clone)");
        return Door;
    }

    public void TurnOff()
    {
        activated = false;
    }
    
}
