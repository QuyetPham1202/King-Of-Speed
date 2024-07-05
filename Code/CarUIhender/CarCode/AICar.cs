using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.Experimental.GlobalIllumination;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class AICar : MonoBehaviour
{
    public enum AiMode { followPlayer, followWaypoints, followMouse };
    [Header("AI settings")]
    public AiMode aiMode;
    public float waypointRadius = 0.5f;
    public float maxSpeed = 18f;
    public bool isAvoidingCars = true;
    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;
    private RaycastHit2D circleCastHit1;
    [Range(0.0f, 1.0f)]
    public float skillevel = 1.0f;
    float orignalMaximumSpead = 0;

    bool isRunningIngStuckCkeck = false;
    bool isFirstTemporaryWaypoint = false;
    int stuckCkeckCounter = 0;
    List<Vector2> temporarWaypoints = new List<Vector2>();
    float angleToTarget = 0;
    public float cellSize = 2.0f;
    Vector2 avoidanceVectorLerped = Vector3.zero;
    Mapcar currentMapcar = null;
    Mapcar previousWaypoint = null;
    List<Transform> waypointsTransforms;

    float initialSpeed = 0;
    PolygonCollider2D polygonCollider2D;
    int currentWaypointIndex = 0;
    Mapcar[] allMapPoint;

    TopDownCarControllider topDownCarControllider;
    AStarLine aStarLine;
    int carAndPlayerLayerMask = 0;

    void Awake()
    {
        topDownCarControllider = GetComponent<TopDownCarControllider>();
        allMapPoint = FindObjectsOfType<Mapcar>();
        aStarLine = GetComponent<AStarLine>();
        polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();
        orignalMaximumSpead = maxSpeed;
    }

    private void Start()
    {
        SetMaxSpeedBasedOnSkillLever(maxSpeed);
        initialSpeed = maxSpeed;

    }
    public void ResetPath()
    {
        currentWaypointIndex = 0;
        temporarWaypoints.Clear();
        isFirstTemporaryWaypoint = false;
        stuckCkeckCounter = 0;

        if (currentMapcar != null)
        {
            targetPosition = currentMapcar.transform.position;
        }
        else
        {

            currentMapcar = FindClosestWaypoint();
            if (currentMapcar != null)
            {
                targetPosition = currentMapcar.transform.position;
            }
        }
    }
    void FixedUpdate()
    {
        if (GameManager.instance.GetGameState() == GameStates.countDown)
            return;
        Vector2 inputVector = Vector2.zero;

        switch (aiMode)
        {
            case AiMode.followPlayer:
                FollowPlayer();
                break;
            case AiMode.followWaypoints:
                if (temporarWaypoints.Count == 0)
                    FollowWaypoints();
                else FollowTemporarWayPoint();
                break;
            case AiMode.followMouse:
                FollowMousePosition();
                break;
        }

        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottleOrBrake(inputVector.x);



        if (topDownCarControllider.GetVelocityMagnitude() < 0.5f && Mathf.Abs(inputVector.y) > 0.01 && !isRunningIngStuckCkeck)
            StartCoroutine(StuckCheck());
        if (stuckCkeckCounter >= 4 && !isRunningIngStuckCkeck)
            StartCoroutine(StuckCheck());

        topDownCarControllider.SetInputVector(inputVector);


    }

    void FollowPlayer()
    {
        if (targetTransform == null)
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (targetTransform != null)
            targetPosition = targetTransform.position;
    }
    void FollowTemporarWayPoint()
    {
        targetPosition = temporarWaypoints[0] + new Vector2(cellSize / 2.0f, cellSize / 2.0f);
        float distanceToWayPoint = (targetPosition - transform.position).magnitude;
        float minDistanceToReachWaypoint1 = 4f;
        float currentMaxSpeed = Mathf.Lerp(10, maxSpeed, distanceToWayPoint / minDistanceToReachWaypoint1);
        SetMaxSpeedBasedOnSkillLever(currentMaxSpeed);

        float minDistanceToReachWaypoint = 1.5f;
        if (!isFirstTemporaryWaypoint)
        {
            minDistanceToReachWaypoint = 3.0f;

        }
        if (distanceToWayPoint <= minDistanceToReachWaypoint)
        {
            temporarWaypoints.RemoveAt(0);
            isFirstTemporaryWaypoint = false;
        }
    }

    void FollowWaypoints()
    {
        if (currentMapcar == null || currentWaypointIndex >= currentMapcar.nextWaypointNode.Length)
        {
            currentMapcar = FindClosestWaypoint();
            currentWaypointIndex = 0;
            previousWaypoint = currentMapcar;
        }

        if (currentMapcar != null && currentMapcar.nextWaypointNode.Length > 0)
        {

            targetPosition = currentMapcar.nextWaypointNode[currentWaypointIndex].transform.position;


            float distanceToWaypoint = (targetPosition - transform.position).magnitude;


            if (distanceToWaypoint <= currentMapcar.minDistanceToReachWaypoint)
            {

                currentWaypointIndex++;
            }
        }

    }
    void FollowMousePosition()
    {
        Vector3 worldPositon = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        targetPosition = worldPositon;
    }
    Mapcar FindClosestWaypoint()
    {
        Mapcar closestWaypoint = null;
        float closestDistance = float.MaxValue;


        foreach (Mapcar waypoint in allMapPoint)
        {
            if (waypoint.nextWaypointNode.Length > 0)
            {

                float distance = Vector3.Distance(transform.position, waypoint.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestWaypoint = waypoint;
                }
            }
        }

        return closestWaypoint;
    }
    float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;

        vectorToTarget.Normalize();

        if (isAvoidingCars)
            AvoidCars(vectorToTarget, out vectorToTarget);
        angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        float steerAmount = angleToTarget / 45.0f;

        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }
    float ApplyThrottleOrBrake(float inputX)
    {

        if (topDownCarControllider.GetVelocityMagnitude() > maxSpeed)
            return 0f;

        float reduceSeedDueToCornering = Mathf.Abs(inputX) / 1.0f;

        float throttle = 1.05f - reduceSeedDueToCornering * skillevel;
        if (temporarWaypoints.Count() != 0)
        {
            if (angleToTarget > 70)
                throttle = throttle * -1;
            else if (angleToTarget < -70)
                throttle = throttle * -1;
            else if (stuckCkeckCounter > 3)
                throttle = throttle * -1;
        }
        return throttle;
    }

    void SetMaxSpeedBasedOnSkillLever(float newSpeed)
    {
        maxSpeed = Mathf.Clamp(newSpeed, 0, orignalMaximumSpead);
        float skllBaseMaxiumSpeed = Mathf.Clamp(skillevel, 0.3f, 1.0f);
        maxSpeed = maxSpeed * skllBaseMaxiumSpeed;
    }

    Vector2 FindNearestPointOnline(Vector2 lineStartPotision, Vector2 lineEndPosition, Vector2 point)
    {
        Vector2 lineHeadingVector = (lineEndPosition - lineStartPotision);
        float maxDistance = lineHeadingVector.magnitude;
        lineHeadingVector.Normalize();

        Vector2 lineVectorStartToPoint = point - lineStartPotision;
        float dotPoroduct = Vector2.Dot(lineVectorStartToPoint, lineHeadingVector);

        dotPoroduct = Mathf.Clamp(dotPoroduct, 0f, maxDistance);
        return lineStartPotision + lineHeadingVector * dotPoroduct;
    }
    void AvoidCars(Vector2 vectorToTarget, out Vector2 newVectorToTarget)
    {
        if (IsCarsInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector))
        {
            Debug.DrawRay(transform.position + transform.up * 0.5f, transform.up * 12, Color.red);


            Vector2 avoidanceVector1 = Vector2.Reflect((position - transform.position).normalized, otherCarRightVector);
            Vector2 avoidanceVector = Vector2.zero;

            avoidanceVector = Vector2.Reflect((position - transform.position).normalized, otherCarRightVector);

            float distanceToTarget = (targetPosition - transform.position).magnitude;
            float driveToTargetInfluence = 6.0f / distanceToTarget;
            driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.3f, 1.0f);
            float avoidanceInfluence = 1.0f - driveToTargetInfluence;

            avoidanceVectorLerped = Vector2.Lerp(avoidanceVectorLerped, avoidanceVector, Time.fixedDeltaTime * 4);

            newVectorToTarget = vectorToTarget * driveToTargetInfluence + avoidanceVector * avoidanceInfluence;
            newVectorToTarget.Normalize();

            Debug.DrawRay(transform.position, avoidanceVector * 8, Color.green);
            Debug.DrawRay(transform.position, newVectorToTarget * 8, Color.yellow);
        }
        else
        {
            newVectorToTarget = vectorToTarget;
        }
        bool IsCarsInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector)
        {
            polygonCollider2D.enabled = false;
/*            carAndPlayerLayerMask = LayerMask.GetMask("Car", "Player");
*/            RaycastHit2D raycastHit2D = Physics2D.CircleCast(transform.position + transform.up * 0.5f, 1.2f, transform.up, 12, carAndPlayerLayerMask);
            polygonCollider2D.enabled = true;

            if (raycastHit2D.collider != null)
            {
                Debug.DrawRay(transform.position, transform.up * 5, Color.red);
                position = raycastHit2D.collider.transform.position;
                otherCarRightVector = raycastHit2D.collider.transform.right;

                Debug.Log("Đã va chạm với: " + raycastHit2D.collider.gameObject.name);
                string collidedLayer = LayerMask.LayerToName(raycastHit2D.collider.gameObject.layer);
                Debug.Log("Đã va chạm với layer: " + collidedLayer);
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.up * 5, Color.black);
            }

            position = Vector3.zero;
            otherCarRightVector = Vector3.zero;
            return false;
        }

    }
    //check xem co bi ket hay ko
    IEnumerator StuckCheck()
    {
        Vector3 initialStuckPosition = transform.position;
        isRunningIngStuckCkeck = true;
        yield return new WaitForSeconds(1f);
        if ((transform.position - initialStuckPosition).sqrMagnitude < 2)
        {
            temporarWaypoints = aStarLine.FindPath(currentMapcar.nextWaypointNode[currentWaypointIndex].transform.position);
            if (temporarWaypoints == null)
                temporarWaypoints = new List<Vector2>();
            stuckCkeckCounter++;
            isFirstTemporaryWaypoint = true;
        }
        else
        {
            Debug.Log("Đã không bị kẹt nữa");
            stuckCkeckCounter = 0;
            yield return new WaitForSeconds(2f);
            maxSpeed = initialSpeed;
        }
        isRunningIngStuckCkeck = false;
    }
    private void Update()
    {
        carAndPlayerLayerMask = LayerMask.GetMask("Car", "Player");
        /*       int carAndPlayerLayerMask1 = LayerMask.GetMask("Car", "Player");*/
        circleCastHit1 = Physics2D.CircleCast(transform.position + transform.up * 0.5f, 1.2f, transform.up, 12, carAndPlayerLayerMask);
    }
    void OnDrawGizmos()
    {
        // Vẽ CircleCast màu đỏ trong phương thức OnDrawGizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.up * 0.5f, 1.2f);

        // Vẽ ray của CircleCast nếu đã hit collider
        if (circleCastHit1.collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + transform.up * 0.5f, circleCastHit1.point);
            Debug.Log("da va cham");
        }
        else
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * 0.5f + transform.up * 12);
        }
    }
}