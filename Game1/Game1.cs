using _2.Zad_Generic_Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        public Game1()
        {

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = 900,
                PreferredBackBufferWidth = 500
            };


            Content.RootDirectory = "Content";
        }/// <summary>    
         /// Bottom paddle object     
         /// </summary>     
        public Paddle PaddleBottom { get; private set; }
        /// <summary>     
        /// Top paddle object  
        /// </summary>     
        public Paddle PaddleTop { get; private set; }
        /// <summary>      
        /// Ball object    
        /// </summary>    
        public Ball Balla { get; private set; }
        /// <summary>    
        /// Background image   
        /// </summary>    
        public Background Backgrounda { get; private set; }
        /// <summary>       
        /// Sound when ball hits an obstacle.    
        /// SoundEffect is a type defined in Monogame framework   
        /// </summary>     
        public SoundEffect HitSound { get; private set; }
        /// <summary>     
        /// Background music. Song is a type defined in Monogame framework  
        /// </summary>     
        public Song Music { get; private set; }
        /// <summary>         /// Generic list that holds Sprites that should be drawn on screen 
        /// </summary>     
        private IGenericList<Sprite> SpritesForDrawList = new GenericList<Sprite>();


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Ask graphics device about screen bounds we are using. 
            var screenBounds = GraphicsDevice.Viewport.Bounds;
            // Load paddle texture using Content.Load static method 
            Texture2D paddleTexture = Content.Load<Texture2D>("paddle");  // loading texutres for paddle
            PaddleBottom = new Paddle(paddleTexture);  //applying
            PaddleTop = new Paddle(paddleTexture);      //applying
            PaddleBottom.Position.Y = 848;   //Y value 900 - paddle height = 52
            PaddleBottom.Position.X = 250;        // middle of x range
            PaddleTop.Position = new Vector2(250, 40);
            Texture2D ballTexture = Content.Load<Texture2D>("ball");
            Balla = new Ball(ballTexture);
            Balla.Position = screenBounds.Center.ToVector2();
            Texture2D backgroundTexture = Content.Load<Texture2D>("background");
            Backgrounda = new Background(backgroundTexture, screenBounds.Width, screenBounds.Height);
            HitSound = Content.Load<SoundEffect>("hit");
            Music = Content.Load<Song>("music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Music);
            SpritesForDrawList.Add(Backgrounda);

            SpritesForDrawList.Add(PaddleTop);
            SpritesForDrawList.Add(PaddleBottom);
            SpritesForDrawList.Add(Balla);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var touchState = Keyboard.GetState();
            if (touchState.IsKeyDown(Keys.Left))
            {
                PaddleBottom.Position.X -= (float)(PaddleBottom.Speed *
                    gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (touchState.IsKeyDown(Keys.Right))
            {
                PaddleBottom.Position.X += (float)(PaddleBottom.Speed *
                    gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (touchState.IsKeyDown(Keys.A))
                PaddleTop.Position.X -= (float)(PaddleTop.Speed *
                    gameTime.ElapsedGameTime.TotalMilliseconds);
            if (touchState.IsKeyDown(Keys.D))
                PaddleTop.Position.X += (float)(PaddleTop.Speed *
                   gameTime.ElapsedGameTime.TotalMilliseconds);
            //-------------boundaries------------------------
            var bounds = graphics.GraphicsDevice.Viewport.Bounds;
            PaddleBottom.Position.X = MathHelper.Clamp(PaddleBottom.Position.X,
                bounds.Left, bounds.Right - PaddleBottom.Size.Width);
            PaddleTop.Position.X = MathHelper.Clamp(PaddleTop.Position.X,
                bounds.Left, bounds.Right - PaddleBottom.Size.Width);
            //-----------Ball movement----------------
            Balla.Position += Balla.Direction *
                (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Balla.Speed);

            //-----------Ball walls hit--------
            if (Balla.Position.X < bounds.Left || Balla.Position.X > bounds.Right -
                Balla.Size.Width)
            {
                Balla.Direction.X = -Balla.Direction.X;
                Balla.Speed = Balla.Speed * Ball.BumpSpeedIncreaseFactor;
                HitSound.Play();
            }
            //--------------Ball score ------------
            if (Balla.Position.Y > bounds.Bottom || Balla.Position.Y < bounds.Top)
            {
                Balla.Position = bounds.Center.ToVector2();
                Balla.Speed = Ball.InitialSpeed;
            }
            if (CollisionDetector.Overlaps(Balla, PaddleTop) && Balla.Direction.Y < 0
            || (CollisionDetector.Overlaps(Balla, PaddleBottom) && Balla.Direction.Y > 0))
            {
                Balla.Direction.Y = -Balla.Direction.Y;
                Balla.Speed *= Ball.BumpSpeedIncreaseFactor;
            }
            // Reset ball          
            if (Balla.Position.Y > bounds.Bottom || Balla.Position.Y < bounds.Top)
            {
                Balla.Position = bounds.Center.ToVector2();
                Balla.Speed =Ball.InitialSpeed;
            }

        }

        //------------------Collision -----------
        public  class  CollisionDetector
        {
            public static bool Overlaps(Ball balll, Paddle pad)
            {
                if (balll.Position.X < pad.Position.X + pad.Size.Width &&
                    balll.Position.X + balll.Size.Width > pad.Position.X &&
                    balll.Position.Y < pad.Position.Y + pad.Size.Height &&
                    balll.Size.Height + balll.Position.Y > pad.Position.Y)
                    return true;



                return false;
            }



        }
    



   protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int i = 0; i < SpritesForDrawList.Count; i++)
            {
                SpritesForDrawList.GetElement(i).Draw(spriteBatch); 
            }


            spriteBatch.End();
            base.Draw(gameTime);

        }
        public abstract class Sprite
        {
            /// <summary> 
            /// /// Represents size of the Sprite on the screen (in pixels).
            /// /// Rectangle type is defined in Monogame framework.         
            /// /// </summary>         
            public Rectangle Size;
            /// Represents position of the Sprite on the screen 
            /// Vector2 is two-dimensional vector (X and Y component)
            /// defined in Monogame framework.         
            /// </summary>       
            public Vector2 Position;
            /// <summary>        
            /// Represents the texture of the Sprite on the screen.      
            /// Texture2D is a type defined in Monogame framework.
            /// </summary>       
            public Texture2D Texture { get; set; }
            protected Sprite(Texture2D spriteTexture, int width, int height)
            {
                Texture = spriteTexture;
                Size = new Rectangle(0, 0, width, height);
                Position = new Vector2(0, 0);
            }
            /// <summary>    
            /// Base draw method     
            /// </summary>       
            public virtual void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, Position, Size, Color.White);
            }

        }
        public class Background : Sprite
        {
            public Background(Texture2D spriteTexture, int width, int height) : base(spriteTexture, width, height)
            {
            }
        }
        public class Ball : Sprite
        {
            /// <summary>    
            /// Initial ball speed. Constant     
            /// </summary>     
            public const float InitialSpeed = 0.4f;
            /// <summary>       
            /// Defines a factor of speed increase when bumping on the paddle.      
            /// Constant       
            /// </summary>        
            public const float BumpSpeedIncreaseFactor = 1.01f;
            /// <summary>         
            /// Defines ball size. Constant    
            /// </summary>       
            public const int BallSize = 50;
            /// <summary>      
            /// Defines current ball speed in time.   
            /// </summary>     
            public float Speed { get; set; }
            /// <summary>    
            /// Defines ball direction.     
            /// Valid values (-1,-1), (1,1), (1,-1), (-1,1).     
            /// Using Vector2 to simplify game calculation. Potentially    
            /// dangerous because vector 2 can swallow other values as well.     
            /// Think about creating your own, more suitable type.        
            /// </summary>   
            public Vector2 Direction;
            public Ball(Texture2D spriteTexture)
                : base(spriteTexture, BallSize, BallSize)
            {
                Speed = InitialSpeed;
                //Initial direction
                Direction = new Vector2(1, 1);

            }
        }
        public class Paddle : Sprite
        {
            /// <summary>
            /// Initial paddle speed .Constant
            /// </summary>
            private const float InitialSpeed = 0.9f;
            /// <summary>      
            /// Paddle height. Constant     
            /// </summary>
            private const int PaddleHeight = 20;
            /// <summary>  
            /// Paddle width. Constant  
            /// </summary>         
            private const int PaddleWidth = 200;
            /// <summary>     
            /// Current paddle speed in time     
            /// /// </summary>
            public float Speed { get; set; }
            public Paddle(Texture2D spriteTexture)
                : base(spriteTexture, PaddleWidth, PaddleHeight)
            {
                Speed = InitialSpeed;
            }
            /// <summary>    
            /// Overriding draw method. Masking paddle texture with black color.         /// </summary>        
            /// <param name="spriteBatch"></param>   
            public override void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(Texture, Position, Size, Color.Black);
            }
        }
        


    }
}
