using UnityEngine;
public class MazeGenerator : MonoBehaviour
{
    public int width = 10; 
    public int height = 10; 
    private int[,] maze;
    public GameObject wallPrefab; 
    public GameObject pathPrefab;
    public GameObject[][] map;
    public float detail, seed;

    void Start()
    {
        map = new GameObject[width][];
        for (int i = 0; i < map.Length; i++)
        {
            map[i] = new GameObject[height];
        }
        GenerateMaze();
        PrintMaze();


        Camera.main.transform.position =
            new Vector3(((float)width / 2) - 0.5f, ((float)height / 2) - 0.5f, -60);
    }
    void GenerateMaze()
    {
        maze = new int[width, height];
        // Inicializar el laberinto
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = 1; // 1 representa un muro, 0 representa un camino
            }
        }
        // Llamar al método de generación recursiva
        GeneratePath(1, 1);
        // Puedes imprimir el laberinto en la consola para ver la representación
      
    }
    void GeneratePath(int x, int y)
    {
        maze[x, y] = 0; // Marcar la posición actual como parte del camino
                        // Direcciones posibles (arriba, derecha, abajo, izquierda)
        int[] directions = { 0, 1, 2, 3 };
        Shuffle(directions);
        // Explorar direcciones posibles
        for (int i = 0; i < directions.Length; i++)
        {
            int nextX = x + 2 * (directions[i] == 1 ? 1 : (directions[i] == 3 ? -1 : 0));
            int nextY = y + 2 * (directions[i] == 2 ? 1 : (directions[i] == 0 ? -1 : 0));
            // Verificar si la próxima posición está dentro de los límites y aún no ha sido visitada
            if (nextX > 0 && nextX < width - 1 && nextY > 0 && nextY < height - 1 && maze[nextX, nextY] == 1)
            {
                maze[x + (nextX - x) / 2, y + (nextY - y) / 2] = 0; // Romper el muro entre las posiciones
                GeneratePath(nextX, nextY);
            }
        }
    }
    void Shuffle(int[] array)
    {
        // Método para barajar un array
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
    void PrintMaze()
    {
        float verticalSpacing = 1f; 

        for (int j = height - 1; j >= 0; j--)
        {
            for (int i = 0; i < width; i++)
            {
                if (maze[i, j] == 1)
                {
                   
                    float noiseValue = Mathf.PerlinNoise((i / 2 + seed) / detail, (j / 2 + seed) / detail);
                    int terrainHeight = (int)(noiseValue * detail);

                    for (int y = 0; y < terrainHeight; y++)
                    {
                        map[i][y] = Instantiate(pathPrefab, new Vector3(i, y, j * verticalSpacing), Quaternion.identity);

                    }
                }
                else
                {
                    map[i][j] = Instantiate(wallPrefab, new Vector3(i, 0, j * verticalSpacing), Quaternion.identity);
                }
            }
           
        }
    }



}