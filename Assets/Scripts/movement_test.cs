/* protected virtual Vector3 ComputeDesiredMove(Vector2 input)
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
             } */