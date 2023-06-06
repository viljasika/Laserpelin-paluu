using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

namespace Laserpelin_paluu;

public class Laserpelin_paluu : PhysicsGame
{
    PhysicsObject torni;
    PhysicsObject piippu;
    public override void Begin()
    {

        luokenttä();
        luotorni();
        pelaa();
        
        

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");


        
        
    }
    
    void luokenttä()
    {
        Level.Background.Color = Color.Black;
        Level.CreateBorders(1.0, false);
        Camera.ZoomToLevel();
    }
        
        
    void luotorni()
    {
        torni = new PhysicsObject(100, 100);
        torni.IgnoresCollisionResponse = true;
        torni.Shape = Shape.Circle;
        torni.Color = Color.Gray;
        torni.X = (0.0);
        torni.Y = (-300.0);
        
        Add(torni);
            
        piippu = new PhysicsObject(125, 40);
        piippu.IgnoresCollisionResponse = true;
        piippu.Shape = Shape.Rectangle;
        piippu.Color = Color.Gold;
        piippu.X = (0.0);
        piippu.Y = (-300.0);

        Add(piippu);
    }

    void pelaa()
    {
        Mouse.ListenMovement(0.1, Tahtaa, "Tähtää aseella");
        
    }
        
    void Tahtaa()
    {
        Vector suunta = (Mouse.PositionOnWorld - piippu.AbsolutePosition).Normalize();
        piippu.Angle = suunta.Angle;
    }
    
}