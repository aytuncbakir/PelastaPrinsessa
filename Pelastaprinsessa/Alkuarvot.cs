using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using Jypeli.Effects;


/// @author Aytunc Bakir
/// @version 04.04.2022
/// <summary>
/// Pelin alkuarvot tassa luokassa 
/// </summary>
public static class  AlkuArvot
{
   
    public static int[] tallennetutArvot = new int[4]; // {elemat, pisteet,taso, montakoTormata}
    public static string isAmmu;
    public static int rajahdysPisteet;
    public static int tasoMaksimi = 10;
    public static int[] maxArvot = { 100, 10000, 10 , 3 };  // {elamatMaksimi,pisteetMaksimi, tasoMaksimi, montakoTormataMaksimi}

    public static bool ammuSuunta = false;

    public static ExplosionSystem explosionSystem;

    public static Vector rajahdysVector;
    public static PhysicsObject rajahdysTaso;

    public static IntMeter pelaajanPisteet;
    public static IntMeter pelaajanElemat;
    public static IntMeter tasoLaskuri;
    public static IntMeter montakoTormataLaskuri;

    public const double RUUDUN_LEVEYS = 40;
    public const double RUUDUN_KORKEUS = 40;

    public static int seinanRajahdyksenPisteet = 1000;
    public static int sanoRakastansinuaPisteet = 800;

    public static string[] rakennaKentta = { 
                            "x,LuoTaso,lattia",
                            "t,LuoTaso,lattia",
                            "T,LuoTaso,lattia2",
                            "w,LuoTaso,vetta",
                            "P,LuoTaso,lattia3",
                            "p,LuoPelaaja,pelaaja",
                            "5,LuoVihu,5",  
                            "4,LuoVihu,4",  
                            "3,LuoVihu,3",  
                            "2,LuoVihu,2",  
                            "*,LuoKerattava,tahti",
                            "o,LuoKerattava,omena",
                            "?,LuoKerattava,sydan",
                            "m,LuoPrinsessa,prinsessa"
            };
}