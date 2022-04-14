﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public Rigidbody2D rb;
   public float strength = 300f, velocity=5f, runSpeed=100f;
   public float bulletSpeed = 10;
   public Rigidbody bullet;
   public bool onFloor=false, isRunning=false;
   public int jumping=0;
   public int currentHealth = 0, maxHealth=250;
   public HealthBar healthBar;

   void Awake(){
    rb = GetComponent<Rigidbody2D>();
    currentHealth=maxHealth;
   }
   void Update(){
     jump();
     move();

     //press J or click to shoot
     if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0)){
             Fire();
     }

     //damage player test
     if (Input.GetKeyDown(KeyCode.X)){
        DamagePlayer(10);
     }

     //remove gravity if on floor so that player movement doesn't slow down
     /*if(d){
        rb.gravityScale=0f;
     }else
        rb.gravityScale=1f;*/



   }

     //jump method press Space to jump
     void jump(){
        //check if it's the first jump
        if(Input.GetKeyDown(KeyCode.Space)&&onFloor&&jumping==0){
          rb.AddForce(transform.up*strength);
          onFloor=false;
          jumping++;
        //check for second jump, while player in the air
        }else if(Input.GetKeyDown(KeyCode.Space)&&!onFloor&&jumping==1){
          rb.velocity= new Vector2(rb.velocity.x,0f);
          rb.AddForce(transform.up*strength);
          jumping=0;  
        }
     }

     //create bullets and give them velocity
     void Fire()
     {
         Rigidbody bulletClone = (Rigidbody) Instantiate(bullet, transform.position, transform.rotation);
         bulletClone.velocity = new Vector2(bulletSpeed,0.0f);
         bulletClone.AddForce(bulletClone.velocity,(ForceMode.Force));
     }

     void move(){
        //if A is press down move left
        if (Input.GetKeyDown(KeyCode.A)) 
         {
          //if holding shift, increase speed
          if(Input.GetKey(KeyCode.RightShift)){
              isRunning=true;
              rb.velocity = new Vector2 (runSpeed*-1,rb.velocity.y);
            }else
              rb.velocity = new Vector2 (velocity * -1,rb.velocity.y);  
         }

        //
        if (Input.GetKeyUp(KeyCode.A)) 
        {
            rb.velocity = new Vector2 (0,0);
        }
        //same as above
        if (Input.GetKeyDown(KeyCode.D)) 
        {
            if(Input.GetKey(KeyCode.RightShift)){
              isRunning=true;
              rb.velocity = new Vector2 (runSpeed,rb.velocity.y);
            }else
              rb.velocity = new Vector2 (velocity,rb.velocity.y);
        }
        if (Input.GetKeyUp(KeyCode.D)) 
        {
            rb.velocity = new Vector2 (0,0);
        }
     }

     private void OnTriggerEnter2D(Collider2D other){
        //check if the object is floor and set jump to first jump
        if (other.gameObject.tag == "Floor"){
            onFloor=true;
            jumping=0;
        }
        if (other.gameObject.tag == "Bullet"){
            DamagePlayer(10);
        }
     }

     
     private void DamagePlayer(int damage){
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
     }
 


}
