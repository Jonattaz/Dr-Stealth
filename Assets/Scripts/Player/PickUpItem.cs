using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames{

    public class PickUpItem : MonoBehaviour
    {
        Transform pickUpPoint;
        Transform player;
        Rigidbody rb;

        [SerializeField] float pickUpDistance;
        [SerializeField] float forceMulti;
        [SerializeField] bool readyToThrow;
        [SerializeField] bool itemIsPicked;


        // Start is called before the first frame update
        void Start(){
            rb = GetComponent<Rigidbody>();
            player = UnityEngine.GameObject.Find("Player").transform;
            pickUpPoint = UnityEngine.GameObject.Find("PickUpPoint").transform;
        }

        // Update is called once per frame
        void Update(){

            if(UnityEngine.Input.GetKey(KeyCode.F) && itemIsPicked == true && readyToThrow){
                forceMulti += 300 * Time.deltaTime;
            }            

            pickUpDistance = Vector3.Distance(player.position, transform.position);

            if(pickUpDistance <= 2){
                if(UnityEngine.Input.GetKeyDown(KeyCode.F) && itemIsPicked == false && pickUpPoint.childCount < 1){
                    
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<BoxCollider>().enabled = false;
                    this.transform.position = pickUpPoint.position;
                    this.transform.parent = GameObject.Find("PickUpPoint").transform;

                    itemIsPicked = true;
                    forceMulti = 0;
                }
            }

            if(UnityEngine.Input.GetKeyUp(KeyCode.F) && itemIsPicked == true){
                readyToThrow = true;

                if(forceMulti > 10){
                    rb.AddForce(player.transform.forward * forceMulti);
                    this.transform.parent = null;
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<BoxCollider>().enabled = true;
                    itemIsPicked = false;

                    forceMulti = 0;
                    readyToThrow = false;
                }
                forceMulti = 0;
            }


        }
    }
}



























