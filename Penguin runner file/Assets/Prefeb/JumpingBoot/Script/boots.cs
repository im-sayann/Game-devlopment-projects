using UnityEngine;

public class boots : MonoBehaviour
{
    private Animator anim;

    private JumpingState jumpState;
    public float bootsTime;
    public float bootsJumpForce;
    

    private void Start()
    {
        anim = GetComponentInParent<Animator>();

        jumpState = FindObjectOfType<JumpingState>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Workinggggggggggggggggggggggggg");
        if (other.CompareTag("Player"))
        {
            SphereCollider sphereCollider = other.GetComponent<SphereCollider>();
            PickupBoots();
        }
    }


    private void PickupBoots()
    {

        Invoke("BootReset", bootsTime);
        
        jumpState.jumpForce = bootsJumpForce;
        gameObject.SetActive(false);



    }

    private void BootReset()
    {
        Debug.Log("Reset Done");
        jumpState.jumpForce = 15.31f;
    }

    public void OnShowChunk()
    {
        anim?.SetTrigger("Idle");

    }
}
