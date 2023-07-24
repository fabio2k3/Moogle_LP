using System.Text.RegularExpressions;

namespace MoogleEngine;
public class Tools
{
    public int countFilesTxt;
    
    public static string pathaccessTXT = @"..\..\Content\";
    public static string specialscharacters = "[!¡¿?.;:,(){}/\'@#%&%$]+";
    public static int countcharacters = 100; // Cantidad de caracteres a mostrar del fichero...
    public static List<string> listConjuncPrep = new List<string>() { "a", "ante", "bajo", "con", "contra", "de", "desde", "durante", "en", "entre", "hacia", "hasta", "mediante", "para", "por", "según", "sin", "sobre", "tras", "versus", "el", "la", "los", "las", "y", "e", "ni", "pero", "mas", "sino", "o", "u" };

    public static string GetDirectory()
    {
        return Directory.GetCurrentDirectory() + pathaccessTXT;
    }

    public static string CleanQuery(string strQuery)
    {
        strQuery = strQuery.ToLower().Trim();
        strQuery = Regex.Replace(strQuery, Tools.specialscharacters, " ");
        strQuery = Tools.RemoveAcents(strQuery);
 
        foreach (string c in Tools.listConjuncPrep)
            strQuery = strQuery.Replace(" " + c + " ", " ");

        strQuery = Tools.RemoveAcents(strQuery);
        return strQuery;


    }

    public static string CleanLineTxt(string line)
    {
        line = line.ToLower().Trim(); 
        line = Regex.Replace(line, Tools.specialscharacters, " ");
        line = Tools.RemoveAcents(line);
        return line;
    }

    public static int CountWordsInTxt(string line)
    {
        string[] word = line.Split(' ');
        return word.Length;
    }

    public static int CountWordsInLine(string wordToSearch, string line)
    {
        int count = 0; bool endofline = false;
        int pos = line.IndexOf(wordToSearch); 
        while (pos != -1)
        {
            //Se encontró la palabra a inicios de la línea
            if (pos == 0)
            { 
                if (line.Substring(wordToSearch.Length, 1) == " ")
                    count++;
            }
            else
            {
                // La palabra a buscar se encuentra al final de la linea
                if (line.Length == (pos + wordToSearch.Length))
                {
                    count++;
                    endofline = true;
                }
                else
                {
                    // La palabra a buscar se encuentra en el medio de la linea
                    if (line.Substring(pos - 1, 1) == " ")
                        count++;
                }

            }
            if (endofline == true)
                line = String.Empty;
            else
                line = line.Substring(pos + wordToSearch.Length + 1);

            pos = line.IndexOf(wordToSearch);

        }
        return count;
    }

    public static string RemoveAcents(string strToRemoveAcent)
    {
        Regex a = new Regex("[á|à|ä|â]",RegexOptions.Compiled);
        Regex e = new Regex("[é|è|ë|ê]",RegexOptions.Compiled);
        Regex i = new Regex("[í|ì|ï|î]",RegexOptions.Compiled);
        Regex o = new Regex("[ó|ò|ö|ô]",RegexOptions.Compiled);
        Regex u = new Regex("[ú|ù|ü|û]",RegexOptions.Compiled);

        strToRemoveAcent = a.Replace(strToRemoveAcent, "a");
        strToRemoveAcent = e.Replace(strToRemoveAcent, "e");
        strToRemoveAcent = i.Replace(strToRemoveAcent, "i");
        strToRemoveAcent = o.Replace(strToRemoveAcent, "o");
        strToRemoveAcent = u.Replace(strToRemoveAcent, "u");
        
        return strToRemoveAcent;
    }

}