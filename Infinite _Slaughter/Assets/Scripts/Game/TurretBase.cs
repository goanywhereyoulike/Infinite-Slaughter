using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    [SerializeField]
    GameObject turret;
    [SerializeField]
    Canvas buildUI;
    public float range;
    bool isInRange;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = range;
        turret.gameObject.SetActive(false);
        buildUI.gameObject.SetActive(false);
        isInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)&&isInRange)
        {
            turret.gameObject.SetActive(true);

            Destroy(buildUI.gameObject);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buildUI.gameObject.SetActive(true);
            isInRange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buildUI.gameObject.SetActive(false);
            isInRange = false;
        }
    }
}
