using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{  
    public Camera FollowingCamera;
    public GameObject TargetForCamera;
    public GameObject TargetForCameraTop;

    private GameManager gm;
    private Player player;
    private CinemachineFreeLook cameraFreeLookOptions;
    private Vector3 lastPos;

    private const float cameraZoomMin = 20.0f;
    private const float cameraZoomMax = 120.0f;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        player = gm.Player;
        cameraFreeLookOptions = FollowingCamera.GetComponent<CinemachineFreeLook>();
        lastPos = transform.position;
    }

    private void Update()
    {
        //Debug.Log(player.Rigidbody.velocity.magnitude);

        player.ResetSpeed();
        //player.ResetMass();

        //zoom
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0.1)
        {
            cameraFreeLookOptions.m_Lens.FieldOfView -= 1;
            if (cameraFreeLookOptions.m_Lens.FieldOfView < cameraZoomMin)
                cameraFreeLookOptions.m_Lens.FieldOfView = cameraZoomMin;
        }
        if (mw < -0.1)
        {
            cameraFreeLookOptions.m_Lens.FieldOfView += 1;
            if (cameraFreeLookOptions.m_Lens.FieldOfView > cameraZoomMax)
                cameraFreeLookOptions.m_Lens.FieldOfView = cameraZoomMax;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!gm.IsPauseMenuShowed)
            {
                //levelup menu (its here cause can be called when timescale = 0)
                if (gm.IsLevelUpMenuShowed)
                    gm.LevelUpMenuShow(false);
                else
                    gm.LevelUpMenuShow(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gm.IsLevelUpMenuShowed)
                gm.LevelUpMenuShow(false);

            //pause menu (its here cause can be called when timescale = 0)
            if (gm.IsPauseMenuShowed)
                gm.PauseMenuShow(false);
            else
                gm.PauseMenuShow(true);
        }

        //jump (its here cause dbljump is not working norm in FixedUpdate)
        player.Jump(FollowingCamera.transform);

        //snatch (dont remember why its here but it works and i`m glad)
        player.Snatch();

        //growing and reducing (its here cause needed in KeyDown)
        player.GrowthUp();
        player.Reduce();

        //shooting
        player.Shot(FollowingCamera.transform, TargetForCamera);
        player.SuperShot(FollowingCamera.transform, TargetForCamera);

        //ТУТ неправильно, надо луч кидать как-то по направлению движения или вообще по-другому делать
        //collision detecting
        RaycastHit hit;
        if (Physics.Linecast(lastPos, transform.position, out hit))
        {
            Debug.Log("Go ray");
            if (hit.transform.tag != "Player" && hit.transform.tag != "Bullet")
            {
                Debug.Log("Ray hit");
                player.OnCollideWith(hit.transform.gameObject, FollowingCamera.transform);
            }
        }
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        //weighting
        player.WeightUp();

        //shift
        player.Shift();

        //move player
        player.Move(FollowingCamera.transform);

        //move camera
        TargetForCamera.transform.position = new Vector3(transform.position.x, transform.position.y + player.TargetForCameraYPlus, transform.position.z);
        TargetForCameraTop.transform.position = new Vector3(transform.position.x, transform.position.y + player.TargetForCameraTopYPlus, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Behavior when meet PickUp objects
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            player.MediatorsCount++;
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        player.OnCollideWith(collision.gameObject, FollowingCamera.transform);
    }*/
}
