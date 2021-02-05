using System.Collections;
using UnityEngine;
using Cinemachine;


public class MoveCamera : Singleton<MoveCamera>
{
    public bool ClickAbility { get => clickAbility; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineVirtualCamera animationVirtualCamera;
    [SerializeField] private float zoomOutMax = 11;
    [SerializeField] private float zoomOutMin = 5;
    private Transform cameraTransform;
    private Transform myTransform;
    private Vector3 startPos;
    private bool TwoFingers = false;
    private bool isNeedToMoveCamera = true;
    private bool clickAbility = true;

    public void OpenLevel(Level level)
    {
        Debug.Log("Level done!");
        StartCoroutine(FlyAnimation(level));
    }

    private void Awake()
    {
        cameraTransform = mainCamera.transform;
        myTransform = transform;
    }

    private void Update()
    {
        if (isNeedToMoveCamera == true)
        {
            if (Input.touchCount < 3)
            {
                if (Input.touchCount == 0)
                {
                    TwoFingers = false;
                }
                if (Input.touchCount == 2)
                {
                    TwoFingers = true;

                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    Zoom(-deltaMagnitudeDiff * 0.01f);
                }
                else if (TwoFingers != true)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        startPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        Vector3 direction = startPos - mainCamera.ScreenToWorldPoint(Input.mousePosition);

                        if (Vector3.Distance(cameraTransform.position, myTransform.position) < 1)
                            myTransform.position += direction;
                        else
                            myTransform.position = cameraTransform.position;
                    }

                    Zoom(Input.GetAxis("Mouse ScrollWheel"));
                }
            }
        }
    }

    private void Zoom(float increment)
    {
        virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(virtualCamera.m_Lens.OrthographicSize - increment,
            zoomOutMin, zoomOutMax);
    }

    private IEnumerator FlyAnimation(Level level)
    {
        animationVirtualCamera.transform.position = virtualCamera.transform.position;
        LockCamera(true);
        ActivateMainVcam(false);
        animationVirtualCamera.m_Follow = level.LevelArea.transform;
        yield return new WaitForSeconds(5f);
        ActivateMainVcam(true);
        animationVirtualCamera.m_Follow = null;
        yield return new WaitForSeconds(2.5f);
        LockCamera(false);
    }

    private void LockCamera(bool isLock)
    {
        isNeedToMoveCamera = !isLock;
        clickAbility = !isLock;
    }

    private void ActivateMainVcam(bool isActive)
    {
        virtualCamera.enabled = isActive;
        animationVirtualCamera.gameObject.SetActive(!isActive);
    }
}

