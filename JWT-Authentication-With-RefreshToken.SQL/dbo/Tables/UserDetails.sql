CREATE TABLE [dbo].[UserDetails] (
    [UserId]             INT                                               IDENTITY (1, 1) NOT NULL,
    [FullName]           VARCHAR (20)                                      NOT NULL,
    [LoginId]            VARCHAR (50)                                      NOT NULL,
    [Address]            VARCHAR (200)                                     NOT NULL,
    [EmailAddress]       VARCHAR (50)                                      NULL,
    [IsActive]           BIT                                               NOT NULL,
    [PasswordChangeDate] DATETIME                                          NULL,
    [CreatedUserId]      INT                                               NOT NULL,
    [CreatedDateTime]    DATETIME                                          NOT NULL,
    [ModifiedUserId]     INT                                               NULL,
    [ModifiedDate]       DATETIME                                          NULL,
    [Password]           VARCHAR (20) MASKED WITH (FUNCTION = 'default()') NULL,
    [PhoneNumber]        VARCHAR (10)                                      NULL,
    CONSTRAINT [PK_UserDetails_UserId] PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([LoginId] ASC)
);

