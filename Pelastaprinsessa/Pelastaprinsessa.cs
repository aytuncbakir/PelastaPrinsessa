using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using Jypeli.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


/// @author Aytunc Bakir
/// @version 04.04.2022
/// <summary>
/// Pelastaprinsessa 
/// </summary>
public class Pelastaprinsessa : PhysicsGame
{


    /// <summary>
    /// Funktion kutsu.
    /// </summary>
    public override void Begin()
    {
        StringUtils.LueAlkuTiedotTiedostosta();
        LuoKentta();
        MediaPlayer.Play("pelitaso" + AlkuArvot.tallennetutArvot[2]);
        LisaaLaskurit();
    }


    /// <summary>
    /// Tämä funktio luo pelin kentän.
    /// </summary>
    public void LuoKentta()
    {

        SetWindowSize(1700, 900);
        CenterWindow();
        Level.Size = new Vector(1700, 900);

        TileMap kentta = TileMap.FromLevelAsset("kentta" + AlkuArvot.tallennetutArvot[2]);
        for (int i = 0; i < AlkuArvot.rakennaKentta.Length; i++)
        {
            string[] arvot = StringUtils.SplitSanat(',', AlkuArvot.rakennaKentta[i]);
            if (arvot[1] == "LuoTaso")
                kentta.SetTileMethod(char.Parse(arvot[0]), LuoTaso, arvot[2]);
            if (arvot[1] == "LuoVihu")
                kentta.SetTileMethod(char.Parse(arvot[0]), LuoVihu, Int32.Parse(arvot[2]));
            else if (arvot[1] == "LuoKerattava")
                kentta.SetTileMethod(char.Parse(arvot[0]), LuoKerattava, arvot[2]);
            else if (arvot[1] == "LuoPrinsessa")
                kentta.SetTileMethod(char.Parse(arvot[0]), LuoPrinsessa, arvot[2]);
            else if (arvot[1] == "LuoPelaaja")
                kentta.SetTileMethod(char.Parse(arvot[0]), LuoPelaaja, arvot[2]);
        }
        kentta.Execute(AlkuArvot.RUUDUN_LEVEYS, AlkuArvot.RUUDUN_KORKEUS);
        kentta.Optimize();
        Level.CreateBorders();
        Camera.ZoomToLevel();
        Gravity = new Vector(0, -1000);
    }


    /// <summary>
    /// Tämä funktio laittaa laskurit peliin.
    /// </summary>
    public void LisaaLaskurit()
    {
        string elamat = "Elamat";
        string pisteet = "Pisteet";
        string taso = "Taso";
        string tormays = "Törmays";
        AlkuArvot.pelaajanElemat = LuoLaskuri(Screen.Left + 400.0, Screen.Top - 50.0, elamat,0);
        AlkuArvot.montakoTormataLaskuri = LuoLaskuri(Screen.Left + 400.0, Screen.Top - 100.0, tormays, 3);

        AlkuArvot.tasoLaskuri = LuoLaskuri(Screen.Right - 400.0, Screen.Top - 50.0, taso, 2);
        AlkuArvot.pelaajanPisteet = LuoLaskuri(Screen.Right - 400.0, Screen.Top - 100.0, pisteet,1);
    }


    /// <summary>Luo laskuri</summary>
    /// <param name="x">x-position</param>
    /// <param name="y">y-position</param>
    /// <param name="LaskurinNimi">Laskurin nimi</param>
    /// <param name="arvonIndeksi">Laskurin value</param>
    /// <returns>laskuri</returns>
    public IntMeter LuoLaskuri(double x, double y, string LaskurinNimi, int arvonIndeksi)
    { 
        Label naytto = new Label();
        IntMeter laskuri = AsentaMaksimiArvot(arvonIndeksi);
        naytto.Title= LaskurinNimi.ToUpper()+": ";
        naytto.BindTo(laskuri);
        naytto.X = x;
        naytto.Y = y;
        naytto.TextColor = Color.Black;
        naytto.BorderColor = Level.Background.Color;
        naytto.Color = Color.White;
        Add(naytto);
        return laskuri;
    }


    /// <summary>Asenta maksimi arvot laskuri</summary>
    /// <param name="arvonIndeksi">Laskurin value</param>
    /// <returns>laskuri</returns>
    public IntMeter AsentaMaksimiArvot(int arvonIndeksi)
    {
        IntMeter laskuri = null;
        Pelaaja pelaaja = Pelaaja.GetPelaaja();
        if (arvonIndeksi == 0) 
            laskuri = new IntMeter(pelaaja.GetElamiaJaljella());
        else
            laskuri = new IntMeter(AlkuArvot.tallennetutArvot[arvonIndeksi]);
        laskuri.MaxValue = AlkuArvot.maxArvot[arvonIndeksi];
        return laskuri;
    }


