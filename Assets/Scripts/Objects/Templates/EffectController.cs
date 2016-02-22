using UnityEngine;
using System.Collections;

public class EffectController : ObjectData
{
	public ParticleSystem m_Particle = null;
	public SpriteRenderer m_Sprite = null;


	public bool m_AutoActivate = false;
	private bool m_IsActivated = false;

    public bool m_ShakeCamera = false;
    public float m_ShakeCameraRadius = 1;
    public float m_ShakeCameraTime = 3;

	public bool m_ChangeAlpha = false;
	public float m_AlphaDelta = 0.1f;

    // Use this for initialization
    void Start () {
        m_Particle = GetComponent<ParticleSystem>();
		m_Sprite = GetComponent<SpriteRenderer>();
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
			m_Particle.Play();
			StartCoroutine(ShakeCamera(m_ShakeCameraTime, m_ShakeCameraRadius));
        }

		if(m_ChangeAlpha)
		{
			var tmp = m_Sprite.color;
			tmp.a -= m_AlphaDelta;
			if (tmp.a < 0)
			{
				tmp.a = 0;
				m_AlphaDelta *= -1;
			}
			if (tmp.a > 1)
			{
				tmp.a = 1;
				m_AlphaDelta *= -1;
			}

			m_Sprite.color = tmp;
		}

        m_IsActivated = false;
    }

    public void ActivateEffect()
    {
        m_IsActivated = true;
    }

	void LateUpdate()
	{
		if (m_AutoActivate)
			m_IsActivated = true;
	}



    IEnumerator ShakeCamera(float time, float rad)
    {
        Camera cam = Camera.main;
		Vector3 orig_pos = cam.transform.localPosition;
        Vector3 dest = new Vector3(Random.Range(-rad, rad), Random.Range(-rad, rad), orig_pos.z);

        while(time > 0)
        {
            Vector3 next = Vector3.Lerp(dest, cam.transform.localPosition, GameTime.deltaTime);
            cam.transform.localPosition = next;

            if (Vector3.Distance(next, dest) < 0.1f)
            {
                dest = new Vector3(Random.Range(-rad, rad), Random.Range(-rad, rad), orig_pos.z);
            }
            time -= GameTime.deltaTime;
            
            yield return null;
        }

        cam.transform.localPosition = orig_pos;

        yield return null;
    }


}
