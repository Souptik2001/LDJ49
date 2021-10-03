using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class MainPlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 velocity;
    public float acceleration;
    public float jumpForce;
    public MultiplayerGameManager multiplayerGameManager;
    public OverallGameManager overallGameManager;
    public LevelFragments levelFragmentsManager;
    public ParticleSystem particleSystem;



    [Header("Ground detection")]
    CircleCollider2D cCollider;
    const float skinWidth = .005f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    float horizontalRaySpacing;
    float verticalRaySpacing;
    RayCastOrigin rayCastOrigin;
    public LayerMask collisionMask;
    CollisionInfo collisions;
    float prevFrameVelocityY;


    bool isGrounded;


    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope;
        public float slopeAngle;
        public float slopeAngleOld;
        public bool descendingSlope;
        public bool presentOnSlope;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
            descendingSlope = false;
            presentOnSlope = false;
        }
    }
    struct RayCastOrigin
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        cCollider = GetComponent<CircleCollider2D>();
        calculateRaySpacing();
        velocity = new Vector2();
    }


    void calculateRaySpacing()
    {
        Bounds bounds = cCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    void updateRayCastOrigin()
    {
        Bounds bounds = cCollider.bounds;
        bounds.Expand(skinWidth * -2);
        rayCastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        rayCastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        rayCastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        rayCastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }



    public void verticalCollisions(Vector2 velocity)
    {
        float directionY = -1;
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        Dictionary<GameObject, int> groundsDetected = new Dictionary<GameObject, int>();

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? rayCastOrigin.bottomLeft : rayCastOrigin.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            if (hit)
            {
                GameObject groundDetectedGameObject = hit.transform.gameObject;
                if (groundsDetected.ContainsKey(groundDetectedGameObject) == true)
                {
                    groundsDetected[groundDetectedGameObject]++;
                }
                else
                {
                    groundsDetected.Add(groundDetectedGameObject, 1);
                }
                // velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                if (rayLength < 0.01f)
                {
                    collisions.below = true;
                }
            }
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red); // Debuging
        }
    }



    void CheckForGround()
    {
        Bounds colliderBounds = cCollider.bounds;
        if(Physics2D.Raycast(transform.position, Vector3.down, (colliderBounds.size.y/2)+0.03f, collisionMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }


    void FixedUpdate()
    {
        CheckForGround();
        // Horizontal movement
        if (Input.GetKey(KeyCode.A))
        {
            if(velocity.x > 0)
            {
                velocity.x = 0;
            }
            if (velocity.y != 0)
            {
                velocity.x = Mathf.Lerp(velocity.x, -acceleration * 0.6f * Time.fixedDeltaTime, 0.85f);
            }
            else
            {
                velocity.x = Mathf.Lerp(velocity.x, -acceleration * Time.fixedDeltaTime, 0.85f);
            }
        }else if (Input.GetKey(KeyCode.D))
        {
            if (velocity.x < 0)
            {
                velocity.x = 0;
            }
            if (velocity.y != 0)
            {
                velocity.x = Mathf.Lerp(velocity.x, acceleration * 0.6f * Time.fixedDeltaTime, 0.85f);
            }
            else
            {
                velocity.x = Mathf.Lerp(velocity.x, acceleration * Time.fixedDeltaTime, 0.85f);
            }
        }
        else
        {
            if (velocity.y == 0)
            {
                velocity.x = 0;
            }
        }
        // Horizontal Movement

        // Vertical Movement
        if (Input.GetKey(KeyCode.Space))
        {
            if (velocity.y == 0 && prevFrameVelocityY<=0 && isGrounded)
            {
                rb.AddForce(new Vector2(0, jumpForce));
            }
        }
        // Vertical Movement


        rb.velocity = new Vector2(velocity.x, rb.velocity.y);
        velocity.y = rb.velocity.y;
        prevFrameVelocityY = velocity.y;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (multiplayerGameManager != null)
        {
            CameraShaker.Instance.ShakeOnce(5f, 8f, .1f, .4f);
        }
        else
        {
            CameraShaker.Instance.ShakeOnce(5f, 8f, .1f, 1f);
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Danger"))
        {
            FindObjectOfType<AudioManager>().Play("Die");
            if (overallGameManager != null)
            {
                overallGameManager.GetToMainMenu();
            }
            if (multiplayerGameManager != null)
            {
                multiplayerGameManager.GetToMainMenu();
            }
            return;
        }
        particleSystem.Play();
        levelFragmentsManager.RemovePickUp(collision.gameObject);
        FindObjectOfType<AudioManager>().Play("PickUp");
        if (overallGameManager != null)
        {
            overallGameManager.PickUpPicked();
        }
        if(multiplayerGameManager != null)
        {
            multiplayerGameManager.PickUpPicked();
        }
    }

}
