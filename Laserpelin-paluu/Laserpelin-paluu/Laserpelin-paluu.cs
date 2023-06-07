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
    public override void Begin()
    {

        luokenttä();
        luotorni();
        pelaa();
        
        
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Mouse.Listen(MouseButton.Left, ButtonState.Down, AmmuAseella, "Lopeta peli", torni);


        
        
    }
    
    void luokenttä()
    {
        Level.Background.Color = Color.Black;
        Level.CreateBorders(1.0, false);
        Camera.ZoomToLevel();
        Timer ajastin = new Timer();
        ajastin.Interval = 1.5;
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
        torni.FireRate = 2.0;
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
        
        

    }

    private void luopahis()
    {
        FollowerBrain seuraajanAivot = new FollowerBrain(torni);
        seuraajanAivot.Speed = 25;
        seuraajanAivot.DistanceFar = 100000; 
        seuraajanAivot.DistanceClose = 0;
        seuraajanAivot.StopWhenTargetClose = false;
            
        pahis = new PhysicsObject(100, 100);
        pahis.Shape = Shape.Triangle;
        pahis.Color = Color.Blue;
        pahis.Y = (-400.0);
        //pahis.Velocity = new Vector(-pahis.X, 300-pahis.Y);
        pahis.IgnoresGravity = true;
        pahis.X = (RandomGen.NextDouble(-200, 200));
        Add(pahis);
        pahis.Brain = seuraajanAivot;
        pahis.Brain.Active = true;
    }

    void AmmuAseella(Cannon cannon)
    {
        PhysicsObject ammus = cannon.Shoot();
        
        if(ammus != null)
        {
            ammus.Size *= 10;
            //ammus.Image = ...
            ammus.MaximumLifetime = TimeSpan.FromSeconds(20.0);
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