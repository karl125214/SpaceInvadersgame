using Invaders_Demo;
using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{

    internal class Invaders

    {
        public event EventHandler ResetGameButtonPressed;
        public event EventHandler GodModeButtonPressed;
        public event EventHandler FastFireButtonPressed;
        public event EventHandler KillEnemiesButtonPressed;
        

        enum GameState
        {
            Play,
            ScoreScreen,
            MainMenuUI,
            Quit,
            Settings,
            Back,
            PauseScreen,
            DeveloperScreen

        }

        public enum Cheats
        {
            Godmode,
            FastFire,
            KillEnemies
        }

        
        MainMenu myMainMenu;

        GameSettings InvadersSettings;

        List<Cheats> activeCheats;
        List<Music> musicActive;


        bool isMusicActive;

        int text_width = 400;
        int text_height = 400;
        int window_width = 600;
        int window_height = 600;
        Player player;
        List<Enemy> enemies;


        double enemyShootInterval;
        double lastEnemyShootTime;
        float enemyBulletSpeed;
        float enemyBulletSize;
        float enemySpeed;
        float enemySpeedDown;
        float enemyMaxYLine;
        int bulletSize;
        //Players score/points
        int scoreCounter = 0;

        Texture playerImage;

        List<Texture> enemyImages;
        Texture bulletImage;

        Sound EnemyDeath;
        Sound BulletShoot;
        Music music;


        float timePlayed = 0;

        //FOR VOLUME SLIDER
        float slidervolume = 1.0f;
        float musicvolume = 1.0f;
        float sfxvolume = 1.0f;
        
        int musicsliderwidth = 100;
        int sfxsliderwidth = 100;
        int sliderheight = 20;
        
        int sliderx = 90;
        int slidery = 180;
        int sliderindent = 1;
        int sliderbetween = 1;

       Stack<GameState> currentStack = new Stack<GameState> ();


        List<Bullet> bullets;
        public void Run()
        {

            Init();
            GameLoop();

            // VOID INIT 
            void Init()
            {
                Raylib.InitWindow(window_width, window_height, "Space Invaders Demo");
                Raylib.InitAudioDevice();


                Raylib.SetExitKey(KeyboardKey.KEY_BACKSPACE);


                InvadersSettings = new GameSettings();
                

                music = Raylib.LoadMusicStream("AssetsGame/Sounds/GameAmbient.mp3");
                
               

                myMainMenu = new MainMenu();
                myMainMenu.StartButtonPressedEvent += startPressed;
                myMainMenu.OptionButtonPressedEvent += settingsPressed;
                myMainMenu.ExitButtonPressedEvent += QuitPressed;
                InvadersSettings.BackButtonPressedEvent += BackButtonPressed;

                
                InvadersSettings.MusicStartedEvent += MusicStartedPressed;
                InvadersSettings.MusicStoppedEvent += MusicStoppedPressed;

                
                ResetGameButtonPressed += ResetGamePressed;
                GodModeButtonPressed += GodModePressed;
                FastFireButtonPressed += FastFirePressed;
                KillEnemiesButtonPressed += KillEnemiesPressed;


                activeCheats = new List<Cheats>();
                musicActive = new List<Music>();


                
                currentStack.Push(GameState.MainMenuUI);

                Raylib.SetTargetFPS(30);

                //KARLSTYLE GUILOADSTYLE

                RayGui.GuiLoadStyle("AssetsGame/buttonfocuskarl.rgs");
                RayGui.GuiLoadStyle("AssetsGame/buttonpressedkarl.rgs");

                playerImage = Raylib.LoadTexture("AssetsGame/PNG/playerShip1_green.png");

                

                EnemyDeath = Raylib.LoadSound("AssetsGame/Bonus/EnemyDeathwav.wav");
                BulletShoot = Raylib.LoadSound("AssetsGame/Bonus/LaserSound.wav");
                

                //Enemy Images
                enemyImages = new List<Texture>(4);
                enemyImages.Add(Raylib.LoadTexture("AssetsGame/PNG/Enemies/enemyBlack1.png"));
                enemyImages.Add(Raylib.LoadTexture("AssetsGame/PNG/Enemies/enemyBlue1.png"));
                enemyImages.Add(Raylib.LoadTexture("AssetsGame/PNG/Enemies/enemyGreen1.png"));
                enemyImages.Add(Raylib.LoadTexture("AssetsGame/PNG/Enemies/enemyRed1.png"));

                bulletImage = Raylib.LoadTexture("AssetsGame/PNG/Lasers/laserBlue09.png");

               

                ResetGame();
                }

            
            }

        void startPressed(object sender, EventArgs args)
        {
            currentStack.Push(GameState.Play);
            
        }

        void QuitPressed(object sender, EventArgs args)
        {
            currentStack.Push(GameState.Quit); 
        }

        void settingsPressed(object sender, EventArgs args)
        {
            currentStack.Push(GameState.Settings);
        }
        void BackButtonPressed(object sender, EventArgs args)
        {
            currentStack.Pop();
        }
        void ResetGamePressed(object sender, EventArgs args)
        {
            ResetGame();
        }
        void GodModePressed(object sender, EventArgs args)
        {
            if (activeCheats.Contains(Cheats.Godmode))
            {
                activeCheats.Remove(Cheats.Godmode);
                
            }
            else
            {
                activeCheats.Add(Cheats.Godmode);
            }
            
        }
        void FastFirePressed(object sender, EventArgs args)
        {
            if (activeCheats.Contains(Cheats.FastFire))
            {
                activeCheats.Remove(Cheats.FastFire);
            }
            else
            {
                activeCheats.Add(Cheats.FastFire);
            }
        }
        void KillEnemiesPressed(object sender, EventArgs args)
        {
            if (!activeCheats.Contains(Cheats.KillEnemies))
            {
                activeCheats.Add(Cheats.KillEnemies);
                
            }
            
        }
        void MusicStartedPressed(object sender, EventArgs args)
        {
            Console.WriteLine("madeit");
            
                isMusicActive = true;
                Raylib.PlayMusicStream(music);
        }
        void MusicStoppedPressed(object sender, EventArgs args)
        {
            Console.WriteLine("wedidintmakeit");
            isMusicActive = false;
            Raylib.StopMusicStream(music);
        }
        
            void GameLoop()
            {
                while (Raylib.WindowShouldClose() == false)
                {

                if (isMusicActive)
                {
                    Console.WriteLine("Music");
                    Raylib.UpdateMusicStream(music);
                }

                switch (currentStack.Peek())
                    {

                        case GameState.Play:
                            Update();
                       
                        timePlayed += Raylib.GetFrameTime();
                            if (activeCheats.Contains(Cheats.KillEnemies))
                        {
                            foreach (Enemy enemy in enemies)
                            {
                                enemy.active = false;
                                scoreCounter = 280;
                            }
                            activeCheats.Remove(Cheats.KillEnemies);

                        }
                            Raylib.BeginDrawing();
                            Raylib.ClearBackground(Raylib.BLUE);
                            Draw();
                            PauseMenu();
                            DeveloperMenu();
                            Raylib.EndDrawing();
                            break;


                        case GameState.ScoreScreen:
                            ScoreUpdate();
                            Raylib.BeginDrawing();
                            Raylib.ClearBackground(Raylib.DARKGRAY);
                        
                        ScoreDraw();
                            Raylib.EndDrawing();

                            break;

                        case GameState.MainMenuUI:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.BLACK);
                        myMainMenu.ShowMenu();
                        Raylib.EndDrawing();
                        timePlayed = 0;
                        break;


                        case GameState.Quit:
                        CloseWindow();
                        break;


                        case GameState.Settings:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.BLACK);
                        InvadersSettings.ShowSettings();
                        Raylib.EndDrawing();
                        break;

                    case GameState.PauseScreen:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.BLACK);
                        PauseMenu();
                        PauseDraw();
                        BackToSettings();
                        BackToMainMenu();
                        Raylib.EndDrawing();
                        break;

                    case GameState.DeveloperScreen:
                        Raylib.BeginDrawing();
                        Raylib.ClearBackground(Raylib.WHITE);
                        DeveloperMenu();
                        DeveloperDraw();

                        BackToSettings();
                        Raylib.EndDrawing();

                        break;

                    
                    }
                        

                

                    
                }
            }
        void ResetGame()
        {
            Vector2 PlayerStart = new Vector2(window_width / 2, window_height - 80);
            float playerSpeed = 200;
            int playerSize = 50;
            scoreCounter = 0;

            player = new Player(PlayerStart, playerSpeed, playerSize, playerImage, activeCheats);

            bullets = new List<Bullet>();

            enemies = new List<Enemy>();

            //todo show enemies (ENEMY STUFF)
            int startX = 0;
            int startY = playerSize;
            int currentX = 0;
            int currentY = 0;
            int rows = 2;
            int columns = 4;
            int enemyBetween = playerSize;
            int enemySize = 45;

            int maxScore = 40;
            int minScore = 10;
            int currentScore = maxScore;



            enemyShootInterval = 1.0f;
            lastEnemyShootTime = 5.0f;
            enemyBulletSpeed = 125;
            enemyBulletSize = 10;
            bulletSize = 10;
            enemySpeed = 100;
            enemySpeedDown = 10;
            enemyMaxYLine = window_height - playerSize * 4;

            for (int row = 0; row < rows; row++)
            {
                currentX = startX;
                //Score decrase when going down
                currentScore = maxScore - row * 10;
                if (currentScore < minScore)
                {
                    currentScore = minScore;
                }

                currentY += playerSize;
                for (int col = 0; col < columns; col++)
                {
                    currentX += playerSize;
                    Vector2 enemyStart = new Vector2(currentX, currentY);

                    int enemyScore = currentScore;

                    Enemy enemy = new Enemy(enemyStart, new Vector2(1, 0), enemySpeed, enemySize, enemyImages[row], enemyScore);
                    // BELOW IS WHAT SPAWNS THE ENEMIES
                   
                    enemies.Add(enemy);
                    
                   
                    
                   
                    

                    currentX += playerSize;
                }
                currentY += playerSize;
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="collision"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        bool KeepInsideArea(Transformcomponent transform, Collisioncomponent collision, int left, int top, int right, int bottom)
        {
            float newX = Math.Clamp(transform.position.X, left, right - collision.size.X);

            float newY = Math.Clamp(transform.position.Y, left, right - collision.size.Y);

            bool xChange = newX != transform.position.X;
            bool yChange = newY != transform.position.Y;

            transform.position.X = newX;
            transform.position.Y = newY;

            return xChange || yChange;
        }

        bool isInsideArea(Transformcomponent transform, Collisioncomponent collision, int left, int top, int right, int bottom)
        {
            float x = transform.position.X;
            float r = x + collision.size.X;

            float y = transform.position.Y;
            float b = y + collision.size.Y;

            if (x < left || y < top || r > right || b > bottom)
            {
                return false;
            } 
            else
            {
                return true;
            }
        }
        void Update()
        {

            UpdatePlayer();
            UpdateEnemies();
            UpdateBullets();
            UpdateCollisions(); //btween enemies and bullets
        }


            
            
           // KeepInsideArea(player.transform, player.collision, 0, 0, window_width, window_height);


            // CREATE BULLET 
            void CreateBullet(Vector2 pos, Vector2 dir, float speed, int size, Texture bulletImage)
            {
                bool Found = false;
                foreach(Bullet bullet in bullets)
                {
                    if (bullet.isActive == false)
                    {
                        //RESET THIS
                        bullet.Reset(pos,dir, speed, size);
                        
                        Found = true;
                        break;
                    }
                }
                if (Found == false)
                {
                    bullets.Add(new Bullet(pos, dir, speed, size, bulletImage));
                    
                }
            Raylib.PlaySound(BulletShoot);
            }

            Rectangle getRectangle(Transformcomponent t, Collisioncomponent c)
            {
                Rectangle r = new Rectangle(t.position.X,
                    t.position.Y, c.size.X, c.size.Y);
                return r;
            }

            
        
        void UpdatePlayer()
        {

            KeepInsideArea(player.transform, player.collision, 0, 0, window_width, window_height);
            bool playerShoots = player.Update();
            if (playerShoots)

            {
                float x = player.transform.position.X + player.collision.size.X / 2 - enemyBulletSize/ 2;

                float y = player.transform.position.Y - enemyBulletSize;
                Vector2 bPos = new Vector2 (x, y);
                //BULLET CREATION

                CreateBullet(bPos , new Vector2(0, -1), enemyBulletSpeed, bulletSize, bulletImage);



                Console.WriteLine($"Bullet count: {bullets.Count}");


                //ENEMY UPDATE


            }
        }
        void UpdateEnemies()
        {
            
            bool changeFormationDirection = false;
            bool canGoDown = true; //enemies can go lower by default
            foreach (Enemy enemy in enemies)
            {
                enemy.Update();
                

                Rectangle enemyRec = getRectangle(enemy.transform, enemy.collision);

                //ENEMY KEEPINSIDE AREA
                bool enemyOut = isInsideArea(enemy.transform, enemy.collision, 0, 0, window_width, window_height) == false;
                if (enemyOut) 
                {
                    changeFormationDirection= true;

                }
                if (enemy.transform.position.Y > enemyMaxYLine)
                {
                    canGoDown = false;
                }

                

                double timeNow = Raylib.GetTime();
                if (timeNow - lastEnemyShootTime >= enemyShootInterval)
                {
                    Enemy shooter = FindBestEnemyShooter();
                    if (shooter != null)
                        //create enemy 
                    {
                        CreateBullet(shooter.transform.position + new Vector2(0, shooter.collision.size.Y),
                            new Vector2(0, 1), enemyBulletSpeed, (int)enemyBulletSize, bulletImage);
                        lastEnemyShootTime = timeNow;
                        Console.WriteLine("Enemy shot");
                    }
                }     
                
            }

            if (changeFormationDirection)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.transform.direction.X *= -1.0f;
                    if (canGoDown)
                    {
                        enemy.transform.position.Y += enemySpeedDown;
                    }
                }
            }




            Enemy FindBestEnemyShooter()
            {

                Enemy best = null;
                //start from worst
                float bestY = 0.0f;
                float bestXDifference = window_width;
                for(int i = enemies.Count-1; i >= 0; i--)
                {
                    Enemy test = enemies[i];
                    if (test.active)
                    {
                        if(test.transform.position.Y >= bestY)
                        {
                            // found letter Y
                            bestY = test.transform.position.Y;

                            float xDifference = Math.Abs(player.transform.position.X - test.transform.position.X);
                            if (xDifference < bestXDifference && xDifference < 10)
                            {
                                bestXDifference = xDifference;
                                best = test;
                            }
                        }
                    }

                    
                }

                return best;
            }
           
            
        }
        void UpdateBullets()
        {
            foreach (Bullet bullet in bullets)
            {

                if (bullet.isActive == false)
                {
                    continue;
                }

                bool isOutside = KeepInsideArea(bullet.transform, bullet.collision, 0, 0, window_width, window_height);

                if (isOutside)
                {
                    bullet.isActive = false;
                    continue;
                }

                bullet.Update();
                Rectangle bulletRec = getRectangle(bullet.transform, bullet.collision);
               // if (enemy.active)
               // {

                //}



            }
        }
        void UpdateCollisions()
        {
            Rectangle playerRec = getRectangle(player.transform, player.collision);
            foreach (Enemy enemy in enemies)
            {
                if (enemy.active)
                {
                    Rectangle enemyRec = getRectangle(enemy.transform, enemy.collision);

                    foreach (Bullet bullet in bullets)
                    {
                        if (bullet.isActive)
                        {
                            Rectangle bulletRec = getRectangle(bullet.transform, bullet.collision);
                            

                            if (Raylib.CheckCollisionRecs(bulletRec, enemyRec))
                            {
                                //hit enemy
                                Console.WriteLine($"Hit enemy for " + enemy.scoreValue + " points");
                                scoreCounter += enemy.scoreValue;
                                bullet.isActive = false;
                                //ENEMIES DIE AND GET DELETED HERE
                                enemy.active = false;
                                Raylib.PlaySound(EnemyDeath);


                                int enemiesLeft = CountAliveEnemies();
                                if (enemiesLeft == 0)
                                {
                                    currentStack.Push(GameState.ScoreScreen); 
                                }
                                //don't test the rest of the bullets
                                break;
                            }
                            if (Raylib.CheckCollisionRecs(bulletRec, playerRec))
                            {
                                if (!activeCheats.Contains(Cheats.Godmode))
                                {
                                    currentStack.Push(GameState.ScoreScreen);
                                    player.active = false;
                                }
                                


                            }
                        }
                        
                           
                        
                        
                    }
                }
                


            }
            
        }
        void Draw()
        {
            player.Draw();

            foreach(Bullet bullet in bullets)
            {

                if (bullet.isActive)
                {
                    bullet.Draw();
                    
                }

            }

            //ENEMY DRAW
            foreach( Enemy enemy in enemies)
            {
                if (enemy.active)
                {
                    enemy.Draw();
                }
                
            }
            //draw score
            Raylib.DrawText(scoreCounter.ToString(), 10, 10, 16, Raylib.BLACK);
            
        }

        int CountAliveEnemies()
        {
            int alive = 0;
            foreach(Enemy enemy in enemies)
            {
                if (enemy.active)
                {
                    alive++;
                }
            }
            return alive;
        }
        

        void ScoreUpdate()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
            {
                ResetGame();
                scoreCounter = 0;
                currentStack.Pop();
            }
        }

        void BackToSettings()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_B))
            {
                
                currentStack.Push(GameState.Settings);
            }
        }

        void BackToMainMenu()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_M))
            {
                ResetGame();
                scoreCounter = 0;
                currentStack.Push(GameState.MainMenuUI);
            }
        }

        void ScoreDraw()
        {
            Raylib.DrawText($"Final score {scoreCounter}", window_width / 7, window_height / 7 -60, 20, Raylib.WHITE);

            Raylib.DrawText("Game over, press ENTER to play again", window_width / 7, window_height / 7, 20, Raylib.WHITE);    
        }

        void CloseWindow()
        {
          Raylib.CloseWindow();
        }

        void PauseMenu()
        {
            
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
            {


                if (currentStack.Peek() == GameState.PauseScreen)
                {
                    currentStack.Pop();
                }
                else if (currentStack.Peek() == GameState.Play)
                {
                    currentStack.Push(GameState.PauseScreen);
                }
                
            }
            
        }

        void DeveloperMenu()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_H))
            {
                if (currentStack.Peek() == GameState.DeveloperScreen)
                {
                    currentStack.Pop();
                }
                else if (currentStack.Peek() == GameState.Play)
                {
#if DEBUG 
                    currentStack.Push(GameState.DeveloperScreen);
#endif
                }
            }     

        }


        void PauseDraw()
        {
            
            Raylib.DrawText("Pause Menu", text_width / 8f, text_height / 13 + 480, 30, Raylib.WHITE);

            Raylib.DrawText("press ESC to go back to game", text_width / 8f, text_height / 13 + 80, 30, Raylib.WHITE);
            Raylib.DrawText("press M to go back to Main menu", text_width / 8f, text_height / 13 + 40, 30, Raylib.WHITE);
            Raylib.DrawText("press B to go back to settings", text_width / 8f, text_height / 13 + 240, 30, Raylib.WHITE);

            Raylib.DrawText("Time played: " + timePlayed, text_width /8f, text_height / 13 + 0, 30, Raylib.WHITE);

            Raylib.DrawText("Music Volume", text_width / 3f, text_height / 12.5f + 125, 15, Raylib.WHITE);
            Raylib.DrawText("Sfx Volume",text_width / 2.7f, text_height / 13.5f + 190, 15, Raylib.WHITE);



            musicvolume = RayGui.GuiSlider(new Rectangle(sliderx + sliderindent, slidery, musicsliderwidth * 2, sliderheight), "LowVol", "MaxVol", musicvolume, 0.0f, 1.0f);
            sfxvolume = RayGui.GuiSlider(new Rectangle(sliderx + sliderindent, slidery + sliderheight + 40, sfxsliderwidth * 2, sliderheight), "LowVol", "MaxVol", sfxvolume,  0.0f, 1.0f);
            Raylib.SetMusicVolume(music, musicvolume);
            Raylib.SetSoundVolume(BulletShoot, sfxvolume);
            Raylib.SetSoundVolume(EnemyDeath, sfxvolume);


        }

        void DeveloperDraw()
        {
            Raylib.DrawText("Developer Menu", text_width / 8f, text_height / 13 + 480, 30, Raylib.BLACK);

            Raylib.DrawText("press H to go back to game", text_width / 8f, text_height / 13 + 80, 30, Raylib.BLACK);
            

            

            if (RayGui.GuiButton(new Rectangle(50, 200, 70, 30), "Reset Game"))
            {
                ResetGameButtonPressed.Invoke(this, EventArgs.Empty);     
            }

            if (RayGui.GuiButton(new Rectangle(150, 200, 70, 30), "God Mode"))
            {
                GodModeButtonPressed.Invoke(this,EventArgs.Empty);
            }

            if (RayGui.GuiButton(new Rectangle(250, 200, 70, 30), "Fast Fire"))
            {
                FastFireButtonPressed.Invoke(this, EventArgs.Empty);
            }

            if (RayGui.GuiButton(new Rectangle(350, 200, 70, 30), "Kill Enemies"))
            {
                KillEnemiesButtonPressed.Invoke(this, EventArgs.Empty);
            }
        }
    }
    

        
    }

