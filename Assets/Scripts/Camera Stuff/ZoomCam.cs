using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ZoomCam : MonoBehaviour
{
    public static ZoomCam instance;
    [SerializeField] private CinemachineVirtualCamera zoomCam;
    [SerializeField] private float timeDilation;
    [SerializeField] private float zoomTime;
    [SerializeField] private float damageThreshold;
    private Coroutine currentZoomRoutine;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ZoomIn(GameObject focus, float damage)
    {
        //Ganon cam is no longer supported in this version, therefore we return
        return;

        if (damage < damageThreshold || damage > 10000)
        {
            return;
        }

        if (currentZoomRoutine != null)
        {
            StopCoroutine(currentZoomRoutine);
        }

        currentZoomRoutine = StartCoroutine(zoomer(focus));
    }

    public IEnumerator zoomer(GameObject focus)
    {
        Time.timeScale = timeDilation;
        zoomCam.Priority = 1000;
        zoomCam.Follow = focus.transform;
        zoomCam.LookAt = focus.transform;
        yield return new WaitForSeconds(zoomTime*timeDilation);
        zoomCam.Priority = 0;
        zoomCam.Follow = null;
        zoomCam.LookAt = null;
        Time.timeScale = 1;
    }


}
