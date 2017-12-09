using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDamage : MonoBehaviour {

    private int initHp = 100;
    private int currHp;
    private MeshRenderer renderer;
    public Canvas hubCanvas;
    public Image hpBar;
    public GameObject box1;
    public GameObject box2;
    public GameObject box3;
    public GameObject box4;
    public GameObject box5;

    // Use this for initialization
    void Awake()
    {
        currHp = initHp;
        hpBar.color = Color.green;
        renderer = GetComponent<MeshRenderer>();
    }

    void OnCollisionEnter(Collision coll)
    {
        if (currHp > 0 && coll.gameObject.tag == "CANNON")
        {
            currHp -= 5;
            hpBar.fillAmount = (float)currHp / (float)initHp;
            if (hpBar.fillAmount <= 0.4f)
            {
                hpBar.color = Color.red;
            }
            else if (hpBar.fillAmount <= 0.6f)
            {
                hpBar.color = Color.yellow;
            }
        }
        if (currHp <= 0)
        {
            StartCoroutine(this.GameOver());
        }
    }
    /*
    void OnTriggerEnter(Collider coll)
    {
        if (currHp > 0 && coll.GetComponent<Collider>().tag == "CANNON")
        {
            currHp -= 20;
            hpBar.fillAmount = (float)currHp / (float)initHp;
            if (hpBar.fillAmount <= 0.4f)
            {
                hpBar.color = Color.red;
            }
            else if (hpBar.fillAmount <= 0.6f)
            {
                hpBar.color = Color.yellow;
            }
        }
        if (currHp <= 0)
        {
            StartCoroutine(this.GameOver());
        }
    }
    */
    IEnumerator GameOver()
    {
        if (hubCanvas.enabled)
        {
            hubCanvas.enabled = false;
            box1.AddComponent<Rigidbody>();
            box2.AddComponent<Rigidbody>();
            box3.AddComponent<Rigidbody>();
            box4.AddComponent<Rigidbody>();
            box5.AddComponent<Rigidbody>();
            box1.GetComponent<MeshRenderer>().enabled = true;
            box2.GetComponent<MeshRenderer>().enabled = true;
            box3.GetComponent<MeshRenderer>().enabled = true;
            box4.GetComponent<MeshRenderer>().enabled = true;
            box5.GetComponent<MeshRenderer>().enabled = true;
            renderer.enabled = false;
        }
        yield return new WaitForSeconds(6.0f);

        //hpBar.fillAmount = 1.0f;
        //hpBar.color = Color.green;
        //hubCanvas.enabled = true;
        //currHp = initHp;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
