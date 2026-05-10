using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject door;
    Animator anim;
    [SerializeField] float maxAperture = 4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = door.GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            anim.SetTrigger("open");
    }

    /*void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            anim.SetTrigger("close");
    }*/
}
