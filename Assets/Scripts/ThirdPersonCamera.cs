using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public PlayerMovement Target;

    void Update()
    {
        if (Target == null) {
            return;
        }

        Vector3 localGravity = Target.GetLocalGravity();
        Vector3 offset = new Vector3(0, 2, -5);

        if (localGravity == Vector3.down) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (localGravity == Vector3.up) {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            offset = new Vector3(0, -2, -5);
        }
        else if (localGravity == Vector3.left) {
            transform.rotation = Quaternion.Euler(0, 0, 270);
            offset = new Vector3(2, 0, -5);
        }
        else if (localGravity == Vector3.right) {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            offset = new Vector3(-2, 0, -5);
        }

        transform.position = Target.transform.position + offset;
    }
}

