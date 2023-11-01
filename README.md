# SQLiteDemo
Small project Manager class, student and teacher using SQLite

# Using
- C# .Net framework 4.7.2
  * OOP
  * SQLiteConnection
  * SQLiteCommand
- WPF
- MVVM

# How to run

Clone or Download and extract folder this project

Open project in Visual Studio 2022 (click in SQLiteDemo.sln to open)
In a folder DAO -> DatabaseConnection.cs, change DataSource = full path of file SQLiteDemoDB.db
![image](https://github.com/GuenKainto/SQLiteDemo/assets/109414890/640026ad-038d-4c4a-bb0a-8c0e0b728917)

Right click in to solution and build, in first time maybe it with like this
![image](https://github.com/GuenKainto/SQLiteDemo/assets/109414890/0c2154be-65fb-4f3d-aec5-13ccc050512b)
When you have that bug, Right click in to solution, Clean and Rebuild solution again 
(Maybe one-two times and it will add SQLite.Interop.dll file into project, you can check it in bin/Debug folder)
