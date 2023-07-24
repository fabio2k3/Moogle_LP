using System.Text.RegularExpressions;

namespace MoogleEngine;

public class Query
{
    public string strQuery = String.Empty;
    public Dictionary<int, string> dicWordsOfQuery = new Dictionary<int, string>();
    public int countwordsInQuery;
    

    public void Inicializar(string query)
    {
        strQuery = query;
        strQuery = Tools.CleanQuery(strQuery);
        this.FillDictionaryQuery();
        countwordsInQuery = dicWordsOfQuery.Count;
    }

    public void FillDictionaryQuery()
    {
        //Método que adiciona cada palabra del query, al diccionario: dicWordsOfQuery
        string aux = strQuery; string wordtoadd;
        int keyDic = 0; int pos = aux.IndexOf(" ");
        while (pos != -1)
        {
            wordtoadd = aux.Substring(0, pos);
            dicWordsOfQuery.Add(keyDic, wordtoadd);
            keyDic++;
            aux = aux.Substring(pos).Trim();
            pos = aux.IndexOf(" ");
        }
        if (aux.Length != 0)
            dicWordsOfQuery.Add(keyDic, aux);
    }

}
