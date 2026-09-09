using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class button : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject Button;

    private door doorscp;
    
   
    void Start()
    {
        doorscp = door.GetComponent<door>();
        LevelManager.Instance.RegisterButton(this);
    }
    private void hit()
    {
        Button.SetActive(false);
    }
    

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            hit();
            doorscp.opendoor();
            
        }
    }

    public void Reset()
    {
        Button.SetActive(true);
        doorscp.closedoor();
    }
}
