using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Effects;
using Jypeli.Controls;
using Jypeli.Widgets;

namespace Laserpelin_paluu;

public class Laserpelin_paluu : PhysicsGame
{
    Cannon torni;
    PhysicsObject piippu;
    PhysicsObject pahis;
    PhysicsObject seinä;
    PhysicsObject seinä2;
    PhysicsObject body;
    PhysicsObject nelio;
    PhysicsObject pallo;
    PhysicsObject juusto;
    PhysicsObject soikio;
    PhysicsObject mega;
    int pMaxMaara = 200;
    Image rajahdyskuva = LoadImage("rajahdysp");
    List<Label> valikonKohdat;
    IntMeter pistelaskuri;
    
    Image pahiskuva = LoadImage("dorito-tyhja");
    Image neliokuva = LoadImage("pringles");
    Image seinäkuva = LoadImage("Seina-tyhja");
    Image pallokuva = LoadImage("pahkina");
    Image soikiokuva = LoadImage("taffel");
    Image megakuva = LoadImage("meganacho");
    public override void Begin()
    {
        //MultiSelectWindow alkuvalikko = new MultiSelectWindow("LaserPelin Paluu!", "Aloita peli", "Lopeta");
        //Add(alkuvalikko);
        //Mouse.ListenOn(, MouseButton.Left, ButtonState.Pressed, alkaa, null);
        

        
        //torni.ProjectileCollision = AmmusOsui;

        valikko();


    }

    void valikko()
    {
        MultiSelectWindow alkuvalikko = new MultiSelectWindow("LaserPelin Paluu!", "Aloita peli", "Lopeta", "Kaaos-pelimuoto");
        Add(alkuvalikko);
        
        alkuvalikko.AddItemHandler(0, alkaa);
        alkuvalikko.AddItemHandler(1, Exit);
        alkuvalikko.AddItemHandler(2, kaaos);
        
    }

    void alkaa()
    {
        luokenttä();
        luotorni();
        pelaa();
        LuoPistelaskuri();


        

        
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Mouse.Listen(MouseButton.Left, ButtonState.Down, AmmuAseella, "Lopeta peli", torni);
        
    }
    
    void luokenttä()
    {
        Level.Background.Color = Color.Black;
        Level.CreateBorders(1.0, false);
        Camera.ZoomToLevel();
        Timer ajastin = new Timer();
        ajastin.Interval = 3;
        ajastin.Timeout += luopahis;
        ajastin.Start();
    }
        
        
    void luotorni()
    {
        torni = new Cannon(100, 100);
        torni.X = (0.0); 
        torni.Y = (300.0);
        torni.IsVisible = false;
        torni.Power.DefaultValue = 50000;
        torni.FireRate = 1.0;
        torni.AmmoIgnoresGravity = false;
        torni.ProjectileCollision += AmmusOsui;

        Add(torni);

        PhysicsObject body = new PhysicsObject(100, 100);
        body.IgnoresCollisionResponse = true;
        body.Shape = Shape.Circle;
        body.Color = Color.Gray;
        body.X = (0.0); 
        body.Y = (300.0);
        
        torni.Add(body);

        piippu = new PhysicsObject(75, 40);
        piippu.IgnoresCollisionResponse = true;
        piippu.Shape = Shape.Rectangle;
        piippu.Color = Color.Gold;
        piippu.Position = torni.Position + new Vector(25.0, 0.0);

        torni.Add(piippu);

        LuoSeinat();
        

    }

    void LuoSeinat()
    {
        seinä = new PhysicsObject(475, 100);
        seinä2 = new PhysicsObject(475, 100);
        seinä.X = (-225);
        seinä.Y = (0);
        seinä.Image = seinäkuva;
        seinä.Mass = 6;
        Add(seinä);
        seinä2.Mass = 6;
        seinä2.X = (225);
        seinä2.Y = (0);
        seinä2.Image = seinäkuva;
        Add(seinä2);
    }

