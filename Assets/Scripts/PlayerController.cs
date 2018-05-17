using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class PlayerController : MonoBehaviour
    {
        public int x
        {
            get { return Mathf.RoundToInt(transform.localPosition.x); }
            set
            {
                transform.localPosition = new Vector3(value,
                                                      transform.localPosition.y,
                                                      transform.localPosition.z);
            }
        }
        public int y
        {
            get { return Mathf.RoundToInt(transform.localPosition.y); }
            set
            {
                transform.localPosition = new Vector3(transform.localPosition.x,
                                                      value,
                                                      transform.localPosition.z);
            }
        }

        void MoveNorth()
        {
            if ((Maze.Instance[x, y].corridors & Directions.North) == Directions.North)
                this.y++;
        }
        void MoveEast()
        {
            if ((Maze.Instance[x, y].corridors & Directions.East) == Directions.East)
                this.x++;
        }
        void MoveSouth()
        {
            if ((Maze.Instance[x, y].corridors & Directions.South) == Directions.South)
                this.y--;
        }
        void MoveWest()
        {
            if ((Maze.Instance[x, y].corridors & Directions.West) == Directions.West)
                this.x--;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                MoveNorth();

            else if (Input.GetKeyDown(KeyCode.RightArrow))
                MoveEast();

            else if (Input.GetKeyDown(KeyCode.DownArrow))
                MoveSouth();

            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                MoveWest();
        }
    }
}