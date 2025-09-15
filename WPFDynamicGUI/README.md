# WPF Dynamic GUI

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.0-yellow.svg)]

Das Projekt zeigt den ersten Ansatz um Felder in einer GUI dynamisch zu behandeln.
D.h. Es werden nicht immer alle möglichen Label und InputControls auf der GUI erstellt, sondern diese können bei Bedarf aus einer Liste ausgewählt werden.

<img src=".\..\MainWindow.png" style="width:750px;"/></br>

Über den Button **Weiteres Feld** kann nun ein zusätzliches Eingabefeld hinzugefügt werden.

<img src=".\..\MainWindow_A.png" style="width:750px;"/></br>

Problem ist in dieser Variante das es keine Typ-Abhängigkeit gibt. Alle Datentypen haben erstmal nur eine TextBox.
Im nächsten Schritt, soll ein definiertes Control zum passenden Datentyp dargestellt werden.

