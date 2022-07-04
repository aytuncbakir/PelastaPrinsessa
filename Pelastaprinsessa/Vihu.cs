using Jypeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// @author Aytunc Bakir
/// @version 04.04.2022
/// <summary>
/// Vihu Luokka
/// </summary>
public class Vihu : PhysicsObject
{
    private int elamiaJaljella;


    /// <summary>Vihun consructor</summary>
    /// <param name="width"> width</param>
    /// <param name="height"> height </param>
    /// <param name="elamat">Vihun elemat</param>
    public Vihu(double width, double height, int elamat) : base(width, height)
    {
        this.elamiaJaljella = elamat;
    }


    /// <summary>OtaVastaanOsuma, kun pelaajan heito törmaa vihulle laske vihun elemat yhden</summary>
    public void OtaVastaanOsuma()
    {
            elamiaJaljella--;
            if(elamiaJaljella < 0) this.Destroy();
    }


}