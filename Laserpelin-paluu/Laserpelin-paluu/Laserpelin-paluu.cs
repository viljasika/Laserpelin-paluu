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
    int pMaxMaara = 200;
    Image rajahdyskuva = LoadImage("rajahdysp");
    List<Label> valikonKohdat;
    IntMeter pistelaskuri;
    
    Image pahiskuva = LoadImage("dorito-tyhja");
    Image neliokuva = LoadImage("pringles");
    Image seinäkuva = LoadImage("Seina-tyhja");
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
        MultiSelectWindow alkuvalikko = new MultiSelectWindow("LaserPelin Paluu!", "Aloita peli", "Lopeta");
        Add(alkuvalikko);
        
        alkuvalikko.AddItemHandler(0, alkaa);
        alkuvalikko.AddItemHandler(1, Exit);
        
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

        seinä = new PhysicsObject(450, 100);
        seinä2 = new PhysicsObject(450, 100);
        seinä.X = (-225);
        seinä.Y = (0);
        seinä.Image = seinäkuva;
        seinä.Mass = 5;
        Add(seinä);
        seinä2.Mass = 5;
        seinä2.X = (225);
        seinä2.Y = (0);
        seinä2.Image = seinäkuva;
        Add(seinä2);

    }

    private void luopahis()
    {
        pistelaskuri.AddValue(1);

        if (pistelaskuri == 15)
        {
            Pringles();
        }
        if (pistelaskuri == 30)
        {
            Pringles();
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
        ammus.Destroy();
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
        seuraajanAivot.Speed = 80;
        seuraajanAivot.DistanceFar = 100000; 
        seuraajanAivot.DistanceClose = 100;
        seuraajanAivot.StopWhenTargetClose = true;
        seuraajanAivot.TargetClose += PahisOsuu;
            
        nelio = new PhysicsObject(175, 175);
        nelio.Shape = Shape.Rectangle;
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
    
}