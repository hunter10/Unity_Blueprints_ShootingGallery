using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : MonoBehaviour {

    private bool beenHit = false;
    private Animator animator;
    private GameObject parent;

    private bool activated;
    private Vector3 originalPos;


    void Awake()
    {
        GameController._instance.targets.Add(this);    
    }


	void Start () {

        parent = transform.parent.gameObject;
        animator = parent.GetComponent<Animator>();
        originalPos = parent.transform.position;

        //ShowTarget(); // 테스트 용 
	}
	
	void OnMouseDown()
    {
        if(!beenHit)
        {
            GameController._instance.IncreaseScore();

            beenHit = true;
            animator.Play("Flip");

            StopAllCoroutines();

            StartCoroutine(HideTarget());
        }
    }

    public void ShowTarget()
    {
        if(!activated)
        {
            activated = true;
            beenHit = false;
            animator.Play("Idle");

            iTween.MoveBy(parent, iTween.Hash("y", 1.4, 
                                              "easeType", "easeInOutExpo", 
                                              "time", 0.5,
                                              "oncomplete", "OnShown",
                                              "oncompletetarget", gameObject
                                             ));

        }
    }


    public IEnumerator HideTarget()
    {
        yield return new WaitForSeconds(.5f);
        iTween.MoveBy(parent, iTween.Hash("y", originalPos.y - parent.transform.position.y, 
                                          "easeType", "easeOutQuead", 
                                          "loopType", "none", 
                                          "time", 0.5, 
                                          "oncomplete", "OnHidden",
                                          "oncompletetarget", gameObject
                                         ));
    }

    void OnHidden()
    {
        parent.transform.position = originalPos;
        activated = false;
    }



    void OnShown()
    {
        StartCoroutine("MoveTarget");
    }


    public float moveSpeed = 1f;    // x 축에서 얼마나 빠르게 움직이는가
    public float frequency = 5f;    // 사인 움직임의 속도
    public float magnitude = 0.1f;  // 사인 움직임의 크기
    IEnumerator MoveTarget()
    {
        var relativeEndPos = parent.transform.position;

        if(transform.eulerAngles == Vector3.zero)
        {
            relativeEndPos.x = 6;
        }
        else{
            relativeEndPos.x = -6;
        }

        var movementTime = Vector3.Distance(parent.transform.position, relativeEndPos) * moveSpeed;
        var pos = parent.transform.position;
        var time = 0f;

        while(time < movementTime)
        {
            time += Time.deltaTime;
            pos += parent.transform.right * Time.deltaTime * moveSpeed;
            parent.transform.position = pos + (parent.transform.up * Mathf.Sin(Time.time * frequency) * magnitude);

            yield return new WaitForSeconds(0);
        }

        StartCoroutine(HideTarget());
    }

}
