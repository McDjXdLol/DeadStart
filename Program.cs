using Raylib_cs;
using System.Numerics;

class Program
{
    // Window Settings
    public static int WIDTH = 1024;
    public static int HEIGHT = 576;

    private static readonly int WINDOW_WIDTH = Math.Min(WIDTH, 1920);
    private static readonly int WINDOW_HEIGHT = Math.Min(HEIGHT, 1080);
    private static readonly bool FULLSCREEN = false;
    private static readonly Color window_background_color = Color.Gray;
    private static Texture2D background_image;


    // Fonts settings
    private static readonly int debug_font_size = 15;
    private static readonly Color debug_text_color = Color.Black;

    // Debug Variables
    private static readonly Color debug_background = Color.White;
    private static int mouse_x = Raylib.GetMouseX();
    private static int mouse_y = Raylib.GetMouseY();
    private static string mouse_xy = "X: " + mouse_x + " Y: " + mouse_y;
    private static string player_xy = "X: " + player_x + " Y: " + player_y;
    private static string fps = "FPS:" + Raylib.GetFPS();


    // Player Variables

    // Player coordinates
    private static int player_x = 0;
    private static int player_y = WINDOW_HEIGHT / 2;

    // Player settings
    private static readonly Color player_outline_color = Color.Blue;
    private static readonly int player_size = 20;
    private static Rectangle player_rect;
    private static readonly int player_speed = 4;
    private static readonly int player_jumpforce = player_size + 20;
    private static bool player_alive = true;
    private static int player_deaths = 0;
    private static int biggest_score = 0;
    private static Texture2D player_texture;

    // Camera
    private static Camera2D cam;

    // Games Variables
    private static Rectangle restart_butt;
    private static Rectangle[]? floors;
    private static List<Rectangle> obstacles = [];

    // Other player variables
    private static bool touching_ground = false;
    private static readonly Random rnd = new();

    // Function that moves player to the right
    private static void GravityRight()
    {
        player_x += player_speed;
    }

    // Main Gravity Function
    private static void Gravity()
    {
        bool onAnyFloor = false;

        Rectangle future_player_rect = new Rectangle(player_x, player_y + 3, player_size, player_size);

        foreach (Rectangle floor in floors!)
        {
            if (Raylib.CheckCollisionRecs(floor, future_player_rect))
            {
                onAnyFloor = true;
                break;
            }
        }

        if (!onAnyFloor)
        {
            player_y += player_speed;
            touching_ground = false;
        }
        else
        {
            touching_ground = true;
        }

    }

