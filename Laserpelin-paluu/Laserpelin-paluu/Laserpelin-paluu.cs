using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
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
    Image pahiskuva = LoadImage("dorito-tyhja");
    Image seinäkuva = LoadImage("seina-tyhja");
    public override void Begin()
    {

        luokenttä();
        luotorni();
        pelaa();
        
        
        
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Mouse.Listen(MouseButton.Left, ButtonState.Down, AmmuAseella, "Lopeta peli", torni);
        //torni.ProjectileCollision = AmmusOsui;


        
        
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

        seinä = new PhysicsObject(300, 50);
        seinä2 = new PhysicsObject(300, 50);
        seinä.X = (-200);
        seinä.Y = (0);
        seinä.Image = seinäkuva;
        Add(seinä);
        seinä2.X = (200);
        seinä2.Y = (0);
        seinä2.Image = seinäkuva;
        Add(seinä2);

    }

    private void luopahis()
    {
        
        
        FollowerBrain seuraajanAivot = new FollowerBrain(torni);
        seuraajanAivot.Speed = 40;
        seuraajanAivot.DistanceFar = 100000; 
        seuraajanAivot.DistanceClose = 0;
        seuraajanAivot.StopWhenTargetClose = false;
            
        pahis = new PhysicsObject(100, 100);
        pahis.Shape = Shape.Triangle;
        pahis.Color = RandomGen.NextColor();
        pahis.Image = pahiskuva;
        pahis.Y = (-400.0);
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
    
}