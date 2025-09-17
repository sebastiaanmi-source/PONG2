using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;
using System.Security.Cryptography;

enum Speltoestand {Startscherm, Spel, Einde}
class PONG2 : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D blauweSpeler;
    Texture2D rodeSpeler;
    Texture2D Bal;
    Texture2D leven;
    Texture2D leven1B;
    Texture2D leven2B;
    Texture2D leven3B;
    Texture2D leven1R;
    Texture2D leven2R;
    Texture2D leven3R;
    Vector2 blauwePositie;
    Vector2 rodePositie;
    Vector2 balPositie;
    Vector2 balSnelheid;
    Random rnd = new Random();
    Rectangle blauwzijkant;
    Rectangle blauwbovenkant;
    Rectangle blauwonderkant;
    Rectangle roodbovenkant;
    Rectangle roodonderkant;
    Rectangle roodzijkant;
    Rectangle balr;
    SpriteFont Arial;
    Speltoestand FaseSpel = Speltoestand.Startscherm; 

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
        balSnelheid = BalOrigin();
        leven = Content.Load<Texture2D>("life");
        leven1B = Content.Load<Texture2D>("life");
        leven2B = Content.Load<Texture2D>("life");
        leven3B = Content.Load<Texture2D>("life");

        leven1R = Content.Load<Texture2D>("heart");
        leven2R = Content.Load<Texture2D>("heart");
        leven3R = Content.Load<Texture2D>("heart");

        Arial = Content.Load<SpriteFont>("Arial");
    }   
    public Vector2 BalOrigin()
    {
        int[] ArrayX = { -3, -2, 2, 3 };
        int rndArrayX = rnd.Next(0, 3);
        int rndX = ArrayX[rndArrayX];
        int[] ArrayY = { -3, -2, -1, 1, 2, 3 };
        int rndArrayY = rnd.Next(0, 5);
        int rndY = ArrayY[rndArrayY];
        return new Vector2(rndX, rndY);
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
        balPositie += balSnelheid;
        blauwzijkant = new Rectangle((int)blauwePositie.X + blauweSpeler.Width, (int)blauwePositie.Y, 5, blauweSpeler.Height);
        blauwbovenkant = new Rectangle((int)blauwePositie.X, (int)blauwePositie.Y, blauweSpeler.Width, 5);
        blauwonderkant = new Rectangle((int)blauwePositie.X, (int)blauwePositie.Y -5 + blauweSpeler.Height, blauweSpeler.Width, 5);
        roodzijkant = new Rectangle((int)rodePositie.X, (int)rodePositie.Y, 5, rodeSpeler.Height);
        roodbovenkant = new Rectangle((int)rodePositie.X, (int)rodePositie.Y, rodeSpeler.Width, 5);
        roodonderkant = new Rectangle((int)rodePositie.X, (int)rodePositie.Y -5 + blauweSpeler.Height, rodeSpeler.Width, 5);
        balr = new Rectangle((int)balPositie.X, (int)balPositie.Y, Bal.Width, Bal.Height);
//Balpositie door vector te maken en die de hele tijd bij elkaar op te tellen. Wanneer rand wordt geraakt door de bal wordt de Y component negatief en keert deze dus om

        if (balPositie.Y < 0 || balPositie.Y > 600 - Bal.Height)
        {
            balSnelheid.Y *= -1;
        }
// wanneer bal een speler raakt, x compononent omkeren en verhogen voor snelheid. Als de boven/onderkant geraakt wordt, naar boven of onder weerkaatsen. 
        if (blauwzijkant.Intersects(balr) || roodzijkant.Intersects(balr))
        {
            balSnelheid.X *= (float)-1.1;
        }
        if (blauwbovenkant.Intersects(balr) || blauwonderkant.Intersects(balr) || roodbovenkant.Intersects(balr) || roodonderkant.Intersects(balr))
        {
            balSnelheid.Y *= -1;
        }
// als bal de wand raakt herstarten in het midden met nieuwe beginsnelheid


    }
    public void levensblauw()
    {
        if (balPositie.X < 0)
        {
            if (leven1B != null)
            {
                leven1B = null;
            }
            else if (leven2B != null)
            {
                leven2B = null;
            }
            else if (leven3B != null)
            {
                leven3B = null;
            }

            balPositie = new Vector2(graphics.PreferredBackBufferWidth / 2 - Bal.Width, graphics.PreferredBackBufferHeight / 2 - Bal.Height);
            balSnelheid = BalOrigin();
        }
            
    }

    public void levensrood()
    {
        if (balPositie.X > 1200)
        {
            if (leven1R != null)
            {
                leven1R = null;
            }
            else if (leven2R != null)
            {
                leven2R = null;
            }
            else if (leven3R != null)
            {
                leven3R = null;
            }

            balPositie = new Vector2(graphics.PreferredBackBufferWidth / 2 - Bal.Width, graphics.PreferredBackBufferHeight / 2 - Bal.Height);
            balSnelheid = BalOrigin();
        }

    }
    protected override void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        base.Update(gameTime);
        if (FaseSpel == Speltoestand.Startscherm)
        {
            if (state.IsKeyDown(Keys.Space))
            {
                FaseSpel = Speltoestand.Spel;
            }
        }
        else if (FaseSpel == Speltoestand.Spel)
        {
            SpelerInput();
            levensblauw();
            levensrood();
            BalBeweging();
            
            if (leven1B == null && leven2B == null && leven3B == null || leven1R == null && leven2R == null && leven3R == null)
            {
                FaseSpel = Speltoestand.Einde;
            }
        }
