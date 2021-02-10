USE TripService

SELECT a.Id, Email, c.Name, COUNT(*) AS Trips
FROM Accounts AS a
	JOIN AccountsTrips AS [at] ON at.AccountId = a.Id
	JOIN Trips AS t ON t.Id = at.TripId
	JOIN Rooms AS r ON r.Id = t.RoomId
	JOIN Hotels AS h ON h.Id = r.HotelId
	JOIN Cities AS c ON c.Id = h.CityId
	WHERE c.Id = a.CityId
	GROUP BY a.Id, Email, c.Name
	ORDER BY Trips DESC, a.Id 