    private void luopahis()
    {
        pistelaskuri.AddValue(1);
        
        if (RandomGen.NextInt(1, 10) == 1)
        {
            Juusto();
        }
        
        if (pistelaskuri == 10)
        {
            bossi();
        }
        if (pistelaskuri == 15)
        {
            bossi();
        }
        if (pistelaskuri == 20)
        {
            bossi();
        }
        if (pistelaskuri == 25)
        {
            bossi();
        }
        if (pistelaskuri == 30)
        {
            bossi();
            bossi();
        }
        if (pistelaskuri == 35)
        {
            bossi();
            bossi();
        }
        if (pistelaskuri == 40)
        {
            bossi();
            bossi();
        }


        
        FollowerBrain seuraajanAivot = new FollowerBrain(torni);
        seuraajanAivot.Speed = 80;
        seuraajanAivot.DistanceFar = 100000; 
        seuraajanAivot.DistanceClose = 100;
        seuraajanAivot.StopWhenTargetClose = true;
        seuraajanAivot.TargetClose += PahisOsuu;
            
        pahis = new PhysicsObject(100, 100);
        pahis.Shape = Shape.Triangle;
        pahis.Color = RandomGen.NextColor();
        pahis.Image = pahiskuva;
        pahis.Y = (-400.0);
        pahis.Mass = 1;
        //pahis.Velocity = new Vector(-pahis.X, 300-pahis.Y);
        pahis.IgnoresGravity = false;
        pahis.X = (RandomGen.NextDouble(-400, 400));
        Add(pahis);
        pahis.Brain = seuraajanAivot;
        pahis.Brain.Active = true;

    }

    void AmmuAseella(Cannon cannon)
    {
        PhysicsObject ammus = cannon.Shoot();
        
        if(ammus != null)
        {
            ammus.Size *= 7;
            //ammus.Image = ...
            //ammus.MaximumLifetime = TimeSpan.FromSeconds(20.0);
        }
    }

    void pelaa()
    {
        Mouse.ListenMovement(0.1, Tahtaa, "Tähtää aseella");
        Gravity = new Vector(0.0, -1000.0);
        
    }
    void AmmusOsui(PhysicsObject ammus, PhysicsObject kohde)
    {
        if (kohde.Tag == "tuho")
        {
            ammus.Destroy();
        }
    }
    
    void Tahtaa()
    {
        Vector suunta = (Mouse.PositionOnWorld - piippu.AbsolutePosition).Normalize();
        torni.Angle = suunta.Angle;
    }

    void PahisOsuu()
    {
        ExplosionSystem rajahdys = new ExplosionSystem(rajahdyskuva, pMaxMaara);
        rajahdys.MaxLifetime = 1;
        rajahdys.MaxVelocity = 200;
        rajahdys.MinLifetime = 0.5;
        rajahdys.MinVelocity = 90;
        
        Add(rajahdys);

        double x = 0;
        double y = 300;
        int pMaara = 50;
        // "Käynnistetään" räjähdys
        rajahdys.AddEffect(x, y, pMaara);
        
        Timer.SingleShot(2, havio);
        

    }

    void havio()
    {
        ClearAll();
        valikko();

    }
    void LuoPistelaskuri()
    {
        pistelaskuri = new IntMeter(0);               
      
        Label pistenaytto = new Label(); 
        pistenaytto.X = Screen.Left + 100;
        pistenaytto.Y = Screen.Top - 50;
        pistenaytto.TextColor = Color.Black;
        pistenaytto.Color = Color.White;

        pistenaytto.BindTo(pistelaskuri);
        Add(pistenaytto);
    }

    void Pringles()
    {
        FollowerBrain seuraajanAivot = new FollowerBrain(torni);
        seuraajanAivot.Speed = 40;
        seuraajanAivot.DistanceFar = 100000; 
        seuraajanAivot.DistanceClose = 100;
        seuraajanAivot.StopWhenTargetClose = true;
        seuraajanAivot.TargetClose += PahisOsuu;
            
        nelio = new PhysicsObject(175, 175);
        nelio.Shape = Shape.Circle;
        nelio.Image = neliokuva;
        nelio.Y = (-400.0);
        nelio.Mass = 20;
        //pahis.Velocity = new Vector(-pahis.X, 300-pahis.Y);
        nelio.IgnoresGravity = false;
        nelio.X = (RandomGen.NextDouble(-400, 400));
        Add(nelio);
        nelio.Brain = seuraajanAivot;
        nelio.Brain.Active = true;
    }
    void Pahkina()
    {
        FollowerBrain seuraajanAivot = new FollowerBrain(torni);
        seuraajanAivot.Speed = 130;
        seuraajanAivot.DistanceFar = 100000; 
        seuraajanAivot.DistanceClose = 50;
        seuraajanAivot.StopWhenTargetClose = true;
        seuraajanAivot.TargetClose += PahisOsuu;
            
        pallo = new PhysicsObject(50, 50);
        pallo.Shape = Shape.Rectangle;
        pallo.Image = pallokuva;
        pallo.Y = (-400.0);
        pallo.Mass = 1;
        //pahis.Velocity = new Vector(-pahis.X, 300-pahis.Y);
        pallo.IgnoresGravity = false;
        pallo.X = (RandomGen.NextDouble(-400, 400));
        Add(pallo);
        pallo.Brain = seuraajanAivot;
        pallo.Brain.Active = true;
    }

