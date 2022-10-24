using UnityEngine;

namespace Mz.App
 {
     public class CameraMain : MzBehaviourBase
     {
         public UnityEngine.Camera Camera { get; private set; }
         public Rigidbody2D Target { get; set; }
         
         [SerializeField]
         private float _smoothSpeedPosition = 0.0625f;
         public float SmoothSpeedPosition { get => _smoothSpeedPosition; set => _smoothSpeedPosition = value; }
         [SerializeField] private float _smoothSpeedRotation = 0.25f;
         public float SmoothSpeedRotation { get => _smoothSpeedRotation; set => _smoothSpeedRotation = value; }
         
         public Vector3 Offset { get; set; } = new Vector3(0, 0, -10f);
         
         public void PreInitialize()
         {
             Camera = gameObject.AddComponent<UnityEngine.Camera>();
             Camera.clearFlags = CameraClearFlags.SolidColor;
             Camera.backgroundColor = Color.black;
             gameObject.AddComponent<AudioListener>();
             
             transform.position = Offset;
         }

         void FixedUpdate()
         {
             if (Target == null) return;
             transform.rotation = Quaternion.Lerp(transform.rotation, Target.transform.rotation, SmoothSpeedRotation);

             //var forwardVelocity = Vector3.Dot(Target.velocity, Target.transform.up);
             var lead = Target.transform.up * 3f;
             var positionDesired = Target.transform.position + Offset;// + lead;
             transform.position = Vector3.Lerp(transform.position, positionDesired, SmoothSpeedPosition);

             transform.RotateAround (transform.position, transform.right, -3);
             //transform.LookAt(Target.transform);
         }
     }
 }