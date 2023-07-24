namespace MoogleEngine;

public static class Moogle
{
    public static SearchResult Query(string query) 
    {
        SearchItem[] items = null;
        try
        {
            Query qry = new();
            qry.Inicializar(query);

            FilesTXT files = new FilesTXT();
            files.Inicializar();

            TFIDF tfidf = new TFIDF();
            tfidf.Inicializar(files.dicFilesTxt, qry.dicWordsOfQuery, files.dicFilesSnippet);
            tfidf.UpdateSimilitudCoseno(files.countFilesTxt, qry.countwordsInQuery);

            if (tfidf.countElements > 0)
            {
                items = new SearchItem[tfidf.countElements];

                int i = 0; int j = 0; int z = 0; int indice = 0;
                double major = 0;
                string namefile = string.Empty; string snipper = String.Empty;
                for (i = 0; i < files.countFilesTxt; i++)
                {
                    major = 0;
                    for (j = 0; j < files.countFilesTxt; j++)
                    {
                        if (tfidf.arraySimCoseno[j] > major)
                        {
                            major = tfidf.arraySimCoseno[j];
                            namefile = files.dicFilesTxt[j];
                            snipper = files.dicFilesSnippet[j];
                            indice = j;
                        }
                    }
                    if (major > 0)
                    {
                        items[z] = new SearchItem(namefile, snipper, 0.9f);
                        z++;
                    }
                    tfidf.arraySimCoseno[indice] = 0;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            //.WriteLine("Executing finally block. ");
        }
 
        return new SearchResult(items, query);
    }

}
