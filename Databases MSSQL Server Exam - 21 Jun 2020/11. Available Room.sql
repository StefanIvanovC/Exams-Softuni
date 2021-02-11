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

SELECT dbo.udf_GetAvailableRoom(94, '2015-07-26', 3)

