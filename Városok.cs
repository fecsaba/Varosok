using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Városok
{
    class Város
    {
        public int Lakosság { get; set; }
        public string Név { get; set; }
        public string Ország { get; set; }
        public double LakosságMill { get; set; }

        public Város(string sor)
        {
            string[] m = sor.Split(';');
            Név = m[0];
            Ország = m[1];
            Lakosság = int.Parse(m[2]);
            LakosságMill = Lakosság / 1000.0;
        }
    }
    class Városok
    {
        static void Main()
        {
            // 2. feladat: adatok beolvasása
            List<Város> v = new List<Város>();
            foreach (var i in File.ReadAllLines("varosok.txt").Skip(1))
            { //.Skip(1) -> Első sor elhagyása
                v.Add(new Város(i));
            }

            Console.WriteLine($"3. feladat: Városok száma: {v.Count} db");

            // 4. feladat:
            int indiaFő = 0;
            foreach (var i in v)
            {
                if (i.Ország=="India")
                {
                    indiaFő += i.Lakosság * 1000;
                }
            }
            Console.WriteLine($"4. feladat: indiai nagyvárosok lakosságának összege: {indiaFő} fő");

            // Linq:
            Console.WriteLine($"4. feladat: indiai nagyvárosok lakosságának összege: {v.Where(x=>x.Ország=="India").Sum(x=>x.Lakosság)*1000} fő");

            // 5. Legnagyobb város adatai
            Console.WriteLine("5. feladat: A legnagyobb város adatai");
            int maxi = 0; // Legelső legyen a legnagyobb
            for (int i = 1; i < v.Count; i++)
            {
                if (v[i].Lakosság>v[maxi].Lakosság)
                {
                    maxi = i;
                }
            }
            Console.WriteLine($"\tNév: {v[maxi].Név}");
            Console.WriteLine($"\tOrszág: {v[maxi].Ország}");
            Console.WriteLine($"\tLakosság: {v[maxi].Lakosság} ezer fő");

            //Linq: 
            var legnagyobb = v.OrderByDescending(x => x.Lakosság).First();
            Console.WriteLine("5. feladat: A legnagyobb város adatai");
            Console.WriteLine($"\tNév: {legnagyobb.Név}");
            Console.WriteLine($"\tOrszág: {legnagyobb.Ország}");
            Console.WriteLine($"\tLakosság: {legnagyobb.Lakosság} ezer fő");

            // 6. Van-e magyar város?
            bool nincsMagyar = true;
            foreach (var i in v)
            {
                if (i.Ország=="Magyarország")
                {
                    nincsMagyar = false;
                    break;
                }
            }
            Console.WriteLine($"6. feladat: {(nincsMagyar?"Nincs":"Van")} magyar város az adatok között.");
            Console.WriteLine($"6. feladat: {(v.Exists(x=>x.Ország=="Magyarország") ? "Van" : "Nincs")} magyar város az adatok között.");

            // 7. Városok egy szóközzel
            int városDb = 0;
            foreach (var i in v)
            {
                //if (i.Név.Split().Length == 2)
                //    városDb++;
                // "Favágó" megoldás:
                int szóközDb = 0;
                foreach (var j in i.Név)
                {
                    if (j == ' ') szóközDb++;
                }
                if (szóközDb == 1) városDb++;
            }
            Console.WriteLine($"7 .feladat: Városok egy szóközzel: {városDb} db");
            Console.WriteLine($"7 .feladat: Városok egy szóközzel: {v.Count(x=>x.Név.Split().Length==2)} db");

            // Statisztika - Szótárral (Dictionary)
            Dictionary<string, int> stat = new Dictionary<string, int>();
            foreach (var i in v)
            {
                if (stat.ContainsKey(i.Ország))
                {
                    stat[i.Ország]++;
                } else
                {
                    stat.Add(i.Ország, 1);
                }
            }
            Console.WriteLine("8. feladat: Ország statisztika");
            foreach (var i in stat)
            {
                if (i.Value > 5)
                {
                    Console.WriteLine($"\t{i.Key} - {i.Value} db");
                }
            }
            
            // Linq:
            Console.WriteLine("8. feladat: Ország statisztika");
            Console.WriteLine(v.GroupBy(g=>g.Ország).Where(x=>x.Count()>5).Aggregate("",(c,n)=>c+$"\t{n.Key} - {n.Count()} db\n"));


            // Statisztika halmaz + szótár
            HashSet<string> h = new HashSet<string>();
            foreach (var i in v) h.Add(i.Ország);
            
            Dictionary<string, int> stat2 = new Dictionary<string, int>();
            foreach (var i in h) stat2.Add(i, 0);
            foreach (var i in v) stat2[i.Ország]++;
            Console.WriteLine("8. feladat: Ország statisztika");
            foreach (var i in stat2)
            {
                if (i.Value > 5)
                {
                    Console.WriteLine($"\t{i.Key} - {i.Value} db");
                }
            }

            Console.WriteLine("9. feladat: kina.txt");
            List<string> ki = new List<string>();
            foreach (var i in v)
            {
                if (i.Ország == "Kína")
                    ki.Add($"{i.Név};{i.Lakosság}");
            }
            File.WriteAllLines("kina.txt", ki);

            //Megoldás linq:
            File.WriteAllText("kina2.txt", v.Where(x => x.Ország == "Kína").Aggregate("",(c, n) => c + $"{n.Név}-{n.Lakosság}\r\n"));


            // 10. Feladat:
            // Orosz nagyvárosok közül melyik a legkisebb?
            Console.WriteLine("10. feladat");
            int mini = Int32.MaxValue;
            int ind = 0;
            List<Város> orosz = new List<Város>();
            for (int i = 0; i < v.Count; i++)
            {
                if (v[i].Lakosság < mini && v[i].Ország == "Oroszország")
                {
                    mini = v[i].Lakosság;
                    orosz.Add(v[i]);
                    ind = i;
                }
                
            }
            Console.WriteLine($"\tA legkisebb orosz város {v[ind].Név} ");

            

            Console.WriteLine($"\tA legkisebb orosz város {v.Where(x => x.Ország == "Oroszország").OrderBy(y => y.Lakosság).First().Név }");


            // 11. feladat:
            // Hány ország neve végződik "a" karakterre?
            Console.WriteLine("11. feladat");
            int db = 0;
            foreach (var i in v)
            {
                if (i.Ország.Substring(i.Ország.Length -1, 1) == "a" )
                {
                    db++;
                }
            }
            Console.WriteLine($"\t{db} db ország neve végződik a-val");
            // 12. feladat:
            // Készíts valós típusú jellemzőt a Város osztályba,
            // ami a város lakosságát millió főben adja meg!
            Console.WriteLine("12. feladat");
            Console.WriteLine(v[0].Lakosság);
            Console.WriteLine(v[0].LakosságMill);
            // 13. feladat:
            // Döntsed el és írjad ki a képenyőre,
            // hogy az USA-ban van-e 6 milló
            // főnél népesebb város!
            Console.WriteLine("13. feladat");
            bool van = false;
            foreach (var i in v)
            {
                if (i.Ország == "USA" && i.LakosságMill > 6)
                {
                    van = true;
                    break;
                }

            }
            Console.WriteLine($"{(van?"van":"nincs")} 6 mill-nál nagyobb város az USA-ban");
            // 14. feladat:
            // Statisztika, képernyőre
            // Összetett adatszerkezet használata
            // 5 millió alatti
            // 5-10 közötti
            // 10 millió feletti városok száma
            Console.WriteLine("14. feladat");
            db = 0;
            foreach (var i in v)
            {
                if (i.LakosságMill < 5)
                {
                    db++;
                }
            }
            Console.WriteLine($"\t5 mill alatt {db} db");

            db = 0;
            foreach (var i in v)
            {
                if (i.LakosságMill > 5 && i.LakosságMill < 10)
                {
                    db++;
                }
            }
            Console.WriteLine($"\t5 és 10 mill között {db} db");

            db = 0;
            foreach (var i in v)
            {
                if (i.LakosságMill > 10 )
                {
                    db++;
                }
            }
            Console.WriteLine($"\t10 mill felett {db} db");

            //Ugyanez pepitában

            Dictionary<string, int> statisztika = new Dictionary<string, int>();
            statisztika.Add("5 mill. alatt", 0);
            statisztika.Add("5 és 10 mill. között", 0);
            statisztika.Add("10 mill. felett", 0);

            foreach (var i in v)
            {
                if (i.LakosságMill < 5)
                {
                    statisztika["5 mill. alatt"]++;
                }
                else if (i.LakosságMill > 5 && i.LakosságMill < 10)
                {
                    statisztika["5 és 10 mill. között"]++;
                }
                else
                {
                    statisztika["10 mill. felett"]++;
                }
            }
            foreach (var i in statisztika)
            {
                Console.WriteLine($"\t{i.Key} {i.Value} db");
            }

            // 15. feladat:
            // Határozza meg és írja ki a képernyőre 
            // az európai nagyvárosok városok lakosságának átlagát
            Console.WriteLine("15. feladat");

            
            List<string> Eu = new List<string>
            {
                { "Ukrajna" },
                { "Törökország" },
                { "Görögország"},
                { "Spanyolország"},
                { "Nagy-Britannia"},
                { "Németország" }

            };
            foreach (var i in Eu)
            {
                Console.WriteLine($"\t{i} átlagosan {v.Where(x => x.Ország == i).Average(y => y.Lakosság)} ezer fő");
            }
            

            Console.ReadKey();
        }
    }
}
