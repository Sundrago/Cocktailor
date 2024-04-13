using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialControl : MonoBehaviour
{
    // public GameObject[] tutorial;
    // public GameObject xo_ui;
    // int status;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     //PlayerPrefs.SetInt("tutorial", 0);
    //     if (!PlayerPrefs.HasKey("tutorial"))
    //     {
    //         PlayerPrefs.SetInt("tutorial", 0);
    //         status = 0;
    //     } else
    //     {
    //         status = 6;
    //     }
    //
    //     if(status == 6)
    //     {
    //         foreach(GameObject go in tutorial)
    //         {
    //             Destroy(go);
    //         }
    //     } else
    //     {
    //         foreach (GameObject go in tutorial)
    //         {
    //             go.SetActive(false);
    //         }
    //     }        
    // }
    //
    //
    // public void UpdateStatus(int input)
    // {
    //     if(status == 0 & input == 1)
    //     {
    //         ShowTutorial(0);
    //         status = 1;
    //     }
    //     else if (status == 1 & input == 1)
    //     {
    //         HideTutorial(0);
    //         ShowTutorial(1);
    //         status = 2;
    //     }
    //     else if (status == 2 & input == 2)
    //     {
    //         HideTutorial(1);
    //         ShowTutorial(2);
    //         status = 3;
    //     }
    //     else if (status == 3 & input == 1)
    //     {
    //         HideTutorial(2);
    //         ShowTutorial(3);
    //         status = 4;
    //     }
    //     else if (status == 4 & (input == 3 | input == 1))
    //     {
    //         HideTutorial(3);
    //         ShowTutorial(4);
    //         status = 5;
    //     }
    //     else if (status == 5 & input == 4)
    //     {
    //         HideTutorial(4);
    //         status = 6;
    //         PlayerPrefs.SetInt("tutorial", 6);
    //         PlayerPrefs.Save();
    //     }
    //
    //     PlayerPrefs.SetInt("tutorial", status);
    //     PlayerPrefs.Save();
    // }
    //
    // // Update is called once per frame
    // void ShowTutorial(int i)
    // {
    //     tutorial[i].SetActive(true);
    //     tutorial[i].GetComponent<Animator>().SetTrigger("show");
    // }
    //
    // void HideTutorial(int i)
    // {
    //     tutorial[i].GetComponent<Animator>().SetTrigger("hide");
    // }

}
