using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMover : MonoBehaviour {
    private enum MovementMode { radialMove, linearMove };
    private enum MovementOrder { fromFirst2Last, randomly};

    [SerializeField] private GameObject target;
    [SerializeField] private Renderer targetRenderer;

    /*[SerializeField]*/ private Vector3[] pos = {
        //            alfa    beta   dist
        new Vector3 (  0.0f,   0.0f, 1.0f ),        // 1++
        new Vector3 (  5.0f,  45.0f, 2.0f ),        // 4++
        new Vector3 ( 10.0f, 315.0f, 4.0f ),        //26++
        new Vector3 (  5.0f, 135.0f, 5.0f ),        //14++
        new Vector3 ( 20.0f, 225.0f, 3.0f ),        //32++
        new Vector3 ( 10.0f,  45.0f, 4.0f ),        //20++
        new Vector3 (  5.0f, 270.0f, 2.0f ),        // 9++
        new Vector3 ( 30.0f, 135.0f, 5.0f ),        //36++
        new Vector3 ( 10.0f,  90.0f, 4.0f ),        //21++
        new Vector3 (  5.0f, 180.0f, 2.0f ),        // 7++
        new Vector3 ( 20.0f, 270.0f, 3.0f ),        //33++
        new Vector3 ( 10.0f, 225.0f, 4.0f ),        //24++
        new Vector3 (  5.0f, 315.0f, 5.0f ),        //18++
        new Vector3 (  5.0f,   0.0f, 5.0f ),        //11++
        new Vector3 ( 30.0f,  45.0f, 5.0f ),        //35++
        new Vector3 ( 20.0f,   0.0f, 3.0f ),        //27++
        new Vector3 (  5.0f, 180.0f, 5.0f ),        //15++
        new Vector3 (  5.0f, 225.0f, 2.0f ),        // 8++
        new Vector3 ( 10.0f, 270.0f, 4.0f ),        //25++
        new Vector3 ( 30.0f, 225.0f, 5.0f ),        //37++
        new Vector3 (  5.0f,  45.0f, 5.0f ),        //12++
        new Vector3 ( 10.0f,   0.0f, 4.0f ),        //19++
        new Vector3 (  0.0f,   0.0f, 5.0f ),        // 2++
        new Vector3 ( 20.0f,  45.0f, 3.0f ),        //28++
        new Vector3 (  5.0f,  90.0f, 2.0f ),        // 5++
        new Vector3 ( 10.0f, 180.0f, 4.0f ),        //23++
        new Vector3 (  5.0f, 315.0f, 2.0f ),        //10++
        new Vector3 (  5.0f, 270.0f, 5.0f ),        //17++
        new Vector3 ( 20.0f, 315.0f, 3.0f ),        //34++
        new Vector3 ( 20.0f,  90.0f, 3.0f ),        //29++
        new Vector3 (  5.0f,   0.0f, 2.0f ),        // 3++
        new Vector3 ( 10.0f, 135.0f, 4.0f ),        //22++
        new Vector3 ( 20.0f, 135.0f, 3.0f ),        //30++
        new Vector3 (  5.0f, 225.0f, 5.0f ),        //16++
        new Vector3 ( 20.0f, 180.0f, 3.0f ),        //31++
        new Vector3 (  5.0f,  90.0f, 5.0f ),        //13++
        new Vector3 ( 30.0f, 315.0f, 5.0f ),        //38++
        new Vector3 (  5.0f, 135.0f, 2.0f ),        // 6++
    };
    [SerializeField] private int nextPos = 0, curPos = 0;
    [SerializeField] private float moveTime = 0.5f, stableTime = 2.0f;
    [SerializeField] private float distanceFactor = 0.01f, angleFactor = 0.01f;
    [SerializeField] private Color stayColor, moveColor;
    [SerializeField] private MovementMode movementMode;
    [SerializeField] private MovementOrder movementOrder;

    [Tooltip("Define key for the test start")]
    [SerializeField] private KeyCode keyStart = KeyCode.A;
    [Tooltip("Define key for the test stop")]
    [SerializeField] private KeyCode keyStop = KeyCode.S;
    private Coroutine currentCouroutine = null;
    private bool measurementOngoing = false;

	// Use this for initialization
	void Start () {
        curPos = nextPos = 0;
        currentCouroutine = StartCoroutine("MoveToNewPos", true);
	}

    // Update is called once per frame
    void Update()
    {
        ReadKeys();
    }

    #region read user input
    private void ReadKeys()
    {
        if (Input.GetKeyDown(keyStart) && currentCouroutine == null)
        {
            GameObject.FindObjectOfType<FileLogger>().printProgress("Start_key_pressed\t" + keyStart.ToString(), true);
            curPos = nextPos = 0;
            currentCouroutine = StartCoroutine("MoveToNewPos", false);
        }
        if (Input.GetKeyDown(keyStop) && currentCouroutine != null)
        {
            GameObject.FindObjectOfType<FileLogger>().printProgress("Stop_key_pressed\t" + keyStop.ToString());
            if (currentCouroutine != null)
                StopCoroutine(currentCouroutine);
            if (measurementOngoing)
                markMeasurement(false);
            nextPos = 0;
            currentCouroutine = StartCoroutine("MoveToNewPos", true);
        }
    }
    #endregion
    #region support functions
    // calculate position, versio 1
    private Vector3 calculatePos3(float angle_a, float angle_b, float radi)
    {
        return calculatePos3(new Vector3(angle_a, angle_b, radi));
    }

    // calculate position, version for vector3()
    private Vector3 calculatePos3(Vector3 nextPos)
    {
        Vector3 result;
        float rsina;
        rsina = nextPos.z * Mathf.Sin(Mathf.Deg2Rad * nextPos.x);
        result.z = nextPos.z * Mathf.Cos(Mathf.Deg2Rad * nextPos.x);
        result.y = rsina * Mathf.Sin(Mathf.Deg2Rad * nextPos.y);
        result.x = rsina * Mathf.Cos(Mathf.Deg2Rad * nextPos.y);
        return result;
    }


    //
    float ArcLength(Vector3 startPos, Vector3 endPos, int numSegments)
    {
        float length = 0.0f;
        float lerpAmount = 1.0f / numSegments;
        float lerp = lerpAmount;
        for (int i = 0; i < numSegments; i++)
        {
            length += Vector3.Distance(
              calculatePos3(new Vector3(Mathf.LerpAngle(startPos.x, endPos.x, lerp), Mathf.LerpAngle(startPos.y, endPos.y, lerp), Mathf.Lerp(startPos.z, endPos.z, lerp))),
              calculatePos3(new Vector3 (Mathf.LerpAngle(startPos.x, endPos.x, lerp - lerpAmount), Mathf.LerpAngle(startPos.y, endPos.y, lerp - lerpAmount), Mathf.Lerp(startPos.z, endPos.z, lerp - lerpAmount)))
              );
            lerp += lerpAmount;
        }
        return length;
    }

    private void markMeasurement(bool ongoing)
    {
        if (ongoing)
        {
            measurementOngoing = true;
            GameObject.FindObjectOfType<FileLogger>().printProgress("Measure_start");
        }
        else
        {
            GameObject.FindObjectOfType<FileLogger>().printProgress("Measure_end");
            measurementOngoing = false;
        }
    }
    #endregion
    #region coroutines - a) move target ball and b) stable time wait

    // param oneShot: -> false for continuos operation, true for ending the movement sequency
    IEnumerator MoveToNewPos(bool oneShot)
    {
        Vector3 start = target.transform.localPosition;
        Vector3 destination = calculatePos3(pos[nextPos]);
        float runningTime = 0;
        //Debug.Log("MoveToNewPos() from:" + start + ", to:" +destination + "("+pos[nextPos]+ "), curPos ="+curPos+", nextPos=" + nextPos + ", target"+target);
        targetRenderer.material.color = moveColor;
        float distanceAddTime = ArcLength(pos[curPos], pos[nextPos], 6) * distanceFactor;
        float thisMoveTime = moveTime + distanceAddTime;
        while (runningTime < thisMoveTime)
        {
            runningTime += Time.deltaTime;
            switch (movementMode)
            {
                case MovementMode.radialMove:
                    target.transform.localPosition = calculatePos3(new Vector3(
                        Mathf.LerpAngle(pos[curPos].x, pos[nextPos].x, runningTime / thisMoveTime), 
                        Mathf.LerpAngle(pos[curPos].y, pos[nextPos].y, runningTime / thisMoveTime), 
                        Mathf.Lerp(pos[curPos].z, pos[nextPos].z, runningTime / thisMoveTime)));
                    break;
                case MovementMode.linearMove:
                    target.transform.localPosition = Vector3.Lerp(start, destination, runningTime / thisMoveTime);
                    break;
                default:
                    Debug.LogError("MoveToNewPos() - movementMode error");
                    break;
            }
            yield return null;
        }
        target.transform.localPosition = destination;
        GameObject.FindObjectOfType<FileLogger>().printProgress("New_position_xyz\t" + destination.x + "\t" + destination.y + "\t" + destination.z);
        GameObject.FindObjectOfType<FileLogger>().printProgress("New_position_abr\t" + pos[nextPos].x + "\t" + pos[nextPos].y + "\t" + pos[nextPos].z);
        GameObject.FindObjectOfType<FileLogger>().printProgress("New_position_index\t" + nextPos);
        if (!oneShot)
            currentCouroutine = StartCoroutine("WaitForStableTime");
        else
            currentCouroutine = null;
    }

    IEnumerator WaitForStableTime()
    {
        targetRenderer.material.color = stayColor;
        curPos = nextPos;
        markMeasurement(true);
        yield return new WaitForSeconds(stableTime);
        markMeasurement(false);
        switch (movementOrder)
        {
            case MovementOrder.fromFirst2Last:
                nextPos++;
                if (nextPos >= pos.Length)
                {
                    nextPos = 0;
                    currentCouroutine = StartCoroutine("MoveToNewPos", true);
                }
                else
                    currentCouroutine = StartCoroutine("MoveToNewPos", false);
                break;
            case MovementOrder.randomly:
                nextPos = Random.Range(0, pos.Length);
                currentCouroutine = StartCoroutine("MoveToNewPos", false);
                break;
            default:
                currentCouroutine = null;           /* error */
                break;
        }
    }
    #endregion
}