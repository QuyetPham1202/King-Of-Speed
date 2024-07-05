using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCarControllider : MonoBehaviour
{
    [Header("Car settings")]
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;
    bool startJump = false;
    bool hasCollided = false;
    [Header("Sprites")]
    public SpriteRenderer carSpriteRender;
    public SpriteRenderer carShowRender;


    [Header("Jumping")]
    public AnimationCurve jumpCurve;
    public ParticleSystem landingParticleSystem;
    float accelerationInput = 0;
    float steeringInput = 0;

    float rotationAngle = 0;
    float velocityVsUp = 0;

    bool isJumping = false;
    Rigidbody2D carRighdbody2D;
    Collider2D carCollider;
    CarSfxHandler carSfxHandler;
    CarSurfaceHandler carSurfaceHandler;
    private AICar aiCar;
    private Vector3 positionCar;
    private void Start()
    {
        positionCar = transform.position;
    }
    private void Awake()
    {
        carRighdbody2D = GetComponent<Rigidbody2D>();
        carCollider = GetComponentInChildren<Collider2D>();
    }
    private void FixedUpdate()
    {

        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager.instance is null");
            return;
        }

        if (GameManager.instance.GetGameState() == GameStates.countDown)
            return;
        ApplyEngingeForce();

        KillOrthogonalVeclocity();

        ApplySteering();
    }
    void ApplyEngingeForce()
    {
      

        velocityVsUp = Vector2.Dot(transform.up, carRighdbody2D.velocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0)
        {
            return;
        }
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }
        if (carRighdbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0 && !isJumping)
        {
            return;
        }
        if (accelerationInput == 0)
        {
            carRighdbody2D.drag = Mathf.Lerp(carRighdbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);

        }
        else carRighdbody2D.drag = 0;

        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        carRighdbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }
    void ApplySteering()
    {
        float minSpeedBeforeAllowTurningFactor = (carRighdbody2D.velocity.magnitude / 8);

        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);
        rotationAngle -= steeringInput * turnFactor;

        carRighdbody2D.MoveRotation(rotationAngle);

    }
    void KillOrthogonalVeclocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRighdbody2D.velocity, transform.up);
        Vector2 rightVeclocity = transform.right * Vector2.Dot(carRighdbody2D.velocity, transform.right);

     
        carRighdbody2D.velocity = forwardVelocity + rightVeclocity * driftFactor;
    }
    float GetLaterlVelocity()
    {
        return Vector2.Dot(transform.right, carRighdbody2D.velocity);
    }
    public Surface.SurfaceTypes GetSurface()
    {
        return carSurfaceHandler.GetCurrrentSurface();
    }
    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLaterlVelocity();
        isBraking = false;

        if (isJumping)
            return false; 
        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }
        if (Mathf.Abs(GetLaterlVelocity()) > 4.0f)
            return true;

        return false;
    }
    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
    public float GetVelocityMagnitude()
    {
        return carRighdbody2D.velocity.magnitude;
    }

    public void Jump(float jumpHeightScale, float jumpPushScale)
    {
        if (!isJumping)
            StartCoroutine(JumpCoroutine(jumpHeightScale, jumpPushScale));
    }
    private IEnumerator JumpCoroutine(float jumpHeightScale, float jumpPushScale)
    {
        isJumping = true;
        float jumpStartTime = Time.time;
        float jumpDuration = carRighdbody2D.velocity.magnitude * 0.05f;

        jumpHeightScale *= carRighdbody2D.velocity.magnitude;
        jumpHeightScale = Mathf.Clamp(jumpHeightScale, 0.0f, 1f);

        /*carCollider.enabled = false;*/

        carSpriteRender.sortingLayerName = "Default ";
        carShowRender.sortingLayerName = "Backgroud";

        carRighdbody2D.AddForce(carRighdbody2D.velocity.normalized * jumpPushScale * 10, ForceMode2D.Impulse);

        while (isJumping)
        {
            float jumpCompletedPercentage = (Time.time - jumpStartTime) / jumpDuration;
            jumpCompletedPercentage = Mathf.Clamp01(jumpCompletedPercentage);

            carSpriteRender.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;
            carShowRender.transform.localScale = carSpriteRender.transform.localScale * 0.75f;
            carShowRender.transform.localPosition = new Vector3(1, -1, 0.0f) * 3 * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;

            if (jumpCompletedPercentage >= 1.0f)
            {
                break;
            }

            yield return null;
        }

        carShowRender.transform.localScale = Vector3.one;
        carShowRender.transform.localPosition = Vector3.zero;

        carCollider.enabled = true;

        carShowRender.sortingLayerName = "Backgroud";
        carSpriteRender.sortingLayerName = "Default";

        if (jumpHeightScale > 0.2f)
        {/*
            landingParticleSystem.Play();*/
        }

        isJumping = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jump"))
        {
          
            JumpData jumpData = collision.GetComponent<JumpData>();
            if (jumpData != null)
            {
                Jump(jumpData.jumpHeightScale, jumpData.jumpPushScale);
            }
            else
            {
                Debug.LogError("Thành phần JumpData không tồn tại trên đối tượng có thẻ 'Jump'.");
            }

        }
        else if (collision.CompareTag("gastank"))
        {
     
            StartCoroutine(TemporarySpeedChange(+10f, 3f));
            Debug.Log("Đã cộng vào tốc độ");
        }
        if (!hasCollided)
        {
            if (collision.CompareTag("slough"))
            {

                StartCoroutine(TemporarySpeedChange(-4f, 2f));
                Debug.Log("Đã trừ vào tốc độ");
            }
            if (maxSpeed < 0)
            {
                maxSpeed = 0;
            }
            hasCollided = true;
            StartCoroutine(ResetCollisionFlagAfterDelay(5f));
        }

            if (collision.CompareTag("ColliderOut"))
        {
            gameObject.SetActive(false);
            gameObject.transform.position = positionCar;
            gameObject.SetActive(true);
            if( aiCar != null)
            {
                aiCar.ResetPath();
            }
        }
  
    }

        IEnumerator ResetCollisionFlagAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            hasCollided = false;
        }
        private IEnumerator TemporarySpeedChange(float speedChange, float duration)
    {
        maxSpeed += speedChange;
        yield return new WaitForSeconds(duration);
        maxSpeed -= speedChange;

    }
}
