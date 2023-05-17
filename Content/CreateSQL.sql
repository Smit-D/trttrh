use FirstTaskDB
CREATE TABLE dbo.[User] 
(
	[UserId] bigint PRIMARY KEY IDENTITY(1,1),
	[FirstName] varchar(50),
	[LastName] varchar(50),
	[Email] varchar(250) unique,
	[Password] varchar(20),
	[Avtar] varchar(MAX),
	[PhoneNumber] varchar(10),
	[CountryId] bigint, 
	[StateId] bigint,
	[CityId] bigint, 
	[RoleId] tinyint,
	[CreatedAt] DateTime Default current_timestamp NOT NULL,
	[UpdatedAt] DateTime,
	[DeletedAt] DateTime,
)
CREATE TABLE [dbo].[Country] 
(
	[CountryId] bigint PRIMARY KEY IDENTITY(1,1),
	[CountryName] varchar(56),
	[CreatedAt] DateTime Default current_timestamp NOT NULL,
	[UpdatedAt] DateTime,
	[DeletedAt] DateTime,
)
CREATE TABLE [dbo].[State] 
(
	[StateId] bigint PRIMARY KEY IDENTITY(1,1),
	[StateName] varchar(56),
	[CountryId] bigint,
	[CreatedAt] DateTime Default current_timestamp NOT NULL,
	[UpdatedAt] DateTime,
	[DeletedAt] DateTime,
)
CREATE TABLE [dbo].[City] 
(
	[CityId] bigint PRIMARY KEY IDENTITY(1,1),
	[CityName] varchar(56),
	[stateId] bigint,
	[CountryId] bigint,
	[CreatedAt] DateTime Default current_timestamp NOT NULL,
	[UpdatedAt] DateTime,
	[DeletedAt] DateTime,
)
CREATE TABLE [dbo].[Gender] 
(
	[GenderId] tinyint PRIMARY KEY IDENTITY(1,1),
	[Gender] varchar(56),
	[CreatedAt] DateTime Default current_timestamp NOT NULL,
	[UpdatedAt] DateTime,
	[DeletedAt] DateTime,
)
CREATE TABLE [dbo].[Roles] 
(
	[RoleId] tinyint PRIMARY KEY IDENTITY(1,1),
	[RoleName] varchar(50),
	[CreatedAt] DateTime Default current_timestamp NOT NULL,
	[UpdatedAt] DateTime,
	[DeletedAt] DateTime,
)
/*Add Constraints*/

ALTER TABLE [User]
ADD CONSTRAINT fk_User_CountryId_Country
FOREIGN KEY (CountryId) REFERENCES Country(CountryId)
GO
ALTER TABLE [User]
ADD CONSTRAINT fk_User_StateId_State
FOREIGN KEY (StateId) REFERENCES [State](StateId)
GO
ALTER TABLE [User]
ADD CONSTRAINT fk_User_CityId_City
FOREIGN KEY (CityId) REFERENCES City(CityId)
GO
ALTER TABLE [User]
ADD CONSTRAINT fk_User_GenderId_Gender
FOREIGN KEY (GenderId) REFERENCES Gender(GenderId)
GO
ALTER TABLE [User]
ADD CONSTRAINT fk_User_RoleId_Roles
FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
GO

ALTER TABLE [City]
ADD CONSTRAINT fk_City_StateId_State
FOREIGN KEY (StateId) REFERENCES [State](StateId)
GO
ALTER TABLE [City]
ADD CONSTRAINT fk_City_CountryId_Country
FOREIGN KEY (CountryId) REFERENCES [Country](CountryId)
GO

ALTER TABLE [State]
ADD CONSTRAINT fk_State_CountryId_Country
FOREIGN KEY (CountryId) REFERENCES [Country](CountryId)
GO