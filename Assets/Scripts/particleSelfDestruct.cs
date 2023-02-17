using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(PhotonView))]
public class particleSelfDestruct : MonoBehaviour
{
	// If true, deactivate the object instead of destroying it
	public bool OnlyDeactivate;

	void OnEnable()
	{
		StartCoroutine("CheckIfAlive");
	}

	IEnumerator CheckIfAlive()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();

		while (true && ps != null)
		{
			yield return new WaitForSeconds(0.5f);
			if (!ps.IsAlive(true))
			{
				if (OnlyDeactivate)
				{
#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
#else
					this.gameObject.SetActive(false);
#endif
				}
				else
                {
					if (GetComponent<PhotonView>().IsMine)
                    {
						PhotonNetwork.Destroy(this.gameObject);
					}
				}
				break;
			}
		}
	}
}
