using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Player : MonoBehaviour
{
    public GameObject BulletPrefab;

    //костыль. надо бы избавиться
    public GameState CurrentGameState { get; set; }

    public Rigidbody Rigidbody { get { return GameObject.Find("PlayerBody").GetComponent<Rigidbody>(); } }
    public float TargetForCameraYPlus { get; set; } = 0.5f;
    public float TargetForCameraTopYPlus { get; set; } = 2f;

    public List<Feature> Features = new List<Feature>();

    private float mediatorsCount;
    public float MediatorsCount { get { return mediatorsCount; } set { mediatorsCount = value; MediatorsCountChangedNotify?.Invoke(); } }

    private float Speed { get; set; } //at the moment
    private float CurrentSpeed { get; set; } //usualy
    private float DamagableVelocityDamage { get; set; } //damage to object when player collide it when rigidbody velocity is over some value
  
    private SphereCollider PlayerCollider { get { return GameObject.Find("PlayerBody").GetComponent<SphereCollider>(); } }
    private Transform PlayerTransform { get { return GameObject.Find("PlayerBody").GetComponent<Transform>(); } }

    private bool IsGrounded { get { bool res = Physics.Raycast(PlayerTransform.position, -Vector3.up, PlayerCollider.bounds.extents.y + 0.1f); return res; } }
    private float JumpHeight { get; set; }
    private float SpeedShift { get; set; }
    private float CurrentTimeOfSnatch { get; set; } = 0.0f;
    private bool IsDamagableVelocity { get { return Rigidbody.velocity.magnitude > DefaultDamagableVelocity; } }
    private bool WasSecondJump { get; set; }
    private PlayerSizeState SizeState { get; set; } = PlayerSizeState.Normal;

    private const float DefaultSpeed = 100.0f;
    private const float DefaultSpeedShift = 250.0f;
    private const float DefaultJumpHeight = 100.0f;
    private const float TimeOfSnatch = 0.1f;
    private const float DefaultMassMultiplier = 5f;
    private const float DefaultSizeMultiplier = 5f;
    private const int DefaultMedsCountForSuperShotHorizontal = 9;
    private const int DefaultMedsCountForSuperShotTop = 9;
    private const int DefaultMedsCountForSuperShotBottom = 9;
    private const float DefaultDamagableVelocity = 7.5f;
    private const float DefaultDamagableVelocityDamage = 10.0f;

    public delegate void MediatorsCountChanged();
    public event MediatorsCountChanged MediatorsCountChangedNotify;

    public Player()
    {
        Speed = DefaultSpeed;
        CurrentSpeed = DefaultSpeed;
        SpeedShift = DefaultSpeedShift;
        JumpHeight = DefaultJumpHeight;
        DamagableVelocityDamage = DefaultDamagableVelocityDamage;
        MediatorsCount = 1000.0f;
    }

    public bool HasFeature(string featureName)
    {
        return Features.Any(x => x.Name == featureName);
    }

    public void Move(Transform relativeTransform)
    {
        if (CurrentGameState == GameState.play)
        {
            //forward
            if (Input.GetKey(KeyCode.W))
            {
                Rigidbody.AddForce(relativeTransform.forward * Speed * Time.deltaTime);
            }

            //backward
            if (Input.GetKey(KeyCode.S))
            {
                Rigidbody.AddForce(-relativeTransform.forward * Speed * Time.deltaTime);
            }

            //right
            if (Input.GetKey(KeyCode.D))
            {
                Rigidbody.AddForce(relativeTransform.right * Speed * Time.deltaTime);
            }

            //left
            if (Input.GetKey(KeyCode.A))
            {
                Rigidbody.AddForce(-relativeTransform.right * Speed * Time.deltaTime);
            }
        }
    }

    public void Jump(Transform relativeTransform)
    {
        if (CurrentGameState == GameState.play)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (HasFeature(FeatureNames.jump) || HasFeature(FeatureNames.dbljump) || HasFeature(FeatureNames.combojump))
                {
                    if (IsGrounded)
                    {
                        var movement = relativeTransform.position.normalized;
                        movement = new Vector3(movement.x, movement.y + JumpHeight, movement.z);

                        Rigidbody.AddForce(movement);// * Speed * Time.deltaTime);
                        WasSecondJump = false;
                    }
                    else if (HasFeature(FeatureNames.dbljump) && !WasSecondJump)
                    {
                        var movement = relativeTransform.position.normalized;
                        movement = new Vector3(movement.x, movement.y + JumpHeight + 50, movement.z);

                        Rigidbody.AddForce(movement);// * Speed * Time.deltaTime);
                        WasSecondJump = true;
                    }
                    else if (HasFeature(FeatureNames.combojump))
                    {
                        var movement = relativeTransform.position.normalized;
                        movement = new Vector3(movement.x, movement.y + JumpHeight, movement.z);

                        Rigidbody.AddForce(movement);// * Speed * Time.deltaTime);
                    }
                }
            }
        }
    }

    public void Snatch()
    {
        if (CurrentGameState == GameState.play)
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (HasFeature(FeatureNames.snatch))
                {
                    CurrentTimeOfSnatch += Time.deltaTime;
                    Speed = CurrentSpeed + 3000.0f;

                    if (CurrentTimeOfSnatch > TimeOfSnatch)
                    {
                        ResetSpeed();
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                CurrentTimeOfSnatch = 0.0f;
            }
        }
    }

    public void Shift()
    {
        if (CurrentGameState == GameState.play)
        {
            if (HasFeature(FeatureNames.shift))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    Speed = CurrentSpeed + SpeedShift;
                }
            }
        }
    }

    public void WeightUp()
    {
        if (CurrentGameState == GameState.play)
        {
            if (Input.GetKey(KeyCode.F))
            {
                if (HasFeature(FeatureNames.weighting))
                {
                    //variant with just multiplier
                    Rigidbody.AddForce(Physics.gravity * DefaultMassMultiplier);
                }
            }
        }
    }

    public void GrowthUp()
    {
        if (CurrentGameState == GameState.play)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (HasFeature(FeatureNames.growth))
                {
                    if (SizeState == PlayerSizeState.Big)
                    {
                        PlayerTransform.localScale = PlayerTransform.localScale / DefaultSizeMultiplier;
                        SizeState = PlayerSizeState.Normal;
                    }
                    else if (SizeState == PlayerSizeState.Small)
                    {
                        PlayerTransform.localScale = PlayerTransform.localScale * DefaultSizeMultiplier;
                        SizeState = PlayerSizeState.Normal;
                    }
                    else
                    {
                        PlayerTransform.localScale = PlayerTransform.localScale * DefaultSizeMultiplier;
                        SizeState = PlayerSizeState.Big;
                    }
                }
            }
        }
    }

    public void Reduce()
    {
        if (CurrentGameState == GameState.play)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (HasFeature(FeatureNames.reduction))
                {
                    if (SizeState == PlayerSizeState.Big)
                    {
                        PlayerTransform.localScale = PlayerTransform.localScale / DefaultSizeMultiplier;
                        SizeState = PlayerSizeState.Normal;
                    }
                    else if (SizeState == PlayerSizeState.Small)
                    {
                        PlayerTransform.localScale = PlayerTransform.localScale * DefaultSizeMultiplier;
                        SizeState = PlayerSizeState.Normal;
                    }
                    else
                    {
                        PlayerTransform.localScale = PlayerTransform.localScale / DefaultSizeMultiplier;
                        SizeState = PlayerSizeState.Small;
                    }
                }
            }
        }
    }

    public void Shot(Transform relativeTransform, GameObject targetForCamera)
    {
        if (CurrentGameState == GameState.play)
        {
            if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
            {
                if (HasFeature(FeatureNames.shot))
                {
                    var bullet = Instantiate(BulletPrefab, targetForCamera.transform.position, targetForCamera.transform.rotation);
                    var bulletScript = bullet.GetComponent<MediatorBullet>();
                    bulletScript.SetDirectionRelateToTransform(relativeTransform);
                    if (HasFeature(FeatureNames.strongshot))
                    {
                        bulletScript.SetDamageMode(BulletDamageMode.Strong);
                    }
                    else
                    {
                        bulletScript.SetDamageMode(BulletDamageMode.Normal);
                    }
                    bulletScript.Go();
                    if (!HasFeature(FeatureNames.freeshot))
                    {
                        MediatorsCount--;
                    }
                }
            }
        }
    }

    public void SuperShot(Transform relativeTransform, GameObject targetForCamera)
    {
        if (CurrentGameState == GameState.play)
        {
            if (Input.GetMouseButtonDown((int)MouseButton.RightMouse))
            {
                if (HasFeature(FeatureNames.supershot))
                {
                    var bullets = new List<GameObject>();
                    for (int i = 0; i < DefaultMedsCountForSuperShotHorizontal; i++)
                    {
                        bullets.Add(Instantiate(BulletPrefab, targetForCamera.transform.position, targetForCamera.transform.rotation));
                        bullets.Last().transform.Rotate(Vector3.up, i * (360.0f / DefaultMedsCountForSuperShotHorizontal));
                    }
                    for (int i = 0; i < DefaultMedsCountForSuperShotTop - 1; i++)
                    {
                        bullets.Add(Instantiate(BulletPrefab, targetForCamera.transform.position, targetForCamera.transform.rotation));
                        bullets.Last().transform.Rotate(new Vector3(-45.0f, i * 45.0f, 45.0f));
                    }
                    bullets.Add(Instantiate(BulletPrefab, targetForCamera.transform.position, targetForCamera.transform.rotation));
                    bullets.Last().transform.Rotate(new Vector3(-90.0f, 0, 90.0f));
                    for (int i = 0; i < DefaultMedsCountForSuperShotBottom - 1; i++)
                    {
                        bullets.Add(Instantiate(BulletPrefab, targetForCamera.transform.position, targetForCamera.transform.rotation));
                        bullets.Last().transform.Rotate(new Vector3(45.0f, i * 45.0f, 45.0f));
                    }
                    bullets.Add(Instantiate(BulletPrefab, targetForCamera.transform.position, targetForCamera.transform.rotation));
                    bullets.Last().transform.Rotate(new Vector3(90.0f, 0, 90.0f));

                    for (int i = 0; i < bullets.Count; i++)
                    {
                        var bulletScript = bullets[i].GetComponent<MediatorBullet>();
                        bulletScript.SetDirectionRelateToTransform(targetForCamera.transform);
                        bulletScript.SetDamageMode(BulletDamageMode.Super);
                        bulletScript.Go();
                    }

                    if (!HasFeature(FeatureNames.freeshot))
                    {
                        MediatorsCount -= 10;
                    }
                }
            }
        }
    }

    public void ResetSpeed()
    {
        Speed = CurrentSpeed;
    }

    public void ResetMass()
    {
        Rigidbody.mass = 0.5f;
    }

    public void OnCollideWith(GameObject other, Transform relativeTransform)
    {
        if (IsDamagableVelocity)
        {
            Debug.Log("Velocity is damagable");
            if (!other.CompareTag("Player") && !other.CompareTag("Bullet"))
            {
                var def = other.GetComponent<Defeatable>();
                if (def != null)
                {
                    def.GetDamage(DamagableVelocityDamage, other.transform, relativeTransform);
                }
            }
        }
    }
}

public enum PlayerSizeState
{
    Normal, Big, Small
}
