using UnityEngine;

public class WallLaser : MonoBehaviour
{
    private float onOffTime = 0.5f;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = onOffTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            foreach (Transform child in gameObject.transform)
            {
                if (child.transform.gameObject.activeInHierarchy)
                {
                    child.transform.gameObject.SetActive(false);
                } else
                {
                    child.transform.gameObject.SetActive(true);
                } 
            }
            timer = onOffTime;
        }
    }
}
