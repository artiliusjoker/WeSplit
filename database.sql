USE [master]
GO
CREATE DATABASE WESPLIT
GO
USE WESPLIT
go

-- Truncate tables ------------------------------------------------------------------------------------------
Backup database WESPLIT
to disk = 'D:\backup\wesplit.bak';
truncate table [dbo].[COST]
go
truncate table [dbo].[LOCATION]
go
truncate table [dbo].[MEMBER]
go
truncate table [dbo].[TRIP]
go
truncate table [dbo].[TRIP_COSTS]
go
truncate table [dbo].[TRIP_IMAGES]
go
truncate table [dbo].[TRIP_LOCATIONS]
go
truncate table [dbo].[TRIP_MEMBERS]
go
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

ALTER TABLE [dbo].[TRIP_COSTS]
ADD CONSTRAINT FK_TRIPCOSTS_COST
FOREIGN KEY ([COST_ID]) REFERENCES [dbo].[COST] ([COST_ID])
go

ALTER TABLE [dbo].[TRIP_COSTS]
ADD CONSTRAINT FK_TRIPCOSTS_TRIP
FOREIGN KEY ([TRIP_ID]) REFERENCES [dbo].[TRIP] ([TRIP_ID])
go

ALTER TABLE [dbo].[TRIP_IMAGES]
ADD CONSTRAINT FK_TRIPIMAGES_TRIP
FOREIGN KEY ([TRIP_ID]) REFERENCES [dbo].[TRIP] ([TRIP_ID])
go

ALTER TABLE [dbo].[TRIP_LOCATIONS]
ADD CONSTRAINT FK_TRIPLOCATIONS_LOCATION
FOREIGN KEY ([LOCATION_ID]) REFERENCES [dbo].[LOCATION] ([LOCATION_ID])
go

ALTER TABLE [dbo].[TRIP_LOCATIONS]
ADD CONSTRAINT FK_TRIPLOCATIONS_TRIP
FOREIGN KEY ([TRIP_ID]) REFERENCES [dbo].[TRIP] ([TRIP_ID])
go

ALTER TABLE [dbo].[TRIP_MEMBERS]
ADD CONSTRAINT FK_TRIPMEMBERS_MEMBER
FOREIGN KEY ([MEMBER_ID]) REFERENCES [dbo].[MEMBER] ([MEMBER_ID])
go

