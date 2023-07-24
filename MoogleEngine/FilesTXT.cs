namespace MoogleEngine;
public class FilesTXT
{
    public Dictionary<int, string> dicFilesTxt = new Dictionary<int, string>();
    public Dictionary<int, string> dicFilesSnippet = new Dictionary<int, string>();
    public int countFilesTxt;

    public void Inicializar()
    {
        this.FillDictionaryTXT();
        countFilesTxt = dicFilesTxt.Count();
    }

    public void FillDictionaryTXT()
    {
        //Método que adiciona cada archivo TXT al diccionario: dicFilesTxt
        string[] files = Directory.GetFiles(Tools.GetDirectory(), "*.txt");
        int keyDic = 0;
        foreach (string file in files)
        {
            FileInfo fi = new FileInfo(file);
            dicFilesTxt.Add(keyDic, fi.Name);
            dicFilesSnippet.Add(keyDic, String.Empty);
            keyDic++;
        }
    }

}