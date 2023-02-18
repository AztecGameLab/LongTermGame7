using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class FadeIntoBlack : MonoBehaviour
{
    //Image (black square) that fades in
    public GameObject blackOutSquare;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //yarnspinner command to fade in
    //[YarnCommand("fade_in_camera")]
    public void FadeInCamera()
    {
        StartCoroutine(FadeBlackOutSquare());
    }

    //yarnspinner command to fade out
    //[YarnCommand("fade_out_camera")]
    public void FadeOutCamera()
    {
        StartCoroutine(FadeBlackOutSquare(false));
    }

    //Causes screen to fade into black. When yarnspinenr uses "false", fade out of black
    [YarnCommand("fade_camera")]
    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 5)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        //if we are fading to black
        if (fadeToBlack)
        {
            //while image square is not fully apparent
            while (blackOutSquare.GetComponent<Image>().color.a<1)
            {
                //Fade black square onto UI
                fadeAmount=objectColor.a+(fadeSpeed*Time.deltaTime);
                objectColor=new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
        //If we are fading OUT of black
        else
        {
            while (blackOutSquare.GetComponent<Image>().color.a>0)
            {
                fadeAmount=objectColor.a-(fadeSpeed*Time.deltaTime);
                objectColor=new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }
}
