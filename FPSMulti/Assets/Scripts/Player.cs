using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace FPSMulti
{
    public class Player : MonoBehaviourPunCallbacks
    {
        #region Variables

        public float speed;
        public float sprintModifier;
        public float crouchModifier;
        public Camera normalCam;
        public Transform weaponParent;
        public float jumpForce;
        public LayerMask ground;
        public Transform groundDetector;
        public GameObject cameraParent;
        public float maxHealth;
        public Text debugtext;

        public float crouchAmount;
        public GameObject crouchingCollider;
        public GameObject standingCollider;


        private Rigidbody rig;
        private float baseFOV = 60.0f; //field of view
        private float sprintFOVModifier = 1.25f;
        private bool sprint = false;
        private bool jump = false;
        private bool aim = false;
        private bool crouch = false;
        private Vector3 weaponParentOriginal;
        private Vector3 weaponParentCurrentPos;
        private float movementCounter;
        private float idleCounter;
        private Manager mng;
        private Weapon weapon;
        private bool crouched;

        private float currentHealth;
        private Transform uiHealthBar;
        private Text uiAmmo;

        #endregion Variables

        #region Monobehaviour Callbacks

        private void Start()
        {
            mng = GameObject.Find("Manager").GetComponent<Manager>();
            weapon = GetComponent<Weapon>();
            currentHealth = maxHealth;
            cameraParent.SetActive(photonView.IsMine); //ustaw jedyna aktywn¹ kamerê obecnego gracza

            if (!photonView.IsMine) //jesli to nie jest gracz, zmien z "LocalPlayer" na "Player"
            {
                gameObject.layer = 9;
            }
            else
            {
                uiHealthBar = GameObject.Find("HUD/Health/Bar").transform;
                uiAmmo = GameObject.Find("HUD/Ammo/AmmoText").GetComponent<Text>();
                Debug.Log(uiAmmo.text);
                RefreshHealthBar();
            }
            baseFOV = normalCam.fieldOfView;

            if (Camera.main) Camera.main.enabled = false;

            rig = GetComponent<Rigidbody>();
            weaponParentOriginal = weaponParent.localPosition;
        }

        private void Update()
        {
            if (!photonView.IsMine) return;

            //Axes
            float hMove = Input.GetAxisRaw("Horizontal");
            float vMove = Input.GetAxisRaw("Vertical");

            //Controls
            sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            jump = Input.GetKeyDown(KeyCode.Space);
            aim = Input.GetKey(KeyCode.Z) || Input.GetMouseButton(1);
            crouch =  Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);
           
            //States
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.15f, ground);
            bool isJumping = jump && isGrounded;
            bool isSprinting = sprint && vMove > 0 && !isJumping && isGrounded; //czyli tylko gdy porusza siê w przod
            bool isCrounching = crouch && !isSprinting && isGrounded;

            //Crouching
            if(isCrounching)
            {
                photonView.RPC("SetCrouch", RpcTarget.All, !crouched);
            }
            //Jumping
            if (isJumping)
            {
                if (crouched) photonView.RPC("SetCrouch", RpcTarget.All, false);
                rig.AddForce(Vector3.up * jumpForce);
            }

            if (Input.GetKeyDown(KeyCode.U)) TakeDamage(100);

            //UI Refreshes
            RefreshHealthBar();
            weapon.RefreshAmmo(uiAmmo);
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (!photonView.IsMine) return;

            //Axes
            float hMove = Input.GetAxisRaw("Horizontal");
            float vMove = Input.GetAxisRaw("Vertical");

            //Controls
            sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            jump = Input.GetKeyDown(KeyCode.Space);

            //States
            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = jump && isGrounded;
            bool isAiming = aim;
            bool isSprinting = sprint && vMove > 0 && !isJumping && isGrounded && !isAiming; //czyli tylko gdy porusza siê w przod

            //Movement
            Vector3 direction = new Vector3(hMove, 0, vMove);
            direction.Normalize();

            float adjSpeed = speed;
            //Sprint - velocity and field of view
            if (isSprinting)
            {
                if (crouched) photonView.RPC("SetCrouch", RpcTarget.All, false);
                adjSpeed *= sprintModifier;
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);
            }
            else if(crouched)
            {
                adjSpeed *= crouchModifier;
            }
            else
            {
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
            }

            Vector3 targetVelocity = transform.TransformDirection(direction) * adjSpeed * 10 * Time.deltaTime;
            targetVelocity.y = rig.velocity.y;
            rig.velocity = targetVelocity;
        }

        #endregion Monobehaviour Callbacks

        #region Private Methods

        private void RefreshHealthBar()
        {
            float healthRatio = (float)currentHealth / (float)maxHealth;
            uiHealthBar.localScale = Vector3.Lerp(uiHealthBar.localScale, new Vector3(healthRatio, 1, 1), Time.deltaTime * 8f);
        }

        [PunRPC]
        void SetCrouch(bool state)
        {
            if (crouched == state) return;
            crouched = state;

            if(crouched)
            {
                standingCollider.SetActive(false);
                crouchingCollider.SetActive(true);
                weaponParentCurrentPos += Vector3.down * crouchAmount;
            }
            else
            {
                standingCollider.SetActive(true);
                crouchingCollider.SetActive(false);
                weaponParentCurrentPos -= Vector3.down * crouchAmount;
            }
        }
     
        #endregion Private Methods

        #region Public Methods

        public void TakeDamage(int dmg)
        {
            if (photonView.IsMine)
            {
                currentHealth -= dmg;
                Debug.Log(currentHealth);
                //  RefreshHealthBar();

                if (currentHealth <= 0)
                {
                    mng.Spawn();
                    photonView.RPC("DestroyGun", RpcTarget.All, 0);
                    PhotonNetwork.Destroy(gameObject);
                    Debug.Log("===> You died!");
                }
            }
        }
        public void GatherAmmo(int count)
        {
            weapon.GatheredAmmo(count);
        }
        #endregion Public Methods
    }
}