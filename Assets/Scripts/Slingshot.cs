using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;
    public AudioClip pull,
                     release;

    [Header("Dynamic")]
    public bool allowInput = true;
    public GameObject launchPoint,
                      projectile;
    Rubberband rubberBand;
    public Vector3 launchPos;
    public bool aimingMode;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
        rubberBand = GetComponentInChildren<Rubberband>();
    }

    void OnMouseEnter()
    {
        if (allowInput)
            launchPoint.SetActive(true);
    }
    void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }
    void OnMouseDown()
    {
        if (!allowInput)
            return;

        aimingMode = true;
        projectile = Instantiate(projectilePrefab);
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;

        AudioSource.PlayClipAtPoint(pull, Camera.main.transform.position);
    }
    void Update()
    {
        if (!aimingMode)
            return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDalta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDalta.magnitude > maxMagnitude)
        {
            mouseDalta.Normalize();
            mouseDalta *= maxMagnitude;
        }

        Vector3 projectilePos = launchPos + mouseDalta;
        projectile.transform.position = projectilePos;

        rubberBand.DrawPull(mouseDalta);

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
            projectileRB.isKinematic = false;
            projectileRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projectileRB.velocity = -mouseDalta * velocityMult;

            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);

            FollowCam.POI = projectile;
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.SHOT_FIRED();

            AudioSource.PlayClipAtPoint(release, Camera.main.transform.position);
            rubberBand.DrawRest();
        }
    }
}
