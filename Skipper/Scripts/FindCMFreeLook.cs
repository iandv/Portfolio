using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FindCMFreeLook : MonoBehaviour
{
    private CinemachineFreeLook cmCamera;

    private void Start()
    {
        cmCamera = FindObjectOfType<CinemachineFreeLook>();
    }

    public void ToggleMouseX()
    {
        if (cmCamera != null)
        {
            cmCamera.m_XAxis.m_InvertInput = !cmCamera.m_XAxis.m_InvertInput;
        }
    }

    public void ToggleMouseY()
    {
        if (cmCamera != null)
        {
            cmCamera.m_YAxis.m_InvertInput = !cmCamera.m_YAxis.m_InvertInput;
        }
    }
}