    /// <summary>Luo pelaaja</summary>
    /// <param name="leveys"> leveys</param>
    /// <param name="korkeus"> korkeus </param>
    /// <param name="kuvanNimi">Pelaajan kuva</param>
    public void LuoPelaaja(Vector paikka, double leveys, double korkeus, string kuvanNimi)
    {
        Pelaaja pelaaja = Pelaaja.GetPelaaja();
        pelaaja.Position = paikka;
        pelaaja.Image = LoadImage(kuvanNimi);
        pelaaja.Tag = kuvanNimi;
        Pelaaja.SetAmmu(bool.Parse(AlkuArvot.isAmmu));
        Add(pelaaja);
        AsentaPelaajanCollisions();
    }


    /// <summary>Pelaaja hyppaa</summary>
    /// <param name="pelaaja"> Pelaaja olio</param>
    /// <param name="voima"> hyppamisen voima </param>
    public void PelaajaHyppaa(PlatformCharacter pelajaa, double voima)
    {
        pelajaa.Jump(voima);
    }


    /// <summary>Pelaaja likkuu</summary>
    /// <param name="pelaaja"> Pelaaja olio</param>
    /// <param name="suunta"> liikkumisen suunta </param>
    public void PelaajaLiikuta(PlatformCharacter pelaaja, double suunta)
    {
        if (suunta > 0)
            AlkuArvot.ammuSuunta = true;
        else
            AlkuArvot.ammuSuunta = false;
        pelaaja.Walk(suunta);
    }


    /// <summary>Asenta pelaajan collisions </summary>
    public void AsentaPelaajanCollisions()
    {
        Pelaaja pelaaja = Pelaaja.GetPelaaja();
        AddCollisionHandler<Pelaaja, PhysicsObject>(pelaaja, "tahti", PelaajaTormasiKerattavan);
        AddCollisionHandler<Pelaaja, PhysicsObject>(pelaaja, "prinsessa", PelaajaTormasiKerattavan);
        AddCollisionHandler<Pelaaja, PhysicsObject>(pelaaja, "sydan", PelaajaTormasiKerattavan);
        AddCollisionHandler<Pelaaja, PhysicsObject>(pelaaja, "omena", PelaajaTormasiKerattavan);
        AddCollisionHandler<Pelaaja, Vihu>(pelaaja, PelaajaTormasiVihuun);
        AddCollisionHandler<Pelaaja, PhysicsObject>(pelaaja, PelaajaTormasiVihunHeitettava);
        AddCollisionHandler<Pelaaja, PhysicsObject>(pelaaja, PelaajaTormasiVarallisenTasoon);

        Keyboard.Listen(Key.Right, ButtonState.Down, PelaajaLiikuta, "Lopeta pelaaja oikealle", pelaaja, pelaaja.GetLiikutusvoima());
        Keyboard.Listen(Key.Left, ButtonState.Down, PelaajaLiikuta, "Lopeta pelaaja vasemmalle", pelaaja, -pelaaja.GetLiikutusvoima());
        Keyboard.Listen(Key.Up, ButtonState.Pressed, PelaajaHyppaa, "Pelaaja hyppää", pelaaja, pelaaja.GetHyppyvoima());
        Keyboard.Listen(Key.Space, ButtonState.Pressed, Heita, "Heita ammus", pelaaja, "vihu", Shape.Hexagon, Color.Yellow);

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }


    /// <summary>Luo taso</summary>
    /// <param name="leveys"> leveys</param>
    /// <param name="korkeus"> korkeus </param>
    /// <param name="kuvanNimi">Tason kuva</param>
    public void LuoTaso(Vector paikka, double leveys, double korkeus, string kuvanNimi)
    {
        PhysicsObject taso = new PhysicsObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Image = LoadImage(kuvanNimi);
        taso.Tag = kuvanNimi;

        if (taso.Tag.ToString() == "lattia3")
        {
            AlkuArvot.rajahdysVector = paikka;
            AlkuArvot.rajahdysTaso = taso;
        }
        taso.MakeStatic();
        Add(taso);
    }


