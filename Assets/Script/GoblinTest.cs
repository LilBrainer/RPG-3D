using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class GoblinTest : MonoBehaviour
{
    public float velocidad = 0.0f;
    public float aceleracion = 0.1f;
    public float desaceleracion = 0.5f;
    private Animator m_Animator;

    public bool grounded = true;

    public Rigidbody rb;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool mover = Input.GetKey(KeyCode.W);
        bool aim = Input.GetKey(KeyCode.P);

        if(mover && velocidad < 1.0f)
        {
            velocidad += Time.deltaTime * aceleracion;
        }

        if(!mover && velocidad > 0.0f) 
        {
            velocidad -= Time.deltaTime * desaceleracion;
        }

        if(!mover && velocidad < 0.0f) 
        {
            velocidad = 0.0f;
        }

        if (aim)
        {
            m_Animator.SetBool("Aiming", !m_Animator.GetBool("Aiming"));
        }

        


        if (Input.GetKeyDown(KeyCode.D))
        {
            m_Animator.SetBool("Dancing", !m_Animator.GetBool("Dancing"));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            m_Animator.SetBool("TPsoe", !m_Animator.GetBool("TPsoe"));
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            m_Animator.SetTrigger("Jump");
        }

        m_Animator.SetBool("Grounded", grounded);

        m_Animator.SetFloat("Velocidad", velocidad);


        Debug.Log(grounded);
    }

    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
        if (m_Animator.GetBool("Falling") == true)
        {
            m_Animator.SetBool("Falling", false);
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        grounded = false;
    }
}
