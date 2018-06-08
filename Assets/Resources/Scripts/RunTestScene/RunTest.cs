using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTest : MonoBehaviour
{
    public Animator playerAnim;
    public GameObject tilePrefab;

    Queue<GameObject> tileInstances = new Queue<GameObject>();
    int count = 0;

    private void Start()
    {
        tileInstances.Enqueue(Instantiate(tilePrefab, new Vector3(0.0f, 0.0f), Quaternion.identity));
        tileInstances.Enqueue(Instantiate(tilePrefab, new Vector3(6.0f, 0.0f), Quaternion.identity));
        tileInstances.Enqueue(Instantiate(tilePrefab, new Vector3(12.0f, 0.0f), Quaternion.identity));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            count++;
            Debug.Log("count increased. count = " + count);
        }

        if(count > 0)
        {
            if(playerAnim.GetBool("isRunning") == false)
            {
                StartCoroutine("Run");
                count--;
                Debug.Log("count decreased. count = " + count);
            }
        }
    }

    IEnumerator Run()
    {
        playerAnim.SetBool("isRunning", true);
        StartCoroutine("MoveTile");
        yield return new WaitForSeconds(0.2f);

        playerAnim.SetBool("isRunning", false);
        yield return null;
    }

    IEnumerator MoveTile()
    {
        float frame = 1.0f / 40.0f;
        float duration = 0.2f;

        for(int i = 0; i < duration / frame; i++)
        {
            foreach (GameObject item in tileInstances)
            {
                item.transform.position += new Vector3(1 / duration * -1, 0.0f, 0.0f) * frame;
            }

            AddTile();
            yield return new WaitForSeconds(frame);
        }
    }

    void AddTile()
    {
        if (tileInstances.Peek().transform.position.x <= -6.0f)
        {
            Destroy(tileInstances.Dequeue());
            tileInstances.Enqueue(Instantiate(tilePrefab, new Vector3(12.0f, 0.0f), Quaternion.identity));
        }
    }

    public void OnMoveButtonClick()
    {
        count++;
        Debug.Log("count increased. count = " + count);
    }
}