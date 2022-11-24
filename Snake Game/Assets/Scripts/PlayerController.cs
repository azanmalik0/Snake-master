using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    Rigidbody rb;
    public float speedSnake;
    //public float rotationSpeed;
    private Touch touch;
    float speedmodifier;
    public GameObject snakeBodyPrefab;
    //public Transform currenBodyPart;
    //public Transform previousBodyPart;
    //private float Dis;
    //public float minDistance=0.95f;
    public int Gap = 10;
    public float bodyPartSpeed = 5;
    public List<GameObject> bodyParts=new List<GameObject> ();
    public List<Vector3> posHistory=new List<Vector3> ();
    public GameObject gameoverPanel;
    


    private void Awake()
    {
        
    }

    void Start()
    {
        gameoverPanel.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        speedmodifier = 0.08f;
        //bodyParts.Add(this.transform);
        GrowBody();
        GrowBody();
        GrowBody();
        GrowBody();
       
        
    }

    
    void Update()
    {
        transform.position += Vector3.forward * speedSnake * Time.deltaTime;


        SwipeCheck();

        //float currentSpeed=speedSnake;
        //bodyParts[0].Translate(bodyParts[0].forward * currentSpeed * Time.smoothDeltaTime, Space.World);
        //rb.velocity = Vector3.forward * speedSnake * Time.deltaTime;

        //for (int i = 1; i < bodyParts.Count; i++)
        //{
        //    currenBodyPart = bodyParts[i];
        //    previousBodyPart = bodyParts[i - 1];

        //    Dis = Vector3.Distance(previousBodyPart.position, currenBodyPart.position);
        //    Vector3 newPos = previousBodyPart.position;
        //    // Vector3 newPos = new Vector3(previousBodyPart.transform.position.x, previousBodyPart.transform.position.y, previousBodyPart.transform.position.z - 5);

        //    float T = Time.deltaTime * Dis / minDistance * currentSpeed;
        //     //newPos.y = bodyParts[0].position.y;

        //    currenBodyPart.position = Vector3.Slerp(currenBodyPart.position, newPos, T);

        //}


    //---Storing Position History---


        posHistory.Insert(0, new Vector3(transform.position.x, transform.position.y, transform.position.z-5));

    //-----------Snakelike Movement------------------------



        int index = 0;
        foreach (var Part in bodyParts)
        {
            Vector3 point = posHistory[Mathf.Min(index * Gap, posHistory.Count - 1)];
        //  Part.transform.position = point;
            Vector3 moveDirection = point - Part.transform.position;
            Part.transform.position += moveDirection * Time.deltaTime * bodyPartSpeed;
        //  bodyPart.transform.LookAt(point);
            index++;
            


        }
        

        

    }

    //----------------Swipe Controls---------------------------


    void SwipeCheck()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {


                transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * speedmodifier, transform.position.y, transform.position.z);




            }
            if (transform.position.x > 23.8f)
            {
                transform.position = new Vector3(23.8f, transform.position.y, transform.position.z);
            }
            if (transform.position.x < -23.8f)
            {
                transform.position = new Vector3(-23.8f, transform.position.y, transform.position.z);
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Fruit"))
        {
            
            Destroy(other.gameObject);
            GrowBody();
            speedSnake += 5;
            bodyPartSpeed += 1;
            
            
        }
        if (other.CompareTag("Explosive"))
        {
           
            Destroy(other.gameObject);
            Destroy(bodyParts[bodyParts.Count - 1].gameObject);
            bodyParts.Remove(bodyParts[bodyParts.Count-1]);
            speedSnake -= 5;
            bodyPartSpeed += 2;


        }
        if (other.CompareTag("Endgame"))
        {
           
           
            Time.timeScale = 0;
            gameoverPanel.gameObject.SetActive(true);
           this.rb.isKinematic =false;


        }

    }
   
    void GrowBody()
    {

        //Transform part = (Instantiate(snakeBodyPrefab,new Vector3(bodyParts[bodyParts.Count - 1].position.x, bodyParts[bodyParts.Count - 1].position.y, bodyParts[bodyParts.Count - 1].position.z) , transform.rotation)as GameObject).transform;

        ////Transform part = (Instantiate(snakeBodyPrefab, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;
        //part.SetParent(transform);
        //bodyParts.Add(part.transform);

        GameObject part = Instantiate(snakeBodyPrefab,new Vector3(transform.position.x,transform.position.y,transform.position.z),transform.rotation);
        //GameObject part = Instantiate(snakeBodyPrefab);
        bodyParts.Add(part);
        
        
        
            
        
    }
}
