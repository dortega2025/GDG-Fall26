using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class TimeKeepEx : MonoBehaviour
{
    public bool pauseTime = false;

    public float timeSinceGameStart;
    public double millisecondCountOffest;
    public double totalMilliSecondsPassed = 0;

    private bool timeJustPauesed = false;
    private double timeAtPause = 0;

    [Header("Current Time")]
    public float Minutes;
    public float Seconds;
    public double Milliseconds;

    void Start()
    {
        resetTime();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceGameStart = Time.time;
        if (pauseTime)
        {
            timeJustPauesed = true;
            if (timeAtPause == 0) {timeAtPause = Time.time;}
            return;
        }

        if (timeJustPauesed)
        {
            timeJustPauesed = false;

            millisecondCountOffest += Math.Abs(timeAtPause - millisecondCountOffest);

            timeAtPause = 0;
        }


        //Gen update
        
        totalMilliSecondsPassed = Math.Floor((Time.time - millisecondCountOffest) * 1000);

        //Formatting
        float tmpSeconds = (float)Math.Floor(totalMilliSecondsPassed/1000);
        Minutes = (float)Math.Floor(tmpSeconds/60);
        Seconds = tmpSeconds - Minutes*60;
        Milliseconds = totalMilliSecondsPassed - Seconds*1000;
        
    }
    public void resetTime()
    {
        totalMilliSecondsPassed = 0;
        millisecondCountOffest = Time.time;
    }
}