// gaat hier wat fout, spel komt wel in eindtoestand, maar wanneer de space bar ingedrukt wordt gaat het naar de speltoestand, alleen gaat het alleen hierheen wanneer deze ingedrukt blijft, niet wanneer hij maar een keer wordt ingedrukt. Komt doordat de levens niet opnieuw getekent worden en hij dus constant denkt dat het spel voorbij is.
        else if (FaseSpel == Speltoestand.Einde)
        {
            if (state.IsKeyDown(Keys.Space))
            {
                FaseSpel = Speltoestand.Spel;
            }
        }
    }


    protected override void Draw(GameTime gameTime)
    {
        if (FaseSpel == Speltoestand.Startscherm)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.DrawString(Arial, "druk op 'space' om te beginnen!", new Vector2(500, 300), Color.White);
            spriteBatch.End();
        }
        else if (FaseSpel == Speltoestand.Spel)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            spriteBatch.Draw(blauweSpeler, blauwePositie, Color.White);
            spriteBatch.Draw(rodeSpeler, rodePositie, Color.White);
            spriteBatch.Draw(Bal, balPositie, Color.White);
            if (leven1B != null)
            {
                spriteBatch.Draw(leven, new Vector2(100, 50), Color.White);
            }
            if (leven2B != null)
            {
                spriteBatch.Draw(leven, new Vector2(75, 50), Color.White);
            }
            if (leven3B != null)
            {
                spriteBatch.Draw(leven, new Vector2(50, 50), Color.White);
            }
            if (leven1R != null)
            {
                spriteBatch.Draw(leven, new Vector2(1100, 50), Color.White);
            }
            if (leven2R != null)
            {
                spriteBatch.Draw(leven, new Vector2(1125, 50), Color.White);
            }
            if (leven3R != null)
            {
                spriteBatch.Draw(leven, new Vector2(1150, 50), Color.White);
            }
            spriteBatch.End();
        }
        else if (FaseSpel == Speltoestand.Einde)
        {
            // (blauw heeft gewonnen
            if (leven1R == null && leven2R == null && leven3R == null)
            {
                GraphicsDevice.Clear(Color.Blue);
                spriteBatch.Begin();
                spriteBatch.DrawString(Arial, "Blauw is de winnaar!", new Vector2(400, 200), Color.White);
                spriteBatch.DrawString(Arial, "Druk op 'space' om opniew te beginnen", new Vector2(400, 300), Color.White);
                spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.Red);
                spriteBatch.Begin();
                spriteBatch.DrawString(Arial, "Rood is de winnaar!", new Vector2(400, 200), Color.White);
                spriteBatch.DrawString(Arial, "Druk op 'space' om opniew te beginnen", new Vector2(400, 300), Color.White);
                spriteBatch.End();
            }
        }
    }

}
