using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObjectScript : MonoBehaviour
{
   public GameObject player;
   public GameObject gravyGun;
   public GameObject terrain;
   public Camera camera;
   
   public int distance = 100;
   public float pullForce = 10f;
   public float pushForce = 50f;
   private float stopDistance = 5f;
   RaycastHit raycast;
   private float timeCount=0;
   private int click = 1;
   public float distanceCheck;
   private GameObject gravObj;
   
    // Update is called once per frame
    void FixedUpdate()
    {
        PushObject();
        PullObject();
        StartCoroutine("dropObject",0);
    }

    void PullObject()
    {
        Vector3 fwd = camera.transform.forward;
        
       
        gravObj = GameObject.Find(PlayerPrefs.GetString("PulledObj"));

        
        distanceCheck = Vector3.Distance(gravObj.transform.position,camera.transform.position);

        //Pull Object   
        if(Physics.Raycast (camera.transform.position, fwd, out raycast, distance))
        {
            //Debug.Log(raycast.distance);
            if(raycast.transform.tag == "Object")
            {
                if(click==1 & Input.GetMouseButton(1))
                {
                    PlayerPrefs.SetString("PulledObj", raycast.transform.name);
                    raycast.transform.GetComponent<Rigidbody>().AddForce((camera.transform.position-raycast.transform.position).normalized*pullForce/50,ForceMode.VelocityChange); 
                    if(distanceCheck<stopDistance)
                    {
                        raycast.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        raycast.transform.GetComponent<Rigidbody>().Sleep();
                        raycast.transform.GetComponent<Rigidbody>().useGravity=false;
                        raycast.transform.SetParent(gravyGun.transform);
                        //this.raycast.transform.rotation = Quaternion.Slerp(raycast.transform.rotation, Quaternion.LookRotation(camera.transform.position - raycast.transform.position), 100f * Time.deltaTime);
                        click=2; 
                    }                      
                }
            } 
            else
            {
                timeCount+=Time.deltaTime;
                if(timeCount>=1)
                {
                    gravObj.transform.GetComponent<Rigidbody>().useGravity=true;
                    gravObj.transform.parent=null;   
                    click=1;
                    timeCount=0;
                }
            }   
        } 
    }

    void PushObject()
    {
        
        //Vector3 fwd = camera.transform.forward;
        if(Physics.Raycast (camera.transform.position, camera.transform.forward, out raycast, distance*0.7f) && raycast.transform.tag == "Object" && Input.GetMouseButtonDown(0))
        {
            raycast.transform.GetComponent<Rigidbody>().AddForce(camera.transform.forward*pushForce*100); 
            raycast.transform.GetComponent<Rigidbody>().useGravity=true;
            raycast.transform.parent=null;
            Debug.Log("PUSHED!");
            click=1; 
        }
        
    }
            //Drop object if close
            public IEnumerator dropObject()
            {
                if(click==2 & gravObj.transform.gameObject.GetComponent<Rigidbody>().useGravity==false & Input.GetMouseButtonDown(1))
                {
                    gravObj.transform.gameObject.GetComponent<Rigidbody>().useGravity=true;
                    gravObj.transform.gameObject.transform.parent=null;
                    //Debug.Log("Canceled");
                    yield return new WaitForSeconds(1);
                    click=1;
                    

                }
            }
        
        
    }

