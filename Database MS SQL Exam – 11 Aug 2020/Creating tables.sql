CREATE DATABASE Bakery

USE Bakery

CREATE TABLE Products
(
	Id INT PRIMARY KEY IDENTITY ,
	[Name] VARCHAR(25) UNIQUE,
	Description VARCHAR(250) ,
	Recipe,
	Price

 
)