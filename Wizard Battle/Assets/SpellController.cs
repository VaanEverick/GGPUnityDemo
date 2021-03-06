﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour {

    GameObject target;
    public GameObject user;
    public Vector3 direction;
    public int lifeSpan;
    float speed;
    int damage;
    Terrain terrain;
    int posX;
    int posY;
    Vector2 heightMapDimensions;
    int digSize = 5;
    float desiredHeight = .001f;

    public string Tag;

	// Use this for initialization
	void Start ()
    {
        switch (Tag)
        {
            case "FireBall": lifeSpan = 100; speed = 6f; damage = 8;   break;
            case "MagicMissile": lifeSpan = 250; speed = 8f; damage = 3; break;
            case "RockWall": lifeSpan = 500;   break;
            case "Dig": terrain = Terrain.activeTerrain; heightMapDimensions = new Vector2(terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight); lifeSpan = 3; break;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		switch(Tag)
            {
            case "FireBall": Fireball(); break;
            case "MagicMissile": MagicMissile(); break;
            case "RockWall": RockWall(); break;
            case "Dig": Dig(); break;
        }
	}

    void Fireball()
    {
        transform.position += direction / speed;
        lifeSpan--;
        
    }

    void MagicMissile()
    {

    }

    void RockWall()
    {

        //TODO : replace height check with terrain height level check
        //TODO : set rotation based on player look

        if (transform.position.y  < Terrain.activeTerrain.terrainData.GetHeight((int)transform.position.x, (int)transform.position.y) * 2 )
        {
            transform.position = new Vector3(0, .04f, 0) + transform.position;
        }
        else
        {
            lifeSpan--;
        }
    }

    void Dig()
    {
        if (lifeSpan > 0)
        {
            Vector3 temp = user.transform.position + (user.GetComponentInChildren<Camera>().transform.forward * 4f) + terrain.gameObject.transform.position;
            Vector3 loc;

            loc.x = temp.x / terrain.terrainData.size.x;
            loc.y = temp.y / terrain.terrainData.size.y;
            loc.z = temp.z / terrain.terrainData.size.z;

            posX = (int)(loc.x * heightMapDimensions.x);
            posY = (int)(loc.z * heightMapDimensions.y);

            int offset = digSize / 2;

            float[,] heights = terrain.terrainData.GetHeights(posX - offset, posY - offset, digSize, digSize);

            for (int i = 0; i < digSize; i++)
            {
                for (int j = 0; j < digSize; j++)
                {
                    heights[i, j] += desiredHeight;
                }
            }

            terrain.terrainData.SetHeights(posX - offset, posY - offset, heights);

            lifeSpan--;
        }
    }

    public void SetDirection()
    {

        direction = user.GetComponentInChildren<Camera>().transform.forward;
       
    }
}