    /// <summary>Pelaaja tormasi varallisen tasoon</summary>
    /// <param name="pelaaja"> pelaaja</param>
    /// <param name="taso"> taso </param>
    public void PelaajaTormasiVarallisenTasoon(Pelaaja pelaaja, PhysicsObject taso)
    {
        pelaaja = Pelaaja.GetPelaaja();
        
        if(taso.Tag.ToString() == "lattia2" || taso.Tag.ToString() == "vetta")
        {
            Pelaaja.SetAmmu(false);
            pelaaja.OtaVastaanOsuma();
            SoitaEpaonnustumisenMusiikit(taso.Tag.ToString());
            TallennaPelinArvot(); 
            LataaUudestanPeli();
        }
    }


    /// <summary>Soita epäonnustumisen laulua</summary>
    /// <param name="musiikinNimi"> musiikin nimi </param>
    public void SoitaEpaonnustumisenMusiikit(string musiikinNimi)
    {
        PlaySound(musiikinNimi);
        PlaySound("epaonnistuminen");
    }


    /// <summary>Tallenna pelin</summary>
    public void TallennaPelinArvot()
    {
        StringUtils.ClearFile();
        if (AlkuArvot.tallennetutArvot[2] > AlkuArvot.tasoMaksimi )
        {
            StringUtils.KirjoitaFile("elamat:" + 5);
            StringUtils.KirjoitaFile("pisteet:" + 0);
            StringUtils.KirjoitaFile("taso:" + 1);
            StringUtils.KirjoitaFile("montakoTormata:" + 0);
            StringUtils.KirjoitaFile("isammu:" + false);
            LataaUudestanPeli();
        }
        else
        {
            if(Pelaaja.GetPelaaja().GetElamiaJaljella() < 1)
                StringUtils.KirjoitaFile("elamat:" + 5);
            else
            StringUtils.KirjoitaFile("elamat:" + Pelaaja.GetPelaaja().GetElamiaJaljella());

            StringUtils.KirjoitaFile("pisteet:" + AlkuArvot.pelaajanPisteet.Value);
            StringUtils.KirjoitaFile("taso:" + AlkuArvot.tallennetutArvot[2]);
            StringUtils.KirjoitaFile("montakoTormata:" + AlkuArvot.tallennetutArvot[3]);
            StringUtils.KirjoitaFile("isammu:" + Pelaaja.Isammu());
        }

    }


    /// <summary>Pelaaja tormasi vihuun</summary>
    /// <param name="pelaaja"> pelaaja</param>
    /// <param name="vihu"> vihu </param>
    public void PelaajaTormasiVihuun(Pelaaja pelaaja, Vihu vihu)
    {
        AlkuArvot.tallennetutArvot[3]++;
        TarkistaMontakotormasi();
    }


    /// <summary>Tarkista, että montakotormasi on nyt</summary>
    public void TarkistaMontakotormasi()
    {
        if (AlkuArvot.tallennetutArvot[3] == 3)
        {
            Pelaaja.SetAmmu(false);
            SoitaEpaonnustumisenMusiikit("lattia2");
            Pelaaja.GetPelaaja().OtaVastaanOsuma();
            AlkuArvot.rajahdysPisteet = 0;
            AlkuArvot.tallennetutArvot[3] = 0;
            TallennaPelinArvot();
            LataaUudestanPeli();
        }
        //MessageDisplay.Add("" + AlkuArvot.montakoTormata);
    }


    /// <summary>Pelaaja tormasi vihun heitettava</summary>
    /// <param name="pelaaja"> pelaaja</param>
    /// <param name="kohde"> vihun heitettava </param>
    public void PelaajaTormasiVihunHeitettava(Pelaaja pelaaja, PhysicsObject kohde)
    {
        //MessageDisplay.Add("Kerättiin vihuuun");
        //vihu.OtaVastaanOsuma();

        if (kohde.Tag.ToString() == "vihunHeito")
        {
            AlkuArvot.tallennetutArvot[3]++;
            AlkuArvot.montakoTormataLaskuri.Value += 1;
            kohde.Destroy();
        }
        TarkistaMontakotormasi();
    }


    /// <summary>Lataa peli uudestaan</summary>
    public void LataaUudestanPeli()
    {
        AlkuArvot.rajahdysPisteet = 0;
        Pelaaja.GetPelaaja().SetPelaaja();
        ClearAll();
        Begin();
    }


