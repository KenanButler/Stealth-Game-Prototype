using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_script : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bulletcounttext;
    [SerializeField] TextMeshProUGUI bombcounttext;
    [SerializeField] TextMeshProUGUI healthtext;
    [SerializeField]
    private GameObject player;


    private TankBody body;
    


    private void Start()
    {
       
        body = player.GetComponent<TankBody>();

    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(body.currentbullet);
        bulletcounttext.text = "bullets: " + body.currentbullet.ToString() + "/"+body.maxbullet; 
        bombcounttext.text = "bombs: " + body.currentbomb.ToString() + "/"+body.maxbomb; 
        healthtext.text = "Health: "+body.health.ToString() + "/3";
    }
}
