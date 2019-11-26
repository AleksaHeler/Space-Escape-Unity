using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    //Vars
    public bool pickedUpKey = false;
    public bool pressedButton = false;
    public bool openedDoor = false;
    public int level = 0;

    public Transform spawnPoint; //Player spawn
    public Transform keySpawnPoint; //Key spawn
    public GameObject playerPrefab; //Prefabs
    public GameObject keyPrefab;
    public GameObject deadBodyPrefab;

    public AudioClip[] audioClips; //Audio clips
    public AudioClip yaas;
    public Text textField; //Screen text display
    private AudioSource source; //Audio source for playing

    private GameObject player; //GameObject references
    private GameObject key;
    private GameObject button;


    void Start()
    {
        //Getting AudioSource component
        source = GetComponent<AudioSource>();

        //Getting Player reference
        player = GameObject.FindGameObjectWithTag("Player");
        key = GameObject.FindGameObjectWithTag("Key");
        button = GameObject.FindGameObjectWithTag("Button");

        //Setting info text
        if (level == 0)
            textField.text = "Door ->";

        //Getting spawn points
        if (!spawnPoint)
            spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        if (!keySpawnPoint)
            spawnPoint = GameObject.FindGameObjectWithTag("KeySpawn").transform;
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1)
            DestroyPlayer();
        GameObject[] keys = GameObject.FindGameObjectsWithTag("Key");
        if (keys.Length > 1)
            DestroyKey();

        //Handle user key input
        if (Input.GetKeyDown(KeyCode.R))
            Respawn();
        else if (Input.GetKeyDown(KeyCode.Q))
            source.PlayOneShot(audioClips[(int)Random.Range(0, audioClips.Length)], 1);

        //If player dissapears
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (!player)
            {
                Respawn();
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        //If key dissapears without player picking it up
        if (!key && !pickedUpKey)
        {
            key = GameObject.FindGameObjectWithTag("Key");
            if (!key)
            {
                Instantiate(keyPrefab, keySpawnPoint.position, Quaternion.identity);
                key = GameObject.FindGameObjectWithTag("Key");
            }
        }
    }

    void DestroyPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            Destroy(player.gameObject);
            Debug.Log("Destroyed a player");
        }
    }

    void DestroyKey()
    {
        key = GameObject.FindGameObjectWithTag("Key");
        if (key)
        {
            Destroy(key.gameObject);
            Debug.Log("Destroyed a key");
        }
    }

    void Respawn()
    {
        //Reset vars
        pickedUpKey = false;
        pressedButton = false;
        openedDoor = false;

        //Unpress the button
        button.transform.localScale = new Vector3(0.1f, button.transform.localScale.y, button.transform.localScale.z);

        //Find player and key
        player = GameObject.FindGameObjectWithTag("Player");
        key = GameObject.FindGameObjectWithTag("Key");

        //If there are key and player in the scene delete them
        if (key)
            Destroy(key.gameObject);
        if (player)
            Destroy(player.gameObject);

        //Instantiate new player and key
        Instantiate(keyPrefab, keySpawnPoint.position, Quaternion.identity);
        Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void NextLevel()
    {
        level++;

        //Reset vars
        pickedUpKey = false;
        pressedButton = false;
        openedDoor = false;

        Respawn();

        //Unpress the button
        button.transform.localScale = new Vector3(0.1f, button.transform.localScale.y, button.transform.localScale.z);

        //Change the text
        if (level == 1)
            textField.text = "Pick it!";
        if (level == 2)
            textField.text = "Click it!";
        if (level == 3)
            textField.text = "Try everything";
        if (level > 3)
            EndGame();
        //Finish the game if last level
    }

    public void PickUpKey()
    {
        //On levels 0,2 you shouldn't pick up the key
        if (level == 0 || level == 2)
            return;
        pickedUpKey = true;
        Destroy(key);
    }

    public void PressButton()
    {
        //On levels 0,1 you shouldn't pick up the key
        if (level == 0 || level == 1)
            return;
        button.transform.localScale = new Vector3(0.05f, button.transform.localScale.y, button.transform.localScale.z);
        pressedButton = true;
    }

    public void OpenDoor()
    {
        //source.PlayOneShot(yaas, 1);

        if (level == 0) //just go trough the door
        {
            source.PlayOneShot(yaas, 1);
            NextLevel();
        }
        else if (level == 1 && pickedUpKey) //pick up the key
        {
            source.PlayOneShot(yaas, 1);
            NextLevel();
        }
        else if (level == 2 && pressedButton) //press the button
        {
            source.PlayOneShot(yaas, 1);
            NextLevel();
        }
        else if (level == 3 && pressedButton && pickedUpKey) //pick up the key and press the button
        {
            source.PlayOneShot(yaas, 1);
            NextLevel();
        }
    }

    public void Spikes()
    {
        //Destroy old dead body
        GameObject deadBody = GameObject.FindGameObjectWithTag("DeadBody");
        if (deadBody)
            Destroy(deadBody.gameObject);

        //Instantiate another dead body
        Instantiate(deadBodyPrefab, player.transform.position, Quaternion.identity);
        Respawn();
    }

    void EndGame()
    {
        //Change to next scene (the end)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