    /// <summary>Pelaaja tormasi kerattava</summary>
    /// <param name="pelaaja"> pelaaja</param>
    /// <param name="kohde"> kerattava </param>
    public void PelaajaTormasiKerattavan(Pelaaja tormaaja, PhysicsObject kohde)
    {
        //MessageDisplay.Add(kohde.Tag.ToString());
        if (kohde.Tag.ToString() == "tahti")
        {
            kohde.Destroy();
            AlkuArvot.pelaajanPisteet.Value += 100;
            AlkuArvot.rajahdysPisteet += 100;
            PlaySound("menestys");
            TarkistaRajahdys("tahti");
        }
        else if (kohde.Tag.ToString() == "prinsessa")
        {
            AlkuArvot.pelaajanPisteet.Value += 200;
            TarkistaRajahdys("prinsessa");
           
        }
        else if (kohde.Tag.ToString() == "sydan")
        {
            //MessageDisplay.Add(kohde.Tag.ToString());
            Pelaaja.GetPelaaja().OtaSydaanOsuma();
            AlkuArvot.pelaajanElemat.Value = Pelaaja.GetPelaaja().GetElamiaJaljella();
            kohde.Destroy();
            PlaySound("menestys");
        }
        else if (kohde.Tag.ToString() == "omena")
        {     
            kohde.Destroy();
            Pelaaja.SetAmmu(true);
            PlaySound("menestys");
        }
    }


    /// <summary>Tarkista rajahdyksia</summary>
    /// <param name="pelaaja"> pelaaja</param>
    /// <param name="kohde"> kerattava </param>
    public void TarkistaRajahdys(string kerattavaTyyppi)
    {
        if (kerattavaTyyppi == "tahti")
        {
            if (AlkuArvot.rajahdysPisteet >= AlkuArvot.seinanRajahdyksenPisteet)
            {
                Rajahdys("exp", "bum");
                AlkuArvot.rajahdysTaso.Destroy();
                AlkuArvot.rajahdysPisteet = 0;
               
            }
            else if (AlkuArvot.rajahdysPisteet == AlkuArvot.sanoRakastansinuaPisteet)
            {
                Rajahdys("rakastansinua", "sydan");
            }
        }
        else if (kerattavaTyyppi == "prinsessa")
        {
            Rajahdys("peliloppu", "sydan");
            AlkuArvot.tallennetutArvot[2]++; 
            TallennaPelinArvot();
            LataaUudestanPeli();
        }
    }


    /// <summary>Rajahdys tapahtuu</summary>
    /// <param name="musiikki"> pelaaja</param>
    /// <param name="kuvanNimi"> rajahdys olion kuva </param>
    public void Rajahdys(string musiikki, string kuvanNimi)
    {
        PlaySound(musiikki);
        Image rajahdysKuva = LoadImage(kuvanNimi);
        AlkuArvot.explosionSystem = new ExplosionSystem(rajahdysKuva, 50);
        Add(AlkuArvot.explosionSystem);
        AlkuArvot.explosionSystem.AddEffect(AlkuArvot.rajahdysVector.X, AlkuArvot.rajahdysVector.Y, 30);
    }


    /// <summary>Luo kerattava</summary>
    /// <param name="paikka"> kerattavan paikka</param>
    /// <param name="leveys"> leveys</param>
    /// <param name="korkeus"> korkeus </param>
    /// <param name="kuvanNimi">kerattavan kuva</param>
    public void LuoKerattava(Vector paikka, double leveys, double korkeus, string kuvanNimi)
    {
        PhysicsObject kerattava = new PhysicsObject(leveys, korkeus);
        kerattava.Position = paikka;
        kerattava.MakeStatic();
        kerattava.Image = LoadImage(kuvanNimi);
        //MessageDisplay.Add(kuvanNimi);
        kerattava.Tag = kuvanNimi;
        Add(kerattava);
    }


    /// <summary>Luo prinsessa</summary>
    /// <param name="paikka"> prinsessan paikka</param>
    /// <param name="leveys"> leveys</param>
    /// <param name="korkeus"> korkeus </param>
    /// <param name="kuvanNimi">prinsessa kuva</param>
    public void LuoPrinsessa(Vector paikka, double leveys, double korkeus, string kuvanNimi)
    {
        PhysicsObject prinsessa = new PhysicsObject(leveys, korkeus);
        prinsessa.Position = paikka;
        //MessageDisplay.Add(kuvanNimi);
        prinsessa.Tag = kuvanNimi;
        prinsessa.CanRotate = false;
        prinsessa.Image = LoadImage(kuvanNimi);
        Add(prinsessa);
        Lisaabrainoliolle(prinsessa, 2);
        //MessageDisplay.Add("********");
    }


