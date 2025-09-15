# WPF Window to Console

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.0-yellow.svg)]

In diesem kleinen Projekt wird gezeigt, wie Informationen und Steuerung für eine NET Console aus einem WPF Dialog erfolgen werden kann.

<img src="WPFToConsole.png" style="width:750px;"/></br>

Im Prinzip werden zwei Instanzen, die Konsole als auch das WPF Programm gestartet. Aus dem Konsolenprogramm wird dann die WPF instanz aufgerufen.

```csharp
public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var app = new App();
        app.InitializeComponent();
        app.Run();
    }
}
```

Die Steuerung der Console kann dann ganz *normal* über *Console.* aus dem WPF Programm heraus erfolgen.

```csharp
private void OnSendToConsole(object sender, RoutedEventArgs e)
{
    string line = this.OutputTextBox.Text;
    Console.Out.WriteLine(line);
}
```

```csharp
private void OnClearConsole(object sender, RoutedEventArgs e)
{
    Console.Clear();
}
```
