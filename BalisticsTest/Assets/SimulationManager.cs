using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{

    public Transform m_ProjectileStartTransform;
    public float m_ProjectileSpeed = 100.0f;

    public Transform m_TargetStartTransform;
    public float m_TargetSpeed;

    private float m_SolverIterationScale;
    public int m_SolverIterations = 1;

    public GameObject m_ProjectilePrefab, m_TargetPrefab;


    //calculated variables
    private Vector3 m_ProjectileVelocityCalculated, m_TargetVelocityCalculated;
    private float m_TimeToTargetCalculated;


    //simulation
    private Vector3 m_SimulatingProjectileVelocity, m_SimulatingTargetVelocity;
    private GameObject m_SimulatingProjectile, m_SimulatingTarget;
    private float m_SimulationTimeLeft;
    
    
    public LineRenderer m_ProjectileLineRenderer, m_TargetLineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 aimDir;
        float timeToTarget;
        Vector3 targetVelocity = m_TargetStartTransform.forward * m_TargetSpeed;

        ProjectileMath.CalculateProjectileAim(m_ProjectileStartTransform.position, m_ProjectileSpeed, m_TargetStartTransform.position, targetVelocity, m_SolverIterations, out timeToTarget, out aimDir);

        Vector3[] projectilePositions = new Vector3[2] { m_ProjectileStartTransform.position, m_ProjectileStartTransform.position + (aimDir * timeToTarget * m_ProjectileSpeed) };
        m_ProjectileLineRenderer.SetPositions(projectilePositions);

        Vector3[] targetPositions = new Vector3[2] { m_TargetStartTransform.position, m_TargetStartTransform.position + (targetVelocity * timeToTarget) };
        m_TargetLineRenderer.SetPositions(targetPositions);

        m_ProjectileStartTransform.rotation = Quaternion.LookRotation(aimDir.normalized);

        m_ProjectileVelocityCalculated = m_ProjectileStartTransform.forward * m_ProjectileSpeed;
        m_TargetVelocityCalculated = targetVelocity;
        m_TimeToTargetCalculated = timeToTarget;


        UpdateSimulation();
    }

   

private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 300, 150),"");
        GUILayout.Label("Projectile speed: " + m_ProjectileSpeed);
        m_ProjectileSpeed = GUILayout.HorizontalSlider(m_ProjectileSpeed, 5.0f, 50.0f);

        GUILayout.Label("Target speed: " + m_TargetSpeed);
        m_TargetSpeed = GUILayout.HorizontalSlider(m_TargetSpeed, 5.0f, 50.0f);


        GUILayout.Label("Calculation Iterations: " + m_SolverIterations);
         m_SolverIterationScale = GUILayout.HorizontalSlider(m_SolverIterationScale, 0, 1);
        float solverIterations = m_SolverIterationScale * m_SolverIterationScale;

        m_SolverIterations = Mathf.Clamp((int)(solverIterations * 500), 1, 500);


        if (GUILayout.Button("Start Simulation"))
        {
            if(m_SimulatingProjectile && m_SimulatingTarget)
            {
                Destroy(m_SimulatingProjectile);
                Destroy(m_SimulatingTarget);
            }

            m_SimulationTimeLeft = m_TimeToTargetCalculated;
            m_SimulatingProjectileVelocity = m_ProjectileVelocityCalculated;
            m_SimulatingTargetVelocity = m_TargetVelocityCalculated;

            m_SimulatingProjectile = Instantiate(m_ProjectilePrefab, m_ProjectileStartTransform.position, m_ProjectileStartTransform.rotation);
            m_SimulatingTarget = Instantiate(m_TargetPrefab, m_TargetStartTransform.position, m_TargetStartTransform.rotation);
        }

    }

    void UpdateSimulation()
    {
       if(m_SimulationTimeLeft > 0)
        {
            float deltaTime = 1.0f / 60;
            m_SimulationTimeLeft -= deltaTime;

            m_SimulatingProjectile.transform.position += m_SimulatingProjectileVelocity * deltaTime;
            m_SimulatingTarget.transform.position += m_SimulatingTargetVelocity * deltaTime;
        }
    }


}
