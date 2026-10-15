using System;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class ScrCreateTurrain : MonoBehaviour
{
    [SerializeField] GameObject startRoom;

    public GameObject[] possableRooms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(startRoom, new Vector3(0, 1, 0), new quaternion(0, 0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        GameObject roomToCreate = possableRooms[UnityEngine.Random.Range(0, possableRooms.Length)];
        
    }
}
