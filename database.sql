USE [master]
GO
CREATE DATABASE WESPLIT
GO
USE WESPLIT
go

-- Create function for searching, count amount of money ------------------------------------------------------------------------------------------
DROP FUNCTION dbo.fuConvertToUnsign2
go
DROP FUNCTION dbo.RemoveAllSpaces
go
CREATE FUNCTION RemoveAllSpaces
(
       @InputStr varchar(8000)
)
RETURNS varchar(8000)
AS
BEGIN
declare @ResultStr varchar(8000)
set @ResultStr = @InputStr
while charindex('-', @ResultStr) > 0
       set @ResultStr = replace(@InputStr, '-', '')

return @ResultStr
END

GO
CREATE FUNCTION fuConvertToUnsign2
(
 @strInput NVARCHAR(4000)
) 
RETURNS NVARCHAR(4000)
AS
Begin
 Set @strInput=rtrim(ltrim(lower(@strInput)))
 IF @strInput IS NULL RETURN @strInput
 IF @strInput = '' RETURN @strInput
 Declare @text nvarchar(50), @i int
 Set @text='-''`~!@#$%^&*()?><:|}{,./\"''='';–'
 Select @i= PATINDEX('%['+@text+']%',@strInput ) 
 while @i > 0
 begin
 set @strInput = replace(@strInput, substring(@strInput, @i, 1), '')
 set @i = patindex('%['+@text+']%', @strInput)
 End
 Set @strInput =replace(@strInput,' ',' ')
 
 DECLARE @RT NVARCHAR(4000)
 DECLARE @SIGN_CHARS NCHAR(136)
 DECLARE @UNSIGN_CHARS NCHAR (136)
 SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
 ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý'
 +NCHAR(272)+ NCHAR(208)
 SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
 iiiiiooooooooooooooouuuuuuuuuuyyyyy'
 DECLARE @COUNTER int
 DECLARE @COUNTER1 int
 SET @COUNTER = 1
 WHILE (@COUNTER <=LEN(@strInput))
 BEGIN 
 SET @COUNTER1 = 1
 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
 BEGIN
 IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) 
 = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) )
 BEGIN 
 IF @COUNTER=1
 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) 
 + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) 
 ELSE
 SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) 
 +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) 
 + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER)
 BREAK
 END
 SET @COUNTER1 = @COUNTER1 +1
 END
 SET @COUNTER = @COUNTER +1
 End
 SET @strInput = replace(@strInput,' ','-')
 RETURN lower(@strInput)
END
GO

