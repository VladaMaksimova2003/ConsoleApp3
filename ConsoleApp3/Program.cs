namespace ConsoleApplication3
{
    public interface IPoint
    {
        double X { get; set; }
        double Y { get; set; }
    }

    class Point : IPoint
    {
        private double _x;
        private double _y;

        public Point(double x, double y)
        {
            this._x = x;
            this._y = y;
        }

        public double X
        {
            get => _x;
            set => _x = value;
        }

        public double Y
        {
            get => _y;
            set => _y = value;
        }

        public override string ToString()
        {
            return $"({_x}, {_y})";
        }

    }

    class LineSegment
    {
        private IPoint _startPoint;
        private IPoint _endPoint;

        public LineSegment(IPoint startPoint, IPoint endPoint)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
        }

        public IPoint StartPoint
        {
            get => _startPoint;
            set => _startPoint = value;
        }

        public IPoint EndPoint
        {
            get => _endPoint;
            set => _endPoint = value;
        }

        public static implicit operator int(LineSegment line)
        {
            return (int)line.StartPoint.X;
        }

        public static explicit operator double(LineSegment line)
        {
            return line.StartPoint.Y;
        }

        public static LineSegment operator -(LineSegment line, int value)
        {
            line.StartPoint.X -= value;
            return line;
        }

        public static LineSegment operator -(int value, LineSegment line)
        {
            double value1 = (double)value - line.StartPoint.Y;
            line.StartPoint.Y = value1;
            return line;
        }

        public static bool operator <(LineSegment line1, LineSegment line2)
        {
            return Intersection.CheckIntersection(line1, line2) ? true : false;
        }

        public static bool operator >(LineSegment line1, LineSegment line2)
        {
            return Intersection.CheckIntersection(line1, line2) ? false : true;
        }

        public static LineSegment operator ++(LineSegment line2)
        {
            line2.StartPoint.X--;
            line2.EndPoint.X++;
            return line2;
        }

        public static float operator !(LineSegment line2)
        {
            float deltaX = (float)(line2.EndPoint.X - line2.StartPoint.X);
            float deltaY = (float)(line2.EndPoint.Y - line2.StartPoint.Y);

            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }
        public override string ToString()
        {
            return $"(x1, y1): {_startPoint}, (x2, y2): {_endPoint}";
        }
    }
    public interface ICoefficient
    {
        double A { get; set; }
        double B { get; set; }
        double C { get; set; }
    }

    class Coefficient : ICoefficient
    {
        private double _a;
        private double _b;
        private double _c;

        public Coefficient(double a, double b)
        {
            this._a = a;
            this._b = b;
        }

        public double A
        {
            get => _a;
            set => _a = value;
        }

        public double B
        {
            get => _b;
            set => _b = value;
        }

        public double C
        {
            get => _c;
            set => _c = value;
        }
    }


    class Intersection
    {
        static public double GetNegation(Coefficient _equation1, Coefficient _equation2)
        {
            return _equation1.A * _equation2.B - _equation2.A * _equation1.B;
        }

        static public double GetX(Coefficient _equation1, Coefficient _equation2, double negation)
        {
            return (_equation2.B * _equation1.C - _equation1.B * _equation2.C) / negation;
        }

        static public double GetY(Coefficient _equation1, Coefficient _equation2, double negation)
        {
            return (_equation1.A * _equation2.C - _equation2.A * _equation1.C) / negation;
        }

        static public bool IsWithinBounds(double x, double y, LineSegment line)
        {
            return Math.Min(line.StartPoint.X, line.EndPoint.X) <= x
                && x <= Math.Max(line.StartPoint.X, line.EndPoint.X)
                && Math.Min(line.StartPoint.Y, line.EndPoint.Y) <= y
                && y <= Math.Max(line.StartPoint.Y, line.EndPoint.Y);
        }

        public static bool CheckIntersection(LineSegment line1, LineSegment line2)
        {
            Coefficient equation1 = new Coefficient(line1.EndPoint.Y - line1.StartPoint.Y,
                                      line1.StartPoint.X - line1.EndPoint.X
                                      );
            equation1.C = equation1.A * line1.StartPoint.X + equation1.B * line1.StartPoint.Y;

            Coefficient equation2 = new Coefficient(line2.EndPoint.Y - line2.StartPoint.Y,
                                      line2.EndPoint.X - line2.StartPoint.X
                                      );
            equation2.C = equation2.A * line2.StartPoint.X + equation2.B * line2.StartPoint.Y;

            double negation = GetNegation(equation1, equation2);

            if (negation == 0) return false;


            double x = GetX(equation1, equation2, negation);
            double y = GetY(equation1, equation2, negation);

            if (IsWithinBounds(x, y, line1)) return true;
            return false;
        }
    }


    //1. a1x + b1y = c1
    //2. a2x + b2y = c2
    //умножаем 1. на b2 и 2. на b1 =>
    //1. a1b2x + b1b2y = c1b2
    //2. a2b1x + b2b1y = c2b1
    //вычитаем =>
    //(a1b2 – a2b1)x = c1b2 – c2b1
    //(a1b2 – a2b1)y = a1c2 – a2c1
    //если (a1b2 – a2b1) = 0, то параллельны
    //проверка =>
    //min(x1, x2) <= x <= max(x1, x2)
    //min(y1, y2) <= y <= max(y1, y2)


    internal class Program
    {
        static void UnaryOperations(LineSegment line2)
        {
            line2++;
            Console.WriteLine("Расширенный отрезок на 1 line2: " + line2.ToString());
            Console.WriteLine($"Длинна отрезка line2: {!line2} ");
        }

        static void TypeConversionOperations(LineSegment line1)
        {
            int ResultForInt = line1;
            double ResultForDouble = (double)line1;
            Console.WriteLine($"Операции приведения типа:int(неявная): {ResultForInt}, double (явная): {ResultForDouble} ");
        }

        static void BinaryOperations(LineSegment line1, LineSegment line2)
        {
            Console.WriteLine($"Левосторонняя операция X - 5 = {line1 - 5} ");
            Console.WriteLine($"Правосторонняя операция, 5 - Y = {5 - line1} ");
            Console.WriteLine(line1 < line2 ? "Прямые line1 и line2 пересекаются" : "Прямые line1 и line2 не пересекаются");
        }

        public static void Main(string[] args)
        {
            Point startPoint1 = new Point(2.123, 15.12);
            Point endPoint1 = new Point(1, -1);
            Point startPoint2 = new Point(0, 0);
            Point endPoint2 = new Point(1, 0);
            LineSegment line1 = new LineSegment(startPoint1, endPoint1);
            LineSegment line2 = new LineSegment(startPoint2, endPoint2);

            Console.WriteLine(line1.ToString());
            Console.WriteLine(line2.ToString());

            UnaryOperations(line2);
            TypeConversionOperations(line1);
            BinaryOperations(line1, line2);
        }
    }
}