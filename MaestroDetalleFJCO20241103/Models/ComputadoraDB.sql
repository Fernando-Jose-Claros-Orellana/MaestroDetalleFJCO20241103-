CREATE DATABASE Computadora
GO

USE Computadora

-- Crear la tabla 'Computadora'
CREATE TABLE Computadora (
    id INT PRIMARY KEY IDENTITY,
    nombre VARCHAR(100),
    marca VARCHAR(50),
    precio DECIMAL(10, 2)
);

-- Crear la tabla 'Componente'
CREATE TABLE Componente (
    id INT PRIMARY KEY IDENTITY,
    computadora_id INT,
    nombre VARCHAR(100),
    tipo VARCHAR(50),
    precio DECIMAL(10, 2),
    FOREIGN KEY (computadora_id) REFERENCES Computadora(id) ON DELETE CASCADE
);
