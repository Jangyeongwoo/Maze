using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;



namespace MazeProgram
{
    internal class Maze
    {
        int row = 0;  //미로의 행
        int col = 0;  //미로의 열
        string[,] map = new string[0, 0];
        Stack<Location2D> stack1 = new Stack<Location2D>();
        Stack<Location2D> stack2 = new Stack<Location2D>();
        Location2D entry = new Location2D();
        Location2D exit = new Location2D();
        Stack<Location2D> jcStack = new Stack<Location2D>();
        Location2D add = new Location2D();


        public Maze()
        {
            init(0, 0);
        }
        void init(int r, int c)   //map 이차원 배열 생성
        {
            map = new string[r, c];
        }
    
        public void load(string fname) //미로 파일을 읽어옴
        {

            int k = 0;
            int s = 0;
            string text = File.ReadAllText(@"a.txt");//파일 전체를 읽는다.
            string[] result1 = text.Split('\n');  //행을 구하기 위해 줄바꿈을 기준으로 문자열을 나눈다.
            string[] result2 = text.Split(' ');    //열을 구하기 위해 공백을 기준으로 문자열을 나눈다.
            row = result1.Length;
            col = (result2.Length + (result1.Length - 1)) / result1.Length;
            Console.WriteLine("몇행? : {0}", row);
            Console.WriteLine("몇열? : {0}", col - 1);
            // 2차원 배열map에 미로를 이루는 값들을 복사 
            init(row, col);
            for (int i = 0; i < row; i++)
            {

                for (int j = 0; j < col; j++)
                {
                    while (k < result2.Length)
                    {

                        map[i, j] =result2[k];
                        k++;


                        if (map[i, j] == "0" && s == 0 && (i == 0 || j == 0 || i == row - 1 || j == col - 2))  
                        {
                            entry.Row = i;
                            entry.Col = j;
                            stack1.Push(entry);
                            Console.WriteLine($" < 출발점: {entry.Row},{entry.Col} > ");
                            s++;
                            map[i, j] = "3";  //출발점
                            Console.WriteLine();
                        }

                        else if (map[i, j] == "0" && s == 1 && (i == 0 || j == 0 || i == row - 1 || j == col - 2))
                        {
                            exit.Row = i;
                            exit.Col = j;
                            Console.WriteLine($" < 도착점: {exit.Row},{exit.Col} > ");
                            map[i, j] = "9";   //도착점
                        }

                        break;
                    }
                }
            }
        }

        bool isValidLoc(int r, int c)
        {
            if (r < 0 || c < 0 || r >= row || c >= col) //범위를 벗어나면 갈 수 없다
                return false;
            else
                return (map[r, c] == "0" || map[r, c] == "9");   //비어있는 통로나 도착지점일 때만 true
        }
        public void print() //현재 Maze를 화면에 출력
        {
            int s = 0;
            Console.WriteLine("--------전체 미로의 크기----------- ");
            Console.WriteLine("--------------{0}*{1}--------------", row, col - 1);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col - 1; j++)
                {
                    //출발점은 항상 미로의 벽에 있어야하고 '0'이어야 한다. 그런데 도착점도 같은 조건이기 때문에 그 둘을 구분하기위해 int형 변수 s를 생성해서 s가0이면 출발점 1면 도착점으로한다.
                    //맨처음 벽에있는 0을 만나면 그것을 출발점으로 설정하고 s를 1증가시킨다.
                    if (map[i, j] == "0" && s == 0 && (i == 0 || j == 0 || i == row - 1 || j == col - 2)) //출발점
                    {
                        Console.Write("0");
                        s++;
                    }
                    //미로가 갈 수 있는 길은 무조건 벽 안쪽에 있다.
                    else if (map[i, j] == "0" && ((i != 0 && i != row - 1) && (j != 0 && j != col - 1)))  //갈 수 있는길
                    {
                        Console.Write("  ");
                    }
                    //출발점을 이미 찾은 상태라면 s의 값은1이 되어있을 것이다. 따라서 s가 2인 상태에서 만나는 벽에있는 '0'은 벽에 있는 두번째 '0'이라는 뜻이고 그것이 도착점이 된다.
                    else if (map[i, j] == "0" && s == 1 && (i == 0 || j == 0 || i == row - 1 || j == col - 2))  //도착점
                    {
                        Console.Write("◎");
                    }
                    else if (map[i, j] == "1")
                    {
                        Console.Write("■");
                    }
                    else if (map[i, j] == "7")
                    {
                        Console.Write("＊"); //지나온 길
                    }
                    else if (map[i, j] == "3")
                    {
                        Console.Write("○");
                    }
                    else if (map[i, j] == "9")
                    {
                        Console.Write("◎");
                    }
                    else if (map[i, j] == "5")
                    {
                        Console.Write("☆");
                    }


                }

