namespace Program
{
    class test
    {
        static void Main(string[] args)
        {
            var gost = new GOST28656.GOST28656();

            // Получаем одно значение пдк
            Console.WriteLine(gost[177][45, 1]);
            // Получаем массив значений пдк и обращаемся к 0 индексу
            Console.WriteLine(gost[276][45][0]);
            // Получаем одно значение double по давлению
            Console.WriteLine(gost[163][45, 2.0]);
        }
    }
}

