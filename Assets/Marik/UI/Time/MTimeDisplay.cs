using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;

public class MTimeDisplay : MonoBehaviour
{
    [SerializeField] TimeKeepEx timeKeeper; 

    [Header("Number sprites")]
    [SerializeField] Sprite Zero; 
    [SerializeField] Sprite One; 
    [SerializeField] Sprite Two; 
    [SerializeField] Sprite Three; 
    [SerializeField] Sprite Four; 
    [SerializeField] Sprite Five; 
    [SerializeField] Sprite Six; 
    [SerializeField] Sprite Seven; 
    [SerializeField] Sprite Eight; 
    [SerializeField] Sprite Nine; 

    [Header("Digits")]
    [SerializeField] GameObject thousanths; 
    [SerializeField] GameObject hundredths; 
    [SerializeField] GameObject tenths; 
    [SerializeField] GameObject secondsOnes; 
    [SerializeField] GameObject secondsTwos; 
    [SerializeField] GameObject MinutesOnes; 
    [SerializeField] GameObject MinutesTwos; 

    private UnityEngine.UI.Image thousanthsImage;
    private UnityEngine.UI.Image hundredthsImage;
    private UnityEngine.UI.Image tenthsImage;
    private UnityEngine.UI.Image secondsOnesImage;
    private UnityEngine.UI.Image secondsTwosImage;
    private UnityEngine.UI.Image MinutesOnesImage;
    private UnityEngine.UI.Image MinutesTwosImage;

    void Start()
    {
    thousanthsImage = thousanths.GetComponent<UnityEngine.UI.Image>();
    hundredthsImage = hundredths.GetComponent<UnityEngine.UI.Image>();
    tenthsImage = tenths.GetComponent<UnityEngine.UI.Image>();
    secondsOnesImage = secondsOnes.GetComponent<UnityEngine.UI.Image>();
    secondsTwosImage = secondsTwos.GetComponent<UnityEngine.UI.Image>();
    MinutesOnesImage = MinutesOnes.GetComponent<UnityEngine.UI.Image>();
    MinutesTwosImage = MinutesTwos.GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update() //timeKeeper
    {
        thousanthsImage.sprite = DigitToSprite((int)(timeKeeper.Milliseconds%10));
        hundredthsImage.sprite = DigitToSprite((int)(timeKeeper.Milliseconds/10%10));
        tenthsImage.sprite = DigitToSprite((int)(timeKeeper.Milliseconds/100%10));
        secondsOnesImage.sprite = DigitToSprite((int)(timeKeeper.Seconds%10));
        secondsTwosImage.sprite = DigitToSprite((int)(timeKeeper.Seconds/10));
        MinutesOnesImage.sprite = DigitToSprite((int)(timeKeeper.Minutes%10));
        MinutesTwosImage.sprite = DigitToSprite((int)(timeKeeper.Minutes/10));
    }

    private Sprite DigitToSprite(int number)
    {
        switch(number)
        {
            case 1:
            return One;
            case 2:
            return Two;
            case 3:
            return Three;
            case 4:
            return Four;
            case 5:
            return Five;
            case 6:
            return Six;
            case 7:
            return Seven;
            case 8:
            return Eight;
            case 9:
            return Nine;
            default:
            return Zero;
        }
    }
}
