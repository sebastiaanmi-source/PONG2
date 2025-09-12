using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;
using System.Security.Cryptography;

class PONG2 : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D blauweSpeler;
    Texture2D rodeSpeler;
    Texture2D Bal;
    Vector2 blauwePositie;
    Vector2 rodePositie;
    Vector2 balPositie;
    Vector2 balSnelheid;
    Random rnd = new Random();
 
    

    [STAThread]
    static void Main()
    {
        PONG2 game = new PONG2();
        game.Run();
    }

    public PONG2()
    {
        Content.RootDirectory = "Content";
        graphics = new GraphicsDeviceManager(this);

        graphics.PreferredBackBufferWidth = 1200;
        graphics.PreferredBackBufferHeight = 600;

    }

    protected override void LoadContent()
    //inladen van alle sprites en de locatie van de spelers vaststellen zodat alles in het midden begint   
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        blauweSpeler = Content.Load<Texture2D>("blauweSpeler");
        blauwePositie = new Vector2(0, graphics.PreferredBackBufferHeight / 2 - blauweSpeler.Height / 2);

        rodeSpeler = Content.Load<Texture2D>("rodeSpeler");
        rodePositie = new Vector2(graphics.PreferredBackBufferWidth - rodeSpeler.Width, graphics.PreferredBackBufferHeight / 2 - rodeSpeler.Height / 2);

        Bal = Content.Load<Texture2D>("bal");
        balPositie = new Vector2(graphics.PreferredBackBufferWidth / 2 - Bal.Width, graphics.PreferredBackBufferHeight / 2 - Bal.Height);

        int[] ArrayX = { -2, 2 };
        int rndArrayX = rnd.Next(0, 2);
        int rndX = ArrayX[rndArrayX];
        int[] ArrayY = {-3, -2, -1, 1, 2,3 };
        int rndArrayY = rnd.Next(0, 2);
        int rndY = ArrayY[rndArrayY];
        balSnelheid = new Vector2(rndX, rndY);
    }

    public void SpelerInput()
    {

        //Positie verandert door W en S in te drukken voor Blauwe Speler en checkt of speler zich op de rand bevindt

        
        KeyboardState state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.S) && blauwePositie.Y < graphics.PreferredBackBufferHeight - blauweSpeler.Height)
        {
            blauwePositie.Y += 5;
        }
        if (state.IsKeyDown(Keys.W) && blauwePositie.Y > 0)
        {
            blauwePositie.Y -= 5;
        }

        //Positie verandert door pijlte omhoog/omlaag voor rode speler  en checkt of speler zich op de rand bevindt   
        if (state.IsKeyDown(Keys.Down) && rodePositie.Y < graphics.PreferredBackBufferHeight - rodeSpeler.Height)
        {
            rodePositie.Y += 5;
        }
        if (state.IsKeyDown(Keys.Up) && rodePositie.Y > 0)
        {
            rodePositie.Y -= 5;
        }
    }

    public void BalBeweging()
    {
        //Balpositie door vector te maken en die de hele tijd bij elkaar op te tellen. Wanneer rand wordt geraakt door de bal wordt de Y component negatief en keert deze dus om
        balPositie += balSnelheid;
        int getal = rnd.Next(-1, 2);


        if (balPositie.Y < 0 || balPositie.Y > 600 - Bal.Height)
        {
            balSnelheid.Y *= -1;
        }


            {
            balSnelheid.X *= -1;
        }

    }
    protected override void Update(GameTime gameTime)
    {
    base.Update(gameTime);
        SpelerInput();
        BalBeweging();
    }


    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        spriteBatch.Begin();
        spriteBatch.Draw(blauweSpeler, blauwePositie, Color.White);
        spriteBatch.Draw(rodeSpeler, rodePositie, Color.White);
        spriteBatch.Draw(Bal, balPositie, Color.White);
        spriteBatch.End();
    }

}
