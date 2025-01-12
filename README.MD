# Tetris

Welcome to **Tetris**, a Tetris game developed in C# using Visual Studio and the WPF (Windows Presentation Foundation) application framework.

## 🎮 Features

- Modern and responsive graphical interface.
- Smooth animations and user interactions.
- Intuitive controls for moving, rotating, and placing pieces.
- Scoring system to track your performance.

## 🚀 Why WPF?

I chose **WPF (Windows Presentation Foundation)** to develop this game because:

1. **Powerful Graphics**: WPF offers advanced tools for creating modern and visually appealing graphical interfaces through XAML. This includes animations, transformations, and custom controls—perfect for a game like Tetris.

2. **Flexibility**: Unlike Windows Forms, WPF allows for more modular interface design and better separation of logic from design using patterns like MVVM (Model-View-ViewModel), even if this project doesn't fully implement MVVM.

3. **Resolution Independence**: WPF uses device-independent units, ensuring consistent appearance across different screens and resolutions.

4. **Modernity**: WPF is more suitable for modern applications compared to WinForms, which is an older technology with limitations for complex or animated interfaces.

## 🛠 Prerequisites

Before running the game, make sure you have:

- **Visual Studio** (or any compatible .NET editor)
- **.NET SDK 5.0** installed

## ▶️ How to Run the Game

1. Clone or download this repository to your local machine:

   ```bash
   git clone https://github.com/KxroTM/TetrisGame
   ```

2. Navigate to the project directory in a terminal or open it in Visual Studio.

3. Run the following command to start the game :

   ```bash
   dotnet run
   ```

## 📂 Project Structure

- MainWindow.xaml: Defines the user interface of the game (grid, buttons, etc.).
- MainWindow.xaml.cs: Contains the main logic of the game, including collision and movement handling.
- Class: Includes the classes implementing the core game logic, such as pieces and grid management.
- Assets: Contains graphical resources (images, sounds, etc.).

## 🛠 Technologies Used

- Language: C#
- Framework: WPF (.NET 5.0)
- IDE: Visual Studio