    /// <summary>Luo vihu</summary>
    /// <param name="paikka"> vihun paikka</param>
    /// <param name="leveys"> leveys</param>
    /// <param name="korkeus"> korkeus </param>
    /// <param name="kuvanNimi">vihun kuva</param>
    public void LuoVihu(Vector paikka, double leveys, double korkeus, int liikemaara)
    {
        Vihu vihu = new Vihu(leveys, korkeus, 3);
        vihu.Shape = Shape.Circle;
        vihu.Position = paikka;
        vihu.Tag = "vihu";
        vihu.CanRotate = false;
        vihu.Image = LoadImage("" + liikemaara);
        Add(vihu);
        Lisaabrainoliolle(vihu, liikemaara);
        Aloitaheitto(vihu);
    }


    /// <summary>vihu aloittaa heittamaan</summary>
    /// <param name="vihu"> vihu olio</param>
    public void Aloitaheitto(Vihu vihu)
    {
        Timer heittoajastin = new Timer();
        heittoajastin.Interval = 6.0;
        heittoajastin.Timeout += delegate () { Heita(vihu, "pelaaja", Shape.Pentagon, Color.DarkGreen); };
        heittoajastin.Start();
        vihu.Destroyed += heittoajastin.Stop;
    }


    /// <summary>Lisaa brain oliolle</summary>
    /// <param name="olio"> pelin olio</param>
    /// <param name="liikemaara"> olion liikemaara</param>
    public void Lisaabrainoliolle(PhysicsObject olio, int liikemaara)
    {
        PathFollowerBrain pfb = new PathFollowerBrain();
        List<Vector> reitti = new List<Vector>();
        reitti.Add(olio.Position);
        Vector seuraavaPiste = olio.Position + new Vector(liikemaara * 40, 0);
        reitti.Add(seuraavaPiste);
        pfb.Path = reitti;
        pfb.Loop = true;
        pfb.Speed = 40;
        olio.Brain = pfb;
    }


    /// <summary>Heita</summary>
    /// <param name="heittavaOlio"> pelaaja tai vihu</param>
    /// <param name="tunniste"> kuka olio on</param>
    /// <param name="muoto"> heitettavan muoto</param>
    /// <param name="color"> heitettavan color</param>
    public void Heita(PhysicsObject heittavaOlio, string tunniste, Shape muoto, Color color)
    {
        //MessageDisplay.Add(puole.ToString());
        if (tunniste == "vihu")
        {
            if (Pelaaja.Isammu())
            {
                HeitaaAsenta(heittavaOlio, tunniste, muoto, color);
            }
        }
        else if (tunniste == "pelaaja")
        {
            HeitaaAsenta(heittavaOlio, tunniste, muoto, color);
        }
    }


    /// <summary>HeitaaAsenta</summary>
    /// <param name="heittavaOlio"> pelaaja tai vihu</param>
    /// <param name="tunniste"> kuka olio on</param>
    /// <param name="muoto"> heitettavan muoto</param>
    /// <param name="color"> heitettavan color</param>
    public void HeitaaAsenta(PhysicsObject heittavaOlio, string tunniste, Shape muoto, Color color)
    {
        PhysicsObject heitettava = new PhysicsObject(AlkuArvot.RUUDUN_LEVEYS / 2, AlkuArvot.RUUDUN_KORKEUS / 2);
        heitettava.Shape = muoto;
        if (muoto == Shape.Pentagon)
            heitettava.Tag = "vihunHeito";
        heitettava.Color = color;
        heitettava.Mass = 5;
        AddCollisionHandler<PhysicsObject, Vihu>(heitettava, tunniste, HeitettavaTormasi);
        if (AlkuArvot.ammuSuunta)
        {
            heitettava.Position = heittavaOlio.Position + new Vector(AlkuArvot.RUUDUN_LEVEYS / 2, AlkuArvot.RUUDUN_KORKEUS / 2);
            heitettava.Hit(new Vector(1500, 1500));
        }
        else
        {
            heitettava.Position = heittavaOlio.Position + new Vector(-AlkuArvot.RUUDUN_LEVEYS / 2, AlkuArvot.RUUDUN_KORKEUS / 2);
            heitettava.Hit(new Vector(-1500, 1500));
        }
        heitettava.MaximumLifetime = TimeSpan.FromSeconds(1);
        Add(heitettava);
        PlaySound("ammu");
    }


    /// <summary>Pelaajan heito törmasi vihuun</summary>
    /// <param name="pelaaja"> pelaaja</param>
    /// <param name="vihu"> vihu</param>
    public void HeitettavaTormasi(PhysicsObject pelaaja, Vihu vihu)
    {
        //MessageDisplay.Add("ammun vihuun");
        vihu.OtaVastaanOsuma();
    }


}