    void bossi()
    {
        Juusto();

        int luku = RandomGen.NextInt(1, 5);
        if (luku == 1)
        {
            Pringles();
        }
        
        if (luku == 2)
        {
            for (int i = 0; i < 6; i++)
            {
                Pahkina();
            }
            
        }
        if (luku == 3)
        {
            MegaNacho();

        }
        if (luku == 4)
        {
            Taffel();
        }
        
    }

    void Juusto()
    {
        juusto = new PhysicsObject(50, 50);
        juusto.Shape = Shape.Circle;
        juusto.Color = Color.Red;
        juusto.Y = (400.0);
        juusto.Mass = 1;
        juusto.Tag = "tuho";
        //pahis.Velocity = new Vector(-pahis.X, 300-pahis.Y);
        juusto.IgnoresGravity = false;
        juusto.X = (RandomGen.NextDouble(-400, 400));
        juusto.IgnoresCollisionResponse = true;
        Add(juusto);
    }
    void MegaNacho()
    {
        mega = new PhysicsObject(75, 75);
        mega.Shape = Shape.Circle;
        mega.Image = megakuva;
        mega.Y = (400.0);
        mega.Mass = 30;
        mega.Tag = "tuho";
        //pahis.Velocity = new Vector(-pahis.X, 300-pahis.Y);
        mega.IgnoresGravity = false;
        mega.X = (RandomGen.NextDouble(-400, 400));
        mega.IgnoresCollisionResponse = false;
        Add(mega);
    }
    void Taffel()
    {
        FollowerBrain seuraajanAivot = new FollowerBrain(torni);
        seuraajanAivot.Speed = 10;
        seuraajanAivot.DistanceFar = 100000; 
        seuraajanAivot.DistanceClose = 100;
        seuraajanAivot.StopWhenTargetClose = true;
        seuraajanAivot.TargetClose += PahisOsuu;
            
        soikio = new PhysicsObject(100, 200);
        soikio.Shape = Shape.Circle;
        soikio.Image = soikiokuva;
        soikio.Y = (-400.0);
        soikio.Mass = 2;
        //pahis.Velocity = new Vector(-pahis.X, 300-pahis.Y);
        soikio.IgnoresGravity = false;
        soikio.X = (RandomGen.NextDouble(-400, 400));
        Add(soikio);
        soikio.Brain = seuraajanAivot;
        soikio.Brain.Active = true;
        soikio.AngularVelocity = 40;
        AddCollisionHandler(soikio, TaffelTormaa);
    }

    void TaffelTormaa(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        
        tormaaja.AngularVelocity = 30;
    }

    void kaaos()
    {
        alkaa();
        Timer kaaost = new Timer();
        kaaost.Interval = 1;
        kaaost.Timeout += kaaostapahtuu;
        kaaost.Start();
    }

    void kaaostapahtuu()
    {
        
        if (RandomGen.NextInt(1,3)==1)
        {
            Gravity = new Vector(0.0, RandomGen.NextInt(-2000, 0));
        }
        if (RandomGen.NextInt(1,3)==1)
        {
            Gravity = new Vector(0.0, 0.0);
        }
        if (RandomGen.NextInt(1,4)==1)
        {
            bossi();
        }
        if (RandomGen.NextInt(1,3)==1)
        {
            torni.FireRate = RandomGen.NextDouble(1,20);
        }
        if (RandomGen.NextInt(1,10)==1)
        {
            LuoSeinat();
        }
        if (RandomGen.NextInt(1,7)==1)
        {
            seinä.Destroy();
        }
        if (RandomGen.NextInt(1,7)==1)
        {
            seinä2.Destroy();
        }
        if (RandomGen.NextInt(1,10)==1)
        {
            for (int i = 0; i < RandomGen.NextInt(1, 10); i++)
            {
                Juusto();
            }
        }
    }

}