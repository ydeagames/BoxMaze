using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class Maze
{
    // 2次元配列の迷路情報
    private int[,] Data;
    private int Width { get; set; }
    private int Height { get; set; }
    public struct RouteNode
    {
        public Cell pos;
        public Cell before;
    }
    private List<List<RouteNode>> Routes;

    // 穴掘り開始候補座標
    private List<Cell> StartCells;
    // スタート
    Cell startCell;

    // コンストラクタ
    public Maze(Cell size, Cell start)
    {
        var width = size.X;
        var height = size.Y;
        // 5未満のサイズや偶数では生成できない
        if (width < 5 || height < 5) throw new ArgumentOutOfRangeException();
        if (width % 2 == 0) width++;
        if (height % 2 == 0) height++;

        // 迷路情報を初期化
        this.startCell = start;
        this.Width = width;
        this.Height = height;
        Data = new int[width, height];
        StartCells = new List<Cell>();
        Routes = new List<List<RouteNode>>();
        Routes.Add(new List<RouteNode>());

        CreateMaze(start);
    }

    public int[,] GetMaze()
    {
        return Data;
    }

    public List<List<RouteNode>> GetRoutes()
    {
        return Routes;
    }

    // 生成処理
    private void CreateMaze(Cell start)
    {
        // 全てを壁で埋める
        // 穴掘り開始候補(x,yともに偶数)座標を保持しておく
        for (int y = 0; y < this.Height; y++)
        {
            for (int x = 0; x < this.Width; x++)
            {
                if (x == 0 || y == 0 || x == this.Width - 1 || y == this.Height - 1)
                {
                    Data[x, y] = Path;  // 外壁は判定の為通路にしておく(最後に戻す)
                }
                else
                {
                    Data[x, y] = Wall;
                }
            }
        }

        // 穴掘り開始
        Routes.Last().Add(new RouteNode() { pos = start });
        Dig(start.X, start.Y, 0);

        // 外壁を戻す
        for (int y = 0; y < this.Height; y++)
        {
            for (int x = 0; x < this.Width; x++)
            {
                if (x == 0 || y == 0 || x == this.Width - 1 || y == this.Height - 1)
                {
                    Data[x, y] = Wall;
                }
            }
        }
    }

    // 座標(x, y)に穴を掘る
    private void Dig(int x, int y, int count)
    {
        // 指定座標から掘れなくなるまで堀り続ける
        var rnd = new Random();
        while (true)
        {
            // 掘り進めることができる方向のリストを作成
            var directions = new List<Direction>();
            try
            {
                if (this.Data[x, y - 1] == Wall && this.Data[x, y - 2] == Wall)
                    directions.Add(Direction.Up);
                if (this.Data[x + 1, y] == Wall && this.Data[x + 2, y] == Wall)
                    directions.Add(Direction.Right);
                if (this.Data[x, y + 1] == Wall && this.Data[x, y + 2] == Wall)
                    directions.Add(Direction.Down);
                if (this.Data[x - 1, y] == Wall && this.Data[x - 2, y] == Wall)
                    directions.Add(Direction.Left);
            }
            catch (IndexOutOfRangeException e)
            {
                UnityEngine.Debug.LogFormat("ERROR {0}", e);
            }

            // 掘り進められない場合、ループを抜ける
            if (directions.Count == 0) break;

            int bx = x, by = y, bcount = count;
            // 指定座標を通路とし穴掘り候補座標から削除
            SetPath(x, y, count);
            // 掘り進められる場合はランダムに方向を決めて掘り進める
            var dirIndex = rnd.Next(directions.Count);
            // 決まった方向に先2マス分を通路とする
            switch (directions[dirIndex])
            {
                case Direction.Up:
                    SetPath(x, --y, ++count);
                    SetPath(x, --y, ++count);
                    break;
                case Direction.Right:
                    SetPath(++x, y, ++count);
                    SetPath(++x, y, ++count);
                    break;
                case Direction.Down:
                    SetPath(x, ++y, ++count);
                    SetPath(x, ++y, ++count);
                    break;
                case Direction.Left:
                    SetPath(--x, y, ++count);
                    SetPath(--x, y, ++count);
                    break;
            }
            Routes.Last().Add(new RouteNode() { pos = new Cell() { X = x, Y = y, Count = count }, before = new Cell() { X = bx, Y = by, Count = bcount } });
        }

        // どこにも掘り進められない場合、穴掘り開始候補座標から掘りなおし
        // 候補座標が存在しないとき、穴掘り完了
        var cell = GetStartCell();
        if (cell != null)
        {
            if (Routes.Last().Count > 0)
                Routes.Add(new List<RouteNode>());
            Dig(cell.X, cell.Y, cell.Count);
        }
    }

    // 座標を通路とする(穴掘り開始座標候補の場合は保持)
    private void SetPath(int x, int y, int count)
    {
        this.Data[x, y] = Path;
        if (x % 2 == 1 && y % 2 == 1)
        {
            // 穴掘り候補座標
            StartCells.Add(new Cell() { X = x, Y = y, Count = count });
        }
    }

    // 穴掘り開始位置をランダムに取得する
    private Cell GetStartCell()
    {
        if (StartCells.Count == 0) return null;

        // ランダムに開始座標を取得する
        var rnd = new Random();
        var index = rnd.Next(StartCells.Count);
        var cell = StartCells[index];
        StartCells.RemoveAt(index);

        return cell;
    }

    public Maze.RouteNode? GetGoal()
    {
        Maze.RouteNode? longest = null;
        foreach (var route in GetRoutes())
            if (route.Count > 0)
            {
                var last = route.Last();
                if (!longest.HasValue || last.pos.Count > longest.Value.pos.Count)
                    longest = last;
            }
        return longest;
    }

    // デバッグ用処理
    public void DebugPrint()
    {
        var start = startCell;
        var goal = GetGoal();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Width: {Data.GetLength(0)}");
        sb.AppendLine($"Height: {Data.GetLength(1)}");
        for (int y = 0; y < Data.GetLength(1); y++)
        {
            for (int x = 0; x < Data.GetLength(0); x++)
            {
                if (x == start.X && y == start.Y)
                    sb.Append("Ｓ");
                else if (goal.HasValue && x == goal.Value.pos.X && y == goal.Value.pos.Y)
                    sb.Append("Ｇ");
                else
                    sb.Append(Data[x, y] == Wall ? "■" : "　");
            }
            sb.AppendLine();
        }
        UnityEngine.Debug.Log(sb.ToString());
    }

    // 通路・壁情報
    public const int Path = 0;
    public const int Wall = 1;

    // セル情報
    public class Cell
    {
        public int Count { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int IX
        {
            get
            {
                return (X - 1) / 2;
            }
            set
            {
                X = value * 2 + 1;
            }
        }
        public int IY
        {
            get
            {
                return (Y - 1) / 2;
            }
            set
            {
                Y = value * 2 + 1;
            }
        }
    }

    // 方向
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}