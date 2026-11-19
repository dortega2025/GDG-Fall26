using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class MRoomCreationControl : MonoBehaviour
{
    [SerializeField] string[] possibleZones;
    [Header("Possible Rooms in Each Zone")]
    [SerializeField] GameObject[] zone1Options;
    [SerializeField] GameObject[] zone2Options;
    [SerializeField] GameObject[] zone3Options;
    [SerializeField] GameObject[] zone4Options;
    [SerializeField] GameObject[] zone5Options;

    [Header("Other Rooms")]
    [SerializeField] GameObject[] startRooms; //Put in same order as zones list
    [SerializeField] GameObject[] transHalls; //Put in same order as zones list
    [SerializeField] GameObject[] bossRooms; //Put in same order as zones list
    [SerializeField] GameObject[] endRooms; //Put in same order as zones list

    [Header("Game Options")]
    [SerializeField] int zonesPerGame;
    [SerializeField] int roomsPerZone;
    [SerializeField] GameObject playerPrefab;

    [Header("Debug Shown Info")]
    [SerializeField] string[] chosenZones;
    [SerializeField] List<GameObject> roomOrder;
    [SerializeField] List<string> roomType;
    [SerializeField] int currentRoomIndex;
    [SerializeField] List<GameObject> currentlyActiveRooms;
    [SerializeField] Vector3 lastRoomEndpoint;

    ///
    private GameObject[][] zOptionsAll;
    
    void Awake()
    {
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, transform.position + new Vector3(2, 1, 0), transform.rotation);
        }

        lastRoomEndpoint = transform.position;

        currentRoomIndex = 0;

        BuildzOptionsAll();
        chosenZones = RandomizeStringArray(possibleZones);
        for(int roomIndex = 0; roomIndex < zonesPerGame; roomIndex++)
        {
            int zoneIndex = FindIndexInArray(chosenZones[roomIndex], possibleZones);
            GameObject[] relatedRoomsArray = zOptionsAll[zoneIndex];

            if (roomIndex == 0)
            {
                roomOrder.Add(startRooms[zoneIndex]); //Put start room here
                roomType.Add("Start");
            }

            float usedRoomSpace = 0;
            GameObject[] possibleRooms = RandomizeGameObjectArray(relatedRoomsArray);
            int arrayLength = relatedRoomsArray.Length;
            while (usedRoomSpace < roomsPerZone)
            {
                int chosenRoomIndex = UnityEngine.Random.Range(0, arrayLength);
                Debug.Log(chosenRoomIndex);
                GameObject chosenRoom = possibleRooms[chosenRoomIndex];
                roomOrder.Add(chosenRoom);
                roomType.Add("Standered");
                usedRoomSpace += chosenRoom.GetComponent<MRoomCode>().roomExistanceValue;
            }

            //Adds final rooms of a Zone
            roomOrder.Add(bossRooms[zoneIndex]);
            roomType.Add("Boss");

            roomOrder.Add(transHalls[zoneIndex]);
            roomType.Add("Transhall");

            if (roomIndex == zoneIndex - 1)
            {
                //Adds finalrooms
                roomOrder.Add(endRooms[zoneIndex]);
                roomType.Add("Final");
            }
        }
        BuildNextZone();     
    }

    public void BuildNextZone()
    {
        string lastBuiltRoomType = null;
        while (lastBuiltRoomType != "Transhall" && lastBuiltRoomType != "Final")
        {
            GameObject newRoom = Instantiate(roomOrder[currentRoomIndex], lastRoomEndpoint, transform.rotation);
            lastBuiltRoomType = roomType[currentRoomIndex];
            lastRoomEndpoint = newRoom.GetComponent<MRoomCode>().roomEndPointObj.transform.position;
            currentlyActiveRooms.Add(newRoom);
            currentRoomIndex++;
            if (lastBuiltRoomType == "Transhall")
            {
                transform.position = lastRoomEndpoint + new Vector3(0, 5, 0);
                MTransHall hallCode = newRoom.transform.Find("TransHallControl").GetComponent<MTransHall>();
                if (hallCode != null)
                {hallCode.relatedGameObject = gameObject;}

            }

        }
    }

    public void DestroyLastZone()
    {
        for(int i = 0; i < currentlyActiveRooms.Count - 1; i++) //So it doesn't destroy the current transRoom
        {
            Destroy(currentlyActiveRooms[i]);
        }
    }

    private void BuildzOptionsAll()
    {
        zOptionsAll = new GameObject[5][];
        zOptionsAll[0] = zone1Options;
        zOptionsAll[1] = zone2Options;
        zOptionsAll[2] = zone3Options;
        zOptionsAll[3] = zone4Options;
        zOptionsAll[4] = zone5Options;
    }

    private int FindIndexInArray(string target, string[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if (array[i] == target)
            {
                return i;
            }
        }
        return -1;
    }

    private GameObject[] RandomizeGameObjectArray(GameObject[] inputArray)
    {
        int arrLength = inputArray.Length;
        GameObject[] tempArray = new GameObject[arrLength];
        inputArray.CopyTo(tempArray, 0);
        for(int i = 0; i < 10*arrLength; i++)
        {
            int rR1 = UnityEngine.Random.Range(0, arrLength);
            int rR2 = UnityEngine.Random.Range(0, arrLength);
            GameObject temp = tempArray[rR1];
            tempArray[rR1] = tempArray[rR2];
            tempArray[rR2] = temp;
        }
        return tempArray;
    }

    private string[] RandomizeStringArray(string[] inputArray)
    {
        int arrLength = inputArray.Length;
        string[] tempArray = new string[arrLength];
        inputArray.CopyTo(tempArray, 0);
        for(int i = 0; i < 10*arrLength; i++)
        {
            int rR1 = UnityEngine.Random.Range(0, arrLength);
            int rR2 = UnityEngine.Random.Range(0, arrLength);
            string temp = tempArray[rR1];
            tempArray[rR1] = tempArray[rR2];
            tempArray[rR2] = temp;
        }

        string[] outputArray = new string[zonesPerGame];
        for(int i = 0; i < zonesPerGame; i++)
        {
            outputArray[i] = tempArray[i];
        }
        return outputArray;
    }
}