                Console.WriteLine();
            }
        }


        public void searchExit()        //실제 미로를 탐색하는 함수
        {
            
            int count = 0;
            while (stack1.Count > 0)
            //스택이 비어있지 않는 동안
            {

                Location2D here = stack1.Pop();//stack에 들어가있는 최상단요소를 here에 pop()시킨다.
                stack2.Push(here);    //그리고 그 here을 stack2에 push() stack2에는 마지막으로 간 경로가 최상위에 push()된다.
                int r = here.Row;
                int c = here.Col;
              
           
                if (map[r, c] != "3" && map[r, c] != "9") map[r, c] = "7";//stack2의 최상단 요소가 출발점과 도착점이 아니라면 지나온 길로 표시한다.

                if (exit.Row == r && exit.Col == c)     //도착점이라면
                {
                    while (stack2.Count > 0)
                    {    //stack2의 요소들을 전부다 jcstack (연결리스트를 이용한스택)에 push()한다. 그러면 staka2에 쌓였던 것에 역순으로 jcstack에 쌓이게 된다.
                         // 이것은 경로를 왔던 순서대로 출력하게 해준다.

                        jcStack.Push(stack2.Peek());
                        stack2.Pop();


                    }
                    while (jcStack.Count > 0)
                    {    //jcstack에 요소가 없을 때까지
                        Console.WriteLine($"({jcStack.Peek().Row},{jcStack.Peek().Col})");//jcstack의 최상단 요소를 출력
                        jcStack.Pop(); //다음 요소를 출력하기 위해 pop()
                                

                    }
                 
                    Console.WriteLine("\n-------------도착!---------------");
                    return;
                }
                //위에서 초기화 한 stack2의 최상단 요소의 행과 열r 과 c로 조건을 만들었는데 (r,c)주변에 갈 곳이있으면 그 갈 수 있는 곳을
                //stack에 push()하고 count를 +1해준다. 그런데 탐색 순서가 북 동 남 서 이기때문에 만약 북, 동, 남, 서 모든곳 다 갈 수 있다면
                // (r,c)는 분기점이 되고 stack 에는 최상단요소로 서쪽이 push()된다. 따라서 서쪽방향 먼저 탐색 하게 된다.
                else
                {

                    if (isValidLoc(r - 1, c))
                    {

                        add.Row = r - 1;
                        add.Col = c;
                        stack1.Push(add);
                        count++;                     
                    }

                    if (isValidLoc(r, c - 1))
                    {
                        add.Row = r;
                        add.Col = c - 1;
                        stack1.Push(add);
                        count++;                                          
                    }

                    if (isValidLoc(r + 1, c))
                    {
                        add.Row = r + 1;
                        add.Col = c;
                        stack1.Push(add);
                        count++;                      
                    }


                    if (isValidLoc(r, c + 1))
                    {
                        add.Row = r;
                        add.Col = c + 1;
                        stack1.Push(add);
                        count++;
                    }
    
                    //count가 2보다 크거나 같으면 (r,c)주변에 갈 수 있는곳이 2곳 이상이라는 뜻이므로 분기점이 된다.
                    if (count >= 2)
                    {
                        map[r, c] = "5";//분기점은 '5'로 설정해서 다른 경로들과 차별을 둔다.

                        while (stack2.Count >0)
                        {    //분기점을 만나면 stack2에 쌓였던 요소들을 jcstack에 pop()해서 담는다.
                            
                           

                            jcStack.Push(stack2.Pop());
                        }
                        while (jcStack.Count > 0)
                        {    //stack2의 요소들을 다 jcstack에 담았다면 이제 jcstack의 요소들을 출력한다.
                            if (map[jcStack.Peek().Row, jcStack.Peek().Col] == "3")
                            {

                                Console.WriteLine("-------------출발!---------------\n");
                                Console.WriteLine("<분기점을 만나면 그 곳 까지 경로를 출력합니다.>\n");
                            }
                            Console.Write($"({jcStack.Peek().Row},{jcStack.Peek().Col})");
                            jcStack.Pop();
                        }
                        //jcstack에는 stack2에 쌓였던 것에 역순으로 쌓여있기 때문에 처음 출발 했던 곳부터 출력된다.         
                        Console.WriteLine("<☆>");
                        Console.WriteLine();
                    }
                  
                }
                count = 0;
            }
        }
        public void dfsMaze()
        {
            Console.WriteLine("---------------------------------\n");
            Console.WriteLine("---------------------------------               [이웃 위치 탐색 순서]\n");
            Console.WriteLine("---------------------------------                      ①↑\n");
            Console.WriteLine("---------------------------------                    ②←  →④\n");
            Console.WriteLine("------DFS으로 미로 탈출하기------                        ↓③\n");
            Console.WriteLine("---------------------------------\n");
            Console.WriteLine("---------------------------------\n");
        }

    }
}




   

