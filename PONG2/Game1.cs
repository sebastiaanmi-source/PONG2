using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;

enum Speltoestand {Startscherm, Spel, Einde, Reset}
class PONG2 : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Random rnd = new Random();

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

    Rectangle blauwzijkant;
    Rectangle blauwbovenkant;
    Rectangle blauwonderkant;
    Rectangle roodbovenkant;
    Rectangle roodonderkant;
    Rectangle roodzijkant;
    Rectangle balr;
    SpriteFont Arial;
    Speltoestand FaseSpel = Speltoestand.Startscherm;
    
    Texture2D obst1;
    Vector2 obstakelpositie1;
    Vector2 Obstakelrichting1;
    Rectangle obstakel1;

    Texture2D obst2;
    Vector2 obstakelpositie2;
    Vector2 Obstakelrichting2;
    Rectangle obstakel2;
    Vector2 obstakelpositie3;
    Vector2 Obstakelrichting3;
    Rectangle obstakel3;

    int count = 0;

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
        leven = Content.Load<Texture2D>("heart");

        obst1 = Content.Load<Texture2D>("rodeSpeler");
        obst2 = Content.Load<Texture2D>("rodeSpeler");
        obstakelpositie1 = new Vector2(graphics.PreferredBackBufferWidth / 2 - obst1.Width / 2, 0);
        obstakelpositie2 = new Vector2(800-obst2.Width, graphics.PreferredBackBufferHeight-rodeSpeler.Height);
        obstakelpositie3 = new Vector2(400, 0);
        Obstakelrichting1 = new Vector2(0, 1);
        Obstakelrichting2 = new Vector2(0, 2);
        Obstakelrichting3 = new Vector2(0, 2);

        Arial = Content.Load<SpriteFont>("Arial");
    }   
//Random nummer uit lijst kiezen en random hoek berekenen voor willekeurige start elke keer.
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
        blauwbovenkant = new Rectangle((int)blauwePositie.X, (int)blauwePositie.Y, blauweSpeler.Width, 5);
        blauwonderkant = new Rectangle((int)blauwePositie.X, (int)blauwePositie.Y - 5 + blauweSpeler.Height, blauweSpeler.Width, 5);
        roodbovenkant = new Rectangle((int)rodePositie.X, (int)rodePositie.Y, rodeSpeler.Width, 5);
        roodonderkant = new Rectangle((int)rodePositie.X, (int)rodePositie.Y - 5 + rodeSpeler.Height, rodeSpeler.Width, 5);
        balr = new Rectangle((int)balPositie.X, (int)balPositie.Y, Bal.Width, Bal.Height);



        if (balPositie.Y < 0 || balPositie.Y > 600 - Bal.Height)
        {
            balSnelheid.Y *= -1;
        }
        if (blauwbovenkant.Intersects(balr) || blauwonderkant.Intersects(balr) || roodbovenkant.Intersects(balr) || roodonderkant.Intersects(balr))
        {
            balSnelheid.Y *= -1;
        }

    }
    //zorg dat er een minimale angle is zodat de bal niet horizontaal kan gaan midden in het spel, nogal onhandig lmao
    public void SpelerZijkant(Rectangle zijkant, Vector2 zijkantpositie, Texture2D Speler, Rectangle balr)
    {
        float balmidden = balr.Y + balr.Height / 2;

        if (zijkant.Intersects(balr))
        {
            float Spelermidden = zijkantpositie.Y + (Speler.Height / 2);
            float afstandmidden = Math.Abs((balmidden - Spelermidden)) / (Speler.Height / 2);
            count++;
            if (Math.Abs(balSnelheid.X) > 15)
            {
                balSnelheid.X *= -1;
            }
            else
            {
                balSnelheid.X *= -1.2f;
            }

            if (afstandmidden > 0.5)
            {
                balSnelheid.Y *= 1.15f;
            }
            else
            {
                balSnelheid.Y *= 0.9f;
            }
        }
        //if (roodzijkant.Intersects(balr))

        //{
        //    count++;
        //    float Spelermiddenr = rodePositie.Y + (rodeSpeler.Height / 2);
        //    float afstandmiddenr = Math.Abs((balmidden - Spelermiddenr)) / (rodeSpeler.Height / 2);

        //    if (Math.Abs(balSnelheid.X) > 10)
        //    {
        //        balSnelheid.X *= -1;
        //    }
        //    else
        //    {
        //        balSnelheid.X *= -1.1f;
        //    }

        //    if (afstandmiddenr > 0.5)
        //    {
        //        balSnelheid.Y *= 1.1f;
        //    }
        //    else
        //    {
        //        balSnelheid.Y *= 0.9f;
        //    }
        //}
    }


