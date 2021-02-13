CREATE FUNCTION udf_GetAvailableRoom (@HotelId INT, @Date DATE , @People INT)
RETURNS NVARCHAR(MAX)
BEGIN
	DECLARE @RoomInfo VARCHAR(MAX) = (SELECT TOP(1) 'Room ' + CONVERT(VARCHAR, r.Id) + ': ' + r.Type + ' (' + CONVERT(VARCHAR, r.Beds) +' beds)' + 
' - $' +  CONVERT(VARCHAR,(h.BaseRate + r.Price) * @People)
	FROM ROOMS AS r
		JOIN Hotels AS h ON r.HotelId = h.Id
		WHERE Beds >= @People AND HotelId = @HotelId AND NOT EXISTS (SELECT * FROM Trips t WHERE t.CancelDate IS NULL 
		AND @Date BETWEEN ArrivalDate AND ReturnDate)
	ORDER BY (h.BaseRate + r.Price) * @People DESC);

	IF (@RoomInfo IS NULL) 
			RETURN 'No Rooms Available';
	RETURN @RoomInfo;
END
GO

CREATE FUNCTION udf_AllUserCommits(@username VARCHAR) 
RETURNS INT
BEGIN
	DECLARE @countOfComits int = (SELECT u.Username, Count(*)  FROM Users AS u 
										 JOIN Commits AS c ON c.ContributorId = u.Id
										 GROUP BY u.Username)
END
GO


SELECT * FROM USERS

SELECT u.Username, Count(*)  FROM Users AS u 
										 JOIN Commits AS c ON c.ContributorId = u.Id
										 GROUP BY u.Username