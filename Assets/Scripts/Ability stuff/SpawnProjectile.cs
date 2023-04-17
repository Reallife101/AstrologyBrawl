using Photon.Pun;
using UnityEngine;


public class SpawnProjectile : Ranged
{
    [SerializeField] Transform spawnOrigin;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject player;
    [SerializeField] bool spawnAsChild;

    private PhotonView pv;

    private void Start()
    {

        if (transform.parent) 
        { 
            pv = transform.parent.gameObject.GetComponent<PhotonView>();
        }
        else
            pv = GetComponent<PhotonView>();
        
        //Debug.Log(pv.GetInstanceID());
    }

    public override void Use()
    {
        if (pv.IsMine)
        {
            spawn();
        }
    }


    void spawn()
    {
        GameObject pro = PhotonNetwork.Instantiate(projectile.name, spawnOrigin.position, Quaternion.identity);

        if (spawnAsChild)
        {
            pro.transform.SetParent(gameObject.transform);
        }

        //If spawned object has doDamage and spawner has a photon view
        doDamage dm = pro.GetComponent<doDamage>();

        if (pv && dm)
        {
            dm.ownerID = pv.GetInstanceID();

            if (transform.parent != null)
            {
                dm.SetSender(transform.parent.gameObject); 
            }
            else
            {
                dm.SetSender(gameObject);
            }
        }

        // If the character faces the other direction, flip the scale and right transform
        if (player.transform.localScale.x<0)
        {
            pro.transform.right = -transform.right.normalized;
            Vector3 proScale = pro.transform.localScale;
           // pro.transform.localScale = new Vector3(proScale.x, proScale.y, proScale.z);
        }
    }
}
