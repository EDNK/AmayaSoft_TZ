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

    void Start()
    {
        prevAnswers=new List<string>();
        Cells=new List<GameObject>();
        GenerateLevel();
        gameStarts.Invoke();
    }

    public void GenerateLevel()
    {   
        // Проверка на окончание игры
        if (currentLevel==levelCount)
        {
            gameEnds.Invoke();
            return;
        }

        // Строки и столбцы для ячеек из конфигурации уровня
        int n=levelConfig.Cols[currentLevel];
        int m=levelConfig.Rows[currentLevel];
        
        // Количество ячеек, которые необходимо создать для уровня
        int countToAdd=n*m-Cells.Count;

        // Создание недостающих (если нужно, т.е. countToAdd>0)
        for(int i=0; i<countToAdd; i++)
            Cells.Add(Instantiate(cellPrefab, new Vector3(0f,0f,0f), Quaternion.identity));

        // Расположение ячеек по сетке NxM
        PositionAllCells(n,m);

        // Переприсвоение спрайтов и текста с запросом
        SpriteAllCells(n,m);

        currentLevel++;    
    }

    void PositionAllCells(int n, int m)
    {   
        // Переменные координаты для ячеек
        float x,y;
        
        x=1-n;
        y=m-1;

        // Задание положение ячеек
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
    
        // Отключение неиспользуемыых ячеек
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

        // Получение всех карточек вариации
        var lst = new List<CardData>(cardTypes[levelType].CardData);
        
        // Заполнение ячейки случайными данными
        for(int i=0;i<n*m;i++)
        {
            tmp=Cells[i].GetComponent<Cell>();            
            randCard=Random.Range(0,lst.Count);
            tmp.SetInfo(lst[randCard]);
            tmp.gen=this;
            lst.RemoveAt(randCard);                    
        }
        
        tmp=Cells[correctCardIndex].GetComponent<Cell>();

        // Выбор случайного значения, которого ещё не было на прошлых уровнях
        while(prevAnswers.Contains(tmp.CurrentId))
            tmp.SetInfo(lst[Random.Range(0,lst.Count)]);

        tmp.isCorrect=true;
        
        // Смена текста задания
        TaskText.text="Find "+tmp.CurrentId;

        // Пополнение списка прошлых ответов
        prevAnswers.Add(tmp.CurrentId);

        lst.Clear();
    }

    // Деактивация всех ячеек (в конце игры)
    public void DeactivateAllCells()
    {
        foreach (var cell in Cells)
        {
            cell.SetActive(false);
        }
    }

    // Очищение/инициализация запуск события начала игры
    public void NewGame()
    {
        prevAnswers.Clear();
        currentLevel=0;
        GenerateLevel();
        gameStarts.Invoke();
    }
}
