using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float scaleSpeed = 15f;
    private bool otherSpawn = false;
    private GameObject[] lasers;
    [SerializeField] private GameObject health;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GameObject.Find("HealthLogic");
    }

    // Update is called once per frame
    void Update()
    {
        lasers = GameObject.FindGameObjectsWithTag("Laser");
        if (lasers.Length > 1 && otherSpawn == false)
        {
            otherSpawn = true;
        }
        Vector3 currentScale = transform.localScale;
        if (currentScale.x < 15f && otherSpawn == true)
        {
            currentScale.x += scaleSpeed * Time.deltaTime;
        transform.localScale = currentScale;
        }
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        health.GetComponent<Health>().TakeDamage(0.5f);
    }
}
