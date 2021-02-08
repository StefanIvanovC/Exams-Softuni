/*Make all rooms’ prices 14% more expensive where the hotel ID is either 5, 7 or 9.*/

SELECT * FROM Rooms
WHERE HotelId IN (5,7,9)

UPDATE Rooms SET Price = Price + Price * 0.14
WHERE HotelId IN (5, 7, 9) 
