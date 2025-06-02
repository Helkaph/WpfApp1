CREATE DATABASE Exam
GO
USE Exam
GO

CREATE TABLE Buildings
(
id INT PRIMARY KEY IDENTITY,
[address] VARCHAR(255) NOT NULL,
management_start DATE NOT NULL,
build_year INT NOT NULL
)
GO
CREATE TABLE BuildingDetails
(
id INT PRIMARY KEY IDENTITY,
building_id INT REFERENCES Buildings(id),
floors INT NOT NULL,
apartments INT NOT NULL,
total_area DECIMAL(18, 2) NOT NULL
)
GO
CREATE TABLE Apartments
(
id INT PRIMARY KEY IDENTITY,
building_id INT REFERENCES Buildings(id),
number INT NOT NULL
)
GO
CREATE TABLE Owners
(
id INT PRIMARY KEY IDENTITY,
fullname VARCHAR(100) NOT NULL,
phone VARCHAR(20) NOT NULL
)
GO
CREATE TABLE ApartmentOwners
(
id INT PRIMARY KEY IDENTITY,
apartment_id INT REFERENCES Apartments(id),
owner_id INT REFERENCES Owners(id)
)
GO
CREATE TABLE Debts
(
id INT PRIMARY KEY IDENTITY,
apartment_id INT REFERENCES Apartments(id),
water_debt DECIMAL(18, 2) NULL,
energy_debt DECIMAL(18, 2) NULL,
[date] DATE NOT NULL
)
GO
CREATE TABLE Payments
(
id INT PRIMARY KEY IDENTITY,
apartment_id INT REFERENCES Apartments(id),
time_period VARCHAR(20) NOT NULL,
accrued DECIMAL(18, 2) NOT NULL,
paid DECIMAL(18, 2) NULL
)
GO
CREATE TABLE Roles
(
id INT PRIMARY KEY IDENTITY,
role_name VARCHAR(20)
)
GO
CREATE TABLE Users
(
id INT PRIMARY KEY IDENTITY,
UserName VARCHAR(60) NOT NULL,
HashPassword VARCHAR(MAX) NOT NULL,
[Role] INT REFERENCES Roles(id),
IsBlocked BIT NULL DEFAULT 0,
FailAttempts INT DEFAULT 0,
LastLogin DATETIME NULL,
IsFirstPassword BIT DEFAULT 1
)
GO

CREATE FUNCTION dbo.GetTimelyPaymentPercentPerBuilding (@Month VARCHAR(7))
RETURNS TABLE
AS
RETURN
(
 SELECT
	b.id AS building_id,
	b.[address],
	CAST(
		100.0 * COUNT(DISTINCT CASE WHEN p.paid >= p.accrued THEN ao.owner_id END)
		/ NULLIF(COUNT(DISTINCT ao.owner_id), 0) AS DECIMAL(5,2)
		) AS timely_payment_percent
	FROM Buildings b
	JOIN Apartments a ON a.building_id = b.id
	JOIN ApartmentOwners ao ON ao.apartment_id = a.id
	LEFT JOIN Payments p ON p.apartment_id = a.id AND FORMAT(CAST(p.time_period + '-01' AS DATE), 'yyyy-MM') = @Month
	GROUP BY b.id, b.[address]
)
GO