using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public ArrayList[,] ObjectGraph = new ArrayList[40, 100];
}
