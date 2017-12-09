using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {

    private SkinnedMeshRenderer[] renderers;
    private GameObject expEffect = null;
    public GameObject BothPos;
    public GameObject body;
    private int initHp = 100;
    private int currHp;
    public Canvas hubCanvas;
    public Image hpBar;
    private PlayerAni Playerani;

    // Use this for initialization
    void Awake () {
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        Playerani = body.GetComponent<PlayerAni>();
        currHp = initHp;
        hpBar.color = Color.green;

    }
    void OnTriggerEnter(Collider coll)
    {
        if (currHp > 0 && coll.GetComponent<Collider>().tag == "CANNON")
        {
            Playerani.playerani.CrossFade("Standing React Large Gut");
            Playerani.playerstate = "Standing React Large Gut";
            currHp -= 10;
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
            StartCoroutine(this.Relife());
        }
    }
    IEnumerator Relife()
    {
        Playerani.playerani.CrossFade("Dying Backwards");
        Playerani.playerstate = "Dying Backwards";
        hubCanvas.enabled = false;
        //SetVisible(false);

        yield return new WaitForSeconds(4.6f);
        transform.position = BothPos.transform.position;
        BothPos.GetComponent<Specific>().Open = true;
        hpBar.fillAmount = 1.0f;
        hpBar.color = Color.green;
        hubCanvas.enabled = true;
        //SetVisible(true);

        currHp = initHp;
    }
    void SetVisible(bool isVisible)
    {
        foreach (SkinnedMeshRenderer _renderer in renderers)
        {
            _renderer.enabled = isVisible;
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
