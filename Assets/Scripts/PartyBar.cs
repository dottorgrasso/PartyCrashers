﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PartyBar : MonoBehaviour {

    public int m_Max = 100;
    public int m_Current = 0;
    public float m_DecreaseRate = 5f;
    public int m_DecreaseAmount = 5;
    public float m_fillSpeed = 2f;

    public bool m_Active;

    private Image m_Bar;

    float m_TempTimer;

	// Use this for initialization
	void Start () {
        m_Bar = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

        if(GameManager.m_Instance.m_GameState != GameManager.GameState.Minigame)
        {
            dungeonPartyBarDrain();
        }
        else if(GameManager.m_Instance.m_GameState == GameManager.GameState.Minigame)
        {
            minigamePartyBarDrain();
        }
    }

    void dungeonPartyBarDrain()
    {
        //set bar equal to percentage
        m_Bar.fillAmount = Mathf.Lerp(m_Bar.fillAmount, (float)m_Current / m_Max, m_fillSpeed * Time.deltaTime);

        if (m_Active)
        {

            if (m_TempTimer <= Time.time - m_DecreaseRate)
            {
                m_Current -= m_DecreaseAmount;
                m_TempTimer = Time.time;
            }

            //if bar hits 0 load minigame
            if (m_Current >= m_Max)
            {
                loadMinigame();
            }
        }
    }

    void minigamePartyBarDrain()
    {
        m_Bar.fillAmount = Mathf.Lerp(m_Bar.fillAmount, (float)m_Current / m_Max, m_fillSpeed * Time.deltaTime);

        if (m_Active)
        {
            //set bar equal to percentage

            if (m_TempTimer <= Time.time - m_DecreaseRate)
            {
                m_Current -= m_DecreaseAmount;
                m_TempTimer = Time.time;
            }

            //if bar hits 0 load minigame
            if (m_Current <= 0)
            {
                loadBackToGame();
            }
        }
    }

    void loadMinigame()
    {
        GameManager.m_Instance.m_GameState = GameManager.GameState.Minigame;
        //int randomNumber = Random.Range(1, 3);

        GameManager.m_Instance.savePlayers();

        if(GameManager.m_Instance.m_Tutorial == GameManager.Tutorial.Lobby_01 ||
            GameManager.m_Instance.m_Tutorial == GameManager.Tutorial.Lobby_02 ||
            GameManager.m_Instance.m_Tutorial == GameManager.Tutorial.Lobby_03)
        {
            ++GameManager.m_Instance.m_Tutorial;
        }

        SceneManager.LoadScene(Random.Range(5, 7));
    }

    void loadBackToGame()
    {
        GameManager.m_Instance.m_GameState = GameManager.GameState.Dungeon;
        //int randomNumber = Random.Range(1, 3);

        GameManager.m_Instance.savePlayers();

        if (GameManager.m_Instance.m_Tutorial == GameManager.Tutorial.Lobby_01 ||
    GameManager.m_Instance.m_Tutorial == GameManager.Tutorial.Lobby_02 ||
    GameManager.m_Instance.m_Tutorial == GameManager.Tutorial.Lobby_03)
        {
            SceneManager.LoadScene(GameManager.m_Instance.m_Tutorial.ToString()); //ballroom blitz
        }
        else
        {
            SceneManager.LoadScene(Random.Range(5, 7));
        }
    }
}
