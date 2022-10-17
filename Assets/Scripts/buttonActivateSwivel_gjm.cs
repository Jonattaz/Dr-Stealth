using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{


    public GameObject swivelAxis;


    Animator swivelAnnimation;

    bool buttonStatus;

    bool buttonMessage;



    
    void OnGUI()
    {
        //GUI.Label(new Rect(10, 10, 140, 20), "Airspeed:" + VT + " m/s");

        if (buttonMessage)
        {
            //GUI.Label(new Rect(10, 10, 140, 20), "buttonStatus: " + swivelAnnimation.GetBool("buttonDown"));
            //GUI.Label(new Rect(10, 20, 140, 20), "* PRESS BUTTON TO ACTIVATE SWIVEL. ");

            GUI.Label(new Rect(10, 10, 700, 700), "* Press E To Activate.");

            
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        swivelAnnimation = swivelAxis.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {

        //buttonMessage = true;

        //swivelAnnimation.SetBool("buttonDown", true);

    }


    void OnTriggerStay(Collider other)
    {
        buttonMessage = true;
        if (Input.GetKey("e"))
        {
            swivelAnnimation.SetBool("buttonDown", true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        //swivelAnnimation.SetBool("buttonDown", false);
        buttonMessage = false;
        swivelAnnimation.SetBool("buttonDown", false);
    }

}
