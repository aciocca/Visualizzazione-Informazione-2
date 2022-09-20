using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class flyingscript : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float flyingSpeed = 1000.0f;
    [SerializeField] float dragForce = 100.0f;
    
    [Header("References")]
    [SerializeField] InputActionReference leftControllerFlyReference;
    [SerializeField] InputActionReference directionUpReference;
    [SerializeField] InputActionReference directionDownReference;
    [SerializeField] Transform trackingReference;

    Rigidbody _rigidbody;
    
    void Awake(){
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation; // Diminuisce motion sickness
    }
    
    void FixedUpdate()
    {
        if (leftControllerFlyReference.action.IsPressed()
            && directionUpReference.action.IsPressed()){
                Vector3 localVelocity = trackingReference.transform.forward;
                Vector3 worldVelocity = trackingReference.TransformDirection(localVelocity);
                _rigidbody.AddForce(worldVelocity * flyingSpeed, ForceMode.Acceleration);                
        }
        if (leftControllerFlyReference.action.IsPressed()
            && directionDownReference.action.IsPressed()){
                Vector3 localVelocity = Vector3.down;
                Vector3 worldVelocity = trackingReference.TransformDirection(localVelocity);
                _rigidbody.AddForce(worldVelocity * flyingSpeed, ForceMode.Acceleration);
            }
        if (_rigidbody.velocity.sqrMagnitude > 0.01f){
            _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
        }
    }
    protected virtual Vector3 ComputeDesiredMove(Vector2 input)
    {
         if (input == Vector2.zero)
                     return Vector3.zero;
     
                 var xrOrigin = system.xrOrigin;
                 if (xrOrigin == null)
                     return Vector3.zero;
     
                 // Assumes that the input axes are in the range [-1, 1].
                 // Clamps the magnitude of the input direction to prevent faster speed when moving diagonally,
                 // while still allowing for analog input to move slower (which would be lost if simply normalizing).
                 var inputMove = Vector3.ClampMagnitude(new Vector3(m_EnableStrafe ? input.x : 0f, 0f, input.y), 1f);
     
                 var originTransform = xrOrigin.Origin.transform;
                 var originUp = originTransform.up;
     
                 // Determine frame of reference for what the input direction is relative to
                 var forwardSourceTransform = m_ForwardSource == null ? xrOrigin.Camera.transform : m_ForwardSource;
                 var inputForwardInWorldSpace = forwardSourceTransform.forward;
                 if (Mathf.Approximately(Mathf.Abs(Vector3.Dot(inputForwardInWorldSpace, originUp)), 1f))
                 {
                     // When the input forward direction is parallel with the rig normal,
                     // it will probably feel better for the player to move along the same direction
                     // as if they tilted forward or up some rather than moving in the rig forward direction.
                     // It also will probably be a better experience to at least move in a direction
                     // rather than stopping if the head/controller is oriented such that it is perpendicular with the rig.
                     inputForwardInWorldSpace = -forwardSourceTransform.up;
                 }
     
                 //TLS Edits based on https://answers.unity.com/questions/1851515/how-to-fly-in-controllers-forward-direction.html
     
                 var inputForwardProjectedInWorldSpace = forwardSourceTransform.forward; // Vector3.ProjectOnPlane(inputForwardInWorldSpace, originUp);
                 //var forwardRotation = Quaternion.FromToRotation(originTransform.forward, inputForwardProjectedInWorldSpace);
                 Quaternion forwardRotation = Quaternion.LookRotation(inputForwardInWorldSpace, forwardSource.up);
     
                 var translationInRigSpace = forwardRotation * inputMove * (m_MoveSpeed * Time.deltaTime);
                 var translationInWorldSpace = originTransform.TransformDirection(translationInRigSpace);
     
                 return translationInWorldSpace;
    }
}


