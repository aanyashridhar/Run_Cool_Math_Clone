using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetAdjacentWall(Vector3 LocalGravity)
    {
        float x = transform.position.x;
        float y = transform.position.y;

        //find wall, case: player is on floor
        if (LocalGravity == Vector3.down)
        {
            //adjacent is left wall, case: on left seam & moving left
            if (x < -2.75 && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))) {
                return Vector3.left;
            }
            //adjacent is right wall, case: on right seam & moving right
            if (x > 2.75 && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))) {
                return Vector3.right;
            }
        }

        //find wall, case: player is on ceiling
        else if (LocalGravity == Vector3.up)
        {
            //adjacent is left wall, case: on left seam & moving right
            if (x < -2.75 && Input.GetKey(KeyCode.RightArrow)) {
                return Vector3.left;
            }
            //adjacent is right wall, case: on right seam & moving left
            if (x > 2.75 && Input.GetKey(KeyCode.LeftArrow)) {
                return Vector3.right;
            }
        }

        //find wall, case: player is on left wall
        else if (LocalGravity == Vector3.left)
        {
            //adjacent is floor, case: on right seam & moving right
            if (y < -2.75 && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))) {
                return Vector3.down;
            }
            //adjacent is ceiling, case: on left seam & moving left 
            if (y > 2.75 && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))) {
                return Vector3.up;
            }
        }

        //find wall, case: player is on right wall
        else if (LocalGravity == Vector3.right)
        {
            //adjacent is floor, case: on left seam & moving left
            if (y < -2.75 && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))) {
                return Vector3.down;
            }
            //adjacent is ceiling, case: on right seam & moving right
            if (y > 2.75 && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))) {
                return Vector3.up;
            }
        }

        return Vector3.zero;
    }

    public void FlipToWall(Vector3 newGravity, Vector3 oldGravity)
    {
        Vector3 newPosition = transform.position;
        Quaternion newRotation = Quaternion.identity;

        //rotate & position, case: player flips to floor
        if (newGravity == Vector3.down)
        {
            newRotation = Quaternion.Euler(0, 0, 0);
            newPosition.y = -3.75f + 0.5f;

            //coming off left wall
            if (oldGravity == Vector3.left) {
                newPosition.x = -3.75f + 0.5f;
            }
            //coming off right wall
            else if (oldGravity == Vector3.right) {
                newPosition.x = 3.75f - 0.5f;
            }
        }
        //rotate & position, case: player flips to ceiling
        else if (newGravity == Vector3.up)
        {
            newRotation = Quaternion.Euler(0, 0, 180);
            newPosition.y = 3.75f - 0.5f;

            ///coming off left wall
            if (oldGravity == Vector3.left) {
                newPosition.x = -3.75f + 0.5f;
            }
            //coming off right wall
            else if (oldGravity == Vector3.right) {
                newPosition.x = 3.75f - 0.5f;
            }
        }
        //rotate & position, case: player flips to left wall
        else if (newGravity == Vector3.left)
        {
            newRotation = Quaternion.Euler(0, 0, 270);
            newPosition.x = -3.75f + 0.5f;

            //coming off floor
            if (oldGravity == Vector3.down) {
                newPosition.y = -3.75f + 0.5f;
            }
            //coming off ceiling
            else if (oldGravity == Vector3.up) {
                newPosition.y = 3.75f - 0.5f;
            }
        }
        //rotate & position, case: player flips to right wall
        else if (newGravity == Vector3.right)
        {
            newRotation = Quaternion.Euler(0, 0, 90);
            newPosition.x = 3.75f - 0.5f;

            //coming off floor
            if (oldGravity == Vector3.down) {
                newPosition.y = -3.75f + 0.5f;
            }
            //coming off ceiling
            else if (oldGravity == Vector3.up) {
                newPosition.y = 3.75f - 0.5f;
            }
        }

        transform.rotation = newRotation;
        transform.position = newPosition;
    }
}