    // Function that listens to keyboard keys
    private static void ListenKeyboard()
    {
        if (player_alive)
        {
            // Movement Keys:
            // Left
            if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.A))
            {
                player_x -= player_speed;
            }
            // Right
            if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.D))
            {
                player_x += player_speed;
            }
            // Up/Jump
            if (Raylib.IsKeyDown(KeyboardKey.Up) || Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Space))
            {
                if (touching_ground)
                {
                    for (int x = 0; x < player_jumpforce; x++)
                    {
                        player_y--;
                    }
                }
            }
            // Down
            if (Raylib.IsKeyDown(KeyboardKey.Down) || Raylib.IsKeyDown(KeyboardKey.S))
            {
                foreach (Rectangle floor in floors!)
                {
                    if (!Raylib.CheckCollisionRecs(floor, player_rect))
                    {
                        player_y += player_speed;
                    }
                }
            }
        }
        // Others:
        if (Raylib.IsKeyPressed(KeyboardKey.F5))
        {
            obstacles.Clear();
            RandomizeObstacles();
        }
    }

    // Function that create Floors
    private static Rectangle Floor(float x, float y, int width, int height)
    {
        return new Rectangle(x, y, width, height);
    }

    // Function that create Obstacles
    private static Rectangle Obstacle(float x, float y, float width, float height)
    {
        return new Rectangle(x, y, width, height);
    }

    // Function that checks if player is in collision with obstacle
    private static void CheckPlayerDeathByCollision()
    {
        foreach (Rectangle obstacle in obstacles!)
        {
            if (Raylib.CheckCollisionRecs(obstacle, player_rect))
            {
                player_alive = false;
                player_deaths++;
            }
        }
    }

    // Function that draw player outline
    private static void DrawOutline()
    {
        // Draw Player outline
        // Left
        Raylib.DrawLine(player_x, player_y, player_x, player_y + player_size, player_outline_color);
        // Right
        Raylib.DrawLine(player_x + player_size, player_y, player_x + player_size, player_y + player_size, player_outline_color);
        // Up
        Raylib.DrawLine(player_x, player_y, player_x + player_size, player_y, player_outline_color);
        // Down
        Raylib.DrawLine(player_x, player_y + player_size, player_x + player_size, player_y + player_size, player_outline_color);

    }

    // Function to draw/show debug menu
    private static void DrawDebug(List<Vector2> clicked_points)
    {
        // Debug Info
        // Debug background
        Raylib.DrawRectangle(0, 0, 160, 80, debug_background);

        // Debug Data refresh
        player_xy = "X: " + player_x + " Y: " + player_y;
        mouse_xy = "X: " + mouse_x + " Y: " + mouse_y;
        mouse_x = Raylib.GetMouseX();
        mouse_y = Raylib.GetMouseY();
        fps = "FPS:" + Raylib.GetFPS();

        // Draw Info
        Raylib.DrawText(player_xy, 10, 5, debug_font_size, debug_text_color);
        Raylib.DrawText(mouse_xy, 10, 5 + debug_font_size, debug_font_size, debug_text_color);
        Raylib.DrawText(fps, 10, 5 + (debug_font_size * 2), debug_font_size, debug_text_color);

        if (Raylib.IsMouseButtonPressed(MouseButton.Right))
        {
            for (int x = 0; x < clicked_points.Count; x += 2)
            {
                if (clicked_points.Count % 2 == 0)
                {
                    Console.WriteLine("     Obstacle(" + Math.Round(clicked_points[x].X) + ", " + Math.Round(clicked_points[x].Y) + ", " + Math.Round(clicked_points[x + 1].X - clicked_points[x].X) + ", " + Math.Round(clicked_points[x + 1].Y - clicked_points[x].Y) + "),");
                }
            }
        }
    }

    // Function that draw the player (with outline)
    private static void DrawPlayer()
    {
        // Draw Player
        player_rect = new Rectangle(player_x, player_y, player_size, player_size);
        Raylib.DrawTextureEx(player_texture, new Vector2(player_x, player_y), 0f, 1f, Color.White);
        DrawOutline();
    }

    // Function that draw obstacles
    private static void DrawObstacles()
    {
        // Drawing Obstacles
        foreach (Rectangle obstacle in obstacles)
        {
            Raylib.DrawRectangleRec(obstacle, Color.Red);
        }
    }

    // Function that draw floors
    private static void DrawFloors()
    {
        // Drawing floors
        foreach (Rectangle floor in floors!)
        {
            Raylib.DrawRectangleRec(floor, Color.Brown);
        }
    }

    // Function that is used to draw obstacles on click
    private static void ClickToDraw(List<Vector2> clicked_points)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 mouseInGame = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), cam);
            clicked_points.Add(mouseInGame);
            for (int x = 0; x < clicked_points.Count; x += 2)
            {
                if (clicked_points.Count % 2 == 0)
                {
                    obstacles.Add(Obstacle(clicked_points[x].X, clicked_points[x].Y, (clicked_points[x + 1].X - clicked_points[x].X), (clicked_points[x + 1].Y - clicked_points[x].Y)));

                }
            }
        }
    }

    private static void RandomizeObstacles()
    {
        for (int i = 0; i < rnd.Next(50, 500); i++)
        {
            obstacles.Add(Obstacle(rnd.Next(50, 20000), 595, rnd.Next(3, 10), 10));
        }
    }
    [STAThread]
    public static void Main()
    {
        // Initialize window
        Raylib.InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "DeadStart");
        Image icon = Raylib.LoadImage("textures/icon.png");
        Raylib.SetWindowIcon(icon);
        background_image = Raylib.LoadTexture("textures/background.png");
        player_texture = Raylib.LoadTexture("textures/player.png");

        // Set FPS "Limit"
        Raylib.SetTargetFPS(60);

        // Set Fullscreen
        if (FULLSCREEN)
        {
            Raylib.ToggleFullscreen();
        }

        // Create new Camera (2D)
        cam = new Camera2D
        {
            Target = new Vector2(player_x, player_y),
            Offset = new Vector2(WINDOW_WIDTH / 2f, WINDOW_HEIGHT / 2f),
            Rotation = 0f,
            Zoom = 1f
        };

        // Floors List
        floors =
            [
                Floor(0, 600, 20000, 5)
            ];

        // Predefined Obstacles List
        obstacles =
            [
            ];

        // List of clicks/placed obstacles
        List<Vector2> clicked_points = [];

        RandomizeObstacles();
        // Main loop
        while (!Raylib.WindowShouldClose())
        {
            // Refresh
            Raylib.BeginDrawing();
            Raylib.ClearBackground(window_background_color);

            // Draw background
            Raylib.DrawTexture(background_image, 0, 0, Color.White);
            if (player_alive)
            {
                // Camera initialize
                Vector2 camTarget = new(player_x, player_y);
                cam.Target = Vector2.Lerp(cam.Target, camTarget, 0.1f);

                // Shown in camera
                Raylib.BeginMode2D(cam);

                ClickToDraw(clicked_points);

                // Draw Objects
                DrawFloors();
                DrawObstacles();
                DrawPlayer();

                Raylib.DrawText("" + player_deaths, 200, 400, 50, Color.Black);

                // Other functions
                ListenKeyboard();
                GravityRight();
                Gravity();
                CheckPlayerDeathByCollision();

                // Exiting "Camera Mode"
                Raylib.EndMode2D();

                // Draw stats/points
                int points_width = Raylib.MeasureText("Points: " + (int)player_x / 100, 20);
                Raylib.DrawText("Points: " + (int)player_x / 100, (WINDOW_WIDTH / 2) - (points_width / 2), 10, 20, Color.Brown);

                if (biggest_score > (int)player_x / 100)
                {
                    int record_width = Raylib.MeasureText("Record: " + biggest_score, 20);
                    Raylib.DrawText("Record: " + biggest_score, (WINDOW_WIDTH / 2) - (record_width / 2), 40, 20, Color.Brown);
                }
                else
                {
                    int record_width = Raylib.MeasureText("Record: " + (int)player_x / 100, 20);
                    Raylib.DrawText("Record: " + (int)player_x / 100, (WINDOW_WIDTH / 2) - (record_width / 2), 40, 20, Color.Brown);
                }

                // DrawDebug(clicked_points);
            }
            else
            {
                Color rest_butt_color = Color.SkyBlue;
                restart_butt = new Rectangle((WINDOW_WIDTH / 2) - 100, (WINDOW_HEIGHT / 2) - 50, 200, 100);

                // Raylib.DrawLine(WINDOW_WIDTH / 2, 0, WINDOW_WIDTH / 2, WINDOW_HEIGHT, Color.Green);
                // Raylib.DrawLine(0, WINDOW_HEIGHT / 2, WINDOW_WIDTH, WINDOW_HEIGHT / 2, Color.Green);

                if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), restart_butt))
                {
                    rest_butt_color = Color.Blue;
                    restart_butt.Position = new Vector2((WINDOW_WIDTH / 2) - 100, (WINDOW_HEIGHT / 2) - 53);
                    if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                    {
                        biggest_score = Math.Max((int)player_x / 100, biggest_score);
                        player_x = 0;
                        player_y = WINDOW_HEIGHT / 2;
                        player_alive = true;
                    }
                }

                if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.KpEnter))
                {
                    biggest_score = Math.Max((int)player_x / 100, biggest_score);
                    player_x = 0;
                    player_y = WINDOW_HEIGHT / 2;
                    player_alive = true;
                }
                Raylib.DrawRectangleRec(restart_butt, rest_butt_color);
                Raylib.DrawText("RESTART", (WINDOW_WIDTH / 2) - 60, (WINDOW_HEIGHT / 2) - 10, 25, Color.Black);
            }

            // End
            Raylib.EndDrawing();
        }
        // Exit
        Raylib.UnloadImage(icon);
        Raylib.UnloadTexture(player_texture);
        Raylib.UnloadTexture(background_image);
        Raylib.CloseWindow();
    }


}