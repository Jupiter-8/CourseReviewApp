-- Delete data from all tables
DELETE FROM [dbo].[AspNetRoleClaims];
DELETE FROM [dbo].[AspNetRoles];
DELETE FROM [dbo].[AspNetUserClaims];
DELETE FROM [dbo].[AspNetUserLogins];
DELETE FROM [dbo].[AspNetUserTokens];
DELETE FROM [dbo].[AspNetUserRoles];

DELETE FROM [dbo].[ObservedCourses];
DELETE FROM [dbo].[HelpfullReviews];
DELETE FROM [dbo].[OwnerComments];
DELETE FROM [dbo].[ReviewReports];
DELETE FROM [dbo].[Reviews];
DELETE FROM [dbo].[AspNetUsers];
DELETE FROM [dbo].[Categories];
DELETE FROM [dbo].[LearningSkills];
DELETE FROM [dbo].[Courses];
GO
-- Delete data from all tables

-- AspNetRoles
SET IDENTITY_INSERT [dbo].[AspNetRoles] ON

INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [RoleValue]) VALUES (1, N'Course_owner', N'COURSE_OWNER', N'22e965e1-d5a0-46c9-b066-c84f0ac2c60e', 1)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [RoleValue]) VALUES (2, N'Course_client', N'COURSE_CLIENT', N'22e965e1-d5a0-46c9-b066-c84f0ac2c60e', 2)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [RoleValue]) VALUES (3, N'Moderator', N'MODERATOR', N'97122088-0058-4998-8340-541c278869d6', 3)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp], [RoleValue]) VALUES (4, N'Admin', N'ADMIN', N'54133f1a-9d65-44ae-be88-88e4b916cf3b', 4)

SET IDENTITY_INSERT [dbo].[AspNetRoles] OFF
GO
-- AspNetRoles

