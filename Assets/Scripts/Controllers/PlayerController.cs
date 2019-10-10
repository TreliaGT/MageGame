using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    Vector3 inputVector;
    public float movespeed = 8f;
    public float gravity = -20f;
    CharacterController controller;

    [Header("Looking")]
    public Transform lookCamera;
    public float sensitivityx = 15f;
    public float sensitivityy = 15f;
    public float minY = -90;
    public float maxY = 90;
    float currentYrolation;
    Vector2 aimVector;

    [Header("Health")]
    public int MaxHealth = 10;
    public int CurrentHealth = 10;
    public Text HealthText;

    [Header("Casting")]
    public float castrange = 300f;
    public LayerMask castMask;
    public float castrate = 0.25f;
    public bool Casting = false;
    public Transform Tip;
    public GameObject CastEffectPrefab;
    public GameObject CastLinePrefab;
    public GameObject Wand;


    public int MaxMana = 20;
    public int CurrentMana = 20;
    public Text ManaText;
    EnemyController enemy;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        CurrentMana = MaxMana;
        ManaText.text = CurrentMana.ToString() + "/" + MaxMana.ToString() + "  Mana";
        HealthText.text = CurrentHealth.ToString() + "/" + MaxHealth.ToString() + " Health";
    }

    // Update is called once per frame
    void Update()
    {
        getinput();
        Move();
        Look();
    }

    void getinput()
    {
        //x local right and left 
        //y local up and down
        //z local forward and back
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.z = Input.GetAxis("Vertical");
        aimVector.x = Input.GetAxis("Mouse X");
        aimVector.y = Input.GetAxis("Mouse Y");
        if (Input.GetButtonDown("Fire1") && !Casting)
        {
            Shoot();
        }
    }


    void Move()
    {
        Vector3 moveVector = transform.TransformDirection(inputVector.normalized);
        moveVector *= movespeed;
        moveVector.y = gravity;
        moveVector *= Time.deltaTime;
        controller.Move(moveVector);
    }

    void Look()
    {
        transform.Rotate(transform.up, aimVector.x * sensitivityx);
        currentYrolation += aimVector.y * sensitivityy;

        currentYrolation = Mathf.Clamp(currentYrolation, minY, maxY);
        lookCamera.eulerAngles = new Vector3(-currentYrolation, lookCamera.eulerAngles.y, lookCamera.eulerAngles.z);
    }

    void Shoot()
    {
        if (CurrentMana <= 0)
        {
            return;
        }
        else
        {
            CurrentMana -= 1;
            ManaText.text = CurrentMana.ToString() + "/" + MaxMana.ToString() + "  Mana";
            Instantiate(CastLinePrefab, Tip.position, Quaternion.identity, Tip).transform.forward = Tip.forward;
            RaycastHit hit;
            if (Physics.Raycast(lookCamera.position, lookCamera.forward, out hit, castrange, castMask))
            {
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.GetComponent<EnemyController>().TakeHealth();
                }
                else
                {
                    Instantiate(CastEffectPrefab, hit.point, Quaternion.identity).transform.forward = hit.transform.TransformDirection(hit.normal);
                }
                //  print(hit.point);
                print(hit.transform.gameObject.name);
            }
            StartCoroutine(FireRoute());
        }
    }

    public void TakeDamage()
    {
        CurrentHealth = CurrentHealth - 3;
        HealthText.text = CurrentHealth.ToString() + "/" + MaxHealth.ToString() + " Health";
        if (CurrentHealth <= 0)
        {
            print("You dead");
        }
    }

    IEnumerator FireRoute()
    {
        Casting = true;
        yield return new WaitForSeconds(castrate);
        Casting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ManaFlower" && CurrentMana < MaxMana)
        {
            int newMana = 0;
            other.GetComponent<PickUp>().pickup(out newMana);
            CurrentMana += newMana;
            CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
            ManaText.text = CurrentMana.ToString() + "/" + MaxMana.ToString() + "  Mana";
        }
      else if (other.tag == "HealthFlower" && CurrentHealth < MaxHealth)
        {
            int newHealth = 0;
            other.GetComponent<PickUp>().pickup(out newHealth);
            CurrentHealth += newHealth;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            HealthText.text = CurrentHealth.ToString() + "/" + MaxHealth.ToString() + "  Health";
        }
    }

}