-- Create table --------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[COST](
	[COST_ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](30) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[COST_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)ON [PRIMARY]
GO

CREATE TABLE [dbo].[LOCATION](
	[LOCATION_ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](255) NOT NULL,
	[ADDRESS] [nvarchar](255) NOT NULL,
	[DESCRIPTION] [nvarchar](255) NULL,
	PRIMARY KEY CLUSTERED 
	(
	[LOCATION_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[MEMBER](
	[MEMBER_ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [nvarchar](255) NOT NULL,
	[PHONENUMBER] [nvarchar](11) NOT NULL,
	[AVATAR] [nvarchar](255) NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[MEMBER_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TRIP](
	[TRIP_ID] [int] IDENTITY(1,1) NOT NULL,
	[DESCRIPTION] [nvarchar](255) NULL,
	[TITTLE] [nvarchar](255) NOT NULL,
	[THUMNAIL] [nvarchar](255) NOT NULL,
	[TOTALCOSTS] [float] NOT NULL,
	[TOGODATE] [datetime] NULL,
	[RETURNDATE] [datetime] NULL,
	[ISDONE] [bit] NULL,
	PRIMARY KEY CLUSTERED 
	(
		[TRIP_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TRIP_COSTS](
	[COST_ID] [int] NOT NULL,
	[TRIP_ID] [int] NOT NULL,
	[AMOUNT] [float] NULL,
	CONSTRAINT [PK_TRIP_COSTS] PRIMARY KEY CLUSTERED 
	(
		[COST_ID] ASC,
		[TRIP_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TRIP_IMAGES](
	[TRIP_ID] [int] NOT NULL,
	[IMAGE_PATH] [nvarchar](100) NOT NULL,
	CONSTRAINT [PK_TRIP_IMAGES] PRIMARY KEY CLUSTERED 
	(
		[TRIP_ID] ASC,
		[IMAGE_PATH] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TRIP_LOCATIONS](
	[TRIP_ID] [int] NOT NULL,
	[LOCATION_ID] [int] NOT NULL,
	[COSTS] [float] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[TRIP_ID] ASC,
		[LOCATION_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TRIP_MEMBERS](
	[TRIP_ID] [int] NOT NULL,
	[MEMBER_ID] [int] NOT NULL,
	[AMOUNT_PAID] [float] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[TRIP_ID] ASC,
		[MEMBER_ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, 
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Foreign key ----------------------------------------------------------------------------------------------------------------------------------------------

ALTER TABLE [dbo].[TRIP_COSTS]  WITH CHECK ADD FOREIGN KEY([COST_ID])
REFERENCES [dbo].[COST] ([COST_ID])
GO
ALTER TABLE [dbo].[TRIP_COSTS]  WITH CHECK ADD FOREIGN KEY([TRIP_ID])
REFERENCES [dbo].[TRIP] ([TRIP_ID])
GO
ALTER TABLE [dbo].[TRIP_IMAGES]  WITH CHECK ADD  CONSTRAINT [FK_TRIP_IMAGES] FOREIGN KEY([TRIP_ID])
REFERENCES [dbo].[TRIP] ([TRIP_ID])
GO
ALTER TABLE [dbo].[TRIP_IMAGES] CHECK CONSTRAINT [FK_TRIP_IMAGES]
GO
ALTER TABLE [dbo].[TRIP_LOCATIONS]  WITH CHECK ADD FOREIGN KEY([LOCATION_ID])
REFERENCES [dbo].[LOCATION] ([LOCATION_ID])
GO
ALTER TABLE [dbo].[TRIP_LOCATIONS]  WITH CHECK ADD FOREIGN KEY([TRIP_ID])
REFERENCES [dbo].[TRIP] ([TRIP_ID])
GO
ALTER TABLE [dbo].[TRIP_MEMBERS]  WITH CHECK ADD FOREIGN KEY([MEMBER_ID])
REFERENCES [dbo].[MEMBER] ([MEMBER_ID])
GO
ALTER TABLE [dbo].[TRIP_MEMBERS]  WITH CHECK ADD FOREIGN KEY([TRIP_ID])
REFERENCES [dbo].[TRIP] ([TRIP_ID])
GO

-- Insert sample data ---------------------------------------------------------------------------------------------------------------------------------------

-- Default
ALTER TABLE [dbo].[TRIP] ADD  DEFAULT ((0)) FOR [TOTALCOSTS]
GO
ALTER TABLE [dbo].[TRIP_LOCATIONS] ADD  DEFAULT ((0)) FOR [COSTS]
GO
ALTER TABLE [dbo].[TRIP_MEMBERS] ADD  DEFAULT ((0)) FOR [AMOUNT_PAID]
GO

SET IDENTITY_INSERT [dbo].[COST] ON 
INSERT [dbo].[COST] ([COST_ID], [NAME]) VALUES (1, N'Chỗ ở')
INSERT [dbo].[COST] ([COST_ID], [NAME]) VALUES (2, N'Di chuyển')
INSERT [dbo].[COST] ([COST_ID], [NAME]) VALUES (3, N'Ăn uống')
INSERT [dbo].[COST] ([COST_ID], [NAME]) VALUES (4, N'Khác')
SET IDENTITY_INSERT [dbo].[COST] OFF
GO

SET IDENTITY_INSERT [dbo].[LOCATION] ON 
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (1, N'Lạc tiên giới', N'1/3 Đường Lâm Sinh, Phường 5, TP Đà Lạt', N'Chiêu đãi du khách bằng phong cảnh núi rừng nguyên sơ, bên trên có khinh khí cầu, bên dưới là hồ nước tĩnh lặng')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (2, N'Bắc Thang Lên Hỏi Ông Trời', N'Thôn Túy Sơn, Xã Xuân Thọ, Đà Lạt', N'Là điểm đến “sống ảo” mới toanh của Đà Lạt')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (3, N'Nông trại Sunny Farm', N'Dốc Số 7, Trại Mát, P. 11, Tp. Đà Lạt', N'Nnấc thang lên thiên đường, quán cà phê nhỏ xinh ở Sunny Farm chính là điểm nhấn thu hút du khách. Từ vị trí của Sunny Farm bạn có thể phóng tầm mắt chiêm ngưỡng vẻ đẹp của khu Trại Mát')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (4, N'Bảo tàng tranh 3D Art in us', N'02-04, Đường số 9, KDC Himlam, Phường Tân Hưng, Quận 7, TP HCM', N'Là không gian để bạn bung tỏa hết sức sáng tạo và cảm xúc để tạo nên thế giới kỳ ảo của riêng mình qua từng bức ảnh chụp')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (5, N'World Of Heineken', N'Bitexco Financial Tower, 2 Hải Triều, Bến Nghé, Hồ Chí Minh', N'Khám phá lịch sử phát triển hãng Heineken – hãng bia nổi tiếng thế giới')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (6, N'Vietopia', N'khu dân cư Him Lam, quận 7', N'Là mô hình công viên vui chơi kiểu mới, tạo môi trường cho trẻ trải nghiệm các ngành nghề một cách chân thật nhất')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (7, N'du lịch Tre Việt', N'25 Phan Văn Đáng, Phú Hữu, Nhơn Trạch, Đồng Nai', N'Tại đây với nhiều trò chơi hấp như chèo thuyền kayak, thuyền thúng, du thuyền bamboo, đạp xe trên sông hay vượt chướng ngại vật,…')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (8, N' Khu du lịch Bửu Long', N'Huỳnh Văn Nghệ, Kp4, Bửu Long, Thành phố Biên Hòa, Đồng Nai', N' Là khu vui chơi lớn nhất ở Nha Trang có diện tích hơn 200,000 m2 đạt tiêu chuẩn quốc tế')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (9, N'Viện Hải Dương học Nha Trang', N'01 Cầu Đá, Trần Phú, TP. Nha Trang', N'Địa điểm này hấp dẫn các em nhỏ bổ sung những kiến thức về các loài sinh vật biển và chiêm ngưỡng đủ các loại động vật biển khác nhau')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (10, N'Vinpearl', N'đảo Hòn Tre, phường Vĩnh Nguyên, thành phố Nha Trang, Khánh Hòa, Việt Nam', N'Nghỉ dưỡng, hòa mình với không khí biển')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (11, N'Mù Cang Chải', N'Mù Cang Chải, Yên Bái ', N'Được giới trẻ hội tụ về đây rầm rộ để được đắm chìm trong sắc màu lấp lánh của mùa vàng trên núi')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (12, N'Điện Biên', N'Điện Biên', N'Là 1 trong những địa điểm du lịch Tây Bắc tuyệt đẹp bởi hào khí chiến thắng')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (13, N'Tháp Eiffel', N'Tháp Eiffel, Avenue Anatole France, Pa ri, Pháp', N'Là một công trình kiến trúc bằng thép nằm bên cạnh sông Seine.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (14, N'Quảng trường Concorde', N'Quảng trường Concorde, Pa ri, Pháp', N'Là một một trong những quảng trường nổi tiếng của nước Pháp nằm ở đại lộ Champs- Élysées bên bờ sông Seine')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (15, N'Đại lộ Champs-Élysées', N'Đại lộ Champs-Élysées, Pa ri, Pháp', N'Là đại lộ lớn và nổi tiếng nhất của thành phố Paris, nối hai quảng trường Concorde và Etoile với nhiều cửa hàng thời trang cao cấp')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (16, N'Đài tưởng niệm', N'bảo tàng quốc gia gần 9/11 Memorial & Museum, Greenwich Street, Thành phố New York, Tiểu bang New York, Hoa Kỳ', N'Nơi đặt đài kỷ niệm và bảo tàng tưởng nhớ sự kiện khủng bố 11/9.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (17, N'Broadway', N'Broadway Theatre, Broadway, Thành phố New York, Tiểu bang New York, Hoa Kỳ', N'Con phố trải trải dài suốt dọc khu Manhattan. Một trong những khu mua sắm đông đúc nhất của nó là SoHo')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (18, N'Núi Phú Sỹ', N'Phú Sĩ, Kitayama, Fujinomiya, Shizuoka, Nhật Bản', N' Luôn thiêng liêng, sẽ che chở, bảo vệ cho sự bình an và phồn thịnh')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (19, N'Tháp Tokyo Tower', N'Tháp Tokyo, 4 Chome-2-8 Shibakoen, Minato, Tôkyô, Nhật Bản', N'Cảm giác choáng ngợp, bất ngờ trước một bức tranh đầy sắc màu của những ánh đèn nhấp nháy, phản ánh sự phồn hoa và hiện đại.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (20, N'Đền Kinkaku-ji ở Kyoto', N'Kinkaku-ji, 1 Kinkakujichō, Kita Ward, Kyōto, Nhật Bản', N'Được ôm trọn bởi thiên nhiên xanh thẳm, mát rượi, đền Kinkaku-ji ở Kyoto là một công trình kiến trúc nổi tiếng vô cùng.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (21, N'Cung điện Gyeongbokgung', N'161 Sajik-ro, Sejongno, Jongno-gu, Seoul, Hàn Quốc', N'Là cung điện lớn nhất và quan trọng nhất của Hàn Quốc')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (22, N'Cung điện Changdeokgung', N'99 Yulgok-ro, Gwonnong-dong, Jongno-gu, Seoul, Hàn Quốc', N'Là cung điện lớn thứ 2 ở Seoul. Cung điện Changdeokgung nổi tiếng với một khu vườn bí mật lớn nơi có vô số ngôi đền linh thiêng')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (23, N'Khu làng cổ Bukcheon Hanok', N'Gye-dong, Jongno-gu, Seoul, Hàn Quốc', N'Nằm giữa 2 cung điện Gyeongbokgung và Cung điện Changdeokgung là ngôi làng cổ nổi tiếng mang tên Bukcheon Hanok đẹp như tranh vẽ')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (24, N'Asiatique The Riverfront', N'Charoenkrung Soi 72-76, Charoenkrung Rd, Wat Phrayakrai Dist', N'Là một khu phức hợp giải trí và mua sắm lớn bên cạnh sông Chao Phraya ở Bangkok')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (25, N'Maeklong Railway Market', N'Kasem Sukhum Alley, Mae Klong, Samut Songkhram, 75000', N'Họp chợ trên đường ray xe lửa ư? Nghe có vẻ khó tin và đầy nguy hiểm nhưng đó là cách hoạt động của khu chợ trời nổi tiếng nhất Thái Lan')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (26, N'Open House Central Embassy', N'Tầng 6, Central Embassy, 1031 Ploenchit Road, Pathumwan, Bangkok', N'Là một không gian mở cung cấp nhiều dịch vụ như nhà hàng, mua sắm, ăn uống.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (27, N'Tượng chúa Giêsu Kito Vua', N' Thùy Vân, Phường 2, Tp. Vũng Tàu, Bà Rịa – Vũng Tàu', N'Biểu tượng của thành phố biển, là một bức tượng Chúa Giêsu được đặt trên đỉnh Núi Nhỏ của thành phố Vũng Tàu')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (28, N'Biển Long Hải', N'Nằm cách thành phố Vũng Tàu 12km', N' Là điểm đến không thể bỏ qua khi tới du lịch Vũng Tàu bởi bức tranh thiên nhiên hoang sơ tuyệt đẹp.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (29, N'Bãi biển Bãi Cháy', N'Bãi Cháy, Hạ Long, Quảng Ninh', N' Là bãi biển nhân tạo với chiều dài gần 1km, bãi biển rộng với cát trắng mịn, nước trong.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (30, N'Sun World Halong Complex', N'Số 9 Đường Hạ Long, Bãi Cháy, Hạ Long, Quảng Ninh.', N' Là tổ hợp vui chơi giải trí đẳng cấp quốc tế với tổng diện tích lên tới 214 ha')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (31, N'Du lịch sinh thái dưới nước Cồn Quy', N' xã Tân Thạch và Quới Sơn, huyện Châu Thành, tỉnh Bến Tre, Việt Nam', N'Nằm dọc theo con sông Tiền và cách trung tâm thành phố Bến Tre 23km là Cồn Quy. Đây là một trong những điểm đến nổi tiếng nhất khi nhắc đến Bến Tre')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (32, N'Cồn Phụng', N'cồn Phụng, Tân Thạch, Châu Thành, Bến Tre', N'Được bình chọn là khu du lịch tiêu biểu ở Đồng bằng sông Cửu Long,')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (33, N'Cồn Phú Đa', N'cồn Phú Đa, Vĩnh Bình, Chợ Lách, Bến Tre', N' Nổi bật với cảnh trí thiên nhiên đẹp, thiên nhiên trong lành trong hệ thống toàn bộ cồn nổi ở Bến Tre')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (34, N'Thủ đô Athens', N'Athens, Hy Lạp', N'Là thành phố cổ lớn nhất của Hy Lạp với niên đại khoảng 3000 năm.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (35, N'Santorini ', N'Santorini , A-ten, Hy Lạp', N'Là một phần của quần đảo Cyclades và còn có tên gọi khác là Thira')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (36, N'Thessaloniki', N'Thessaloniki, Hy Lạp', N'Một thành phố năng động tươi trẻ nhưng vẫn vẹn nguyên những giá trị cổ như bước ra từ chuyện thần thoại. Đó là những gì du khách nghĩ khi nhắc đến thành phố Thessaloniki.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (37, N'Đường sách Nguyễn Văn Bình', N'Nguyễn Văn Bình, quận 1, tp. HCM', N'Quy tụ các nhà xuất bản lớn nhất cả nước với những đầu sách hấp dẫn')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (38, N'Lost Escape Room', N'Lầu 5 – Diamond Plaza – 34 Lê Duẩn – Quận 1 – Tp.Hồ Chí Minh', N'Là trò chơi nhập vai thực tế. Với nhiều chủ đề cho bạn lựa chọn')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (39, N'Snow Town', N' Căn hộ CBD Premium, 125 Đồng Văn Cống, Phường Thạnh Mỹ Lợi, Quận 2, Hồ Chí Minh', N'Cùng chìm đắm sắc trắng tinh khôi với những hoạt động đầy thú vị như trượt tuyết, nặn người tuyết hay chơi ném tuyết')
SET IDENTITY_INSERT [dbo].[LOCATION] OFF
GO

SET IDENTITY_INSERT [dbo].[MEMBER] ON 
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (1, N'Nguyễn Văn A', N'0988029100', N'\\Image\\Member\\avatar01.jpg')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (2, N'Nguyễn Văn B', N'0988029099', N'\\Image\\Member\\avatar02.jpg')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (3, N'Trương Minh Quân', N'0988029098', N'\\Image\\Member\\avatar03.jpg')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (4, N'Nguyễn An', N'0988029097', N'\\Image\\Member\\avatar04.jpg')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (5, N'Nguyễn Trần Beckham', N'0988029096', N'\\Image\\Member\\avatar05.jpg')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (6, N'Lê Pele', N'0988029095', N'\\Image\\Member\\avatar06.jpg')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (7, N'Trần Tiến', N'0988029094', N'\\Image\\Member\\avatar07.jpg')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (8, N'Phùng Thanh Độ', N'0988029093', N'\\Image\\Member\\avatar08.jpg')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (9, N'Nguyễn Anh Khoa', N'0988029092', N'\\Image\\Member\\avatar09.jpg')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (10, N'Trần Diễm Kiều', N'0988029091', N'\\Image\\Member\\avatar10.jpg')
SET IDENTITY_INSERT [dbo].[MEMBER] OFF
GO

SET IDENTITY_INSERT [dbo].[TRIP] ON 
INSERT [dbo].[TRIP] ([TRIP_ID], [TITTLE], [DESCRIPTION], [THUMNAIL],  [TOTALCOSTS], [TOGODATE], [RETURNDATE], [ISDONE]) 
VALUES (1, N'Chuyến đi châu Âu đầu tiên', N'Chuyến đi đầu tiên với những người bạn yêu dấu', N'Assets\trips\1\1.jpg', 2150000, CAST(N'2018-10-10T00:00:00.000' AS DateTime), CAST(N'2018-10-15T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[TRIP] ([TRIP_ID], [TITTLE], [DESCRIPTION], [THUMNAIL],  [TOTALCOSTS], [TOGODATE], [RETURNDATE], [ISDONE]) 
VALUES (2, N'Dạo chơi Sài Gòn', N'chuyến đi đầu tiên vòng quanh Tp.HCM', N'Assets\trips\2\1.jpg', 2400000, CAST(N'2019-09-11T00:00:00.000' AS DateTime), CAST(N'2019-09-15T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[TRIP] ([TRIP_ID], [TITTLE], [DESCRIPTION], [THUMNAIL],  [TOTALCOSTS], [TOGODATE], [RETURNDATE], [ISDONE]) 
VALUES (3, N'Phượt miền Trung', N'Thăm cố đô Huế, tắm biển Đà Nẵng,...', N'Assets\trips\3\1.jpg', 1850000, CAST(N'2021-11-11T00:00:00.000' AS DateTime), CAST(N'2021-11-15T00:00:00.000' AS DateTime), 0)
INSERT [dbo].[TRIP] ([TRIP_ID], [TITTLE], [DESCRIPTION], [THUMNAIL],  [TOTALCOSTS], [TOGODATE], [RETURNDATE], [ISDONE]) 
VALUES (4, N'Chuyến đi các nước Đông Nam Á', N'Lào Campuchia Thái Lan Myanmar', N'Assets\trips\4\1.jpg', 1900000, CAST(N'2020-09-10T00:00:00.000' AS DateTime), CAST(N'2020-09-25T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[TRIP] ([TRIP_ID], [TITTLE], [DESCRIPTION], [THUMNAIL],  [TOTALCOSTS], [TOGODATE], [RETURNDATE], [ISDONE]) 
VALUES (5, N'Dạo một vòng miền Bắc', N'Đi nghỉ dưỡng trốn cái nóng ở miền Nam', N'Assets\trips\5\1.jpg', 1450000, CAST(N'2018-08-10T00:00:00.000' AS DateTime), CAST(N'2018-08-15T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[TRIP] ([TRIP_ID], [TITTLE], [DESCRIPTION], [THUMNAIL],  [TOTALCOSTS], [TOGODATE], [RETURNDATE], [ISDONE]) 
VALUES (6, N'Miền Tây nhẹ nhàng, thơ mộng', N'Khám phá, ăn uống ở miền Tây Nam Bộ', N'Assets\trips\6\1.jpg', 2500000, CAST(N'2018-09-11T00:00:00.000' AS DateTime), CAST(N'2018-10-15T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[TRIP] ([TRIP_ID], [TITTLE], [DESCRIPTION], [THUMNAIL],  [TOTALCOSTS], [TOGODATE], [RETURNDATE], [ISDONE]) 
VALUES (7, N'Chuyến đi đáng quên', N'Quá nhiều kỷ niệm xấu', N'Assets\trips\7\1.jpg', 3000000, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-15T00:00:00.000' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[TRIP] OFF
GO

SET IDENTITY_INSERT [dbo].[TRIP_COSTS] ON 
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (1, 1, 2000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (2, 1, 900000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (3, 1, 300000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (4, 1, 3000000)

INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (1, 2, 100000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (2, 2, 3000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (3, 2, 300000)

INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (1, 3, 3000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (2, 3, 300000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (3, 3, 200000)

INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (3, 4, 300000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (4, 4, 3000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (3, 4, 300000)

INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (1, 5, 5000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (2, 5, 2000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (3, 5, 3000000)

INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (1, 7, 2000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (2, 7, 2000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (3, 7, 600000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (4, 7, 600000)

INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (1, 6, 2000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (3, 6, 2000000)
INSERT [dbo].[TRIP_COSTS] ([COST_ID], [TRIP_ID], [AMOUNT]) VALUES (2, 6, 5000000)
SET IDENTITY_INSERT [dbo].[TRIP_COSTS] OFF
GO

SET IDENTITY_INSERT [dbo].[TRIP_LOCATIONS] ON 
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (1, 1, 800000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (1, 2, 500000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (1, 3, 450000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (2, 4, 500000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (2, 5, 800000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (2, 6, 500000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (3, 7, 450000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (3, 8, 500000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (4, 9, 800000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (4, 10, 500000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (5, 11, 450000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (5, 12, 500000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (6, 30, 450000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (6, 31, 500000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (6, 31, 500000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (6, 31, 500000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (7, 32, 450000)
INSERT [dbo].[TRIP_LOCATIONS] ([TRIP_ID], [LOCATION_ID], [COSTS]) VALUES (7, 33, 500000)
SET IDENTITY_INSERT [dbo].[TRIP_LOCATIONS] OFF
GO

SET IDENTITY_INSERT [dbo].[TRIP_MEMBERS] ON 
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (1, 1, 520000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (1, 2, 500000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (1, 3, 30000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (1, 4, 2000000)

INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (2, 5, 400000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (2, 6, 900000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (2, 7, 200000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (2, 8, 0)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (2, 9, 300000)

INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (3, 1, 0)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (3, 2, 0)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (3, 3, 0)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (3, 10, 0)

INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (4, 4, 0)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (4, 5, 3000000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (4, 6, 300000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (4, 7, 300000)

INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (5, 8, 2500000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (5, 9, 2500000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (5, 10, 2500000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (5, 1, 2500000)

INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (6, 4, 0)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (6, 8, 20000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (6, 9, 300000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (6, 1, 400000)

INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (7, 7, 400000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (7, 5, 500000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (7, 3, 100000)
INSERT [dbo].[TRIP_MEMBERS] ([TRIP_ID], [MEMBER_ID], [AMOUNT_PAID]) VALUES (7, 10, 500000)
SET IDENTITY_INSERT [dbo].[TRIP_MEMBERS] OFF
GO

INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (1, N'Assets\trips\1\list\1.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (1, N'Assets\trips\1\list\2.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (1, N'Assets\trips\1\list\3.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (1, N'Assets\trips\1\list\4.jpg')

INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (2, N'Assets\trips\2\list\1.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (2, N'Assets\trips\2\list\2.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (2, N'Assets\trips\2\list\3.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (2, N'Assets\trips\2\list\4.jpg')

INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (3, N'Assets\trips\3\list\1.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (3, N'Assets\trips\3\list\2.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (3, N'Assets\trips\3\list\3.jpg')

INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (4, N'Assets\trips\4\list\1.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (4, N'Assets\trips\4\list\2.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (4, N'Assets\trips\4\list\3.jpg')

INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (5, N'Assets\trips\5\list\1.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (5, N'Assets\trips\5\list\2.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (5, N'Assets\trips\5\list\3.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (5, N'Assets\trips\5\list\4.jpg')

INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (6, N'Assets\trips\6\list\1.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (6, N'Assets\trips\6\list\2.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (6, N'Assets\trips\6\list\3.jpg')

INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (7, N'Assets\trips\7\list\1.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (7, N'Assets\trips\7\list\2.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (7, N'Assets\trips\7\list\3.jpg')
INSERT [dbo].[TRIP_IMAGES] ([TRIP_ID], [IMAGE_PATH]) VALUES (7, N'Assets\trips\7\list\4.jpg')
