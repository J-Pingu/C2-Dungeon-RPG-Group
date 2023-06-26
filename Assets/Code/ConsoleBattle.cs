using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleBattle : MonoBehaviour
{
    public class Character      //initialize characters
    {
        public string name;     //every character has a name
        public int health;
        public bool alive;
    }

    public class Player : Character      //initialize Player
    {
        public bool defending = false;
        public Player()
        {
            name = "Player";
            health = 10;
        }

        public void attack(Character target)
        {
            int iRand = Random.Range(1,3);          //determine the dmg
            target.health = target.health-iRand;    //change the health of our target
            Debug.Log("You attack and deal "+iRand+" Dmg!");
        }

        public void heal()
        {
            if (health >= 8)
            {
                health = 10;
            }
            else
            {
                health += 2;
            }
        }
        public void defend()
        {
            defending = true;
        }
    }

    public class Enemy : Character          //initialize enemy AI
    {
        public bool charging;
        public Enemy()
        {
            name = "AI";
            health = 10;
        }
        public void attack(Character target)
        {
            int iRand = Random.Range(2,5);
            target.health = target.health-iRand;
            Debug.Log("The enemy attacks and deals"+iRand+"Dmg!");
        }
        public void charge()
        {
            charging = true;
        }
        public void chargedAttack(Character target)
        {
            target.health = 0;
        }
    }
    
    static int turn = 0;      //Determining whose turn it is to attack.
    Player player = new Player();               //create Player Object player
    Enemy ai = new Enemy();                     //create Enemy Object ai
    public void PlayerTurnText()                //Define a status output 
        {
            Debug.Log("Player Health: "+player.health);
            Debug.Log("Enemy Health: "+ai.health);
            Debug.Log("Choose an option:");
            Debug.Log("Press '1' to attack and deal 1-2 Dmg.");
            Debug.Log("Press '2' to heal yourself by 2 HP.");
            Debug.Log("Press '3' to block the next enemy attack.");
        }
    
    public void CheckForDeath()     //checks if anyone has 0 hp and either goes into a state where you have to restart the game or move on
    {
        if (player.health == 0)
        {
            Debug.Log("You died.");
            turn = 2;
            return;
        }
        else if (ai.health == 0)
        {
            Debug.Log("You've successfully defeated the enemy!");
            turn = 2;
            return;
        }
        else
        {
            return;
        }
    }
    // Start is called before the first frame update
    
    void Start()
    {
        Debug.Log("Initialize Console Battle!");    
        Debug.Log("Defeat the enemy ai!");
        Debug.Log("Choose an option:");
        Debug.Log("Press '1' to attack and deal 1-2 Dmg.");
        Debug.Log("Press '2' to heal yourself by 2 HP.");
        Debug.Log("Press '3' to block the next enemy attack.");
    }

    // Update is called once per frame
    void Update()
    {
        if (turn == 0)          //player turn
        {
            player.defending = false;               //reset if the player ist defending
            if (Input.GetKeyDown(KeyCode.Alpha1))      //Pressing 1
            {
                player.attack(ai);
                turn = turn ^ 1;
                CheckForDeath();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) //Pressing 2
            {
                if (player.health == 10)
                {
                    Debug.Log("You're at full health!");
                    Debug.Log("Do something else!");
                }
                else 
                {
                    player.heal();
                    turn ^= 1;
                }

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) //Pressing 3
            {
                player.defend();
                turn = turn ^ 1;
            }
        }
        if (turn ==1)       //enemy turn
        {
            if (player.defending)       //Check if Player is defending
            {
                    Debug.Log("You've blocked the attack successfully");
                    ai.charging = false;
                    turn ^= 1;
                    PlayerTurnText();
            }
            else                        //If Player is not defending
            {
                if (ai.charging)        //If Enemy is already charging then attack with the charged attack
                {
                    ai.chargedAttack(player);
                    Debug.Log("The enemy unleashes a charged attack. It deals 10 DMG!!");
                    do
                    {
                        CheckForDeath();
                        if (turn == 2)
                        {
                            break;
                        }
                        else
                        {
                            PlayerTurnText();
                        }
                    }   while (false);
                }
            
                else
                {
                    int iRand = Random.Range(0,100);
                    if (iRand <= 20)                    //20% to ai chooeses attack
                    {               
                        ai.attack(player);
                        turn ^=1;
                        CheckForDeath();
                        PlayerTurnText();
                    }
                    else                                //80%  ai chooeses to charge
                    {
                        ai.charge();
                        Debug.Log("The enemy begins charging up for an attack!");
                        turn ^=1;
                        PlayerTurnText();
                    }
                }
            }
        }
    }
}
