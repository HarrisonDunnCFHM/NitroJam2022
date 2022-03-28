using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    //config params
    [SerializeField] Light screenGlow;
    
    //cached refs
    Animator myAnimator;
    SpriteRenderer myRenderer;
    ViewerManager viewerManager;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        viewerManager = FindObjectOfType<ViewerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        myRenderer.material.color = screenGlow.color;
    }

    public void Chomp()
    {
        myAnimator.Play("Mouth Chomp");
    }

    public void EatHalf()
    {
        float removeViewers = Mathf.Ceil((viewerManager.allViewers.Count + 1)/ 2);
        StartCoroutine(RemoveViewers(removeViewers));
    }

    IEnumerator RemoveViewers(float viewersToRemove)
    {
        if (viewerManager.allViewers.Count < viewersToRemove || viewersToRemove == 0) { Debug.Log("not enough to eat"); yield return null; }
        else if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Mouth Chomp")) { Debug.Log("ALREADY EATING"); yield return null; }
        else
        {
            Chomp();
            yield return new WaitForSeconds(0.75f);
            for (int i = 0; i < viewersToRemove; i++)
            {
                Viewer viewerToRemove = viewerManager.allViewers[0];
                viewerManager.allViewers.Remove(viewerToRemove);
                StartCoroutine(viewerToRemove.Splat());
            }
            foreach(Viewer viewer in viewerManager.allViewers)
            {
                viewer.moving = true;
            }
        }
    }
}
