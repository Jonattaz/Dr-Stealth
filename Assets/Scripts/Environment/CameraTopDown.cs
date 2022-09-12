using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PudimdimGames{

    public class CameraTopDown : MonoBehaviour
    {
        // Controla a velocidade da câmera
        [SerializeField]
        float speed = 0f;

        // Referência ao objeto que a câmera precisa seguir
        [SerializeField]
        Transform target;

        // Controla a posição da câmera em relação ao target
        [SerializeField]
        Vector3 cameraPosition;

        // Controla a rotação da câmera em relação ao target
        [SerializeField]
        Vector3 cameraRotation;

        [Space(10)]
        // Controla se a câmera deve ou não focar no target
        [SerializeField]
        bool lookAt = false;

        [Space(10)]
        // Posição minima e máxima da câmera em X
        [SerializeField]
        float[] xCameraLimit = new float[2];

        // Posição minima e máxima da câmera em z
        [SerializeField]
        float[] zCameraLimit = new float[2];

        // Update is called once per frame
        void Update()
        {
        CameraController();
        }

        void CameraController(){        

            // Verifica se é para focar no target
            if(lookAt){
                // Foca no target
                transform.LookAt(target.position);
            }

            // Obtém a posição em X
            float posX = Mathf.Clamp(target.position.x, xCameraLimit[0], xCameraLimit[1]);

            // Obtém a posição em Z
            float posZ = Mathf.Clamp(target.position.z, zCameraLimit[0], zCameraLimit[1]);        

            // Faz a movimentação da câmera 
            transform.position = Vector3.Lerp(transform.position, new Vector3(posX, target.position.y, posZ) + cameraPosition, 
            speed * Time.deltaTime);
            // Faz a rotação da câmera
            transform.localEulerAngles = cameraRotation;

        }
    }
}