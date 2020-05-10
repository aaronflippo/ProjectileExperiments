using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public List<Transform> m_FocusTransforms;
    public Transform m_OrbitTransform;

    private float m_CameraOrbitDistance;

    private void Start()
    {
        m_CameraOrbitDistance = -transform.localPosition.z;

    }
    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButton(1))
        {
            float dx = 2.0f*Input.GetAxis("Mouse X");
            float dy = -2.0f*Input.GetAxis("Mouse Y");

            Vector3 currentRotation = m_OrbitTransform.rotation.eulerAngles;
            Vector3 newRotation = currentRotation;
            newRotation.y += dx;
            newRotation.x += dy;

            m_OrbitTransform.eulerAngles = newRotation;
        }

        float deltaZoom = -3.0f*Input.GetAxis("Mouse ScrollWheel");
        if(deltaZoom != 0)
        {
            m_CameraOrbitDistance += deltaZoom * 3.0f;
            m_CameraOrbitDistance = Mathf.Max(m_CameraOrbitDistance, 3);
            transform.localPosition = -1*new Vector3(0, 0, m_CameraOrbitDistance);

        }

        if (m_FocusTransforms.Count > 0)
        {
            //find the average point
            Vector3 focusPos = Vector3.zero;
            for (int i = 0; i < m_FocusTransforms.Count; i++)
            {
                focusPos += m_FocusTransforms[i].position;
            }

            m_OrbitTransform.position = focusPos;

        }
       
    }

}
