using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeReference] private GameObject NodeLU;//the left/up node
    [SerializeReference] private GameObject NodeRD;//the right/down node
    [SerializeReference] private GameObject Platform;//the platform that moves
    public float Speed;// speed of platform
    public float Dir;// direction of platform 1 == to LU; 2== to RD
    float Distance;//how close it is to next Nodule
    public float DistTolerance;//how close the platform can be before turning around

    void Start()
    {
      if(Dir==1)
        {
            Debug.Log("Going to node LU");
        }
        else
        {
            Debug.Log("Going to node RD");
        }
    }

    void FixedUpdate()
    {

        if (Dir == 1)
        {
            Platform.transform.position = Vector2.MoveTowards(Platform.transform.position, NodeLU.transform.position, Speed * Time.deltaTime);
            //moving to LU
            Distance = Vector2.Distance(Platform.transform.position, NodeLU.transform.position);
            //Checking distanct to LU
        }
        else
        {
            Platform.transform.position = Vector2.MoveTowards(Platform.transform.position, NodeRD.transform.position, Speed * Time.deltaTime);
            //moving to RD
            Distance = Vector2.Distance(Platform.transform.position, NodeRD.transform.position);
            //Checking distance to RD
        }

        if (Distance <= DistTolerance)
        {
            if (Dir == 1)
            {
                Dir = 2;
                //if going to LU go then go to RD
            }
            else
            {
                Dir = 1;
                //else going to RD then go to LU
            }
        }
    }

        private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }


}
    
    



