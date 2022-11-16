using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Papae.UnitySDK.Managers;


namespace PudimdimGames{

    public class AI_Enemy : MonoBehaviour{
        
        // Patroling randomly between waypoints
        public Transform[] moveSpots;
        private int randomSpot;
        [HideInInspector]
        public string stateText;
    
        // AI sight
        [SerializeField] private bool playerIsInLOS = false;
        [SerializeField] private float fieldOfViewAngle;
        [SerializeField] private float losRadius;

        // AI sight and Memory
        [SerializeField]private bool aiMemorizesPlayer = false;
        [SerializeField] private float memoryStartTime;
        private float  increasingMemoryTime;

        // AI hearing
        private Vector3 noisePosition;
        [SerializeField]private bool aiHeardPlayer;
        [SerializeField] private float noiseTravelDistance;
        [SerializeField] private float spinSpeed;
        private bool canSpin = false;
        private float isSpinningTime; // Search at player-noise-position
        public float spinTime = 3f;


        // Wait time at waypoint for patrolling
        private float waitTime;
        [SerializeField] private float startWaitTime;

        NavMeshAgent nav;

        // AI strafe
        [SerializeField] private float distToPlayer; // straferadius
        
        // When to chase
        [SerializeField] private float chaseRadius;
        [SerializeField] private float facePlayerFactor;
        [SerializeField] private bool move;
        float distance;
        float idleSpeed = 0;

        Animator anim;

        float normalSpeed = 2;
        [SerializeField] private bool caught; 

        [SerializeField] GameObject[] itens;

        /// Awake is called when the script instance is being loaded.
        void Awake(){
            nav = GetComponent<NavMeshAgent>();
            nav.enabled = true;
            anim = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start(){
            waitTime = startWaitTime;
            randomSpot = Random.Range(0, moveSpots.Length);
            caught = false;

        }

        // Update is called once per frame
        void Update(){
            distance = Vector3.Distance(Comp_CharacterController.playerPos, transform.position);
            anim.SetFloat("Speed", nav.speed);
            
            if(!caught){
                               
                if(nav.isActiveAndEnabled){
                    CheckLOS();
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
        }

        void NoiseCheck(){
            for (int i = 0; i < itens.Length; i++){
                if(Vector3.Distance(transform.position, itens[i].GetComponent<PickUpItem>().getItemPos) <= noiseTravelDistance){
                                        
                    if(itens[i].GetComponent<PickUpItem>().getItemSound){
                        aiHeardPlayer = true;
                        if(CountDownTimer.TimerInstance == null){
                            if(itens[i].GetComponent<PickUpItem>().getItemSound){
                                aiHeardPlayer = true;
                                stateText = "Enemy Heard a Noise";
                                Debug.Log("Enemy Heard a Noise CountDown");
                                noisePosition = itens[i].GetComponent<PickUpItem>().getItemPos;
                            }
                        }else{       
                            if(itens[i].GetComponent<PickUpItem>().getItemSound || CountDownTimer.TimerInstance.noise){
                                aiHeardPlayer = true;
                                stateText = "Enemy Heard a Noise";
                                Debug.Log("Enemy Heard a Noise countdown else " + aiHeardPlayer);
                                if(CountDownTimer.TimerInstance.noise){
                                    noisePosition = CountDownTimer.TimerInstance.getObjPos;
                                    
                                }else{
                                    noisePosition = itens[i].GetComponent<PickUpItem>().getItemPos;
                                }
                                         
                            }
                        }
                        
                        if(Vector3.Distance(transform.position, itens[i].GetComponent<PickUpItem>().getItemPos) <= 1.7f){
                            itens[i].GetComponent<PickUpItem>().getItemSound = false;
                            aiHeardPlayer = false;
                        }
    
                    }
                }
            }
        }

        void GoToNoisePosition(){
            transform.LookAt(noisePosition);
            nav.speed = normalSpeed;
            nav.SetDestination(noisePosition);
            stateText = "Enemy heard a noise";
            if(Vector3.Distance(transform.position, noisePosition) <= noiseTravelDistance && canSpin == true){
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
            Vector3 direction = Comp_CharacterController.playerPos - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if(angle < fieldOfViewAngle * 0.5f){
                RaycastHit hit;
                if(Physics.Raycast(transform.position, direction.normalized, out hit, losRadius)){
                    if(hit.collider.tag == "Player"){
                        playerIsInLOS = true;
                        aiMemorizesPlayer = true;
                    }
                }else{
                        playerIsInLOS = false;
                        aiMemorizesPlayer = false;
                    }
            }
        }

        void Patrol(){
            if(move){    
                stateText = "Patrol Mode";
                Vector3 LookAtPos = new Vector3(moveSpots[randomSpot].position.x,transform.position.y,moveSpots[randomSpot].position.z);
                transform.LookAt(LookAtPos);
                nav.speed = normalSpeed;
                nav.SetDestination(moveSpots[randomSpot].position);
                
                
                if(Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 2.0f){
                    if(waitTime <= 0){
                        nav.speed = normalSpeed;
                        randomSpot = Random.Range(0, moveSpots.Length);
                        waitTime = startWaitTime;
                    }else{
                        nav.speed = idleSpeed;
                        waitTime -= Time.deltaTime; 
                    }
                }
            }else{
                
                nav.speed = idleSpeed;
            }
        }

        void ChasePlayer(){            
               if(distance > distToPlayer){ 
                    stateText = "Chase Mode";
                    nav.speed = normalSpeed;
                    transform.LookAt(Comp_CharacterController.playerPos);
                    nav.SetDestination(Comp_CharacterController.playerPos);
               }else{
                    stateText = "You Got Caught";
                    nav.speed = idleSpeed;
                    caught = true;
                    if(!CheatController.cheatInstance.getCheat)
                        GameMenu.gameMenuInstance.getCaughtInfo = caught;
               }
            
        }


        void FacePlayer(){
            Vector3 direction = (Comp_CharacterController.playerPos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * facePlayerFactor);
        }

    }

}























