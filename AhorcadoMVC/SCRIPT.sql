-- CREAR BASE DE DATOS (opcional si ya existe)
CREATE DATABASE AhorcadoDB;
GO

USE AhorcadoDB;
GO

-- ELIMINAR TABLA SI EXISTE (precauci�n: borra datos)
IF OBJECT_ID('Palabra', 'U') IS NOT NULL
    DROP TABLE Palabra;
GO

-- CREAR TABLA Palabra
CREATE TABLE Palabra (
    Id INT PRIMARY KEY,
    Texto NVARCHAR(100) NOT NULL,
    Dificultad INT NOT NULL CHECK (Dificultad BETWEEN 1 AND 3)
);
GO

CREATE TABLE Usuario (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    Contrasena NVARCHAR(510) NOT NULL,
    Rol NVARCHAR(40) NOT NULL CHECK (Rol = 'Jugador' OR Rol = 'Admin'),
    HighestScore INT NULL DEFAULT(0)
);

CREATE TABLE Partida (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UsuarioId INT NOT NULL,
    PalabraId INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT(GETDATE()),
    Resultado NVARCHAR(40) NOT NULL CHECK (Resultado = 'Gan�' OR Resultado = 'Perdi�'),
    Puntaje INT NOT NULL,

    CONSTRAINT FK_Partida_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id),
    CONSTRAINT FK_Partida_Palabra FOREIGN KEY (PalabraId) REFERENCES Palabra(Id)
);



-- INSERTS (100 palabras)
INSERT INTO Palabra (Id, Texto, Dificultad) VALUES
(1, N'perro', 1),
(2, N'gato', 1),
(3, N'libro', 1),
(4, N'flor', 1),
(5, N'mesa', 1),
(6, N'silla', 1),
(7, N'pluma', 1),
(8, N'camino', 1),
(9, N'puerta', 1),
(10, N'rat�n', 1),
(11, N'pared', 1),
(12, N'cielo', 1),
(13, N'nube', 1),
(14, N'arena', 1),
(15, N'bosque', 1),
(16, N'fruta', 1),
(17, N'ropa', 1),
(18, N'carro', 1),
(19, N'manzana', 1),
(20, N'papel', 1),
(21, N'l�piz', 1),
(22, N'agua', 1),
(23, N'casa', 1),
(24, N'piedra', 1),
(25, N'pasto', 1),
(26, N'juego', 1),
(27, N'reloj', 1),
(28, N'barco', 1),
(29, N'taza', 1),
(30, N'pan', 1),
(31, N'luz', 1),
(32, N'sol', 1),
(33, N'dado', 1),
(34, N'�rbol', 2),
(35, N'm�sica', 2),
(36, N'revisi�n', 2),
(37, N'c�ntaro', 2),
(38, N'tel�fono', 2),
(39, N'historia', 2),
(40, N'lim�n', 2),
(41, N'f�tbol', 2),
(42, N'ventana', 2),
(43, N'c�sped', 2),
(44, N'c�mara', 2),
(45, N'p�jaro', 2),
(46, N'pantalla', 2),
(47, N'bebida', 2),
(48, N'herida', 2),
(49, N'zapato', 2),
(50, N'c�lido', 2),
(51, N'd�lar', 2),
(52, N'pel�cula', 2),
(53, N'domingo', 2),
(54, N'balc�n', 2),
(55, N't�mpano', 2),
(56, N'dif�cil', 2),
(57, N'r�brica', 2),
(58, N'rec�mara', 2),
(59, N'pl�tano', 2),
(60, N's�bado', 2),
(61, N'c�firo', 2),
(62, N'c�modo', 2),
(63, N'cuadro', 2),
(64, N'r�pido', 2),
(65, N'm�todo', 2),
(66, N'h�roe', 2),
(67, N'am�gdalas', 2),
(68, N'murci�lago', 3),
(69, N'jalape�o', 3),
(70, N'esdr�jula', 3),
(71, N'hipop�tamo', 3),
(72, N'ornitorrinco', 3),
(73, N'ping�ino', 3),
(74, N'paralelep�pedo', 3),
(75, N'transcripci�n', 3),
(76, N'aeropuerto', 3),
(77, N'psicolog�a', 3),
(78, N'extra�ar', 3),
(79, N'z�ngano', 3),
(80, N'inm�vil', 3),
(81, N'ant�rtida', 3),
(82, N'almohadilla', 3),
(83, N'subt�tulo', 3),
(84, N'reverberar', 3),
(85, N'cr�teres', 3),
(86, N'geometr�a', 3),
(87, N'f�sforo', 3),
(88, N't�xico', 3),
(89, N'cu�driceps', 3),
(90, N'fant�stico', 3),
(91, N'cat�strofe', 3),
(92, N'par�sito', 3),
(93, N'm�rmoles', 3),
(94, N'ox�moron', 3),
(95, N'an�mona', 3),
(96, N'colibr�', 3),
(97, N'crucigrama', 3),
(98, N'hip�rbole', 3),
(99, N'hurac�n', 3),
(100, N'guarida', 3);
GO
