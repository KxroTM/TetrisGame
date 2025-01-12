using System;
using NAudio.Wave;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace TetrisGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png",UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png",UriKind.Relative)),
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),

        };

        private readonly Image[,] imageControls;
        private readonly int maxSpeed = 1000;
        private readonly int minSpeed = 75;
        private readonly int speedStep = 25;
        private Game game = new Game();
        private bool isPaused = false;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        public float Volume = 0.1f;  
        private bool backHome = false;
        
        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(game.GridGame);
        }

        private Image[,] SetupGameCanvas(GridGame grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int row = 0; row < grid.Rows; row++)
            {
                for (int column = 0; column < grid.Columns; column++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    Canvas.SetTop(imageControl, (row - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, column * cellSize);
                    GameCanva.Children.Add(imageControl);
                    imageControls[row, column] = imageControl;
                }
            }

            return imageControls;
        }

        private void DrawGrid(GridGame grid)
        {
            for (int row = 0; row < grid.Rows; row++)
            {
                for (int column = 0; column < grid.Columns; column++)
                {
                    int id = grid[row, column];
                    imageControls[row, column].Opacity = 1;
                    imageControls[row, column].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePosition())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(Queue queue)
        {
            Block next = queue.Next;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = game.BlockDropDistance();

            foreach (Position p in block.TilePosition())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void Draw(Game game)
        {
            DrawGrid(game.GridGame);
            DrawGhostBlock(game.CurrentBlock);
            DrawBlock(game.CurrentBlock);
            DrawNextBlock(game.Queue);
            ScoreText.Text = $"Score: {game.Score}";
        }

        private async Task GameLoop()
        {
            Draw(game);
            while (!game.GameOver)
            {
                int speed = Math.Max(minSpeed, maxSpeed - (game.Score * speedStep));
                await Task.Delay(speed);
                if (isPaused)
                {
                    continue;
                }
                game.MoveBlockDown();
                Draw(game);
            }

            if (!backHome) {
                AudioFileReader audioFile = new AudioFileReader(@"Assets\sound\me_game_gameover.wav");
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
                outputDevice.Volume = Volume;
                outputDevice.Play();
            }
            MenuGameOver.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Final Score: {game.Score}";
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            outputDevice = new WaveOutEvent();
            audioFile = new AudioFileReader(@"Assets\sound\tetris_ost.wav");

            outputDevice.Init(audioFile);
            outputDevice.PlaybackStopped += (s, args) =>
            {
                audioFile.Position = 0; 
                outputDevice.Play();    
            };

            if (outputDevice != null)
            {
                outputDevice.Volume = Volume;
                outputDevice.Play();
            }
        }



        private async void Retry_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySound(@"Assets\sound\se_sys_select.wav", Volume);

            game = new Game();
            MenuGameOver.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (game.GameOver)
            {
                return;
            }

            if (isPaused)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    SoundManager.PlaySound(@"Assets\sound\se_game_rotate.wav", Volume);
                    game.MoveBlockLeft();
                    break;
                case Key.Right:
                    SoundManager.PlaySound(@"Assets\sound\se_game_rotate.wav", Volume);

                    game.MoveBlockRight();
                    break;
                case Key.Down:
                    SoundManager.PlaySound(@"Assets\sound\se_game_rotate.wav", Volume);

                    game.MoveBlockDown();
                    break;
                case Key.Up:
                    SoundManager.PlaySound(@"Assets\sound\se_game_rotate.wav", Volume);

                    game.RotateBlockCW();
                    break;
                case Key.Z:
                    SoundManager.PlaySound(@"Assets\sound\se_game_rotate.wav", Volume);

                    game.RotateBlockCCW();
                    break;
                case Key.Space:
                    SoundManager.PlaySound(@"Assets\sound\se_game_rotate.wav", Volume);

                    game.DropBlock();
                    break;
                case Key.Escape:
                    isPaused = !isPaused;
                    MenuPause.Visibility = Visibility.Visible;
                    SoundManager.PlaySound(@"Assets\sound\se_game_pause.wav",Volume);
                    break;
                default:
                    return;
            }

            Draw(game);
        }


        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySound(@"Assets\sound\se_sys_select.wav", Volume);
            LaunchScreen.Visibility = Visibility.Hidden;
            GameScreen.Visibility = Visibility.Visible;

            StartGame();
        }

        private async void StartGame()
        {
            SoundManager.PlaySound(@"Assets\sound\se_sys_select.wav", Volume);

            SoundManager.PlaySound(@"Assets\sound\me_game_start2_vo_fr.wav", Volume);

            game = new Game();
            game.Score = 0;
            MenuGameOver.Visibility = Visibility.Hidden;

            backHome = false;
            isPaused = false;
            await GameLoop();
        }


        private void Resume_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySound(@"Assets\sound\se_sys_select.wav", Volume);

            MenuPause.Visibility = Visibility.Hidden;
            isPaused = false;
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySound(@"Assets\sound\se_sys_select.wav", Volume);

            LaunchScreen.Visibility = Visibility.Hidden;
            OptionsMenu.Visibility = Visibility.Visible;
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySound(@"Assets\sound\se_sys_select.wav", Volume);

            Close();    
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySound(@"Assets\sound\se_sys_select.wav", Volume);
            backHome = true;
            MenuPause.Visibility = Visibility.Hidden;
            game.GameOver = true;
            MenuGameOver.Visibility = Visibility.Hidden;
            GameScreen.Visibility = Visibility.Hidden;
            LaunchScreen.Visibility = Visibility.Visible;
        }


        private void BackHome(object sender, RoutedEventArgs e)
        {
            SoundManager.PlaySound(@"Assets\sound\se_sys_select.wav", Volume);
            OptionsMenu.Visibility = Visibility.Hidden;
            LaunchScreen.Visibility = Visibility.Visible;
        }


        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double newVolume = e.NewValue;
            if (outputDevice != null)
            {
                Volume = (float)newVolume;
                outputDevice.Volume = Volume;
                SoundManager.SetVolume(Volume);
            }

            if (VolumePercentage != null)
            {
                VolumePercentage.Text = $"{(int)(newVolume * 100)}%";
            }
        }
    }
}
