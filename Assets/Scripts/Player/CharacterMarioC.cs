using UnityEngine;
using System.Collections;

public class CharacterMarioC : Abstract {	
	public float jumpSpeed = 12f;
	public float gravity = 9.81f;

	private float moveforward=0;
	private float verticalSpeed = 0f;
	
	private bool grounded=false;
	private bool jumping=false;
	private bool downing=false;
	private bool stumble=false;
	private bool flying=false;
	private bool movingToFlyGround=false;
	private float vsletAcceleration=10;
	//private bool downing=false;
	
	private bool freezed=false;
	
	private float forcex=0;
	
	private Player playerScript;
	
	private float glideTimer;
	private bool glideFlag=false;
	private bool groundingFlag=false;
	
	private float heightnormal=1.5f, heightslide=0.5f;
	private CapsuleCollider walkinbearCollider;
	
	private GameObject walkingBear;
	
	private float flyingYsmex=8,groundYsmex=0;
	
	private float timeToGround;
	
	private CharacterController controller;
	// Update is called once per frame
	
	private void UpdateSmoothedMovementDirection ()
	{
		if(!playerScript.GetFlagOnlyFizik())
		{
			if(moveforward>=0)
			{
				moveforward=-0.5f;
			}else
			{
				moveforward=0.5f;
			}
		}
	}
	
	void Start()
	{
		playerScript=GlobalOptions.GetPlayerScript();
		controller = GetComponent<CharacterController>();
		walkingBear=singleTransform.FindChild("WalkingBear").gameObject;
		walkinbearCollider=walkingBear.collider as CapsuleCollider;
	}
	
	void Update() {

		UpdateSmoothedMovementDirection();
	
		if (grounded&&!jumping) {
			verticalSpeed = 0;
		}
		// Apply gravity
		if(!freezed)
		{
			verticalSpeed -= gravity * Time.deltaTime;
		}
		else
		{
			moveforward=0;
		}
		
		Vector3 right = singleTransform.TransformDirection(Vector3.right);
		Vector3 forward = singleTransform.TransformDirection(Vector3.forward);
		
		Vector3 movement = moveforward*forward + new Vector3 (0, verticalSpeed, 0) + forcex*right;
		
		if(freezed)
		{
			movement=Vector3.zero;
		}
		
		movement *= Time.fixedDeltaTime;
		// Move the controller
		CollisionFlags flags = controller.Move(movement);
		grounded = (flags & CollisionFlags.CollidedBelow) != 0;
		
		stumble = (flags & CollisionFlags.CollidedSides) != 0;
		
		if(stumble&&!flying&&!groundingFlag)
		{
			Debug.Log ("StumbleMario");
			playerScript.Stumble();
		}
		
		// We are in jump mode but just became grounded
		if (grounded)
		{
			if(jumping)
			{
				GlobalOptions.playerStates=PlayerStates.WALK;
			}
			
			if(downing)
			{
				Glide();
			}
			jumping = false;
		}
		downing = false;
		
		if(glideFlag)
		{
			MakeGlide();
		}
		
		if(movingToFlyGround)
		{
			MoveToFlyGround();
		}
	}
	
	private void Glide()
	{
		if(!glideFlag)
		{
			glideFlag=true;
			glideTimer=Time.time;
			walkinbearCollider.height=heightslide;
			walkinbearCollider.center=new Vector3(walkinbearCollider.center.x,walkinbearCollider.center.y-(heightnormal-heightslide)/2,walkinbearCollider.center.z);
		}
	}
	
	private void MakeGlide()
	{
		if(Time.time-glideTimer>1)
		{
			walkinbearCollider.height=heightnormal;
			walkinbearCollider.center=new Vector3(walkinbearCollider.center.x,walkinbearCollider.center.y+(heightnormal-heightslide)/2,walkinbearCollider.center.z);
			glideFlag=false;
			GlobalOptions.playerStates=PlayerStates.WALK;
			Debug.Log("GlideEnd");
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
		if (grounded&&!jumping&&!glideFlag&&!flying) {
			jumping = true;
			verticalSpeed = jumpSpeed;
		}
	}
	
	public void Down()
	{
		if (grounded||jumping&&!flying) {
			downing = true;
			verticalSpeed = -jumpSpeed;
		}
		if(jumping)
		{
			GlobalOptions.playerStates=PlayerStates.JUMP;
		}
		
		if(flying)
		{
			GlobalOptions.playerStates=PlayerStates.FLY;
		}
	}
	
	public void Fly(bool inflag)
	{
		flying = inflag;
		movingToFlyGround=true;
		groundingFlag=false;
	}
	
	public void MoveToGroundEmmidiately()
	{
		groundingFlag=false;
		playerScript.Character.layer=11;
		movingToFlyGround=false;
		walkingBear.transform.localPosition=new Vector3(0,groundYsmex,0);
	}
	
	private void MoveToFlyGround()
	{
		float ypos=walkingBear.transform.localPosition.y;
		//взлёт
		if(flying)
		{
			playerScript.Character.layer=13;
			ypos+=vsletAcceleration*Time.deltaTime;
			if(flyingYsmex<=ypos)
			{
				ypos=flyingYsmex;
				movingToFlyGround=false;
			}
		}
		else
		{
			//падение
			if(!groundingFlag)
			{
				ypos-=vsletAcceleration*Time.deltaTime;
				if(groundYsmex>=ypos)
				{
					ypos=groundYsmex;
					timeToGround=Time.time;
					groundingFlag=true;
					playerScript.Character.layer=16;
				}
			}
			else
			{
				if(Time.time-timeToGround>4)
				{
					groundingFlag=false;
					movingToFlyGround=false;
					playerScript.Character.layer=11;
				}
			}
		}
		walkingBear.transform.localPosition=new Vector3(0,ypos,0);
	}
	
	public void Freeze()
	{
		freezed=true;
	}
	
	public void Respawn()
	{
		Debug.Log ("Respawn");
		flying=false;
		stumble=false;
		freezed=false;
		groundingFlag=false;
		movingToFlyGround=false;
	}
	
	public void SetMovement(float inmovement)
	{
		moveforward=inmovement;
		Debug.Log (moveforward);
	}
	
	public void LeftRight(float inx)
	{
		forcex=inx;
	}
}