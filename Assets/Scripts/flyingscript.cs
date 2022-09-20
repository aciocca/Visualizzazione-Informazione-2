using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class flyingscript : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float flyingSpeed = 10.0f;
    [SerializeField] float dragForce = 5.0f;
    
    [Header("References")]
    [SerializeField] InputActionReference leftControllerFlyReference;
    [SerializeField] InputActionReference directionForwardReference;
    [SerializeField] InputActionReference directionBackwardsReference;
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
            && directionForwardReference.action.IsPressed()){
                Vector3 localVelocity = trackingReference.transform.forward;
                _rigidbody.AddForce(localVelocity * flyingSpeed, ForceMode.Acceleration);                
        }
        if (leftControllerFlyReference.action.IsPressed()
            && directionBackwardsReference.action.IsPressed()){
                Vector3 localVelocity = trackingReference.transform.forward;
                _rigidbody.AddForce(-localVelocity * flyingSpeed, ForceMode.Acceleration);
        }
        if (leftControllerFlyReference.action.IsPressed()
            && directionUpReference.action.IsPressed()){
                _rigidbody.AddForce(Vector3.up * flyingSpeed, ForceMode.Acceleration);
        }
        if (leftControllerFlyReference.action.IsPressed()
            && directionDownReference.action.IsPressed()){ 
                _rigidbody.AddForce(-Vector3.up * flyingSpeed, ForceMode.Acceleration);
        }
        if (_rigidbody.velocity.sqrMagnitude > 0.01f){
            _rigidbody.AddForce(-_rigidbody.velocity * dragForce, ForceMode.Acceleration);
        }
    }
}
