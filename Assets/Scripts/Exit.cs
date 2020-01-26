using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Exit : MonoBehaviour
{
    public Transform entryDoor;

    private void Start()
    {
        Assert.AreEqual(transform.localRotation.y, -entryDoor.localRotation.y);
    }
}
