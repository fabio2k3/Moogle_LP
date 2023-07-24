namespace MoogleEngine;

public class TFIDF
{
    public double[] arrayQueryTF { get; set; }
    public double[] arrayQueryIDF { get; set; }
    public double[] arraySimCoseno{ get; set; }


    public double[,] mtzGeneral;
    public double[,] mtzGeneralTFIDF;

    public int countElements = 0;

    public void Inicializar(Dictionary<int, string> dicFilesTxt, Dictionary<int, string> dicWordsOfQuery, Dictionary<int, string> dicFilesSnipper)
    {
        this.FillMatrizGeneral(dicFilesTxt, dicWordsOfQuery, dicFilesSnipper);
        this.UpdateArrayQueryTF(dicFilesTxt.Count, dicWordsOfQuery.Count);
        this.UpdateArrayQueryIDF(dicFilesTxt.Count, dicWordsOfQuery.Count);
        this.FillMatrizTFIDF(dicFilesTxt, dicWordsOfQuery);
        this.arraySimCoseno = new double[dicFilesTxt.Count];

    }
    public void FillMatrizGeneral(Dictionary<int, string> dicFilesTxt, Dictionary<int, string> dicWordsOfQuery, Dictionary<int, string> dicFilesSnipper)
    {
        //La cantidad de fila de la matriz mtzGeneral se corresponde con la cantidad de archivos TXT
        //La cantidad de columnas de la matriz mtzGeneral se corresponde con la cantidad de palabras de la query
        //Este método actualiza cada elemento de la matriz, calculando la cantidad de veces q aparece
        //cada palabra de la query en cada TXT
        mtzGeneral = new double[dicFilesTxt.Count, dicWordsOfQuery.Count];
        string line;
        int countwords = 0; int count = 0; int countaux = 0; bool found = false; string snipperline = String.Empty; int pos = 0; int endline = 0;
  
        foreach (int i in dicFilesTxt.Keys)
        {
            countwords = 0; count = 0; countaux = 0; found = false; snipperline = String.Empty; found = false;
            for (int j = 0; j < dicWordsOfQuery.Count; j++)
            {
                count = 0;  
                StreamReader sr = new StreamReader(Tools.GetDirectory() + dicFilesTxt[i]);
                line = sr.ReadLine();
                line = Tools.CleanLineTxt(line);

                while (line != null)
                {
                    countaux = Tools.CountWordsInLine(dicWordsOfQuery[j], line);
                    if (countaux > 0)
                    {
                        if (found == false)
                        {
                            pos = line.IndexOf(dicWordsOfQuery[j].ToString());
                            snipperline = line.Substring(pos);
                            if (snipperline.Length > Tools.countcharacters) 
                                snipperline = snipperline.Substring(0, Tools.countcharacters);
                            found = true;
                        }
                    }
                        
                    count = count + countaux;
                    countwords = countwords + Tools.CountWordsInTxt(line);
                    line = sr.ReadLine();
                }
                sr.Close();
                mtzGeneral[i, j] = count;
            }
            if (found)
                dicFilesSnipper[i] = snipperline;

        }
    }
    
    public void UpdateArrayQueryTF(int countfiles, int countwords)
    {
        //Método que calcula el TF para cada palabra del query y lo almacena en arrayQueryTF
        arrayQueryTF = new double[countwords];

        for (int i = 0; i < countwords; i++)
        {
            int count = 0;
            for (int j = 0; j < countfiles; j++)
                if (mtzGeneral[j, i] != 0)
                    count++;
            if (count == 0)
                arrayQueryTF[i] = 0;
            else
            {
                if (countwords > 1)
                    arrayQueryTF[i] = Math.Log(count + 1, 2) / Math.Log(countwords, 2);
                else
                    arrayQueryTF[i] = 1;
            }
                
        }
    }
    
    public void UpdateArrayQueryIDF(int countfiles, int countwords)
    {
        //Método que calcula el IDF para cada palabra del query y lo almacena en arrayQueryIDF
        arrayQueryIDF = new double[countwords];

        for (int i = 0; i < countwords; i++)
        {
            int count = 0;
            for (int j = 0; j < countfiles; j++)
            {
                if (mtzGeneral[j, i] != 0)
                    count++;
            }
            if (count == 0)
                arrayQueryIDF[i] = 0;
            else
                arrayQueryIDF[i] = Math.Log(1 + (countfiles / count),2);
        }

    }
    
    public void FillMatrizTFIDF(Dictionary<int, string> dicFilesTxt, Dictionary<int, string> dicWordsOfQuery)
    {
        //La cantidad de fila de la matriz mtzGeneralTFIDF se corresponde con la cantidad de archivos TXT
        //La cantidad de columnas de la matriz mtzGmtzGeneralTFIDFeneral se corresponde con la cantidad de palabras de la query
        //Este método actualiza cada elemento de la matriz, calculando el TF * IDF de cada palabra del query
        mtzGeneralTFIDF = new double[dicFilesTxt.Count, dicWordsOfQuery.Count];
        string line;

        foreach (int i in dicFilesTxt.Keys)
        {
            int countwords = 0;
            for (int j = 0; j < dicWordsOfQuery.Count; j++)
            {
                int count = 0;
                StreamReader sr = new StreamReader(Tools.GetDirectory() + dicFilesTxt[i]);
                line = sr.ReadLine();
                line = Tools.CleanLineTxt(line);

                while (line != null)
                {
                    count = count + Tools.CountWordsInLine(dicWordsOfQuery[j], line);
                    countwords = countwords + Tools.CountWordsInTxt(line);
                    line = sr.ReadLine();
                }

                //Calcula TF
                double tf = (Math.Log(count + 1, 2)) / (Math.Log(countwords, 2));
                sr.Close();
                mtzGeneralTFIDF[i, j] = tf * arrayQueryIDF[j];
            }
        }
    }
    public void UpdateSimilitudCoseno(int countfiles, int countwords)
    {
        int count = 0;
        for (int i = 0; i < countfiles; i++)
        {
            double numerador = 0;
            double denominador1 = 0;
            double denominador2 = 0;

            for (int j = 0; j < countwords; j++)
                numerador = numerador + (mtzGeneralTFIDF[i, j] * arrayQueryIDF[j]);

            for (int j = 0; j < countwords; j++)
                denominador1 = denominador1 + (Math.Pow(mtzGeneralTFIDF[i, j], 2));

            denominador1 = Math.Sqrt(denominador1);

            for (int j = 0; j < countwords; j++)
                denominador2 = denominador2 + (Math.Pow(arrayQueryTF[j], 2)); //Debe ser arrayQueryTFIDF

            denominador2 = Math.Sqrt(denominador2);

            if ((denominador1 != 0) && (denominador2 != 0))
                arraySimCoseno[i] = numerador / (denominador1 * denominador2);

            if (arraySimCoseno[i] > 0)
                count++;
        }
        this.countElements = count;
    }
 
 
}

