﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// A very simplistic car driving on the x-z plane.
[RequireComponent(typeof(Rigidbody2D))]
public class Player: MonoBehaviour
{   [Header("Player Paremetres")]
    [SerializeField,Range(1f,100f)] private float life=100f; 
    [SerializeField,Range(10f,100f)]private float speed = 10.0f;
    [SerializeField,Range(10f,100f)]private float rotationSpeed = 100.0f;
    [Header("Single Shoot")]
    [SerializeField] private GameObject singleBullet;
    [SerializeField] private Transform singleShootTransform;
    [SerializeField,Range(0f,3f)] private float singleShootfireRate;
    [Header("Triple Shoot")]
    [SerializeField] private GameObject tripleBullet;
    [SerializeField] private Transform tripleShootTransformLeft;
    [SerializeField] private Transform tripleShootTransformRight;
    [SerializeField,Range(0f,1f)] private float tripleShootfireRate;
    private Image healthBar;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool canShoot=true;
    [Header("Ship deterioration")]
    [SerializeField] private Sprite[] deterioration;
    [SerializeField] private GameObject deathEffect;
    private void Start() {
        rb=GetComponent<Rigidbody2D>();
        healthBar=GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        sprite=GetComponent<SpriteRenderer>();
    }
      void Update()
    {
        float translation = -Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        rotation *= Time.deltaTime;
        rb.velocity=transform.up*translation;
        transform.Rotate(0, 0, -rotation);

        if(Input.GetKeyDown(KeyCode.Space)&&canShoot){
            SingleShoot();
            canShoot=false;
            StartCoroutine(Wait(1/singleShootfireRate));
        }

        if(Input.GetKeyDown(KeyCode.G)&&canShoot){
            TripleShoot();
            canShoot=false;
            StartCoroutine(Wait(1/singleShootfireRate));
        }

        healthBar.fillAmount=life/100;
        if(life<=70&&life>30){
            sprite.sprite=deterioration[1];
        }else if(life<=30&life>0){
        sprite.sprite=deterioration[2];
        }
        else if(life<=0){
         GameOver();
        }
    }
    private void SingleShoot()=>Instantiate(singleBullet,singleShootTransform.position,singleShootTransform.rotation);
    private void TripleShoot(){
        Instantiate(tripleBullet,tripleShootTransformRight.position,tripleShootTransformRight.rotation);
        Instantiate(tripleBullet,tripleShootTransformLeft.position,tripleShootTransformLeft.rotation);
    }
    private IEnumerator Wait(float _seconds){
        yield return new WaitForSeconds(_seconds);
        canShoot=true;
    }
    public void Damege(float damege){
        life-=damege;
    }
    private void GameOver(){
        Instantiate(deathEffect,transform.position,transform.rotation);
        Destroy(gameObject,3f);
    }
    private void OnDestroy() {
         GameObject.FindObjectOfType<UI>().FinalScreen();
    }
}