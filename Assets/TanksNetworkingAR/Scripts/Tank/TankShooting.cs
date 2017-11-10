﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TankShooting : NetworkBehaviour
{
    public int m_PlayerNumber = 1;            // Used to identify the different players.
    public Rigidbody m_Shell;                 // Prefab of the shell.
    public Transform m_FireTransform;         // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;       // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;          // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;              // Audio that plays when each shot is fired.
    public float m_MinLaunchForce = 15f;      // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;      // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxChargeTime = 0.75f;     // How long the shell can charge for before it is fired at max force.

    [SyncVar]
    public int m_localID;

    private string m_FireButton;            // The input axis that is used for launching shells.
    private Rigidbody m_Rigidbody;          // Reference to the rigidbody component.
    [SyncVar]
    private float m_CurrentLaunchForce;     // The force that will be given to the shell when the fire button is released.
    [SyncVar]
    private float m_ChargeSpeed;            // How fast the launch force increases, based on the max charge time.
    private bool m_Fired;                   // Whether or not the shell has been launched with this button press.

	private TankMovement tankMovement;

    private void Awake()
    {
        // Set up the references.
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        // The fire axis is based on the player number.
        m_FireButton = "Fire" + (m_localID + 1);

        // The rate that the launch force charges up is the range of possible forces by the max charge time.
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

		tankMovement = gameObject.GetComponent<TankMovement> ();
    }

    [ClientCallback]
    private void Update()
    {
        if (!isLocalPlayer)
            return;

		CheckFireButton ();


        // The slider should have a default value of the minimum launch force.
        m_AimSlider.value = m_MinLaunchForce;

        // If the max force has been exceeded and the shell hasn't yet been launched...
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // ... use the max force and launch the shell.
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        // Otherwise, if the fire button has just started being pressed...
		else if (fireButtonDown)
        {
            // ... reset the fired flag and reset the launch force.
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            // Change the clip to the charging clip and start it playing.
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
		else if (fireButtonActive  && !m_Fired)
        {
            // Increment the launch force and update the slider.
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        // Otherwise, if the fire button is released and the shell hasn't been launched yet...
		else if (fireButtonUp && !m_Fired)
        {
            // ... launch the shell.
            Fire();
        }
    }

	private bool fireButtonDown;
	private bool fireButtonActive;
	private bool fireButtonUp;

	void CheckFireButton()
	{
		fireButtonDown = false;
		fireButtonActive = false;
		fireButtonUp = false;

		if (tankMovement.IsDragging) return;

		fireButtonDown = false;
		fireButtonActive = false;
		fireButtonUp = false;

		if (Input.GetMouseButtonDown (0)) {
			fireButtonDown = true;
		} else if (Input.GetMouseButton (0)) {
			fireButtonActive = true;
		} else if (Input.GetMouseButtonUp (0)) {
			fireButtonUp = true;
		}
			
	}

    private void Fire()
    {
        // Set the fired flag so only Fire is only called once.
        m_Fired = true;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        CmdFire(m_Rigidbody.velocity, m_CurrentLaunchForce, m_FireTransform.forward, m_FireTransform.position, m_FireTransform.rotation);

        // Reset the launch force.  This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_MinLaunchForce;
    }

    [Command]
    private void CmdFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
    {
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
             Instantiate(m_Shell, position, rotation) as Rigidbody;

        // Create a velocity that is the tank's velocity and the launch force in the fire position's forward direction.
        Vector3 velocity = rigidbodyVelocity + launchForce * forward;

        // Set the shell's velocity to this velocity.
        shellInstance.velocity = velocity;

        NetworkServer.Spawn(shellInstance.gameObject);
    }

    // This is used by the game manager to reset the tank.
    public void SetDefaults()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }
}