using UnityEngine;
public class DoorCloseTrigger : MonoBehaviour
{
    public DoorController door;

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            door.CloseDoor();
    }
}