using UnityEngine;

public class StartButton : MonoBehaviour
{
    [SerializeReference] private GameObject Collection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initiate()
    {
       
        Collection.SetActive(false);
    }
}
