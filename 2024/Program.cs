using System.Net.Http.Json;

using var http = new HttpClient();
using var response = await http.GetAsync("https://puzzles.code100.dev/code100/puzzles/100hits/coordinatesystem.json");
var coordinatesystem = await response.Content.ReadFromJsonAsync<CoordinateSystem>();

var on100 = coordinatesystem!.Coordinates
    .Where(c =>
           IsInBox(c, x: 145, y: 75, width: 20, height: 225-75)
        || IsOnCircle(c, center: new Coordinate(250, 150), innerRadius: 55, outerRadius: 55+20)
        || IsOnCircle(c, center: new Coordinate(410, 150), innerRadius: 55, outerRadius: 55+20)
    )
    .Count();

Console.WriteLine(on100);


bool IsInBox(Coordinate c, int x, int y, int width, int height)
{
    return c.x >= x && c.x <= x + width && c.y >= y && c.y <= y + height;
}

bool IsOnCircle(Coordinate c, Coordinate center, int innerRadius, int outerRadius)
{
    var distance = EuclideanDistance(center, c);
    return distance >= innerRadius && distance <= outerRadius;
}

double EuclideanDistance(Coordinate a, Coordinate b)
{
    var x = Math.Abs(a.x - b.x);
    var y = Math.Abs(a.y - b.y);
    return Math.Sqrt(x * x + y * y);
}

record CoordinateSystem(int width, int height, int[][] coords)
{
    public Coordinate[] Coordinates => coords.Select(c => new Coordinate(c[0], c[1])).ToArray();
}

record Coordinate(int x, int y);