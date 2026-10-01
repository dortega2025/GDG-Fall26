using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeReference] private GameObject Player;
    [SerializeField] private float lagMax;// how long it takes for the Last and new position  to update
    public float lag;
    private Vector2 LastPosition;
    private Vector2 NewPosition;
    private float Dist;
    public float Speed;
    public bool Moving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LastPosition = Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(lag >= lagMax && Moving==false)
        {
            CalcMovement();
            Moving = true;
        }
        else 
        {
            lag += 1 * Time.deltaTime;
        }

        if (Moving == true)
        {
            transform.position = Vector2.MoveTowards(LastPosition, NewPosition, Speed);
            CheckPos();
        }

    }
    public void CheckPos()
    {
        if(transform.position.x == NewPosition.x && transform.position.y == NewPosition.y)
        {
            LastPosition = NewPosition;
            Moving = false;
            lag = 0;
        }
    }
    public void CalcMovement()
    {
        NewPosition = Player.transform.position;
        if (NewPosition != LastPosition) 
        {
           Dist = Vector2.Distance(LastPosition, NewPosition);

            Speed = Dist / lag;
        }
    }
}
