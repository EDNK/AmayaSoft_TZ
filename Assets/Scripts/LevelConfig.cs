using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName="New LevelConfig", menuName="Level Config Data", order=2)]
public class LevelConfig : ScriptableObject
{
    // Количество строк на i-том уровне = rows[i]
    [SerializeField] private int[] rows;
    public int[] Rows => rows;
    
    // Количество столбцов на i-том уровне = rows[i]
    [SerializeField] private int[] cols;  
    public int[] Cols => cols;
 
}
