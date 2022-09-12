using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PudimdimGames{
    public class AI_Enemy : MonoBehaviour{
        
        // Patroling randomly between waypoints
        public Transform[] moveSpots;
        private int randomSpot;

        // AI sight
        [SerializeField] private bool playerIsInLOS = false;
        [SerializeField] private float fieldOfViewAngle = 160f;
        [SerializeField] private float losRadius = 45f;

        // AI sight and Memory
        private bool aiMemorizesPlayer = false;
        [SerializeField] private float memoryStartTime = 10f;
        private float  increasingMemoryTime;

        // AI hearing
        private Vector3 noisePosition;
        private bool aiHeardPlayer;
        [SerializeField] private float noiseTravelDistance = 50f;
        [SerializeField] private float spinSpeed;
        private bool canSpin = false;
        private float isSpinningTime; // Search at player-noise-position
        public float spinTime = 3f;


        // Wait time at waypoint for patrolling
        [SerializeField] private float waitTime;
        [SerializeField] private float startWaitTime = 1f;

        [SerializeField] private Vector3 target;


        NavMeshAgent nav;

        // AI strafe
        [SerializeField] private float distToPlayer = 5.0f; // straferadius

        private float randomStrafeStartTime;
        private float waitStrafeTime;
        [SerializeField] private float t_minStrafe; // min and max AI waits once it has reached the "strafe" position before strafing again
        [SerializeField] private float t_maxStrafe;

        [SerializeField] private Transform strafeRight;
        [SerializeField] private Transform strafeLeft;
        private int randomStrafeDir;
        

        // When to chase
        [SerializeField] private float chaseRadius = 20f;
        [SerializeField] private float facePlayerFactor = 20f;
        
        /// Awake is called when the script instance is being loaded.
        void Awake(){
            nav = GetComponent<NavMeshAgent>();
            nav.enabled = true;
        }

        // Start is called before the first frame update
        void Start(){
            waitTime = startWaitTime;
            randomSpot = Random.Range(0, moveSpots.Length);
            
            StartCoroutine(TrackTarget());

        }

        // Update is called once per frame
        void Update(){
            float distance = Vector3.Distance(target, transform.position);

            if(distance > chaseRadius){
                Patrol();
            }
            else if(distance <= chaseRadius){
                ChasePlayer();

                FacePlayer();
            }

            if(distance <= losRadius){
                CheckLOS();
            }

            if(nav.isActiveAndEnable){
                if(playerIsInLOS == false && aiMemorizesPlayer == false && aiHeardPlayer == false){
                    Patrol();
                    NoiseCheck();


                    StopCoroutine(AiMemory());
                }else if(playerIsInLOS == false && aiMemorizesPlayer == false && aiHeardPlayer == true){
                    canSpin = true;
                    GoToNoisePosition();
                }else if(playerIsInLOS == true){
                    aiMemorizesPlayer = true;
                    FacePlayer();
                    ChasePlayer();
                }else if(aiMemorizesPlayer == true && playerIsInLOS == true){
                    ChasePlayer();

                    StartCoroutine(AiMemory());
                }
            }

        }

        void NoiseCheck(){
            float distance = Vector3.Distance(target, transform.position);

            if(distance <= noiseTravelDistance){
                if(Input.GetButton("Fire1")){
                    noisePosition = target;
                    aiHeardPlayer = true;
                }else{
                    aiHeardPlayer = false;
                    canSpin = false;
                }
            }
        }

        void GoToNoisePosition(){
            nav.SetDestination(noisePosition);

            if(Vector3.Distance(transform.position, noisePosition) <= 5f && canSpin == true){
                isSpinningTime += Time.deltaTime;
                transform.Rotate(Vector3.up * spinSpeed, Space.World);

                if(isSpinningTime >= spinTime){
                    canSpin = false;
                    aiHeardPlayer = false;
                    isSpinningTime = 0f;
                }   
            }
        }

        IEnumerator AiMemory(){
            increasingMemoryTime = 0;
            
            while (increasingMemoryTime < memoryStartTime){
                increasingMemoryTime += Time.deltaTime;
                aiMemorizesPlayer = true;
                yield return null;
            }

            aiHeardPlayer = false;
            aiMemorizesPlayer = false;
        }

        // Check Line Of Sight
        void CheckLOS(){
            Vector3 direction = target - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if(angle < fieldOfViewAngle * 0.5f){
                RaycastHit hit;

                if(Physics.Raycast(tansform.position, direction.normalized, out hit, losRadius)){
                    if(hit.collider.tag == "Player"){
                        playerIsInLOS = true;
                        aiMemorizesPlayer = true;
                    }else{
                        playerIsInLOS = false;
                    }
                }
            }
        }

        void Patrol(){
            nav.SetDestination(moveSpots[randomSpot].position);
            if(Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 2.0f){
                if(waitTime <= 0){
                    randomSpot = Random.Range(0, moveSpots.Length);

                    waitTime = startWaitTime;
                }else{
                    waitTime -= Time.deltaTime; 
                }
            }
        }

        void ChasePlayer(){
            float distance = Vector3.Distance(target, transform.position);
            if(distance <= chaseRadius && distance > distToPlayer){
                nav.SetDestination(target);
            }
            else if(nav.isActiveAndEnable && distance <= distToPlayer){
                randomStrafeDir = Random.Range(0,2);
                randomStrafeStartTime = Random.Range(t_minStrafe, t_maxStrafe);

                if(waitStrafeTime <= 0){
                    if(randomStrafeDir == 0){
                        nav.SetDestination(strafeLeft.position);
                    }
                    else if( randomStrafeDir == 1){
                        nav.SetDestination(strafeRight.position);
                    }

                    waitStrafeTime = randomStrafeStartTime;
                }else{
                    waitStrafeTime -= Time.deltaTime;
                }
            }
        }

        void FacePlayer(){
            Vector3 direction = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(tranform.rotation, lookRotation, Time.deltaTime * facePlayerFactor);
        }

        IEnumerator TrackTarget(){
            while(true){
                target = target.GameObject.transform.position;
                yield return null;
            }

        }

    }

}



























