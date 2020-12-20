using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Forditoprogram
{
    class Automata
    {
        string input = "";      // Maga az input
        int i;                  // Index az input kezeléséhez
        Stack<string> st;       // A stack, amibe pakolom a szabályokat
        List<int> sorszamok;    // Ebben a listában tárolom a lépések sorszámait, amiket alkalmaztam
        List<string[]> lepesek; // Ebben küldöm vissza a formnak külön tömbökben a megjelenítendő rendezett hármas elemeit
        string stack = "";      // Egy string, aminek a segítségével kiolvassuk, hogy mik találhatók a stackben
        string elem;            // A stack (tetején található) eleme
        string[,] matrix;       // A mátrix, amit a formból  hozunk a TextBox-ok alapján
        string sorszam;         // A rendezett hármas, harmadik eleme, a végrehajtott parancsok soszáma

        public string simple(string s0)
        {
            return Regex.Replace(s0, "[0-9]+", "i");
        }

        public Automata(string input, string[,] matrix)
        {
            i = 0;
            lepesek = new List<string[]>();
            sorszamok = new List<int>();
            st = new Stack<string>();
            st.Push("#");
            st.Push(matrix[1, 0]);

            if(input[input.Length - 1] == '#')
                this.input = $"{simple(input)}";
            else
                this.input = $"{simple(input)}#";

            this.matrix = matrix;
            readStack();
            lepesek.Add(new string[] { this.input, stack, "e" });
        }

        public List<string[]> validal()
        {
            while (elem != "#")
            {
                elem = st.Pop();
                string karakter = input[i].ToString();

                int sor = -1;
                int oszlop = -1;
                string ertek;

                // Leolvasom a keresett sor indexét a stack legfelső eleme alapján
                for (int k = 1; k < 12; k++)
                {
                    if (elem == matrix[k, 0])
                    {
                        sor = k;
                        break;
                    }
                }

                // Leolvasom a keresett oszlop indexét az input aktuálisan indexelt eleme alapján
                for (int k = 1; k < 7; k++)
                {
                    if (karakter == matrix[0, k])
                    {
                        oszlop = k;
                        break;
                    }
                }

                // A leolvasott indexek alapján tárolom a "cella" értékét
                try
                {
                    ertek = matrix[sor, oszlop];
                }
                catch(IndexOutOfRangeException e)
                {
                    lepesek.Add(new string[] { "Az automata hibára futott, a megadott input nem validálható! \nKilépő cella sora: " + elem + "\nKilépő cella oszlopa: " + karakter });
                    return lepesek;
                }


                switch (ertek)
                {
                    case "":
                        lepesek.Add(new string[] { "Az automata hibára futott, a megadott input nem validálható! \nKilépő cella sora: " + elem + "\nKilépő cella oszlopa: " + karakter });
                        return lepesek;
                    case "elfogad":
                        lepesek.Add(new string[] { "A bevitt input valid!" });
                        return lepesek;
                    case "pop":
                        i++;
                        break;
                    default: // Mikor a cella egy zárójeles szabályt tartalmaz
                        ertek = ertek.Substring(1, ertek.Length - 2);
                        string[] ertekek = ertek.Split(',');

                        string elso = ertekek[0];
                        string masodik = ertekek[1];

                        // Ha a szabály tartalmaz aposztrófot akkor kicsit bonyolodik a dolog, mivel alapvető esetben 
                        // azt is külön karakterként fogná fel és úgy tenné bele a verembe
                        if (elso.Contains('\''))
                        {
                            List<string> ap = new List<string>();
                            // Szétválasztjuk az aposztrófok mentén a szabályt
                            string[] asd = elso.Split('\'');

                            // Az első for arra az esetre kell, ha több aposztrófos jel is lenne a szabályban
                            for (int i = 0; i < asd.Length; i++)
                            {
                                // A második ciklus karakterenként menti a szabály jeleit egy listába
                                // és az utolsó elemnél (ahol az aposztróf elválasztatta a Splittel)
                                // hozzátesz a karakterhez egy aposztófot, hogy ne vesszen el a Split miatt
                                for (int j = 0; j < asd[i].Length; j++)
                                {
                                    if (j == asd[i].Length - 1)
                                    {
                                        ap.Add(asd[i][j].ToString() + "'");
                                    }
                                    else
                                    {
                                        ap.Add(asd[i][j].ToString());
                                    }
                                }
                            }

                            // Így egy elemként tudjuk odaadni a veremnek például az (E') jelet, ugyanúgy, mint a (T)-t
                            // A lista hátuljáról kezdve tesszük bele a verembe, hogy mikor kivesszük őket, a sorrend jó legyen
                            for (int i = ap.Count - 1; i >= 0; i--)
                            {
                                st.Push(ap[i]);
                            }
                        }
                        else if(elso != "e")
                        {
                            // Amennyiben a szabély nem tartalmaz aposztrofos jelet, szimplán karakterenként betehetjük őket a verembe
                            for (int i = elso.Length - 1; i >= 0 ; i--)
                            {
                                st.Push(elso[i].ToString());
                            }
                        }

                        sorszamok.Add(Int32.Parse(masodik));
                        readSorszam();
                        break; // default ág vége
                }

                string inPut = input.Substring(i);
                readStack(); 
                string[] tmp = new string[]
                {
                    inPut, stack, sorszam
                };
                lepesek.Add(tmp);
            }
            return null;
        }

        // A vermet "átmásolom" egy listába, így anélkül kiolvashatom az adatokat belőle, hogy ezzel módosítanám
        private void readStack()
        {
            stack = "";
            List<string> stCopy = st.ToList();
            for (int i = 0; i < stCopy.Count; i++)
            {
                stack += stCopy[i];
            }
        }


        private void readSorszam()
        {
            sorszam = "";
            foreach (int s in sorszamok)
            {
                sorszam += s.ToString();
            }
        }
    }
}
