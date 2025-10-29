using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator ani;
    ParticleSystem particleSystem;

    public Color gold;
    public Color cyan;
    
    private Camera playerCamera;
    public Camera PlayerCamera {
        get {return playerCamera;}
        set {playerCamera = value;}
    }

    private bool jump;
    public bool Jump {
        get {return jump;}
        set {jump = value;}
    }

    private Vector3 localGravity;
    public Vector3 LocalGravity {
        get {return localGravity;}
        set {localGravity = value;}
    }

    private float vertical;
    public float Vertical {
        get {return vertical;}
        set {vertical = value;}
    }

    private float speed;
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private CharacterController controller;
    public CharacterController Controller {
        get {return controller;}
        set {controller = value;}
    }

    private Vector3 move;
    public Vector3 Move {
        get {return move;}
        set {move = value;}
    }

    void Start() {
        Jump = false;
        Speed = 10f;
        LocalGravity = Vector3.down;

        Controller = GetComponent<CharacterController>();

        ani = GetComponent<Animator>();
        particleSystem = GameObject.FindWithTag("particles").GetComponent<ParticleSystem>();

        PlayerCamera = Camera.main;
        PlayerCamera.GetComponent<ThirdPersonCamera>().Target = this;
    }

    void Update()
    {
        move = Vector3.zero;

        //fell far enough, end game
        if (GetComponent<PlayerState>().IsFalling(LocalGravity, 10f)) {
            FindObjectOfType<ScoreManager>().End();
        }

        //don't allow inputs & flip if falling
        if (!GetComponent<PlayerState>().IsFalling(LocalGravity, 6f)) {
            Inputs();
            Flips();
        }

        Gravity();  //allows apply gravity
        Animations();
        
        Controller.Move(move);
    }

    public void Inputs() {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && GetComponent<PlayerState>().IsGrounded(LocalGravity))
        {
            Jump = true;
        }

        float lateral = 1f;
        
        //invert left-right movement, case: player is on ceiling or left wall
        if (LocalGravity == Vector3.up || LocalGravity == Vector3.left) {
            lateral = -1f;
        }

        //left-right movement, case: player is upright
        if ((LocalGravity == Vector3.down || LocalGravity == Vector3.up)) {
            float x = Input.GetAxis("Horizontal") * 5f * Time.deltaTime;
            move = new Vector3(lateral * x, 0f, Speed * Time.deltaTime);
        }
        //left-right movement, case: player is sideways
        else if ((LocalGravity == Vector3.left || LocalGravity == Vector3.right)) {
            float y = Input.GetAxis("Horizontal") * 5f * Time.deltaTime;
            move = new Vector3(0f, lateral * y, Speed * Time.deltaTime);
        }
    }

    public void Flips() {
        Vector3 wall = GetComponent<PlayerWalls>().GetAdjacentWall(LocalGravity);
        if (wall != Vector3.zero)
        {
            Debug.Log("flipping onto " + wall);
            LocalGravity = wall;
            
            GetComponent<PlayerWalls>().FlipToWall(wall, LocalGravity);

            Vertical = 0f;
            Jump = false;
        }
    }

    public void Gravity() {
        float longitudinal = 1f;
        
        //invert up-down movement, case: player is on floor or left wall
        if (LocalGravity == Vector3.left || LocalGravity == Vector3.down) {
            longitudinal = -1f;
        }

        Vertical += 20f * longitudinal * Time.deltaTime;

        if (Jump && GetComponent<PlayerState>().IsGrounded(LocalGravity))
        {
            Debug.Log("grounded");
            ani.SetTrigger("jump");
            //jump, case: player is on floor or left wall
            if (LocalGravity == Vector3.down || LocalGravity == Vector3.left) {
                Vertical = 12f;
            }
            //jump, case: player is on ceiling or right wall
            else if (LocalGravity == Vector3.up || LocalGravity == Vector3.right) {
                Vertical = -12f;
            }

            Jump = false;
        }

        //jump & gravity, case: player is upright
        if (LocalGravity == Vector3.down || LocalGravity == Vector3.up) {
            move.y += Vertical * Time.deltaTime;
        }
        //jump & gravity, case: player is sideways
        else if (LocalGravity == Vector3.left || LocalGravity == Vector3.right) {
            move.x += Vertical * Time.deltaTime;
        }
    }

    public void Animations() {
        if (GetComponent<PlayerState>().IsGrounded(LocalGravity)) {
            ani.SetBool("jump", false);
            ani.SetBool("run", true);
        }
        else {
            ani.SetBool("run", false);
            ani.SetBool("jump", true);
        }
    }

    public Vector3 GetLocalGravity() {
        return LocalGravity;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("coin")) {
            Debug.Log("scored");

            FindObjectOfType<ScoreManager>().AddScore(1);

            var main = particleSystem.main;
            main.startColor = gold;
            particleSystem.Play();
            Invoke("CleanParticles", 0.5f);

            Destroy(other.gameObject);
        }
        if (other.gameObject.tag.Equals("powerup")) {
            Debug.Log("power up");
            
            Speed = Mathf.Clamp(Speed * 1.5f, 10f, 25f);

            var main = particleSystem.main;
            main.startColor = cyan;
            particleSystem.Play();
            Invoke("CleanParticles", 0.5f);

            Destroy(other.gameObject);
        }
    }

    public void LevelChange() {
        Speed = 10f;
    }

    public void CleanParticles() {
        particleSystem.Stop();
        particleSystem.Clear();
    }
}