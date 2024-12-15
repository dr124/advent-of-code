namespace Advent._2024.Week3;

public class Day15(string[] input) : IDay
{
    private readonly Vec2 _position =  Extensions.ReadInput(input, '@').First();
    private readonly HashSet<Vec2> _boxes = Extensions.ReadInput(input, 'O').ToHashSet();
    private readonly HashSet<Vec2> _walls =  Extensions.ReadInput(input, '#').ToHashSet();
    private readonly Vec2[] _moves = Extensions.ReadInput(input, c => c is '<' or '^' or '>' or 'v')
        .Select(x => x.On(input))
        .Select(MapMove)
        .ToArray();
    
    public object Part1()
    {
        var boxes = _boxes.Select(x => new Box { Position = x}).ToList();
        return Solve(_walls, boxes, _position, _moves);
    }

    public object Part2()
    {
        var newWalls = _walls.SelectMany(x => new[] { Enlarge(x), Enlarge(x) + Vec2.Right }).ToHashSet();
        var boxes = _boxes.Select(x => new DoubleBox { Position = Enlarge(x) }).Cast<Box>().ToList();
        return Solve(newWalls, boxes, Enlarge(_position), _moves);
    }

    private static int Solve(HashSet<Vec2> walls, List<Box> boxes, Vec2 position, IEnumerable<Vec2> moves)
    {
        foreach (var move in moves)
        {
            var newPosition = position + move;
            if (walls.Contains(newPosition))
                continue;
            
            if (boxes.FirstOrDefault(b => b.IsOnBox(newPosition)) is { } box)
            {
                var affected = new HashSet<Box>();

                if (!TryMove(box, walls, move, boxes, affected))
                    continue;
                
                foreach (var affectedBox in affected)
                {
                    affectedBox.Position += move;
                }
            }
            
            position = newPosition;
        }

        return boxes.Sum(GetCoordinate);
    }

    private static bool TryMove(Box box, HashSet<Vec2> walls, Vec2 move, List<Box> boxes, HashSet<Box> affected)
    {
        affected.Add(box);

        var positionsToCheck = box.GetAffectedPositions(move);
        if (positionsToCheck.Any(walls.Contains))
        {
            return false;
        }

        return positionsToCheck
            .Select(positionToCheck => boxes.Except(affected).FirstOrDefault(b => b != box && b.IsOnBox(positionToCheck)))
            .OfType<Box>()
            .Select(affectedBox => TryMove(affectedBox, walls, move, boxes, affected))
            .All(x => x);
    }
    
    
    private record Box
    {
        public Vec2 Position { get; set; }
        
        public virtual bool IsOnBox(Vec2 position) => position == Position;
        
        public virtual Vec2[] GetAffectedPositions(Vec2 move) => [Position + move];
    }

    private record DoubleBox : Box
    {
        private Vec2 DoublePosition => Position + Vec2.Right;

        public override bool IsOnBox(Vec2 position) =>
            position == DoublePosition || position == DoublePosition;

        public override Vec2[] GetAffectedPositions(Vec2 move) =>
            DoublePosition != Position
                ? [Position + move, DoublePosition + move]
                : [Position + move];
    }
    
    private static Vec2 Enlarge(Vec2 position) => new(position.X * 2, position.Y);
    
    private static int GetCoordinate(Box point) => 100 * point.Position.Y + point.Position.X;
    
    private static Vec2 MapMove(char c) => c switch
    {
        '<' => Vec2.Left,
        '>' => Vec2.Right,
        '^' => Vec2.Up,
        'v' => Vec2.Down,
        _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
    };
}