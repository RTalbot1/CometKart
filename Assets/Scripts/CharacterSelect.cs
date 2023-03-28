using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class CharacterSelect: MonoBehaviour
{
    public GameObject[] characters;
    public int SelectedCharacter = 0;
    public TMP_Text label;

    void Start()
    {
        for (int i = 1; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
        
        label.text = characters[SelectedCharacter].name;
    }
     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.SetInt("SelectedCharacter", SelectedCharacter);
            // load the next scene here
            //SceneManager.LoadScene(sceneName: "Game");
        }
    }

    public void NextCharacter() 
    {
        characters[SelectedCharacter].SetActive(false);
        SelectedCharacter = (SelectedCharacter + 1) % characters.Length;
        label.text = characters[SelectedCharacter].name;
        characters[SelectedCharacter].SetActive(true);
        label.text = characters[SelectedCharacter].name;
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void PreviousCharacter()
    {
        characters[SelectedCharacter].SetActive(false);
        SelectedCharacter--;
        if (SelectedCharacter < 0)
        {
            SelectedCharacter += characters.Length;
        }
        label.text = characters[SelectedCharacter].name;
        characters[SelectedCharacter].SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

}
