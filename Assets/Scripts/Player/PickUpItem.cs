using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Papae.UnitySDK.Managers;


namespace PudimdimGames{

    public class PickUpItem : MonoBehaviour
    {
        [SerializeField] private Transform pickUpPoint;
        [SerializeField] private Transform player;
        [SerializeField] AudioClip itemNoise;
        private Rigidbody rb;
        private Vector3 itemPos;

        [SerializeField] private float pickUpDistance;
        [SerializeField] private float forceMulti;
        [SerializeField] private bool readyToThrow;
        [SerializeField] private bool itemIsPicked;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform releasePosition;
        [SerializeField] private bool noiseMode;
        [SerializeField] private bool itemSound;
        
        [HideInInspector] public Vector3 getItemPos;
        [HideInInspector] public bool getItemSound;

        [Header("Display Controls")]
        [Range(10, 100)]
        [SerializeField] private int linePoints = 25;
        [SerializeField] [Range(0.01f, 0.10f)] 

        private float timeBetweenPoints = 0.01f;
        private LayerMask itemCollisionMask;

        
        // Start is called before the first frame update
        void Start(){
            itemSound = false;
            noiseMode = false;
            int itemLayer = this.gameObject.layer;
            for (int i = 0; i < 32; i++){
                if (!Physics.GetIgnoreLayerCollision(itemLayer, i)){
                    itemCollisionMask |= 1 << i;
                }
            }

            StartCoroutine(TrackTarget());

            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update(){

            if(UnityEngine.Input.GetKey(KeyCode.F) && itemIsPicked == true && readyToThrow){
                this.transform.position = pickUpPoint.position;
                this.transform.parent = pickUpPoint.transform;
                
                if(forceMulti <= 1000){
                    forceMulti += 300 * Time.deltaTime;
                }
                
                DrawProjection();
            }            

            if(pickUpDistance <= 2 && itemIsPicked && forceMulti <= 10){
                this.transform.position = pickUpPoint.position;
            }

            pickUpDistance = Vector3.Distance(player.position, transform.position);

            if(pickUpDistance <= 2){
                if(UnityEngine.Input.GetKeyDown(KeyCode.F) && itemIsPicked == false && pickUpPoint.childCount < 1){
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<BoxCollider>().enabled = false;
                    this.transform.position = pickUpPoint.position;
                    this.transform.parent = pickUpPoint.transform;

                    itemIsPicked = true;
                    forceMulti = 0;
                }
            }

            if(UnityEngine.Input.GetKeyUp(KeyCode.F) && itemIsPicked == true){
                readyToThrow = true;
                if(forceMulti > 10){
                    rb.AddForce(player.transform.forward * forceMulti );
                    this.transform.parent = null;
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<BoxCollider>().enabled = true;
                    itemIsPicked = false;

                    noiseMode = true;
                    forceMulti = 0;
                    readyToThrow = false;
                    lineRenderer.enabled = false;
                }
                forceMulti = 0;
            }
        }
        
        private void DrawProjection(){
            lineRenderer.enabled = true;
            lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
            Vector3 startPosition = pickUpPoint.position;
            Vector3 startVelocity = forceMulti * pickUpPoint.transform.forward / rb.mass;
            int i = 0;
            lineRenderer.SetPosition(i,startPosition);

            for (float time = 0; time < linePoints; time += timeBetweenPoints){
                i++;
                Vector3 point = startPosition + time * startVelocity;
                point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
                lineRenderer.SetPosition(i, point);

                Vector3 lastPosition = lineRenderer.GetPosition(i - 1);

                if(Physics.Raycast(lastPosition, (point - lastPosition).normalized, out RaycastHit hit, 
                (point - lastPosition).magnitude, itemCollisionMask)){
                    lineRenderer.SetPosition(i, hit.point);
                    lineRenderer.positionCount = i + 1;
                    return;

                }
            }
        }

        void OnCollisionEnter(Collision other){
            getItemSound = itemSound;
            if(noiseMode){
                itemSound = true;
                getItemSound = itemSound;
                Papae.UnitySDK.Managers.AudioManager.Instance.PlaySFX(itemNoise, 1f);
                
            }
        }

        void OnCollisionStay(Collision other)
        {
            getItemSound = itemSound;
            noiseMode = false;
            if(!noiseMode){
                //itemSound = false;
            }
        }

         IEnumerator TrackTarget(){
            while(true){
                itemPos = gameObject.transform.position;
                getItemPos = itemPos;
                yield return null;
            }
        }

    }
}



























