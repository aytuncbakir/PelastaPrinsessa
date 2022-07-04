using System;
using Jypeli;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// @author Aytunc Bakir
/// @version 04.04.2022
/// <summary>
/// Pelaaja luokka.
/// </summary>
public class Pelaaja : PlatformCharacter
{

    private int elamiaJaljella;
    private const double LIIKUTUSVOIMA = 200;
    private const double HYPPYVOIMA = 800;
    private static Pelaaja pelaaja = null;
    private static bool isAmmu;


    /// <summary>Pelaahan consructor</summary>
    /// <param name="width"> width</param>
    /// <param name="height"> height </param>
    /// <param name="elamat">Pelaajan elemat</param>
    /// <param name="ammuvalue">Mihin pelaaja heittaa</param>
    private Pelaaja(double width, double height,int elamat,bool ammuvalue) : base(width, height)
    {
        this.elamiaJaljella = elamat;
        isAmmu = ammuvalue;
    }


    /// <summary>OtaVastaanOsuma, kun pelaajan törmaa vihulle, virallisen tasolle tai hän putoaa veteen  
    /// laske pelaajan elamat yhden</summary>
    public void OtaVastaanOsuma()
    {
        elamiaJaljella--;  
    }


    /// <summary>OtaSydaanOsuma, kun pelaajan kerää sydamen hän saa ykis lisää elemä</summary>
    public void OtaSydaanOsuma()
    {
        elamiaJaljella++;
    }


    /// <summary>GetElamiaJaljella, anna pelaajan elämät jäljellä</summary>
    public int GetElamiaJaljella()
    {
        return elamiaJaljella;
    }


    /// <summary>SetAmmu,set pelaajan ammukyky</summary>
    public static void SetAmmu(bool value)
    { 
        isAmmu = value;
    }


    /// <summary>Isammu,onka pelaajalla ammukyky</summary>
    public static bool Isammu() 
    {
        return isAmmu;
    }


    /// <summary>SetPelaaja,set pelaaja to null</summary>
    public void SetPelaaja()
    {
        pelaaja = null;
    }


    /// <summary>GetPelaaja: anna pelaaja, jos hän elossa. jos hän kuoli, luo pelaajan uudestaan</summary>
    /// <returns>pelaaja</returns>
    public static Pelaaja GetPelaaja()
    {
        if(pelaaja == null)
            pelaaja = new Pelaaja(AlkuArvot.RUUDUN_LEVEYS, AlkuArvot.RUUDUN_KORKEUS,AlkuArvot.tallennetutArvot[0], Isammu());
        return pelaaja;
    }


    /// <summary>GetLiikutusvoima: anna pelaajan liikutusvoima</summary>
    /// <returns>liikutusvoima</returns>
    public double GetLiikutusvoima()
    {
        return LIIKUTUSVOIMA;
    }


    /// <summary>GetHyppyvoima: anna pelaajan hyppyvoima</summary>
    /// <returns>liikutusvoima</returns>
    public double GetHyppyvoima()
    {
        return HYPPYVOIMA;
    }


}

