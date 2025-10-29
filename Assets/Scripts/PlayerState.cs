using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsGrounded(Vector3 LocalGravity) {
        //is player touching ground, case: player is on floor
        if (LocalGravity == Vector3.down) {
            return Mathf.Abs(transform.position.y + 4f) < 0.1f;
        }
        //is player touching ground, case: player is on ceiling
        else if (LocalGravity == Vector3.up) {
            return Mathf.Abs(transform.position.y - 4f) < 0.1f;
        }
        //is player touching ground, case: player is on left wall
        else if (LocalGravity == Vector3.left) {
            return Mathf.Abs(transform.position.x + 4f) < 0.1f;
        }
        //is player touching ground, case: player is on right wall
        else if (LocalGravity == Vector3.right) {
            return Mathf.Abs(transform.position.x - 4f) < 0.1f;
        }

        return false;
    }

    public bool IsFalling(Vector3 LocalGravity, float bound) {
        //is player falling, case: player is on floor
        if (LocalGravity == Vector3.down) {
            return transform.position.y < -1 * bound;
        }
        //is player falling, case: player is on ceiling
        else if (LocalGravity == Vector3.up) {
            return transform.position.y > bound;
        }
        //is player falling, case: player is on left wall
        else if (LocalGravity == Vector3.left) {
            return transform.position.x < -1 * bound;
        }
        //is player falling, case: player is on right wall
        else if (LocalGravity == Vector3.right) {
            return transform.position.x > bound;
        }

        return false;
    }
}
