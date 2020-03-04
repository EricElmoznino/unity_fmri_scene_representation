using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollision : MonoBehaviour
{
    private Logger logger;

    private void Start()
    {
        logger = GameObject.FindGameObjectWithTag("GameController").GetComponent<Logger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ExitDoor")
        {
            Transform door = other.gameObject.GetComponent<Exit>().entryDoor;
            Vector3 door_loc = door.position;
            Vector3 door_normal = door.rotation * Vector3.up;
            float player_radius = gameObject.GetComponent<CapsuleCollider>().radius;
            Vector3 teleport = door_loc + door_normal * player_radius;
            teleport.y = gameObject.transform.position.y;
            gameObject.transform.position = teleport;
            logger.room = door.parent.gameObject;
        }
    }
}
