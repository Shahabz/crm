using UnityEngine;
using System.Collections;

public class CharacterMarioC : MonoBehaviour {	
	public float jumpSpeed = 12f;
	public float gravity = 9.81f;

	private Vector3 moveDirection = Vector3.zero;
	private float verticalSpeed = 0f;
	
	private bool grounded=false;
	private bool jumping=false;
	private bool downing=false;
	
	private bool freezed=false;
	
	private CharacterController controller;
	// Update is called once per frame
	
	private void UpdateSmoothedMovementDirection ()
	{
		// Target direction relative to the camera	
		Vector3 targetDirection=new Vector3(0,0,1);
		
		if(moveDirection.z>=0)
		{
			moveDirection =new  Vector3(0, 0, -0.1f);
		}else
		{
			moveDirection =new  Vector3(0, 0, 0.1f);
		}
	}
	
	void Start()
	{
		controller = GetComponent<CharacterController>();
	}
	
	void Update() {

		UpdateSmoothedMovementDirection();
	
		if (grounded&&!jumping||freezed) {
			verticalSpeed = 0;
		}
		// Apply gravity
		if(!freezed)
		{
			verticalSpeed -= gravity * Time.deltaTime;
		}
		
		Vector3 movement = moveDirection + new Vector3 (0, verticalSpeed, 0);
		movement *= Time.deltaTime;
		
		// Move the controller
		CollisionFlags flags = controller.Move(movement);
		grounded = (flags & CollisionFlags.CollidedBelow) != 0;
		
		// We are in jump mode but just became grounded
		if (grounded)
		{
			jumping = false;
		}
	}
	
	public bool isGrounded()
	{
		return grounded;	
	}
	
	public bool isJumping()
	{
		return jumping;	
	}
	
	public void Jump()
	{
		if (grounded&&!jumping) {
			jumping = true;
			verticalSpeed = jumpSpeed;
		}
	}
	
	public void Down()
	{
		if (grounded||jumping) {
			downing = true;
			verticalSpeed = -jumpSpeed;
		}
	}
	
	public void Freeze()
	{
		freezed=true;
	}
	
	public void Respawn()
	{
		freezed=false;
		//controller.velocity=new Vector3(0,0,0);
	}
}
