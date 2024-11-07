using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePainting2 : MonoBehaviour
{

    public GameObject m_objecttorotate;

    [Space]
    public int m_minScale;
    public int m_maxScale;

    private float initialFingersDistance;

    private Vector3 initialScale;
    private float m_firstpoint;
    private float m_secondpoint;

    private int m_inc = 0;

    void Update()
    {

        if (Input.touchCount == 0)
        {
            m_inc = 0;
            return;
        }


        if (m_objecttorotate == null)
        {
            return;
        }


        if (Input.touchCount == 1)
        {
            if (m_inc == 0)
            {
                m_firstpoint = (int)Input.GetTouch(0).position.x;
                m_secondpoint = (int)Input.GetTouch(0).position.x;
            }

            m_inc++;

            if (m_inc <= 10)
            {
                return;
            }

            m_secondpoint = (int)Input.GetTouch(0).position.x;

            if (m_firstpoint < m_secondpoint)
            {
                _Rotating(false);
            }
            else if (m_firstpoint > m_secondpoint)
            {
                _Rotating(true);
            }


            return;
        }

        if (Input.touches.Length == 2)
        {
            _Scaling();
            return;
        }
    }


    private void LateUpdate()
    {
        if (m_inc >= 10)
        {
            m_firstpoint = (int)Input.GetTouch(0).position.x;
        }
    }

    void _Rotating(bool m_right)
    {

        //Debug.Log(m_right);

        if (m_right)
        {
            m_objecttorotate.transform.Rotate(Vector3.up * Time.deltaTime * 200f);
        }
        else
        {
            m_objecttorotate.transform.Rotate(Vector3.down * Time.deltaTime * 200f);
        }
    }


    void _Scaling()
    {
        if (Input.touches.Length == 2)
        {
            Touch t1 = Input.touches[0];
            Touch t2 = Input.touches[1];

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                initialFingersDistance = Vector2.Distance(t1.position, t2.position);
                initialScale = m_objecttorotate.transform.localScale;
            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {

                float currentFingersDistance = Vector2.Distance(t1.position, t2.position);
                var scaleFactor = currentFingersDistance / initialFingersDistance;

                Vector3 m_scale = initialScale * scaleFactor;

                m_scale.x = Mathf.Clamp(m_scale.x, m_minScale, m_maxScale);
                m_scale.y = Mathf.Clamp(m_scale.y, m_minScale, m_maxScale);
                m_scale.z = Mathf.Clamp(m_scale.z, m_minScale, m_maxScale);
                m_objecttorotate.transform.localScale = m_scale;
                //m_detailstext.text = m_scale.ToString();
            }
        }

    }
}
