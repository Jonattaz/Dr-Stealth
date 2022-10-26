using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceGuard : MonoBehaviour
{
    [SerializeField] private GameObject dialog;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        dialog.SetActive(true);
    }

    void OnTriggerExit(Collider other){
        dialog.SetActive(false);
    }
}
