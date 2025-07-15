using Raylib_cs;
using System.Numerics;

class Program
{
    // Window Settings
    private static readonly int WINDOW_WIDTH = 1024;
    private static readonly int WINDOW_HEIGHT = 576;
    private static readonly bool FULLSCREEN = false;
    private static readonly Color window_background_color = Color.Gray;

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
    private static int player_y = WINDOW_HEIGHT/2;

    // Player settings
    private static readonly Color player_color = Color.White;
    private static readonly Color player_outline_color = Color.Green;
    private static readonly int player_size = 20;
    private static readonly int player_speed = 4;
    private static readonly int player_jumpforce = player_size + 20;
    private static bool player_alive = true;

    private static Rectangle player_rect;

    // Camera
    private static Camera2D cam;

    // Games Variables
    private static Rectangle restart_butt;
    private static Rectangle[]? floors;
    private static List<Rectangle> obstacles = [];

    // Other player variables
    private static bool touching_ground  = false;

    // Function that moves player to the right
    private static void GravityRight()
    {
        player_x += player_speed;
    }

    // Main Gravity Function
    private static void Gravity()
    {
        bool onAnyFloor = false;

        foreach (Rectangle floor in floors!)
        {
            if (Raylib.CheckCollisionRecs(floor, player_rect))
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
    }
    private static Rectangle Floor(float x, float y, int width, int height)
    {
        return new Rectangle(x, y, width, height);
    }

    private static Rectangle Obstacle(float x, float y, float width, float height)
    {
        return new Rectangle(x, y, width, height);
    }

    private static void CheckCollision()
    {
        foreach (Rectangle obstacle in obstacles!)
        {
            if (Raylib.CheckCollisionRecs(obstacle, player_rect))
            {
                player_alive = false;
            }
        }
    }

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
    private static void DrawPlayer()
    {
        // Draw Player
        player_rect = new Rectangle(player_x, player_y, player_size, player_size);
        Raylib.DrawRectangleRec(player_rect, player_color);
        DrawOutline();
    }
    [STAThread]
    public static void Main()
    {
        // Initialize window
        Raylib.InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Learn Raylib");

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

        // Debug/Test Text
        string test_text = "This is a test. A very important test. Not just any test, but a test that tests not only the camera system, but also the drawing capabilities, the font rendering, the pixel placement, the anti-aliasing (if any), the speed of text parsing, and the psychological endurance of any poor soul who dares to read through this absolute abomination of a paragraph, which, mind you, has no actual point, but rather exists solely for the purpose of being long, repetitive, verbose, and unnecessarily elaborate in a way that makes you question not only the sanity of the person who wrote it, but also the sanity of the person who decided to include it in the code, despite knowing full well that it serves no practical purpose other than to fill space, stress-test the UI layout, wrap lines like a burrito made of alphabet soup, and maybe, just maybe, make you laugh, or cry, or rage quit, or all three at once, because truly, deeply, fundamentally, there is nothing more soul-shattering than staring at a wall of endless, purposeless, meandering sentences that go on and on and on and on and on without ever reaching a point, conclusion, or meaningful piece of information, much like this one, which by now has already overstayed its welcome and yet somehow refuses to end, like a bad reboot of a movie franchise nobody asked for but still gets greenlit because some executive thinks 'eh, why not?', and so here we are, still reading, still going, still resisting the urge to Alt+F4 this entire experience, and yet, if you've come this far, if you're still reading, then congratulations, you've either got nerves of steel, nothing better to do, or you're just really, really invested in how badly a sentence can be abused in the name of testing camera text rendering logic in a framework designed for making games but currently being used to simulate the mental collapse of a developer trapped in an infinite loop of verbosity, verbosity, and yes, more verbosity.";

        // Floors List
        floors =
            [
                Floor(0, 600, 20000, 5)
            ];

        // Obstacles List
        obstacles =
            [
                Obstacle(9968, 596, 28, 12),
            ];

        // List of clicks/placed obstacles
        List<Vector2> clicked_points = [];


        // Main loop
        while (!Raylib.WindowShouldClose())
        {
            // Refresh
            Raylib.BeginDrawing();
            Raylib.ClearBackground(window_background_color);

            if (player_alive)
            {
                // Camera initialize
                Vector2 camTarget = new(player_x, player_y);
                cam.Target = Vector2.Lerp(cam.Target, camTarget, 0.1f);

                // Shown in camera
                Raylib.BeginMode2D(cam);
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

                DrawPlayer();

                // Draw Text
                Raylib.DrawText(test_text, 0 / 4, 600, 25, Color.Lime);

                // Drawing floors
                foreach (Rectangle floor in floors)
                {
                    Raylib.DrawRectangleRec(floor, Color.Brown);
                }

                // Drawing Obstacles
                foreach (Rectangle obstacle in obstacles)
                {
                    Raylib.DrawRectangleRec(obstacle, Color.Red);
                }

                // Other functions
                ListenKeyboard();
                GravityRight();
                Gravity();
                CheckCollision();

                // Exiting "Camera Mode"
                Raylib.EndMode2D();



                DrawDebug(clicked_points);
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
                        player_x = 0;
                        player_y = WINDOW_HEIGHT / 2;
                        player_alive = true;
                    }
                }
                Raylib.DrawRectangleRec(restart_butt, rest_butt_color);
                Raylib.DrawText("RESTART", (WINDOW_WIDTH / 2) - 60, (WINDOW_HEIGHT / 2) - 10, 25, Color.Black);
            }

            // End
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    
}