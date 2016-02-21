using UnityEngine;
using System.Collections;

public class ParticleController : ObjectData {
    public ParticleSystem m_Particle = null;
    private bool m_IsActivated = false;

    public bool m_ShakeCamera = false;
    public float m_ShakeCameraRadius = 1;
    public float m_ShakeCameraTime = 3;

    // Use this for initialization
    void Start () {
        m_Particle = GetComponent<ParticleSystem>();
        if (m_Particle == null)
            Debug.LogError("No Particle System : " + name);

	}
	
	// Update is called once per frame
	void Update () {
        if (!GameSceneController.Inst().IsInGame())
            return;
        if (!m_IsActivated)
            return;
        

        // shake camera
        if(m_ShakeCamera)
        {
            if(Debug.isDebugBuild)
                Debug.Log("Shaking Camera for " + m_ShakeCameraTime + "seconds");
            StartCoroutine(ShakeCamera(m_ShakeCameraTime, m_ShakeCameraRadius));
        }


        m_IsActivated = false;
    }

    public void ActivateParticle()
    {
        m_IsActivated = true;
        m_Particle.Play();
    }



    IEnumerator ShakeCamera(float time, float rad)
    {
        Camera cam = Camera.main;
        Vector3 orig_pos = cam.transform.position;
        Vector3 dest = orig_pos + new Vector3(Random.Range(-rad, rad), Random.Range(-rad, rad), 0);

        while(time > 0)
        {
            Vector3 next = Vector3.Lerp(dest, cam.transform.position, GameTime.deltaTime);
            Debug.Log(next);
            cam.transform.position = next;

            if (Vector3.Distance(next, dest) < 0.1f)
            {
                Debug.Log(time);
                dest = orig_pos + new Vector3(Random.Range(-rad, rad), Random.Range(-rad, rad), 0);
            }
            time -= GameTime.deltaTime;
            
            yield return null;
        }

        cam.transform.position = orig_pos;
        Debug.Log("ASDF");

        yield return null;
    }


}
