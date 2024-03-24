using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [HideInInspector] public Vector3 moveVector;
    [HideInInspector] public float verticalVelocity;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public int currentLane;

    public float distanceInBetweenLanes = 3.0f;
    public float baseRunSpeed = 7.0f;
    public float baseSidewaySpeed = 12.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;

    public CharacterController controller;
    public Animator anim;
    public AudioClip hitSound;


    private BaseState state;
    private bool isPaused;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        state = GetComponent<RunningState>();
        state.Construct();

        isPaused = true;

        InvokeRepeating("AutoSpeed", 20.0f, 20.0f);
    }


    private void AutoSpeed()
    {
        if (baseRunSpeed != 15)
        {
            baseRunSpeed += 0.5f;
        }
        else
        {
            CancelInvoke("AutoSpeed");
        }
    }

    private void Update()
    {
        if(!isPaused)
            UpdateMotor();
    }
    private void UpdateMotor()
    {
        // Check if were grounded
        isGrounded = controller.isGrounded;

        // How should we be moving right now? based on states
        moveVector = state.ProcessMotion();

        //Are we trying to change state?
        state.Transition();

        //Feed our animator some values
        anim?.SetBool("IsGrounded", isGrounded);
        anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));

        //Move the player
        controller.Move(moveVector * Time.deltaTime);
    }
    public float SnapToLane()
    {
        float r = 0.0f;

        if (transform.position.x != (currentLane * distanceInBetweenLanes)) //
        {
            float deltaToDesirePosition = (currentLane * distanceInBetweenLanes) - transform.position.x;
            r = (deltaToDesirePosition > 0) ? 1 : -1;
            r *= baseSidewaySpeed;

            float actualDistance = r * Time.deltaTime;
            if (Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesirePosition))
                r = deltaToDesirePosition * (1 / Time.deltaTime);
        }
        else
        {
            r = 0;
        }

        return r;

    }

    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }
    public void ChangeState(BaseState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }
    public void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if (verticalVelocity < -terminalVelocity)
            verticalVelocity = -terminalVelocity;
    }

    public void PausePlayer()
    {
        isPaused = true;
    }
    public void ResumePlayer()
    {
        isPaused = false;
    }
    public void RespawnPlayer()
    {
        ChangeState(GetComponent<PlayerRespawnState>());
        GameManager.Instance.ChangeCamera(GameCamera.Respawn);

    }
    public void ResetPlayer()
    {
        currentLane = 0;
        transform.position = Vector3.zero;
        anim?.SetTrigger("Idle");
        PausePlayer();
        ChangeState(GetComponent<RunningState>());
    }


    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);
        if (hitLayerName == "Death")
        {
            AudioManagerReal.Instance.PlaySFX(hitSound, 0.10f);
            ChangeState(GetComponent<DeathState>());

        }
    }
}