-- AspNetUsers, password for each user: Pass1234;
SET IDENTITY_INSERT [dbo].[AspNetUsers] ON

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (1, N'user1', N'USER1', N'co@gmail.com', N'CO@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'1168F29F0DEA407C8BF129C0DBAD48E9', N'6deabd01-27f6-4273-aea3-7765d58c0760', N'123321123', 0, 0, NULL, 1, 0, N'Michael', N'Scott', CAST(N'2021-08-22T11:20:00.0000000' AS DateTime2), 1, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (2, N'user2', N'USER2', N'cc@gmail.com', N'CC@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'2B2B49AA693747CF95DEFE7C8C8033A7', N'869950df-bc16-4ddf-ad35-e9fce11863a0', N'000111222', 0, 0, NULL, 1, 0, N'Dwight', N'Schrute', CAST(N'2021-11-12T20:00:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (3, N'user3', N'USER3', N'aa@gmail.com', N'AA@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'FD956280B79F495CB70A5C0530365F7B', N'40a9434d-cfec-49f5-9305-2a8ccc9ebc90', N'921351222', 0, 0, NULL, 1, 0, N'Jim', N'Halpert', CAST(N'2021-05-16T00:19:42.0000000' AS DateTime2), 0, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (4, N'user4', N'USER4', N'co2@gmail.com', N'CO2@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'6C7F483F86BD400CB8B9EC9612036403', N'16a20a42-9376-4491-be9e-d835632a8df6', N'123987444', 0, 0, NULL, 1, 0, N'Stanley', N'Hudson', CAST(N'2021-10-21T07:11:00.0000000' AS DateTime2), 1, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (5, N'user5', N'USER5', N'cc2@gmail.com', N'CC2@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'5C70FDD232BA4EF9B34A27037F595ED7', N'34b2f88b-7f6a-42eb-98d4-b0a2bccec109', N'999888777', 0, 0, NULL, 1, 0, N'Pam', N'Beesly', CAST(N'2021-11-01T08:55:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (6, N'user6', N'USER6', N'cc3@gmail.com', N'CC3@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'EF3C0ED58EFB4E1BB692DF45121BE997', N'9ef91ba2-e3a8-40d0-8740-4327ad1b2f9e', N'123321111', 0, 0, NULL, 1, 0, N'Kevin', N'Malone', CAST(N'2021-10-11T05:55:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (7, N'user7', N'USER7', N'cc4@gmail.com', N'CC4@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'2D194650EE3A4676944063B205DD00A3', N'766c6aa2-6fd2-4e35-aec0-99deb22c642b', N'098234123', 0, 0, NULL, 1, 0, N'Angela', N'Martin', CAST(N'2021-10-01T21:51:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (8, N'user8', N'USER8', N'cc5@gmail.com', N'CC5@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'8FB4CA15108F4CBEB4FD368DBF98818C', N'072dd51a-6356-4669-9e74-125397aafb5e', N'123321222', 0, 0, NULL, 1, 0, N'Creed', N'Bratton', CAST(N'2021-10-30T11:21:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (9, N'user9', N'USER9', N'cc6@gmail.com', N'CC6@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'ED3DA8E98EDD41FEACE1C71A57FEE738', N'80bdc220-4cd5-4c52-bca0-7b1384735677', N'231123321', 0, 0, NULL, 1, 0, N'Oscar', N'Martinez', CAST(N'2021-11-20T23:21:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (10, N'user10', N'USER10', N'cc7@gmail.com', N'CC7@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'939FC19F6F364ED78F7D6994B2FD4724', N'ef73a088-7cf9-4eb4-ac69-405e5039f433', N'999000999', 0, 0, NULL, 1, 0, N'Ryan', N'Howard', CAST(N'2021-11-04T22:20:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (11, N'user11', N'USER11', N'cc8@gmail.com', N'CC8@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'C545E2823A454D27A11DBF5A94A7699C', N'90de1964-75de-4c86-a9e0-c846bd21e3c7', N'221341123', 0, 0, NULL, 1, 0, N'Meredith', N'Palmer', CAST(N'2021-10-14T21:30:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (12, N'user12', N'USER12', N'cc9@gmail.com', N'CC9@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'CD238C674CE744F08547D2EDBC1F1DEC', N'84bd845b-86fb-4c2c-b1ef-abaa24f9bcb0', N'123321000', 0, 0, NULL, 1, 0, N'Andy', N'Bernard', CAST(N'2021-11-24T11:20:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (13, N'user13', N'USER13', N'cc10@gmail.com', N'CC10@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'590E0E0CD09149F6A358346E2995D0B5', N'61497362-c4ca-4270-b5d3-febf46be6dd2', N'444555111', 0, 0, NULL, 1, 0, N'Toby', N'Flenderson', CAST(N'2021-11-30T15:59:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (14, N'user14', N'USER14', N'cc11@gmail.com', N'CC11@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'379400BBA3EC49938E9F96A1A9785A0B', N'5a840b0f-4135-4f8e-bfb7-c0aecc5b71fd', N'567765333', 0, 0, NULL, 1, 0, N'Phyllis', N'Vance', CAST(N'2021-10-11T19:50:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [RegistrationDate], [UserType], [IsActive], [AvatarPath])
VALUES (15, N'user15', N'USER15', N'cc12@gmail.com', N'CC12@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAEM322STzYdCAUovfCoRhjvqojtl4pW/6l6bONWXzESXQFJQh4p+A8W8/bNVKOc2QSg==', N'E42ACA1673184A37847E684DCAE66E10', N'48bfc9d4-01f3-4b77-a172-bf6ba940ab8d', N'444111222', 0, 0, NULL, 1, 0, N'Darryl', N'Philbin', CAST(N'2021-11-12T10:23:00.0000000' AS DateTime2), 2, 1, N'default_user_avatar.jpg')

SET IDENTITY_INSERT [dbo].[AspNetUsers] OFF
GO
-- AspNetUsers

-- AspNetUserRoles
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (1, 1)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (2, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (3, 4)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (4, 1)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (4, 3) -- Moderator role
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (5, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (6, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (7, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (8, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (9, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (10, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (11, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (12, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (13, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (14, 2)
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (15, 2)
GO
-- AspNetUserRoles

-- Categories
SET IDENTITY_INSERT [dbo].[Categories] ON

INSERT [dbo].[Categories] ([Id], [Name]) VALUES(1, N'Development');
INSERT [dbo].[Categories] ([Id], [Name]) VALUES(2, N'Business');
INSERT [dbo].[Categories] ([Id], [Name]) VALUES(3, N'Finance & Accounting');
INSERT [dbo].[Categories] ([Id], [Name]) VALUES(4, N'Personal Development');
INSERT [dbo].[Categories] ([Id], [Name]) VALUES(5, N'Marketing');
INSERT [dbo].[Categories] ([Id], [Name]) VALUES(6, N'Music');

INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(7, N'Programming Languages', 1);
INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(8, N'Mobile Development', 1);
INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(9, N'Software Testing', 1);

INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(10, N'Entrepreneurship', 2);
INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(11, N'Management', 2);
INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(12, N'Sales', 2);

INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(13, N'Finance', 3);
INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(14, N'Cryptocurrency & Blockchain', 3);

INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(15, N'Leadership', 4);
INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(16, N'Motivation', 4);
INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(17, N'Personal Productivity', 4);

INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(18, N'Digital Marketing', 5);

INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(19, N'Instruments', 6);
INSERT [dbo].[Categories] ([Id], [Name], [ParentCategoryId]) VALUES(20, N'Music Production', 6);

SET IDENTITY_INSERT [dbo].[Categories] OFF
 --Categories

 --Courses
SET IDENTITY_INSERT [dbo].[Courses] ON

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(1, N'Learn and Understand C++', N'Take this course to learn C++ which you can use for most software you use daily!', CAST(N'2021-09-21T14:00:00.0000000' AS DateTime2), N'www.testsite.com', 1, 7, 1,
	N'If you have no previous knowledge or experience in C++, you will like that the course begins with C++ basics. If you have learned about C++ already in another course and want to improve what you already know, the course has hours of different topics in C++ with one topic per section.
	 Each section is linked to the previous one in terms of utilizing what was already learned. Each topic is supplied with lots of examples which help students in their process of learning. Also, some new features introduced in C++11 standard are explained. This is what makes it interesting
	  for both beginner and advanced students. Even if you already have a lot of experience in programming in C++, this course can help you learn some new information you had missed before. Upon the completion of this course, you should be able to write programs that have real-life applications.',
	   30, N'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(2, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-11-11T10:30:00.0000000' AS DateTime2), N'www.testsite.com', 1, 7, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 7, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(8, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-11-15T00:00:00.0000000' AS DateTime2), N'www.testsite.com', 1, 7, 1,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 17, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(9, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-10-13T00:20:00.0000000' AS DateTime2), N'www.testsite.com', 1, 8, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 10, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(3, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-11-12T13:11:00.0000000' AS DateTime2), N'www.testsite.com', 1, 10, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 12, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(4, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-09-17T21:00:00.0000000' AS DateTime2), N'www.testsite.com', 1, 13, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 14, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(5, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-11-24T10:41:00.0000000' AS DateTime2), N'www.testsite.com', 1, 17, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 13, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(6, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-09-24T11:34:00.0000000' AS DateTime2), N'www.testsite.com', 1, 18, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 11, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(7, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-12-30T20:30:00.0000000' AS DateTime2), N'www.testsite.com', 1, 19, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 20, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(10, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-08-10T00:10:45.0000000' AS DateTime2), N'www.testsite.com', 1, 7, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 14, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(11, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-11-16T20:20:00.0000000' AS DateTime2), N'www.testsite.com', 1, 7, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 12, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(15, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-11-06T10:55:00.0000000' AS DateTime2), N'www.testsite.com', 1, 7, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 11, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(13, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-09-29T20:12:00.0000000' AS DateTime2), N'www.testsite.com', 1, 7, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 10, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(14, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-11-03T11:11:00.0000000' AS DateTime2), N'www.testsite.com', 1, 7, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 10, 'English', N'default_course_image.jpg');

INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(12, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-10-10T10:00:00.0000000' AS DateTime2), N'www.testsite.com', 1, 7, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 10, 'English', N'default_course_image.jpg');
	
INSERT [dbo].[Courses] ([Id], [Name], [ShortDescription], [DateAdded], [CourseWebsiteUrl], [Status], [CategoryId], [OwnerId], [LongDescription], [Duration], [Language], [ImagePath])
	VALUES(16, N'Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusan', N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean m', CAST(N'2021-12-10T20:00:00.0000000' AS DateTime2), N'www.testsite.com', 1, 9, 4,
	N'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
	quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.
	Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', 15, 'English', N'default_course_image.jpg');

SET IDENTITY_INSERT [dbo].[Courses] OFF
-- Courses

-- Reviews
SET IDENTITY_INSERT [dbo].[Reviews] ON

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(1, N'Very good course, I have learned a lot. Thanks.',
	  5, 1, 2, CAST(N'2022-01-03T18:21:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(2, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  5, 1, 7, CAST(N'2021-10-17T09:19:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(4, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  5, 1, 6, CAST(N'2021-10-20T18:00:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(3, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  4, 1, 5, CAST(N'2021-10-17T11:11:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(5, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  2, 1, 8, CAST(N'2021-10-24T10:00:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(6, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  3, 1, 9, CAST(N'2021-10-30T07:00:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(7, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  5, 1, 10, CAST(N'2021-11-02T11:22:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(8, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  4, 1, 11, CAST(N'2021-11-05T20:05:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(9, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  5, 1, 12, CAST(N'2022-01-03T11:12:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(10, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  4, 1, 13, CAST(N'2021-12-29T00:00:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(11, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  5, 1, 14, CAST(N'2021-12-16T23:32:00.0000000' AS DateTime2));

INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(12, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  5, 1, 15, CAST(N'2021-11-19T19:48:00.0000000' AS DateTime2));
	
INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(13, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  3, 7, 15, CAST(N'2022-01-03T10:48:00.0000000' AS DateTime2));
	
INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(14, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  5, 8, 15, CAST(N'2022-01-01T13:00:00.0000000' AS DateTime2));
	
INSERT [dbo].[Reviews] ([Id], [Contents], [RatingValue], [CourseId], [AuthorId], DateAdded)
	VALUES(15, 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis,
	 ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibu',
	  4, 2, 15, CAST(N'2021-12-29T08:20:00.0000000' AS DateTime2));

SET IDENTITY_INSERT [dbo].[Reviews] OFF
-- Reviews

-- OwnerResponses
SET IDENTITY_INSERT [dbo].[OwnerComments] ON

INSERT [dbo].[OwnerComments]([Id], [ReviewId], [AuthorId], [Contents], [DateAdded]) VALUES(1, 1, 1, 
'Thank you for your kind words. Welcome to my other courses.', CAST(N'2021-10-19T15:23:00.0000000' AS DateTime2));

SET IDENTITY_INSERT [dbo].[OwnerComments] OFF
-- OwnerResponses

-- ReviewReports
SET IDENTITY_INSERT [dbo].[ReviewReports] ON

INSERT [dbo].[ReviewReports](Id, ReportContents, ReportingUserId, ReviewId, DateAdded, ReportReason) VALUES(1, N'Review is a spam.', 6, 8, CAST(N'2021-11-11T00:10:00.0000000' AS DateTime2), 0);
INSERT [dbo].[ReviewReports](Id, ReportContents, ReportingUserId, ReviewId, DateAdded, ReportReason) VALUES(2, N'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes,
 nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.',
 7, 8, CAST(N'2021-11-11T11:00:00.0000000' AS DateTime2), 2);

SET IDENTITY_INSERT [dbo].[ReviewReports] OFF
-- ReviewReports

-- HelpfullReviews
INSERT [dbo].[HelpfullReviews]([UserId], [ReviewId]) VALUES(1, 2);
INSERT [dbo].[HelpfullReviews]([UserId], [ReviewId]) VALUES(15, 1);
INSERT [dbo].[HelpfullReviews]([UserId], [ReviewId]) VALUES(11, 1);
INSERT [dbo].[HelpfullReviews]([UserId], [ReviewId]) VALUES(12, 1);
-- HelpfullReviews

-- Learning skills
SET IDENTITY_INSERT [dbo].[LearningSkills] ON

INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(1, 1, N'Set up enviroment');
INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(2, 1, N'Interaction with user');
INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(3, 1, N'Loops');
INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(4, 1, N'Arrays');
INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(5, 1, N'Functions');
INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(6, 1, N'Exceptions');
INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(7, 1, N'Structures');
INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(8, 1, N'Classes');
INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(9, 1, N'STL');
INSERT [dbo].[LearningSkills](Id, CourseId, Name) VALUES(10, 1, N'Decomposition');

SET IDENTITY_INSERT [dbo].[LearningSkills] OFF
-- Learning skills

