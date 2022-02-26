namespace helyjegy;

public class Helyjegy
{
    public int Id { get; set; }
    public int Ules { get; set; }
    public int Fel { get; set; }
    public int Le { get; set; }
}
public class Program
{
    static List<Helyjegy> lista = new List<Helyjegy>();
    static int eladottJegyek, vonalHossz, kmDij;
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        //1. feladat
        Beolvas();

        //2. feladat
        Feladat2();

        //3. feladat
        Feladat3();

        //4. feladat
        Feladat4();

        //5. feladat
        Feladat5();

        //6. feladat
        Feladat6();

        //7. feladat
        Feladat7();

        Console.ReadKey();
    }
    private static void Beolvas()
    {
        StreamReader sr = new StreamReader(@"eladott.txt");

        string[] line = sr.ReadLine().Split(' ');
        eladottJegyek = int.Parse(line[0]);
        vonalHossz = int.Parse(line[1]);
        kmDij = int.Parse(line[2]);

        int id = 0;

        while (!sr.EndOfStream)
        {
            string[] line2 = sr.ReadLine().Split(' ');
            Helyjegy uj = new Helyjegy();

            uj.Id = ++id;
            uj.Ules = int.Parse(line2[0]);
            uj.Fel = int.Parse(line2[1]);
            uj.Le = int.Parse(line2[2]);

            lista.Add(uj);
        }

        sr.Close();
    }
    private static void Feladat2()
    {
        Console.WriteLine("2. feladat");

        var utolso = lista.LastOrDefault();

        Console.WriteLine($"Az utolsó jegyvásárló sorszáma: {utolso.Id}, beutazott távolsága: {utolso.Le - utolso.Fel} km");
        Console.WriteLine();
    }
    private static void Feladat3()
    {
        Console.WriteLine("3. feladat");
        Console.Write("Utasok, akik végigutazták a teljes utat: ");

        var szures = lista.Where(x => x.Le == vonalHossz).ToList();

        foreach (var item in szures)
        {
            Console.Write($"{item.Id} ");
        }

        Console.WriteLine();
    }
    private static void Feladat4()
    {
        Console.WriteLine("4. feladat");

        int bevetel = 0;

        foreach (var item in lista)
        {
            int km = item.Le - item.Fel;
            int kerekitett = km / 5 * 5;

            if (kerekitett % 10 == 0) bevetel += (kerekitett / 10) * kmDij;
            else bevetel += (kerekitett / 10 + 1) * kmDij;
        }

        Console.WriteLine($"A társaság bevétele: {bevetel}Ft.");
        Console.WriteLine();
    }
    private static void Feladat5()
    {
        Console.WriteLine("5. feladat");

        var sorrend = lista.OrderByDescending(x => x.Le).ToList();
        int utolsoElotti = 0;

        foreach (var item in sorrend)
        {
            if (item.Le < vonalHossz)
            {
                utolsoElotti = item.Le;
                break;
            }
        }

        var megallo = lista.Where(x => x.Le == utolsoElotti).ToList();

        Console.WriteLine($"A végállomást megelőzően {megallo.Count} utas szállt le.");
        Console.WriteLine();
    }
    private static void Feladat6()
    {
        Console.WriteLine("6. feladat");

        var csoport = lista.Where(x => x.Fel != 0 && x.Le != vonalHossz).GroupBy(x => x.Le).ToList();

        Console.WriteLine($"A busz {csoport.Count} helyen állt meg.");
        Console.WriteLine();
    }
    private static void Feladat7()
    {
        Console.WriteLine("7. feladat");

        Console.Write("Adja meg a kívánt távolságot az utaslistához: ");
        int bTav = int.Parse(Console.ReadLine());

        StreamWriter sw = new StreamWriter(@"kihol.txt");
        int[] ulesek = new int[48];

        for (int i = 0; i < bTav; i++)
        {
            foreach (var item in lista.Where(x => x.Fel == i || x.Le == i))
            {
                if (i == item.Fel)
                {
                    ulesek[item.Ules - 1] = item.Id;
                }
                else if (i == item.Le)
                {
                    ulesek[item.Ules - 1] = 0;
                }
            }
        }

        int count = 0;

        foreach (var item in ulesek)
        {
            count++;

            if (item == 0) sw.WriteLine($"{count}. ülés: üres");
            else sw.WriteLine($"{count}. ülés: {item}. utas");
        }

        Console.WriteLine("Utaslista kész!");
        sw.Close();
    }
}