public void obstakelbeweging()
    {
        if (count > 9 && count <= 19)
        {
            obstakel1 = new Rectangle((int)obstakelpositie1.X, (int)obstakelpositie1.Y, obst1.Width, obst1.Height);
            obstakelpositie1 += Obstakelrichting1;

            if (obstakelpositie1.Y < 0 || obstakelpositie1.Y > graphics.PreferredBackBufferHeight - obst1.Height)
                Obstakelrichting1.Y *= -1;

            if (obstakel1.Intersects(balr))
                balSnelheid.X *= -1;
        }
        else
        {
            obstakel1 = Rectangle.Empty;
        }

        if (count > 19)
        {
            obstakel2 = new Rectangle((int)obstakelpositie2.X, (int)obstakelpositie2.Y, obst2.Width, obst2.Height);
            obstakel3 = new Rectangle((int)obstakelpositie3.X, (int)obstakelpositie3.Y, obst2.Width, obst2.Height);

            obstakelpositie2 += Obstakelrichting2;
            obstakelpositie3 += Obstakelrichting3;

            if (obstakelpositie2.Y < 0 || obstakelpositie2.Y > graphics.PreferredBackBufferHeight - obst2.Height)
                Obstakelrichting2.Y *= -1;
            if (obstakelpositie3.Y < 0 || obstakelpositie3.Y > graphics.PreferredBackBufferHeight - obst2.Height)
                Obstakelrichting3.Y *= -1;

            if (obstakel2.Intersects(balr)|| obstakel3.Intersects(balr))
                balSnelheid.X *= -1;
        }
        else
        {
            obstakel2 = Rectangle.Empty;
            obstakel3 = Rectangle.Empty;
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
            count = 0;
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
            count = 0;
        }

    }
    protected override void Update(GameTime gameTime)
    {
        blauwzijkant = new Rectangle((int)blauwePositie.X + blauweSpeler.Width, (int)blauwePositie.Y, 5, blauweSpeler.Height - 1);
        roodzijkant = new Rectangle((int)rodePositie.X - 5, (int)rodePositie.Y + 1, rodeSpeler.Width, rodeSpeler.Height - 1);
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
            SpelerZijkant(blauwzijkant, blauwePositie, blauweSpeler, balr);
            SpelerZijkant(roodzijkant, rodePositie, rodeSpeler, balr);

            if (count > 9)
            {
                obstakelbeweging();
            }

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
                Vector2 screenCenter = new Vector2(1200 / 2, 600 / 2);
                Vector2 position = screenCenter - textSize / 2;

                spriteBatch.DrawString(Arial, message, position, Color.White);

                spriteBatch.End();
            }
            else if (FaseSpel == Speltoestand.Spel)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            spriteBatch.DrawString(Arial, count.ToString(), new Vector2(graphics.PreferredBackBufferWidth/2, 40), Color.Black);
            spriteBatch.Draw(blauweSpeler, blauwePositie, Color.White);
            spriteBatch.Draw(rodeSpeler, rodePositie, Color.White);
            spriteBatch.Draw(Bal, balPositie, Color.White);
            if (count > 9)
            {
                spriteBatch.Draw(obst1, obstakel1, Color.White);
            }
            if (count > 19)
            {
                spriteBatch.Draw(obst2, obstakel2, Color.White);
                spriteBatch.Draw(obst2, obstakel3, Color.White);
            }
            if (leven1B != null)
            {
                spriteBatch.Draw(leven, new Vector2(110, 50), Color.White);
            }
            if (leven2B != null)
            {
                spriteBatch.Draw(leven, new Vector2(75, 50), Color.White);
            }
            if (leven3B != null)
            {
                spriteBatch.Draw(leven, new Vector2(40, 50), Color.White);
            }
            if (leven1R != null)
            {
                spriteBatch.Draw(leven, new Vector2(1090 - leven.Width, 50), Color.White);
            }
            if (leven2R != null)
            {
                spriteBatch.Draw(leven, new Vector2(1125 - leven.Width, 50), Color.White);
            }
            if (leven3R != null)
            {
                spriteBatch.Draw(leven, new Vector2(1160 -leven.Width, 50), Color.White);
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
                string message2 = "Druk op 'space' om opnieuw te spelen";
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
                string message2 = "Druk op 'space' om opnieuw te spelen";
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
