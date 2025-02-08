CREATE TABLE AspNetUsers (
    Id NVARCHAR(450) PRIMARY KEY,
    UserName NVARCHAR(256) NULL,
    NormalizedUserName NVARCHAR(256) NULL,
    Email NVARCHAR(256) NULL,
    NormalizedEmail NVARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX) NULL,
    SecurityStamp NVARCHAR(MAX) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL,
    PhoneNumber NVARCHAR(MAX) NULL,
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEnd DATETIMEOFFSET NULL,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL,
    FullName NVARCHAR(256) NOT NULL
);

CREATE TABLE NoteItems (
    Id INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(256) NOT NULL,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    IsDeleted BIT NOT NULL DEFAULT 0,
    UserId NVARCHAR(450) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

CREATE INDEX IX_AspNetUsers_NormalizedUserName ON AspNetUsers (NormalizedUserName);
CREATE INDEX IX_AspNetUsers_NormalizedEmail ON AspNetUsers (NormalizedEmail);
CREATE INDEX IX_NoteItems_UserId ON NoteItems (UserId);