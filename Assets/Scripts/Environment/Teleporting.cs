using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames{
    
    public class Teleporting : MonoBehaviour
    {
        [SerializeField] private Transform teleportTarget;
        [SerializeField] private GameObject player;
        
        void OnTriggerEnter(Collider other){
            StartCoroutine(Teleport());
            CameraFade.FadeInstance.Fade();
            player.GetComponent<Comp_CharacterController>().enabled = false;
        }

        IEnumerator Teleport(){
            yield return new WaitForSeconds(1);
            // Deixar tela escura e travar o jogador
            player.transform.position = new Vector3(
                teleportTarget.transform.position.x,
                teleportTarget.transform.position.y,
                teleportTarget.transform.position.z
            );
            CameraFade.FadeInstance.Fade();
            player.GetComponent<Comp_CharacterController>().enabled = true;
        }

    }
}