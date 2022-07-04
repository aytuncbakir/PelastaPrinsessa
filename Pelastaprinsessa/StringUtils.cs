using System;
using System.IO;
using System.Linq;


/// @author Aytunc Bakir
/// @version 04.04.2021
/// <summary>
/// StringUtils
/// </summary>
public static class StringUtils
{

    public const string path = "C:\\Users\\aytun\\source\\repos\\ohj1ht\\Pelastaprinsessa\\Pelastaprinsessa\\Content\\lataa_peli.txt";
    private static string[] sanat = null;


    /// <summary>LueAlkuTiedotTiedostosta, kutsutaan funktiota:  LueFile(),SplitSanat()  </summary>
    public static void LueAlkuTiedotTiedostosta()
    {
        LueFile();
        SplitSanat();
    }


    /// <summary>LueFile: Lue tiedosto polkusta</summary>
    /// <returns>sanat taulukko</returns>
    public static string[] LueFile()
    {
        try
        {
            sanat = File.ReadAllLines(path);
            return sanat;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);  
        }

        return sanat;
    }


    /// <summary>SplitSanat, ':' merkkin mukaan</summary>
    public static void SplitSanat()
    {
        
        for(int i = 0; i<sanat.Length; i++)
        {
            string[] arvot = sanat[i].Split(':');
            if (arvot[0] == "elamat")
                AlkuArvot.tallennetutArvot[0] = Int32.Parse(arvot[1]);
            else if (arvot[0] == "pisteet")
                AlkuArvot.tallennetutArvot[1] = Int32.Parse(arvot[1]);
            else if (arvot[0] == "taso")
                AlkuArvot.tallennetutArvot[2] = Int32.Parse(arvot[1]);
            else if (arvot[0] == "montakoTormata")
                AlkuArvot.tallennetutArvot[3] = Int32.Parse(arvot[1]);
            else if(arvot[0] == "isammu")
                AlkuArvot.isAmmu = arvot[1];
        }

    }


    /// <summary>LueFile: Lue tiedosto polkusta</summary>
    /// <param name="token"> merkki</param>
    /// <param name="lause"> riivi </param>
    /// <returns>sanat taulukko</returns>
    public static string[] SplitSanat(char token, string lause)
    {
        string[] arvot = lause.Split(token);
        return arvot;
    }


    /// <summary>KirjoitaFile: kirjoita tiedostolle</summary>
    /// <param name="riivi"> merkkijono, lause</param>
    public static void KirjoitaFile(string rivi)
    {
        StreamWriter writer = null;
        try
        {    
            writer = File.AppendText(path);
            writer.WriteLine(rivi);
            writer.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


    /// <summary>ClearFile: tyhjennä tiedosto</summary>
    public static void ClearFile()
    {
        if (!File.Exists(path))
            File.Create(path);

        TextWriter tw = new StreamWriter(path, false);
        tw.Write(string.Empty);
        tw.Close();
    }


}
