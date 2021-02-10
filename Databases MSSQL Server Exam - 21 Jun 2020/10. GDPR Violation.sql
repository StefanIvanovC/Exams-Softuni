USE TripService

SELECT
TripId,
FirstName + ' ' + ISNULL(MiddleName + ' ', '') + LastName AS FullName,
c.Name AS [From],
ch.Name [To],
CASE 
	WHEN CancelDate IS NULL THEN CONVERT(NVARCHAR(MAX),DATEDIFF(DAY, ArrivalDate, ReturnDate)) + ' days'
	ELSE 'Canceled'
END AS [Duration]
	FROM Accounts AS a
	JOIN AccountsTrips AS at ON a.Id = at.AccountId
	JOIN Trips AS t ON t.Id = at.TripId
	JOIN Cities AS c ON c.Id = a.CityId
	JOIN Rooms AS r ON r.Id = t.RoomId
	JOIN Hotels AS h ON h.Id = r.HotelId
	JOIN Cities AS ch ON ch.Id = h.CityId
	ORDER BY FullName, TripId, c.Name, ch.Name