ALTER TABLE [dbo].[TRIP_MEMBERS]
ADD CONSTRAINT FK_TRIPMEMBERS_TRIP
FOREIGN KEY ([TRIP_ID]) REFERENCES [dbo].[TRIP] ([TRIP_ID])
go

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
-- Địa điểm VN
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (1, N'Lạc Tiên Giới', N'Đường Lâm Sinh, Phường 5, TP. Đà Lạt', N'Chiêu đãi du khách bằng phong cảnh núi rừng nguyên sơ, bên trên có khinh khí cầu, bên dưới là hồ nước tĩnh lặng.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (2, N'Fansipan - nóc nhà của Đông Dương', N'Cách thị trấn Sapa 9km về phía Tây Nam', N'Là đỉnh núi cao nhất Việt Nam và cao nhất toàn khu vực Đông Dương. Nằm ở độ cao lên tới 3.143m và thuộc dãy núi Hoàng Liên Sơn.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (3, N'A Pa Chải', N'xã Sín Thầu, huyện Mường Nhé, tỉnh Điện Biên, Việt Nam', N'Phượt A Pa Chải luôn là niềm mơ ước của những kẻ thích du lich bụi. Không chỉ để làm giàu bản đồ du lịch cá nhân mà để hưởng cái cảm giác chiến thắng mọi thứ.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (4, N'Bảo tàng tranh 3D Art', N'Đường số 9, Khu đô thị Him Lam, Quận 7', N'Là không gian để bạn bung tỏa hết sức sáng tạo và cảm xúc để tạo nên thế giới kỳ ảo của riêng mình qua từng bức ảnh chụp.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (5, N'Bitexco Financial Tower', N'2 Hải Triều, Bến Nghé, Hồ Chí Minh', N'Khu vui chơi mua sắm tại trung tâm thành phố Hồ Chí Minh.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (6, N'Vietopia', N'Đường số 9, Khu đô thị Him Lam, Quận 7', N'Công viên vui chơi cho trẻ nhỏ lớn nhất TP.Hồ Chí Minh.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (7, N'Du lịch Tre Việt', N'25 Phan Văn Đáng, Phú Hữu, Nhơn Trạch, Đồng Nai', N'Khu du lịch sinh thái hàng đầu miền Nam Việt Nam, có đầy đủ trò chơi.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (8, N'Lăng Chủ tịch Hồ Chí Minh', N'Đường Hùng Vương, Quảng trường Ba Đình, Hà Nội', N'Lăng Chủ tịch Hồ Chí Minh, còn gọi là Lăng Hồ Chủ tịch, Lăng Bác, là nơi gìn giữ thi hài Hồ Chí Minh.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (9, N'Viện Hải Dương học Nha Trang', N'01 Cầu Đá, Trần Phú, TP. Nha Trang', N'Địa điểm này hấp dẫn các em nhỏ bổ sung những kiến thức về các loài sinh vật biển và chiêm ngưỡng đủ các loại động vật biển khác nhau.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (10, N'Vinpearl', N'Đảo Hòn Tre, phường Vĩnh Nguyên, thành phố Nha Trang, tỉnh Khánh Hòa, Việt Nam', N'Nha Trang được mệnh danh là hòn ngọc của biển Đông, Viên ngọc xanh vì giá trị thiên nhiên, vẻ đẹp cũng như khí hậu của nó. Đây là nơi được mệnh danh là Los Angles VN.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (11, N'Mù Cang Chải', N'Mù Cang Chải, Yên Bái ', N'Mù Cang Chải được công nhận là một trong những danh thắng bậc nhất đất Việt. Nếu bạn yêu vẻ mộc mạc đồng quê, yêu màu xanh miền sơn cước, bạn sẽ yêu Mù Cang Chải.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (12, N'Điện Biên', N'Điện Biên', N'Điện Biên là tỉnh giàu tiềm năng du lịch, đặc biệt là về lĩnh vực văn hóa – lịch sử.')
-- Địa điểm châu âu
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (13, N'Tháp Eiffel', N'Đại lộ Avenue Anatole France, Paris, Pháp', N'Tháp Eiffel tại Paris là một trong những công trình biểu tượng nổi tiếng nhất của thành phố này và cũng là một trong những điểm tham quan được ghé thăm nhiều nhất thế giới.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (14, N'Vatican City', N'Nằm giữa thành phố Rome, Ý', N'Với diện tích khoảng 44 hécta (110 mẫu Anh), và dân số khoảng 1000 người, khiến Vatican được quốc tế công nhận là thành phố quốc gia độc lập nhỏ nhất thế giới về diện tích và dân số.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (15, N'Đại lộ Champs-Élysées', N'Đại lộ Champs-Élysées, Paris, Pháp', N'Là đại lộ lớn và nổi tiếng nhất của thành phố Paris, nối hai quảng trường Concorde và Etoile với nhiều cửa hàng thời trang cao cấp')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (16, N'Lâu đài Gravensteen', N'Ghent, East Flanders nước Bỉ', N'Lâu đài là một pháo đài trung cổ đặc trưng, với những chiếc cầu thang xoắn, ngục tối có tường cao bao quanh và hào phòng vệ.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (17, N'Đấu trường La Mã', N'Thành phố Rome, Ý', N'Đấu trường được sử dụng cho các võ sĩ giác đấu và nô lệ có nguồn gốc tù binh chiến tranh thi đấu và trình diễn công chúng. Đấu trường được xây dựng khoảng năm 70-80 sau Công Nguyên dưới thời hoàng đế Vespasian.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (18, N'Đồng hồ Big Ben', N'Palace of Westminster, thành phố London, nước Anh', N'Tòa tháp là một biểu tượng văn hóa nước Anh được công nhận trên toàn thế giới. Đây là một trong những biểu tượng nổi bật nhất của Vương quốc Anh và dân chủ nghị viện.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (19, N'Cung điện Buckingham', N'Westminster, London SW1A 1AA, United Kingdom', N'Cung điện Buckingham hiện là nơi sinh sống của Nữ hoàng Elizabeth II tại London từ khi bà lên ngôi vào năm 1952.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (20, N'Cổng Brandenburg', N'Pariser Platz, 10117 Berlin, Germany', N'Là cổng thành phố trước đây và là một trong những biểu tượng chính của thành phố Berlin, Đức. Cổng này nằm ở quận Trung tâm (Bezirk Mitte) của Berlin.')
-- Địa điểm ĐNA
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (21, N'Cung điện Gyeongbokgung', N'Indonesia', N'Là cung điện lớn nhất và quan trọng nhất của Hàn Quốc')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (22, N'Cung điện Changdeokgung', N'Campuchia', N'Là cung điện lớn thứ 2 ở Seoul. Cung điện Changdeokgung nổi tiếng với một khu vườn bí mật lớn nơi có vô số ngôi đền linh thiêng')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (23, N'Khu làng cổ Bukcheon Hanok', N'Lào', N'Nằm giữa 2 cung điện Gyeongbokgung và Cung điện Changdeokgung là ngôi làng cổ nổi tiếng mang tên Bukcheon Hanok đẹp như tranh vẽ')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (24, N'Asiatique The Riverfront', N'Charoenkrung Soi 72-76, Charoenkrung Rd, Wat Phrayakrai Dist, Thái Lan', N'Là một khu phức hợp giải trí và mua sắm lớn bên cạnh sông Chao Phraya ở Bangkok')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (25, N'Maeklong Railway Market', N'Kasem Sukhum Alley, Mae Klong, Samut Songkhram, Thái Lan', N'Họp chợ trên đường ray xe lửa ư? Nghe có vẻ khó tin và đầy nguy hiểm nhưng đó là cách hoạt động của khu chợ trời nổi tiếng nhất Thái Lan')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (26, N'Open House Central Embassy', N'Central Embassy, 1031 Ploenchit Road, Pathumwan, Bangkok, Thái Lan', N'Là một không gian mở cung cấp nhiều dịch vụ như nhà hàng, mua sắm, ăn uống.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (27, N'Tượng chúa Giêsu Kito Vua', N'Thùy Vân, Phường 2, Tp. Vũng Tàu, Bà Rịa – Vũng Tàu', N'Biểu tượng của thành phố biển, là một bức tượng Chúa Giêsu được đặt trên đỉnh Núi Nhỏ của thành phố Vũng Tàu.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (28, N'Biển Long Hải', N'Campuchia', N' Là điểm đến không thể bỏ qua khi tới du lịch Vũng Tàu bởi bức tranh thiên nhiên hoang sơ tuyệt đẹp.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (29, N'Bãi biển Bãi Cháy', N'Indonesia', N' Là bãi biển nhân tạo với chiều dài gần 1km, bãi biển rộng với cát trắng mịn, nước trong.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (30, N'Sun World Halong Complex', N'Lào', N' Là tổ hợp vui chơi giải trí đẳng cấp quốc tế với tổng diện tích lên tới 214 ha')
-- Địa điểm khác
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (31, N'Du lịch sinh thái dưới nước Cồn Quy', N' xã Tân Thạch và Quới Sơn, huyện Châu Thành, tỉnh Bến Tre, Việt Nam', N'Nằm dọc theo con sông Tiền và cách trung tâm thành phố Bến Tre 23km là Cồn Quy. Đây là một trong những điểm đến nổi tiếng nhất khi nhắc đến Bến Tre')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (32, N'Cồn Phụng', N'cồn Phụng, Tân Thạch, Châu Thành, Bến Tre', N'Được bình chọn là khu du lịch tiêu biểu ở Đồng bằng sông Cửu Long,')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (33, N'Cồn Phú Đa', N'cồn Phú Đa, Vĩnh Bình, Chợ Lách, Bến Tre', N' Nổi bật với cảnh trí thiên nhiên đẹp, thiên nhiên trong lành trong hệ thống toàn bộ cồn nổi ở Bến Tre')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (34, N'Thủ đô Athens', N'Athens, Hy Lạp', N'Là thành phố cổ lớn nhất của Hy Lạp với niên đại khoảng 3000 năm.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (35, N'Santorini ', N'Santorini , A-ten, Hy Lạp', N'Là một phần của quần đảo Cyclades và còn có tên gọi khác là Thira')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (36, N'Thessaloniki', N'Thessaloniki, Hy Lạp', N'Một thành phố năng động tươi trẻ nhưng vẫn vẹn nguyên những giá trị cổ như bước ra từ chuyện thần thoại. Đó là những gì du khách nghĩ khi nhắc đến thành phố Thessaloniki.')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (37, N'Đường sách Nguyễn Văn Bình', N'Nguyễn Văn Bình, quận 1, tp. HCM', N'Quy tụ các nhà xuất bản lớn nhất cả nước với những đầu sách hấp dẫn')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (38, N'Lost Escape Room', N'Lầu 5 – Diamond Plaza – 34 Lê Duẩn – Quận 1 – Tp.Hồ Chí Minh', N'Là trò chơi nhập vai thực tế. Với nhiều chủ đề cho bạn lựa chọn')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (39, N'Snow Town', N' Căn hộ CBD Premium, 125 Đồng Văn Cống, Phường Thạnh Mỹ Lợi, Quận 2, Hồ Chí Minh', N'Cùng chìm đắm sắc trắng tinh khôi với những hoạt động đầy thú vị như trượt tuyết, nặn người tuyết hay chơi ném tuyết')
INSERT [dbo].[LOCATION] ([LOCATION_ID], [NAME], [ADDRESS], [DESCRIPTION]) VALUES (40, N'Snow Town', N' Căn hộ CBD Premium, 125 Đồng Văn Cống, Phường Thạnh Mỹ Lợi, Quận 2, Hồ Chí Minh', N'Cùng chìm đắm sắc trắng tinh khôi với những hoạt động đầy thú vị như trượt tuyết, nặn người tuyết hay chơi ném tuyết')

SET IDENTITY_INSERT [dbo].[LOCATION] OFF
GO

SET IDENTITY_INSERT [dbo].[MEMBER] ON 
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (1, N'Nguyễn Văn A', N'0988029100', N'NO')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (2, N'Nguyễn Văn B', N'0988029099', N'NO')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (3, N'Trương Minh Quân', N'0988029098', N'NO')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (4, N'Nguyễn An', N'0988029097', N'NO')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (5, N'Nguyễn Trần Beckham', N'0988029096', N'NO')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (6, N'Lê Pele', N'0988029095', N'NO')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (7, N'Trần Tiến', N'0988029094', N'NO')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (8, N'Phùng Thanh Độ', N'0988029093', N'NO')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (9, N'Nguyễn Anh Khoa', N'0988029092', N'NO')
INSERT [dbo].[MEMBER] ([MEMBER_ID], [NAME], [PHONENUMBER], [AVATAR]) VALUES (10, N'Trần Diễm Kiều', N'0988029091', N'NO')
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

-- Check constraints
select table_view,
    object_type, 
    constraint_type,
    constraint_name,
    details
from (
    select schema_name(t.schema_id) + '.' + t.[name] as table_view, 
        case when t.[type] = 'U' then 'Table'
            when t.[type] = 'V' then 'View'
            end as [object_type],
        case when c.[type] = 'PK' then 'Primary key'
            when c.[type] = 'UQ' then 'Unique constraint'
            when i.[type] = 1 then 'Unique clustered index'
            when i.type = 2 then 'Unique index'
            end as constraint_type, 
        isnull(c.[name], i.[name]) as constraint_name,
        substring(column_names, 1, len(column_names)-1) as [details]
    from sys.objects t
        left outer join sys.indexes i
            on t.object_id = i.object_id
        left outer join sys.key_constraints c
            on i.object_id = c.parent_object_id 
            and i.index_id = c.unique_index_id
       cross apply (select col.[name] + ', '
                        from sys.index_columns ic
                            inner join sys.columns col
                                on ic.object_id = col.object_id
                                and ic.column_id = col.column_id
                        where ic.object_id = t.object_id
                            and ic.index_id = i.index_id
                                order by col.column_id
                                for xml path ('') ) D (column_names)
    where is_unique = 1
    and t.is_ms_shipped <> 1
    union all 
    select schema_name(fk_tab.schema_id) + '.' + fk_tab.name as foreign_table,
        'Table',
        'Foreign key',
        fk.name as fk_constraint_name,
        schema_name(pk_tab.schema_id) + '.' + pk_tab.name
    from sys.foreign_keys fk
        inner join sys.tables fk_tab
            on fk_tab.object_id = fk.parent_object_id
        inner join sys.tables pk_tab
            on pk_tab.object_id = fk.referenced_object_id
        inner join sys.foreign_key_columns fk_cols
            on fk_cols.constraint_object_id = fk.object_id
    union all
    select schema_name(t.schema_id) + '.' + t.[name],
        'Table',
        'Check constraint',
        con.[name] as constraint_name,
        con.[definition]
    from sys.check_constraints con
        left outer join sys.objects t
            on con.parent_object_id = t.object_id
        left outer join sys.all_columns col
            on con.parent_column_id = col.column_id
            and con.parent_object_id = col.object_id
    union all
    select schema_name(t.schema_id) + '.' + t.[name],
        'Table',
        'Default constraint',
        con.[name],
        col.[name] + ' = ' + con.[definition]
    from sys.default_constraints con
        left outer join sys.objects t
            on con.parent_object_id = t.object_id
        left outer join sys.all_columns col
            on con.parent_column_id = col.column_id
            and con.parent_object_id = col.object_id) a
order by table_view