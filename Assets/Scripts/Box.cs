using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] GameObject desappearEffect;
    [SerializeField] List<GameObject> dropItems;
    
    Animator boxAnim;
    AudioManager theAudioManager;
    void Start()
    {
        theAudioManager = GameObject.FindObjectOfType<AudioManager>();
        boxAnim = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyBox();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            hp--;
            boxAnim.SetTrigger("Hit_T");
        }
    }

    void DestroyBox()
    {
        if(hp <= 0)
        {
            theAudioManager.PlaySFX(5);
            transform.parent.gameObject.SetActive(false);
            Instantiate(desappearEffect, gameObject.transform.position, desappearEffect.transform.rotation);
            int rand = Random.Range(0, dropItems.Count);
            Instantiate(dropItems[rand], gameObject.transform.position, dropItems[rand].transform.rotation);

            Destroy(transform.parent.gameObject);
        }
    }
}
