using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;
using System.Security.Cryptography;

enum Speltoestand {Startscherm, Spel, Einde, Reset}
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
    Rectangle bb;
    Rectangle bm;
    Rectangle bo;
    Rectangle blauwbovenkant;
    Rectangle blauwonderkant;
    Rectangle roodbovenkant;
    Rectangle roodonderkant;
    Rectangle rb;
    Rectangle rm;
    Rectangle ro;
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
        int[] arrayX = { -1, 1 };
        int rndX = arrayX[rnd.Next(0, 2)];

        double Graden = rnd.Next(15, 46);
        double Rad = Math.PI * Graden / 180;

        float richting = 6f;

        float startX = (float)(rndX * Math.Cos(Rad) * richting);
        float startY = (float)(Math.Sin(Rad) * richting);

        return new Vector2(startX, startY);
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
        //float top = blauwePositie.Y;
        //float hoogte = blauweSpeler.Height;
        //float relatief = balPositie.Y - top;

        //if (relatief < hoogte / 3)
        //{
        //    balSnelheid.X *= (float)-1.1;
        //    balSnelheid = new Vector2(balSnelheid.X, -4);
        //}
        //else if (relatief >2 * hoogte / 3)
        //{
        //    balSnelheid.X *= (float)-1.1;
        //    balSnelheid = new Vector2(balSnelheid.X, 4);
        //}
        //else
        //{
        //    balSnelheid.X *= (float)-1.1;
        //}
        //bm = blauwePositie.Y;
        //bo = blauwePositie.Y;

        bb = new Rectangle((int)blauwePositie.X + blauweSpeler.Width, (int)blauwePositie.Y, 5, blauweSpeler.Height / 3 - 1);
        bm = new Rectangle((int)(blauwePositie.X) + blauweSpeler.Width, (int)blauwePositie.Y + blauweSpeler.Height / 3, 5, blauweSpeler.Height / 3 - 1);
        bo = new Rectangle((int)(blauwePositie.X) + blauweSpeler.Width, (int)blauwePositie.Y + blauweSpeler.Height / 3 * 2, 5, blauweSpeler.Height / 3);
        blauwbovenkant = new Rectangle((int)blauwePositie.X, (int)blauwePositie.Y, blauweSpeler.Width, 5);
        blauwonderkant = new Rectangle((int)blauwePositie.X, (int)blauwePositie.Y - 5 + blauweSpeler.Height, blauweSpeler.Width, 5);
        rb = new Rectangle((int)rodePositie.X, (int)rodePositie.Y, 5, rodeSpeler.Height / 3 - 1);
        rm = new Rectangle((int)rodePositie.X, (int)rodePositie.Y + rodeSpeler.Height / 3, 5, rodeSpeler.Height / 3 - 1);
        ro = new Rectangle((int)rodePositie.X, (int)rodePositie.Y + rodeSpeler.Height / 3 * 2, 5, rodeSpeler.Height / 3);
        roodbovenkant = new Rectangle((int)rodePositie.X, (int)rodePositie.Y, rodeSpeler.Width, 5);
        roodonderkant = new Rectangle((int)rodePositie.X, (int)rodePositie.Y - 5 + rodeSpeler.Height, rodeSpeler.Width, 5);
        balr = new Rectangle((int)balPositie.X, (int)balPositie.Y, Bal.Width, Bal.Height);
        //Balpositie door vector te maken en die de hele tijd bij elkaar op te tellen.Wanneer rand wordt geraakt door de bal wordt de Y component negatief en keert deze dus om

        if (balPositie.Y < 0 || balPositie.Y > 600 - Bal.Height)
        {
            balSnelheid.Y *= -1;
        }
//bovenste gedeelte van de speler wordt geraakt, dus gaat op schuinere hoek omlaag.
        if (bb.Intersects(balr) || rb.Intersects(balr))
        {
            balSnelheid.X *= (float)-1.1;
            balSnelheid = new Vector2(balSnelheid.X, 4);
        }
//onderste gedeelte van de speler wordt geraakt, dus gaat op schuinere hoek omhoog.
        if (bo.Intersects(balr) || ro.Intersects(balr))
        {
            balSnelheid.X *= (float)-1.1;
            balSnelheid = new Vector2(balSnelheid.X, -4);
        }
// wanneer bal een speler raakt, x compononent omkeren en verhogen voor snelheid. Als de boven/onderkant geraakt wordt, naar boven of onder weerkaatsen. 
        if (bm.Intersects(balr) || rm.Intersects(balr))
        {
            balSnelheid.X *= (float)-1.1;
//boven of onderkant raken schiet de bal weer recht omhoog
        }
        if (blauwbovenkant.Intersects(balr) || blauwonderkant.Intersects(balr) || roodbovenkant.Intersects(balr) || roodonderkant.Intersects(balr))
        {
            balSnelheid.Y *= -1;
        }

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
            
            if (leven3B == null || leven3R == null)
            {
                FaseSpel = Speltoestand.Einde;
            }
        }

        else if (FaseSpel == Speltoestand.Einde)
        {
            if (state.IsKeyDown(Keys.Space))
            {
                FaseSpel = Speltoestand.Reset;
            }
        }
        else if (FaseSpel == Speltoestand.Reset)
        {
            leven1B = leven;
            leven2B = leven;
            leven3B = leven;
            leven1R = leven;
            leven2R = leven;
            leven3R = leven;

            FaseSpel = Speltoestand.Spel;
        }
    }


    protected override void Draw(GameTime gameTime)
    {
        if (FaseSpel == Speltoestand.Startscherm)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                string message = "Press 'space' to start";
                Vector2 textSize = Arial.MeasureString(message);

                // Schermmidden is (1200/2, 600/2) = (600, 300)
                Vector2 screenCenter = new Vector2(1200 / 2, 600 / 2);

                // Trek de helft van de tekstgrootte af voor centreren
                Vector2 position = screenCenter - textSize / 2;

                spriteBatch.DrawString(Arial, message, position, Color.White);

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
                string message1 = "Blauw is de winnaar!";
                string message2 = "Druk op 'enter' om opnieuw te spelen";
                Vector2 textSize1 = Arial.MeasureString(message1);
                Vector2 textSize2 = Arial.MeasureString(message2);
                Vector2 screenCenter = new Vector2(1200 / 2, 600 / 2);
                Vector2 position1 = screenCenter - textSize1 / 2;
                Vector2 position2 = new Vector2(screenCenter.X - textSize2.X / 2, position1.Y + textSize1.Y + 10);


                spriteBatch.DrawString(Arial, message1, position1, Color.White);
                spriteBatch.DrawString(Arial, message2, position2, Color.Green);

                spriteBatch.End();
            }
            //rood heeft gewonnen
            else
            {
                GraphicsDevice.Clear(Color.Red);
                spriteBatch.Begin();
                string message1 = "Rood is de winnaar!";
                string message2 = "Druk op 'enter' om opnieuw te spelen";
                Vector2 textSize1 = Arial.MeasureString(message1);
                Vector2 textSize2 = Arial.MeasureString(message2);
                Vector2 screenCenter = new Vector2(1200 / 2, 600 / 2);
                Vector2 position1 = screenCenter - textSize1 / 2;
                Vector2 position2 = new Vector2(screenCenter.X - textSize2.X / 2, position1.Y + textSize1.Y + 10);


                spriteBatch.DrawString(Arial, message1, position1, Color.White);
                spriteBatch.DrawString(Arial, message2, position2, Color.Yellow);

                spriteBatch.End();
            }
        }
    }

}
