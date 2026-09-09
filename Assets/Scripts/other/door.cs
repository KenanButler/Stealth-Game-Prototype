using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour

{
    [SerializeField] GameObject doormesh;
    public void opendoor()
    {
        doormesh.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;
    }

    public void closedoor()
    {
        doormesh.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
    }
}
