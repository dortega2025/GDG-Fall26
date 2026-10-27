using UnityEngine;

public class GreenFlameBlast : MonoBehaviour
{
    public float die;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {
        die += Time.deltaTime;
        if (die > 10)
        {
            Destroy(gameObject);
        }
    }
}
