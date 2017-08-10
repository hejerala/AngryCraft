using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    //public GameObject SpikeBall;
    private Rigidbody bulletRigidBody;
    public float delayForDeath = 5.0f;
    public float shootForce = 4000;

	// Use this for initialization
	void Start () {
        //When the bullet is created it shoots it
        bulletRigidBody = GetComponent<Rigidbody>();
        //We can access a component on the same gameobject by calling GetComponent<Type>()
        //The rigid body component handles all physics, such as forces
        bulletRigidBody.AddForce(transform.forward * shootForce, ForceMode.Force);

    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnCollisionEnter(Collision collisionInfo) {
        Destroy(this.gameObject, delayForDeath);//five second delay before destroy
    }
}
