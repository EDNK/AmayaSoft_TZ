using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int levelCount=0;
    [SerializeField] private int currentLevel;
    [SerializeField] private LevelConfig levelConfig;   
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Text taskText;
    public Text TaskText
    {
        get
        {
            return taskText;
        }
    }
    [SerializeField] private List<CardBundleData> cardTypes;
    [SerializeField] private UnityEvent gameStarts;
    [SerializeField] private UnityEvent levelEnd;
    public UnityEvent LevelEnd
    {
        get
        {
            return levelEnd;
        }
    }
    [SerializeField] private UnityEvent gameEnds;
    private List<GameObject> cells;
    public List<GameObject> Cells
    {
        set
    {
        cells=value;
    } 
        get
    {
        return cells;
    }
    }

    private List<string> prevAnswers;

    // Загенерировать уровень
    void Start()
    {
        prevAnswers=new List<string>();
        Cells=new List<GameObject>();
        GenerateLevel();
        gameStarts.Invoke();
    }

    public void GenerateLevel()
    {   
        if (currentLevel==levelCount)
        {
            gameEnds.Invoke();
            return;
        }
        // Берём строки и столбцы для ячеек из конфигурации уровня
        int n=levelConfig.Cols[currentLevel];
        int m=levelConfig.Rows[currentLevel];
        
        // Количество ячеек, которые необходимо создать для уровня
        // Если число отрицательное, то для уровня требуется меньше ячеек, чем создано, поэтому необходимо деактивировать лишние
        int countToAdd=n*m-Cells.Count;

        // Создание недостающих (если нужно)
        for(int i=0; i<countToAdd; i++)
            Cells.Add(Instantiate(cellPrefab, new Vector3(0f,0f,0f), Quaternion.identity));

        // Располагаем ячейки по сетке NxM
        PositionAllCells(n,m);

        // Переприсваиваем спрайты и текст с запросом
        SpriteAllCells(n,m);

        currentLevel++;    
    }

    void PositionAllCells(int n, int m)
    {   
        // Переменные координаты для ячеек
        float x,y;
        
        x=1-n;
        y=m-1;
        for(int i=0;i<m;i++)
        {
            for(int j=0;j<n;j++)
            {
            Cells[i*n+j].SetActive(true);
            Cells[i*n+j].transform.position=new Vector3(x,y,0f);

            x+=2f;

            }
            x=1-n;
            y-=2f;
        }
    
        // Отключаем неиспользуемые ячейки
        for(int i=m*n; i<Cells.Count-1; i++)
            Cells[i].SetActive(false);
    }
    void SpriteAllCells(int n, int m)
    {
        Cell tmp;

        // Вариация уровня
        int levelType=Random.Range(0,cardTypes.Count);
        int randCard;

        // Номер карточки, которая будет ответом
        int correctCardIndex=Random.Range(0,n*m);

        // Забираем все карточки вариации
        var lst = new List<CardData>(cardTypes[levelType].CardData);
        
        // Заполняем ячейки случайными данными
        for(int i=0;i<n*m;i++)
        {
            tmp=Cells[i].GetComponent<Cell>();            
            randCard=Random.Range(0,lst.Count);
            tmp.SetInfo(lst[randCard]);
            tmp.gen=this;
            lst.RemoveAt(randCard);                    
        }
        
        tmp=Cells[correctCardIndex].GetComponent<Cell>();

        while(prevAnswers.Contains(tmp.CurrentId))
            tmp.SetInfo(lst[Random.Range(0,lst.Count)]);

        tmp.isCorrect=true;
        
        TaskText.text="Find "+tmp.CurrentId;
        prevAnswers.Add(tmp.CurrentId);

        lst.Clear();
    }

    public void DeactivateAllCells()
    {
        foreach (var cell in Cells)
        {
            cell.SetActive(false);
        }
    }

    public void NewGame()
    {
        prevAnswers.Clear();
        currentLevel=0;
        GenerateLevel();
        gameStarts.Invoke();
    }
}
